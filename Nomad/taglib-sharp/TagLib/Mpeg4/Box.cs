namespace TagLib.Mpeg4
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class Box
    {
        private long data_position;
        private IsoHandlerBox handler;
        private BoxHeader header;

        protected Box(ByteVector type) : this(new BoxHeader(type))
        {
        }

        protected Box(BoxHeader header) : this(header, null)
        {
        }

        protected Box(BoxHeader header, IsoHandlerBox handler)
        {
            this.header = header;
            this.data_position = header.Position + header.HeaderSize;
            this.handler = handler;
        }

        public void AddChild(Box box)
        {
            ICollection<Box> children = this.Children as ICollection<Box>;
            if (children != null)
            {
                children.Add(box);
            }
        }

        public void ClearChildren()
        {
            ICollection<Box> children = this.Children as ICollection<Box>;
            if (children != null)
            {
                children.Clear();
            }
        }

        public Box GetChild(ByteVector type)
        {
            if (this.Children != null)
            {
                IEnumerator<Box> enumerator = this.Children.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        Box current = enumerator.Current;
                        if (current.BoxType == type)
                        {
                            return current;
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
            }
            return null;
        }

        public Box GetChildRecursively(ByteVector type)
        {
            if (this.Children != null)
            {
                IEnumerator<Box> enumerator = this.Children.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        Box current = enumerator.Current;
                        if (current.BoxType == type)
                        {
                            return current;
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
                IEnumerator<Box> enumerator2 = this.Children.GetEnumerator();
                try
                {
                    while (enumerator2.MoveNext())
                    {
                        Box childRecursively = enumerator2.Current.GetChildRecursively(type);
                        if (childRecursively != null)
                        {
                            return childRecursively;
                        }
                    }
                }
                finally
                {
                    if (enumerator2 == null)
                    {
                    }
                    enumerator2.Dispose();
                }
            }
            return null;
        }

        protected IEnumerable<Box> LoadChildren(TagLib.File file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            List<Box> list = new List<Box>();
            long dataPosition = this.DataPosition;
            long num2 = dataPosition + this.DataSize;
            this.header.Box = this;
            while (dataPosition < num2)
            {
                Box item = BoxFactory.CreateBox(file, dataPosition, this.header, this.handler, list.Count);
                list.Add(item);
                dataPosition += item.Size;
            }
            this.header.Box = null;
            return list;
        }

        protected ByteVector LoadData(TagLib.File file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            file.Seek(this.DataPosition);
            return file.ReadBlock(this.DataSize);
        }

        public void RemoveChild(ByteVector type)
        {
            ICollection<Box> children = this.Children as ICollection<Box>;
            if (children != null)
            {
                foreach (Box box in new List<Box>(children))
                {
                    if (box.BoxType == type)
                    {
                        children.Remove(box);
                    }
                }
            }
        }

        public void RemoveChild(Box box)
        {
            ICollection<Box> children = this.Children as ICollection<Box>;
            if (children != null)
            {
                children.Remove(box);
            }
        }

        public ByteVector Render()
        {
            return this.Render(new ByteVector());
        }

        protected virtual ByteVector Render(ByteVector topData)
        {
            bool flag = false;
            ByteVector vector = new ByteVector();
            if (this.Children != null)
            {
                IEnumerator<Box> enumerator = this.Children.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        Box current = enumerator.Current;
                        if (current.GetType() == typeof(IsoFreeSpaceBox))
                        {
                            flag = true;
                        }
                        else
                        {
                            vector.Add(current.Render());
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
            }
            else if (this.Data != null)
            {
                vector.Add(this.Data);
            }
            if (flag || (this.BoxType == TagLib.Mpeg4.BoxType.Meta))
            {
                long padding = this.DataSize - vector.Count;
                if ((this.header.DataSize != 0) && (padding >= 8L))
                {
                    vector.Add(new IsoFreeSpaceBox(padding).Render());
                }
                else
                {
                    vector.Add(new IsoFreeSpaceBox(0x800L).Render());
                }
            }
            this.header.DataSize = topData.Count + vector.Count;
            vector.Insert(0, topData);
            vector.Insert(0, this.header.Render());
            return vector;
        }

        public virtual ByteVector BoxType
        {
            get
            {
                return this.header.BoxType;
            }
        }

        public virtual IEnumerable<Box> Children
        {
            get
            {
                return null;
            }
        }

        public virtual ByteVector Data
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        protected virtual long DataPosition
        {
            get
            {
                return this.data_position;
            }
        }

        protected int DataSize
        {
            get
            {
                return (int) ((this.header.DataSize + this.data_position) - this.DataPosition);
            }
        }

        public IsoHandlerBox Handler
        {
            get
            {
                return this.handler;
            }
        }

        public bool HasChildren
        {
            get
            {
                ICollection<Box> children = this.Children as ICollection<Box>;
                return ((children != null) && (children.Count > 0));
            }
        }

        protected BoxHeader Header
        {
            get
            {
                return this.header;
            }
        }

        public virtual int Size
        {
            get
            {
                return (int) this.header.TotalBoxSize;
            }
        }
    }
}

