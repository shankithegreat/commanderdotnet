namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class TermsOfUseFrame : Frame
    {
        private StringType encoding;
        private string language;
        private string text;

        public TermsOfUseFrame(string language) : base(FrameType.USER, 4)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            this.language = language;
        }

        public TermsOfUseFrame(string language, StringType encoding) : base(FrameType.USER, 4)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            this.encoding = encoding;
            this.language = language;
        }

        public TermsOfUseFrame(ByteVector data, byte version) : base(data, version)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            base.SetData(data, 0, version, true);
        }

        protected internal TermsOfUseFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(header)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            base.SetData(data, offset, version, false);
        }

        public override Frame Clone()
        {
            return new TermsOfUseFrame(this.language, this.encoding) { text = this.text };
        }

        public static TermsOfUseFrame Get(TagLib.Id3v2.Tag tag, string language, bool create)
        {
            IEnumerator<Frame> enumerator = tag.GetFrames(FrameType.USER).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    TermsOfUseFrame frame2 = current as TermsOfUseFrame;
                    if ((frame2 != null) && ((language == null) || (language == frame2.Language)))
                    {
                        return frame2;
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
            TermsOfUseFrame frame3 = new TermsOfUseFrame(language);
            tag.AddFrame(frame3);
            return frame3;
        }

        public static TermsOfUseFrame GetPreferred(TagLib.Id3v2.Tag tag, string language)
        {
            TermsOfUseFrame frame = null;
            IEnumerator<Frame> enumerator = tag.GetFrames(FrameType.USER).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    TermsOfUseFrame frame3 = current as TermsOfUseFrame;
                    if (frame3 != null)
                    {
                        if (frame3.Language == language)
                        {
                            return frame3;
                        }
                        if (frame == null)
                        {
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
            this.text = data.ToString(this.encoding, 4, data.Count - 4);
        }

        protected override ByteVector RenderFields(byte version)
        {
            StringType type = Frame.CorrectEncoding(this.TextEncoding, version);
            return new ByteVector { ((byte) type), ByteVector.FromString(this.Language, 0), ByteVector.FromString(this.text, type) };
        }

        public override string ToString()
        {
            return this.text;
        }

        public string Language
        {
            get
            {
                return (((this.language == null) || (this.language.Length <= 2)) ? "XXX" : this.language.Substring(0, 3));
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
                return this.text;
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

