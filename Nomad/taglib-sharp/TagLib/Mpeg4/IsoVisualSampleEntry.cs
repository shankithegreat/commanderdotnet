namespace TagLib.Mpeg4
{
    using System;
    using System.Globalization;
    using TagLib;

    public class IsoVisualSampleEntry : IsoSampleEntry, ICodec, IVideoCodec
    {
        private ushort height;
        private ushort width;

        public IsoVisualSampleEntry(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, file, handler)
        {
            file.Seek(base.DataPosition + 0x10L);
            this.width = file.ReadBlock(2).ToUShort();
            this.height = file.ReadBlock(2).ToUShort();
        }

        protected override long DataPosition
        {
            get
            {
                return (base.DataPosition + 0x3eL);
            }
        }

        public string Description
        {
            get
            {
                object[] args = new object[] { this.BoxType };
                return string.Format(CultureInfo.InvariantCulture, "MPEG-4 Video ({0})", args);
            }
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

        public int VideoHeight
        {
            get
            {
                return this.height;
            }
        }

        public int VideoWidth
        {
            get
            {
                return this.width;
            }
        }
    }
}

