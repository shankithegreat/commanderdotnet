namespace TagLib.WavPack
{
    using System;
    using TagLib;
    using TagLib.NonContainer;

    [SupportedMimeType("taglib/wv", "wv"), SupportedMimeType("audio/x-wavpack")]
    public class File : TagLib.NonContainer.File
    {
        private ByteVector header_block;

        public File(string path) : base(path)
        {
        }

        public File(TagLib.File.IFileAbstraction abstraction) : base(abstraction)
        {
        }

        public File(string path, ReadStyle propertiesStyle) : base(path, propertiesStyle)
        {
        }

        public File(TagLib.File.IFileAbstraction abstraction, ReadStyle propertiesStyle) : base(abstraction, propertiesStyle)
        {
        }

        public override TagLib.Tag GetTag(TagTypes type, bool create)
        {
            TagLib.Tag tag = (this.Tag as TagLib.NonContainer.Tag).GetTag(type);
            if ((tag != null) || !create)
            {
                return tag;
            }
            switch (type)
            {
                case (TagTypes.None | TagTypes.Id3v1):
                    return base.EndTag.AddTag(type, this.Tag);

                case (TagTypes.None | TagTypes.Id3v2):
                    return base.StartTag.AddTag(type, this.Tag);

                case (TagTypes.None | TagTypes.Ape):
                    return base.EndTag.AddTag(type, this.Tag);
            }
            return null;
        }

        protected override void ReadEnd(long end, ReadStyle propertiesStyle)
        {
            this.GetTag(TagTypes.None | TagTypes.Ape, true);
        }

        protected override Properties ReadProperties(long start, long end, ReadStyle propertiesStyle)
        {
            StreamHeader header = new StreamHeader(this.header_block, end - start);
            return new Properties(TimeSpan.Zero, new ICodec[] { header });
        }

        protected override void ReadStart(long start, ReadStyle propertiesStyle)
        {
            if ((this.header_block == null) || (propertiesStyle != ReadStyle.None))
            {
                base.Seek(start);
                this.header_block = base.ReadBlock(0x20);
            }
        }
    }
}

