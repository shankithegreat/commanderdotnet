namespace TagLib.Riff
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct AviStreamHeader
    {
        private ByteVector type;
        private ByteVector handler;
        private uint flags;
        private uint priority;
        private uint initial_frames;
        private uint scale;
        private uint rate;
        private uint start;
        private uint length;
        private uint suggested_buffer_size;
        private uint quality;
        private uint sample_size;
        private ushort left;
        private ushort top;
        private ushort right;
        private ushort bottom;
        [Obsolete("Use WaveFormatEx(ByteVector,int)")]
        public AviStreamHeader(ByteVector data) : this(data, 0)
        {
        }

        public AviStreamHeader(ByteVector data, int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((offset + 0x38) > data.Count)
            {
                throw new CorruptFileException("Expected 56 bytes.");
            }
            this.type = data.Mid(offset, 4);
            this.handler = data.Mid(offset + 4, 4);
            this.flags = data.Mid(offset + 8, 4).ToUInt(false);
            this.priority = data.Mid(offset + 12, 4).ToUInt(false);
            this.initial_frames = data.Mid(offset + 0x10, 4).ToUInt(false);
            this.scale = data.Mid(offset + 20, 4).ToUInt(false);
            this.rate = data.Mid(offset + 0x18, 4).ToUInt(false);
            this.start = data.Mid(offset + 0x1c, 4).ToUInt(false);
            this.length = data.Mid(offset + 0x20, 4).ToUInt(false);
            this.suggested_buffer_size = data.Mid(offset + 0x24, 4).ToUInt(false);
            this.quality = data.Mid(offset + 40, 4).ToUInt(false);
            this.sample_size = data.Mid(offset + 0x2c, 4).ToUInt(false);
            this.left = data.Mid(offset + 0x30, 2).ToUShort(false);
            this.top = data.Mid(offset + 50, 2).ToUShort(false);
            this.right = data.Mid(offset + 0x34, 2).ToUShort(false);
            this.bottom = data.Mid(offset + 0x36, 2).ToUShort(false);
        }

        public ByteVector Type
        {
            get
            {
                return this.type;
            }
        }
        public ByteVector Handler
        {
            get
            {
                return this.handler;
            }
        }
        public uint Flags
        {
            get
            {
                return this.flags;
            }
        }
        public uint Priority
        {
            get
            {
                return this.priority;
            }
        }
        public uint InitialFrames
        {
            get
            {
                return this.initial_frames;
            }
        }
        public uint Scale
        {
            get
            {
                return this.scale;
            }
        }
        public uint Rate
        {
            get
            {
                return this.rate;
            }
        }
        public uint Start
        {
            get
            {
                return this.start;
            }
        }
        public uint Length
        {
            get
            {
                return this.length;
            }
        }
        public uint SuggestedBufferSize
        {
            get
            {
                return this.suggested_buffer_size;
            }
        }
        public uint Quality
        {
            get
            {
                return this.quality;
            }
        }
        public uint SampleSize
        {
            get
            {
                return this.sample_size;
            }
        }
        public ushort Left
        {
            get
            {
                return this.left;
            }
        }
        public ushort Top
        {
            get
            {
                return this.top;
            }
        }
        public ushort Right
        {
            get
            {
                return this.right;
            }
        }
        public ushort Bottom
        {
            get
            {
                return this.bottom;
            }
        }
    }
}

