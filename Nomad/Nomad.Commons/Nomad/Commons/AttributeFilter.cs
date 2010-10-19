namespace Nomad.Commons
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    [Serializable]
    public class AttributeFilter : BasicFilter
    {
        public AttributeFilter()
        {
        }

        public AttributeFilter(FileAttributes Include)
        {
            this.IncludeAttributes = Include;
        }

        public AttributeFilter(FileAttributes Include, FileAttributes Exclude) : this(Include)
        {
            this.ExcludeAttributes = Exclude;
        }

        public override bool EqualTo(object obj)
        {
            AttributeFilter filter = obj as AttributeFilter;
            return (((filter != null) && (filter.IncludeAttributes == this.IncludeAttributes)) && (filter.ExcludeAttributes == this.ExcludeAttributes));
        }

        private static string GetNextToken(string[] tokens, ref int position)
        {
            if (tokens.Length <= position)
            {
                throw new FormatException();
            }
            return tokens[position++];
        }

        public bool MatchAttributes(FileAttributes Value)
        {
            return (((Value & this.ExcludeAttributes) == 0) && ((Value & this.IncludeAttributes) == this.IncludeAttributes));
        }

        public static AttributeFilter Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            FileAttributes include = 0;
            FileAttributes exclude = 0;
            Dictionary<string, FileAttributes> dictionary = new Dictionary<string, FileAttributes>(StringComparer.OrdinalIgnoreCase);
            foreach (FileAttributes attributes3 in Enum.GetValues(typeof(FileAttributes)))
            {
                dictionary.Add(attributes3.ToString(), attributes3);
            }
            int position = 0;
            string[] tokens = s.Split(new char[] { ' ' });
            while (true)
            {
                FileAttributes attributes4;
                if (!GetNextToken(tokens, ref position).Equals("attribute", StringComparison.OrdinalIgnoreCase))
                {
                    throw new FormatException("'Attribute' expected");
                }
                string nextToken = GetNextToken(tokens, ref position);
                string key = GetNextToken(tokens, ref position);
                if (!dictionary.TryGetValue(key, out attributes4))
                {
                    throw new FormatException(string.Format("Unknown attribute name ({0})", key));
                }
                if (nextToken == "=")
                {
                    include |= attributes4;
                }
                else
                {
                    if (nextToken != "<>")
                    {
                        throw new FormatException("'=' or '<>' expected");
                    }
                    exclude |= attributes4;
                }
                if (position >= tokens.Length)
                {
                    return new AttributeFilter(include, exclude);
                }
                if (!GetNextToken(tokens, ref position).Equals("and", StringComparison.OrdinalIgnoreCase))
                {
                    throw new FormatException("'and' expected");
                }
            }
        }

        private bool ShouldSerializeExcludeAttributes()
        {
            return (this.ExcludeAttributes != 0);
        }

        private bool ShouldSerializeIncludeAttributes()
        {
            return (this.IncludeAttributes != 0);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (FileAttributes attributes in Enum.GetValues(typeof(FileAttributes)))
            {
                if ((attributes & this.IncludeAttributes) > 0)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" and ");
                    }
                    builder.Append("Attribute = ");
                    builder.Append(attributes);
                }
                if ((attributes & this.ExcludeAttributes) > 0)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" and ");
                    }
                    builder.Append("Attribute <> ");
                    builder.Append(attributes);
                }
            }
            return builder.ToString();
        }

        public FileAttributes ExcludeAttributes { get; set; }

        public FileAttributes IncludeAttributes { get; set; }
    }
}

