namespace TagLib.Riff
{
    using System;
    using TagLib;

    public class AviAudioStream : AviStream
    {
        public AviAudioStream(AviStreamHeader header) : base(header)
        {
        }

        public override void ParseItem(ByteVector id, ByteVector data, int start, int length)
        {
            if (id == "strf")
            {
                base.Codec = new WaveFormatEx(data, start);
            }
        }
    }
}

