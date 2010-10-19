namespace Nomad.FileSystem.Archive.Common
{
    using Nomad.FileSystem.Archive.SevenZip;
    using Nomad.FileSystem.Archive.Wcx;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class ArchiveFormatManager
    {
        private static int FClassFormatCount;
        private static Dictionary<Guid, ArchiveFormatInfo> FClassMap;
        private static Dictionary<string, List<ArchiveFormatInfo>> FExtensionMap;

        public static IEnumerable<FindFormatResult> FindFormat(Stream stream)
        {
            return new <FindFormat>d__10(-2) { <>3__stream = stream };
        }

        public static IEnumerable<FindFormatResult> FindFormat(string extension)
        {
            return new <FindFormat>d__9(-2) { <>3__extension = extension };
        }

        public static ICollection<FindFormatResult> FindFormat(Stream stream, string extension)
        {
            Dictionary<ArchiveFormatInfo, FindFormatResult> dictionary = new Dictionary<ArchiveFormatInfo, FindFormatResult>();
            foreach (FindFormatResult result in FindFormat(stream))
            {
                dictionary.Add(result.Format, result);
            }
            foreach (FindFormatResult result in FindFormat(extension))
            {
                FindFormatResult result2;
                if (dictionary.TryGetValue(result.Format, out result2))
                {
                    result2.FindSource |= FindFormatSource.Extension;
                }
                else
                {
                    dictionary.Add(result.Format, result);
                }
            }
            return dictionary.Values;
        }

        public static ArchiveFormatInfo GetFormat(Guid classId)
        {
            ArchiveFormatInfo info;
            if (ClassFormatMap.TryGetValue(classId, out info))
            {
                return info;
            }
            return null;
        }

        public static ArchiveFormatInfo GetFormat(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            if (name != string.Empty)
            {
                foreach (ArchiveFormatInfo info in GetFormats())
                {
                    if (info.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return info;
                    }
                }
            }
            return null;
        }

        public static IEnumerable<ArchiveFormatInfo> GetFormats()
        {
            return new <GetFormats>d__0(-2);
        }

        internal static void ResetExtensionMap()
        {
            FExtensionMap = null;
        }

        private static IDictionary<Guid, ArchiveFormatInfo> ClassFormatMap
        {
            get
            {
                int formatCount = FormatCount;
                if ((FClassMap == null) || (FClassFormatCount != formatCount))
                {
                    FClassMap = new Dictionary<Guid, ArchiveFormatInfo>();
                    foreach (ArchiveFormatInfo info in GetFormats())
                    {
                        FClassMap.Add(info.ClassId, info);
                    }
                    FClassFormatCount = formatCount;
                }
                return FClassMap;
            }
        }

        private static IDictionary<string, List<ArchiveFormatInfo>> ExtensionFormatMap
        {
            get
            {
                if (FExtensionMap == null)
                {
                    FExtensionMap = new Dictionary<string, List<ArchiveFormatInfo>>(StringComparer.OrdinalIgnoreCase);
                    foreach (ArchiveFormatInfo info in GetFormats())
                    {
                        if (info.Extension != null)
                        {
                            foreach (string str in info.Extension)
                            {
                                List<ArchiveFormatInfo> list;
                                if (FExtensionMap.TryGetValue(str, out list))
                                {
                                    list.Add(info);
                                }
                                else
                                {
                                    list = new List<ArchiveFormatInfo> {
                                        info
                                    };
                                    FExtensionMap.Add(str, list);
                                }
                            }
                        }
                    }
                }
                return FExtensionMap;
            }
        }

        public static int FormatCount
        {
            get
            {
                return (SevenZipFormatInfo.Formats.Count + WcxFormatInfo.Formats.Count);
            }
        }

        [CompilerGenerated]
        private sealed class <FindFormat>d__10 : IEnumerable<FindFormatResult>, IEnumerable, IEnumerator<FindFormatResult>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private FindFormatResult <>2__current;
            public Stream <>3__stream;
            public IEnumerator<FindFormatResult> <>7__wrap13;
            public IEnumerator<FindFormatResult> <>7__wrap15;
            private int <>l__initialThreadId;
            public FindFormatResult <NextSevenZipResult>5__11;
            public FindFormatResult <NextWcxResult>5__12;
            public Stream stream;

            [DebuggerHidden]
            public <FindFormat>d__10(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally14()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap13 != null)
                {
                    this.<>7__wrap13.Dispose();
                }
            }

            private void <>m__Finally16()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap15 != null)
                {
                    this.<>7__wrap15.Dispose();
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
                            break;

                        case 2:
                            goto Label_0098;

                        case 4:
                            goto Label_0111;

                        default:
                            goto Label_012F;
                    }
                    this.<>1__state = -1;
                    this.<>7__wrap13 = SevenZipFormatInfo.FindFormat(this.stream).GetEnumerator();
                    this.<>1__state = 1;
                    while (this.<>7__wrap13.MoveNext())
                    {
                        this.<NextSevenZipResult>5__11 = this.<>7__wrap13.Current;
                        if (this.<NextSevenZipResult>5__11.Format.Disabled)
                        {
                            continue;
                        }
                        this.<>2__current = this.<NextSevenZipResult>5__11;
                        this.<>1__state = 2;
                        return true;
                    Label_0098:
                        this.<>1__state = 1;
                    }
                    this.<>m__Finally14();
                    this.<>7__wrap15 = WcxFormatInfo.FindFormat(this.stream).GetEnumerator();
                    this.<>1__state = 3;
                    while (this.<>7__wrap15.MoveNext())
                    {
                        this.<NextWcxResult>5__12 = this.<>7__wrap15.Current;
                        if (this.<NextWcxResult>5__12.Format.Disabled)
                        {
                            continue;
                        }
                        this.<>2__current = this.<NextWcxResult>5__12;
                        this.<>1__state = 4;
                        return true;
                    Label_0111:
                        this.<>1__state = 3;
                    }
                    this.<>m__Finally16();
                Label_012F:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<FindFormatResult> IEnumerable<FindFormatResult>.GetEnumerator()
            {
                ArchiveFormatManager.<FindFormat>d__10 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new ArchiveFormatManager.<FindFormat>d__10(0);
                }
                d__.stream = this.<>3__stream;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Archive.Common.FindFormatResult>.GetEnumerator();
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
                            this.<>m__Finally14();
                        }
                        break;

                    case 3:
                    case 4:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally16();
                        }
                        break;
                }
            }

            FindFormatResult IEnumerator<FindFormatResult>.Current
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
        private sealed class <FindFormat>d__9 : IEnumerable<FindFormatResult>, IEnumerable, IEnumerator<FindFormatResult>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private FindFormatResult <>2__current;
            public string <>3__extension;
            public List<ArchiveFormatInfo>.Enumerator <>7__wrapc;
            private int <>l__initialThreadId;
            public List<ArchiveFormatInfo> <FormatList>5__a;
            public ArchiveFormatInfo <NextFormat>5__b;
            public string extension;

            [DebuggerHidden]
            public <FindFormat>d__9(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finallyd()
            {
                this.<>1__state = -1;
                this.<>7__wrapc.Dispose();
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            break;

                        case 2:
                            goto Label_00EA;

                        default:
                            goto Label_0109;
                    }
                    this.<>1__state = -1;
                    if (this.extension == null)
                    {
                        throw new ArgumentNullException("extension");
                    }
                    if (this.extension.StartsWith(".", StringComparison.Ordinal))
                    {
                        this.extension = this.extension.Substring(1);
                    }
                    if (ArchiveFormatManager.ExtensionFormatMap.TryGetValue(this.extension, out this.<FormatList>5__a))
                    {
                        this.<>7__wrapc = this.<FormatList>5__a.GetEnumerator();
                        this.<>1__state = 1;
                        while (this.<>7__wrapc.MoveNext())
                        {
                            this.<NextFormat>5__b = this.<>7__wrapc.Current;
                            if (this.<NextFormat>5__b.Disabled)
                            {
                                continue;
                            }
                            this.<>2__current = new FindFormatResult(this.<NextFormat>5__b, FindFormatSource.Extension);
                            this.<>1__state = 2;
                            return true;
                        Label_00EA:
                            this.<>1__state = 1;
                        }
                        this.<>m__Finallyd();
                    }
                Label_0109:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<FindFormatResult> IEnumerable<FindFormatResult>.GetEnumerator()
            {
                ArchiveFormatManager.<FindFormat>d__9 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new ArchiveFormatManager.<FindFormat>d__9(0);
                }
                d__.extension = this.<>3__extension;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Archive.Common.FindFormatResult>.GetEnumerator();
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
                            this.<>m__Finallyd();
                        }
                        break;
                }
            }

            FindFormatResult IEnumerator<FindFormatResult>.Current
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
        private sealed class <GetFormats>d__0 : IEnumerable<ArchiveFormatInfo>, IEnumerable, IEnumerator<ArchiveFormatInfo>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ArchiveFormatInfo <>2__current;
            public IEnumerator<SevenZipFormatInfo> <>7__wrap3;
            public IEnumerator<WcxFormatInfo> <>7__wrap5;
            private int <>l__initialThreadId;
            public ArchiveFormatInfo <NextFormatInfo>5__1;
            public ArchiveFormatInfo <NextFormatInfo>5__2;

            [DebuggerHidden]
            public <GetFormats>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally4()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap3 != null)
                {
                    this.<>7__wrap3.Dispose();
                }
            }

            private void <>m__Finally6()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap5 != null)
                {
                    this.<>7__wrap5.Dispose();
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
                            this.<>7__wrap3 = SevenZipFormatInfo.Formats.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap3.MoveNext())
                            {
                                this.<NextFormatInfo>5__1 = this.<>7__wrap3.Current;
                                this.<>2__current = this.<NextFormatInfo>5__1;
                                this.<>1__state = 2;
                                return true;
                            Label_007E:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally4();
                            this.<>7__wrap5 = WcxFormatInfo.Formats.GetEnumerator();
                            this.<>1__state = 3;
                            while (this.<>7__wrap5.MoveNext())
                            {
                                this.<NextFormatInfo>5__2 = this.<>7__wrap5.Current;
                                this.<>2__current = this.<NextFormatInfo>5__2;
                                this.<>1__state = 4;
                                return true;
                            Label_00DD:
                                this.<>1__state = 3;
                            }
                            this.<>m__Finally6();
                            break;

                        case 2:
                            goto Label_007E;

                        case 4:
                            goto Label_00DD;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<ArchiveFormatInfo> IEnumerable<ArchiveFormatInfo>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new ArchiveFormatManager.<GetFormats>d__0(0);
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Archive.Common.ArchiveFormatInfo>.GetEnumerator();
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

                    case 3:
                    case 4:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally6();
                        }
                        break;
                }
            }

            ArchiveFormatInfo IEnumerator<ArchiveFormatInfo>.Current
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

