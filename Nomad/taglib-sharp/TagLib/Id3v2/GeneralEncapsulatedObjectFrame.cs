namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using TagLib;

    public class GeneralEncapsulatedObjectFrame : Frame
    {
        private ByteVector data;
        private string description;
        private StringType encoding;
        private string file_name;
        private string mime_type;

        public GeneralEncapsulatedObjectFrame() : base(FrameType.GEOB, 4)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
        }

        public GeneralEncapsulatedObjectFrame(ByteVector data, byte version) : base(data, version)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            base.SetData(data, 0, version, true);
        }

        protected internal GeneralEncapsulatedObjectFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(header)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            base.SetData(data, offset, version, false);
        }

        public override Frame Clone()
        {
            GeneralEncapsulatedObjectFrame frame = new GeneralEncapsulatedObjectFrame {
                encoding = this.encoding,
                mime_type = this.mime_type,
                file_name = this.file_name,
                description = this.description
            };
            if (this.data != null)
            {
                frame.data = new ByteVector(this.data);
            }
            return frame;
        }

        public static GeneralEncapsulatedObjectFrame Get(TagLib.Id3v2.Tag tag, string description, bool create)
        {
            GeneralEncapsulatedObjectFrame frame;
            IEnumerator<Frame> enumerator = tag.GetFrames(FrameType.GEOB).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    frame = current as GeneralEncapsulatedObjectFrame;
                    if ((frame != null) && (frame.Description == description))
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
            frame = new GeneralEncapsulatedObjectFrame {
                Description = description
            };
            tag.AddFrame(frame);
            return frame;
        }

        protected override void ParseFields(ByteVector data, byte version)
        {
            if (data.Count < 4)
            {
                throw new CorruptFileException("An object frame must contain at least 4 bytes.");
            }
            int offset = 0;
            this.encoding = (StringType) data[offset++];
            int num2 = data.Find(ByteVector.TextDelimiter(StringType.Latin1), offset);
            if (num2 >= offset)
            {
                this.mime_type = data.ToString(StringType.Latin1, offset, num2 - offset);
                ByteVector pattern = ByteVector.TextDelimiter(this.encoding);
                offset = num2 + 1;
                num2 = data.Find(pattern, offset, pattern.Count);
                if (num2 >= offset)
                {
                    this.file_name = data.ToString(this.encoding, offset, num2 - offset);
                    offset = num2 + pattern.Count;
                    num2 = data.Find(pattern, offset, pattern.Count);
                    if (num2 >= offset)
                    {
                        this.description = data.ToString(this.encoding, offset, num2 - offset);
                        offset = num2 + pattern.Count;
                        data.RemoveRange(0, offset);
                        this.data = data;
                    }
                }
            }
        }

        protected override ByteVector RenderFields(byte version)
        {
            StringType type = Frame.CorrectEncoding(this.encoding, version);
            ByteVector vector = new ByteVector {
                (byte) type
            };
            if (this.MimeType != null)
            {
                vector.Add(ByteVector.FromString(this.MimeType, StringType.Latin1));
            }
            vector.Add(ByteVector.TextDelimiter(StringType.Latin1));
            if (this.FileName != null)
            {
                vector.Add(ByteVector.FromString(this.FileName, type));
            }
            vector.Add(ByteVector.TextDelimiter(type));
            if (this.Description != null)
            {
                vector.Add(ByteVector.FromString(this.Description, type));
            }
            vector.Add(ByteVector.TextDelimiter(type));
            vector.Add(this.data);
            return vector;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (this.Description.Length == 0)
            {
                builder.Append(this.Description);
                builder.Append(" ");
            }
            object[] args = new object[] { this.MimeType, this.Object.Count };
            builder.AppendFormat(CultureInfo.InvariantCulture, "[{0}] {1} bytes", args);
            return builder.ToString();
        }

        public string Description
        {
            get
            {
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

        public string FileName
        {
            get
            {
                if (this.file_name != null)
                {
                    return this.file_name;
                }
                return string.Empty;
            }
            set
            {
                this.file_name = value;
            }
        }

        public string MimeType
        {
            get
            {
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

        public ByteVector Object
        {
            get
            {
                return ((this.data == null) ? new ByteVector() : this.data);
            }
            set
            {
                this.data = value;
            }
        }

        public StringType TextEncoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                this.encoding = value;
            }
        }
    }
}

