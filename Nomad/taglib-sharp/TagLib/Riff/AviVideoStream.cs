namespace TagLib.Riff
{
    using System;
    using TagLib;

    public class AviVideoStream : AviStream
    {
        public AviVideoStream(AviStreamHeader header) : base(header)
        {
        }

        public override void ParseItem(ByteVector id, ByteVector data, int start, int length)
        {
            if (id == "strf")
            {
                base.Codec = new BitmapInfoHeader(data, start);
            }
        }
    }
}

