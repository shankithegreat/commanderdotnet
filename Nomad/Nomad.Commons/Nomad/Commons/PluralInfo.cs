namespace Nomad.Commons
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class PluralInfo
    {
        private static PluralInfo CurrentPluralInfo;
        private int FallbackIndex;
        private static Regex ParamRegex;
        private LinkedList<PluralRange>[] PluralRanges;

        public PluralInfo(string format)
        {
            if (format == null)
            {
                throw new ArgumentNullException();
            }
            if (format == string.Empty)
            {
                throw new ArgumentException();
            }
            string[] strArray = format.Split(new char[] { ';' });
            this.PluralRanges = new LinkedList<PluralRange>[strArray.Length];
            this.FallbackIndex = -1;
            for (int i = 0; i < strArray.Length; i++)
            {
                this.PluralRanges[i] = new LinkedList<PluralRange>();
                foreach (string str in strArray[i].Split(new char[] { ',' }))
                {
                    if (str.Equals("F", StringComparison.OrdinalIgnoreCase))
                    {
                        this.FallbackIndex = i;
                    }
                    else
                    {
                        this.PluralRanges[i].AddLast(PluralRange.Parse(str));
                    }
                }
            }
        }

        public static string Format(string format, params object[] args)
        {
            if (Current == null)
            {
                throw new InvalidOperationException();
            }
            return Current.Format(null, format, args);
        }

        public string Format(IFormatProvider provider, string format, params object[] args)
        {
            if (format == null)
            {
                throw new ArgumentNullException();
            }
            if (format == string.Empty)
            {
                throw new ArgumentException();
            }
            if (ParamRegex == null)
            {
                ParamRegex = new Regex(@"\{(?<ParamNo>\d+)(:(?<Format>.+?))?}", RegexOptions.Compiled);
            }
            return ParamRegex.Replace(format, delegate (Match match) {
                int index = int.Parse(match.Groups["ParamNo"].Value);
                Group group = match.Groups["Format"];
                string str = group.Success ? group.Value : null;
                if ((str != null) && (str.IndexOf('|') >= 0))
                {
                    string[] strArray;
                    switch (Type.GetTypeCode(args[index].GetType()))
                    {
                        case TypeCode.Boolean:
                            strArray = str.Split(new char[] { '|' });
                            if (strArray.Length != 2)
                            {
                                throw new ArgumentException("Two values expected for boolean parameter");
                            }
                            return strArray[Convert.ToBoolean(args[index]) ? 0 : 1];

                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                            strArray = str.Split(new char[] { '|' });
                            for (int j = 0; j < this.PluralRanges.Length; j++)
                            {
                                foreach (PluralRange range in this.PluralRanges[j])
                                {
                                    if (range.IsMatch(args[index]))
                                    {
                                        return strArray[j];
                                    }
                                }
                            }
                            if (this.FallbackIndex < 0)
                            {
                                throw new ArgumentException("Plural range not found");
                            }
                            return strArray[this.FallbackIndex];
                    }
                }
                if (str == null)
                {
                    return string.Format(provider, "{0}", new object[] { args[index] });
                }
                return string.Format(provider, "{0:" + str + "}", new object[] { args[index] });
            });
        }

        public static PluralInfo Current
        {
            get
            {
                return CurrentPluralInfo;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                CurrentPluralInfo = value;
            }
        }
    }
}

