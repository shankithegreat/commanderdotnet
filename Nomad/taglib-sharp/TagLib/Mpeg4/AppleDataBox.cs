namespace TagLib.Mpeg4
{
    using System;
    using TagLib;

    public class AppleDataBox : FullBox
    {
        private ByteVector data;

        public AppleDataBox(ByteVector data, uint flags) : base("data", 0, flags)
        {
            this.Data = data;
        }

        public AppleDataBox(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, file, handler)
        {
            this.Data = base.LoadData(file);
        }

        protected override ByteVector Render(ByteVector topData)
        {
            ByteVector vector = new ByteVector(4) {
                topData
            };
            return base.Render(vector);
        }

        public override ByteVector Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = (value == null) ? new ByteVector() : value;
            }
        }

        protected override long DataPosition
        {
            get
            {
                return (base.DataPosition + 4L);
            }
        }

        public string Text
        {
            get
            {
                return (((base.Flags & 1) == 0) ? null : this.Data.ToString(StringType.UTF8));
            }
            set
            {
                base.Flags = 1;
                this.Data = ByteVector.FromString(value, StringType.UTF8);
            }
        }

        public enum FlagType
        {
            ContainsData = 0,
            ContainsJpegData = 13,
            ContainsPngData = 14,
            ContainsText = 1,
            ForTempo = 0x15
        }
    }
}

