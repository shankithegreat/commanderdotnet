namespace Microsoft.Shell
{
    using System;
    using System.Runtime.Serialization;
    using System.Threading;

    [Serializable]
    public class SafeShellItem : ShellItem
    {
        private int ThreadId;
        private WeakReference WeakFolder;

        public SafeShellItem(IntPtr absolutePidl) : base(absolutePidl)
        {
        }

        public SafeShellItem(IShellFolder folder, IntPtr relativePidl) : base(folder, relativePidl)
        {
        }

        public SafeShellItem(IntPtr hwndOwner, string absoluteName) : base(hwndOwner, absoluteName)
        {
        }

        protected SafeShellItem(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SafeShellItem(IShellFolder folder, IntPtr hwndOwner, string relativeName) : base(folder, hwndOwner, relativeName)
        {
        }

        public IShellFolder BindToFolder()
        {
            return this.Folder.BindToFolder(base.RelativePidl);
        }

        public SFGAO GetAttributesOf(SFGAO mask)
        {
            IntPtr[] apidl = new IntPtr[] { base.RelativePidl };
            this.Folder.GetAttributesOf(1, apidl, ref mask);
            return mask;
        }

        public string GetDisplayNameOf(SHGNO uFlags)
        {
            return this.Folder.GetDisplayNameOf(base.RelativePidl, uFlags);
        }

        public SafeShellItem GetParent()
        {
            IntPtr parent = ShellItem.GetParent(base.AbsolutePidl);
            if (parent != IntPtr.Zero)
            {
                return new SafeShellItem(parent);
            }
            return null;
        }

        public T GetUIObjectOf<T>(IntPtr hwndOwner)
        {
            IntPtr[] apidl = new IntPtr[] { base.RelativePidl };
            return this.Folder.GetUIObjectOf<T>(hwndOwner, apidl);
        }

        public IShellFolder Folder
        {
            get
            {
                if (!((Thread.CurrentThread.ManagedThreadId == this.ThreadId) && this.WeakFolder.IsAlive))
                {
                    IShellFolder target = base.GetFolder();
                    this.ThreadId = Thread.CurrentThread.ManagedThreadId;
                    this.WeakFolder = new WeakReference(target);
                    return target;
                }
                return (IShellFolder) this.WeakFolder.Target;
            }
        }
    }
}

