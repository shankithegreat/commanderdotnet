namespace TagLib.Ape
{
    using System;
    using TagLib;

    public class Item : ICloneable
    {
        private ReadOnlyByteVector data;
        private string key;
        private bool read_only;
        private int size_on_disk;
        private string[] text;
        private ItemType type;

        private Item(Item item)
        {
            this.type = item.type;
            this.key = item.key;
            if (item.data != null)
            {
                this.data = new ReadOnlyByteVector(item.data);
            }
            if (item.text != null)
            {
                this.text = (string[]) item.text.Clone();
            }
            this.read_only = item.read_only;
            this.size_on_disk = item.size_on_disk;
        }

        public Item(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.key = key;
            this.text = new string[] { value };
        }

        public Item(string key, params string[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.key = key;
            this.text = (string[]) value.Clone();
        }

        public Item(string key, ByteVector value)
        {
            this.key = key;
            this.type = ItemType.Binary;
            this.data = value as ReadOnlyByteVector;
            if (this.data == null)
            {
                this.data = new ReadOnlyByteVector(value);
            }
        }

        [Obsolete("Use Item(string,string[])")]
        public Item(string key, StringCollection value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.key = key;
            this.text = value.ToArray();
        }

        public Item(ByteVector data, int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.Parse(data, offset);
        }

        public Item Clone()
        {
            return new Item(this);
        }

        protected void Parse(ByteVector data, int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (data.Count < (offset + 11))
            {
                throw new CorruptFileException("Not enough data for APE Item");
            }
            uint num = data.Mid(offset, 4).ToUInt(false);
            uint num2 = data.Mid(offset + 4, 4).ToUInt(false);
            this.ReadOnly = (num2 & 1) == 1;
            this.Type = ((ItemType) (num2 >> 1)) & (ItemType.Locator | ItemType.Binary);
            int num3 = data.Find(ByteVector.TextDelimiter(StringType.UTF8), offset + 8);
            this.key = data.ToString(StringType.UTF8, offset + 8, (num3 - offset) - 8);
            if (num > ((data.Count - num3) - 1))
            {
                throw new CorruptFileException("Invalid data length.");
            }
            this.size_on_disk = ((num3 + 1) + ((int) num)) - offset;
            if (this.Type == ItemType.Binary)
            {
                this.data = new ReadOnlyByteVector(data.Mid(num3 + 1, (int) num));
            }
            else
            {
                this.text = data.Mid(num3 + 1, (int) num).ToStrings(StringType.UTF8, 0);
            }
        }

        public ByteVector Render()
        {
            uint num = (uint) ((!this.ReadOnly ? 0 : 1) | (((int) this.Type) << 1));
            if (this.IsEmpty)
            {
                return new ByteVector();
            }
            ByteVector data = null;
            if (((this.type == ItemType.Binary) && (this.text == null)) && (this.data != null))
            {
                data = this.data;
            }
            if ((data == null) && (this.text != null))
            {
                data = new ByteVector();
                for (int i = 0; i < this.text.Length; i++)
                {
                    if (i != 0)
                    {
                        data.Add((byte) 0);
                    }
                    data.Add(ByteVector.FromString(this.text[i], StringType.UTF8));
                }
            }
            if ((data == null) || (data.Count == 0))
            {
                return new ByteVector();
            }
            ByteVector vector2 = new ByteVector();
            vector2.Add(ByteVector.FromUInt((uint) data.Count, false));
            vector2.Add(ByteVector.FromUInt(num, false));
            vector2.Add(ByteVector.FromString(this.key, StringType.UTF8));
            vector2.Add((byte) 0);
            vector2.Add(data);
            this.size_on_disk = vector2.Count;
            return vector2;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public override string ToString()
        {
            if ((this.type != ItemType.Binary) && (this.text != null))
            {
                return string.Join(", ", this.text);
            }
            return null;
        }

        public string[] ToStringArray()
        {
            if ((this.type != ItemType.Binary) && (this.text != null))
            {
                return this.text;
            }
            return new string[0];
        }

        public bool IsEmpty
        {
            get
            {
                if (this.type != ItemType.Binary)
                {
                    return ((this.text == null) || (this.text.Length == 0));
                }
                return ((this.data == null) || this.data.IsEmpty);
            }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this.read_only;
            }
            set
            {
                this.read_only = value;
            }
        }

        public int Size
        {
            get
            {
                return this.size_on_disk;
            }
        }

        public ItemType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        public ByteVector Value
        {
            get
            {
                return ((this.type != ItemType.Binary) ? null : ((ByteVector) this.data));
            }
        }
    }
}

