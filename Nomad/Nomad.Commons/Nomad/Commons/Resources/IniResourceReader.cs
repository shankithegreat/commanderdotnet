namespace Nomad.Commons.Resources
{
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Resources;
    using System.Text;

    public class IniResourceReader : IResourceReader, IEnumerable, IDisposable
    {
        private string FIniPath;
        private string FIniSection;
        private Ini FIniSource;
        private IDictionary<string, string> FSubstitutions;
        private bool HasSubstitutions;
        private const string SectionSubstitutions = ".Substitutions";

        public IniResourceReader(Ini iniSource, string iniSection)
        {
            this.InitializeSource(iniSource);
            this.InitializeSection(iniSection);
        }

        public IniResourceReader(Ini iniSource, Type resourceSource)
        {
            if (resourceSource == null)
            {
                throw new ArgumentNullException("resourceType");
            }
            this.InitializeSource(iniSource);
            this.InitializeSection(resourceSource.FullName);
        }

        public IniResourceReader(string iniPath, string iniSection)
        {
            this.InitializePath(iniPath);
            this.InitializeSection(iniSection);
        }

        public IniResourceReader(string iniPath, Type resourceSource)
        {
            if (resourceSource == null)
            {
                throw new ArgumentNullException("resourceType");
            }
            this.InitializePath(iniPath);
            this.InitializeSection(resourceSource.FullName);
        }

        public void Close()
        {
            this.Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            this.FIniSource = null;
            this.FIniPath = null;
            this.FIniSection = null;
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            if (this.FIniSource != null)
            {
                return new IniResourceEnumerator(this);
            }
            if (this.FIniPath == null)
            {
                throw new ObjectDisposedException("IniResourceReader");
            }
            return new IniFileResourceEnumerator(this);
        }

        private string GetSubstitution(string key)
        {
            string str;
            if (this.FIniSource != null)
            {
                return this.FIniSource.Get(".Substitutions", key);
            }
            if (!this.HasSubstitutions)
            {
                using (TextReader reader = new StreamReader(this.FIniPath))
                {
                    this.FSubstitutions = Ini.ReadSection(reader, ".Substitutions");
                }
                this.HasSubstitutions = true;
            }
            if ((this.FSubstitutions != null) && this.FSubstitutions.TryGetValue(key, out str))
            {
                return str;
            }
            return key;
        }

        private void InitializePath(string iniPath)
        {
            if (iniPath == null)
            {
                throw new ArgumentNullException("iniPath");
            }
            if (iniPath == string.Empty)
            {
                throw new ArgumentException();
            }
            this.FIniPath = iniPath;
        }

        private void InitializeSection(string iniSection)
        {
            if (iniSection == null)
            {
                throw new ArgumentNullException("iniSection");
            }
            if (iniSection == string.Empty)
            {
                throw new ArgumentException();
            }
            this.FIniSection = iniSection;
        }

        private void InitializeSource(Ini iniSource)
        {
            if (iniSource == null)
            {
                throw new ArgumentNullException("iniSource");
            }
            this.FIniSource = iniSource;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }

        private static string UnescapeString(string str)
        {
            StringBuilder builder = null;
            int startIndex = 0;
            int num2 = 0;
            while (num2 < str.Length)
            {
                if (str[num2++] != '\\')
                {
                    continue;
                }
                if (builder == null)
                {
                    builder = new StringBuilder(str.Length);
                }
                builder.Append(str.Substring(startIndex, (num2 - startIndex) - 1));
                switch (str[num2])
                {
                    case 'r':
                        builder.Append('\r');
                        break;

                    case 't':
                        builder.Append('\t');
                        break;

                    case 'v':
                        builder.Append('\v');
                        break;

                    case 'n':
                        builder.Append('\n');
                        break;

                    case 'f':
                        builder.Append('\f');
                        break;

                    case 'a':
                        builder.Append('\a');
                        break;

                    case 'b':
                        builder.Append('\b');
                        break;

                    case '0':
                        builder.Append('\0');
                        break;

                    default:
                        builder.Append(str[num2]);
                        break;
                }
                startIndex = ++num2;
            }
            if (builder != null)
            {
                builder.Append(str.Substring(startIndex));
                return builder.ToString();
            }
            return str;
        }

        private class IniFileResourceEnumerator : IDictionaryEnumerator, IEnumerator
        {
            private bool Finished;
            private IniResourceReader Owner;
            private IniReader Reader;

            public IniFileResourceEnumerator(IniResourceReader reader)
            {
                this.Owner = reader;
            }

            public bool MoveNext()
            {
                if (this.Owner.FIniSection == null)
                {
                    if (this.Reader != null)
                    {
                        this.Reader.Close();
                    }
                    this.Reader = null;
                    throw new ObjectDisposedException("IniResourceReader");
                }
                if (!(this.Finished || (this.Reader != null)))
                {
                    this.Reader = new IniReader(new StreamReader(this.Owner.FIniPath));
                    this.Finished = !this.Reader.MoveToSection(this.Owner.FIniSection);
                }
                if (!this.Finished)
                {
                    this.Finished = !this.Reader.Read();
                }
                while (!this.Finished && (this.Reader.ElementType != IniElementType.KeyValuePair))
                {
                    this.Finished = !this.Reader.Read() || (this.Reader.ElementType == IniElementType.Section);
                }
                if (this.Finished && (this.Reader != null))
                {
                    this.Reader.Close();
                    this.Reader = null;
                }
                return !this.Finished;
            }

            public void Reset()
            {
                if (this.Reader != null)
                {
                    this.Reader.Close();
                }
                this.Reader = null;
                if (this.Owner.FIniSection == null)
                {
                    throw new ObjectDisposedException("IniResourceReader");
                }
            }

            public object Current
            {
                get
                {
                    return this.Entry;
                }
            }

            public DictionaryEntry Entry
            {
                get
                {
                    if ((this.Finished || (this.Reader == null)) || (this.Reader.ElementType != IniElementType.KeyValuePair))
                    {
                        throw new InvalidOperationException();
                    }
                    return new DictionaryEntry(this.Reader.CurrentKey, this.Reader.CurrentValue);
                }
            }

            public object Key
            {
                get
                {
                    if ((this.Finished || (this.Reader == null)) || (this.Reader.ElementType != IniElementType.KeyValuePair))
                    {
                        throw new InvalidOperationException();
                    }
                    return this.Reader.CurrentKey;
                }
            }

            public object Value
            {
                get
                {
                    if ((this.Finished || (this.Reader == null)) || (this.Reader.ElementType != IniElementType.KeyValuePair))
                    {
                        throw new InvalidOperationException();
                    }
                    string currentValue = this.Reader.CurrentValue;
                    if (currentValue.StartsWith('>'))
                    {
                        currentValue = this.Owner.GetSubstitution(currentValue.Substring(1));
                    }
                    return IniResourceReader.UnescapeString(currentValue);
                }
            }
        }

        private class IniResourceEnumerator : IDictionaryEnumerator, IEnumerator, IDisposable
        {
            private IniResourceReader Owner;
            private IEnumerator<KeyValuePair<string, string>> SectionEnumerator;

            public IniResourceEnumerator(IniResourceReader reader)
            {
                this.Owner = reader;
            }

            private void CheckOwner()
            {
                if (this.Owner == null)
                {
                    throw new ObjectDisposedException("IniResourceReader");
                }
                if (this.Owner.FIniSection == null)
                {
                    if (this.SectionEnumerator != null)
                    {
                        this.SectionEnumerator.Dispose();
                    }
                    this.SectionEnumerator = null;
                    throw new ObjectDisposedException("IniResourceReader");
                }
            }

            public void Dispose()
            {
                if (this.SectionEnumerator != null)
                {
                    this.SectionEnumerator.Dispose();
                }
                this.SectionEnumerator = null;
                this.Owner = null;
            }

            public bool MoveNext()
            {
                this.CheckOwner();
                if (this.SectionEnumerator == null)
                {
                    Ini.IniSection section = this.Owner.FIniSource[this.Owner.FIniSection];
                    if (section == null)
                    {
                        return false;
                    }
                    this.SectionEnumerator = ((IEnumerable<KeyValuePair<string, string>>) section).GetEnumerator();
                }
                return this.SectionEnumerator.MoveNext();
            }

            public void Reset()
            {
                this.CheckOwner();
                if (this.SectionEnumerator != null)
                {
                    this.SectionEnumerator.Reset();
                }
            }

            public object Current
            {
                get
                {
                    return this.Entry;
                }
            }

            public DictionaryEntry Entry
            {
                get
                {
                    if (this.SectionEnumerator == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return new DictionaryEntry(this.SectionEnumerator.Current.Key, this.SectionEnumerator.Current.Value);
                }
            }

            public object Key
            {
                get
                {
                    if (this.SectionEnumerator == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return this.SectionEnumerator.Current.Key;
                }
            }

            public object Value
            {
                get
                {
                    if (this.SectionEnumerator == null)
                    {
                        throw new InvalidOperationException();
                    }
                    string substitution = this.SectionEnumerator.Current.Value;
                    if (substitution.StartsWith('>'))
                    {
                        substitution = this.Owner.GetSubstitution(substitution.Substring(1));
                    }
                    return ((substitution != null) ? IniResourceReader.UnescapeString(substitution) : null);
                }
            }
        }
    }
}

