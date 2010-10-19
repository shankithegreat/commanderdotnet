namespace Nomad.Commons.IO
{
    using Nomad.Commons;
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public class PropertySet
    {
        protected Encoding DefaultEncoding;
        private Dictionary<int, object> PropertyMap;
        private Dictionary<int, int> PropertyOffsetMap;
        private BinaryReader Reader;
        private int SetOffset;

        internal PropertySet(BinaryReader reader, Guid formatId)
        {
            this.FormatId = formatId;
            this.Reader = reader;
            this.SetOffset = (int) reader.BaseStream.Position;
            this.ReadPropertySet(reader);
        }

        public void CacheAllProperties()
        {
            if (this.PropertyOffsetMap != null)
            {
                this.PropertyMap = new Dictionary<int, object>(this.PropertyOffsetMap.Count);
                foreach (KeyValuePair<int, int> pair in this.PropertyOffsetMap)
                {
                    this.Reader.BaseStream.Position = this.SetOffset + pair.Value;
                    this.PropertyMap.Add(pair.Key, this.ReadProperty(this.Reader));
                }
                this.PropertyOffsetMap = null;
                this.Reader = null;
            }
        }

        public void CacheProperty(int propertyId)
        {
            int num;
            if ((this.PropertyOffsetMap != null) && this.PropertyOffsetMap.TryGetValue(propertyId, out num))
            {
                if (this.PropertyMap == null)
                {
                    this.PropertyMap = new Dictionary<int, object>();
                }
                else if (this.PropertyMap.ContainsKey(propertyId))
                {
                    return;
                }
                this.Reader.BaseStream.Position = this.SetOffset + num;
                object obj2 = this.ReadProperty(this.Reader);
                this.PropertyMap.Add(propertyId, obj2);
                if (this.PropertyMap.Count == this.PropertyOffsetMap.Count)
                {
                    this.PropertyOffsetMap = null;
                    this.Reader = null;
                }
            }
        }

        public bool ContainsProperty(int propertyId)
        {
            if (this.PropertyOffsetMap != null)
            {
                return this.PropertyOffsetMap.ContainsKey(propertyId);
            }
            return this.PropertyMap.ContainsKey(propertyId);
        }

        private object ReadProperty(BinaryReader reader)
        {
            switch (((VarEnum) reader.ReadInt32()))
            {
                case VarEnum.VT_CF:
                {
                    int num3 = reader.ReadInt32();
                    if ((reader.ReadInt32() == -1) && (reader.ReadInt32() == 3))
                    {
                        reader.BaseStream.Seek(8L, SeekOrigin.Current);
                        using (Stream stream = new SubStream(reader.BaseStream, FileAccess.Read, (long) (num3 - 0x10)))
                        {
                            return new Metafile(stream);
                        }
                    }
                    return null;
                }
                case VarEnum.VT_CLSID:
                    return new Guid(reader.ReadBytes(0x10));

                case VarEnum.VT_FILETIME:
                    return DateTime.FromFileTime(reader.ReadInt64());

                case VarEnum.VT_EMPTY:
                    return null;

                case VarEnum.VT_NULL:
                    return DBNull.Value;

                case VarEnum.VT_I2:
                    return reader.ReadInt16();

                case VarEnum.VT_I4:
                    return reader.ReadInt32();

                case VarEnum.VT_R4:
                    return reader.ReadSingle();

                case VarEnum.VT_R8:
                    return reader.ReadDouble();

                case VarEnum.VT_BSTR:
                    return Encoding.Unicode.GetString(reader.ReadBytes(reader.ReadInt32()));

                case VarEnum.VT_BOOL:
                    return Convert.ToBoolean(reader.ReadInt16());

                case VarEnum.VT_UI1:
                    return reader.ReadByte();

                case VarEnum.VT_UI2:
                    return reader.ReadUInt16();

                case VarEnum.VT_UI4:
                    return reader.ReadUInt32();

                case VarEnum.VT_I8:
                    return reader.ReadInt64();

                case VarEnum.VT_UI8:
                    return reader.ReadUInt64();

                case VarEnum.VT_LPSTR:
                {
                    byte[] array = reader.ReadBytes(reader.ReadInt32());
                    int index = Array.IndexOf<byte>(array, 0);
                    if (this.DefaultEncoding == null)
                    {
                        this.DefaultEncoding = Encoding.Default;
                    }
                    return this.DefaultEncoding.GetString(array, 0, (index < 0) ? array.Length : index);
                }
                case VarEnum.VT_LPWSTR:
                {
                    byte[] sequense = reader.ReadBytes(reader.ReadInt32());
                    byte[] buffer3 = new byte[2];
                    int num2 = ByteArrayHelper.IndexOf(sequense, buffer3, sequense.Length);
                    return Encoding.Unicode.GetString(sequense, 0, (num2 < 0) ? sequense.Length : num2);
                }
            }
            throw new InvalidDataException("Unsupported variant type");
        }

        private void ReadPropertySet(BinaryReader reader)
        {
            reader.ReadInt32();
            int capacity = reader.ReadInt32();
            this.PropertyOffsetMap = new Dictionary<int, int>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                this.PropertyOffsetMap.Add(reader.ReadInt32(), reader.ReadInt32());
            }
        }

        public bool TryGetValue(int propertyId, out object value)
        {
            int num;
            if ((this.PropertyMap != null) && this.PropertyMap.TryGetValue(propertyId, out value))
            {
                return true;
            }
            if ((this.PropertyOffsetMap != null) && this.PropertyOffsetMap.TryGetValue(propertyId, out num))
            {
                this.Reader.BaseStream.Position = this.SetOffset + num;
                value = this.ReadProperty(this.Reader);
                return true;
            }
            value = null;
            return false;
        }

        public Guid FormatId { get; private set; }

        public object this[int propertyId]
        {
            get
            {
                object obj2;
                if ((this.PropertyMap != null) && this.PropertyMap.TryGetValue(propertyId, out obj2))
                {
                    return obj2;
                }
                if (this.PropertyOffsetMap == null)
                {
                    throw new KeyNotFoundException();
                }
                int num = this.PropertyOffsetMap[propertyId];
                this.Reader.BaseStream.Position = this.SetOffset + num;
                return this.ReadProperty(this.Reader);
            }
        }
    }
}

