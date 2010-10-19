namespace TagLib.Ape
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct Footer : IEquatable<Footer>
    {
        public const uint Size = 0x20;
        private uint version;
        private FooterFlags flags;
        private uint item_count;
        private uint tag_size;
        public static readonly ReadOnlyByteVector FileIdentifier;
        public Footer(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Count < 0x20L)
            {
                throw new CorruptFileException("Provided data is smaller than object size.");
            }
            if (!data.StartsWith(FileIdentifier))
            {
                throw new CorruptFileException("Provided data does not start with File Identifier");
            }
            this.version = data.Mid(8, 4).ToUInt(false);
            this.tag_size = data.Mid(12, 4).ToUInt(false);
            this.item_count = data.Mid(0x10, 4).ToUInt(false);
            this.flags = (FooterFlags) data.Mid(20, 4).ToUInt(false);
        }

        static Footer()
        {
            FileIdentifier = "APETAGEX";
        }

        public uint Version
        {
            get
            {
                return ((this.version != 0) ? this.version : 0x7d0);
            }
        }
        public FooterFlags Flags
        {
            get
            {
                return this.flags;
            }
            set
            {
                this.flags = value;
            }
        }
        public uint ItemCount
        {
            get
            {
                return this.item_count;
            }
            set
            {
                this.item_count = value;
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
                return (this.TagSize + (((this.Flags & ((FooterFlags) (-2147483648))) == ((FooterFlags) 0)) ? 0 : 0x20));
            }
        }
        public ByteVector RenderFooter()
        {
            return this.Render(false);
        }

        public ByteVector RenderHeader()
        {
            return (((this.Flags & ((FooterFlags) (-2147483648))) == ((FooterFlags) 0)) ? new ByteVector() : this.Render(true));
        }

        private ByteVector Render(bool isHeader)
        {
            ByteVector vector = new ByteVector {
                FileIdentifier,
                ByteVector.FromUInt(0x7d0, 0),
                ByteVector.FromUInt(this.tag_size, 0),
                ByteVector.FromUInt(this.item_count, 0)
            };
            uint num = 0;
            if ((this.Flags & ((FooterFlags) (-2147483648))) != ((FooterFlags) 0))
            {
                num |= 0x80000000;
            }
            if (isHeader)
            {
                num |= 0x20000000;
            }
            else
            {
                num &= 0xdfffffff;
            }
            vector.Add(ByteVector.FromUInt(num, false));
            vector.Add(ByteVector.FromULong(0L));
            return vector;
        }

        public override int GetHashCode()
        {
            return (int) (((this.flags ^ this.tag_size) ^ this.item_count) ^ this.version);
        }

        public override bool Equals(object other)
        {
            return ((other is Footer) && this.Equals((Footer) other));
        }

        public bool Equals(Footer other)
        {
            return ((((this.flags == other.flags) && (this.tag_size == other.tag_size)) && (this.item_count == other.item_count)) && (this.version == other.version));
        }

        public static bool operator ==(Footer first, Footer second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Footer first, Footer second)
        {
            return !first.Equals(second);
        }
    }
}

