namespace Nomad.Commons.IO
{
    using System;
    using System.IO;

    public abstract class CustomStreamWrapper : Stream
    {
        protected CustomStreamWrapper()
        {
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this.BaseStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this.BaseStream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Close()
        {
            this.BaseStream.Close();
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return this.BaseStream.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            this.BaseStream.EndWrite(asyncResult);
        }

        public override void Flush()
        {
            this.BaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.BaseStream.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            return this.BaseStream.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.BaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.BaseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.BaseStream.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            this.BaseStream.WriteByte(value);
        }

        protected abstract Stream BaseStream { get; }

        public override bool CanRead
        {
            get
            {
                return this.BaseStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this.BaseStream.CanSeek;
            }
        }

        public override bool CanTimeout
        {
            get
            {
                return this.BaseStream.CanTimeout;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this.BaseStream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return this.BaseStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return this.BaseStream.Position;
            }
            set
            {
                this.BaseStream.Position = value;
            }
        }

        public override int ReadTimeout
        {
            get
            {
                return this.BaseStream.ReadTimeout;
            }
            set
            {
                this.BaseStream.ReadTimeout = value;
            }
        }

        public override int WriteTimeout
        {
            get
            {
                return this.BaseStream.WriteTimeout;
            }
            set
            {
                this.BaseStream.WriteTimeout = value;
            }
        }
    }
}

