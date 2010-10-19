namespace TagLib.Aiff
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct StreamHeader : ICodec, IAudioCodec, ILosslessAudioCodec
    {
        public const uint Size = 0x1a;
        private ushort channels;
        private ulong total_frames;
        private ushort bits_per_sample;
        private ulong sample_rate;
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
            this.stream_length = streamLength;
            this.channels = data.Mid(8, 2).ToUShort(true);
            this.total_frames = data.Mid(10, 4).ToULong(true);
            this.bits_per_sample = data.Mid(14, 2).ToUShort(true);
            ByteVector vector = data.Mid(0x11, 1);
            ulong num = data.Mid(0x12, 2).ToULong(true);
            this.sample_rate = 0xac44L;
            ulong num2 = num;
            if ((num2 >= 0xaddcL) && (num2 <= 0xaddeL))
            {
                switch (((int) (num2 - 0xaddcL)))
                {
                    case 0:
                        if (vector == 12)
                        {
                            this.sample_rate = 0x2b77L;
                        }
                        return;

                    case 2:
                        if (vector == 13)
                        {
                            this.sample_rate = 0x56efL;
                        }
                        return;
                }
            }
            if (num2 == 0xac44L)
            {
                if (vector == 14)
                {
                    this.sample_rate = 0xac44L;
                }
                else if (vector == 13)
                {
                    this.sample_rate = 0x5622L;
                }
                else if (vector == 12)
                {
                    this.sample_rate = 0x2b11L;
                }
            }
            else if (num2 == 0xbb80L)
            {
                if (vector == 14)
                {
                    this.sample_rate = 0xbb80L;
                }
                else if (vector == 13)
                {
                    this.sample_rate = 0x5dc0L;
                }
            }
            else if (num2 == 0xfa00L)
            {
                if (vector == 13)
                {
                    this.sample_rate = 0x7d00L;
                }
                else if (vector == 12)
                {
                    this.sample_rate = 0x3e80L;
                }
                else if (vector == 11)
                {
                    this.sample_rate = 0x1f40L;
                }
            }
        }

        static StreamHeader()
        {
            FileIdentifier = "COMM";
        }

        public TimeSpan Duration
        {
            get
            {
                if ((this.sample_rate > 0L) && (this.total_frames > 0L))
                {
                    return TimeSpan.FromSeconds(((double) this.total_frames) / ((double) this.sample_rate));
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
                return "AIFF Audio";
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
        public int BitsPerSample
        {
            get
            {
                return this.bits_per_sample;
            }
        }
    }
}

