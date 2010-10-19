namespace TagLib.Mpeg4
{
    using System;
    using TagLib;

    public abstract class FullBox : Box
    {
        private uint flags;
        private byte version;

        protected FullBox(ByteVector type, byte version, uint flags) : this(new BoxHeader(type), version, flags)
        {
        }

        protected FullBox(BoxHeader header, byte version, uint flags) : base(header)
        {
            this.version = version;
            this.flags = flags;
        }

        protected FullBox(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, handler)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            file.Seek(base.DataPosition);
            ByteVector vector = file.ReadBlock(4);
            this.version = vector[0];
            this.flags = vector.Mid(1, 3).ToUInt();
        }

        protected override ByteVector Render(ByteVector topData)
        {
            byte[] data = new byte[] { this.version };
            ByteVector vector = new ByteVector(data) {
                ByteVector.FromUInt(this.flags).Mid(1, 3),
                topData
            };
            return base.Render(vector);
        }

        protected override long DataPosition
        {
            get
            {
                return (base.DataPosition + 4L);
            }
        }

        public uint Flags
        {
            get
            {
                return this.flags;
            }
            set
            {
                this.flags = value;
            }
        }

        public uint Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = (byte) value;
            }
        }
    }
}

