namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;
    using System.IO;

    internal abstract class ReadArchiveStreamWrapper : Stream
    {
        protected ReadArchiveStreamWrapper()
        {
        }

        public override void Flush()
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override long Length
        {
            get
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
            set
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }
    }
}

