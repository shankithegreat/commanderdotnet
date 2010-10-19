namespace TagLib.Asf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TagLib;

    [SupportedMimeType("taglib/wma", "wma"), SupportedMimeType("taglib/wmv", "wmv"), SupportedMimeType("taglib/asf", "asf"), SupportedMimeType("audio/x-ms-wma"), SupportedMimeType("audio/x-ms-asf"), SupportedMimeType("video/x-ms-asf")]
    public class File : TagLib.File
    {
        private TagLib.Asf.Tag asf_tag;
        private TagLib.Properties properties;

        public File(string path) : this(path, ReadStyle.Average)
        {
        }

        public File(TagLib.File.IFileAbstraction abstraction) : this(abstraction, ReadStyle.Average)
        {
        }

        public File(string path, ReadStyle propertiesStyle) : base(path)
        {
            this.Read(propertiesStyle);
        }

        public File(TagLib.File.IFileAbstraction abstraction, ReadStyle propertiesStyle) : base(abstraction)
        {
            this.Read(propertiesStyle);
        }

        public override TagLib.Tag GetTag(TagTypes type, bool create)
        {
            if (type == TagTypes.Asf)
            {
                return this.asf_tag;
            }
            return null;
        }

        private void Read(ReadStyle propertiesStyle)
        {
            base.Mode = TagLib.File.AccessMode.Read;
            try
            {
                HeaderObject header = new HeaderObject(this, 0L);
                if (header.HasContentDescriptors)
                {
                    base.TagTypesOnDisk |= TagTypes.Asf;
                }
                this.asf_tag = new TagLib.Asf.Tag(header);
                base.InvariantStartPosition = (long) header.OriginalSize;
                base.InvariantEndPosition = base.Length;
                if (propertiesStyle != ReadStyle.None)
                {
                    this.properties = header.Properties;
                }
            }
            finally
            {
                base.Mode = TagLib.File.AccessMode.Closed;
            }
        }

        public uint ReadDWord()
        {
            return base.ReadBlock(4).ToUInt(false);
        }

        public System.Guid ReadGuid()
        {
            return new System.Guid(base.ReadBlock(0x10).Data);
        }

        public TagLib.Asf.Object ReadObject(long position)
        {
            base.Seek(position);
            System.Guid guid = this.ReadGuid();
            if (guid.Equals(TagLib.Asf.Guid.AsfFilePropertiesObject))
            {
                return new FilePropertiesObject(this, position);
            }
            if (guid.Equals(TagLib.Asf.Guid.AsfStreamPropertiesObject))
            {
                return new StreamPropertiesObject(this, position);
            }
            if (guid.Equals(TagLib.Asf.Guid.AsfContentDescriptionObject))
            {
                return new ContentDescriptionObject(this, position);
            }
            if (guid.Equals(TagLib.Asf.Guid.AsfExtendedContentDescriptionObject))
            {
                return new ExtendedContentDescriptionObject(this, position);
            }
            if (guid.Equals(TagLib.Asf.Guid.AsfPaddingObject))
            {
                return new PaddingObject(this, position);
            }
            if (guid.Equals(TagLib.Asf.Guid.AsfHeaderExtensionObject))
            {
                return new HeaderExtensionObject(this, position);
            }
            if (guid.Equals(TagLib.Asf.Guid.AsfMetadataLibraryObject))
            {
                return new MetadataLibraryObject(this, position);
            }
            return new UnknownObject(this, position);
        }

        [DebuggerHidden]
        public IEnumerable<TagLib.Asf.Object> ReadObjects(uint count, long position)
        {
            return new <ReadObjects>c__Iterator3 { count = count, position = position, <$>count = count, <$>position = position, <>f__this = this, $PC = -2 };
        }

        public ulong ReadQWord()
        {
            return base.ReadBlock(8).ToULong(false);
        }

        public string ReadUnicode(int length)
        {
            string str = base.ReadBlock(length).ToString(StringType.UTF16LE);
            int index = str.IndexOf('\0');
            return ((index < 0) ? str : str.Substring(0, index));
        }

        public ushort ReadWord()
        {
            return base.ReadBlock(2).ToUShort(false);
        }

        public override void RemoveTags(TagTypes types)
        {
            if ((types & TagTypes.Asf) == TagTypes.Asf)
            {
                this.asf_tag.Clear();
            }
        }

        public override void Save()
        {
            base.Mode = TagLib.File.AccessMode.Write;
            try
            {
                HeaderObject obj2 = new HeaderObject(this, 0L);
                if (this.asf_tag == null)
                {
                    obj2.RemoveContentDescriptors();
                    base.TagTypesOnDisk &= ~TagTypes.Asf;
                }
                else
                {
                    base.TagTypesOnDisk |= TagTypes.Asf;
                    obj2.AddUniqueObject(this.asf_tag.ContentDescriptionObject);
                    obj2.AddUniqueObject(this.asf_tag.ExtendedContentDescriptionObject);
                    obj2.Extension.AddUniqueObject(this.asf_tag.MetadataLibraryObject);
                }
                ByteVector data = obj2.Render();
                long num = data.Count - ((long) obj2.OriginalSize);
                base.Insert(data, 0L, (long) obj2.OriginalSize);
                base.InvariantStartPosition += num;
                base.InvariantEndPosition += num;
            }
            finally
            {
                base.Mode = TagLib.File.AccessMode.Closed;
            }
        }

        public override TagLib.Properties Properties
        {
            get
            {
                return this.properties;
            }
        }

        public override TagLib.Tag Tag
        {
            get
            {
                return this.asf_tag;
            }
        }

        [CompilerGenerated]
        private sealed class <ReadObjects>c__Iterator3 : IEnumerable<TagLib.Asf.Object>, IEnumerator<TagLib.Asf.Object>, IDisposable, IEnumerator, IEnumerable
        {
            internal TagLib.Asf.Object $current;
            internal int $PC;
            internal uint <$>count;
            internal long <$>position;
            internal TagLib.Asf.File <>f__this;
            internal int <i>__0;
            internal TagLib.Asf.Object <obj>__1;
            internal uint count;
            internal long position;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<i>__0 = 0;
                        break;

                    case 1:
                        this.<i>__0++;
                        break;

                    default:
                        goto Label_009A;
                }
                if (this.<i>__0 < this.count)
                {
                    this.<obj>__1 = this.<>f__this.ReadObject(this.position);
                    this.position += (long) this.<obj>__1.OriginalSize;
                    this.$current = this.<obj>__1;
                    this.$PC = 1;
                    return true;
                }
                this.$PC = -1;
            Label_009A:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<TagLib.Asf.Object> IEnumerable<TagLib.Asf.Object>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new TagLib.Asf.File.<ReadObjects>c__Iterator3 { <>f__this = this.<>f__this, count = this.<$>count, position = this.<$>position };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TagLib.Asf.Object>.GetEnumerator();
            }

            TagLib.Asf.Object IEnumerator<TagLib.Asf.Object>.Current
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

