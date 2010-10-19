namespace TagLib.Asf
{
    using System;
    using TagLib;

    public class DescriptionRecord
    {
        private ByteVector byteValue;
        private System.Guid guidValue;
        private ushort lang_list_index;
        private ulong longValue;
        private string name;
        private ushort stream_number;
        private string strValue;
        private DataType type;

        protected internal DescriptionRecord(TagLib.Asf.File file)
        {
            this.guidValue = System.Guid.Empty;
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if (!this.Parse(file))
            {
                throw new CorruptFileException("Failed to parse description record.");
            }
        }

        public DescriptionRecord(ushort languageListIndex, ushort streamNumber, string name, bool value)
        {
            this.guidValue = System.Guid.Empty;
            this.lang_list_index = languageListIndex;
            this.stream_number = streamNumber;
            this.name = name;
            this.type = DataType.Bool;
            this.longValue = !value ? ((ulong) 0L) : ((ulong) 1L);
        }

        public DescriptionRecord(ushort languageListIndex, ushort streamNumber, string name, System.Guid value)
        {
            this.guidValue = System.Guid.Empty;
            this.lang_list_index = languageListIndex;
            this.stream_number = streamNumber;
            this.name = name;
            this.type = DataType.Guid;
            this.guidValue = value;
        }

        public DescriptionRecord(ushort languageListIndex, ushort streamNumber, string name, string value)
        {
            this.guidValue = System.Guid.Empty;
            this.lang_list_index = languageListIndex;
            this.stream_number = streamNumber;
            this.name = name;
            this.strValue = value;
        }

        public DescriptionRecord(ushort languageListIndex, ushort streamNumber, string name, ushort value)
        {
            this.guidValue = System.Guid.Empty;
            this.lang_list_index = languageListIndex;
            this.stream_number = streamNumber;
            this.name = name;
            this.type = DataType.Word;
            this.longValue = value;
        }

        public DescriptionRecord(ushort languageListIndex, ushort streamNumber, string name, uint value)
        {
            this.guidValue = System.Guid.Empty;
            this.lang_list_index = languageListIndex;
            this.stream_number = streamNumber;
            this.name = name;
            this.type = DataType.DWord;
            this.longValue = value;
        }

        public DescriptionRecord(ushort languageListIndex, ushort streamNumber, string name, ulong value)
        {
            this.guidValue = System.Guid.Empty;
            this.lang_list_index = languageListIndex;
            this.stream_number = streamNumber;
            this.name = name;
            this.type = DataType.QWord;
            this.longValue = value;
        }

        public DescriptionRecord(ushort languageListIndex, ushort streamNumber, string name, ByteVector value)
        {
            this.guidValue = System.Guid.Empty;
            this.lang_list_index = languageListIndex;
            this.stream_number = streamNumber;
            this.name = name;
            this.type = DataType.Bytes;
            this.byteValue = new ByteVector(value);
        }

        protected bool Parse(TagLib.Asf.File file)
        {
            this.lang_list_index = file.ReadWord();
            this.stream_number = file.ReadWord();
            ushort length = file.ReadWord();
            this.type = (DataType) file.ReadWord();
            int num2 = (int) file.ReadDWord();
            this.name = file.ReadUnicode(length);
            switch (this.type)
            {
                case DataType.Unicode:
                    this.strValue = file.ReadUnicode(num2);
                    break;

                case DataType.Bytes:
                    this.byteValue = file.ReadBlock(num2);
                    break;

                case DataType.Bool:
                case DataType.DWord:
                    this.longValue = file.ReadDWord();
                    break;

                case DataType.QWord:
                    this.longValue = file.ReadQWord();
                    break;

                case DataType.Word:
                    this.longValue = file.ReadWord();
                    break;

                case DataType.Guid:
                    this.guidValue = file.ReadGuid();
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

                case DataType.Guid:
                    byteValue = this.guidValue.ToByteArray();
                    break;

                default:
                    return null;
            }
            ByteVector vector2 = TagLib.Asf.Object.RenderUnicode(this.name);
            return new ByteVector { TagLib.Asf.Object.RenderWord(this.lang_list_index), TagLib.Asf.Object.RenderWord(this.stream_number), TagLib.Asf.Object.RenderWord((ushort) vector2.Count), TagLib.Asf.Object.RenderWord((ushort) this.type), TagLib.Asf.Object.RenderDWord((uint) byteValue.Count), vector2, byteValue };
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

        public System.Guid ToGuid()
        {
            return this.guidValue;
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

        public ushort LanguageListIndex
        {
            get
            {
                return this.lang_list_index;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public ushort StreamNumber
        {
            get
            {
                return this.stream_number;
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

