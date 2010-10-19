namespace Nomad.Commons
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    [Serializable]
    public class StringFilter : ValueFilter
    {
        private ContentComparision FComparision;
        private Regex FRegex;
        private string FText;
        public ContentFilterOptions Options;

        public StringFilter()
        {
            this.FComparision = ContentComparision.Ignore;
            this.Options = 0;
        }

        public StringFilter(string text)
        {
            this.FComparision = ContentComparision.Ignore;
            this.Options = 0;
            this.Text = text;
            this.Comparision = ContentComparision.Contains;
        }

        public StringFilter(string text, ContentFilterOptions options) : this(text)
        {
            this.Options = options;
        }

        public StringFilter(ContentComparision comparision, string text, ContentFilterOptions options)
        {
            this.FComparision = ContentComparision.Ignore;
            this.Options = 0;
            this.Text = text;
            this.Comparision = comparision;
            this.Options = options;
        }

        internal static Regex CreateRegexForOptions(ContentFilterOptions options, string text)
        {
            string str;
            if ((options & ContentFilterOptions.Regex) > 0)
            {
                str = text;
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                if ((options & ContentFilterOptions.WholeWords) > 0)
                {
                    builder.Append(@"\s");
                }
                if ((options & ContentFilterOptions.SpaceCompress) > 0)
                {
                    foreach (char ch in text)
                    {
                        if (char.IsWhiteSpace(ch) && ((builder.Length == 0) || !char.IsWhiteSpace(builder[builder.Length - 1])))
                        {
                            builder.Append(@"\s+");
                        }
                        else
                        {
                            builder.Append(ch);
                        }
                    }
                }
                else
                {
                    builder.Append(text);
                }
                if ((options & ContentFilterOptions.WholeWords) > 0)
                {
                    builder.Append(@"\s");
                }
                str = builder.ToString();
            }
            return new Regex(str, ((options & ContentFilterOptions.CaseSensitive) > 0) ? RegexOptions.None : RegexOptions.IgnoreCase);
        }

        public override bool EqualTo(object obj)
        {
            StringFilter filter = obj as StringFilter;
            return ((((filter != null) && (this.FComparision == filter.Comparision)) && (this.FText == filter.Text)) && (this.Options == filter.Options));
        }

        public bool MatchString(string str)
        {
            bool flag;
            if (this.FComparision == ContentComparision.Ignore)
            {
                return true;
            }
            if ((this.FRegex == null) && ((this.Options & (ContentFilterOptions.SpaceCompress | ContentFilterOptions.WholeWords | ContentFilterOptions.Regex)) > 0))
            {
                this.FRegex = CreateRegexForOptions(this.Options, this.FText);
            }
            if (this.FRegex != null)
            {
                flag = this.FRegex.IsMatch(str);
            }
            else
            {
                flag = str.IndexOf(this.FText, ((this.Options & ContentFilterOptions.CaseSensitive) > 0) ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase) >= 0;
            }
            if (this.FComparision == ContentComparision.Contains)
            {
                return flag;
            }
            return !flag;
        }

        public override bool MatchValue(object value)
        {
            if (this.FComparision == ContentComparision.Ignore)
            {
                return true;
            }
            string str = value as string;
            return ((str != null) && this.MatchString(str));
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

