namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using TagLib;

    public class TextInformationFrame : Frame
    {
        private StringType encoding;
        private ByteVector raw_data;
        private byte raw_version;
        private string[] text_fields;

        public TextInformationFrame(ByteVector ident) : this(ident, TagLib.Id3v2.Tag.DefaultEncoding)
        {
        }

        public TextInformationFrame(ByteVector data, byte version) : base(data, version)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            this.text_fields = new string[0];
            base.SetData(data, 0, version, true);
        }

        public TextInformationFrame(ByteVector ident, StringType encoding) : base(ident, 4)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            this.text_fields = new string[0];
            this.encoding = encoding;
        }

        protected internal TextInformationFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(header)
        {
            this.encoding = TagLib.Id3v2.Tag.DefaultEncoding;
            this.text_fields = new string[0];
            base.SetData(data, offset, version, false);
        }

        public override Frame Clone()
        {
            TextInformationFrame frame = !(this is UserTextInformationFrame) ? new TextInformationFrame(base.FrameId, this.encoding) : new UserTextInformationFrame(null, this.encoding);
            frame.text_fields = (string[]) this.text_fields.Clone();
            if (this.raw_data != null)
            {
                frame.raw_data = new ByteVector(this.raw_data);
            }
            frame.raw_version = this.raw_version;
            return frame;
        }

        [Obsolete("Use TextInformationFrame.Get(Tag,ByteVector,bool)")]
        public static TextInformationFrame Get(TagLib.Id3v2.Tag tag, ByteVector ident)
        {
            return Get(tag, ident, false);
        }

        public static TextInformationFrame Get(TagLib.Id3v2.Tag tag, ByteVector ident, bool create)
        {
            return Get(tag, ident, TagLib.Id3v2.Tag.DefaultEncoding, create);
        }

        public static TextInformationFrame Get(TagLib.Id3v2.Tag tag, ByteVector ident, StringType encoding, bool create)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }
            if (ident == null)
            {
                throw new ArgumentNullException("ident");
            }
            if (ident.Count != 4)
            {
                throw new ArgumentException("Identifier must be four bytes long.", "ident");
            }
            IEnumerator<TextInformationFrame> enumerator = tag.GetFrames<TextInformationFrame>(ident).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    return enumerator.Current;
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
            TextInformationFrame frame = new TextInformationFrame(ident, encoding);
            tag.AddFrame(frame);
            return frame;
        }

        protected override void ParseFields(ByteVector data, byte version)
        {
            this.raw_data = data;
            this.raw_version = version;
        }

        protected void ParseRawData()
        {
            if (this.raw_data != null)
            {
                ByteVector vector = this.raw_data;
                this.raw_data = null;
                this.encoding = (StringType) vector[0];
                List<string> list = new List<string>();
                ByteVector other = ByteVector.TextDelimiter(this.encoding);
                if ((this.raw_version > 3) || (base.FrameId == FrameType.TXXX))
                {
                    list.AddRange(vector.ToStrings(this.encoding, 1));
                }
                else if ((vector.Count > 1) && !vector.Mid(1, other.Count).Equals(other))
                {
                    string item = vector.ToString(this.encoding, 1, vector.Count - 1);
                    int index = item.IndexOf('\0');
                    if (index >= 0)
                    {
                        item = item.Substring(0, index);
                    }
                    if ((((base.FrameId == FrameType.TCOM) || (base.FrameId == FrameType.TEXT)) || ((base.FrameId == FrameType.TOLY) || (base.FrameId == FrameType.TOPE))) || (((base.FrameId == FrameType.TPE1) || (base.FrameId == FrameType.TPE2)) || ((base.FrameId == FrameType.TPE3) || (base.FrameId == FrameType.TPE4))))
                    {
                        char[] separator = new char[] { '/' };
                        list.AddRange(item.Split(separator));
                    }
                    else if (base.FrameId != FrameType.TCON)
                    {
                        list.Add(item);
                    }
                    else
                    {
                        while ((item.Length > 1) && (item[0] == '('))
                        {
                            int num2 = item.IndexOf(')');
                            if (num2 < 0)
                            {
                                break;
                            }
                            string str2 = item.Substring(1, num2 - 1);
                            list.Add(str2);
                            char[] trimChars = new char[] { '/', ' ' };
                            item = item.Substring(num2 + 1).TrimStart(trimChars);
                            string str3 = Genres.IndexToAudio(str2);
                            if ((str3 != null) && item.StartsWith(str3))
                            {
                                char[] chArray3 = new char[] { '/', ' ' };
                                item = item.Substring(str3.Length).TrimStart(chArray3);
                            }
                        }
                        if (item.Length > 0)
                        {
                            char[] chArray4 = new char[] { '/' };
                            list.AddRange(item.Split(chArray4));
                        }
                    }
                }
                while ((list.Count != 0) && string.IsNullOrEmpty(list[list.Count - 1]))
                {
                    list.RemoveAt(list.Count - 1);
                }
                this.text_fields = list.ToArray();
            }
        }

        public override ByteVector Render(byte version)
        {
            if ((version != 3) || (base.FrameId != FrameType.TDRC))
            {
                return base.Render(version);
            }
            string str = this.ToString();
            if (((str.Length < 10) || (str[4] != '-')) || (str[7] != '-'))
            {
                return base.Render(version);
            }
            ByteVector vector = new ByteVector();
            TextInformationFrame frame = new TextInformationFrame(FrameType.TYER, this.encoding);
            frame.Text = new string[] { str.Substring(0, 4) };
            vector.Add(frame.Render(version));
            frame = new TextInformationFrame(FrameType.TDAT, this.encoding);
            frame.Text = new string[] { str.Substring(5, 2) + str.Substring(8, 2) };
            vector.Add(frame.Render(version));
            if (((str.Length >= 0x10) && (str[10] == 'T')) && (str[13] == ':'))
            {
                frame = new TextInformationFrame(FrameType.TIME, this.encoding);
                frame.Text = new string[] { str.Substring(11, 2) + str.Substring(14, 2) };
                vector.Add(frame.Render(version));
            }
            return vector;
        }

        protected override ByteVector RenderFields(byte version)
        {
            if ((this.raw_data != null) && (this.raw_version == version))
            {
                return this.raw_data;
            }
            StringType type = Frame.CorrectEncoding(this.TextEncoding, version);
            byte[] data = new byte[] { (byte) type };
            ByteVector vector = new ByteVector(data);
            string[] strArray = this.text_fields;
            bool flag = base.FrameId == FrameType.TXXX;
            if ((version > 3) || flag)
            {
                if (flag)
                {
                    if (strArray.Length == 0)
                    {
                        strArray = new string[2];
                    }
                    else if (strArray.Length == 1)
                    {
                        string[] textArray1 = new string[2];
                        textArray1[0] = strArray[0];
                        strArray = textArray1;
                    }
                }
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (i != 0)
                    {
                        vector.Add(ByteVector.TextDelimiter(type));
                    }
                    if (strArray[i] != null)
                    {
                        vector.Add(ByteVector.FromString(strArray[i], type));
                    }
                }
                return vector;
            }
            if (base.FrameId == FrameType.TCON)
            {
                bool flag2 = true;
                StringBuilder builder = new StringBuilder();
                foreach (string str in strArray)
                {
                    if (!flag2)
                    {
                        builder.Append("/").Append(str);
                    }
                    else
                    {
                        byte num2;
                        if (flag2 = byte.TryParse(str, out num2))
                        {
                            object[] args = new object[] { num2 };
                            builder.AppendFormat(CultureInfo.InvariantCulture, "({0})", args);
                        }
                        else
                        {
                            builder.Append(str);
                        }
                    }
                }
                vector.Add(ByteVector.FromString(builder.ToString(), type));
                return vector;
            }
            vector.Add(ByteVector.FromString(string.Join("/", strArray), type));
            return vector;
        }

        [Obsolete("Use TextInformationFrame.Text")]
        public void SetText(StringCollection fields)
        {
            this.raw_data = null;
            this.Text = (fields == null) ? null : fields.ToArray();
        }

        [Obsolete("Use TextInformationFrame.Text")]
        public void SetText(params string[] text)
        {
            this.raw_data = null;
            this.Text = text;
        }

        public override string ToString()
        {
            this.ParseRawData();
            return string.Join("; ", this.Text);
        }

        [Obsolete("Use TextInformationFrame.Text")]
        public StringCollection FieldList
        {
            get
            {
                this.ParseRawData();
                return new StringCollection(this.Text);
            }
        }

        public virtual string[] Text
        {
            get
            {
                this.ParseRawData();
                return (string[]) this.text_fields.Clone();
            }
            set
            {
                this.raw_data = null;
                this.text_fields = (value == null) ? new string[0] : ((string[]) value.Clone());
            }
        }

        public StringType TextEncoding
        {
            get
            {
                this.ParseRawData();
                return this.encoding;
            }
            set
            {
                this.encoding = value;
            }
        }
    }
}

