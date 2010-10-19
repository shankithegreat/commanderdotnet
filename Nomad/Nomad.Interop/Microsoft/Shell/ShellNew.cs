namespace Microsoft.Shell
{
    using Microsoft;
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    [DebuggerDisplay("{GetType().Name}, {Name}")]
    public sealed class ShellNew
    {
        private string FCommand;
        private ShellNewCommand FCommandType;
        private string FExtension;
        private string FFriendlyName;
        private string FIconPath;
        private string FName;
        private static ShellNew[] FShellNewList;

        public ShellNew(ShellNewCommand commandType, string ext, string command)
        {
            switch (commandType)
            {
                case ShellNewCommand.NullFile:
                    if (command != null)
                    {
                        throw new ArgumentException();
                    }
                    break;

                case ShellNewCommand.Command:
                case ShellNewCommand.Handler:
                case ShellNewCommand.FileName:
                    if (command == null)
                    {
                        throw new ArgumentNullException();
                    }
                    if (command == string.Empty)
                    {
                        throw new ArgumentException();
                    }
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }
            if (ext == null)
            {
                throw new ArgumentNullException();
            }
            this.FExtension = ((ext == string.Empty) || (ext[0] == '.')) ? ext : ('.' + ext);
            this.FCommandType = commandType;
            this.FCommand = command;
        }

        private ShellNew(string ext, string name, ShellNewCommand commandType, RegistryKey shellNewKey)
        {
            this.FExtension = ext;
            this.FName = name;
            this.FCommandType = commandType;
            switch (this.FCommandType)
            {
                case ShellNewCommand.Data:
                    this.FCommand = shellNewKey.Name;
                    break;

                case ShellNewCommand.Command:
                    this.FCommand = (string) shellNewKey.GetValue("Command");
                    break;

                case ShellNewCommand.Handler:
                    this.FCommand = (string) shellNewKey.GetValue("Handler");
                    break;

                case ShellNewCommand.FileName:
                    this.FCommand = (string) shellNewKey.GetValue("FileName");
                    break;
            }
            this.FFriendlyName = (string) shellNewKey.GetValue("ItemName");
            this.FIconPath = (string) shellNewKey.GetValue("IconPath");
        }

        private static ShellNew Create(string extension, string name, RegistryKey shellNewKey)
        {
            ShellNewCommand unknown = ShellNewCommand.Unknown;
            ShellNewCommand fileName = unknown;
            foreach (string str in shellNewKey.GetValueNames())
            {
                string str2 = str.ToLower();
                if (str2 != null)
                {
                    if (!(str2 == "filename"))
                    {
                        if (str2 == "command")
                        {
                            goto Label_0075;
                        }
                        if (str2 == "data")
                        {
                            goto Label_0079;
                        }
                        if (str2 == "nullfile")
                        {
                            goto Label_007D;
                        }
                        if (str2 == "handler")
                        {
                            goto Label_0081;
                        }
                    }
                    else
                    {
                        fileName = ShellNewCommand.FileName;
                    }
                }
                goto Label_0085;
            Label_0075:
                fileName = ShellNewCommand.Command;
                goto Label_0085;
            Label_0079:
                fileName = ShellNewCommand.Data;
                goto Label_0085;
            Label_007D:
                fileName = ShellNewCommand.NullFile;
                goto Label_0085;
            Label_0081:
                fileName = ShellNewCommand.Handler;
            Label_0085:
                if (fileName > unknown)
                {
                    unknown = fileName;
                }
            }
            if (unknown == ShellNewCommand.Unknown)
            {
                return null;
            }
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(name))
            {
                if (key != null)
                {
                    name = (string) key.GetValue(null);
                }
            }
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            return new ShellNew(extension, name, unknown, shellNewKey);
        }

        public string CreateFile(string folder, string name)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }
            if (folder == string.Empty)
            {
                throw new ArgumentException();
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name == string.Empty)
            {
                throw new ArgumentException();
            }
            string path = Path.Combine(folder, name + this.FExtension);
            switch (this.FCommandType)
            {
                case ShellNewCommand.NullFile:
                case ShellNewCommand.Data:
                    using (FileStream stream = File.Create(path))
                    {
                        if (this.FCommandType != ShellNewCommand.Data)
                        {
                            return path;
                        }
                        byte[] data = this.Data;
                        if (data != null)
                        {
                            stream.Write(data, 0, data.Length);
                        }
                    }
                    return path;

                case ShellNewCommand.FileName:
                    this.ResolveFileName();
                    if (!File.Exists(this.FCommand))
                    {
                        break;
                    }
                    File.Copy(this.FCommand, path, true);
                    return path;
            }
            return null;
        }

        public static ShellNew Get(string extension)
        {
            if (!string.IsNullOrEmpty(extension) && (extension[0] == '.'))
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(extension))
                {
                    RegistryKey key2;
                    string str = (string) key.GetValue(null);
                    if (string.IsNullOrEmpty(str))
                    {
                        return null;
                    }
                    key2 = key2 = key.OpenSubKey("ShellNew");
                    if (key2 == null)
                    {
                        key2 = key.OpenSubKey(str + @"\ShellNew");
                    }
                    if (key2 != null)
                    {
                        try
                        {
                            ShellNew new2 = Create(extension, str, key2);
                            if (new2 != null)
                            {
                                return new2;
                            }
                        }
                        finally
                        {
                            ((IDisposable) key2).Dispose();
                        }
                    }
                }
            }
            return null;
        }

        public static IEnumerable<ShellNew> GetAll()
        {
            return new <GetAll>d__0(-2);
        }

        public void ResolveFileName()
        {
            if (this.CommandType != ShellNewCommand.FileName)
            {
                throw new InvalidOperationException();
            }
            if (!Path.IsPathRooted(this.Command))
            {
                string[] strArray = new string[] { Environment.GetFolderPath(Environment.SpecialFolder.Templates), Path.Combine(OS.WindowDirectory, "ShellNew") };
                foreach (string str in strArray)
                {
                    string path = Path.Combine(str, this.FCommand);
                    if (File.Exists(path))
                    {
                        this.FCommand = path;
                        break;
                    }
                }
            }
        }

        public static ShellNew[] All
        {
            get
            {
                if (FShellNewList == null)
                {
                    List<ShellNew> list = new List<ShellNew>(Registry.ClassesRoot.SubKeyCount);
                    list.AddRange(GetAll());
                    FShellNewList = list.ToArray();
                }
                return FShellNewList;
            }
        }

        public string Command
        {
            get
            {
                switch (this.FCommandType)
                {
                    case ShellNewCommand.Command:
                    case ShellNewCommand.Handler:
                    case ShellNewCommand.FileName:
                        return this.FCommand;
                }
                return null;
            }
        }

        public ShellNewCommand CommandType
        {
            get
            {
                return this.FCommandType;
            }
        }

        public byte[] Data
        {
            get
            {
                if (this.FCommandType != ShellNewCommand.Data)
                {
                    return null;
                }
                object obj2 = Registry.GetValue(this.FCommand, "Data", null);
                string s = obj2 as string;
                if (s != null)
                {
                    return Encoding.ASCII.GetBytes(s);
                }
                return (obj2 as byte[]);
            }
        }

        public string Extension
        {
            get
            {
                return this.FExtension;
            }
        }

        public string FriendlyName
        {
            get
            {
                if (this.FFriendlyName == null)
                {
                    using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(this.FName))
                    {
                        if (key != null)
                        {
                            this.FFriendlyName = (string) key.GetValue("FriendlyTypeName");
                        }
                    }
                    if (string.IsNullOrEmpty(this.FFriendlyName))
                    {
                        this.FFriendlyName = this.FName;
                    }
                }
                return this.FFriendlyName;
            }
        }

        public string IconPath
        {
            get
            {
                return this.FIconPath;
            }
        }

        public string Name
        {
            get
            {
                return this.FName;
            }
        }

        [CompilerGenerated]
        private sealed class <GetAll>d__0 : IEnumerable<ShellNew>, IEnumerable, IEnumerator<ShellNew>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ShellNew <>2__current;
            public string[] <>7__wrap4;
            public int <>7__wrap5;
            private int <>l__initialThreadId;
            public ShellNew <Command>5__2;
            public string <NextExt>5__1;

            [DebuggerHidden]
            public <GetAll>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally3()
            {
                this.<>1__state = -1;
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>1__state = 1;
                            this.<>7__wrap4 = Registry.ClassesRoot.GetSubKeyNames();
                            this.<>7__wrap5 = 0;
                            while (this.<>7__wrap5 < this.<>7__wrap4.Length)
                            {
                                this.<NextExt>5__1 = this.<>7__wrap4[this.<>7__wrap5];
                                this.<Command>5__2 = ShellNew.Get(this.<NextExt>5__1);
                                if (this.<Command>5__2 == null)
                                {
                                    goto Label_009D;
                                }
                                this.<>2__current = this.<Command>5__2;
                                this.<>1__state = 2;
                                return true;
                            Label_0096:
                                this.<>1__state = 1;
                            Label_009D:
                                this.<>7__wrap5++;
                            }
                            this.<>m__Finally3();
                            break;

                        case 2:
                            goto Label_0096;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<ShellNew> IEnumerable<ShellNew>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new ShellNew.<GetAll>d__0(0);
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Microsoft.Shell.ShellNew>.GetEnumerator();
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
                        this.<>m__Finally3();
                        break;
                }
            }

            ShellNew IEnumerator<ShellNew>.Current
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

