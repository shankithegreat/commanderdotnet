namespace TagLib.Flac
{
    using System;
    using TagLib;

    public class Picture : IPicture
    {
        private int color_depth;
        private string description;
        private int height;
        private int indexed_colors;
        private string mime_type;
        private ByteVector picture_data;
        private PictureType type;
        private int width;

        public Picture(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Count < 0x20)
            {
                throw new CorruptFileException("Data must be at least 32 bytes long");
            }
            int startIndex = 0;
            this.type = (PictureType) data.Mid(startIndex, 4).ToUInt();
            startIndex += 4;
            int count = (int) data.Mid(startIndex, 4).ToUInt();
            startIndex += 4;
            this.mime_type = data.ToString(StringType.Latin1, startIndex, count);
            startIndex += count;
            int num3 = (int) data.Mid(startIndex, 4).ToUInt();
            startIndex += 4;
            this.description = data.ToString(StringType.UTF8, startIndex, num3);
            startIndex += num3;
            this.width = (int) data.Mid(startIndex, 4).ToUInt();
            startIndex += 4;
            this.height = (int) data.Mid(startIndex, 4).ToUInt();
            startIndex += 4;
            this.color_depth = (int) data.Mid(startIndex, 4).ToUInt();
            startIndex += 4;
            this.indexed_colors = (int) data.Mid(startIndex, 4).ToUInt();
            startIndex += 4;
            int length = (int) data.Mid(startIndex, 4).ToUInt();
            startIndex += 4;
            this.picture_data = data.Mid(startIndex, length);
        }

        public Picture(IPicture picture)
        {
            if (picture == null)
            {
                throw new ArgumentNullException("picture");
            }
            this.type = picture.Type;
            this.mime_type = picture.MimeType;
            this.description = picture.Description;
            this.picture_data = picture.Data;
            TagLib.Flac.Picture picture2 = picture as TagLib.Flac.Picture;
            if (picture2 != null)
            {
                this.width = picture2.Width;
                this.height = picture2.Height;
                this.color_depth = picture2.ColorDepth;
                this.indexed_colors = picture2.IndexedColors;
            }
        }

        public ByteVector Render()
        {
            ByteVector vector = new ByteVector {
                ByteVector.FromUInt((uint) this.Type)
            };
            ByteVector data = ByteVector.FromString(this.MimeType, StringType.Latin1);
            vector.Add(ByteVector.FromUInt((uint) data.Count));
            vector.Add(data);
            ByteVector vector3 = ByteVector.FromString(this.Description, StringType.UTF8);
            vector.Add(ByteVector.FromUInt((uint) vector3.Count));
            vector.Add(vector3);
            vector.Add(ByteVector.FromUInt((uint) this.Width));
            vector.Add(ByteVector.FromUInt((uint) this.Height));
            vector.Add(ByteVector.FromUInt((uint) this.ColorDepth));
            vector.Add(ByteVector.FromUInt((uint) this.IndexedColors));
            vector.Add(ByteVector.FromUInt((uint) this.Data.Count));
            vector.Add(this.Data);
            return vector;
        }

        public int ColorDepth
        {
            get
            {
                return this.color_depth;
            }
            set
            {
                this.color_depth = value;
            }
        }

        public ByteVector Data
        {
            get
            {
                return this.picture_data;
            }
            set
            {
                this.picture_data = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        public int IndexedColors
        {
            get
            {
                return this.indexed_colors;
            }
            set
            {
                this.indexed_colors = value;
            }
        }

        public string MimeType
        {
            get
            {
                return this.mime_type;
            }
            set
            {
                this.mime_type = value;
            }
        }

        public PictureType Type
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

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }
    }
}

