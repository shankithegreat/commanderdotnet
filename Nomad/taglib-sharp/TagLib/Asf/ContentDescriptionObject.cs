namespace TagLib.Asf
{
    using System;
    using TagLib;

    public class ContentDescriptionObject : TagLib.Asf.Object
    {
        private string author;
        private string copyright;
        private string description;
        private string rating;
        private string title;

        public ContentDescriptionObject() : base(TagLib.Asf.Guid.AsfContentDescriptionObject)
        {
            this.title = string.Empty;
            this.author = string.Empty;
            this.copyright = string.Empty;
            this.description = string.Empty;
            this.rating = string.Empty;
        }

        public ContentDescriptionObject(TagLib.Asf.File file, long position) : base(file, position)
        {
            this.title = string.Empty;
            this.author = string.Empty;
            this.copyright = string.Empty;
            this.description = string.Empty;
            this.rating = string.Empty;
            if (base.Guid != TagLib.Asf.Guid.AsfContentDescriptionObject)
            {
                throw new CorruptFileException("Object GUID incorrect.");
            }
            if (base.OriginalSize < 0x22L)
            {
                throw new CorruptFileException("Object size too small.");
            }
            ushort length = file.ReadWord();
            ushort num2 = file.ReadWord();
            ushort num3 = file.ReadWord();
            ushort num4 = file.ReadWord();
            ushort num5 = file.ReadWord();
            this.title = file.ReadUnicode(length);
            this.author = file.ReadUnicode(num2);
            this.copyright = file.ReadUnicode(num3);
            this.description = file.ReadUnicode(num4);
            this.rating = file.ReadUnicode(num5);
        }

        public override ByteVector Render()
        {
            ByteVector data = TagLib.Asf.Object.RenderUnicode(this.title);
            ByteVector vector2 = TagLib.Asf.Object.RenderUnicode(this.author);
            ByteVector vector3 = TagLib.Asf.Object.RenderUnicode(this.copyright);
            ByteVector vector4 = TagLib.Asf.Object.RenderUnicode(this.description);
            ByteVector vector5 = TagLib.Asf.Object.RenderUnicode(this.rating);
            ByteVector vector6 = TagLib.Asf.Object.RenderWord((ushort) data.Count);
            vector6.Add(TagLib.Asf.Object.RenderWord((ushort) vector2.Count));
            vector6.Add(TagLib.Asf.Object.RenderWord((ushort) vector3.Count));
            vector6.Add(TagLib.Asf.Object.RenderWord((ushort) vector4.Count));
            vector6.Add(TagLib.Asf.Object.RenderWord((ushort) vector5.Count));
            vector6.Add(data);
            vector6.Add(vector2);
            vector6.Add(vector3);
            vector6.Add(vector4);
            vector6.Add(vector5);
            return base.Render(vector6);
        }

        public string Author
        {
            get
            {
                return ((this.author.Length != 0) ? this.author : null);
            }
            set
            {
                this.author = !string.IsNullOrEmpty(value) ? value : string.Empty;
            }
        }

        public string Copyright
        {
            get
            {
                return ((this.copyright.Length != 0) ? this.copyright : null);
            }
            set
            {
                this.copyright = !string.IsNullOrEmpty(value) ? value : string.Empty;
            }
        }

        public string Description
        {
            get
            {
                return ((this.description.Length != 0) ? this.description : null);
            }
            set
            {
                this.description = !string.IsNullOrEmpty(value) ? value : string.Empty;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return ((((this.title.Length == 0) && (this.author.Length == 0)) && ((this.copyright.Length == 0) && (this.description.Length == 0))) && (this.rating.Length == 0));
            }
        }

        public string Rating
        {
            get
            {
                return ((this.rating.Length != 0) ? this.rating : null);
            }
            set
            {
                this.rating = !string.IsNullOrEmpty(value) ? value : string.Empty;
            }
        }

        public string Title
        {
            get
            {
                return ((this.title.Length != 0) ? this.title : null);
            }
            set
            {
                this.title = !string.IsNullOrEmpty(value) ? value : string.Empty;
            }
        }
    }
}

