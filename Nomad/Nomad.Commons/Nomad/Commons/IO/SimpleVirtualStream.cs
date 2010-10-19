namespace Nomad.Commons.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class SimpleVirtualStream : Stream
    {
        protected uint CurrentSector;
        protected int CurrentSectorPos;
        protected readonly uint[] Fat;
        private GetSectorOffsetHandler GetSectorOffset;
        protected readonly int SectorSize;
        private Stream Source;
        protected readonly List<uint> StreamChain;

        internal SimpleVirtualStream(Stream source, uint[] fat, uint firstSector, int sectorSize, GetSectorOffsetHandler getSectorOffset)
        {
            this.Source = source;
            this.Fat = fat;
            this.CurrentSector = firstSector;
            this.SectorSize = sectorSize;
            this.GetSectorOffset = getSectorOffset;
            this.StreamChain = new List<uint>();
            this.StreamChain.Add(firstSector);
        }

        protected void AddSectorToChain(uint sector)
        {
            if (this.StreamChain.Contains(sector))
            {
                throw new CyclicStreamChainException("Stream chain is cycled");
            }
            this.StreamChain.Add(sector);
        }

        public override void Flush()
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.CurrentSector == 0xfffffffe)
            {
                return 0;
            }
            if (buffer == null)
            {
                throw new ArgumentException();
            }
            this.Source.Seek(this.GetSectorOffset(this.CurrentSector) + this.CurrentSectorPos, SeekOrigin.Begin);
            int num = 0;
            while ((num < count) && (this.CurrentSector != 0xfffffffe))
            {
                int num2 = Math.Min((int) (count - num), (int) (this.SectorSize - this.CurrentSectorPos));
                this.Source.Read(buffer, offset + num, num2);
                num += num2;
                this.CurrentSectorPos += num2;
                if (this.CurrentSectorPos == this.SectorSize)
                {
                    this.CurrentSector = this.Fat[this.CurrentSector];
                    if (this.CurrentSector != 0xfffffffe)
                    {
                        this.AddSectorToChain(this.CurrentSector);
                        this.Source.Seek(this.GetSectorOffset(this.CurrentSector), SeekOrigin.Begin);
                        this.CurrentSectorPos = 0;
                    }
                }
            }
            return num;
        }

        public override long Seek(long offset, SeekOrigin origin)
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

