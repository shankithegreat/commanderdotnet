namespace TagLib.Mpeg4
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class IsoUserDataBox : Box
    {
        private IEnumerable<Box> children;

        public IsoUserDataBox() : base("udta")
        {
            this.children = new List<Box>();
        }

        public IsoUserDataBox(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, handler)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            this.children = base.LoadChildren(file);
        }

        public override IEnumerable<Box> Children
        {
            get
            {
                return this.children;
            }
        }
    }
}

