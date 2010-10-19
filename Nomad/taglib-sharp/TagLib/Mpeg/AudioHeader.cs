namespace TagLib.Mpeg
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct AudioHeader : ICodec, IAudioCodec
    {
        private static readonly int[,] sample_rates;
        private static readonly int[,] block_size;
        private static readonly int[,,] bitrates;
        private uint flags;
        private long stream_length;
        private TagLib.Mpeg.XingHeader xing_header;
        private TagLib.Mpeg.VBRIHeader vbri_header;
        private TimeSpan duration;
        public static readonly AudioHeader Unknown;
        private AudioHeader(uint flags, long streamLength, TagLib.Mpeg.XingHeader xingHeader, TagLib.Mpeg.VBRIHeader vbriHeader)
        {
            this.flags = flags;
            this.stream_length = streamLength;
            this.xing_header = xingHeader;
            this.vbri_header = vbriHeader;
            this.duration = TimeSpan.Zero;
        }

        private AudioHeader(ByteVector data, TagLib.File file, long position)
        {
            this.duration = TimeSpan.Zero;
            this.stream_length = 0L;
            if (data.Count < 4)
            {
                throw new CorruptFileException("Insufficient header length.");
            }
            if (data[0] != 0xff)
            {
                throw new CorruptFileException("First byte did not match MPEG synch.");
            }
            if (((data[1] & 230) <= 0xe0) || ((data[1] & 0x18) == 8))
            {
                throw new CorruptFileException("Second byte did not match MPEG synch.");
            }
            this.flags = data.ToUInt();
            if (((this.flags >> 12) & 15) == 15)
            {
                throw new CorruptFileException("Header uses invalid bitrate index.");
            }
            if (((this.flags >> 10) & 3) == 3)
            {
                throw new CorruptFileException("Invalid sample rate.");
            }
            this.xing_header = TagLib.Mpeg.XingHeader.Unknown;
            this.vbri_header = TagLib.Mpeg.VBRIHeader.Unknown;
            file.Seek(position + TagLib.Mpeg.XingHeader.XingHeaderOffset(this.Version, this.ChannelMode));
            ByteVector vector = file.ReadBlock(0x10);
            if ((vector.Count == 0x10) && vector.StartsWith(TagLib.Mpeg.XingHeader.FileIdentifier))
            {
                this.xing_header = new TagLib.Mpeg.XingHeader(vector);
            }
            if (!this.xing_header.Present)
            {
                file.Seek(position + TagLib.Mpeg.VBRIHeader.VBRIHeaderOffset());
                ByteVector vector2 = file.ReadBlock(0x18);
                if ((vector2.Count == 0x18) && vector2.StartsWith(TagLib.Mpeg.VBRIHeader.FileIdentifier))
                {
                    this.vbri_header = new TagLib.Mpeg.VBRIHeader(vector2);
                }
            }
        }

        static AudioHeader()
        {
            sample_rates = new int[,] { { 0xac44, 0xbb80, 0x7d00, 0 }, { 0x5622, 0x5dc0, 0x3e80, 0 }, { 0x2b11, 0x2ee0, 0x1f40, 0 } };
            block_size = new int[,] { { 0, 0x180, 0x480, 0x480 }, { 0, 0x180, 0x480, 0x240 }, { 0, 0x180, 0x480, 0x240 } };
            bitrates = new int[,,] { { { 0, 0x20, 0x40, 0x60, 0x80, 160, 0xc0, 0xe0, 0x100, 0x120, 320, 0x160, 0x180, 0x1a0, 0x1c0, -1 }, { 0, 0x20, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 160, 0xc0, 0xe0, 0x100, 320, 0x180, -1 }, { 0, 0x20, 40, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 160, 0xc0, 0xe0, 0x100, 320, -1 } }, { { 0, 0x20, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 0x90, 160, 0xb0, 0xc0, 0xe0, 0x100, -1 }, { 0, 8, 0x10, 0x18, 0x20, 40, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 0x90, 160, -1 }, { 0, 8, 0x10, 0x18, 0x20, 40, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 0x90, 160, -1 } } };
            Unknown = new AudioHeader(0, 0L, TagLib.Mpeg.XingHeader.Unknown, TagLib.Mpeg.VBRIHeader.Unknown);
        }

        public TagLib.Mpeg.Version Version
        {
            get
            {
                switch (((this.flags >> 0x13) & 3))
                {
                    case 0:
                        return TagLib.Mpeg.Version.Version25;

                    case 2:
                        return TagLib.Mpeg.Version.Version2;
                }
                return TagLib.Mpeg.Version.Version1;
            }
        }
        public int AudioLayer
        {
            get
            {
                switch (((this.flags >> 0x11) & 3))
                {
                    case 1:
                        return 3;

                    case 2:
                        return 2;
                }
                return 1;
            }
        }
        public int AudioBitrate
        {
            get
            {
                if ((this.xing_header.TotalSize > 0) && (this.duration > TimeSpan.Zero))
                {
                    return (int) Math.Round((double) ((((double) (this.XingHeader.TotalSize * 8L)) / this.duration.TotalSeconds) / 1000.0));
                }
                if ((this.vbri_header.TotalSize > 0) && (this.duration > TimeSpan.Zero))
                {
                    return (int) Math.Round((double) ((((double) (this.VBRIHeader.TotalSize * 8L)) / this.duration.TotalSeconds) / 1000.0));
                }
                return bitrates[(this.Version != TagLib.Mpeg.Version.Version1) ? 1 : 0, (this.AudioLayer <= 0) ? 0 : (this.AudioLayer - 1), ((int) (this.flags >> 12)) & 15];
            }
        }
        public int AudioSampleRate
        {
            get
            {
                return sample_rates[(int) this.Version, ((int) (this.flags >> 10)) & 3];
            }
        }
        public int AudioChannels
        {
            get
            {
                return ((this.ChannelMode != TagLib.Mpeg.ChannelMode.SingleChannel) ? 2 : 1);
            }
        }
        public int AudioFrameLength
        {
            get
            {
                switch (this.AudioLayer)
                {
                    case 1:
                        return (((0xbb80 * this.AudioBitrate) / this.AudioSampleRate) + (!this.IsPadded ? 0 : 4));

                    case 2:
                        break;

                    case 3:
                        if (this.Version == TagLib.Mpeg.Version.Version1)
                        {
                            break;
                        }
                        return (((0x11940 * this.AudioBitrate) / this.AudioSampleRate) + (!this.IsPadded ? 0 : 1));

                    default:
                        return 0;
                }
                return (((0x23280 * this.AudioBitrate) / this.AudioSampleRate) + (!this.IsPadded ? 0 : 1));
            }
        }
        public TimeSpan Duration
        {
            get
            {
                if (this.duration <= TimeSpan.Zero)
                {
                    if (this.xing_header.TotalFrames > 0)
                    {
                        double num = ((double) block_size[(int) this.Version, this.AudioLayer]) / ((double) this.AudioSampleRate);
                        this.duration = TimeSpan.FromSeconds(num * this.XingHeader.TotalFrames);
                    }
                    else if (this.vbri_header.TotalFrames > 0)
                    {
                        double num2 = ((double) block_size[(int) this.Version, this.AudioLayer]) / ((double) this.AudioSampleRate);
                        this.duration = TimeSpan.FromSeconds(Math.Round((double) (num2 * this.VBRIHeader.TotalFrames)));
                    }
                    else if ((this.AudioFrameLength > 0) && (this.AudioBitrate > 0))
                    {
                        int num3 = (int) ((this.stream_length / ((long) this.AudioFrameLength)) + 1L);
                        this.duration = TimeSpan.FromSeconds((((double) (this.AudioFrameLength * num3)) / ((double) (this.AudioBitrate * 0x7d))) + 0.5);
                    }
                }
                return this.duration;
            }
        }
        public string Description
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("MPEG Version ");
                switch (this.Version)
                {
                    case TagLib.Mpeg.Version.Version1:
                        builder.Append("1");
                        break;

                    case TagLib.Mpeg.Version.Version2:
                        builder.Append("2");
                        break;

                    case TagLib.Mpeg.Version.Version25:
                        builder.Append("2.5");
                        break;
                }
                builder.Append(" Audio, Layer ");
                builder.Append(this.AudioLayer);
                if (this.xing_header.Present || this.vbri_header.Present)
                {
                    builder.Append(" VBR");
                }
                return builder.ToString();
            }
        }
        public TagLib.MediaTypes MediaTypes
        {
            get
            {
                return TagLib.MediaTypes.Audio;
            }
        }
        public bool IsProtected
        {
            get
            {
                return (((this.flags >> 0x10) & 1) == 0);
            }
        }
        public bool IsPadded
        {
            get
            {
                return (((this.flags >> 9) & 1) == 1);
            }
        }
        public bool IsCopyrighted
        {
            get
            {
                return (((this.flags >> 3) & 1) == 1);
            }
        }
        public bool IsOriginal
        {
            get
            {
                return (((this.flags >> 2) & 1) == 1);
            }
        }
        public TagLib.Mpeg.ChannelMode ChannelMode
        {
            get
            {
                return (((TagLib.Mpeg.ChannelMode) (this.flags >> 14)) & TagLib.Mpeg.ChannelMode.SingleChannel);
            }
        }
        public TagLib.Mpeg.XingHeader XingHeader
        {
            get
            {
                return this.xing_header;
            }
        }
        public TagLib.Mpeg.VBRIHeader VBRIHeader
        {
            get
            {
                return this.vbri_header;
            }
        }
        public void SetStreamLength(long streamLength)
        {
            this.stream_length = streamLength;
            if ((this.xing_header.TotalFrames == 0) || (this.vbri_header.TotalFrames == 0))
            {
                this.duration = TimeSpan.Zero;
            }
        }

        public static bool Find(out AudioHeader header, TagLib.File file, long position, int length)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            long num = position + length;
            header = Unknown;
            file.Seek(position);
            ByteVector vector = file.ReadBlock(3);
            if (vector.Count >= 3)
            {
                do
                {
                    file.Seek(position + 3L);
                    vector = vector.Mid(vector.Count - 3);
                    vector.Add(file.ReadBlock((int) TagLib.File.BufferSize));
                    for (int i = 0; (i < (vector.Count - 3)) && ((length < 0) || ((position + i) < num)); i++)
                    {
                        if ((vector[i] == 0xff) && (vector[i + 1] > 0xe0))
                        {
                            try
                            {
                                header = new AudioHeader(vector.Mid(i, 4), file, position + i);
                                return true;
                            }
                            catch (CorruptFileException)
                            {
                            }
                        }
                    }
                    position += TagLib.File.BufferSize;
                }
                while ((vector.Count > 3) && ((length < 0) || (position < num)));
            }
            return false;
        }

        public static bool Find(out AudioHeader header, TagLib.File file, long position)
        {
            return Find(out header, file, position, -1);
        }
    }
}

