namespace TagLib.Mpeg4
{
    using System;
    using TagLib;

    public class IsoFreeSpaceBox : Box
    {
        private long padding;

        public IsoFreeSpaceBox(long padding) : base("free")
        {
            this.PaddingSize = padding;
        }

        public IsoFreeSpaceBox(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, handler)
        {
            this.padding = base.DataSize;
        }

        public override ByteVector Data
        {
            get
            {
                return new ByteVector((int) this.padding);
            }
            set
            {
                this.padding = (value == null) ? ((long) 0) : ((long) value.Count);
            }
        }

        public long PaddingSize
        {
            get
            {
                return (this.padding + 8L);
            }
            set
            {
                this.padding = value - 8L;
            }
        }
    }
}

