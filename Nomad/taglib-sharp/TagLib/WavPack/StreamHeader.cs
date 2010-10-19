namespace TagLib.WavPack
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct StreamHeader : IEquatable<StreamHeader>, ICodec, IAudioCodec, ILosslessAudioCodec
    {
        private const int BYTES_STORED = 3;
        private const int MONO_FLAG = 4;
        private const int SHIFT_LSB = 13;
        private const long SHIFT_MASK = 0x3e000L;
        private const int SRATE_LSB = 0x17;
        private const long SRATE_MASK = 0x7800000L;
        public const uint Size = 0x20;
        private static readonly uint[] sample_rates;
        private long stream_length;
        private ushort version;
        private uint flags;
        private uint samples;
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
            if (data.Count < 0x20L)
            {
                throw new CorruptFileException("Insufficient data in stream header");
            }
            this.stream_length = streamLength;
            this.version = data.Mid(8, 2).ToUShort(false);
            this.flags = data.Mid(0x18, 4).ToUInt(false);
            this.samples = data.Mid(12, 4).ToUInt(false);
        }

        static StreamHeader()
        {
            sample_rates = new uint[] { 0x1770, 0x1f40, 0x2580, 0x2b11, 0x2ee0, 0x3e80, 0x5622, 0x5dc0, 0x7d00, 0xac44, 0xbb80, 0xfa00, 0x15888, 0x17700, 0x2ee00 };
            FileIdentifier = "wvpk";
        }

        public TimeSpan Duration
        {
            get
            {
                return ((this.AudioSampleRate <= 0) ? TimeSpan.Zero : TimeSpan.FromSeconds((((double) this.samples) / ((double) this.AudioSampleRate)) + 0.5));
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
                return string.Format(CultureInfo.InvariantCulture, "WavPack Version {0} Audio", args);
            }
        }
        public int AudioBitrate
        {
            get
            {
                return ((this.Duration <= TimeSpan.Zero) ? ((int) 0.0) : ((int) ((((double) (this.stream_length * 8L)) / this.Duration.TotalSeconds) / 1000.0)));
            }
        }
        public int AudioSampleRate
        {
            get
            {
                return (int) sample_rates[(int) ((IntPtr) ((this.flags & 0x7800000L) >> 0x17))];
            }
        }
        public int AudioChannels
        {
            get
            {
                return (((this.flags & 4) == 0) ? 2 : 1);
            }
        }
        public int Version
        {
            get
            {
                return this.version;
            }
        }
        public int BitsPerSample
        {
            get
            {
                return (int) ((((this.flags & 3) + 1) * 8) - ((this.flags & 0x3e000L) >> 13));
            }
        }
        public override int GetHashCode()
        {
            return (int) ((this.flags ^ this.samples) ^ this.version);
        }

        public override bool Equals(object other)
        {
            return ((other is StreamHeader) && this.Equals((StreamHeader) other));
        }

        public bool Equals(StreamHeader other)
        {
            return (((this.flags == other.flags) && (this.samples == other.samples)) && (this.version == other.version));
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

