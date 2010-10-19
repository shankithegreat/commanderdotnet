namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using TagLib;

    public class AttachedPictureFrame : Frame, IPicture
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map0;
        private ByteVector data;
        private string description;
        private string mime_type;
        private ByteVector raw_data;
        private byte raw_version;
        private StringType text_encoding;
        private PictureType type;

        public AttachedPictureFrame() : base(FrameType.APIC, 4)
        {
            this.text_encoding = TagLib.Id3v2.Tag.DefaultEncoding;
        }

        public AttachedPictureFrame(IPicture picture) : base(FrameType.APIC, 4)
        {
            this.text_encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            if (picture == null)
            {
                throw new ArgumentNullException("picture");
            }
            this.mime_type = picture.MimeType;
            this.type = picture.Type;
            this.description = picture.Description;
            this.data = picture.Data;
        }

        public AttachedPictureFrame(ByteVector data, byte version) : base(data, version)
        {
            this.text_encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            base.SetData(data, 0, version, true);
        }

        protected internal AttachedPictureFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(header)
        {
            this.text_encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            base.SetData(data, offset, version, false);
        }

        public override Frame Clone()
        {
            AttachedPictureFrame frame = new AttachedPictureFrame {
                text_encoding = this.text_encoding,
                mime_type = this.mime_type,
                type = this.type,
                description = this.description
            };
            if (this.data != null)
            {
                frame.data = new ByteVector(this.data);
            }
            if (this.raw_data != null)
            {
                frame.data = new ByteVector(this.raw_data);
            }
            frame.raw_version = this.raw_version;
            return frame;
        }

        public static AttachedPictureFrame Get(TagLib.Id3v2.Tag tag, string description, bool create)
        {
            return Get(tag, description, PictureType.Other, create);
        }

        public static AttachedPictureFrame Get(TagLib.Id3v2.Tag tag, PictureType type, bool create)
        {
            return Get(tag, null, type, create);
        }

        public static AttachedPictureFrame Get(TagLib.Id3v2.Tag tag, string description, PictureType type, bool create)
        {
            AttachedPictureFrame frame;
            IEnumerator<Frame> enumerator = tag.GetFrames(FrameType.APIC).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    frame = current as AttachedPictureFrame;
                    if (((frame != null) && ((description == null) || (frame.Description == description))) && ((type == PictureType.Other) || (frame.Type == type)))
                    {
                        return frame;
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
            if (!create)
            {
                return null;
            }
            frame = new AttachedPictureFrame {
                Description = description,
                Type = type
            };
            tag.AddFrame(frame);
            return frame;
        }

        protected override void ParseFields(ByteVector data, byte version)
        {
            if (data.Count < 5)
            {
                throw new CorruptFileException("A picture frame must contain at least 5 bytes.");
            }
            this.raw_data = data;
            this.raw_version = version;
        }

        protected void ParseRawData()
        {
            if (this.raw_data != null)
            {
                int num2;
                int offset = 0;
                this.text_encoding = (StringType) this.raw_data[offset++];
                if (this.raw_version > 2)
                {
                    num2 = this.raw_data.Find(ByteVector.TextDelimiter(StringType.Latin1), offset);
                    if (num2 < offset)
                    {
                        return;
                    }
                    this.mime_type = this.raw_data.ToString(StringType.Latin1, offset, num2 - offset);
                    offset = num2 + 1;
                }
                else
                {
                    ByteVector vector = this.raw_data.Mid(offset, 3);
                    if (vector == "JPG")
                    {
                        this.mime_type = "image/jpeg";
                    }
                    else if (vector == "PNG")
                    {
                        this.mime_type = "image/png";
                    }
                    else
                    {
                        this.mime_type = "image/unknown";
                    }
                    offset += 3;
                }
                ByteVector pattern = ByteVector.TextDelimiter(this.text_encoding);
                this.type = (PictureType) this.raw_data[offset++];
                num2 = this.raw_data.Find(pattern, offset, pattern.Count);
                if (num2 >= offset)
                {
                    this.description = this.raw_data.ToString(this.text_encoding, offset, num2 - offset);
                    offset = num2 + pattern.Count;
                    this.raw_data.RemoveRange(0, offset);
                    this.data = this.raw_data;
                    this.raw_data = null;
                }
            }
        }

        protected override ByteVector RenderFields(byte version)
        {
            if ((this.raw_data != null) && (this.raw_version == version))
            {
                return this.raw_data;
            }
            StringType type = Frame.CorrectEncoding(this.TextEncoding, version);
            ByteVector vector = new ByteVector {
                (byte) type
            };
            if (version == 2)
            {
                string mimeType = this.MimeType;
                if (mimeType != null)
                {
                    int num;
                    if (<>f__switch$map0 == null)
                    {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
                        dictionary.Add("image/png", 0);
                        dictionary.Add("image/jpeg", 1);
                        <>f__switch$map0 = dictionary;
                    }
                    if (<>f__switch$map0.TryGetValue(mimeType, out num))
                    {
                        if (num == 0)
                        {
                            vector.Add("PNG");
                            goto Label_010A;
                        }
                        if (num == 1)
                        {
                            vector.Add("JPG");
                            goto Label_010A;
                        }
                    }
                }
                vector.Add("XXX");
            }
            else
            {
                vector.Add(ByteVector.FromString(this.MimeType, StringType.Latin1));
                vector.Add(ByteVector.TextDelimiter(StringType.Latin1));
            }
        Label_010A:
            vector.Add((byte) this.type);
            vector.Add(ByteVector.FromString(this.Description, type));
            vector.Add(ByteVector.TextDelimiter(type));
            vector.Add(this.data);
            return vector;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(this.Description))
            {
                builder.Append(this.Description);
                builder.Append(" ");
            }
            object[] args = new object[] { this.MimeType, this.Data.Count };
            builder.AppendFormat(CultureInfo.InvariantCulture, "[{0}] {1} bytes", args);
            return builder.ToString();
        }

        public ByteVector Data
        {
            get
            {
                this.ParseRawData();
                return ((this.data == null) ? new ByteVector() : this.data);
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
                this.ParseRawData();
                if (this.description != null)
                {
                    return this.description;
                }
                return string.Empty;
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
                this.ParseRawData();
                if (this.mime_type != null)
                {
                    return this.mime_type;
                }
                return string.Empty;
            }
            set
            {
                this.mime_type = value;
            }
        }

        public StringType TextEncoding
        {
            get
            {
                this.ParseRawData();
                return this.text_encoding;
            }
            set
            {
                this.text_encoding = value;
            }
        }

        public PictureType Type
        {
            get
            {
                this.ParseRawData();
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }
}

