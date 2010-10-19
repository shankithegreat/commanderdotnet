namespace TagLib.Id3v2
{
    using System;
    using TagLib;

    public class ExtendedHeader : ICloneable
    {
        private uint size;

        public ExtendedHeader()
        {
        }

        public ExtendedHeader(ByteVector data, byte version)
        {
            this.Parse(data, version);
        }

        public ExtendedHeader Clone()
        {
            return new ExtendedHeader { size = this.size };
        }

        protected void Parse(ByteVector data, byte version)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.size = ((version != 3) ? 0 : 4) + SynchData.ToUInt(data.Mid(0, 4));
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public uint Size
        {
            get
            {
                return this.size;
            }
        }
    }
}

