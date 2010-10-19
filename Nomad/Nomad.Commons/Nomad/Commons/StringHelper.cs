namespace Nomad.Commons
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public static class StringHelper
    {
        private const TextFormatFlags AllEllipsisFlags = (TextFormatFlags.WordEllipsis | TextFormatFlags.EndEllipsis | TextFormatFlags.PathEllipsis);
        private static string TableCompactChars = "0123456789abcdefghijklmnopqrstuv";

        public static string ApplyCharacterCasing(string str, CharacterCasing characterCasing)
        {
            switch (characterCasing)
            {
                case CharacterCasing.Upper:
                    return str.ToUpper();

                case CharacterCasing.Lower:
                    return str.ToLower();
            }
            return str;
        }

        private static string ByteArrayToCompactString(byte[] value)
        {
            StringBuilder builder = new StringBuilder(((value.Length * 8) / 5) + 1);
            BitArray array = new BitArray(value);
            for (int i = 0; i < array.Count; i += 5)
            {
                int num2 = 0;
                for (int j = 0; (j < 5) && ((i + j) < array.Count); j++)
                {
                    num2 |= ((array[i + j] != null) ? 1 : 0) << j;
                }
                builder.Append(TableCompactChars[num2]);
            }
            return builder.ToString();
        }

        public static string CompactString(string str, int width, Font font, TextFormatFlags formatFlags)
        {
            if ((formatFlags & (TextFormatFlags.WordEllipsis | TextFormatFlags.EndEllipsis | TextFormatFlags.PathEllipsis)) == TextFormatFlags.GlyphOverhangPadding)
            {
                throw new ArgumentException("formatFlags must containt PathEllipsis, WordEllipsis or EndEllipsis");
            }
            try
            {
                Size proposedSize = new Size(width, 1);
                if (TextRenderer.MeasureText(str, font, proposedSize, formatFlags & ~(TextFormatFlags.WordEllipsis | TextFormatFlags.EndEllipsis | TextFormatFlags.PathEllipsis)).Width <= width)
                {
                    return str;
                }
                string text = new StringBuilder(str, str.Length + 4).ToString();
                TextRenderer.MeasureText(text, font, proposedSize, formatFlags | TextFormatFlags.ModifyString);
                int index = text.IndexOf('\0');
                if (index >= 0)
                {
                    return text.Substring(0, index);
                }
                return text;
            }
            catch (AccessViolationException exception)
            {
                Debug.WriteLine(exception);
                return str;
            }
        }

        public static bool ContainsAny(this string str, params char[] anyOf)
        {
            return (str.IndexOfAny(anyOf) >= 0);
        }

        public static bool EndsWith(this string str, char value)
        {
            return ((str.Length > 0) && (str[str.Length - 1] == value));
        }

        public static bool EndsWithAny(this string str, params char[] anyOf)
        {
            if ((str.Length != 0) && (anyOf.Length != 0))
            {
                foreach (char ch in anyOf)
                {
                    if (str[str.Length - 1] == ch)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string GuidToCompactString(Guid value)
        {
            return ByteArrayToCompactString(value.ToByteArray());
        }

        public static int IndexOfAny(this string str, params char[] anyOf)
        {
            return str.IndexOfAny(anyOf);
        }

        private static void ProcessSpaces(string str, StringBuilder resultBuilder, bool skipSpaces, ref int index, ref int lineStart)
        {
            while (index < str.Length)
            {
                switch (str[index])
                {
                    case '\n':
                        resultBuilder.AppendLine(str.Substring(lineStart, index - lineStart));
                        lineStart = index + 1;
                        break;

                    case '\r':
                        if (((index + 1) < str.Length) && (str[index + 1] == '\n'))
                        {
                            resultBuilder.AppendLine(str.Substring(lineStart, index - lineStart));
                            index++;
                            lineStart = index + 1;
                        }
                        break;

                    case ' ':
                        if (skipSpaces)
                        {
                            break;
                        }
                        return;

                    default:
                        if (skipSpaces)
                        {
                            return;
                        }
                        break;
                }
                index++;
            }
            index = -1;
        }

        public static string ReadValue(string str, ref int startIndex, params char[] delimiters)
        {
            if (str == null)
            {
                throw new ArgumentNullException();
            }
            if (startIndex >= str.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            int num = startIndex;
            startIndex = str.IndexOfAny(delimiters, startIndex);
            if (startIndex >= 0)
            {
                startIndex++;
                return str.Substring(num, (startIndex - num) - 1);
            }
            return str.Substring(num);
        }

        public static IEnumerable<string> SplitString(string str, params char[] delims)
        {
            return new <SplitString>d__0(-2) { <>3__str = str, <>3__delims = delims };
        }

        public static bool StartsWith(this string str, char value)
        {
            return ((str.Length > 0) && (str[0] == value));
        }

        public static bool StartsWithAny(this string str, params char[] anyOf)
        {
            if ((str.Length != 0) && (anyOf.Length != 0))
            {
                foreach (char ch in anyOf)
                {
                    if (str[0] == ch)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string WordWrap(string str, int width, Font font)
        {
            int num3;
            StringBuilder resultBuilder = new StringBuilder();
            int lineStart = 0;
            for (int i = 0; i < str.Length; i = num3 + 1)
            {
                ProcessSpaces(str, resultBuilder, true, ref i, ref lineStart);
                num3 = i;
                if (num3 >= 0)
                {
                    ProcessSpaces(str, resultBuilder, false, ref num3, ref lineStart);
                }
                if (num3 < 0)
                {
                    resultBuilder.Append(str.Substring(lineStart));
                    break;
                }
                if (TextRenderer.MeasureText(str.Substring(lineStart, num3 - lineStart), font).Width >= width)
                {
                    resultBuilder.AppendLine(str.Substring(lineStart, i - lineStart));
                    lineStart = i;
                }
            }
            return resultBuilder.ToString();
        }

        [CompilerGenerated]
        private sealed class <SplitString>d__0 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private string <>2__current;
            public char[] <>3__delims;
            public string <>3__str;
            private int <>l__initialThreadId;
            public int <DelimIndex>5__2;
            public int <StartIndex>5__1;
            public char[] delims;
            public string str;

            [DebuggerHidden]
            public <SplitString>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        if (this.str == null)
                        {
                            throw new ArgumentNullException();
                        }
                        this.<StartIndex>5__1 = 0;
                        this.<DelimIndex>5__2 = this.str.IndexOfAny(this.delims);
                        while (this.<DelimIndex>5__2 >= 0)
                        {
                            this.<>2__current = this.str.Substring(this.<StartIndex>5__1, this.<DelimIndex>5__2 - this.<StartIndex>5__1);
                            this.<>1__state = 1;
                            return true;
                        Label_009A:
                            this.<>1__state = -1;
                            this.<StartIndex>5__1 = this.<DelimIndex>5__2 + 1;
                            this.<DelimIndex>5__2 = this.str.IndexOfAny(this.delims, this.<StartIndex>5__1);
                        }
                        if (this.<StartIndex>5__1 >= this.str.Length)
                        {
                            break;
                        }
                        this.<>2__current = this.str.Substring(this.<StartIndex>5__1);
                        this.<>1__state = 2;
                        return true;

                    case 1:
                        goto Label_009A;

                    case 2:
                        this.<>1__state = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                StringHelper.<SplitString>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new StringHelper.<SplitString>d__0(0);
                }
                d__.str = this.<>3__str;
                d__.delims = this.<>3__delims;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.String>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            string IEnumerator<string>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

