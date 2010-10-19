namespace TagLib.Ogg
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct PageHeader
    {
        private List<int> packet_sizes;
        private byte version;
        private PageFlags flags;
        private ulong absolute_granular_position;
        private uint stream_serial_number;
        private uint page_sequence_number;
        private uint size;
        private uint data_size;
        public PageHeader(uint streamSerialNumber, uint pageNumber, PageFlags flags)
        {
            this.version = 0;
            this.flags = flags;
            this.absolute_granular_position = 0L;
            this.stream_serial_number = streamSerialNumber;
            this.page_sequence_number = pageNumber;
            this.size = 0;
            this.data_size = 0;
            this.packet_sizes = new List<int>();
            if ((pageNumber == 0) && (((byte) (flags & PageFlags.FirstPacketContinued)) == 0))
            {
                this.flags = (PageFlags) ((byte) (this.flags | PageFlags.FirstPageOfStream));
            }
        }

        public PageHeader(TagLib.Ogg.File file, long position)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if ((position < 0L) || (position > (file.Length - 0x1bL)))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            file.Seek(position);
            ByteVector vector = file.ReadBlock(0x1b);
            if ((vector.Count < 0x1b) || !vector.StartsWith("OggS"))
            {
                throw new CorruptFileException("Error reading page header");
            }
            this.version = vector[4];
            this.flags = (PageFlags) vector[5];
            this.absolute_granular_position = vector.Mid(6, 8).ToULong(false);
            this.stream_serial_number = vector.Mid(14, 4).ToUInt(false);
            this.page_sequence_number = vector.Mid(0x12, 4).ToUInt(false);
            int length = vector[0x1a];
            ByteVector vector2 = file.ReadBlock(length);
            if ((length < 1) || (vector2.Count != length))
            {
                throw new CorruptFileException("Incorrect number of page segments");
            }
            this.size = (uint) (0x1b + length);
            this.packet_sizes = new List<int>();
            int item = 0;
            this.data_size = 0;
            for (int i = 0; i < length; i++)
            {
                this.data_size += vector2[i];
                item += vector2[i];
                if (vector2[i] < 0xff)
                {
                    this.packet_sizes.Add(item);
                    item = 0;
                }
            }
            if (item > 0)
            {
                this.packet_sizes.Add(item);
            }
        }

        public PageHeader(PageHeader original, uint offset, PageFlags flags)
        {
            this.version = original.version;
            this.flags = flags;
            this.absolute_granular_position = original.absolute_granular_position;
            this.stream_serial_number = original.stream_serial_number;
            this.page_sequence_number = original.page_sequence_number + offset;
            this.size = original.size;
            this.data_size = original.data_size;
            this.packet_sizes = new List<int>();
            if ((this.page_sequence_number == 0) && (((byte) (flags & PageFlags.FirstPacketContinued)) == 0))
            {
                this.flags = (PageFlags) ((byte) (this.flags | PageFlags.FirstPageOfStream));
            }
        }

        public int[] PacketSizes
        {
            get
            {
                return this.packet_sizes.ToArray();
            }
            set
            {
                this.packet_sizes.Clear();
                this.packet_sizes.AddRange(value);
            }
        }
        public PageFlags Flags
        {
            get
            {
                return this.flags;
            }
        }
        public long AbsoluteGranularPosition
        {
            get
            {
                return (long) this.absolute_granular_position;
            }
        }
        public uint PageSequenceNumber
        {
            get
            {
                return this.page_sequence_number;
            }
        }
        public uint StreamSerialNumber
        {
            get
            {
                return this.stream_serial_number;
            }
        }
        public uint Size
        {
            get
            {
                return this.size;
            }
        }
        public uint DataSize
        {
            get
            {
                return this.data_size;
            }
        }
        public ByteVector Render()
        {
            ByteVector vector = new ByteVector {
                "OggS",
                this.version,
                this.flags,
                ByteVector.FromULong(this.absolute_granular_position, 0),
                ByteVector.FromUInt(this.stream_serial_number, 0),
                ByteVector.FromUInt(this.page_sequence_number, 0),
                new ByteVector(4, 0)
            };
            ByteVector lacingValues = this.LacingValues;
            vector.Add((byte) lacingValues.Count);
            vector.Add(lacingValues);
            return vector;
        }

        private ByteVector LacingValues
        {
            get
            {
                ByteVector vector = new ByteVector();
                int[] packetSizes = this.PacketSizes;
                for (int i = 0; i < packetSizes.Length; i++)
                {
                    int num2 = packetSizes[i] / 0xff;
                    int num3 = packetSizes[i] % 0xff;
                    for (int j = 0; j < num2; j++)
                    {
                        vector.Add((byte) 0xff);
                    }
                    if ((i < (packetSizes.Length - 1)) || ((this.packet_sizes[i] % 0xff) != 0))
                    {
                        vector.Add((byte) num3);
                    }
                }
                return vector;
            }
        }
        public override int GetHashCode()
        {
            return (int) (((((((this.LacingValues.GetHashCode() ^ this.version) ^ ((int) this.flags)) ^ ((int) this.absolute_granular_position)) ^ this.stream_serial_number) ^ this.page_sequence_number) ^ this.size) ^ this.data_size);
        }

        public override bool Equals(object other)
        {
            return ((other is PageHeader) && this.Equals((PageHeader) other));
        }

        public bool Equals(PageHeader other)
        {
            return (((((this.packet_sizes == other.packet_sizes) && (this.version == other.version)) && ((this.flags == other.flags) && (this.absolute_granular_position == other.absolute_granular_position))) && (((this.stream_serial_number == other.stream_serial_number) && (this.page_sequence_number == other.page_sequence_number)) && (this.size == other.size))) && (this.data_size == other.data_size));
        }

        public static bool operator ==(PageHeader first, PageHeader second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(PageHeader first, PageHeader second)
        {
            return !first.Equals(second);
        }
    }
}

