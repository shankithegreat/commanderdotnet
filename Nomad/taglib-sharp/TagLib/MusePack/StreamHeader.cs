namespace TagLib.MusePack
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct StreamHeader : ICodec, IAudioCodec
    {
        public const uint Size = 0x38;
        private static ushort[] sftable;
        private long stream_length;
        private int version;
        private uint header_data;
        private int sample_rate;
        private uint frames;
        public static readonly ReadOnlyByteVector FileIdentifier;
        public StreamHeader(ByteVector data, long streamLength)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (!data.StartsWith(FileIdentifier))
            {
                throw new CorruptFileException("Data does not begin with identifier.");
            }
            if (data.Count < 0x38L)
            {
                throw new CorruptFileException("Insufficient data in stream header");
            }
            this.stream_length = streamLength;
            this.version = data[3] & 15;
            if (this.version >= 7)
            {
                this.frames = data.Mid(4, 4).ToUInt(false);
                uint num = data.Mid(8, 4).ToUInt(false);
                this.sample_rate = sftable[(((num >> 0x11) & 1) * 2) + ((num >> 0x10) & 1)];
                this.header_data = 0;
            }
            else
            {
                this.header_data = data.Mid(0, 4).ToUInt(false);
                this.version = ((int) (this.header_data >> 11)) & 0x3ff;
                this.sample_rate = 0xac44;
                this.frames = data.Mid(4, (this.version < 5) ? 2 : 4).ToUInt(false);
            }
        }

        static StreamHeader()
        {
            sftable = new ushort[] { 0xac44, 0xbb80, 0x93a8, 0x7d00 };
            FileIdentifier = "MP+";
        }

        public TimeSpan Duration
        {
            get
            {
                if ((this.sample_rate <= 0) && (this.stream_length <= 0L))
                {
                    return TimeSpan.Zero;
                }
                return TimeSpan.FromSeconds((((double) ((this.frames * 0x480) - 0x240)) / ((double) this.sample_rate)) + 0.5);
            }
        }
        public TagLib.MediaTypes MediaTypes
        {
            get
            {
                return TagLib.MediaTypes.Audio;
            }
        }
        public string Description
        {
            get
            {
                object[] args = new object[] { this.Version };
                return string.Format(CultureInfo.InvariantCulture, "MusePack Version {0} Audio", args);
            }
        }
        public int AudioBitrate
        {
            get
            {
                if (this.header_data != 0)
                {
                    return (((int) (this.header_data >> 0x17)) & 0x1ff);
                }
                return ((this.Duration <= TimeSpan.Zero) ? ((int) 0.0) : ((int) ((((double) (this.stream_length * 8L)) / this.Duration.TotalSeconds) / 1000.0)));
            }
        }
        public int AudioSampleRate
        {
            get
            {
                return this.sample_rate;
            }
        }
        public int AudioChannels
        {
            get
            {
                return 2;
            }
        }
        public int Version
        {
            get
            {
                return this.version;
            }
        }
        public override int GetHashCode()
        {
            return (((int) ((this.header_data ^ this.sample_rate) ^ this.frames)) ^ this.version);
        }

        public override bool Equals(object other)
        {
            return ((other is StreamHeader) && this.Equals((StreamHeader) other));
        }

        public bool Equals(StreamHeader other)
        {
            return ((((this.header_data == other.header_data) && (this.sample_rate == other.sample_rate)) && (this.version == other.version)) && (this.frames == other.frames));
        }

        public static bool operator ==(StreamHeader first, StreamHeader second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(StreamHeader first, StreamHeader second)
        {
            return !first.Equals(second);
        }
    }
}

