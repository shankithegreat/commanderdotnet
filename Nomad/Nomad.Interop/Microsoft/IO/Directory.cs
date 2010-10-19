namespace Microsoft.IO
{
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class Directory
    {
        private static IEnumerable<Microsoft.Win32.WIN32_FIND_DATA> GetContent(string directoryPath, string fileMask)
        {
            return new <GetContent>d__0(-2) { <>3__directoryPath = directoryPath, <>3__fileMask = fileMask };
        }

        public static IEnumerable<FileSystemInfo> GetFileSystemInfos(string directoryPath)
        {
            return GetFileSystemInfos(directoryPath, "*");
        }

        public static IEnumerable<FileSystemInfo> GetFileSystemInfos(string directoryPath, string fileMask)
        {
            return new <GetFileSystemInfos>d__14(-2) { <>3__directoryPath = directoryPath, <>3__fileMask = fileMask };
        }

        public static IEnumerable<string> GetItemNames(string directoryPath)
        {
            return GetItemNames(directoryPath, "*");
        }

        public static IEnumerable<string> GetItemNames(string directoryPath, string fileMask)
        {
            return new <GetItemNames>d__e(-2) { <>3__directoryPath = directoryPath, <>3__fileMask = fileMask };
        }

        public static IEnumerable<ReadOnlyFileSystemInfo> GetItems(string directoryPath)
        {
            return GetItems(directoryPath, "*");
        }

        public static IEnumerable<ReadOnlyFileSystemInfo> GetItems(string directoryPath, string fileMask)
        {
            return new <GetItems>d__8(-2) { <>3__directoryPath = directoryPath, <>3__fileMask = fileMask };
        }

        public static string Rename(string directoryPath, string newName)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException("directoryPath");
            }
            if (directoryPath == string.Empty)
            {
                throw new ArgumentException();
            }
            if (newName == null)
            {
                throw new ArgumentNullException("newName");
            }
            if (newName == string.Empty)
            {
                throw new ArgumentException();
            }
            if (newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new ArgumentException();
            }
            directoryPath = IoHelper.StripTrailingPathSeparator(directoryPath);
            string lpNewFileName = Path.Combine(Path.GetDirectoryName(directoryPath), newName);
            if (!Windows.MoveFile(directoryPath, lpNewFileName))
            {
                throw IoHelper.GetIOException();
            }
            return lpNewFileName;
        }

        public static void SetCompressedState(string directoryPath, CompressionFormat format)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException();
            }
            if (directoryPath == string.Empty)
            {
                throw new ArgumentException();
            }
            SafeFileHandle handle = Windows.CreateFile(directoryPath, FileAccess.ReadWrite, FileShare.None, IntPtr.Zero, FileMode.Open, 0x2000000, IntPtr.Zero);
            if (handle.IsInvalid)
            {
                Win32Exception innerException = new Win32Exception();
                switch (innerException.NativeErrorCode)
                {
                    case 2:
                    case 3:
                        throw new DirectoryNotFoundException(innerException.Message, innerException);

                    case 0x15:
                        throw new DeviceNotReadyException(innerException.Message, innerException);
                }
                throw new Win32IOException(innerException);
            }
            using (handle)
            {
                Microsoft.IO.File.SetCompressedState(handle, format);
            }
        }

        public static void SetCompressedState(string directoryPath, bool compress)
        {
            SetCompressedState(directoryPath, compress ? CompressionFormat.Default : CompressionFormat.None);
        }

        public static void SetCreationTime(string path, DateTime creationTime)
        {
            DateTime? lastAccessTime = null;
            SetDirectoryTimes(path, new DateTime?(creationTime), lastAccessTime, null);
        }

        private static void SetDirectoryTimes(string path, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime)
        {
            using (SafeFileHandle handle = Microsoft.IO.File.Open(path, FileMode.Open, 0x100, FileShare.Delete | FileShare.Write, 0x2000000))
            {
                Microsoft.IO.File.SetFileTimes(handle, creationTime, lastAccessTime, lastWriteTime);
            }
        }

        public static void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            DateTime? creationTime = null;
            SetDirectoryTimes(path, creationTime, new DateTime?(lastAccessTime), null);
        }

        public static void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            DateTime? creationTime = null;
            SetDirectoryTimes(path, creationTime, null, new DateTime?(lastWriteTime));
        }

        [CompilerGenerated]
        private sealed class <GetContent>d__0 : IEnumerable<Microsoft.Win32.WIN32_FIND_DATA>, IEnumerable, IEnumerator<Microsoft.Win32.WIN32_FIND_DATA>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private Microsoft.Win32.WIN32_FIND_DATA <>2__current;
            public string <>3__directoryPath;
            public string <>3__fileMask;
            public Microsoft.Win32.SafeHandles.SafeFindHandle <>7__wrap4;
            private int <>l__initialThreadId;
            public int <ErrorCode>5__3;
            public Microsoft.Win32.WIN32_FIND_DATA <FindData>5__1;
            public Microsoft.Win32.SafeHandles.SafeFindHandle <FindHandle>5__2;
            public string directoryPath;
            public string fileMask;

            [DebuggerHidden]
            public <GetContent>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally5()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap4 != null)
                {
                    this.<>7__wrap4.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<FindHandle>5__2 = Windows.FindFirstFile(Path.Combine(this.directoryPath, this.fileMask), out this.<FindData>5__1);
                            if (this.<FindHandle>5__2.IsInvalid)
                            {
                                int nativeErrorCode = Marshal.GetLastWin32Error();
                                if ((nativeErrorCode != 2) || !(Path.GetPathRoot(this.directoryPath) == this.directoryPath))
                                {
                                    throw IoHelper.GetIOException(nativeErrorCode);
                                }
                                goto Label_014B;
                            }
                            break;

                        case 2:
                            goto Label_0100;

                        default:
                            goto Label_014B;
                    }
                    this.<>7__wrap4 = this.<FindHandle>5__2;
                    this.<>1__state = 1;
                Label_00AE:
                    if (this.<FindData>5__1.cFileName.Equals(".", StringComparison.Ordinal) || this.<FindData>5__1.cFileName.Equals("..", StringComparison.Ordinal))
                    {
                        goto Label_0108;
                    }
                    this.<>2__current = this.<FindData>5__1;
                    this.<>1__state = 2;
                    return true;
                Label_0100:
                    this.<>1__state = 1;
                Label_0108:
                    if (Windows.FindNextFile(this.<FindHandle>5__2, out this.<FindData>5__1))
                    {
                        goto Label_00AE;
                    }
                    this.<ErrorCode>5__3 = Marshal.GetLastWin32Error();
                    if (this.<ErrorCode>5__3 != 0x12)
                    {
                        throw IoHelper.GetIOException(this.<ErrorCode>5__3);
                    }
                    this.<>m__Finally5();
                Label_014B:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<Microsoft.Win32.WIN32_FIND_DATA> IEnumerable<Microsoft.Win32.WIN32_FIND_DATA>.GetEnumerator()
            {
                Microsoft.IO.Directory.<GetContent>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new Microsoft.IO.Directory.<GetContent>d__0(0);
                }
                d__.directoryPath = this.<>3__directoryPath;
                d__.fileMask = this.<>3__fileMask;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Microsoft.Win32.WIN32_FIND_DATA>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally5();
                        }
                        break;
                }
            }

            Microsoft.Win32.WIN32_FIND_DATA IEnumerator<Microsoft.Win32.WIN32_FIND_DATA>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetFileSystemInfos>d__14 : IEnumerable<FileSystemInfo>, IEnumerable, IEnumerator<FileSystemInfo>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private FileSystemInfo <>2__current;
            public string <>3__directoryPath;
            public string <>3__fileMask;
            public IEnumerator<Microsoft.Win32.WIN32_FIND_DATA> <>7__wrap17;
            private int <>l__initialThreadId;
            public Microsoft.Win32.WIN32_FIND_DATA <NextFindData>5__15;
            public FileSystemInfo <Result>5__16;
            public string directoryPath;
            public string fileMask;

            [DebuggerHidden]
            public <GetFileSystemInfos>d__14(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally18()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap17 != null)
                {
                    this.<>7__wrap17.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    int num = this.<>1__state;
                    if (num != 0)
                    {
                        if (num != 3)
                        {
                            goto Label_0134;
                        }
                        goto Label_0112;
                    }
                    this.<>1__state = -1;
                    this.<>7__wrap17 = Microsoft.IO.Directory.GetContent(this.directoryPath, this.fileMask).GetEnumerator();
                    this.<>1__state = 1;
                    while (this.<>7__wrap17.MoveNext())
                    {
                        this.<NextFindData>5__15 = this.<>7__wrap17.Current;
                        try
                        {
                            string path = Path.Combine(this.directoryPath, this.<NextFindData>5__15.cFileName);
                            if ((this.<NextFindData>5__15.dwFileAttributes & FileAttributes.Directory) > 0)
                            {
                                this.<Result>5__16 = new DirectoryInfo(path);
                            }
                            else
                            {
                                this.<Result>5__16 = new FileInfo(path);
                            }
                        }
                        catch (ArgumentException)
                        {
                            this.<Result>5__16 = new UnreadableFileSystemInfo(this.directoryPath + Path.DirectorySeparatorChar + this.<NextFindData>5__15.cFileName, this.<NextFindData>5__15.cFileName);
                        }
                        IoHelper.InitializeFileSystemInfo(this.<Result>5__16, this.<NextFindData>5__15);
                        this.<>2__current = this.<Result>5__16;
                        this.<>1__state = 3;
                        return true;
                    Label_0112:
                        this.<>1__state = 1;
                    }
                    this.<>m__Finally18();
                Label_0134:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<FileSystemInfo> IEnumerable<FileSystemInfo>.GetEnumerator()
            {
                Microsoft.IO.Directory.<GetFileSystemInfos>d__14 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new Microsoft.IO.Directory.<GetFileSystemInfos>d__14(0);
                }
                d__.directoryPath = this.<>3__directoryPath;
                d__.fileMask = this.<>3__fileMask;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.IO.FileSystemInfo>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally18();
                        }
                        break;
                }
            }

            FileSystemInfo IEnumerator<FileSystemInfo>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetItemNames>d__e : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private string <>2__current;
            public string <>3__directoryPath;
            public string <>3__fileMask;
            public IEnumerator<Microsoft.Win32.WIN32_FIND_DATA> <>7__wrap10;
            private int <>l__initialThreadId;
            public Microsoft.Win32.WIN32_FIND_DATA <NextFindData>5__f;
            public string directoryPath;
            public string fileMask;

            [DebuggerHidden]
            public <GetItemNames>d__e(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally11()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap10 != null)
                {
                    this.<>7__wrap10.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrap10 = Microsoft.IO.Directory.GetContent(this.directoryPath, this.fileMask).GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap10.MoveNext())
                            {
                                this.<NextFindData>5__f = this.<>7__wrap10.Current;
                                this.<>2__current = this.<NextFindData>5__f.cFileName;
                                this.<>1__state = 2;
                                return true;
                            Label_007C:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally11();
                            break;

                        case 2:
                            goto Label_007C;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                Microsoft.IO.Directory.<GetItemNames>d__e _e;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    _e = this;
                }
                else
                {
                    _e = new Microsoft.IO.Directory.<GetItemNames>d__e(0);
                }
                _e.directoryPath = this.<>3__directoryPath;
                _e.fileMask = this.<>3__fileMask;
                return _e;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.String>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally11();
                        }
                        break;
                }
            }

            string IEnumerator<string>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetItems>d__8 : IEnumerable<ReadOnlyFileSystemInfo>, IEnumerable, IEnumerator<ReadOnlyFileSystemInfo>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ReadOnlyFileSystemInfo <>2__current;
            public string <>3__directoryPath;
            public string <>3__fileMask;
            public IEnumerator<Microsoft.Win32.WIN32_FIND_DATA> <>7__wrapa;
            private int <>l__initialThreadId;
            public Microsoft.Win32.WIN32_FIND_DATA <NextFindData>5__9;
            public string directoryPath;
            public string fileMask;

            [DebuggerHidden]
            public <GetItems>d__8(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finallyb()
            {
                this.<>1__state = -1;
                if (this.<>7__wrapa != null)
                {
                    this.<>7__wrapa.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrapa = Microsoft.IO.Directory.GetContent(this.directoryPath, this.fileMask).GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrapa.MoveNext())
                            {
                                this.<NextFindData>5__9 = this.<>7__wrapa.Current;
                                this.<>2__current = new ReadOnlyFileSystemInfo(this.directoryPath, this.<NextFindData>5__9);
                                this.<>1__state = 2;
                                return true;
                            Label_0082:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finallyb();
                            break;

                        case 2:
                            goto Label_0082;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<ReadOnlyFileSystemInfo> IEnumerable<ReadOnlyFileSystemInfo>.GetEnumerator()
            {
                Microsoft.IO.Directory.<GetItems>d__8 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new Microsoft.IO.Directory.<GetItems>d__8(0);
                }
                d__.directoryPath = this.<>3__directoryPath;
                d__.fileMask = this.<>3__fileMask;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Microsoft.IO.ReadOnlyFileSystemInfo>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finallyb();
                        }
                        break;
                }
            }

            ReadOnlyFileSystemInfo IEnumerator<ReadOnlyFileSystemInfo>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

