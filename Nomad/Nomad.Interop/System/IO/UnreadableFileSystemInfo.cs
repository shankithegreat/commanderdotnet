namespace System.IO
{
    using System;

    public class UnreadableFileSystemInfo : FileSystemInfo
    {
        private string _FullName;
        private string _Name;

        public UnreadableFileSystemInfo(string fullName, string name)
        {
            this._FullName = fullName;
            this._Name = name;
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override bool Exists
        {
            get
            {
                return false;
            }
        }

        public override string FullName
        {
            get
            {
                return this._FullName;
            }
        }

        public override string Name
        {
            get
            {
                return this._Name;
            }
        }
    }
}

