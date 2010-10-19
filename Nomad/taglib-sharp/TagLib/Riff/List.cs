namespace TagLib.Riff
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using TagLib;

    [Serializable, ComVisible(false)]
    public class List : Dictionary<ByteVector, ByteVectorCollection>
    {
        public List()
        {
        }

        public List(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.Parse(data);
        }

        protected List(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public List(TagLib.File file, long position, int length)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            if ((position < 0L) || (position > (file.Length - length)))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            file.Seek(position);
            this.Parse(file.ReadBlock(length));
        }

        public uint GetValueAsUInt(ByteVector id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            foreach (string str in this.GetValuesAsStrings(id))
            {
                uint num2;
                if (uint.TryParse(str, out num2))
                {
                    return num2;
                }
            }
            return 0;
        }

        public ByteVectorCollection GetValues(ByteVector id)
        {
            ByteVectorCollection vectors;
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            return (!this.TryGetValue(id, out vectors) ? new ByteVectorCollection() : vectors);
        }

        [Obsolete("Use GetValuesAsStrings(ByteVector)")]
        public StringCollection GetValuesAsStringCollection(ByteVector id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            return new StringCollection(this.GetValuesAsStrings(id));
        }

        public string[] GetValuesAsStrings(ByteVector id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            ByteVectorCollection values = this.GetValues(id);
            string[] strArray = new string[values.Count];
            for (int i = 0; i < strArray.Length; i++)
            {
                ByteVector vector = values[i];
                if (vector == null)
                {
                    strArray[i] = string.Empty;
                }
                else
                {
                    int count = vector.Count;
                    while ((count > 0) && (vector[count - 1] == 0))
                    {
                        count--;
                    }
                    strArray[i] = vector.ToString(StringType.UTF8, 0, count);
                }
            }
            return strArray;
        }

        private void Parse(ByteVector data)
        {
            int num2;
            for (int i = 0; (i + 8) < data.Count; i += 8 + num2)
            {
                ByteVector key = data.Mid(i, 4);
                num2 = (int) data.Mid(i + 4, 4).ToUInt(false);
                if (!this.ContainsKey(key))
                {
                    this.Add(key, new ByteVectorCollection());
                }
                this[key].Add(data.Mid(i + 8, num2));
                if ((num2 % 2) == 1)
                {
                    num2++;
                }
            }
        }

        public void RemoveValue(ByteVector id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            if (this.ContainsKey(id))
            {
                this.Remove(id);
            }
        }

        public ByteVector Render()
        {
            ByteVector vector = new ByteVector();
            foreach (ByteVector vector2 in base.Keys)
            {
                IEnumerator<ByteVector> enumerator = this[vector2].GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        ByteVector current = enumerator.Current;
                        if (current.Count != 0)
                        {
                            vector.Add(vector2);
                            vector.Add(ByteVector.FromUInt((uint) current.Count, false));
                            vector.Add(current);
                            if ((current.Count % 2) == 1)
                            {
                                vector.Add((byte) 0);
                            }
                        }
                    }
                    continue;
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
            }
            return vector;
        }

        public ByteVector RenderEnclosed(ByteVector id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            ByteVector vector = this.Render();
            if (vector.Count <= 8)
            {
                return new ByteVector();
            }
            ByteVector data = new ByteVector("LIST");
            data.Add(ByteVector.FromUInt((uint) (vector.Count + 4), false));
            data.Add(id);
            vector.Insert(0, data);
            return vector;
        }

        public void SetValue(ByteVector id, IEnumerable<string> values)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            if (values == null)
            {
                this.RemoveValue(id);
            }
            else
            {
                ByteVectorCollection vectors = new ByteVectorCollection();
                IEnumerator<string> enumerator = values.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        string current = enumerator.Current;
                        if (!string.IsNullOrEmpty(current))
                        {
                            ByteVector item = ByteVector.FromString(current, StringType.UTF8);
                            item.Add((byte) 0);
                            vectors.Add(item);
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
                if (vectors.Count == 0)
                {
                    this.RemoveValue(id);
                }
                else
                {
                    this.SetValue(id, vectors);
                }
            }
        }

        public void SetValue(ByteVector id, IEnumerable<ByteVector> values)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            if (values == null)
            {
                this.RemoveValue(id);
            }
            else if (this.ContainsKey(id))
            {
                this[id] = new ByteVectorCollection(values);
            }
            else
            {
                this.Add(id, new ByteVectorCollection(values));
            }
        }

        public void SetValue(ByteVector id, params string[] values)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            if ((values == null) || (values.Length == 0))
            {
                this.RemoveValue(id);
            }
            else
            {
                this.SetValue(id, (IEnumerable<string>) values);
            }
        }

        public void SetValue(ByteVector id, uint value)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            if (value == 0)
            {
                this.RemoveValue(id);
            }
            else
            {
                string[] values = new string[] { value.ToString(CultureInfo.InvariantCulture) };
                this.SetValue(id, values);
            }
        }

        public void SetValue(ByteVector id, params ByteVector[] values)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            if ((values == null) || (values.Length == 0))
            {
                this.RemoveValue(id);
            }
            else
            {
                this.SetValue(id, (IEnumerable<ByteVector>) values);
            }
        }
    }
}

