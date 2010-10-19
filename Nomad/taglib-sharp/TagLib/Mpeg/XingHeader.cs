namespace TagLib.Mpeg
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct XingHeader
    {
        private uint frames;
        private uint size;
        private bool present;
        public static readonly ReadOnlyByteVector FileIdentifier;
        public static readonly XingHeader Unknown;
        private XingHeader(uint frame, uint size)
        {
            this.frames = frame;
            this.size = size;
            this.present = false;
        }

        public XingHeader(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (!data.StartsWith(FileIdentifier))
            {
                throw new CorruptFileException("Not a valid Xing header");
            }
            int startIndex = 8;
            if ((data[7] & 1) != 0)
            {
                this.frames = data.Mid(startIndex, 4).ToUInt();
                startIndex += 4;
            }
            else
            {
                this.frames = 0;
            }
            if ((data[7] & 2) != 0)
            {
                this.size = data.Mid(startIndex, 4).ToUInt();
                startIndex += 4;
            }
            else
            {
                this.size = 0;
            }
            this.present = true;
        }

        static XingHeader()
        {
            FileIdentifier = "Xing";
            Unknown = new XingHeader(0, 0);
        }

        public uint TotalFrames
        {
            get
            {
                return this.frames;
            }
        }
        public uint TotalSize
        {
            get
            {
                return this.size;
            }
        }
        public bool Present
        {
            get
            {
                return this.present;
            }
        }
        public static int XingHeaderOffset(TagLib.Mpeg.Version version, ChannelMode channelMode)
        {
            bool flag = channelMode == ChannelMode.SingleChannel;
            if (version == TagLib.Mpeg.Version.Version1)
            {
                return (!flag ? 0x24 : 0x15);
            }
            return (!flag ? 0x15 : 13);
        }
    }
}

