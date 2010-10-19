namespace TagLib.NonContainer
{
    using System;
    using TagLib;

    public abstract class File : TagLib.File
    {
        private TagLib.Properties properties;
        private TagLib.NonContainer.Tag tag;

        protected File(string path) : this(path, ReadStyle.Average)
        {
        }

        protected File(TagLib.File.IFileAbstraction abstraction) : this(abstraction, ReadStyle.Average)
        {
        }

        protected File(string path, ReadStyle propertiesStyle) : base(path)
        {
            this.Read(propertiesStyle);
        }

        protected File(TagLib.File.IFileAbstraction abstraction, ReadStyle propertiesStyle) : base(abstraction)
        {
            this.Read(propertiesStyle);
        }

        private void Read(ReadStyle propertiesStyle)
        {
            base.Mode = TagLib.File.AccessMode.Read;
            try
            {
                this.tag = new TagLib.NonContainer.Tag(this);
                base.InvariantStartPosition = this.tag.ReadStart();
                base.TagTypesOnDisk |= this.StartTag.TagTypes;
                this.ReadStart(base.InvariantStartPosition, propertiesStyle);
                base.InvariantEndPosition = (base.InvariantStartPosition != base.Length) ? this.tag.ReadEnd() : base.Length;
                base.TagTypesOnDisk |= this.EndTag.TagTypes;
                this.ReadEnd(base.InvariantEndPosition, propertiesStyle);
                this.properties = (propertiesStyle == ReadStyle.None) ? null : this.ReadProperties(base.InvariantStartPosition, base.InvariantEndPosition, propertiesStyle);
            }
            finally
            {
                base.Mode = TagLib.File.AccessMode.Closed;
            }
        }

        protected virtual void ReadEnd(long end, ReadStyle propertiesStyle)
        {
        }

        protected abstract TagLib.Properties ReadProperties(long start, long end, ReadStyle propertiesStyle);
        protected virtual void ReadStart(long start, ReadStyle propertiesStyle)
        {
        }

        public override void RemoveTags(TagTypes types)
        {
            this.tag.RemoveTags(types);
        }

        public override void Save()
        {
            base.Mode = TagLib.File.AccessMode.Write;
            try
            {
                long num;
                long num2;
                this.tag.Write(out num, out num2);
                base.InvariantStartPosition = num;
                base.InvariantEndPosition = num2;
                base.TagTypesOnDisk = base.TagTypes;
            }
            finally
            {
                base.Mode = TagLib.File.AccessMode.Closed;
            }
        }

        protected TagLib.NonContainer.EndTag EndTag
        {
            get
            {
                return this.tag.EndTag;
            }
        }

        public override TagLib.Properties Properties
        {
            get
            {
                return this.properties;
            }
        }

        protected TagLib.NonContainer.StartTag StartTag
        {
            get
            {
                return this.tag.StartTag;
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

