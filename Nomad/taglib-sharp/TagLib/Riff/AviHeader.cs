namespace TagLib.Riff
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct AviHeader
    {
        private uint microseconds_per_frame;
        private uint max_bytes_per_second;
        private uint flags;
        private uint total_frames;
        private uint initial_frames;
        private uint streams;
        private uint suggested_buffer_size;
        private uint width;
        private uint height;
        [Obsolete("Use AviHeader(ByteVector,int)")]
        public AviHeader(ByteVector data) : this(data, 0)
        {
        }

        public AviHeader(ByteVector data, int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((offset + 40) > data.Count)
            {
                throw new CorruptFileException("Expected 40 bytes.");
            }
            this.microseconds_per_frame = data.Mid(offset, 4).ToUInt(false);
            this.max_bytes_per_second = data.Mid(offset + 4, 4).ToUInt(false);
            this.flags = data.Mid(offset + 12, 4).ToUInt(false);
            this.total_frames = data.Mid(offset + 0x10, 4).ToUInt(false);
            this.initial_frames = data.Mid(offset + 20, 4).ToUInt(false);
            this.streams = data.Mid(offset + 0x18, 4).ToUInt(false);
            this.suggested_buffer_size = data.Mid(offset + 0x1c, 4).ToUInt(false);
            this.width = data.Mid(offset + 0x20, 4).ToUInt(false);
            this.height = data.Mid(offset + 0x24, 4).ToUInt(false);
        }

        public uint MicrosecondsPerFrame
        {
            get
            {
                return this.microseconds_per_frame;
            }
        }
        public uint MaxBytesPerSecond
        {
            get
            {
                return this.max_bytes_per_second;
            }
        }
        public uint Flags
        {
            get
            {
                return this.flags;
            }
        }
        public uint TotalFrames
        {
            get
            {
                return this.total_frames;
            }
        }
        public uint InitialFrames
        {
            get
            {
                return this.initial_frames;
            }
        }
        public uint Streams
        {
            get
            {
                return this.streams;
            }
        }
        public uint SuggestedBufferSize
        {
            get
            {
                return this.suggested_buffer_size;
            }
        }
        public uint Width
        {
            get
            {
                return this.width;
            }
        }
        public uint Height
        {
            get
            {
                return this.height;
            }
        }
        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromMilliseconds((this.TotalFrames * this.MicrosecondsPerFrame) / 1000.0);
            }
        }
    }
}

