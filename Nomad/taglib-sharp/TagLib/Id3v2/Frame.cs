namespace TagLib.Id3v2
{
    using System;
    using TagLib;

    public abstract class Frame : ICloneable
    {
        private byte encryption_id;
        private byte group_id;
        private FrameHeader header;

        protected Frame(FrameHeader header)
        {
            this.header = header;
        }

        protected Frame(ByteVector data, byte version)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Count < ((version >= 3) ? 4 : 3))
            {
                throw new ArgumentException("Data contains an incomplete identifier.", "data");
            }
            this.header = new FrameHeader(data, version);
        }

        public virtual Frame Clone()
        {
            int offset = 0;
            return FrameFactory.CreateFrame(this.Render(4), ref offset, 4, false);
        }

        protected static StringType CorrectEncoding(StringType type, byte version)
        {
            if (TagLib.Id3v2.Tag.ForceDefaultEncoding)
            {
                type = TagLib.Id3v2.Tag.DefaultEncoding;
            }
            return (((version >= 4) || (type != StringType.UTF8)) ? type : StringType.UTF16);
        }

        protected ByteVector FieldData(ByteVector frameData, int offset, byte version)
        {
            if (frameData == null)
            {
                throw new ArgumentNullException("frameData");
            }
            int startIndex = offset + ((int) FrameHeader.Size(version));
            int size = (int) this.Size;
            if (((ushort) (this.Flags & (FrameFlags.Compression | FrameFlags.DataLengthIndicator))) != 0)
            {
                startIndex += 4;
                size -= 4;
            }
            if (((ushort) (this.Flags & FrameFlags.GroupingIdentity)) != 0)
            {
                if (frameData.Count >= startIndex)
                {
                    throw new CorruptFileException("Frame data incomplete.");
                }
                this.group_id = frameData[startIndex++];
                size--;
            }
            if (((ushort) (this.Flags & FrameFlags.Encryption)) != 0)
            {
                if (frameData.Count >= startIndex)
                {
                    throw new CorruptFileException("Frame data incomplete.");
                }
                this.encryption_id = frameData[startIndex++];
                size--;
            }
            size = Math.Min(size, frameData.Count - startIndex);
            if (size < 0)
            {
                throw new CorruptFileException("Frame size less than zero.");
            }
            ByteVector data = frameData.Mid(startIndex, size);
            if (((ushort) (this.Flags & FrameFlags.Unsynchronisation)) != 0)
            {
                int count = data.Count;
                SynchData.ResynchByteVector(data);
                size -= data.Count - count;
            }
            if (((ushort) (this.Flags & FrameFlags.Encryption)) != 0)
            {
                throw new NotImplementedException();
            }
            if (((ushort) (this.Flags & FrameFlags.Compression)) != 0)
            {
                throw new NotImplementedException();
            }
            return data;
        }

        protected abstract void ParseFields(ByteVector data, byte version);
        public virtual ByteVector Render(byte version)
        {
            if (version < 4)
            {
                this.Flags = (FrameFlags) ((ushort) (((int) this.Flags) & 0xfffc));
            }
            if (version < 3)
            {
                this.Flags = (FrameFlags) ((ushort) (((int) this.Flags) & 0x8fb3));
            }
            ByteVector data = this.RenderFields(version);
            if (data.Count == 0)
            {
                return new ByteVector();
            }
            ByteVector vector2 = new ByteVector();
            if (((ushort) (this.Flags & (FrameFlags.Compression | FrameFlags.DataLengthIndicator))) != 0)
            {
                vector2.Add(ByteVector.FromUInt((uint) data.Count));
            }
            if (((ushort) (this.Flags & FrameFlags.GroupingIdentity)) != 0)
            {
                vector2.Add(this.group_id);
            }
            if (((ushort) (this.Flags & FrameFlags.Encryption)) != 0)
            {
                vector2.Add(this.encryption_id);
            }
            if (((ushort) (this.Flags & FrameFlags.Compression)) != 0)
            {
                throw new NotImplementedException("Compression not yet supported");
            }
            if (((ushort) (this.Flags & FrameFlags.Encryption)) != 0)
            {
                throw new NotImplementedException("Encryption not yet supported");
            }
            if (((ushort) (this.Flags & FrameFlags.Unsynchronisation)) != 0)
            {
                SynchData.UnsynchByteVector(data);
            }
            if (vector2.Count > 0)
            {
                data.Insert(0, vector2);
            }
            this.header.FrameSize = (uint) data.Count;
            ByteVector vector3 = this.header.Render(version);
            vector3.Add(data);
            return vector3;
        }

        protected abstract ByteVector RenderFields(byte version);
        protected void SetData(ByteVector data, int offset, byte version, bool readHeader)
        {
            if (readHeader)
            {
                this.header = new FrameHeader(data, version);
            }
            this.ParseFields(this.FieldData(data, offset, version), version);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        [Obsolete("Use ByteVector.TextDelimiter.")]
        public static ByteVector TextDelimiter(StringType type)
        {
            return ByteVector.TextDelimiter(type);
        }

        public short EncryptionId
        {
            get
            {
                return ((((ushort) (this.Flags & FrameFlags.Encryption)) == 0) ? ((short) (-1)) : ((short) this.encryption_id));
            }
            set
            {
                if ((value >= 0) && (value <= 0xff))
                {
                    this.encryption_id = (byte) value;
                    this.Flags = (FrameFlags) ((ushort) (this.Flags | FrameFlags.Encryption));
                }
                else
                {
                    this.Flags = (FrameFlags) ((ushort) (((int) this.Flags) & 0xfffb));
                }
            }
        }

        public FrameFlags Flags
        {
            get
            {
                return this.header.Flags;
            }
            set
            {
                this.header.Flags = value;
            }
        }

        public ReadOnlyByteVector FrameId
        {
            get
            {
                return this.header.FrameId;
            }
        }

        public short GroupId
        {
            get
            {
                return ((((ushort) (this.Flags & FrameFlags.GroupingIdentity)) == 0) ? ((short) (-1)) : ((short) this.group_id));
            }
            set
            {
                if ((value >= 0) && (value <= 0xff))
                {
                    this.group_id = (byte) value;
                    this.Flags = (FrameFlags) ((ushort) (this.Flags | FrameFlags.GroupingIdentity));
                }
                else
                {
                    this.Flags = (FrameFlags) ((ushort) (((int) this.Flags) & 0xffbf));
                }
            }
        }

        public uint Size
        {
            get
            {
                return this.header.FrameSize;
            }
        }
    }
}

