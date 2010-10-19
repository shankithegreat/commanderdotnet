namespace TagLib.Ogg.Codecs
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;
    using TagLib.Ogg;

    public class Theora : Codec, ICodec, IVideoCodec
    {
        private ByteVector comment_data;
        private HeaderPacket header;
        private static ByteVector id = "theora";

        private Theora()
        {
        }

        public static Codec FromPacket(ByteVector packet)
        {
            return ((PacketType(packet) != 0x80) ? null : ((Codec) new Theora()));
        }

        public override TimeSpan GetDuration(long firstGranularPosition, long lastGranularPosition)
        {
            return TimeSpan.FromSeconds(this.header.GranuleTime(lastGranularPosition) - this.header.GranuleTime(firstGranularPosition));
        }

        private static int PacketType(ByteVector packet)
        {
            if ((packet.Count <= id.Count) || (packet[0] < 0x80))
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
            if ((num != 0x80) && (index == 0))
            {
                throw new CorruptFileException("Stream does not begin with theora header.");
            }
            if (this.comment_data == null)
            {
                switch (num)
                {
                    case 0x80:
                        this.header = new HeaderPacket(packet);
                        goto Label_009D;

                    case 0x81:
                        this.comment_data = packet.Mid(7);
                        goto Label_009D;
                }
                return true;
            }
        Label_009D:
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
            byte[] data = new byte[] { 0x81 };
            ByteVector item = new ByteVector(data) {
                id,
                comment.Render(1)
            };
            if ((packets.Count > 1) && (PacketType(packets[1]) == 0x81))
            {
                packets[1] = item;
            }
            else
            {
                packets.Insert(1, item);
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
                return string.Format("Theora Version {0}.{1} Video", this.header.major_version, this.header.minor_version);
            }
        }

        public override TagLib.MediaTypes MediaTypes
        {
            get
            {
                return TagLib.MediaTypes.Video;
            }
        }

        public int VideoHeight
        {
            get
            {
                return this.header.height;
            }
        }

        public int VideoWidth
        {
            get
            {
                return this.header.width;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HeaderPacket
        {
            public byte major_version;
            public byte minor_version;
            public byte revision_version;
            public int width;
            public int height;
            public int fps_numerator;
            public int fps_denominator;
            public int keyframe_granule_shift;
            public HeaderPacket(ByteVector data)
            {
                this.major_version = data[7];
                this.minor_version = data[8];
                this.revision_version = data[9];
                this.width = (int) data.Mid(14, 3).ToUInt();
                this.height = (int) data.Mid(0x11, 3).ToUInt();
                this.fps_numerator = (int) data.Mid(0x16, 4).ToUInt();
                this.fps_denominator = (int) data.Mid(0x1a, 4).ToUInt();
                ushort num = data.Mid(40, 2).ToUShort();
                this.keyframe_granule_shift = (num >> 5) & 0x1f;
            }

            public double GranuleTime(long granularPosition)
            {
                long num = granularPosition >> this.keyframe_granule_shift;
                long num2 = granularPosition - (num << this.keyframe_granule_shift);
                return ((num + num2) * (((double) this.fps_denominator) / ((double) this.fps_numerator)));
            }
        }
    }
}

