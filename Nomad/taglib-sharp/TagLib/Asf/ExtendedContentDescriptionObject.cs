namespace TagLib.Asf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TagLib;

    public class ExtendedContentDescriptionObject : TagLib.Asf.Object, IEnumerable<ContentDescriptor>, IEnumerable
    {
        private List<ContentDescriptor> descriptors;

        public ExtendedContentDescriptionObject() : base(TagLib.Asf.Guid.AsfExtendedContentDescriptionObject)
        {
            this.descriptors = new List<ContentDescriptor>();
        }

        public ExtendedContentDescriptionObject(TagLib.Asf.File file, long position) : base(file, position)
        {
            this.descriptors = new List<ContentDescriptor>();
            if (!base.Guid.Equals(TagLib.Asf.Guid.AsfExtendedContentDescriptionObject))
            {
                throw new CorruptFileException("Object GUID incorrect.");
            }
            if (base.OriginalSize < 0x1aL)
            {
                throw new CorruptFileException("Object size too small.");
            }
            ushort num = file.ReadWord();
            for (ushort i = 0; i < num; i = (ushort) (i + 1))
            {
                this.AddDescriptor(new ContentDescriptor(file));
            }
        }

        public void AddDescriptor(ContentDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException("descriptor");
            }
            this.descriptors.Add(descriptor);
        }

        [DebuggerHidden]
        public IEnumerable<ContentDescriptor> GetDescriptors(params string[] names)
        {
            return new <GetDescriptors>c__Iterator1 { names = names, <$>names = names, <>f__this = this, $PC = -2 };
        }

        public IEnumerator<ContentDescriptor> GetEnumerator()
        {
            return this.descriptors.GetEnumerator();
        }

        public void RemoveDescriptors(string name)
        {
            for (int i = this.descriptors.Count - 1; i >= 0; i--)
            {
                if (name == this.descriptors[i].Name)
                {
                    this.descriptors.RemoveAt(i);
                }
            }
        }

        public override ByteVector Render()
        {
            ByteVector vector = new ByteVector();
            ushort num = 0;
            foreach (ContentDescriptor descriptor in this.descriptors)
            {
                num = (ushort) (num + 1);
                vector.Add(descriptor.Render());
            }
            return base.Render(TagLib.Asf.Object.RenderWord(num) + vector);
        }

        public void SetDescriptors(string name, params ContentDescriptor[] descriptors)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            int count = this.descriptors.Count;
            for (int i = this.descriptors.Count - 1; i >= 0; i--)
            {
                if (name == this.descriptors[i].Name)
                {
                    this.descriptors.RemoveAt(i);
                    count = i;
                }
            }
            this.descriptors.InsertRange(count, descriptors);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.descriptors.GetEnumerator();
        }

        public bool IsEmpty
        {
            get
            {
                return (this.descriptors.Count == 0);
            }
        }

        [CompilerGenerated]
        private sealed class <GetDescriptors>c__Iterator1 : IEnumerable<ContentDescriptor>, IEnumerator<ContentDescriptor>, IDisposable, IEnumerator, IEnumerable
        {
            internal ContentDescriptor $current;
            internal int $PC;
            internal string[] <$>names;
            internal string[] <$s_23>__0;
            internal int <$s_24>__1;
            internal List<ContentDescriptor>.Enumerator <$s_25>__3;
            internal ExtendedContentDescriptionObject <>f__this;
            internal ContentDescriptor <desc>__4;
            internal string <name>__2;
            internal string[] names;

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
                            this.<$s_25>__3.Dispose();
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
                        if (this.names == null)
                        {
                            throw new ArgumentNullException("names");
                        }
                        this.<$s_23>__0 = this.names;
                        this.<$s_24>__1 = 0;
                        while (this.<$s_24>__1 < this.<$s_23>__0.Length)
                        {
                            this.<name>__2 = this.<$s_23>__0[this.<$s_24>__1];
                            this.<$s_25>__3 = this.<>f__this.descriptors.GetEnumerator();
                            num = 0xfffffffd;
                        Label_007D:
                            try
                            {
                                while (this.<$s_25>__3.MoveNext())
                                {
                                    this.<desc>__4 = this.<$s_25>__3.Current;
                                    if (this.<desc>__4.Name == this.<name>__2)
                                    {
                                        this.$current = this.<desc>__4;
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
                                this.<$s_25>__3.Dispose();
                            }
                            this.<$s_24>__1++;
                        }
                        this.$PC = -1;
                        break;

                    case 1:
                        goto Label_007D;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<ContentDescriptor> IEnumerable<ContentDescriptor>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new ExtendedContentDescriptionObject.<GetDescriptors>c__Iterator1 { <>f__this = this.<>f__this, names = this.<$>names };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TagLib.Asf.ContentDescriptor>.GetEnumerator();
            }

            ContentDescriptor IEnumerator<ContentDescriptor>.Current
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

