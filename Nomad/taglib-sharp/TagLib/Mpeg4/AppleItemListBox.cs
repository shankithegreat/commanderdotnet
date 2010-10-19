namespace TagLib.Mpeg4
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class AppleItemListBox : Box
    {
        private IEnumerable<Box> children;

        public AppleItemListBox() : base("ilst")
        {
            this.children = new List<Box>();
        }

        public AppleItemListBox(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, handler)
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

