namespace TagLib.Id3v1
{
    using System;
    using System.Globalization;
    using TagLib;

    public class Tag : TagLib.Tag
    {
        private string album;
        private string artist;
        private string comment;
        public static readonly ReadOnlyByteVector FileIdentifier = "TAG";
        private byte genre;
        public const uint Size = 0x80;
        private static StringHandler string_handler = new StringHandler();
        private string title;
        private byte track;
        private string year;

        public Tag()
        {
            this.Clear();
        }

        public Tag(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Count < 0x80L)
            {
                throw new CorruptFileException("ID3v1 data is less than 128 bytes long.");
            }
            if (!data.StartsWith(FileIdentifier))
            {
                throw new CorruptFileException("ID3v1 data does not start with identifier.");
            }
            this.Parse(data);
        }

        public Tag(File file, long position)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            file.Mode = File.AccessMode.Read;
            if ((position < 0L) || (position > (file.Length - 0x80L)))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            file.Seek(position);
            ByteVector data = file.ReadBlock(0x80);
            if (!data.StartsWith(FileIdentifier))
            {
                throw new CorruptFileException("ID3v1 data does not start with identifier.");
            }
            this.Parse(data);
        }

        public override void Clear()
        {
            this.title = this.artist = this.album = this.year = (string) (this.comment = null);
            this.track = 0;
            this.genre = 0xff;
        }

        private void Parse(ByteVector data)
        {
            this.title = string_handler.Parse(data.Mid(3, 30));
            this.artist = string_handler.Parse(data.Mid(0x21, 30));
            this.album = string_handler.Parse(data.Mid(0x3f, 30));
            this.year = string_handler.Parse(data.Mid(0x5d, 4));
            if ((data[0x7d] == 0) && (data[0x7e] != 0))
            {
                this.comment = string_handler.Parse(data.Mid(0x61, 0x1c));
                this.track = data[0x7e];
            }
            else
            {
                this.comment = string_handler.Parse(data.Mid(0x61, 30));
            }
            this.genre = data[0x7f];
        }

        public ByteVector Render()
        {
            return new ByteVector { FileIdentifier, string_handler.Render(this.title).Resize(30), string_handler.Render(this.artist).Resize(30), string_handler.Render(this.album).Resize(30), string_handler.Render(this.year).Resize(4), string_handler.Render(this.comment).Resize(0x1c), 0, this.track, this.genre };
        }

        public override string Album
        {
            get
            {
                return (!string.IsNullOrEmpty(this.album) ? this.album : null);
            }
            set
            {
                this.album = (value == null) ? string.Empty : value.Trim();
            }
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

        public static StringHandler DefaultStringHandler
        {
            get
            {
                return string_handler;
            }
            set
            {
                string_handler = value;
            }
        }

        public override string[] Genres
        {
            get
            {
                string str = TagLib.Genres.IndexToAudio(this.genre);
                return ((str == null) ? new string[0] : new string[] { str });
            }
            set
            {
                this.genre = ((value != null) && (value.Length != 0)) ? TagLib.Genres.AudioToIndex(value[0].Trim()) : ((byte) 0xff);
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
                return (TagLib.TagTypes.None | TagLib.TagTypes.Id3v1);
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

        public override uint Track
        {
            get
            {
                return this.track;
            }
            set
            {
                this.track = (value >= 0x100) ? ((byte) 0) : ((byte) value);
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

