namespace TagLib.Ogg
{
    using System;
    using TagLib;

    public class Bitstream
    {
        private TagLib.Ogg.Codec codec;
        private long first_absolute_granular_position;
        private int packet_index;
        private ByteVector previous_packet;

        public Bitstream(Page page)
        {
            if (page == null)
            {
                throw new ArgumentNullException("page");
            }
            this.codec = TagLib.Ogg.Codec.GetCodec(page.Packets[0]);
            this.first_absolute_granular_position = page.Header.AbsoluteGranularPosition;
        }

        public TimeSpan GetDuration(long lastAbsoluteGranularPosition)
        {
            return this.codec.GetDuration(this.first_absolute_granular_position, lastAbsoluteGranularPosition);
        }

        private bool ReadPacket(ByteVector packet)
        {
            return this.codec.ReadPacket(packet, this.packet_index++);
        }

        public bool ReadPage(Page page)
        {
            if (page == null)
            {
                throw new ArgumentNullException("page");
            }
            ByteVector[] packets = page.Packets;
            for (int i = 0; i < packets.Length; i++)
            {
                if ((((byte) (page.Header.Flags & PageFlags.FirstPacketContinued)) == 0) && (this.previous_packet != null))
                {
                    if (this.ReadPacket(this.previous_packet))
                    {
                        return true;
                    }
                    this.previous_packet = null;
                }
                ByteVector data = packets[i];
                if (((i == 0) && (((byte) (page.Header.Flags & PageFlags.FirstPacketContinued)) != 0)) && (this.previous_packet != null))
                {
                    this.previous_packet.Add(data);
                    data = this.previous_packet;
                }
                this.previous_packet = null;
                if (i == (packets.Length - 1))
                {
                    this.previous_packet = new ByteVector(data);
                }
                else if (this.ReadPacket(data))
                {
                    return true;
                }
            }
            return false;
        }

        public TagLib.Ogg.Codec Codec
        {
            get
            {
                return this.codec;
            }
        }
    }
}

