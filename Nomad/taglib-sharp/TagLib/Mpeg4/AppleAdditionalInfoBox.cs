namespace TagLib.Mpeg4
{
    using System;
    using TagLib;

    public class AppleAdditionalInfoBox : FullBox
    {
        private ByteVector data;

        public AppleAdditionalInfoBox(ByteVector header, byte version, uint flags) : base(header, version, flags)
        {
        }

        public AppleAdditionalInfoBox(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, file, handler)
        {
            this.Data = file.ReadBlock(base.DataSize);
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

        public string Text
        {
            get
            {
                return this.Data.ToString(StringType.Latin1);
            }
            set
            {
                this.Data = ByteVector.FromString(value, StringType.Latin1);
            }
        }
    }
}

