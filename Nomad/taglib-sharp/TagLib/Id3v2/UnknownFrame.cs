namespace TagLib.Id3v2
{
    using System;
    using TagLib;

    public class UnknownFrame : Frame
    {
        private ByteVector field_data;

        public UnknownFrame(ByteVector type) : this(type, (ByteVector) null)
        {
        }

        public UnknownFrame(ByteVector data, byte version) : base(data, version)
        {
            base.SetData(data, 0, version, true);
        }

        public UnknownFrame(ByteVector type, ByteVector data) : base(type, 4)
        {
            this.field_data = data;
        }

        protected internal UnknownFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(header)
        {
            base.SetData(data, offset, version, false);
        }

        protected override void ParseFields(ByteVector data, byte version)
        {
            this.field_data = data;
        }

        protected override ByteVector RenderFields(byte version)
        {
            // This item is obfuscated and can not be translated.
            if (this.field_data != null)
            {
                goto Label_0012;
            }
            ByteVector vector1 = this.field_data;
            return new ByteVector();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public ByteVector Data
        {
            get
            {
                return this.field_data;
            }
            set
            {
                this.field_data = value;
            }
        }
    }
}

