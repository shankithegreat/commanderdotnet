namespace TagLib.Mpeg4
{
    using System;
    using TagLib;

    public class AppleElementaryStreamDescriptor : FullBox
    {
        private uint average_bitrate;
        private uint buffer_size_db;
        private ByteVector decoder_config;
        private ushort es_id;
        private uint max_bitrate;
        private byte object_type_id;
        private byte stream_priority;
        private byte stream_type;

        public AppleElementaryStreamDescriptor(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, file, handler)
        {
            int offset = 0;
            ByteVector data = file.ReadBlock(base.DataSize);
            this.decoder_config = new ByteVector();
            if (data[offset++] == 3)
            {
                if (ReadLength(data, ref offset) < 20)
                {
                    throw new CorruptFileException("Insufficient data present.");
                }
                this.es_id = data.Mid(offset, 2).ToUShort();
                offset += 2;
                this.stream_priority = data[offset++];
            }
            else
            {
                this.es_id = data.Mid(offset, 2).ToUShort();
                offset += 2;
            }
            if (data[offset++] != 4)
            {
                throw new CorruptFileException("Could not identify decoder configuration descriptor.");
            }
            if (ReadLength(data, ref offset) < 15)
            {
                throw new CorruptFileException("Could not read data. Too small.");
            }
            this.object_type_id = data[offset++];
            this.stream_type = data[offset++];
            this.buffer_size_db = data.Mid(offset, 3).ToUInt();
            offset += 3;
            this.max_bitrate = data.Mid(offset, 4).ToUInt();
            offset += 4;
            this.average_bitrate = data.Mid(offset, 4).ToUInt();
            offset += 4;
            if (data[offset++] != 5)
            {
                throw new CorruptFileException("Could not identify decoder specific descriptor.");
            }
            uint num2 = ReadLength(data, ref offset);
            this.decoder_config = data.Mid(offset, (int) num2);
        }

        private static uint ReadLength(ByteVector data, ref int offset)
        {
            byte num;
            int num2 = offset + 4;
            uint num3 = 0;
            do
            {
                num = data[offset++];
                num3 = (num3 << 7) | ((uint) (num & 0x7f));
            }
            while (((num & 0x80) != 0) && (offset <= num2));
            return num3;
        }

        public uint AverageBitrate
        {
            get
            {
                return (this.average_bitrate / 0x3e8);
            }
        }

        public uint BufferSizeDB
        {
            get
            {
                return this.buffer_size_db;
            }
        }

        public ByteVector DecoderConfig
        {
            get
            {
                return this.decoder_config;
            }
        }

        public uint MaximumBitrate
        {
            get
            {
                return (this.max_bitrate / 0x3e8);
            }
        }

        public byte ObjectTypeId
        {
            get
            {
                return this.object_type_id;
            }
        }

        public ushort StreamId
        {
            get
            {
                return this.es_id;
            }
        }

        public byte StreamPriority
        {
            get
            {
                return this.stream_priority;
            }
        }

        public byte StreamType
        {
            get
            {
                return this.stream_type;
            }
        }
    }
}

