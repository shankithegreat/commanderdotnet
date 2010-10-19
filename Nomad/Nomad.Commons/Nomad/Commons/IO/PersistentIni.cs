namespace Nomad.Commons.IO
{
    using System;
    using System.IO;

    public class PersistentIni : Ini
    {
        private string FFileName;

        public PersistentIni(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException();
            }
            if (fileName == string.Empty)
            {
                throw new ArgumentException();
            }
            this.FFileName = fileName;
        }

        public virtual void Read()
        {
            if (File.Exists(this.FFileName))
            {
                using (TextReader reader = new StreamReader(this.FFileName))
                {
                    base.Read(reader);
                }
            }
        }

        public virtual void Write()
        {
            using (TextWriter writer = new StreamWriter(this.FFileName))
            {
                base.Write(writer);
            }
        }

        public string FileName
        {
            get
            {
                return this.FFileName;
            }
        }
    }
}

