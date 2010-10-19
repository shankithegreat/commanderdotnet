namespace TagLib.Ape
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct StreamHeader : ICodec, IAudioCodec, ILosslessAudioCodec
    {
        public const uint Size = 0x4c;
        private ushort version;
        private CompressionLevel compression_level;
        private uint blocks_per_frame;
        private uint final_frame_blocks;
        private uint total_frames;
        private ushort bits_per_sample;
        private ushort channels;
        private uint sample_rate;
        private long stream_length;
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
            if (data.Count < 0x4cL)
            {
                throw new CorruptFileException("Insufficient data in stream header");
            }
            this.stream_length = streamLength;
            this.version = data.Mid(4, 2).ToUShort(false);
            this.compression_level = (CompressionLevel) data.Mid(0x34, 2).ToUShort(false);
            this.blocks_per_frame = data.Mid(0x38, 4).ToUInt(false);
            this.final_frame_blocks = data.Mid(60, 4).ToUInt(false);
            this.total_frames = data.Mid(0x40, 4).ToUInt(false);
            this.bits_per_sample = data.Mid(0x44, 2).ToUShort(false);
            this.channels = data.Mid(70, 2).ToUShort(false);
            this.sample_rate = data.Mid(0x48, 4).ToUInt(false);
        }

        static StreamHeader()
        {
            FileIdentifier = "MAC ";
        }

        public TimeSpan Duration
        {
            get
            {
                if ((this.sample_rate > 0) && (this.total_frames > 0))
                {
                    return TimeSpan.FromSeconds(((double) (((this.total_frames - 1) * this.blocks_per_frame) + this.final_frame_blocks)) / ((double) this.sample_rate));
                }
                return TimeSpan.Zero;
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
                return string.Format(CultureInfo.InvariantCulture, "Monkey's Audio APE Version {0:0.000}", args);
            }
        }
        public int AudioBitrate
        {
            get
            {
                TimeSpan duration = this.Duration;
                if (duration <= TimeSpan.Zero)
                {
                    return 0;
                }
                return (((int) (((double) (this.stream_length * 8L)) / duration.TotalSeconds)) / 0x3e8);
            }
        }
        public int AudioSampleRate
        {
            get
            {
                return (int) this.sample_rate;
            }
        }
        public int AudioChannels
        {
            get
            {
                return this.channels;
            }
        }
        public double Version
        {
            get
            {
                return (((double) this.version) / 1000.0);
            }
        }
        public int BitsPerSample
        {
            get
            {
                return this.bits_per_sample;
            }
        }
        public CompressionLevel Compression
        {
            get
            {
                return this.compression_level;
            }
        }
    }
}

