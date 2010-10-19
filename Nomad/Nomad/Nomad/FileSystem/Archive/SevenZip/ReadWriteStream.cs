namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal class ReadWriteStream : ReadArchiveStreamWrapper, ISequentialOutStream
    {
        private bool CanSeekFromBeginning = true;
        private EventWaitHandle CloseEvent = new ManualResetEvent(false);
        private byte[] data;
        private ReadWriteExtractCallback ExtractCallback;
        private int offset;
        private EventWaitHandle ReadEvent = new AutoResetEvent(false);
        private int size;
        private WaitHandle[] WaitForReadOrClosed;
        private WaitHandle[] WaitForWriteOrFinished;
        private EventWaitHandle WriteCompletedEvent = new ManualResetEvent(false);
        private EventWaitHandle WriteEvent = new ManualResetEvent(false);

        internal ReadWriteStream(ReadWriteExtractCallback extractCallback)
        {
            this.ExtractCallback = extractCallback;
            this.WaitForWriteOrFinished = new WaitHandle[] { this.WriteEvent, this.WriteCompletedEvent };
            this.WaitForReadOrClosed = new WaitHandle[] { this.ReadEvent, this.CloseEvent };
        }

        private bool CheckWriteOrFinished()
        {
            WaitHandle.WaitAny(this.WaitForWriteOrFinished);
            if (!this.WriteEvent.WaitOne(0, false))
            {
                this.ExtractCallback.ThrowExceptionForError();
                return true;
            }
            return false;
        }

        public override void Close()
        {
            this.CloseEvent.Set();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.CheckWriteOrFinished())
            {
                return 0;
            }
            this.CanSeekFromBeginning = false;
            int length = Math.Min(count, this.size - this.offset);
            Array.Copy(this.data, this.offset, buffer, offset, length);
            this.offset += length;
            if (this.offset == this.size)
            {
                this.WriteEvent.Reset();
                this.ReadEvent.Set();
            }
            return length;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (offset != 0L)
            {
                if ((offset < 0L) || ((origin != SeekOrigin.Current) && (!this.CanSeekFromBeginning || (origin != SeekOrigin.Begin))))
                {
                    throw new ArgumentException();
                }
                if (this.CheckWriteOrFinished())
                {
                    return 0L;
                }
                this.CanSeekFromBeginning = false;
                while (offset >= (this.size - this.offset))
                {
                    offset -= this.size - this.offset;
                    this.WriteEvent.Reset();
                    this.ReadEvent.Set();
                    if (this.CheckWriteOrFinished())
                    {
                        return 0L;
                    }
                }
                this.offset += (int) offset;
            }
            return 0L;
        }

        public int Write(byte[] data, uint size, IntPtr processedSize)
        {
            this.data = data;
            this.offset = 0;
            this.size = (int) size;
            this.WriteEvent.Set();
            WaitHandle.WaitAny(this.WaitForReadOrClosed);
            if (processedSize != IntPtr.Zero)
            {
                Marshal.WriteInt32(processedSize, (int) size);
            }
            if (!(!this.CloseEvent.WaitOne(0, false) || this.WriteCompletedEvent.WaitOne(0, false)))
            {
                return -2147467260;
            }
            return 0;
        }

        public void WriteCompleted()
        {
            this.WriteCompletedEvent.Set();
        }
    }
}

