namespace TagLib.Mpeg
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct VideoHeader : ICodec, IVideoCodec
    {
        private static readonly double[] frame_rates;
        private int width;
        private int height;
        private int frame_rate_index;
        private int bitrate;
        public VideoHeader(TagLib.File file, long position)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            file.Seek(position);
            ByteVector vector = file.ReadBlock(7);
            if (vector.Count < 7)
            {
                throw new CorruptFileException("Insufficient data in header.");
            }
            this.width = vector.Mid(0, 2).ToUShort() >> 4;
            this.height = vector.Mid(1, 2).ToUShort() & 0xfff;
            this.frame_rate_index = vector[3] & 15;
            this.bitrate = ((int) (vector.Mid(4, 3).ToUInt() >> 6)) & 0x3ffff;
        }

        static VideoHeader()
        {
            frame_rates = new double[] { 0.0, 23.976023976023978, 24.0, 25.0, 29.970029970029969, 30.0, 50.0, 59.940059940059939, 60.0 };
        }

        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.Zero;
            }
        }
        public TagLib.MediaTypes MediaTypes
        {
            get
            {
                return TagLib.MediaTypes.Video;
            }
        }
        public string Description
        {
            get
            {
                return "MPEG Video";
            }
        }
        public int VideoWidth
        {
            get
            {
                return this.width;
            }
        }
        public int VideoHeight
        {
            get
            {
                return this.height;
            }
        }
        public double VideoFrameRate
        {
            get
            {
                return ((this.frame_rate_index >= 9) ? 0.0 : frame_rates[this.frame_rate_index]);
            }
        }
        public int VideoBitrate
        {
            get
            {
                return this.bitrate;
            }
        }
    }
}

