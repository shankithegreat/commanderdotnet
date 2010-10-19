namespace TagLib.Id3v2
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct Footer
    {
        public const uint Size = 10;
        private byte major_version;
        private byte revision_number;
        private HeaderFlags flags;
        private uint tag_size;
        public static readonly ReadOnlyByteVector FileIdentifier;
        public Footer(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Count < 10L)
            {
                throw new CorruptFileException("Provided data is smaller than object size.");
            }
            if (!data.StartsWith(FileIdentifier))
            {
                throw new CorruptFileException("Provided data does not start with the file identifier");
            }
            this.major_version = data[3];
            this.revision_number = data[4];
            this.flags = (HeaderFlags) data[5];
            if ((this.major_version == 2) && ((((int) this.flags) & 0x7f) != 0))
            {
                throw new CorruptFileException("Invalid flags set on version 2 tag.");
            }
            if ((this.major_version == 3) && ((((int) this.flags) & 15) != 0))
            {
                throw new CorruptFileException("Invalid flags set on version 3 tag.");
            }
            if ((this.major_version == 4) && ((((int) this.flags) & 7) != 0))
            {
                throw new CorruptFileException("Invalid flags set on version 4 tag.");
            }
            for (int i = 6; i < 10; i++)
            {
                if (data[i] >= 0x80)
                {
                    throw new CorruptFileException("One of the bytes in the header was greater than the allowed 128.");
                }
            }
            this.tag_size = SynchData.ToUInt(data.Mid(6, 4));
        }

        public Footer(Header header)
        {
            this.major_version = header.MajorVersion;
            this.revision_number = header.RevisionNumber;
            this.flags = (HeaderFlags) ((byte) (header.Flags | HeaderFlags.FooterPresent));
            this.tag_size = header.TagSize;
        }

        static Footer()
        {
            FileIdentifier = "3DI";
        }

        public byte MajorVersion
        {
            get
            {
                return ((this.major_version != 0) ? this.major_version : TagLib.Id3v2.Tag.DefaultVersion);
            }
            set
            {
                if (value != 4)
                {
                    throw new ArgumentException("Version unsupported.");
                }
                this.major_version = value;
            }
        }
        public byte RevisionNumber
        {
            get
            {
                return this.revision_number;
            }
            set
            {
                this.revision_number = value;
            }
        }
        public HeaderFlags Flags
        {
            get
            {
                return this.flags;
            }
            set
            {
                if ((((byte) (value & (HeaderFlags.ExtendedHeader | HeaderFlags.ExperimentalIndicator))) != 0) && (this.MajorVersion < 3))
                {
                    throw new ArgumentException("Feature only supported in version 2.3+", "value");
                }
                if ((((byte) (value & HeaderFlags.FooterPresent)) != 0) && (this.MajorVersion < 3))
                {
                    throw new ArgumentException("Feature only supported in version 2.4+", "value");
                }
                this.flags = value;
            }
        }
        public uint TagSize
        {
            get
            {
                return this.tag_size;
            }
            set
            {
                this.tag_size = value;
            }
        }
        public uint CompleteTagSize
        {
            get
            {
                return ((this.TagSize + 10) + 10);
            }
        }
        public ByteVector Render()
        {
            return new ByteVector { FileIdentifier, this.MajorVersion, this.RevisionNumber, this.flags, SynchData.FromUInt(this.TagSize) };
        }
    }
}

