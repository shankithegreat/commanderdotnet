namespace TagLib.Ogg
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using TagLib;
    using TagLib.Ogg.Codecs;

    public abstract class Codec : ICodec
    {
        private static List<CodecProvider> providers = new List<CodecProvider>();

        protected Codec()
        {
        }

        public static void AddCodecProvider(CodecProvider provider)
        {
            providers.Insert(0, provider);
        }

        public static Codec GetCodec(ByteVector packet)
        {
            Codec codec = null;
            foreach (CodecProvider provider in providers)
            {
                codec = provider(packet);
                if (codec != null)
                {
                    return codec;
                }
            }
            codec = Vorbis.FromPacket(packet);
            if (codec == null)
            {
                codec = Theora.FromPacket(packet);
                if (codec == null)
                {
                    throw new UnsupportedFormatException("Unknown codec.");
                }
            }
            return codec;
        }

        public abstract TimeSpan GetDuration(long firstGranularPosition, long lastGranularPosition);
        public abstract bool ReadPacket(ByteVector packet, int index);
        public abstract void SetCommentPacket(ByteVectorCollection packets, XiphComment comment);

        public abstract ByteVector CommentData { get; }

        public abstract string Description { get; }

        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.Zero;
            }
        }

        public abstract TagLib.MediaTypes MediaTypes { get; }

        public delegate Codec CodecProvider(ByteVector packet);
    }
}

