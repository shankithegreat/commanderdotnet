namespace Nomad.Commons.IO
{
    using System;
    using System.IO;

    public class IniReader : IDisposable
    {
        private StringComparison Comparision;
        private bool EOF;
        private TextReader FBaseReader;
        private string FCurrentKey;
        private string FCurrentLine;
        private string FCurrentSection;
        private string FCurrentValue;
        private IniElementType FElementType;

        public IniReader(TextReader reader)
        {
            this.Comparision = StringComparison.OrdinalIgnoreCase;
            if (reader == null)
            {
                throw new ArgumentNullException();
            }
            this.FBaseReader = reader;
        }

        public IniReader(TextReader reader, bool ignoreCase) : this(reader)
        {
            this.Comparision = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        }

        public int CheckCurrentKey(string section, params string[] keys)
        {
            if (this.CheckCurrentSection(section))
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (string.Equals(this.CurrentKey, keys[i], this.Comparision))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public bool CheckCurrentKey(string section, string key)
        {
            return (this.CheckCurrentSection(section) && string.Equals(this.CurrentKey, key, this.Comparision));
        }

        public bool CheckCurrentSection(string section)
        {
            return string.Equals(this.CurrentSection, section, this.Comparision);
        }

        public void Close()
        {
            if (this.FBaseReader != null)
            {
                this.FBaseReader.Close();
            }
            this.FBaseReader = null;
        }

        public void Dispose()
        {
            this.Close();
        }

        public bool MoveToKey(string section, string key)
        {
            while (this.Read())
            {
                if ((string.Equals(this.CurrentSection, section, this.Comparision) && (this.ElementType == IniElementType.KeyValuePair)) && string.Equals(this.CurrentKey, key, this.Comparision))
                {
                    return true;
                }
            }
            return false;
        }

        public bool MoveToSection()
        {
            string currentSection = this.CurrentSection;
            while (this.Read())
            {
                if (!string.Equals(this.CurrentSection, currentSection, this.Comparision))
                {
                    return true;
                }
            }
            return false;
        }

        public bool MoveToSection(string section)
        {
            while (this.Read())
            {
                if (string.Equals(this.CurrentSection, section, this.Comparision))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Read()
        {
            this.FCurrentLine = null;
            if (this.EOF)
            {
                return false;
            }
            this.FCurrentLine = this.BaseReader.ReadLine();
            if (this.FCurrentLine == null)
            {
                this.EOF = true;
                this.FElementType = IniElementType.None;
                return false;
            }
            this.FCurrentKey = null;
            this.FCurrentValue = null;
            if (this.FCurrentLine == string.Empty)
            {
                this.FElementType = IniElementType.EmptyStringLine;
                return true;
            }
            if (this.FCurrentLine[0] == ';')
            {
                this.FElementType = IniElementType.Comment;
                this.FCurrentValue = this.FCurrentLine.Substring(1).TrimStart(new char[] { ' ' });
                return true;
            }
            if (this.FCurrentLine[0] == '[')
            {
                this.FCurrentLine = this.FCurrentLine.TrimEnd(new char[0]);
                if ((this.FCurrentLine.Length <= 2) || (this.FCurrentLine[this.FCurrentLine.Length - 1] != ']'))
                {
                    throw new InvalidDataException();
                }
                this.FCurrentSection = this.FCurrentLine.Substring(1, this.FCurrentLine.Length - 2);
                this.FElementType = IniElementType.Section;
                return true;
            }
            int index = this.FCurrentLine.IndexOf('=');
            if (index > 0)
            {
                this.FCurrentKey = this.FCurrentLine.Substring(0, index).TrimStart(new char[] { ' ' });
                this.FCurrentValue = this.FCurrentLine.Substring(index + 1);
                this.FElementType = IniElementType.KeyValuePair;
                return true;
            }
            this.FElementType = IniElementType.StringLine;
            return true;
        }

        public string ReadValue(string section, string key)
        {
            if (this.MoveToKey(section, key))
            {
                return this.CurrentValue;
            }
            return null;
        }

        public TextReader BaseReader
        {
            get
            {
                if (this.FBaseReader == null)
                {
                    throw new ObjectDisposedException("IniReader");
                }
                return this.FBaseReader;
            }
        }

        public string CurrentKey
        {
            get
            {
                return this.FCurrentKey;
            }
        }

        public string CurrentLine
        {
            get
            {
                return this.FCurrentLine;
            }
        }

        public string CurrentSection
        {
            get
            {
                return this.FCurrentSection;
            }
        }

        public string CurrentValue
        {
            get
            {
                return this.FCurrentValue;
            }
        }

        public IniElementType ElementType
        {
            get
            {
                return this.FElementType;
            }
        }

        public bool EndOfFile
        {
            get
            {
                return this.EOF;
            }
        }
    }
}

