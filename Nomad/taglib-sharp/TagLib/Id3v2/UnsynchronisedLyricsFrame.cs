namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class UnsynchronisedLyricsFrame : Frame
    {
        private string description;
        private StringType encoding;
        private string language;
        private string text;

        public UnsynchronisedLyricsFrame(string description) : this(description, null)
        {
        }

        public UnsynchronisedLyricsFrame(string description, string language) : this(description, language, TagLib.Id3v2.Tag.DefaultEncoding)
        {
        }

        public UnsynchronisedLyricsFrame(ByteVector data, byte version) : base(data, version)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            base.SetData(data, 0, version, true);
        }

        public UnsynchronisedLyricsFrame(string description, string language, StringType encoding) : base(FrameType.USLT, 4)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            this.encoding = encoding;
            this.language = language;
            this.description = description;
        }

        protected internal UnsynchronisedLyricsFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(header)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            base.SetData(data, offset, version, false);
        }

        public override Frame Clone()
        {
            return new UnsynchronisedLyricsFrame(this.description, this.language, this.encoding) { text = this.text };
        }

        public static UnsynchronisedLyricsFrame Get(TagLib.Id3v2.Tag tag, string description, string language, bool create)
        {
            UnsynchronisedLyricsFrame frame;
            IEnumerator<Frame> enumerator = tag.GetFrames(FrameType.USLT).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    frame = current as UnsynchronisedLyricsFrame;
                    if (((frame != null) && (frame.Description == description)) && ((language == null) || (language == frame.Language)))
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
            frame = new UnsynchronisedLyricsFrame(description, language);
            tag.AddFrame(frame);
            return frame;
        }

        public static UnsynchronisedLyricsFrame GetPreferred(TagLib.Id3v2.Tag tag, string description, string language)
        {
            int num = -1;
            UnsynchronisedLyricsFrame frame = null;
            IEnumerator<Frame> enumerator = tag.GetFrames(FrameType.USLT).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    UnsynchronisedLyricsFrame frame3 = current as UnsynchronisedLyricsFrame;
                    if (frame3 != null)
                    {
                        bool flag = frame3.Description == description;
                        bool flag2 = frame3.Language == language;
                        if (flag && flag2)
                        {
                            return frame3;
                        }
                        int num2 = !flag2 ? (!flag ? 0 : 1) : 2;
                        if (num2 > num)
                        {
                            num = num2;
                            frame = frame3;
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
            return frame;
        }

        protected override void ParseFields(ByteVector data, byte version)
        {
            if (data.Count < 4)
            {
                throw new CorruptFileException("Not enough bytes in field.");
            }
            this.encoding = (StringType) data[0];
            this.language = data.ToString(StringType.Latin1, 1, 3);
            string[] strArray = data.ToStrings(this.encoding, 4, 2);
            if (strArray.Length == 1)
            {
                this.description = string.Empty;
                this.text = strArray[0];
            }
            else
            {
                this.description = strArray[0];
                this.text = strArray[1];
            }
        }

        protected override ByteVector RenderFields(byte version)
        {
            StringType type = Frame.CorrectEncoding(this.TextEncoding, version);
            return new ByteVector { ((byte) type), ByteVector.FromString(this.Language, 0), ByteVector.FromString(this.description, type), ByteVector.TextDelimiter(type), ByteVector.FromString(this.text, type) };
        }

        public override string ToString()
        {
            return this.Text;
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

        public string Language
        {
            get
            {
                if ((this.language != null) && (this.language.Length > 2))
                {
                    return this.language.Substring(0, 3);
                }
                return "XXX";
            }
            set
            {
                this.language = value;
            }
        }

        public string Text
        {
            get
            {
                if (this.text != null)
                {
                    return this.text;
                }
                return string.Empty;
            }
            set
            {
                this.text = value;
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

