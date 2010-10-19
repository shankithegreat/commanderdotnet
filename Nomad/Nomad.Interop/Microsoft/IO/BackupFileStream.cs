namespace Microsoft.IO
{
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;

    public class BackupFileStream : Stream
    {
        private FileAccess Access;
        private bool Closed;
        private IntPtr Context;
        public readonly Microsoft.Win32.SafeHandles.SafeFileHandle SafeFileHandle;

        public BackupFileStream(Microsoft.Win32.SafeHandles.SafeFileHandle handle, FileAccess access)
        {
            if (handle == null)
            {
                throw new ArgumentNullException();
            }
            if (handle.IsInvalid)
            {
                throw new ArgumentException();
            }
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                throw new PlatformNotSupportedException();
            }
            switch (access)
            {
                case FileAccess.Read:
                case FileAccess.Write:
                    this.SafeFileHandle = handle;
                    this.Access = access;
                    return;

                case FileAccess.ReadWrite:
                    throw new ArgumentException();
            }
            throw new InvalidEnumArgumentException();
        }

        public override void Close()
        {
            if (!this.Closed)
            {
                if (this.Context != IntPtr.Zero)
                {
                    uint num;
                    if (this.Access == FileAccess.Read)
                    {
                        Windows.BackupRead(this.SafeFileHandle, IntPtr.Zero, 0, out num, true, false, ref this.Context);
                    }
                    else
                    {
                        Windows.BackupWrite(this.SafeFileHandle, IntPtr.Zero, 0, out num, true, false, ref this.Context);
                    }
                }
                this.Closed = true;
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num2;
            if (this.Closed)
            {
                throw new ObjectDisposedException(null);
            }
            if (this.Access != FileAccess.Read)
            {
                throw new InvalidOperationException();
            }
            if ((offset + count) > buffer.Length)
            {
                throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                uint num;
                IntPtr lpBuffer = new IntPtr(handle.AddrOfPinnedObject().ToInt64() + offset);
                if (!Windows.BackupRead(this.SafeFileHandle, lpBuffer, (uint) count, out num, false, false, ref this.Context))
                {
                    throw new Win32Exception();
                }
                num2 = (int) num;
            }
            finally
            {
                handle.Free();
            }
            return num2;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            uint num;
            uint num2;
            if (offset < 0L)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (origin != SeekOrigin.Current)
            {
                throw new ArgumentException("Only seek from current supported");
            }
            if (!Windows.BackupSeek(this.SafeFileHandle, (uint) (((ulong) offset) & 0xffffffffL), (uint) (offset >> 0x20), out num, out num2, ref this.Context))
            {
                throw new Win32Exception();
            }
            return (long) ((num2 << 0x20) | num);
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.Closed)
            {
                throw new ObjectDisposedException(null);
            }
            if (this.Access != FileAccess.Write)
            {
                throw new InvalidOperationException();
            }
            if ((offset + count) > buffer.Length)
            {
                throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                uint num;
                IntPtr lpBuffer = new IntPtr(handle.AddrOfPinnedObject().ToInt64() + offset);
                if (!Windows.BackupWrite(this.SafeFileHandle, lpBuffer, (uint) count, out num, false, false, ref this.Context))
                {
                    throw new Win32Exception();
                }
            }
            finally
            {
                handle.Free();
            }
        }

        public override bool CanRead
        {
            get
            {
                return (!this.Closed && (this.Access == FileAccess.Read));
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
                return (!this.Closed && (this.Access == FileAccess.Write));
            }
        }

        public override long Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

