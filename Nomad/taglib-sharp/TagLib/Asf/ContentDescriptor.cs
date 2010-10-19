namespace TagLib.Asf
{
    using System;
    using TagLib;

    public class ContentDescriptor
    {
        private ByteVector byteValue;
        private ulong longValue;
        private string name;
        private string strValue;
        private DataType type;

        protected internal ContentDescriptor(TagLib.Asf.File file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if (!this.Parse(file))
            {
                throw new CorruptFileException("Failed to parse content descriptor.");
            }
        }

        public ContentDescriptor(string name, bool value)
        {
            this.name = name;
            this.type = DataType.Bool;
            this.longValue = !value ? ((ulong) 0L) : ((ulong) 1L);
        }

        public ContentDescriptor(string name, string value)
        {
            this.name = name;
            this.strValue = value;
        }

        public ContentDescriptor(string name, ushort value)
        {
            this.name = name;
            this.type = DataType.Word;
            this.longValue = value;
        }

        public ContentDescriptor(string name, uint value)
        {
            this.name = name;
            this.type = DataType.DWord;
            this.longValue = value;
        }

        public ContentDescriptor(string name, ulong value)
        {
            this.name = name;
            this.type = DataType.QWord;
            this.longValue = value;
        }

        public ContentDescriptor(string name, ByteVector value)
        {
            this.name = name;
            this.type = DataType.Bytes;
            this.byteValue = new ByteVector(value);
        }

        protected bool Parse(TagLib.Asf.File file)
        {
            int length = file.ReadWord();
            this.name = file.ReadUnicode(length);
            this.type = (DataType) file.ReadWord();
            int num2 = file.ReadWord();
            switch (this.type)
            {
                case DataType.Unicode:
                    this.strValue = file.ReadUnicode(num2);
                    break;

                case DataType.Bytes:
                    this.byteValue = file.ReadBlock(num2);
                    break;

                case DataType.Bool:
                    this.longValue = file.ReadDWord();
                    break;

                case DataType.DWord:
                    this.longValue = file.ReadDWord();
                    break;

                case DataType.QWord:
                    this.longValue = file.ReadQWord();
                    break;

                case DataType.Word:
                    this.longValue = file.ReadWord();
                    break;

                default:
                    return false;
            }
            return true;
        }

        public ByteVector Render()
        {
            ByteVector byteValue = null;
            switch (this.type)
            {
                case DataType.Unicode:
                    byteValue = TagLib.Asf.Object.RenderUnicode(this.strValue);
                    break;

                case DataType.Bytes:
                    byteValue = this.byteValue;
                    break;

                case DataType.Bool:
                case DataType.DWord:
                    byteValue = TagLib.Asf.Object.RenderDWord((uint) this.longValue);
                    break;

                case DataType.QWord:
                    byteValue = TagLib.Asf.Object.RenderQWord(this.longValue);
                    break;

                case DataType.Word:
                    byteValue = TagLib.Asf.Object.RenderWord((ushort) this.longValue);
                    break;

                default:
                    return null;
            }
            ByteVector vector2 = TagLib.Asf.Object.RenderUnicode(this.name);
            return new ByteVector { TagLib.Asf.Object.RenderWord((ushort) vector2.Count), vector2, TagLib.Asf.Object.RenderWord((ushort) this.type), TagLib.Asf.Object.RenderWord((ushort) byteValue.Count), byteValue };
        }

        public bool ToBool()
        {
            return (this.longValue != 0L);
        }

        public ByteVector ToByteVector()
        {
            return this.byteValue;
        }

        public uint ToDWord()
        {
            uint num;
            if (((this.type == DataType.Unicode) && (this.strValue != null)) && uint.TryParse(this.strValue, out num))
            {
                return num;
            }
            return (uint) this.longValue;
        }

        public ulong ToQWord()
        {
            ulong num;
            if (((this.type == DataType.Unicode) && (this.strValue != null)) && ulong.TryParse(this.strValue, out num))
            {
                return num;
            }
            return this.longValue;
        }

        public override string ToString()
        {
            if (this.type == DataType.Unicode)
            {
                return this.strValue;
            }
            if (this.type == DataType.Bytes)
            {
                return this.byteValue.ToString(StringType.UTF16LE);
            }
            return this.longValue.ToString();
        }

        public ushort ToWord()
        {
            ushort num;
            if (((this.type == DataType.Unicode) && (this.strValue != null)) && ushort.TryParse(this.strValue, out num))
            {
                return num;
            }
            return (ushort) this.longValue;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public DataType Type
        {
            get
            {
                return this.type;
            }
        }
    }
}

