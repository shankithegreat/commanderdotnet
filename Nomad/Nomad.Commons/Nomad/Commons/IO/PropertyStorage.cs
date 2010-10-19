namespace Nomad.Commons.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class PropertyStorage : IDisposable
    {
        private Dictionary<Guid, PropertySet> PropertySetMap;
        private BinaryReader Reader;

        public PropertyStorage(Stream source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            source.Seek(0L, SeekOrigin.Begin);
            this.Reader = new BinaryReader(source);
            this.ReadHeader(this.Reader);
        }

        public void Close()
        {
            if (this.Reader != null)
            {
                this.Reader.Close();
            }
            this.Reader = null;
        }

        public void Dispose()
        {
            this.Close();
        }

        public PropertySet GetSection(Guid formatId)
        {
            PropertySet set;
            if (this.PropertySetMap.TryGetValue(formatId, out set))
            {
                return set;
            }
            return null;
        }

        public IEnumerable<PropertySet> GetSections()
        {
            return this.PropertySetMap.Values;
        }

        private void ReadHeader(BinaryReader reader)
        {
            if (reader.ReadUInt16() != 0xfffe)
            {
                throw new InvalidDataException();
            }
            if (reader.ReadUInt16() != 0)
            {
                throw new InvalidDataException();
            }
            reader.ReadUInt32();
            this.ClassId = new Guid(reader.ReadBytes(0x10));
            int capacity = reader.ReadInt32();
            List<KeyValuePair<Guid, int>> list = new List<KeyValuePair<Guid, int>>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                Guid key = new Guid(reader.ReadBytes(0x10));
                int num4 = reader.ReadInt32();
                list.Add(new KeyValuePair<Guid, int>(key, num4));
            }
            this.PropertySetMap = new Dictionary<Guid, PropertySet>(capacity);
            foreach (KeyValuePair<Guid, int> pair in list)
            {
                PropertySet set;
                reader.BaseStream.Seek((long) pair.Value, SeekOrigin.Begin);
                if (pair.Key.Equals(SummaryInformationSet.FMTID_SummaryInformation))
                {
                    set = new SummaryInformationSet(reader);
                }
                else
                {
                    set = new PropertySet(reader, pair.Key);
                }
                this.PropertySetMap.Add(pair.Key, set);
            }
        }

        public Guid ClassId { get; private set; }
    }
}

