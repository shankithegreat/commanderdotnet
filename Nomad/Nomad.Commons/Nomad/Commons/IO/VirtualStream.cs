namespace Nomad.Commons.IO
{
    using System;
    using System.ComponentModel;
    using System.IO;

    internal class VirtualStream : SimpleVirtualStream
    {
        private uint FirstSector;
        private long FPosition;
        private long Size;

        internal VirtualStream(Stream source, uint[] fat, uint firstSector, int sectorSize, long size, GetSectorOffsetHandler getSectorOffset) : base(source, fat, firstSector, sectorSize, getSectorOffset)
        {
            this.FirstSector = firstSector;
            this.Size = size;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num = (int) Math.Min((long) count, this.Size - this.Position);
            int num2 = base.Read(buffer, offset, num);
            this.FPosition += num2;
            return num2;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.Position = offset;
                    break;

                case SeekOrigin.Current:
                    this.Position += offset;
                    break;

                case SeekOrigin.End:
                    this.Position = this.Size + offset;
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }
            return this.Position;
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return this.Size;
            }
        }

        public override long Position
        {
            get
            {
                return this.FPosition;
            }
            set
            {
                if (value < 0L)
                {
                    value = 0L;
                }
                if (value > this.Size)
                {
                    value = this.Size;
                }
                this.FPosition = value;
                base.StreamChain.Clear();
                base.StreamChain.Add(this.FirstSector);
                long fPosition = this.FPosition;
                base.CurrentSector = this.FirstSector;
                while ((fPosition > base.SectorSize) && (base.CurrentSector != 0xfffffffe))
                {
                    fPosition -= base.SectorSize;
                    base.CurrentSector = base.Fat[base.CurrentSector];
                    if (base.CurrentSector != 0xfffffffe)
                    {
                        base.AddSectorToChain(base.CurrentSector);
                    }
                }
                if (fPosition > base.SectorSize)
                {
                    throw new IOException("Stream chain is less than stream size");
                }
                base.CurrentSectorPos = (int) fPosition;
            }
        }
    }
}

