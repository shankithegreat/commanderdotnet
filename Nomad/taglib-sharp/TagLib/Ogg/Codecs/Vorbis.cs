namespace TagLib.Ogg.Codecs
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;
    using TagLib.Ogg;

    public class Vorbis : Codec, ICodec, IAudioCodec
    {
        private ByteVector comment_data;
        private HeaderPacket header;
        private static ByteVector id = "vorbis";

        private Vorbis()
        {
        }

        public static Codec FromPacket(ByteVector packet)
        {
            return ((PacketType(packet) != 1) ? null : ((Codec) new Vorbis()));
        }

        public override TimeSpan GetDuration(long firstGranularPosition, long lastGranularPosition)
        {
            return ((this.header.sample_rate != 0) ? TimeSpan.FromSeconds(((double) (lastGranularPosition - firstGranularPosition)) / ((double) this.header.sample_rate)) : TimeSpan.Zero);
        }

        private static int PacketType(ByteVector packet)
        {
            if (packet.Count <= id.Count)
            {
                return -1;
            }
            for (int i = 0; i < id.Count; i++)
            {
                if (packet[i + 1] != id[i])
                {
                    return -1;
                }
            }
            return packet[0];
        }

        public override bool ReadPacket(ByteVector packet, int index)
        {
            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", "index must be at least zero.");
            }
            int num = PacketType(packet);
            if ((num != 1) && (index == 0))
            {
                throw new CorruptFileException("Stream does not begin with vorbis header.");
            }
            if (this.comment_data == null)
            {
                switch (num)
                {
                    case 1:
                        this.header = new HeaderPacket(packet);
                        goto Label_0091;

                    case 3:
                        this.comment_data = packet.Mid(7);
                        goto Label_0091;
                }
                return true;
            }
        Label_0091:
            return (this.comment_data != null);
        }

        public override void SetCommentPacket(ByteVectorCollection packets, XiphComment comment)
        {
            if (packets == null)
            {
                throw new ArgumentNullException("packets");
            }
            if (comment == null)
            {
                throw new ArgumentNullException("comment");
            }
            byte[] data = new byte[] { 3 };
            ByteVector item = new ByteVector(data) {
                id,
                comment.Render(1)
            };
            if ((packets.Count > 1) && (PacketType(packets[1]) == 3))
            {
                packets[1] = item;
            }
            else
            {
                packets.Insert(1, item);
            }
        }

        public int AudioBitrate
        {
            get
            {
                return (int) ((((float) this.header.bitrate_nominal) / 1000f) + 0.5);
            }
        }

        public int AudioChannels
        {
            get
            {
                return (int) this.header.channels;
            }
        }

        public int AudioSampleRate
        {
            get
            {
                return (int) this.header.sample_rate;
            }
        }

        public override ByteVector CommentData
        {
            get
            {
                return this.comment_data;
            }
        }

        public override string Description
        {
            get
            {
                return string.Format("Vorbis Version {0} Audio", this.header.vorbis_version);
            }
        }

        public override TagLib.MediaTypes MediaTypes
        {
            get
            {
                return TagLib.MediaTypes.Audio;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HeaderPacket
        {
            public uint sample_rate;
            public uint channels;
            public uint vorbis_version;
            public uint bitrate_maximum;
            public uint bitrate_nominal;
            public uint bitrate_minimum;
            public HeaderPacket(ByteVector data)
            {
                this.vorbis_version = data.Mid(7, 4).ToUInt(false);
                this.channels = data[11];
                this.sample_rate = data.Mid(12, 4).ToUInt(false);
                this.bitrate_maximum = data.Mid(0x10, 4).ToUInt(false);
                this.bitrate_nominal = data.Mid(20, 4).ToUInt(false);
                this.bitrate_minimum = data.Mid(0x18, 4).ToUInt(false);
            }
        }
    }
}

