namespace TagLib.Riff
{
    using System;
    using System.Globalization;
    using TagLib;

    public class DivXTag : Tag
    {
        private string artist;
        private string comment;
        private ByteVector extra_data;
        public static readonly ReadOnlyByteVector FileIdentifier = "DIVXTAG";
        private string genre;
        public const uint Size = 0x80;
        private string title;
        private string year;

        public DivXTag()
        {
            this.Clear();
        }

        public DivXTag(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Count < 0x80L)
            {
                throw new CorruptFileException("DivX tag data is less than 128 bytes long.");
            }
            if (!data.EndsWith(FileIdentifier))
            {
                throw new CorruptFileException("DivX tag data does not end with identifier.");
            }
            this.Parse(data);
        }

        public DivXTag(TagLib.Riff.File file, long position)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            file.Mode = TagLib.File.AccessMode.Read;
            if ((position < 0L) || (position > (file.Length - 0x80L)))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            file.Seek(position);
            ByteVector data = file.ReadBlock(0x80);
            if (!data.EndsWith(FileIdentifier))
            {
                throw new CorruptFileException("DivX tag data does not end with identifier.");
            }
            this.Parse(data);
        }

        public override void Clear()
        {
            this.title = this.artist = this.genre = this.year = this.comment = string.Empty;
            this.extra_data = new ByteVector(6);
        }

        private void Parse(ByteVector data)
        {
            this.title = data.ToString(StringType.Latin1, 0, 0x20).Trim();
            this.artist = data.ToString(StringType.Latin1, 0x20, 0x1c).Trim();
            this.year = data.ToString(StringType.Latin1, 60, 4).Trim();
            this.comment = data.ToString(StringType.Latin1, 0x40, 0x30).Trim();
            this.genre = data.ToString(StringType.Latin1, 0x70, 3).Trim();
            this.extra_data = data.Mid(0x73, 6);
        }

        public ByteVector Render()
        {
            return new ByteVector { ByteVector.FromString(this.title, 0).Resize(0x20, 0x20), ByteVector.FromString(this.artist, 0).Resize(0x1c, 0x20), ByteVector.FromString(this.year, 0).Resize(4, 0x20), ByteVector.FromString(this.comment, 0).Resize(0x30, 0x20), ByteVector.FromString(this.genre, 0).Resize(3, 0x20), this.extra_data, FileIdentifier };
        }

        public override string Comment
        {
            get
            {
                return (!string.IsNullOrEmpty(this.comment) ? this.comment : null);
            }
            set
            {
                this.comment = (value == null) ? string.Empty : value.Trim();
            }
        }

        public override string[] Genres
        {
            get
            {
                string str = TagLib.Genres.IndexToVideo(this.genre);
                return ((str == null) ? new string[0] : new string[] { str });
            }
            set
            {
                this.genre = ((value == null) || (value.Length <= 0)) ? string.Empty : TagLib.Genres.VideoToIndex(value[0].Trim()).ToString(CultureInfo.InvariantCulture);
            }
        }

        public override string[] Performers
        {
            get
            {
                return (!string.IsNullOrEmpty(this.artist) ? this.artist.Split(new char[] { ';' }) : new string[0]);
            }
            set
            {
                this.artist = (value == null) ? string.Empty : string.Join(";", value);
            }
        }

        public override TagLib.TagTypes TagTypes
        {
            get
            {
                return TagLib.TagTypes.DivX;
            }
        }

        public override string Title
        {
            get
            {
                return (!string.IsNullOrEmpty(this.title) ? this.title : null);
            }
            set
            {
                this.title = (value == null) ? string.Empty : value.Trim();
            }
        }

        public override uint Year
        {
            get
            {
                uint num;
                return (!uint.TryParse(this.year, NumberStyles.Integer, CultureInfo.InvariantCulture, out num) ? 0 : num);
            }
            set
            {
                this.year = ((value <= 0) || (value >= 0x2710)) ? string.Empty : value.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}

