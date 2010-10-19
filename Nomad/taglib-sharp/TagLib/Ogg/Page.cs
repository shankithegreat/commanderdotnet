namespace TagLib.Ogg
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class Page
    {
        private PageHeader header;
        private ByteVectorCollection packets;

        protected Page(PageHeader header)
        {
            this.header = header;
            this.packets = new ByteVectorCollection();
        }

        public Page(ByteVectorCollection packets, PageHeader header) : this(header)
        {
            if (packets == null)
            {
                throw new ArgumentNullException("packets");
            }
            this.packets = new ByteVectorCollection(packets);
            List<int> list = new List<int>();
            IEnumerator<ByteVector> enumerator = packets.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ByteVector current = enumerator.Current;
                    list.Add(current.Count);
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            header.PacketSizes = list.ToArray();
        }

        public Page(TagLib.Ogg.File file, long position) : this(new PageHeader(file, position))
        {
            file.Seek(position + this.header.Size);
            foreach (int num in this.header.PacketSizes)
            {
                this.packets.Add(file.ReadBlock(num));
            }
        }

        public static void OverwriteSequenceNumbers(TagLib.Ogg.File file, long position, IDictionary<uint, int> shiftTable)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if (shiftTable == null)
            {
                throw new ArgumentNullException("shiftTable");
            }
            bool flag = true;
            IEnumerator<KeyValuePair<uint, int>> enumerator = shiftTable.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<uint, int> current = enumerator.Current;
                    if (current.Value != 0)
                    {
                        flag = false;
                        goto Label_0065;
                    }
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
        Label_0065:
            if (flag)
            {
                return;
            }
            while (position < (file.Length - 0x1bL))
            {
                PageHeader header = new PageHeader(file, position);
                int length = (int) (header.Size + header.DataSize);
                if (shiftTable.ContainsKey(header.StreamSerialNumber) && (shiftTable[header.StreamSerialNumber] != 0))
                {
                    file.Seek(position);
                    ByteVector vector = file.ReadBlock(length);
                    ByteVector data = ByteVector.FromUInt(header.PageSequenceNumber + ((uint) ((long) shiftTable[header.StreamSerialNumber])), false);
                    for (int i = 0x12; i < 0x16; i++)
                    {
                        vector[i] = data[i - 0x12];
                    }
                    for (int j = 0x16; j < 0x1a; j++)
                    {
                        vector[j] = 0;
                    }
                    data.Add(ByteVector.FromUInt(vector.Checksum, false));
                    file.Seek(position + 0x12L);
                    file.WriteBlock(data);
                }
                position += length;
            }
        }

        public ByteVector Render()
        {
            ByteVector vector = this.header.Render();
            IEnumerator<ByteVector> enumerator = this.packets.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ByteVector current = enumerator.Current;
                    vector.Add(current);
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            ByteVector vector3 = ByteVector.FromUInt(vector.Checksum, false);
            for (int i = 0; i < 4; i++)
            {
                vector[i + 0x16] = vector3[i];
            }
            return vector;
        }

        public PageHeader Header
        {
            get
            {
                return this.header;
            }
        }

        public ByteVector[] Packets
        {
            get
            {
                return this.packets.ToArray();
            }
        }

        public uint Size
        {
            get
            {
                return (this.header.Size + this.header.DataSize);
            }
        }
    }
}

