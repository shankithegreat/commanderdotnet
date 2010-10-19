namespace TagLib.Aac
{
    using System;
    using TagLib;
    using TagLib.NonContainer;

    [SupportedMimeType("taglib/aac", "aac"), SupportedMimeType("audio/aac")]
    public class File : TagLib.NonContainer.File
    {
        private AudioHeader first_header;

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
            this.GetTag(TagTypes.None | TagTypes.Id3v1, true);
            this.GetTag(TagTypes.None | TagTypes.Id3v2, true);
        }

        protected override Properties ReadProperties(long start, long end, ReadStyle propertiesStyle)
        {
            this.first_header.SetStreamLength(end - start);
            return new Properties(TimeSpan.Zero, new ICodec[] { this.first_header });
        }

        protected override void ReadStart(long start, ReadStyle propertiesStyle)
        {
            if ((propertiesStyle != ReadStyle.None) && !AudioHeader.Find(out this.first_header, this, start, 0x4000))
            {
                throw new CorruptFileException("ADTS audio header not found.");
            }
        }
    }
}

