namespace Nomad.Commons.IO
{
    using System;
    using System.IO;

    public class IniWriter : IDisposable
    {
        private TextWriter FBaseWriter;

        public IniWriter(TextWriter baseWriter)
        {
            if (baseWriter == null)
            {
                throw new ArgumentNullException();
            }
            this.FBaseWriter = baseWriter;
        }

        public void Close()
        {
            if (this.FBaseWriter != null)
            {
                this.FBaseWriter.Close();
            }
            this.FBaseWriter = null;
        }

        public void Dispose()
        {
            this.Close();
        }

        public void WriteComment(string comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException();
            }
            this.BaseWriter.Write("; ");
            this.BaseWriter.WriteLine(comment);
        }

        public void WriteLine()
        {
            this.BaseWriter.WriteLine();
        }

        public void WriteLine(string line)
        {
            if (line == null)
            {
                throw new ArgumentNullException();
            }
            this.BaseWriter.WriteLine(line);
        }

        public void WriteSection(string section)
        {
            if (section == null)
            {
                throw new ArgumentNullException();
            }
            if (section == string.Empty)
            {
                throw new ArgumentException();
            }
            this.BaseWriter.Write('[');
            this.BaseWriter.Write(section);
            this.BaseWriter.WriteLine(']');
        }

        public void WriteValue(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if ((key == string.Empty) || (key.IndexOf('=') >= 0))
            {
                throw new ArgumentException();
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.BaseWriter.Write(key);
            this.BaseWriter.Write('=');
            this.BaseWriter.WriteLine(value);
        }

        public TextWriter BaseWriter
        {
            get
            {
                if (this.FBaseWriter == null)
                {
                    throw new ObjectDisposedException("IniWriter");
                }
                return this.FBaseWriter;
            }
        }
    }
}

