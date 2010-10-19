namespace TagLib
{
    using System;

    public class Picture : IPicture
    {
        private ByteVector data;
        private string description;
        private string mime_type;
        private PictureType type;

        public Picture()
        {
        }

        public Picture(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            this.Data = ByteVector.FromPath(path);
            this.FillInMimeFromData();
            this.Description = path;
        }

        public Picture(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.Data = new ByteVector(data);
            this.FillInMimeFromData();
        }

        public Picture(File.IFileAbstraction abstraction)
        {
            if (abstraction == null)
            {
                throw new ArgumentNullException("abstraction");
            }
            this.Data = ByteVector.FromFile(abstraction);
            this.FillInMimeFromData();
            this.Description = abstraction.Name;
        }

        [Obsolete("Use Picture(File.IFileAbstraction abstraction) constructor instead.")]
        public static Picture CreateFromFile(File.IFileAbstraction abstraction)
        {
            return new Picture(abstraction);
        }

        [Obsolete("Use Picture(string filename) constructor instead.")]
        public static Picture CreateFromPath(string filename)
        {
            return new Picture(filename);
        }

        private void FillInMimeFromData()
        {
            string str = "image/jpeg";
            string str2 = "jpg";
            if (((this.Data.Count >= 4) && (this.Data[1] == 80)) && ((this.Data[2] == 0x4e) && (this.Data[3] == 0x47)))
            {
                str = "image/png";
                str2 = "png";
            }
            else if (((this.Data.Count >= 3) && (this.Data[0] == 0x47)) && ((this.Data[1] == 0x49) && (this.Data[2] == 70)))
            {
                str = "image/gif";
                str2 = "gif";
            }
            else if (((this.Data.Count >= 2) && (this.Data[0] == 0x42)) && (this.Data[1] == 0x4d))
            {
                str = "image/bmp";
                str2 = "bmp";
            }
            this.MimeType = str;
            this.Type = PictureType.FrontCover;
            this.Description = "cover." + str2;
        }

        public ByteVector Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
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
    }
}

