namespace TagLib.Mpeg
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct VBRIHeader
    {
        private uint frames;
        private uint size;
        private bool present;
        public static readonly ReadOnlyByteVector FileIdentifier;
        public static readonly VBRIHeader Unknown;
        private VBRIHeader(uint frame, uint size)
        {
            this.frames = frame;
            this.size = size;
            this.present = false;
        }

        public VBRIHeader(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (!data.StartsWith(FileIdentifier))
            {
                throw new CorruptFileException("Not a valid VBRI header");
            }
            int startIndex = 10;
            this.size = data.Mid(startIndex, 4).ToUInt();
            startIndex += 4;
            this.frames = data.Mid(startIndex, 4).ToUInt();
            startIndex += 4;
            this.present = true;
        }

        static VBRIHeader()
        {
            FileIdentifier = "VBRI";
            Unknown = new VBRIHeader(0, 0);
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
        public static int VBRIHeaderOffset()
        {
            return 0x24;
        }
    }
}

