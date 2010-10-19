namespace Microsoft.Shell
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class ShellFolderExtension
    {
        public static IShellFolder BindToFolder(this IShellFolder folder, IntPtr pidl)
        {
            return folder.BindToObject<IShellFolder>(pidl);
        }

        public static T BindToObject<T>(this IShellFolder folder, IntPtr pidl)
        {
            object obj2;
            folder.BindToObject(pidl, IntPtr.Zero, typeof(T).GUID, out obj2);
            return (T) obj2;
        }

        public static T BindToStorage<T>(this IShellFolder folder, IntPtr pidl)
        {
            object obj2;
            folder.BindToStorage(pidl, IntPtr.Zero, typeof(T).GUID, out obj2);
            return (T) obj2;
        }

        public static IntPtr FindObject(this IShellFolder folder, IntPtr hwndOwner, string relativeName, SHCONTF grfFlags)
        {
            foreach (IntPtr ptr in folder.GetObjects(hwndOwner, grfFlags))
            {
                if (relativeName.Equals(folder.GetDisplayNameOf(ptr, SHGNO.SHGDN_FORPARSING), StringComparison.OrdinalIgnoreCase))
                {
                    return ptr;
                }
                Marshal.FreeCoTaskMem(ptr);
            }
            return IntPtr.Zero;
        }

        public static string GetDisplayNameOf(this IShellFolder folder, IntPtr pidl, SHGNO uFlags)
        {
            string str;
            STRRET strret;
            folder.GetDisplayNameOf(pidl, uFlags, out strret);
            int errorCode = Shell32.StrRetToBSTR(ref strret, pidl, out str);
            if (errorCode != 0)
            {
                throw Marshal.GetExceptionForHR(errorCode);
            }
            return str;
        }

        public static IEnumerable<IntPtr> GetObjects(this IShellFolder folder, IntPtr hwndOwner, SHCONTF grfFlags)
        {
            return new <GetObjects>d__0(-2) { <>3__folder = folder, <>3__hwndOwner = hwndOwner, <>3__grfFlags = grfFlags };
        }

        public static T GetUIObjectOf<T>(this IShellFolder folder, IntPtr hwndOwner, params IntPtr[] apidl)
        {
            object obj2;
            folder.GetUIObjectOf(hwndOwner, apidl.Length, apidl, typeof(T).GUID, IntPtr.Zero, out obj2);
            return (T) obj2;
        }

        public static IntPtr ParseDisplayName(this IShellFolder folder, IntPtr hwndOwner, string lpszDisplayName)
        {
            IntPtr ptr;
            uint num;
            folder.ParseDisplayName(hwndOwner, IntPtr.Zero, lpszDisplayName, out num, out ptr, 0);
            return ptr;
        }

        [CompilerGenerated]
        private sealed class <GetObjects>d__0 : IEnumerable<IntPtr>, IEnumerable, IEnumerator<IntPtr>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IntPtr <>2__current;
            public IShellFolder <>3__folder;
            public SHCONTF <>3__grfFlags;
            public IntPtr <>3__hwndOwner;
            private int <>l__initialThreadId;
            public IEnumIDList <EnumList>5__1;
            public int <Fetched>5__3;
            public IntPtr <NextObject>5__2;
            public IShellFolder folder;
            public SHCONTF grfFlags;
            public IntPtr hwndOwner;

            [DebuggerHidden]
            public <GetObjects>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally4()
            {
                this.<>1__state = -1;
                Marshal.FinalReleaseComObject(this.<EnumList>5__1);
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.folder.EnumObjects(this.hwndOwner, this.grfFlags, out this.<EnumList>5__1);
                            this.<>1__state = 1;
                            this.<NextObject>5__2 = IntPtr.Zero;
                            while ((this.<EnumList>5__1.Next(1, ref this.<NextObject>5__2, out this.<Fetched>5__3) == 0) && (this.<Fetched>5__3 == 1))
                            {
                                this.<>2__current = this.<NextObject>5__2;
                                this.<>1__state = 2;
                                return true;
                            Label_0076:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally4();
                            break;

                        case 2:
                            goto Label_0076;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<IntPtr> IEnumerable<IntPtr>.GetEnumerator()
            {
                ShellFolderExtension.<GetObjects>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new ShellFolderExtension.<GetObjects>d__0(0);
                }
                d__.folder = this.<>3__folder;
                d__.hwndOwner = this.<>3__hwndOwner;
                d__.grfFlags = this.<>3__grfFlags;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.IntPtr>.GetEnumerator();
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
                            this.<>m__Finally4();
                        }
                        break;
                }
            }

            IntPtr IEnumerator<IntPtr>.Current
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

