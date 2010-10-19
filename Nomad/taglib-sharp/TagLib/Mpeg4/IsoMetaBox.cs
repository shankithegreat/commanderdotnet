namespace TagLib.Mpeg4
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class IsoMetaBox : FullBox
    {
        private IEnumerable<Box> children;

        public IsoMetaBox(ByteVector handlerType, string handlerName) : base("meta", 0, 0)
        {
            if (handlerType == null)
            {
                throw new ArgumentNullException("handlerType");
            }
            if (handlerType.Count < 4)
            {
                throw new ArgumentException("The handler type must be four bytes long.", "handlerType");
            }
            this.children = new List<Box>();
            base.AddChild(new IsoHandlerBox(handlerType, handlerName));
        }

        public IsoMetaBox(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, file, handler)
        {
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

