namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class WdxDetectString
    {
        private List<SimpleToken> RPN;

        public WdxDetectString(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException();
            }
            if (pattern == string.Empty)
            {
                throw new ArgumentException();
            }
            this.RPN = new List<SimpleToken>(ConvertToRPN(GetTokens(pattern)));
        }

        private static IEnumerable<SimpleToken> ConvertToRPN(IEnumerable<SimpleToken> tokens)
        {
            return new <ConvertToRPN>d__4(-2) { <>3__tokens = tokens };
        }

        private bool ExecuteFunction(string functionName, object value, string content)
        {
            string str = value as string;
            switch (functionName)
            {
                case "FIND":
                    if (string.IsNullOrEmpty(str))
                    {
                        return false;
                    }
                    return (content.IndexOf(str, StringComparison.Ordinal) >= 0);

                case "FINDI":
                    if (string.IsNullOrEmpty(str))
                    {
                        return false;
                    }
                    return (content.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            throw new InvalidDataException(string.Format("Unknown function '{0}'", functionName));
        }

        private string GetContent(FileInfo info)
        {
            byte[] buffer = new byte[0x2000];
            using (Stream stream = info.OpenRead())
            {
                int count = stream.Read(buffer, 0, buffer.Length);
                return Encoding.ASCII.GetString(buffer, 0, count);
            }
        }

        private static int GetPrecedence(TokenType token)
        {
            switch (token)
            {
                case TokenType.OperatorEqual:
                case TokenType.OperatorNotEqual:
                case TokenType.OperatorLess:
                case TokenType.OperatorGreater:
                    return 3;

                case TokenType.OperatorAnd:
                    return 2;

                case TokenType.OperatorOr:
                    return 1;

                case TokenType.OperatorNot:
                    return 4;
            }
            throw new InvalidEnumArgumentException();
        }

        private object GetProperty(string propertyName, FileInfo info)
        {
            switch (propertyName)
            {
                case "EXT":
                {
                    string extension = Path.GetExtension(info.Name);
                    return (string.IsNullOrEmpty(extension) ? string.Empty : extension.Substring(1).ToUpper());
                }
                case "SIZE":
                    return info.Length;

                case "FORCE":
                    return false;

                case "MULTIMEDIA":
                    return true;
            }
            throw new InvalidDataException(string.Format("Unknown property '{0}'", propertyName));
        }

        private static IEnumerable<SimpleToken> GetTokens(string pattern)
        {
            return new <GetTokens>d__0(-2) { <>3__pattern = pattern };
        }

        private bool IsAnd(object x, object y)
        {
            return (((x is bool) && (y is bool)) && (((bool) x) && ((bool) y)));
        }

        private bool IsEqual(object x, object y)
        {
            if ((x is bool) && (y is bool))
            {
                return (((bool) x) == ((bool) y));
            }
            if ((x is char) && (y is char))
            {
                return (((char) x) == ((char) y));
            }
            if (((x is int) || (x is char)) && ((y is int) || (y is char)))
            {
                return (Convert.ToInt32(x) == Convert.ToInt32(y));
            }
            return (((x is string) && (y is string)) && string.Equals((string) x, (string) y, StringComparison.Ordinal));
        }

        private bool IsGreater(object y, object x)
        {
            if ((x is int) && (y is int))
            {
                return (((int) x) > ((int) y));
            }
            return (((x is char) && (y is char)) && (((char) x) > ((char) y)));
        }

        private bool IsLess(object y, object x)
        {
            if ((x is int) && (y is int))
            {
                return (((int) x) < ((int) y));
            }
            return (((x is char) && (y is char)) && (((char) x) < ((char) y)));
        }

        public bool IsMatch(FileInfo info)
        {
            Stack<object> stack = new Stack<object>();
            string content = null;
            foreach (SimpleToken token in this.RPN)
            {
                switch (token.TokenType)
                {
                    case TokenType.String:
                        stack.Push(((Token<string>) token).Value);
                        break;

                    case TokenType.Char:
                        stack.Push(((Token<char>) token).Value);
                        break;

                    case TokenType.Number:
                        stack.Push(((Token<int>) token).Value);
                        break;

                    case TokenType.Index:
                    {
                        if (content == null)
                        {
                            content = this.GetContent(info);
                        }
                        int num = ((Token<int>) token).Value;
                        if (num >= content.Length)
                        {
                            return false;
                        }
                        stack.Push(content[num]);
                        break;
                    }
                    case TokenType.Property:
                        stack.Push(this.GetProperty(((Token<string>) token).Value, info));
                        break;

                    case TokenType.Function:
                        if (content == null)
                        {
                            content = this.GetContent(info);
                        }
                        stack.Push(this.ExecuteFunction(((Token<string>) token).Value, stack.Pop(), content));
                        break;

                    case TokenType.OperatorEqual:
                        stack.Push(this.IsEqual(stack.Pop(), stack.Pop()));
                        break;

                    case TokenType.OperatorNotEqual:
                        stack.Push(!this.IsEqual(stack.Pop(), stack.Pop()));
                        break;

                    case TokenType.OperatorLess:
                        stack.Push(this.IsLess(stack.Pop(), stack.Pop()));
                        break;

                    case TokenType.OperatorGreater:
                        stack.Push(this.IsGreater(stack.Pop(), stack.Pop()));
                        break;

                    case TokenType.OperatorAnd:
                        stack.Push(this.IsAnd(stack.Pop(), stack.Pop()));
                        break;

                    case TokenType.OperatorOr:
                        stack.Push(this.IsOr(stack.Pop(), stack.Pop()));
                        break;

                    case TokenType.OperatorNot:
                    {
                        object obj2 = stack.Pop();
                        stack.Push((obj2 is bool) && !((bool) obj2));
                        break;
                    }
                }
            }
            if (!((stack.Count == 1) && (stack.Peek() is bool)))
            {
                return false;
            }
            return (bool) stack.Pop();
        }

        public bool IsMatch(string fileName)
        {
            return this.IsMatch(new FileInfo(fileName));
        }

        private static bool IsOperator(TokenType token)
        {
            switch (token)
            {
                case TokenType.OperatorEqual:
                case TokenType.OperatorNotEqual:
                case TokenType.OperatorLess:
                case TokenType.OperatorGreater:
                case TokenType.OperatorAnd:
                case TokenType.OperatorOr:
                case TokenType.OperatorNot:
                    return true;
            }
            return false;
        }

        private bool IsOr(object x, object y)
        {
            return (((x is bool) && (y is bool)) && (((bool) x) || ((bool) y)));
        }

        [CompilerGenerated]
        private sealed class <ConvertToRPN>d__4 : IEnumerable<WdxDetectString.SimpleToken>, IEnumerable, IEnumerator<WdxDetectString.SimpleToken>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private WdxDetectString.SimpleToken <>2__current;
            public IEnumerable<WdxDetectString.SimpleToken> <>3__tokens;
            public IEnumerator<WdxDetectString.SimpleToken> <>7__wrap8;
            private int <>l__initialThreadId;
            public WdxDetectString.SimpleToken <NextToken>5__7;
            public Stack<WdxDetectString.SimpleToken> <TokenStack>5__6;
            public WdxDetectString.SimpleToken <TopToken>5__5;
            public IEnumerable<WdxDetectString.SimpleToken> tokens;

            [DebuggerHidden]
            public <ConvertToRPN>d__4(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally9()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap8 != null)
                {
                    this.<>7__wrap8.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            break;

                        case 2:
                            goto Label_00FC;

                        case 3:
                            goto Label_01B0;

                        case 4:
                            goto Label_021C;

                        case 5:
                            goto Label_02C6;

                        case 6:
                            goto Label_035D;

                        default:
                            goto Label_0388;
                    }
                    this.<>1__state = -1;
                    this.<TokenStack>5__6 = new Stack<WdxDetectString.SimpleToken>();
                    this.<>7__wrap8 = this.tokens.GetEnumerator();
                    this.<>1__state = 1;
                    while (this.<>7__wrap8.MoveNext())
                    {
                        this.<NextToken>5__7 = this.<>7__wrap8.Current;
                        switch (this.<NextToken>5__7.TokenType)
                        {
                            case WdxDetectString.TokenType.String:
                            case WdxDetectString.TokenType.Char:
                            case WdxDetectString.TokenType.Number:
                            case WdxDetectString.TokenType.Index:
                            case WdxDetectString.TokenType.Property:
                                this.<>2__current = this.<NextToken>5__7;
                                this.<>1__state = 2;
                                return true;

                            case WdxDetectString.TokenType.Function:
                            case WdxDetectString.TokenType.OpenBrace:
                            {
                                this.<TokenStack>5__6.Push(this.<NextToken>5__7);
                                continue;
                            }
                            case WdxDetectString.TokenType.CloseBrace:
                                this.<TopToken>5__5 = null;
                                goto Label_0224;

                            case WdxDetectString.TokenType.OperatorEqual:
                            case WdxDetectString.TokenType.OperatorNotEqual:
                            case WdxDetectString.TokenType.OperatorLess:
                            case WdxDetectString.TokenType.OperatorGreater:
                            case WdxDetectString.TokenType.OperatorAnd:
                            case WdxDetectString.TokenType.OperatorOr:
                            case WdxDetectString.TokenType.OperatorNot:
                                if (this.<TokenStack>5__6.Count <= 0)
                                {
                                    goto Label_01B9;
                                }
                                this.<TopToken>5__5 = this.<TokenStack>5__6.Peek();
                                if (!WdxDetectString.IsOperator(this.<TopToken>5__5.TokenType) || (WdxDetectString.GetPrecedence(this.<NextToken>5__7.TokenType) > WdxDetectString.GetPrecedence(this.<TopToken>5__5.TokenType)))
                                {
                                    goto Label_01B9;
                                }
                                this.<TopToken>5__5 = this.<TokenStack>5__6.Pop();
                                this.<>2__current = this.<TopToken>5__5;
                                this.<>1__state = 3;
                                return true;

                            default:
                            {
                                continue;
                            }
                        }
                    Label_00FC:
                        this.<>1__state = 1;
                        continue;
                    Label_01B0:
                        this.<>1__state = 1;
                    Label_01B9:
                        this.<TokenStack>5__6.Push(this.<NextToken>5__7);
                        continue;
                    Label_01D9:
                        this.<TopToken>5__5 = this.<TokenStack>5__6.Pop();
                        if (this.<TopToken>5__5.TokenType == WdxDetectString.TokenType.OpenBrace)
                        {
                            goto Label_0236;
                        }
                        this.<>2__current = this.<TopToken>5__5;
                        this.<>1__state = 4;
                        return true;
                    Label_021C:
                        this.<>1__state = 1;
                    Label_0224:
                        if (this.<TokenStack>5__6.Count > 0)
                        {
                            goto Label_01D9;
                        }
                    Label_0236:
                        if ((this.<TopToken>5__5 == null) || (this.<TopToken>5__5.TokenType != WdxDetectString.TokenType.OpenBrace))
                        {
                            throw new InvalidDataException("Mismatched parenthesis");
                        }
                        if (this.<TokenStack>5__6.Count <= 0)
                        {
                            continue;
                        }
                        this.<TopToken>5__5 = this.<TokenStack>5__6.Peek();
                        if (this.<TopToken>5__5.TokenType != WdxDetectString.TokenType.Function)
                        {
                            goto Label_02CE;
                        }
                        this.<TopToken>5__5 = this.<TokenStack>5__6.Pop();
                        this.<>2__current = this.<TopToken>5__5;
                        this.<>1__state = 5;
                        return true;
                    Label_02C6:
                        this.<>1__state = 1;
                    Label_02CE:;
                    }
                    this.<>m__Finally9();
                    while (this.<TokenStack>5__6.Count > 0)
                    {
                        this.<TopToken>5__5 = this.<TokenStack>5__6.Pop();
                        switch (this.<TopToken>5__5.TokenType)
                        {
                            case WdxDetectString.TokenType.OpenBrace:
                            case WdxDetectString.TokenType.CloseBrace:
                                throw new InvalidDataException("Mismatched parenthesis");

                            case WdxDetectString.TokenType.OperatorEqual:
                            case WdxDetectString.TokenType.OperatorNotEqual:
                            case WdxDetectString.TokenType.OperatorLess:
                            case WdxDetectString.TokenType.OperatorGreater:
                            case WdxDetectString.TokenType.OperatorAnd:
                            case WdxDetectString.TokenType.OperatorOr:
                            case WdxDetectString.TokenType.OperatorNot:
                                this.<>2__current = this.<TopToken>5__5;
                                this.<>1__state = 6;
                                return true;

                            default:
                                throw new InvalidDataException("Unknown error");
                        }
                    Label_035D:
                        this.<>1__state = -1;
                    }
                Label_0388:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<WdxDetectString.SimpleToken> IEnumerable<WdxDetectString.SimpleToken>.GetEnumerator()
            {
                WdxDetectString.<ConvertToRPN>d__4 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new WdxDetectString.<ConvertToRPN>d__4(0);
                }
                d__.tokens = this.<>3__tokens;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Property.Providers.Wdx.WdxDetectString.SimpleToken>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally9();
                        }
                        break;
                }
            }

            WdxDetectString.SimpleToken IEnumerator<WdxDetectString.SimpleToken>.Current
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

        [CompilerGenerated]
        private sealed class <GetTokens>d__0 : IEnumerable<WdxDetectString.SimpleToken>, IEnumerable, IEnumerator<WdxDetectString.SimpleToken>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private WdxDetectString.SimpleToken <>2__current;
            public string <>3__pattern;
            private int <>l__initialThreadId;
            public WdxDetectString.Tokenizer <GetToken>5__1;
            public string pattern;

            [DebuggerHidden]
            public <GetTokens>d__0(int <>1__state)
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
                        this.<GetToken>5__1 = new WdxDetectString.Tokenizer(this.pattern);
                        while (this.<GetToken>5__1.Read())
                        {
                            switch (this.<GetToken>5__1.CurrentTokenType)
                            {
                                case WdxDetectString.TokenType.String:
                                case WdxDetectString.TokenType.Property:
                                case WdxDetectString.TokenType.Function:
                                    this.<>2__current = new WdxDetectString.Token<string>(this.<GetToken>5__1.CurrentTokenType, this.<GetToken>5__1.ValueString);
                                    this.<>1__state = 2;
                                    return true;

                                case WdxDetectString.TokenType.Char:
                                    this.<>2__current = new WdxDetectString.Token<char>(this.<GetToken>5__1.CurrentTokenType, this.<GetToken>5__1.ValueString[0]);
                                    this.<>1__state = 1;
                                    return true;

                                case WdxDetectString.TokenType.Number:
                                case WdxDetectString.TokenType.Index:
                                    this.<>2__current = new WdxDetectString.Token<int>(this.<GetToken>5__1.CurrentTokenType, this.<GetToken>5__1.ValueInt);
                                    this.<>1__state = 3;
                                    return true;

                                default:
                                    goto Label_0138;
                            }
                        Label_00C2:
                            this.<>1__state = -1;
                            continue;
                        Label_00FA:
                            this.<>1__state = -1;
                            continue;
                        Label_012F:
                            this.<>1__state = -1;
                            continue;
                        Label_0138:
                            this.<>2__current = new WdxDetectString.SimpleToken(this.<GetToken>5__1.CurrentTokenType);
                            this.<>1__state = 4;
                            return true;
                        Label_0159:
                            this.<>1__state = -1;
                        }
                        break;

                    case 1:
                        goto Label_00C2;

                    case 2:
                        goto Label_00FA;

                    case 3:
                        goto Label_012F;

                    case 4:
                        goto Label_0159;
                }
                return false;
            }

            [DebuggerHidden]
            IEnumerator<WdxDetectString.SimpleToken> IEnumerable<WdxDetectString.SimpleToken>.GetEnumerator()
            {
                WdxDetectString.<GetTokens>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new WdxDetectString.<GetTokens>d__0(0);
                }
                d__.pattern = this.<>3__pattern;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Property.Providers.Wdx.WdxDetectString.SimpleToken>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            WdxDetectString.SimpleToken IEnumerator<WdxDetectString.SimpleToken>.Current
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

        private class SimpleToken
        {
            public readonly Nomad.FileSystem.Property.Providers.Wdx.WdxDetectString.TokenType TokenType;

            public SimpleToken(Nomad.FileSystem.Property.Providers.Wdx.WdxDetectString.TokenType type)
            {
                this.TokenType = type;
            }

            public override string ToString()
            {
                switch (this.TokenType)
                {
                    case Nomad.FileSystem.Property.Providers.Wdx.WdxDetectString.TokenType.OpenBrace:
                        return "(";

                    case Nomad.FileSystem.Property.Providers.Wdx.WdxDetectString.TokenType.CloseBrace:
                        return ")";

                    case Nomad.FileSystem.Property.Providers.Wdx.WdxDetectString.TokenType.OperatorEqual:
                        return "=";

                    case Nomad.FileSystem.Property.Providers.Wdx.WdxDetectString.TokenType.OperatorNotEqual:
                        return "!=";

                    case Nomad.FileSystem.Property.Providers.Wdx.WdxDetectString.TokenType.OperatorLess:
                        return "<";

                    case Nomad.FileSystem.Property.Providers.Wdx.WdxDetectString.TokenType.OperatorGreater:
                        return ">";

                    case Nomad.FileSystem.Property.Providers.Wdx.WdxDetectString.TokenType.OperatorAnd:
                        return "&";

                    case Nomad.FileSystem.Property.Providers.Wdx.WdxDetectString.TokenType.OperatorOr:
                        return "|";

                    case Nomad.FileSystem.Property.Providers.Wdx.WdxDetectString.TokenType.OperatorNot:
                        return "!";
                }
                return base.ToString();
            }
        }

        private class Token<T> : WdxDetectString.SimpleToken
        {
            public readonly T Value;

            public Token(WdxDetectString.TokenType type, T value) : base(type)
            {
                this.Value = value;
            }

            public override string ToString()
            {
                switch (base.TokenType)
                {
                    case WdxDetectString.TokenType.String:
                        return ('"' + this.Value.ToString() + '"');

                    case WdxDetectString.TokenType.Char:
                        return ('\'' + this.Value.ToString() + '\'');

                    case WdxDetectString.TokenType.Index:
                        return ('[' + this.Value.ToString() + ']');
                }
                return this.Value.ToString();
            }
        }

        private class Tokenizer
        {
            private WdxDetectString.TokenType FTokenType;
            private int FValueInt;
            private string FValueStr;
            private TextReader Reader;

            public Tokenizer(string pattern)
            {
                this.Reader = new StringReader(pattern);
            }

            public bool Read()
            {
                int num = this.Reader.Read();
                while ((num >= 0) && char.IsWhiteSpace((char) num))
                {
                    num = this.Reader.Read();
                }
                if (num < 0)
                {
                    return false;
                }
                char c = (char) num;
                switch (c)
                {
                    case '[':
                        this.FTokenType = WdxDetectString.TokenType.Index;
                        this.FValueStr = this.ReadTo(']');
                        if (!(!string.IsNullOrEmpty(this.FValueStr) && int.TryParse(this.FValueStr, out this.FValueInt)))
                        {
                            throw new InvalidDataException("Invalid index value");
                        }
                        break;

                    case '|':
                        this.FTokenType = WdxDetectString.TokenType.OperatorOr;
                        break;

                    case '!':
                        this.FTokenType = WdxDetectString.TokenType.OperatorNot;
                        if (((ushort) this.Reader.Peek()) == 0x3d)
                        {
                            this.Reader.Read();
                            this.FTokenType = WdxDetectString.TokenType.OperatorNotEqual;
                        }
                        break;

                    case '"':
                        this.FValueStr = this.ReadTo('"');
                        if (this.FValueStr.Length != 1)
                        {
                            this.FTokenType = WdxDetectString.TokenType.String;
                        }
                        else
                        {
                            this.FTokenType = WdxDetectString.TokenType.Char;
                        }
                        break;

                    case '&':
                        this.FTokenType = WdxDetectString.TokenType.OperatorAnd;
                        break;

                    case '(':
                        this.FTokenType = WdxDetectString.TokenType.OpenBrace;
                        break;

                    case ')':
                        this.FTokenType = WdxDetectString.TokenType.CloseBrace;
                        break;

                    case '<':
                        this.FTokenType = WdxDetectString.TokenType.OperatorLess;
                        break;

                    case '=':
                        this.FTokenType = WdxDetectString.TokenType.OperatorEqual;
                        break;

                    case '>':
                        this.FTokenType = WdxDetectString.TokenType.OperatorGreater;
                        break;

                    default:
                        if (char.IsDigit(c))
                        {
                            this.FValueStr = this.ReadCategory(c, delegate (char ch) {
                                return char.IsDigit(ch);
                            });
                            this.FValueInt = int.Parse(this.FValueStr);
                            this.FTokenType = WdxDetectString.TokenType.Number;
                        }
                        else if (char.IsLetter(c))
                        {
                            this.FValueStr = this.ReadCategory(c, delegate (char ch) {
                                return char.IsLetter(ch);
                            }).ToUpper();
                            if (((ushort) this.Reader.Peek()) == 40)
                            {
                                this.FTokenType = WdxDetectString.TokenType.Function;
                            }
                            else
                            {
                                this.FTokenType = WdxDetectString.TokenType.Property;
                            }
                        }
                        break;
                }
                return true;
            }

            private string ReadCategory(char firstChar, Func<char, bool> checkCategory)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(firstChar);
                for (int i = this.Reader.Peek(); (i < 0) ? false : checkCategory((char) ((ushort) i)); i = this.Reader.Peek())
                {
                    builder.Append((char) this.Reader.Read());
                }
                return builder.ToString();
            }

            private string ReadTo(char value)
            {
                StringBuilder builder = new StringBuilder();
                int num = value;
                int num2 = this.Reader.Read();
                while ((num2 >= 0) && (num2 != num))
                {
                    builder.Append((char) num2);
                    num2 = this.Reader.Read();
                }
                if (num2 < 0)
                {
                    throw new InvalidDataException(string.Format("'{0}' expected, but EOF found", value));
                }
                return builder.ToString();
            }

            public WdxDetectString.TokenType CurrentTokenType
            {
                get
                {
                    return this.FTokenType;
                }
            }

            public int ValueInt
            {
                get
                {
                    return this.FValueInt;
                }
            }

            public string ValueString
            {
                get
                {
                    return this.FValueStr;
                }
            }
        }

        private enum TokenType
        {
            None,
            String,
            Char,
            Number,
            Index,
            Property,
            Function,
            OpenBrace,
            CloseBrace,
            OperatorEqual,
            OperatorNotEqual,
            OperatorLess,
            OperatorGreater,
            OperatorAnd,
            OperatorOr,
            OperatorNot
        }
    }
}

