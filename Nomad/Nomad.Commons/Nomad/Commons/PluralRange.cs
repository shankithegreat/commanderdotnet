namespace Nomad.Commons
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;

    public sealed class PluralRange
    {
        private static Regex ParseRegex;

        private PluralRange()
        {
        }

        public PluralRange(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            this.From = value;
            this.To = value;
        }

        public PluralRange(int fromValue, int toValue)
        {
            if (fromValue < 0)
            {
                throw new ArgumentOutOfRangeException("fromValue");
            }
            if (toValue < fromValue)
            {
                throw new ArgumentOutOfRangeException("toValue");
            }
            this.From = fromValue;
            this.To = toValue;
        }

        public PluralRange(int fromValue, int toValue, int divisor) : this(fromValue, toValue)
        {
            if (divisor < 1)
            {
                throw new ArgumentOutOfRangeException("divisor");
            }
            this.Divisor = divisor;
        }

        private static bool IsInRange<T>(T value, T from, T to) where T: struct
        {
            Comparer<T> comparer = Comparer<T>.Default;
            return ((comparer.Compare(value, from) >= 0) && (comparer.Compare(value, to) <= 0));
        }

        public bool IsMatch(int value)
        {
            return IsInRange<int>(Math.Abs((this.Divisor != 0) ? (value % this.Divisor) : value), this.From, this.To);
        }

        public bool IsMatch(long value)
        {
            return IsInRange<long>(Math.Abs((this.Divisor != 0) ? (value % ((long) this.Divisor)) : value), (long) this.From, (long) this.To);
        }

        public bool IsMatch(object value)
        {
            if (value != null)
            {
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                        return this.IsMatch((int) value);

                    case TypeCode.UInt32:
                        return this.IsMatch((uint) value);

                    case TypeCode.Int64:
                        return this.IsMatch((long) value);

                    case TypeCode.UInt64:
                        return this.IsMatch((ulong) value);
                }
            }
            return false;
        }

        public bool IsMatch(uint value)
        {
            return IsInRange<uint>((this.Divisor != 0) ? ((uint) (value % this.Divisor)) : value, (uint) this.From, (uint) this.To);
        }

        public bool IsMatch(ulong value)
        {
            return IsInRange<ulong>((this.Divisor != 0) ? (value % ((ulong) this.Divisor)) : value, (ulong) this.From, (ulong) this.To);
        }

        public static PluralRange Parse(string format)
        {
            PluralRange range = TryParse(format);
            if (range == null)
            {
                throw new ArgumentException();
            }
            return range;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.From);
            if (this.To != this.From)
            {
                builder.Append('-');
                builder.Append(this.To);
            }
            if (this.Divisor > 0)
            {
                builder.Append('%');
                builder.Append(this.Divisor);
            }
            return builder.ToString();
        }

        public static PluralRange TryParse(string format)
        {
            if (ParseRegex == null)
            {
                ParseRegex = new Regex(@"^(?<From>\d+)(-(?<To>\d+))?(%(?<Divisor>\d+))?$", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            Match match = ParseRegex.Match(format);
            if (!match.Success)
            {
                return null;
            }
            PluralRange range = new PluralRange {
                From = int.Parse(match.Groups["From"].Value)
            };
            if (match.Groups["To"].Success)
            {
                range.To = int.Parse(match.Groups["To"].Value);
            }
            else
            {
                range.To = range.From;
            }
            if (match.Groups["Divisor"].Success)
            {
                range.Divisor = int.Parse(match.Groups["Divisor"].Value);
            }
            return range;
        }

        public static bool TryParse(string format, out PluralRange result)
        {
            result = TryParse(format);
            return (result != null);
        }

        public int Divisor { get; private set; }

        public int From { get; private set; }

        public int To { get; private set; }
    }
}

