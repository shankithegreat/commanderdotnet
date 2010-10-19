namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using TagLib;

    public class UserTextInformationFrame : TextInformationFrame
    {
        public UserTextInformationFrame(string description) : base(FrameType.TXXX)
        {
            base.Text = new string[] { description };
        }

        public UserTextInformationFrame(string description, StringType encoding) : base(FrameType.TXXX, encoding)
        {
            base.Text = new string[] { description };
        }

        public UserTextInformationFrame(ByteVector data, byte version) : base(data, version)
        {
        }

        protected internal UserTextInformationFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(data, offset, header, version)
        {
        }

        [Obsolete("Use UserTextInformationFrame.Get(Tag,string,bool)")]
        public static UserTextInformationFrame Get(TagLib.Id3v2.Tag tag, string description)
        {
            return Get(tag, description, false);
        }

        public static UserTextInformationFrame Get(TagLib.Id3v2.Tag tag, string description, bool create)
        {
            return Get(tag, description, TagLib.Id3v2.Tag.DefaultEncoding, create);
        }

        public static UserTextInformationFrame Get(TagLib.Id3v2.Tag tag, string description, StringType type, bool create)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }
            if (description.Length == 0)
            {
                throw new ArgumentException("Description must not be empty.", "description");
            }
            IEnumerator<UserTextInformationFrame> enumerator = tag.GetFrames<UserTextInformationFrame>(FrameType.TXXX).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    UserTextInformationFrame current = enumerator.Current;
                    if (description.Equals(current.Description))
                    {
                        return current;
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
            UserTextInformationFrame frame2 = new UserTextInformationFrame(description, type);
            tag.AddFrame(frame2);
            return frame2;
        }

        public override string ToString()
        {
            return new StringBuilder().Append("[").Append(this.Description).Append("] ").Append(base.ToString()).ToString();
        }

        public string Description
        {
            get
            {
                string[] text = base.Text;
                return ((text.Length <= 0) ? null : text[0]);
            }
            set
            {
                string[] text = base.Text;
                if (text.Length > 0)
                {
                    text[0] = value;
                }
                else
                {
                    text = new string[] { value };
                }
                base.Text = text;
            }
        }

        public override string[] Text
        {
            get
            {
                string[] text = base.Text;
                if (text.Length < 2)
                {
                    return new string[0];
                }
                string[] strArray2 = new string[text.Length - 1];
                for (int i = 0; i < strArray2.Length; i++)
                {
                    strArray2[i] = text[i + 1];
                }
                return strArray2;
            }
            set
            {
                string[] strArray = new string[(value == null) ? 1 : (value.Length + 1)];
                strArray[0] = this.Description;
                for (int i = 1; i < strArray.Length; i++)
                {
                    strArray[i] = value[i - 1];
                }
                base.Text = strArray;
            }
        }
    }
}

