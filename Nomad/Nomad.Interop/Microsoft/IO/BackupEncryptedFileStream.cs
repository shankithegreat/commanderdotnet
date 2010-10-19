namespace Microsoft.IO
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class BackupEncryptedFileStream : Stream
    {
        private bool Backup;
        private EventWaitHandle DataRead = new AutoResetEvent(false);
        private EventWaitHandle DataWrite = new AutoResetEvent(false);
        private bool Eof;
        private SafeEncryptedFileHandle Handle;
        private byte[] InternalBuffer;
        private int InternalBufferPos;
        private Thread ProcessThread;
        private EventWaitHandle StreamClosed = new ManualResetEvent(false);
        private int WriteCount;

        public BackupEncryptedFileStream(string fileName, FileMode mode)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if ((mode != FileMode.Create) && (mode != FileMode.Open))
            {
                throw new ArgumentException("mode");
            }
            this.Backup = mode == FileMode.Open;
            int error = AdvApi.OpenEncryptedFileRaw(fileName, this.Backup ? OEFR.OPEN_FOR_BACKUP : OEFR.CREATE_FOR_IMPORT, out this.Handle);
            if (error != 0)
            {
                throw new Win32Exception(error);
            }
        }

        public override void Close()
        {
            if (!this.Closed)
            {
                if (this.ProcessThread != null)
                {
                    this.StreamClosed.Set();
                    this.ProcessThread.Join();
                }
                this.Handle.Close();
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.Closed)
            {
                throw new ObjectDisposedException(null);
            }
            if (!this.Backup)
            {
                throw new InvalidOperationException();
            }
            if (this.Eof)
            {
                return 0;
            }
            if (this.ProcessThread == null)
            {
                this.ProcessThread = new Thread(new ThreadStart(this.ReadThreadExecute));
                this.ProcessThread.Start();
            }
            if ((this.InternalBuffer == null) || (this.InternalBufferPos >= this.InternalBuffer.Length))
            {
                this.DataWrite.Set();
                this.DataRead.WaitOne();
            }
            if (this.Eof)
            {
                return 0;
            }
            int length = Math.Min(count, this.InternalBuffer.Length - this.InternalBufferPos);
            Array.Copy(this.InternalBuffer, this.InternalBufferPos, buffer, offset, length);
            this.InternalBufferPos += length;
            if (this.InternalBufferPos >= this.InternalBuffer.Length)
            {
                this.InternalBuffer = null;
            }
            return length;
        }

        private int ReadCallback(byte[] pbData, IntPtr pvCallbackContext, uint ulLength)
        {
            WaitHandle.WaitAny(new WaitHandle[] { this.DataWrite, this.StreamClosed });
            if (this.Closed)
            {
                return 0xe8;
            }
            if (ulLength > 0)
            {
                this.InternalBuffer = new byte[ulLength];
                this.InternalBufferPos = 0;
                Array.Copy(pbData, this.InternalBuffer, (long) ulLength);
            }
            else
            {
                this.Eof = true;
            }
            this.DataRead.Set();
            return 0;
        }

        private void ReadThreadExecute()
        {
            int error = AdvApi.ReadEncryptedFileRaw(new ExportCallback(this.ReadCallback), IntPtr.Zero, this.Handle);
            if ((error != 0) && (error != 0xe8))
            {
                throw new Win32Exception(error);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
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
            if (this.Backup)
            {
                throw new InvalidOperationException();
            }
            if (count != 0)
            {
                if (this.ProcessThread == null)
                {
                    this.ProcessThread = new Thread(new ThreadStart(this.WriteThreadExecute));
                    this.ProcessThread.Start();
                }
                this.InternalBuffer = buffer;
                this.InternalBufferPos = offset;
                this.WriteCount = count;
                this.DataWrite.Set();
                this.DataRead.WaitOne();
            }
        }

        private int WriteCallback(IntPtr pbData, IntPtr pvCallbackContext, ref uint ulLength)
        {
            WaitHandle.WaitAny(new WaitHandle[] { this.DataWrite, this.StreamClosed });
            if (this.Closed)
            {
                ulLength = 0;
                return 0;
            }
            int length = Math.Min(this.WriteCount, (int) ulLength);
            Marshal.Copy(this.InternalBuffer, this.InternalBufferPos, pbData, length);
            ulLength = (uint) length;
            this.InternalBufferPos += length;
            this.WriteCount -= length;
            if (this.WriteCount > 0)
            {
                this.DataWrite.Set();
            }
            else
            {
                this.DataRead.Set();
            }
            return 0;
        }

        private void WriteThreadExecute()
        {
            AdvApi.WriteEncryptedFileRaw(new ImportCallback(this.WriteCallback), IntPtr.Zero, this.Handle);
        }

        public override bool CanRead
        {
            get
            {
                return (!this.Closed && this.Backup);
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
                return (!this.Closed && !this.Backup);
            }
        }

        private bool Closed
        {
            get
            {
                return this.StreamClosed.WaitOne(0, false);
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

