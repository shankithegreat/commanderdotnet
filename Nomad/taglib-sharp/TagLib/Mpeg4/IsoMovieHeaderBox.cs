namespace TagLib.Mpeg4
{
    using System;
    using TagLib;

    public class IsoMovieHeaderBox : FullBox
    {
        private ulong creation_time;
        private ulong duration;
        private ulong modification_time;
        private uint next_track_id;
        private uint rate;
        private uint timescale;
        private ushort volume;

        public IsoMovieHeaderBox(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, file, handler)
        {
            ByteVector vector;
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            int dataSize = base.DataSize;
            if (base.Version == 1)
            {
                vector = file.ReadBlock(Math.Min(0x1c, dataSize));
                if (vector.Count >= 8)
                {
                    this.creation_time = vector.Mid(0, 8).ToULong();
                }
                if (vector.Count >= 0x10)
                {
                    this.modification_time = vector.Mid(8, 8).ToULong();
                }
                if (vector.Count >= 20)
                {
                    this.timescale = vector.Mid(0x10, 4).ToUInt();
                }
                if (vector.Count >= 0x1c)
                {
                    this.duration = vector.Mid(20, 8).ToULong();
                }
                dataSize -= 0x1c;
            }
            else
            {
                vector = file.ReadBlock(Math.Min(0x10, dataSize));
                if (vector.Count >= 4)
                {
                    this.creation_time = vector.Mid(0, 4).ToUInt();
                }
                if (vector.Count >= 8)
                {
                    this.modification_time = vector.Mid(4, 4).ToUInt();
                }
                if (vector.Count >= 12)
                {
                    this.timescale = vector.Mid(8, 4).ToUInt();
                }
                if (vector.Count >= 0x10)
                {
                    this.duration = vector.Mid(12, 4).ToUInt();
                }
                dataSize -= 0x10;
            }
            vector = file.ReadBlock(Math.Min(6, dataSize));
            if (vector.Count >= 4)
            {
                this.rate = vector.Mid(0, 4).ToUInt();
            }
            if (vector.Count >= 6)
            {
                this.volume = vector.Mid(4, 2).ToUShort();
            }
            file.Seek(file.Tell + 70L);
            dataSize -= 0x4c;
            vector = file.ReadBlock(Math.Min(4, dataSize));
            if (vector.Count >= 4)
            {
                this.next_track_id = vector.Mid(0, 4).ToUInt();
            }
        }

        public DateTime CreationTime
        {
            get
            {
                DateTime time = new DateTime(0x770, 1, 1, 0, 0, 0);
                return time.AddTicks((long) (((ulong) 0x989680L) * this.creation_time));
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromSeconds(((double) this.duration) / ((double) this.timescale));
            }
        }

        public DateTime ModificationTime
        {
            get
            {
                DateTime time = new DateTime(0x770, 1, 1, 0, 0, 0);
                return time.AddTicks((long) (((ulong) 0x989680L) * this.modification_time));
            }
        }

        public uint NextTrackId
        {
            get
            {
                return this.next_track_id;
            }
        }

        public double Rate
        {
            get
            {
                return (((double) this.rate) / 65536.0);
            }
        }

        public double Volume
        {
            get
            {
                return (((double) this.volume) / 256.0);
            }
        }
    }
}

