namespace TagLib.Asf
{
    using System;
    using TagLib;

    public class PaddingObject : TagLib.Asf.Object
    {
        private ulong size;

        public PaddingObject(uint size) : base(TagLib.Asf.Guid.AsfPaddingObject)
        {
            this.size = size;
        }

        public PaddingObject(TagLib.Asf.File file, long position) : base(file, position)
        {
            if (!base.Guid.Equals(TagLib.Asf.Guid.AsfPaddingObject))
            {
                throw new CorruptFileException("Object GUID incorrect.");
            }
            if (base.OriginalSize < 0x18L)
            {
                throw new CorruptFileException("Object size too small.");
            }
            this.size = base.OriginalSize;
        }

        public override ByteVector Render()
        {
            return base.Render(new ByteVector((int) (this.size - ((ulong) 0x18L))));
        }

        public ulong Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }
    }
}

