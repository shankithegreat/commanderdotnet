namespace Microsoft.COM
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    public class ComStreamWrapper : Stream
    {
        private FileAccess FAccess;
        private IStream FBaseStream;
        private bool? FCanSeek;
        private long? FSize;
        private ComRelease FStreamRelease;

        public event EventHandler Closed;

        public ComStreamWrapper(IStream baseStream, FileAccess access, ComRelease release)
        {
            if (baseStream == null)
            {
                throw new ArgumentNullException("baseStream");
            }
            if (!Enum.IsDefined(typeof(FileAccess), access))
            {
                throw new ArgumentException("access");
            }
            this.FBaseStream = baseStream;
            this.FAccess = access;
            this.FStreamRelease = release;
        }

        public override void Close()
        {
            if (this.FBaseStream != null)
            {
                switch (this.FStreamRelease)
                {
                    case ComRelease.Single:
                        Marshal.ReleaseComObject(this.FBaseStream);
                        break;

                    case ComRelease.Final:
                        Marshal.FinalReleaseComObject(this.FBaseStream);
                        break;
                }
                this.FBaseStream = null;
                if (this.Closed != null)
                {
                    this.Closed(this, EventArgs.Empty);
                }
            }
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num2;
            if (this.FBaseStream == null)
            {
                throw new ObjectDisposedException("ComStreamWrapper");
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            IntPtr pcbRead = Marshal.AllocHGlobal(4);
            try
            {
                if (offset == 0)
                {
                    this.FBaseStream.Read(buffer, count, pcbRead);
                    return Marshal.ReadInt32(pcbRead);
                }
                byte[] pv = new byte[count];
                this.FBaseStream.Read(pv, count, pcbRead);
                int length = Marshal.ReadInt32(pcbRead);
                Array.Copy(pv, 0, buffer, offset, length);
                num2 = length;
            }
            finally
            {
                Marshal.FreeHGlobal(pcbRead);
            }
            return num2;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long num;
            if (this.FBaseStream == null)
            {
                throw new ObjectDisposedException("ComStreamWrapper");
            }
            if (!Enum.IsDefined(typeof(SeekOrigin), origin))
            {
                throw new ArgumentOutOfRangeException("origin");
            }
            IntPtr plibNewPosition = Marshal.AllocHGlobal(8);
            try
            {
                this.FBaseStream.Seek(offset, (int) origin, plibNewPosition);
                num = Marshal.ReadInt64(plibNewPosition);
            }
            finally
            {
                Marshal.FreeHGlobal(plibNewPosition);
            }
            return num;
        }

        public override void SetLength(long value)
        {
            if (this.FBaseStream == null)
            {
                throw new ObjectDisposedException("ComStreamWrapper");
            }
            if (value < 0L)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.FBaseStream.SetSize(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.FBaseStream == null)
            {
                throw new ObjectDisposedException("ComStreamWrapper");
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (offset == 0)
            {
                this.FBaseStream.Write(buffer, count, IntPtr.Zero);
            }
            else
            {
                byte[] pv = new byte[count];
                this.FBaseStream.Write(pv, count, IntPtr.Zero);
                Array.Copy(pv, 0, buffer, offset, count);
            }
        }

        public IStream BaseStream
        {
            get
            {
                return this.FBaseStream;
            }
        }

        public override bool CanRead
        {
            get
            {
                return (this.FAccess != FileAccess.Write);
            }
        }

        public override bool CanSeek
        {
            get
            {
                if (this.FBaseStream == null)
                {
                    throw new ObjectDisposedException("ComStreamWrapper");
                }
                if (!this.FCanSeek.HasValue)
                {
                    try
                    {
                        this.Seek(0L, SeekOrigin.Current);
                        this.FCanSeek = true;
                    }
                    catch (NotImplementedException)
                    {
                        this.FCanSeek = false;
                    }
                }
                return this.FCanSeek.Value;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return (this.FAccess != FileAccess.Read);
            }
        }

        public override long Length
        {
            get
            {
                if (this.FBaseStream == null)
                {
                    throw new ObjectDisposedException("ComStreamWrapper");
                }
                if (!this.CanSeek)
                {
                    throw new NotImplementedException();
                }
                if (!this.FSize.HasValue)
                {
                    try
                    {
                        System.Runtime.InteropServices.ComTypes.STATSTG statstg;
                        this.FBaseStream.Stat(out statstg, 1);
                        this.FSize = new long?(statstg.cbSize);
                    }
                    catch (SystemException exception)
                    {
                        if ((!(exception is NotImplementedException) && !(exception is NotSupportedException)) && (Marshal.GetHRForException(exception) != -2147287039))
                        {
                            throw;
                        }
                        long position = this.Position;
                        this.FSize = new long?(this.Seek(0L, SeekOrigin.End));
                        this.Position = position;
                    }
                }
                return this.FSize.Value;
            }
        }

        public override long Position
        {
            get
            {
                return this.Seek(0L, SeekOrigin.Current);
            }
            set
            {
                if (this.FBaseStream == null)
                {
                    throw new ObjectDisposedException("ComStreamWrapper");
                }
                this.FBaseStream.Seek(value, 0, IntPtr.Zero);
            }
        }
    }
}

