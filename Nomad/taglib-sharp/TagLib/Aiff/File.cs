namespace TagLib.Aiff
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;
    using TagLib.Id3v2;

    [SupportedMimeType("taglib/aif", "aif"), SupportedMimeType("audio/x-aiff"), SupportedMimeType("audio/aiff"), SupportedMimeType("sound/aiff"), SupportedMimeType("application/x-aiff")]
    public class File : TagLib.File
    {
        public static readonly ReadOnlyByteVector CommIdentifier = "COMM";
        public static readonly ReadOnlyByteVector FileIdentifier = "FORM";
        private ByteVector header_block;
        public static readonly ReadOnlyByteVector ID3Identifier = "ID3 ";
        private TagLib.Properties properties;
        public static readonly ReadOnlyByteVector SoundIdentifier = "SSND";
        private TagLib.Id3v2.Tag tag;

        public File(string path) : this(path, ReadStyle.Average)
        {
        }

        public File(TagLib.File.IFileAbstraction abstraction) : this(abstraction, ReadStyle.Average)
        {
        }

        public File(string path, ReadStyle propertiesStyle) : this(new TagLib.File.LocalFileAbstraction(path), propertiesStyle)
        {
        }

        public File(TagLib.File.IFileAbstraction abstraction, ReadStyle propertiesStyle) : base(abstraction)
        {
            base.Mode = TagLib.File.AccessMode.Read;
            try
            {
                uint num;
                long num2;
                long num3;
                this.Read(true, propertiesStyle, out num, out num2, out num3);
            }
            finally
            {
                base.Mode = TagLib.File.AccessMode.Closed;
            }
            base.TagTypesOnDisk = base.TagTypes;
            this.GetTag(TagTypes.None | TagTypes.Id3v2, true);
        }

        public override TagLib.Tag GetTag(TagTypes type, bool create)
        {
            TagLib.Tag tag = null;
            if (type != (TagTypes.None | TagTypes.Id3v2))
            {
                return tag;
            }
            if ((this.tag == null) && create)
            {
                this.tag = new TagLib.Id3v2.Tag();
                this.tag.Version = 2;
            }
            return this.tag;
        }

        private void Read(bool read_tags, ReadStyle style, out uint aiff_size, out long tag_start, out long tag_end)
        {
            base.Seek(0L);
            if (base.ReadBlock(4) != FileIdentifier)
            {
                throw new CorruptFileException("File does not begin with AIFF identifier");
            }
            aiff_size = base.ReadBlock(4).ToUInt(true);
            tag_start = -1L;
            tag_end = -1L;
            if ((this.header_block == null) && (style != ReadStyle.None))
            {
                long offset = base.Find(CommIdentifier, 0L);
                if (offset == -1L)
                {
                    throw new CorruptFileException("No Common chunk available in AIFF file.");
                }
                base.Seek(offset);
                this.header_block = base.ReadBlock(0x1a);
                StreamHeader header = new StreamHeader(this.header_block, (long) ((ulong) aiff_size));
                ICodec[] codecs = new ICodec[] { header };
                this.properties = new TagLib.Properties(TimeSpan.Zero, codecs);
            }
            long num2 = -1L;
            if (base.Find(SoundIdentifier, 0L, ID3Identifier) == -1L)
            {
                num2 = base.Find(ID3Identifier, 0L);
            }
            long num3 = base.Find(SoundIdentifier, 0L);
            if (num3 == -1L)
            {
                throw new CorruptFileException("No Sound chunk available in AIFF file.");
            }
            base.Seek(num3 + 4L);
            long startPosition = (((long) base.ReadBlock(4).ToULong(true)) + num3) + 4L;
            if (num2 == -1L)
            {
                num2 = base.Find(ID3Identifier, startPosition);
            }
            if (num2 > -1L)
            {
                if (read_tags && (this.tag == null))
                {
                    this.tag = new TagLib.Id3v2.Tag(this, num2 + 8L);
                }
                base.Seek(num2 + 4L);
                uint num6 = base.ReadBlock(4).ToUInt(true) + 8;
                long num7 = num2;
                base.InvariantStartPosition = num7;
                tag_start = num7;
                num7 = tag_start + num6;
                base.InvariantEndPosition = num7;
                tag_end = num7;
            }
        }

        public override void RemoveTags(TagTypes types)
        {
            if ((types == (TagTypes.None | TagTypes.Id3v2)) || (types == ~TagTypes.None))
            {
                this.tag = null;
            }
        }

        public override void Save()
        {
            base.Mode = TagLib.File.AccessMode.Write;
            try
            {
                uint num;
                long num2;
                long num3;
                ByteVector data = new ByteVector();
                if (this.tag != null)
                {
                    ByteVector vector2 = this.tag.Render();
                    if (vector2.Count > 10)
                    {
                        if ((vector2.Count % 2) == 1)
                        {
                            vector2.Add((byte) 0);
                        }
                        data.Add("ID3 ");
                        data.Add(ByteVector.FromUInt((uint) vector2.Count, true));
                        data.Add(vector2);
                    }
                }
                this.Read(false, ReadStyle.None, out num, out num2, out num3);
                if ((num2 < 12L) || (num3 < num2))
                {
                    num2 = num3 = base.Length;
                }
                int num4 = (int) ((num3 - num2) + 8L);
                base.Insert(data, num2, (long) num4);
                if (((data.Count - num4) != 0) && (num2 <= num))
                {
                    if (this.tag == null)
                    {
                        num4 -= 0x10;
                    }
                    else
                    {
                        num4 -= 8;
                    }
                    base.Insert(ByteVector.FromUInt((uint) ((num + data.Count) - num4), true), 4L, 4L);
                }
                base.TagTypesOnDisk = base.TagTypes;
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
                return this.tag;
            }
        }
    }
}

