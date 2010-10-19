namespace Nomad.Commons
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Text.RegularExpressions;

    [Serializable]
    public class NameFilter : BasicFilter
    {
        private NamePatternComparision FNameComparision;
        private string FNamePattern;
        private Regex FRegex;
        [DefaultValue(typeof(NamePatternCondition), "Equal")]
        public NamePatternCondition NameCondition;

        public NameFilter()
        {
            this.FNameComparision = NamePatternComparision.Ignore;
            this.NameCondition = NamePatternCondition.Equal;
        }

        public NameFilter(string pattern)
        {
            this.FNameComparision = NamePatternComparision.Ignore;
            this.NameCondition = NamePatternCondition.Equal;
            this.FNameComparision = NamePatternComparision.Wildcards;
            this.NamePattern = pattern;
            if ((this.NameComparision != NamePatternComparision.RegEx) && (this.NameComparision != NamePatternComparision.Ignore))
            {
                StringBuilder builder = new StringBuilder();
                bool flag = false;
                bool flag2 = this.NamePattern.StartsWith('*');
                char ch = '\0';
                foreach (char ch2 in this.NamePattern)
                {
                    switch (ch2)
                    {
                        case '?':
                        case '[':
                        case '#':
                            return;

                        case '*':
                            if (flag && (ch != '*'))
                            {
                                return;
                            }
                            flag = true;
                            break;

                        default:
                            if (!(!flag || flag2))
                            {
                                return;
                            }
                            builder.Append(ch2);
                            break;
                    }
                    ch = ch2;
                }
                if (!flag)
                {
                    if (ch == '.')
                    {
                        this.FNamePattern = this.FNamePattern.Remove(this.FNamePattern.Length - 1);
                    }
                    this.NameComparision = NamePatternComparision.Equals;
                }
                else if (ch != '.')
                {
                    this.FNamePattern = builder.ToString();
                    if (ch == '*')
                    {
                        this.NameComparision = NamePatternComparision.StartsWith;
                    }
                    else
                    {
                        this.NameComparision = NamePatternComparision.EndsWith;
                    }
                }
            }
        }

        public NameFilter(NamePatternComparision comparision, string pattern)
        {
            this.FNameComparision = NamePatternComparision.Ignore;
            this.NameCondition = NamePatternCondition.Equal;
            this.NamePattern = pattern;
            this.NameComparision = comparision;
        }

        public NameFilter(NamePatternCondition condition, string pattern) : this(pattern)
        {
            this.NameCondition = condition;
        }

        public NameFilter(NamePatternCondition condition, NamePatternComparision comparision, string pattern) : this(comparision, pattern)
        {
            this.NameCondition = condition;
        }

        public override bool EqualTo(object obj)
        {
            NameFilter filter = obj as NameFilter;
            return ((((filter != null) && (filter.NameComparision == this.NameComparision)) && (filter.NameCondition == this.NameCondition)) && string.Equals(filter.NamePattern, this.NamePattern, StringComparison.OrdinalIgnoreCase));
        }

        private static string MaskPartToRegexpStr(string maskPart, string regAnyChar)
        {
            StringBuilder builder = new StringBuilder();
            bool infinity = false;
            int anyCount = 0;
            int digitCount = 0;
            char ch = '\0';
            foreach (char ch2 in maskPart)
            {
                if (UpdateMaskBuilder(builder, ch2, regAnyChar, infinity, anyCount, digitCount))
                {
                    infinity = false;
                    anyCount = 0;
                    digitCount = 0;
                }
                switch (ch2)
                {
                    case '!':
                        if (ch == '[')
                        {
                            builder.Append('^');
                        }
                        break;

                    case '#':
                        digitCount++;
                        break;

                    case '$':
                    case '(':
                    case ')':
                    case '+':
                    case '.':
                    case '\\':
                    case '^':
                    case '{':
                    case '}':
                        builder.Append('\\');
                        builder.Append(ch2);
                        break;

                    case '*':
                        infinity = true;
                        break;

                    case '?':
                        anyCount++;
                        break;

                    default:
                        builder.Append(ch2);
                        break;
                }
                ch = ch2;
            }
            UpdateMaskBuilder(builder, '\0', regAnyChar, infinity, anyCount, digitCount);
            return builder.ToString();
        }

        private static int MaskPrefix(string Str1, string Str2)
        {
            Str1 = Str1.ToUpper();
            Str2 = Str2.ToUpper();
            int num = 0;
            for (int i = 0; i < Str1.Length; i++)
            {
                if (Str1[i] != Str2[i])
                {
                    return num;
                }
                num = i + 1;
            }
            return num;
        }

        public static string MasksToRegexpStr(string[] masks)
        {
            if (masks == null)
            {
                throw new ArgumentNullException();
            }
            if (masks.Length == 0)
            {
                throw new ArgumentException();
            }
            StringBuilder builder = new StringBuilder("^");
            if (masks.Length == 1)
            {
                builder.Append(MaskToRegexpStr(masks[0]));
            }
            else
            {
                List<string> list = new List<string>();
                foreach (string str in masks)
                {
                    list.Add(str.ToLower().Trim());
                }
                list.Sort();
                int num = 0;
                for (int i = list.Count - 1; num < list.Count; i = list.Count - 1)
                {
                    int length = list[num].Length;
                    int num4 = num + 1;
                    while (num4 < list.Count)
                    {
                        int num5 = MaskPrefix(list[num4 - 1], list[num4]);
                        if (num5 == 0)
                        {
                            i = num4 - 1;
                            break;
                        }
                        if (num5 < length)
                        {
                            length = num5;
                        }
                        num4++;
                    }
                    if ((num > 0) || (i < (list.Count - 1)))
                    {
                        builder.Append('(');
                    }
                    if (num == i)
                    {
                        builder.Append(MaskToRegexpStr(list[num]));
                    }
                    else
                    {
                        builder.Append(MaskPartToRegexpStr(list[num].Substring(0, length), "."));
                        builder.Append('(');
                        for (num4 = num; num4 <= i; num4++)
                        {
                            if (num4 > num)
                            {
                                builder.Append('|');
                            }
                            builder.Append(MaskPartToRegexpStr(list[num4].Substring(length), "."));
                        }
                        builder.Append(')');
                    }
                    if ((num > 0) || (i < (list.Count - 1)))
                    {
                        builder.Append(')');
                        if (i < (list.Count - 1))
                        {
                            builder.Append('|');
                        }
                    }
                    num = i + 1;
                }
            }
            builder.Append('$');
            return builder.ToString();
        }

        public static string MasksToRegexpStr(string masks)
        {
            if (masks == null)
            {
                throw new ArgumentNullException();
            }
            return MasksToRegexpStr(masks.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string MaskToRegexpStr(string mask)
        {
            if (mask == null)
            {
                throw new ArgumentNullException();
            }
            if (mask == "*.*")
            {
                return ".+";
            }
            string regAnyChar = ".";
            if (mask.EndsWith('.'))
            {
                mask = mask.Remove(mask.Length - 1);
                regAnyChar = @"[^\.]";
            }
            return MaskPartToRegexpStr(mask, regAnyChar);
        }

        public bool MatchName(string value)
        {
            bool flag = true;
            switch (this.NameComparision)
            {
                case NamePatternComparision.StartsWith:
                    flag = value.StartsWith(this.FNamePattern, StringComparison.OrdinalIgnoreCase);
                    break;

                case NamePatternComparision.EndsWith:
                    flag = value.EndsWith(this.FNamePattern, StringComparison.OrdinalIgnoreCase);
                    break;

                case NamePatternComparision.Equals:
                    flag = string.Equals(value, this.FNamePattern, StringComparison.OrdinalIgnoreCase);
                    break;

                case NamePatternComparision.Wildcards:
                    try
                    {
                        if (this.FRegex == null)
                        {
                            this.FRegex = new Regex(MasksToRegexpStr(this.FNamePattern), RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        }
                        flag = this.FRegex.IsMatch(value);
                    }
                    catch (ArgumentException)
                    {
                        this.NameComparision = NamePatternComparision.Ignore;
                    }
                    break;

                case NamePatternComparision.RegEx:
                    try
                    {
                        if (this.FRegex == null)
                        {
                            this.FRegex = new Regex(this.FNamePattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        }
                        flag = this.FRegex.IsMatch(value);
                    }
                    catch (ArgumentException)
                    {
                        this.NameComparision = NamePatternComparision.Ignore;
                    }
                    break;
            }
            return ((this.NameCondition == NamePatternCondition.NotEqual) ? !flag : flag);
        }

        public override string ToString()
        {
            switch (this.NameComparision)
            {
                case NamePatternComparision.Ignore:
                    return string.Empty;

                case NamePatternComparision.StartsWith:
                    return (this.NamePattern + '*');

                case NamePatternComparision.EndsWith:
                    return ('*' + this.NamePattern);
            }
            return this.NamePattern;
        }

        private static bool UpdateMaskBuilder(StringBuilder builder, char Char, string RegAnyChar, bool infinity, int anyCount, int digitCount)
        {
            if (infinity && (Char != '*'))
            {
                builder.Append(RegAnyChar);
                if (anyCount == 1)
                {
                    builder.Append('+');
                }
                else if (anyCount > 1)
                {
                    builder.AppendFormat("{{{0},}}", anyCount);
                }
                else
                {
                    builder.Append('*');
                }
                return true;
            }
            if (((anyCount > 0) && (Char != '*')) && (Char != '?'))
            {
                builder.Append(RegAnyChar);
                if (anyCount > 1)
                {
                    builder.AppendFormat("{{{0}}}", anyCount);
                }
                return true;
            }
            if ((digitCount > 0) && (Char != '#'))
            {
                if (digitCount == 1)
                {
                    builder.Append(@"\d");
                }
                else
                {
                    builder.AppendFormat(@"\d{{{0}}}", digitCount);
                }
                return true;
            }
            return false;
        }

        public NamePatternComparision NameComparision
        {
            get
            {
                return this.FNameComparision;
            }
            set
            {
                if (this.FNameComparision != value)
                {
                    this.FNameComparision = value;
                    this.FRegex = null;
                }
            }
        }

        public string NamePattern
        {
            get
            {
                return this.FNamePattern;
            }
            set
            {
                if (this.FNamePattern != value)
                {
                    this.FNamePattern = value;
                    if ((string.IsNullOrEmpty(this.FNamePattern) || (this.FNamePattern == "*")) || (this.FNamePattern == "*.*"))
                    {
                        this.NameComparision = NamePatternComparision.Ignore;
                    }
                    else if (this.FNamePattern.StartsWith('^') && this.FNamePattern.EndsWith('$'))
                    {
                        this.NameComparision = NamePatternComparision.RegEx;
                    }
                    this.FRegex = null;
                }
            }
        }
    }
}

