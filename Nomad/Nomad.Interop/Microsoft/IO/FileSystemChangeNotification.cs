namespace Microsoft.IO
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Threading;

    [DesignerCategory("Code")]
    public class FileSystemChangeNotification : Component, ISupportInitialize
    {
        private bool FDisposed;
        private bool FEnable;
        private NotifyFilters FFilter;
        private bool FInitializing;
        private string FPath;
        private volatile ISynchronizeInvoke FSynchronizingObject;
        private bool FWatchSubdirs;
        private NotificationWaitHandle NotificationHandle;
        private readonly object NotificationLock;
        private RegisteredWaitHandle RegisteredWait;

        public event EventHandler Changed;

        public FileSystemChangeNotification()
        {
            this.FFilter = NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.FileName;
            this.FPath = string.Empty;
            this.NotificationLock = new object();
        }

        public FileSystemChangeNotification(string path)
        {
            this.FFilter = NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.FileName;
            this.FPath = string.Empty;
            this.NotificationLock = new object();
            this.Path = path;
        }

        public void BeginInit()
        {
            this.FInitializing = true;
        }

        protected override void Dispose(bool disposing)
        {
            this.Stop();
            this.FDisposed = true;
        }

        public void EndInit()
        {
            this.FInitializing = false;
            this.EnableRaisingEvents = this.FEnable;
        }

        ~FileSystemChangeNotification()
        {
            this.Dispose(true);
        }

        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern SafeNotificationHandle FindFirstChangeNotification([MarshalAs(UnmanagedType.LPTStr)] string lpPathName, [MarshalAs(UnmanagedType.Bool)] bool bWatchSubtree, NotifyFilters dwNotifyFilter);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        private static extern bool FindNextChangeNotification(SafeNotificationHandle hChangeHandle);
        protected void OnChanged(EventArgs e)
        {
            if (this.Changed != null)
            {
                if ((this.FSynchronizingObject != null) && this.FSynchronizingObject.InvokeRequired)
                {
                    this.FSynchronizingObject.Invoke(this.Changed, new object[] { this, e });
                }
                else
                {
                    this.Changed(this, e);
                }
            }
        }

        private void Restart()
        {
            lock (this.NotificationLock)
            {
                this.Stop();
                this.NotificationHandle = new NotificationWaitHandle(this.FPath, this.FWatchSubdirs, this.FFilter);
                if (!this.NotificationHandle.Start())
                {
                    this.NotificationHandle = null;
                }
                else
                {
                    this.RegisteredWait = ThreadPool.RegisterWaitForSingleObject(this.NotificationHandle, new WaitOrTimerCallback(this.WaitForChange), null, -1, true);
                }
            }
        }

        private void Stop()
        {
            lock (this.NotificationLock)
            {
                if (this.RegisteredWait != null)
                {
                    this.RegisteredWait.Unregister(null);
                }
                this.RegisteredWait = null;
                if (this.NotificationHandle != null)
                {
                    this.NotificationHandle.Close();
                }
                this.NotificationHandle = null;
            }
        }

        private void WaitForChange(object state, bool timedOut)
        {
            bool flag = false;
            lock (this.NotificationLock)
            {
                this.RegisteredWait.Unregister(null);
                this.RegisteredWait = null;
                if (this.NotificationHandle.IsValid && this.NotificationHandle.Next())
                {
                    this.RegisteredWait = ThreadPool.RegisterWaitForSingleObject(this.NotificationHandle, new WaitOrTimerCallback(this.WaitForChange), null, -1, true);
                    flag = true;
                }
                else
                {
                    this.NotificationHandle.Close();
                    this.NotificationHandle = null;
                }
            }
            if (flag)
            {
                this.OnChanged(EventArgs.Empty);
            }
        }

        public bool EnableRaisingEvents
        {
            get
            {
                lock (this.NotificationLock)
                {
                    return ((this.NotificationHandle != null) && this.NotificationHandle.IsValid);
                }
            }
            set
            {
                if (value)
                {
                    if (this.FInitializing)
                    {
                        this.FEnable = true;
                    }
                    else if (!this.EnableRaisingEvents)
                    {
                        if (string.IsNullOrEmpty(this.FPath))
                        {
                            throw new InvalidOperationException("Path is null or empty");
                        }
                        if (!System.IO.Directory.Exists(this.FPath))
                        {
                            throw new DirectoryNotFoundException();
                        }
                        if (this.FDisposed)
                        {
                            throw new ObjectDisposedException("FileSystemChangeNotification");
                        }
                        this.Restart();
                    }
                }
                else if (this.FInitializing)
                {
                    this.FEnable = false;
                }
                else
                {
                    this.Stop();
                }
            }
        }

        public bool IncludeSubdirectories
        {
            get
            {
                return this.FWatchSubdirs;
            }
            set
            {
                if (this.FWatchSubdirs != value)
                {
                    this.FWatchSubdirs = value;
                    if (!(!this.EnableRaisingEvents || this.FInitializing))
                    {
                        this.Restart();
                    }
                }
            }
        }

        public NotifyFilters NotifyFilter
        {
            get
            {
                return this.FFilter;
            }
            set
            {
                if (this.FFilter != value)
                {
                    if ((value & (NotifyFilters.CreationTime | NotifyFilters.LastAccess)) > 0)
                    {
                        throw new ArgumentException("CreationTime and LastAccess notify filters not supported");
                    }
                    this.FFilter = value;
                    if (!(!this.EnableRaisingEvents || this.FInitializing))
                    {
                        this.Restart();
                    }
                }
            }
        }

        public string Path
        {
            get
            {
                return this.FPath;
            }
            set
            {
                if (this.FPath != value)
                {
                    if (!System.IO.Directory.Exists(value))
                    {
                        throw new DirectoryNotFoundException();
                    }
                    this.FPath = (value == null) ? string.Empty : value;
                    if (!(!this.EnableRaisingEvents || this.FInitializing))
                    {
                        this.Restart();
                    }
                }
            }
        }

        public ISynchronizeInvoke SynchronizingObject
        {
            get
            {
                return this.FSynchronizingObject;
            }
            set
            {
                this.FSynchronizingObject = value;
            }
        }

        private class NotificationWaitHandle : WaitHandle
        {
            private NotifyFilters FFilter;
            private FileSystemChangeNotification.SafeNotificationHandle FNotificationHandle;
            private string FPath;
            private bool FWatchSubtree;

            public NotificationWaitHandle(string pathName, bool watchSubtree, NotifyFilters filter)
            {
                this.FPath = pathName;
                this.FWatchSubtree = watchSubtree;
                this.FFilter = filter;
            }

            protected override void Dispose(bool explicitDisposing)
            {
                if (this.FNotificationHandle != null)
                {
                    this.FNotificationHandle.Close();
                }
                if (base.SafeWaitHandle != null)
                {
                    base.SafeWaitHandle.SetHandleAsInvalid();
                }
            }

            public bool Next()
            {
                if (this.FNotificationHandle == null)
                {
                    throw new InvalidOperationException();
                }
                return FileSystemChangeNotification.FindNextChangeNotification(this.FNotificationHandle);
            }

            public bool Start()
            {
                if (this.FNotificationHandle != null)
                {
                    throw new InvalidOperationException();
                }
                this.FNotificationHandle = FileSystemChangeNotification.FindFirstChangeNotification(this.FPath, this.FWatchSubtree, this.FFilter);
                if (!((this.FNotificationHandle == null) || this.FNotificationHandle.IsInvalid))
                {
                    this.Handle = this.FNotificationHandle.DangerousGetHandle();
                    base.SafeWaitHandle = new SafeWaitHandle(this.Handle, false);
                    return true;
                }
                Debug.WriteLine(new Win32Exception(), "FileSystemChangeNotification");
                return false;
            }

            public bool IsStarted
            {
                get
                {
                    return (this.FNotificationHandle != null);
                }
            }

            public bool IsValid
            {
                get
                {
                    return ((this.IsStarted && !this.FNotificationHandle.IsInvalid) && !this.FNotificationHandle.IsClosed);
                }
            }

            public FileSystemChangeNotification.SafeNotificationHandle NotificationHandle
            {
                get
                {
                    return this.FNotificationHandle;
                }
            }
        }

        private class SafeNotificationHandle : SafeHandleMinusOneIsInvalid
        {
            protected SafeNotificationHandle() : base(true)
            {
            }

            [return: MarshalAs(UnmanagedType.Bool)]
            [SuppressUnmanagedCodeSecurity, DllImport("Kernel32.dll")]
            private static extern bool FindCloseChangeNotification(IntPtr hChangeHandle);
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            protected override bool ReleaseHandle()
            {
                return FindCloseChangeNotification(base.handle);
            }
        }
    }
}

