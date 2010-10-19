namespace Nomad.Commons.IO
{
    using System;
    using System.ComponentModel;
    using System.IO;

    public class SubStream : StreamWrapper
    {
        private long LogicalLength;
        private long StartPosition;
        private FileAccess StreamAccess;

        public SubStream(Stream baseStream) : this(baseStream, FileAccess.ReadWrite, -1L)
        {
        }

        public SubStream(Stream baseStream, long size) : this(baseStream, FileAccess.ReadWrite, size)
        {
        }

        public SubStream(Stream baseStream, FileAccess access) : this(baseStream, access, -1L)
        {
        }

        public SubStream(Stream baseStream, FileAccess access, long size) : base(baseStream)
        {
            if (size == 0L)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.StreamAccess = access;
            this.LogicalLength = size;
            if (this.CanSeek)
            {
                this.StartPosition = base.Position;
                if (this.LogicalLength < 0L)
                {
                    this.LogicalLength = base.Length - this.StartPosition;
                }
            }
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if ((this.StreamAccess & FileAccess.Read) == 0)
            {
                throw new NotSupportedException();
            }
            if ((this.LogicalLength > 0L) && ((this.Position + count) > this.LogicalLength))
            {
                count = (int) (this.LogicalLength - this.Position);
            }
            return base.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if ((this.StreamAccess & FileAccess.Write) == 0)
            {
                throw new NotSupportedException();
            }
            if ((this.LogicalLength > 0L) && ((this.Position + count) > this.LogicalLength))
            {
                throw new IOException();
            }
            return base.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Close()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if ((this.StreamAccess & FileAccess.Read) == 0)
            {
                throw new NotSupportedException();
            }
            if ((this.LogicalLength > 0L) && ((this.Position + count) > this.LogicalLength))
            {
                count = (int) (this.LogicalLength - this.Position);
            }
            return base.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    if (offset < 0L)
                    {
                        throw new InvalidOperationException();
                    }
                    if ((this.LogicalLength > 0L) && (offset > this.LogicalLength))
                    {
                        offset = this.LogicalLength;
                    }
                    return (base.Seek(offset + this.StartPosition, SeekOrigin.Begin) - this.StartPosition);

                case SeekOrigin.Current:
                    if (this.LogicalLength > 0L)
                    {
                        if ((this.Position + offset) <= this.LogicalLength)
                        {
                            if ((this.Position + offset) < 0L)
                            {
                                offset = -this.Position;
                            }
                            break;
                        }
                        offset = this.LogicalLength - this.Position;
                    }
                    break;

                case SeekOrigin.End:
                    if (offset > 0L)
                    {
                        throw new InvalidOperationException();
                    }
                    if (this.LogicalLength > 0L)
                    {
                        if (-offset > this.LogicalLength)
                        {
                            offset = -this.LogicalLength;
                        }
                        return (base.Seek((this.StartPosition + this.LogicalLength) + offset, SeekOrigin.Begin) - this.StartPosition);
                    }
                    return (base.Seek(offset, SeekOrigin.End) - this.StartPosition);

                default:
                    throw new InvalidEnumArgumentException();
            }
            return (base.Seek(offset, SeekOrigin.Current) - this.StartPosition);
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if ((this.StreamAccess & FileAccess.Write) == 0)
            {
                throw new NotSupportedException();
            }
            if ((this.LogicalLength > 0L) && ((this.Position + count) > this.LogicalLength))
            {
                throw new IOException();
            }
            base.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get
            {
                return (base.CanRead && ((this.StreamAccess & FileAccess.Read) > 0));
            }
        }

        public override bool CanWrite
        {
            get
            {
                return (base.CanWrite && ((this.StreamAccess & FileAccess.Write) > 0));
            }
        }

        public override long Length
        {
            get
            {
                return ((this.LogicalLength < 0L) ? base.Length : this.LogicalLength);
            }
        }

        public override long Position
        {
            get
            {
                return (base.Position - this.StartPosition);
            }
            set
            {
                if ((this.LogicalLength > 0L) && (value > this.LogicalLength))
                {
                    value = this.LogicalLength;
                }
                base.Position = value + this.StartPosition;
            }
        }
    }
}

