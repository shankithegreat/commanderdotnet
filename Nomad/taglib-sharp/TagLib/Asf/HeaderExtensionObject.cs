namespace TagLib.Asf
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class HeaderExtensionObject : TagLib.Asf.Object
    {
        private List<TagLib.Asf.Object> children;

        public HeaderExtensionObject(TagLib.Asf.File file, long position) : base(file, position)
        {
            this.children = new List<TagLib.Asf.Object>();
            if (!base.Guid.Equals(TagLib.Asf.Guid.AsfHeaderExtensionObject))
            {
                throw new CorruptFileException("Object GUID incorrect.");
            }
            if (file.ReadGuid() != TagLib.Asf.Guid.AsfReserved1)
            {
                throw new CorruptFileException("Reserved1 GUID expected.");
            }
            if (file.ReadWord() != 6)
            {
                throw new CorruptFileException("Invalid reserved WORD. Expected '6'.");
            }
            uint num = file.ReadDWord();
            position += 0x2eL;
            while (num > 0)
            {
                TagLib.Asf.Object item = file.ReadObject(position);
                position += (long) item.OriginalSize;
                num -= (uint) item.OriginalSize;
                this.children.Add(item);
            }
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

        public override ByteVector Render()
        {
            ByteVector data = new ByteVector();
            foreach (TagLib.Asf.Object obj2 in this.children)
            {
                data.Add(obj2.Render());
            }
            data.Insert(0, TagLib.Asf.Object.RenderDWord((uint) data.Count));
            data.Insert(0, TagLib.Asf.Object.RenderWord(6));
            data.Insert(0, TagLib.Asf.Guid.AsfReserved1.ToByteArray());
            return base.Render(data);
        }

        public IEnumerable<TagLib.Asf.Object> Children
        {
            get
            {
                return this.children;
            }
        }
    }
}

