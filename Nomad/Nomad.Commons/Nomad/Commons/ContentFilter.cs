namespace Nomad.Commons
{
    using Microsoft.IE;
    using Nomad.FilterReader;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Xml.Serialization;

    [Serializable]
    public class ContentFilter : CustomContentFilter
    {
        [XmlIgnore]
        public System.Text.Encoding Encoding;
        private ContentComparision FComparision;
        private Regex FRegex;
        private string FText;
        public ContentFilterOptions Options;

        public ContentFilter()
        {
            this.FComparision = ContentComparision.Ignore;
            this.Options = 0;
            this.Encoding = System.Text.Encoding.UTF8;
        }

        public ContentFilter(string text)
        {
            this.FComparision = ContentComparision.Ignore;
            this.Options = 0;
            this.Encoding = System.Text.Encoding.UTF8;
            this.Text = text;
            this.Comparision = ContentComparision.Contains;
        }

        public ContentFilter(string text, ContentFilterOptions options) : this(text)
        {
            this.Options = options;
        }

        public ContentFilter(ContentComparision comparision, string text, ContentFilterOptions options)
        {
            this.FComparision = ContentComparision.Ignore;
            this.Options = 0;
            this.Encoding = System.Text.Encoding.UTF8;
            this.Text = text;
            this.Comparision = comparision;
            this.Options = options;
        }

        public override bool EqualTo(object obj)
        {
            ContentFilter filter = obj as ContentFilter;
            return ((((filter != null) && (this.Comparision == filter.Comparision)) && ((this.Text == filter.Text) && (this.Options == filter.Options))) && this.Encoding.Equals(filter.Encoding));
        }

        public override bool MatchStream(Stream contentStream, string fileName)
        {
            bool flag;
            if (this.FComparision == ContentComparision.Ignore)
            {
                return true;
            }
            if ((this.FRegex == null) && ((this.Options & (ContentFilterOptions.SpaceCompress | ContentFilterOptions.WholeWords | ContentFilterOptions.Regex)) > 0))
            {
                this.FRegex = StringFilter.CreateRegexForOptions(this.Options, this.FText);
            }
            TextReader reader = null;
            if ((this.Options & ContentFilterOptions.UseIFilter) > 0)
            {
                reader = Nomad.FilterReader.FilterReader.Create(contentStream, fileName);
            }
            if (reader != null)
            {
                using (reader)
                {
                    flag = this.ReaderContainsText(reader);
                }
            }
            else
            {
                if ((this.Options & ContentFilterOptions.DetectEncoding) > 0)
                {
                    this.Encoding = null;
                }
                flag = this.StreamContainsText(contentStream);
            }
            if (this.FComparision == ContentComparision.Contains)
            {
                return flag;
            }
            return !flag;
        }

        private bool ReaderContainsText(TextReader reader)
        {
            int num;
            char[] buffer = new char[0x20000];
            int index = 0;
            bool flag = false;
            do
            {
                if (!base.OnProgress(0))
                {
                    return flag;
                }
                num = reader.Read(buffer, index, buffer.Length - index);
                if (num > 0)
                {
                    int num3 = index;
                    index = Math.Min(0x200, num);
                    string input = new string(buffer, 0, num + num3);
                    if (this.FRegex != null)
                    {
                        flag = this.FRegex.IsMatch(input);
                    }
                    else
                    {
                        flag = input.IndexOf(this.FText, ((this.Options & ContentFilterOptions.CaseSensitive) > 0) ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase) >= 0;
                    }
                    Array.Copy(buffer, (num + num3) - index, buffer, 0, index);
                }
            }
            while ((num > 0) && !flag);
            return flag;
        }

        private bool StreamContainsText(Stream contentStream)
        {
            int num;
            byte[] buffer = new byte[0x40000];
            byte[] buffer2 = new byte[0x40000];
            byte[] buffer3 = buffer;
            byte[] buffer4 = buffer2;
            int offset = 0;
            bool flag = false;
            IAsyncResult asyncResult = contentStream.BeginRead(buffer3, 0, buffer3.Length, null, null);
            do
            {
                if (!base.OnProgress(0))
                {
                    break;
                }
                num = contentStream.EndRead(asyncResult);
                if (num > 0)
                {
                    buffer3 = Interlocked.Exchange<byte[]>(ref buffer4, buffer3);
                    int num3 = offset;
                    offset = Math.Min(0x400, num);
                    asyncResult = contentStream.BeginRead(buffer3, offset, buffer3.Length - offset, null, null);
                    if (this.Encoding == null)
                    {
                        this.Encoding = MLangHelper.DetectEncoding(buffer4, (int) (num + num3));
                        if (this.Encoding == null)
                        {
                            this.Encoding = System.Text.Encoding.Default;
                        }
                    }
                    string input = this.Encoding.GetString(buffer4, 0, num + num3);
                    if (this.FRegex != null)
                    {
                        flag = this.FRegex.IsMatch(input);
                    }
                    else
                    {
                        flag = input.IndexOf(this.FText, ((this.Options & ContentFilterOptions.CaseSensitive) > 0) ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase) >= 0;
                    }
                    Array.Copy(buffer4, (num + num3) - offset, buffer3, 0, offset);
                }
                else
                {
                    asyncResult = null;
                }
            }
            while ((num > 0) && !flag);
            if (asyncResult != null)
            {
                contentStream.EndRead(asyncResult);
            }
            return flag;
        }

        public ContentComparision Comparision
        {
            get
            {
                return this.FComparision;
            }
            set
            {
                if (this.FComparision != value)
                {
                    this.FComparision = value;
                    this.FRegex = null;
                }
            }
        }

        [DefaultValue("UTF8"), XmlElement("Encoding"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public string EncodingAsString
        {
            get
            {
                return ((this.Encoding == null) ? null : this.Encoding.WebName);
            }
            set
            {
                this.Encoding = (value == null) ? null : System.Text.Encoding.GetEncoding(value);
            }
        }

        public string Text
        {
            get
            {
                return this.FText;
            }
            set
            {
                if (this.FText != value)
                {
                    this.FText = value;
                    if (string.IsNullOrEmpty(value))
                    {
                        this.Comparision = ContentComparision.Ignore;
                    }
                    this.FRegex = null;
                }
            }
        }
    }
}

