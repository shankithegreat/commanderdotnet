namespace TagLib.Riff
{
    using System;
    using TagLib;

    public abstract class AviStream
    {
        private ICodec codec;
        private AviStreamHeader header;

        protected AviStream(AviStreamHeader header)
        {
            this.header = header;
        }

        public virtual void ParseItem(ByteVector id, ByteVector data, int start, int length)
        {
        }

        public static AviStream ParseStreamList(ByteVector data)
        {
            int num2;
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (!data.StartsWith("strl"))
            {
                return null;
            }
            AviStream stream = null;
            for (int i = 4; (i + 8) < data.Count; i += num2 + 8)
            {
                ByteVector id = data.Mid(i, 4);
                num2 = (int) data.Mid(i + 4, 4).ToUInt(false);
                if ((id == "strh") && (stream == null))
                {
                    AviStreamHeader header = new AviStreamHeader(data, i + 8);
                    if (header.Type == "vids")
                    {
                        stream = new AviVideoStream(header);
                    }
                    else if (header.Type == "auds")
                    {
                        stream = new AviAudioStream(header);
                    }
                }
                else if (stream != null)
                {
                    stream.ParseItem(id, data, i + 8, num2);
                }
            }
            return stream;
        }

        public ICodec Codec
        {
            get
            {
                return this.codec;
            }
            protected set
            {
                this.codec = value;
            }
        }

        public AviStreamHeader Header
        {
            get
            {
                return this.header;
            }
        }
    }
}

