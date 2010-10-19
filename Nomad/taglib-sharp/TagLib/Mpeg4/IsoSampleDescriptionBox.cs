namespace TagLib.Mpeg4
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class IsoSampleDescriptionBox : FullBox
    {
        private IEnumerable<Box> children;
        private uint entry_count;

        public IsoSampleDescriptionBox(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, file, handler)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            this.entry_count = file.ReadBlock(4).ToUInt();
            this.children = base.LoadChildren(file);
        }

        public override IEnumerable<Box> Children
        {
            get
            {
                return this.children;
            }
        }

        protected override long DataPosition
        {
            get
            {
                return (base.DataPosition + 4L);
            }
        }

        public uint EntryCount
        {
            get
            {
                return this.entry_count;
            }
        }
    }
}

