namespace TagLib
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public sealed class SupportedMimeType : Attribute
    {
        private string extension;
        private string mimetype;
        private static List<SupportedMimeType> mimetypes = new List<SupportedMimeType>();

        static SupportedMimeType()
        {
            FileTypes.Init();
        }

        public SupportedMimeType(string mimetype)
        {
            this.mimetype = mimetype;
            mimetypes.Add(this);
        }

        public SupportedMimeType(string mimetype, string extension) : this(mimetype)
        {
            this.extension = extension;
        }

        public static IEnumerable<string> AllExtensions
        {
            get
            {
                return new <>c__IteratorA { $PC = -2 };
            }
        }

        public static IEnumerable<string> AllMimeTypes
        {
            get
            {
                return new <>c__Iterator9 { $PC = -2 };
            }
        }

        public string Extension
        {
            get
            {
                return this.extension;
            }
        }

        public string MimeType
        {
            get
            {
                return this.mimetype;
            }
        }

        [CompilerGenerated]
        private sealed class <>c__Iterator9 : IDisposable, IEnumerator, IEnumerable, IEnumerable<string>, IEnumerator<string>
        {
            internal string $current;
            internal int $PC;
            internal List<SupportedMimeType>.Enumerator <$s_282>__0;
            internal SupportedMimeType <type>__1;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.<$s_282>__0.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.<$s_282>__0 = SupportedMimeType.mimetypes.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00A8;
                }
                try
                {
                    while (this.<$s_282>__0.MoveNext())
                    {
                        this.<type>__1 = this.<$s_282>__0.Current;
                        this.$current = this.<type>__1.MimeType;
                        this.$PC = 1;
                        flag = true;
                        return true;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_282>__0.Dispose();
                }
                this.$PC = -1;
            Label_00A8:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new SupportedMimeType.<>c__Iterator9();
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
            }

            string IEnumerator<string>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <>c__IteratorA : IDisposable, IEnumerator, IEnumerable, IEnumerable<string>, IEnumerator<string>
        {
            internal string $current;
            internal int $PC;
            internal List<SupportedMimeType>.Enumerator <$s_283>__0;
            internal SupportedMimeType <type>__1;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.<$s_283>__0.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.<$s_283>__0 = SupportedMimeType.mimetypes.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00B8;
                }
                try
                {
                    while (this.<$s_283>__0.MoveNext())
                    {
                        this.<type>__1 = this.<$s_283>__0.Current;
                        if (this.<type>__1.Extension != null)
                        {
                            this.$current = this.<type>__1.Extension;
                            this.$PC = 1;
                            flag = true;
                            return true;
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_283>__0.Dispose();
                }
                this.$PC = -1;
            Label_00B8:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new SupportedMimeType.<>c__IteratorA();
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
            }

            string IEnumerator<string>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

