namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class InStreamTimedWrapper : InStreamWrapper
    {
        private string _BaseStreamFileName;
        private long BaseStreamLastPosition;
        private Timer CloseTimer;
        private const int KeepAliveInterval = 0x1388;

        public InStreamTimedWrapper(Stream baseStream) : base(baseStream)
        {
            FileStream stream = base._BaseStream as FileStream;
            if (((stream != null) && !base._BaseStream.CanWrite) && base._BaseStream.CanSeek)
            {
                this._BaseStreamFileName = stream.Name;
                this.CloseTimer = new Timer(new TimerCallback(this.CloseStream), null, 0x1388, -1);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void CloseStream(object state)
        {
            if (this.CloseTimer != null)
            {
                this.CloseTimer.Dispose();
                this.CloseTimer = null;
            }
            if (base._BaseStream != null)
            {
                if (base._BaseStream.CanSeek)
                {
                    this.BaseStreamLastPosition = base._BaseStream.Position;
                }
                base._BaseStream.Close();
                base._BaseStream = null;
            }
        }

        public override void Dispose()
        {
            this.CloseStream(null);
            this._BaseStreamFileName = null;
        }

        public void Flush()
        {
            this.CloseStream(null);
        }

        public override uint Read(IntPtr data, uint size)
        {
            this.ReopenStream();
            return base.Read(data, size);
        }

        protected void ReopenStream()
        {
            if ((base._BaseStream == null) || !base._BaseStream.CanRead)
            {
                if (this._BaseStreamFileName == null)
                {
                    throw new ObjectDisposedException("StreamWrapper");
                }
                base._BaseStream = new FileStream(this._BaseStreamFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                base._BaseStream.Position = this.BaseStreamLastPosition;
                this.CloseTimer = new Timer(new TimerCallback(this.CloseStream), null, 0x1388, -1);
            }
            else if (this.CloseTimer != null)
            {
                this.CloseTimer.Change(0x1388, -1);
            }
        }

        public override void Seek(long offset, uint seekOrigin, IntPtr newPosition)
        {
            if ((((base._BaseStream == null) && (this._BaseStreamFileName != null)) && (offset == 0L)) && (seekOrigin == 0))
            {
                this.BaseStreamLastPosition = 0L;
                if (newPosition != IntPtr.Zero)
                {
                    Marshal.WriteInt64(newPosition, this.BaseStreamLastPosition);
                }
            }
            else
            {
                this.ReopenStream();
                base.Seek(offset, seekOrigin, newPosition);
            }
        }

        public string BaseStreamFileName
        {
            get
            {
                return this._BaseStreamFileName;
            }
        }
    }
}

