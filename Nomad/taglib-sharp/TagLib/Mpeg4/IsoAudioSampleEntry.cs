namespace TagLib.Mpeg4
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using TagLib;

    public class IsoAudioSampleEntry : IsoSampleEntry, ICodec, IAudioCodec
    {
        private ushort channel_count;
        private IEnumerable<Box> children;
        private uint sample_rate;
        private ushort sample_size;

        public IsoAudioSampleEntry(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, file, handler)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            file.Seek(base.DataPosition + 8L);
            this.channel_count = file.ReadBlock(2).ToUShort();
            this.sample_size = file.ReadBlock(2).ToUShort();
            file.Seek(base.DataPosition + 0x10L);
            this.sample_rate = file.ReadBlock(4).ToUInt();
            this.children = base.LoadChildren(file);
        }

        public int AudioBitrate
        {
            get
            {
                AppleElementaryStreamDescriptor childRecursively = base.GetChildRecursively("esds") as AppleElementaryStreamDescriptor;
                if (childRecursively == null)
                {
                    return 0;
                }
                return (int) childRecursively.AverageBitrate;
            }
        }

        public int AudioChannels
        {
            get
            {
                return this.channel_count;
            }
        }

        public int AudioSampleRate
        {
            get
            {
                return (int) (this.sample_rate >> 0x10);
            }
        }

        public int AudioSampleSize
        {
            get
            {
                return this.sample_size;
            }
        }

        public override IEnumerable<Box> Children
        {
            get
            {
                return this.children;
            }
        }

        protected override long DataPosition
        {
            get
            {
                return (base.DataPosition + 20L);
            }
        }

        public string Description
        {
            get
            {
                object[] args = new object[] { this.BoxType };
                return string.Format(CultureInfo.InvariantCulture, "MPEG-4 Audio ({0})", args);
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
                return TagLib.MediaTypes.Audio;
            }
        }
    }
}

