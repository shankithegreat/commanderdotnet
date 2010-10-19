namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class SynchronisedLyricsFrame : Frame
    {
        private string description;
        private StringType encoding;
        private string language;
        private SynchedTextType lyrics_type;
        private SynchedText[] text;
        private TimestampFormat timestamp_format;

        public SynchronisedLyricsFrame(ByteVector data, byte version) : base(data, version)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            this.text = new SynchedText[0];
            base.SetData(data, 0, version, true);
        }

        public SynchronisedLyricsFrame(string description, string language, SynchedTextType type) : this(description, language, type, TagLib.Id3v2.Tag.DefaultEncoding)
        {
        }

        public SynchronisedLyricsFrame(string description, string language, SynchedTextType type, StringType encoding) : base(FrameType.SYLT, 4)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            this.text = new SynchedText[0];
            this.encoding = encoding;
            this.language = language;
            this.description = description;
            this.lyrics_type = type;
        }

        protected internal SynchronisedLyricsFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(header)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            this.text = new SynchedText[0];
            base.SetData(data, offset, version, false);
        }

        public override Frame Clone()
        {
            return new SynchronisedLyricsFrame(this.description, this.language, this.lyrics_type, this.encoding) { timestamp_format = this.timestamp_format, text = (SynchedText[]) this.text.Clone() };
        }

        public static SynchronisedLyricsFrame Get(TagLib.Id3v2.Tag tag, string description, string language, SynchedTextType type, bool create)
        {
            IEnumerator<Frame> enumerator = tag.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    SynchronisedLyricsFrame frame2 = current as SynchronisedLyricsFrame;
                    if (((frame2 != null) && ((frame2.Description == description) && ((language == null) || (language == frame2.Language)))) && (type == frame2.Type))
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
            SynchronisedLyricsFrame frame3 = new SynchronisedLyricsFrame(description, language, type);
            tag.AddFrame(frame3);
            return frame3;
        }

        public static SynchronisedLyricsFrame GetPreferred(TagLib.Id3v2.Tag tag, string description, string language, SynchedTextType type)
        {
            int num = -1;
            SynchronisedLyricsFrame frame = null;
            IEnumerator<Frame> enumerator = tag.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    SynchronisedLyricsFrame frame3 = current as SynchronisedLyricsFrame;
                    if (frame3 != null)
                    {
                        int num2 = 0;
                        if (frame3.Language == language)
                        {
                            num2 += 4;
                        }
                        if (frame3.Description == description)
                        {
                            num2 += 2;
                        }
                        if (frame3.Type == type)
                        {
                            num2++;
                        }
                        if (num2 == 7)
                        {
                            return frame3;
                        }
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
            if (data.Count < 6)
            {
                throw new CorruptFileException("Not enough bytes in field.");
            }
            this.encoding = (StringType) data[0];
            this.language = data.ToString(StringType.Latin1, 1, 3);
            this.timestamp_format = (TimestampFormat) data[4];
            this.lyrics_type = (SynchedTextType) data[5];
            ByteVector pattern = ByteVector.TextDelimiter(this.encoding);
            int num = data.Find(pattern, 6, pattern.Count);
            if (num < 0)
            {
                throw new CorruptFileException("Text delimiter expected.");
            }
            this.description = data.ToString(this.encoding, 6, num - 6);
            int offset = num + pattern.Count;
            List<SynchedText> list = new List<SynchedText>();
            while (((offset + pattern.Count) + 4) < data.Count)
            {
                num = data.Find(pattern, offset, pattern.Count);
                if (num < offset)
                {
                    throw new CorruptFileException("Text delimiter expected.");
                }
                string text = data.ToString(this.encoding, offset, num - offset);
                offset = num + pattern.Count;
                if ((offset + 4) > data.Count)
                {
                    break;
                }
                list.Add(new SynchedText((long) data.Mid(offset, 4).ToUInt(), text));
                offset += 4;
            }
            this.text = list.ToArray();
        }

        protected override ByteVector RenderFields(byte version)
        {
            StringType type = Frame.CorrectEncoding(this.TextEncoding, version);
            ByteVector data = ByteVector.TextDelimiter(type);
            ByteVector vector2 = new ByteVector {
                (byte) type,
                ByteVector.FromString(this.Language, 0),
                (byte) this.timestamp_format,
                (byte) this.lyrics_type,
                ByteVector.FromString(this.description, type),
                data
            };
            foreach (SynchedText text in this.text)
            {
                vector2.Add(ByteVector.FromString(text.Text, type));
                vector2.Add(data);
                vector2.Add(ByteVector.FromUInt((uint) text.Time));
            }
            return vector2;
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

        public TimestampFormat Format
        {
            get
            {
                return this.timestamp_format;
            }
            set
            {
                this.timestamp_format = value;
            }
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

        public SynchedText[] Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = (value != null) ? value : new SynchedText[0];
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

        public SynchedTextType Type
        {
            get
            {
                return this.lyrics_type;
            }
            set
            {
                this.lyrics_type = value;
            }
        }
    }
}

