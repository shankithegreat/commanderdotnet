namespace TagLib.Mpeg4
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct BoxHeader
    {
        private ByteVector box_type;
        private ByteVector extended_type;
        private ulong box_size;
        private uint header_size;
        private long position;
        private TagLib.Mpeg4.Box box;
        private bool from_disk;
        public static readonly BoxHeader Empty;
        public BoxHeader(TagLib.File file, long position)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            this.box = null;
            this.from_disk = true;
            this.position = position;
            file.Seek(position);
            ByteVector vector = file.ReadBlock(0x20);
            int startIndex = 0;
            if (vector.Count < (8 + startIndex))
            {
                throw new CorruptFileException("Not enough data in box header.");
            }
            this.header_size = 8;
            this.box_size = vector.Mid(startIndex, 4).ToUInt();
            this.box_type = vector.Mid(startIndex + 4, 4);
            if (this.box_size == 1L)
            {
                if (vector.Count < (8 + startIndex))
                {
                    throw new CorruptFileException("Not enough data in box header.");
                }
                this.header_size += 8;
                this.box_size = vector.Mid(startIndex, 8).ToULong();
                startIndex += 8;
            }
            if (this.box_type == TagLib.Mpeg4.BoxType.Uuid)
            {
                if (vector.Count < (0x10 + startIndex))
                {
                    throw new CorruptFileException("Not enough data in box header.");
                }
                this.header_size += 0x10;
                this.extended_type = vector.Mid(startIndex, 0x10);
            }
            else
            {
                this.extended_type = null;
            }
        }

        public BoxHeader(ByteVector type) : this(type, null)
        {
        }

        public BoxHeader(ByteVector type, ByteVector extendedType)
        {
            this.position = -1L;
            this.box = null;
            this.from_disk = false;
            this.box_type = type;
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (type.Count != 4)
            {
                throw new ArgumentException("Box type must be 4 bytes in length.", "type");
            }
            this.box_size = this.header_size = 8;
            if (type != "uuid")
            {
                if (extendedType != null)
                {
                    throw new ArgumentException("Extended type only permitted for 'uuid'.", "extendedType");
                }
                this.extended_type = extendedType;
            }
            else
            {
                if (extendedType == null)
                {
                    throw new ArgumentNullException("extendedType");
                }
                if (extendedType.Count != 0x10)
                {
                    throw new ArgumentException("Extended type must be 16 bytes in length.", "extendedType");
                }
                this.box_size = this.header_size = 0x18;
                this.extended_type = extendedType;
            }
        }

        static BoxHeader()
        {
            Empty = new BoxHeader("xxxx");
        }

        public ByteVector BoxType
        {
            get
            {
                return this.box_type;
            }
        }
        public ByteVector ExtendedType
        {
            get
            {
                return this.extended_type;
            }
        }
        public long HeaderSize
        {
            get
            {
                return (long) this.header_size;
            }
        }
        public long DataSize
        {
            get
            {
                return (long) (this.box_size - this.header_size);
            }
            set
            {
                this.box_size = (ulong) (value + this.header_size);
            }
        }
        [Obsolete("Use HeaderSize")]
        public long DataOffset
        {
            get
            {
                return (long) this.header_size;
            }
        }
        public long TotalBoxSize
        {
            get
            {
                return (long) this.box_size;
            }
        }
        public long Position
        {
            get
            {
                return (!this.from_disk ? -1L : this.position);
            }
        }
        public long Overwrite(TagLib.File file, long sizeChange)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if (!this.from_disk)
            {
                throw new InvalidOperationException("Cannot overwrite headers not on disk.");
            }
            long headerSize = this.HeaderSize;
            this.DataSize += sizeChange;
            file.Insert(this.Render(), this.position, headerSize);
            return ((sizeChange + this.HeaderSize) - headerSize);
        }

        public ByteVector Render()
        {
            if (((this.header_size == 8) || (this.header_size == 0x18)) && (this.box_size > 0xffffffffL))
            {
                this.header_size += 8;
                this.box_size += (ulong) 8L;
            }
            ByteVector vector = ByteVector.FromUInt(((this.header_size != 8) && (this.header_size != 0x18)) ? 1 : ((uint) this.box_size));
            vector.Add(this.box_type);
            if ((this.header_size == 0x10) || (this.header_size == 0x20))
            {
                vector.Add(ByteVector.FromULong(this.box_size));
            }
            if (this.header_size >= 0x18)
            {
                vector.Add(this.extended_type);
            }
            return vector;
        }

        internal TagLib.Mpeg4.Box Box
        {
            get
            {
                return this.box;
            }
            set
            {
                this.box = value;
            }
        }
    }
}

