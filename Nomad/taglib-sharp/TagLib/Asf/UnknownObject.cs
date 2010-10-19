namespace TagLib.Asf
{
    using System;
    using TagLib;

    public class UnknownObject : TagLib.Asf.Object
    {
        private ByteVector data;

        public UnknownObject(TagLib.Asf.File file, long position) : base(file, position)
        {
            this.data = file.ReadBlock((int) (base.OriginalSize - ((ulong) 0x18L)));
        }

        public override ByteVector Render()
        {
            return base.Render(this.data);
        }

        public ByteVector Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }
    }
}

