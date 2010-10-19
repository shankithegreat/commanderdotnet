namespace TagLib.Flac
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct StreamHeader : ICodec, IAudioCodec, ILosslessAudioCodec
    {
        private uint flags;
        private uint low_length;
        private long stream_length;
        public StreamHeader(ByteVector data, long streamLength)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Count < 0x12)
            {
                throw new CorruptFileException("Not enough data in FLAC header.");
            }
            this.stream_length = streamLength;
            this.flags = data.Mid(10, 4).ToUInt(true);
            this.low_length = data.Mid(14, 4).ToUInt(true);
        }

        public TimeSpan Duration
        {
            get
            {
                return (((this.AudioSampleRate <= 0) || (this.stream_length <= 0L)) ? TimeSpan.Zero : TimeSpan.FromSeconds((((double) this.low_length) / ((double) this.AudioSampleRate)) + this.HighLength));
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
                return (int) (this.flags >> 12);
            }
        }
        public int AudioChannels
        {
            get
            {
                return ((((int) (this.flags >> 9)) & 7) + 1);
            }
        }
        public TagLib.MediaTypes MediaTypes
        {
            get
            {
                return TagLib.MediaTypes.Audio;
            }
        }
        [Obsolete("This property is depreciated, use BitsPerSample instead")]
        public int AudioSampleWidth
        {
            get
            {
                return this.BitsPerSample;
            }
        }
        public int BitsPerSample
        {
            get
            {
                return ((((int) (this.flags >> 4)) & 0x1f) + 1);
            }
        }
        public string Description
        {
            get
            {
                return "Flac Audio";
            }
        }
        private uint HighLength
        {
            get
            {
                return ((this.AudioSampleRate <= 0) ? ((uint) 0L) : ((uint) ((((ulong) ((this.flags & 15) << 0x1c)) / ((long) this.AudioSampleRate)) << 4)));
            }
        }
    }
}

