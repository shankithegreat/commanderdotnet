namespace Nomad.Commons
{
    using Nomad.Properties;
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Text.RegularExpressions;

    [DebuggerDisplay("RegexRename, {RenameRegex.ToString()}, {FReplacement}")]
    public class RegexRenameFilter : IRenameFilter
    {
        private string FReplacement;
        private Regex RenameRegex;

        public RegexRenameFilter(string wildcardPattern)
        {
            if (wildcardPattern == null)
            {
                throw new ArgumentNullException("wildcardPattern");
            }
            if (wildcardPattern == string.Empty)
            {
                throw new ArgumentException("wildcardPattern is empty.");
            }
            StringBuilder builder = new StringBuilder("^");
            StringBuilder builder2 = new StringBuilder();
            BlockType none = BlockType.None;
            bool flag = false;
            bool flag2 = false;
            int num = 0;
            StringBuilder builder3 = new StringBuilder();
            int num2 = 0;
            foreach (char ch in wildcardPattern + '\0')
            {
                BlockType dot;
                switch (ch)
                {
                    case '.':
                        dot = BlockType.Dot;
                        break;

                    case '?':
                        dot = BlockType.Single;
                        num++;
                        break;

                    case '\0':
                        dot = BlockType.End;
                        break;

                    case '*':
                        dot = BlockType.Multi;
                        break;

                    default:
                        dot = BlockType.Text;
                        builder3.Append(ch);
                        break;
                }
                if (none == BlockType.None)
                {
                    none = dot;
                    goto Label_021F;
                }
                if (dot == none)
                {
                    goto Label_021F;
                }
                switch (none)
                {
                    case BlockType.Text:
                        if (!flag)
                        {
                            builder.AppendFormat(".{{0,{0}}}", builder3.Length);
                        }
                        builder2.Append(builder3.ToString());
                        builder3.Remove(0, builder3.Length);
                        goto Label_021B;

                    case BlockType.Dot:
                        if (!flag)
                        {
                            builder.Append(".*?");
                        }
                        builder.Append(@"\.");
                        builder2.Append('.');
                        flag = false;
                        goto Label_021B;

                    case BlockType.Single:
                    case BlockType.Multi:
                        if (flag)
                        {
                            throw new ArgumentException(Resources.sWildcardTooComplex);
                        }
                        break;

                    default:
                        throw new InvalidOperationException();
                }
                builder.AppendFormat("(?<R{0}>.", num2);
                if (none == BlockType.Single)
                {
                    builder.Append("{0,");
                    builder.Append(num);
                    builder.Append("})");
                }
                else
                {
                    builder.Append("*)");
                    flag = true;
                }
                builder2.AppendFormat("${{R{0}}}", num2++);
                num = 0;
                flag2 = true;
            Label_021B:
                none = dot;
            Label_021F:;
            }
            if (!flag)
            {
                builder.Append(".*?");
            }
            builder.Append("$");
            if (flag2)
            {
                this.RenameRegex = new Regex(builder.ToString(), RegexOptions.Singleline | RegexOptions.Compiled);
            }
            this.FReplacement = builder2.ToString();
        }

        public RegexRenameFilter(string pattern, string replacement)
        {
            this.RenameRegex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled);
            this.FReplacement = replacement;
        }

        public string CreateNewName(string sourceName)
        {
            if (this.RenameRegex == null)
            {
                return this.FReplacement;
            }
            return this.RenameRegex.Replace(sourceName, this.FReplacement);
        }

        private enum BlockType
        {
            None,
            Text,
            Dot,
            Single,
            Multi,
            End
        }
    }
}

