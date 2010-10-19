namespace TagLib.Riff
{
    using System;
    using TagLib;

    public abstract class ListTag : Tag
    {
        private List fields;

        protected ListTag()
        {
            this.fields = new List();
        }

        protected ListTag(ByteVector data)
        {
            this.fields = new List(data);
        }

        protected ListTag(List fields)
        {
            if (fields == null)
            {
                throw new ArgumentNullException("fields");
            }
            this.fields = fields;
        }

        protected ListTag(TagLib.File file, long position, int length)
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
            this.fields = new List(file, position, length);
        }

        public override void Clear()
        {
            this.fields.Clear();
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
            return this.fields.GetValueAsUInt(id);
        }

        public ByteVectorCollection GetValues(ByteVector id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            return this.fields.GetValues(id);
        }

        [Obsolete("Use GetValuesAsStrings(ByteVector)")]
        public StringCollection GetValuesAsStringCollection(ByteVector id)
        {
            return new StringCollection(this.fields.GetValuesAsStrings(id));
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
            return this.fields.GetValuesAsStrings(id);
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
            this.fields.RemoveValue(id);
        }

        public ByteVector Render()
        {
            return this.fields.Render();
        }

        public abstract ByteVector RenderEnclosed();
        protected ByteVector RenderEnclosed(ByteVector id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            return this.fields.RenderEnclosed(id);
        }

        public void SetValue(ByteVector id, uint value)
        {
            this.fields.SetValue(id, value);
        }

        public void SetValue(ByteVector id, ByteVectorCollection value)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            this.fields.SetValue(id, value);
        }

        public void SetValue(ByteVector id, params string[] value)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            this.fields.SetValue(id, value);
        }

        [Obsolete("Use SetValue(ByteVector,string[])")]
        public void SetValue(ByteVector id, StringCollection value)
        {
            this.fields.SetValue(id, value);
        }

        public void SetValue(ByteVector id, params ByteVector[] value)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Count != 4)
            {
                throw new ArgumentException("ID must be 4 bytes long.", "id");
            }
            this.fields.SetValue(id, value);
        }

        public override bool IsEmpty
        {
            get
            {
                return (this.fields.Count == 0);
            }
        }
    }
}

