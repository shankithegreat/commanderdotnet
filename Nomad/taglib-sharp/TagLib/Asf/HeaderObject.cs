namespace TagLib.Asf
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class HeaderObject : TagLib.Asf.Object
    {
        private List<TagLib.Asf.Object> children;
        private ByteVector reserved;

        public HeaderObject(TagLib.Asf.File file, long position) : base(file, position)
        {
            if (!base.Guid.Equals(TagLib.Asf.Guid.AsfHeaderObject))
            {
                throw new CorruptFileException("Object GUID incorrect.");
            }
            if (base.OriginalSize < 0x1aL)
            {
                throw new CorruptFileException("Object size too small.");
            }
            this.children = new List<TagLib.Asf.Object>();
            uint count = file.ReadDWord();
            this.reserved = file.ReadBlock(2);
            this.children.AddRange(file.ReadObjects(count, file.Tell));
        }

        public void AddObject(TagLib.Asf.Object obj)
        {
            this.children.Add(obj);
        }

        public void AddUniqueObject(TagLib.Asf.Object obj)
        {
            for (int i = 0; i < this.children.Count; i++)
            {
                if (this.children[i].Guid == obj.Guid)
                {
                    this.children[i] = obj;
                    return;
                }
            }
            this.children.Add(obj);
        }

        public void RemoveContentDescriptors()
        {
            for (int i = this.children.Count - 1; i >= 0; i--)
            {
                if ((this.children[i].Guid == TagLib.Asf.Guid.AsfContentDescriptionObject) || (this.children[i].Guid == TagLib.Asf.Guid.AsfExtendedContentDescriptionObject))
                {
                    this.children.RemoveAt(i);
                }
            }
        }

        public override ByteVector Render()
        {
            ByteVector data = new ByteVector();
            uint num = 0;
            foreach (TagLib.Asf.Object obj2 in this.children)
            {
                if (obj2.Guid != TagLib.Asf.Guid.AsfPaddingObject)
                {
                    data.Add(obj2.Render());
                    num++;
                }
            }
            long num2 = (data.Count + 30L) - ((long) base.OriginalSize);
            if (num2 != 0)
            {
                data.Add(new PaddingObject((num2 <= 0L) ? ((uint) -num2) : ((uint) 0x1000L)).Render());
                num++;
            }
            data.Insert(0, this.reserved);
            data.Insert(0, TagLib.Asf.Object.RenderDWord(num));
            return base.Render(data);
        }

        public IEnumerable<TagLib.Asf.Object> Children
        {
            get
            {
                return this.children;
            }
        }

        public HeaderExtensionObject Extension
        {
            get
            {
                foreach (TagLib.Asf.Object obj2 in this.children)
                {
                    if (obj2 is HeaderExtensionObject)
                    {
                        return (obj2 as HeaderExtensionObject);
                    }
                }
                return null;
            }
        }

        public bool HasContentDescriptors
        {
            get
            {
                foreach (TagLib.Asf.Object obj2 in this.children)
                {
                    if ((obj2.Guid == TagLib.Asf.Guid.AsfContentDescriptionObject) || (obj2.Guid == TagLib.Asf.Guid.AsfExtendedContentDescriptionObject))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public TagLib.Properties Properties
        {
            get
            {
                TimeSpan zero = TimeSpan.Zero;
                List<ICodec> codecs = new List<ICodec>();
                IEnumerator<TagLib.Asf.Object> enumerator = this.Children.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        TagLib.Asf.Object current = enumerator.Current;
                        FilePropertiesObject obj3 = current as FilePropertiesObject;
                        if (obj3 != null)
                        {
                            zero = obj3.PlayDuration - new TimeSpan((long) obj3.Preroll);
                        }
                        else
                        {
                            StreamPropertiesObject obj4 = current as StreamPropertiesObject;
                            if (obj4 != null)
                            {
                                codecs.Add(obj4.Codec);
                            }
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
                return new TagLib.Properties(zero, codecs);
            }
        }
    }
}

