namespace Nomad.Commons.IO
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class CompoundFile : IDisposable
    {
        private uint _ulMiniSectorCutoff;
        private ushort _uMiniSectorShift;
        private ushort _uSectorShift;
        private const uint DIFSECT = 0xfffffffc;
        private CompoundFileDirectoryEntry[] Directory;
        internal const uint ENDOFCHAIN = 0xfffffffe;
        private uint[] Fat;
        private const uint FATSECT = 0xfffffffd;
        private const uint FREESECT = uint.MaxValue;
        private long HeaderStart;
        private uint[] MiniFat;
        private Stream MiniStream;
        private CompoundStorageEntry RootEntry;
        private int SectorSize;
        private Stream Source;
        public readonly System.Version Version;

        public CompoundFile(Stream source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            if (!(source.CanRead && source.CanSeek))
            {
                throw new ArgumentException();
            }
            this.Source = source;
            this.HeaderStart = source.Position;
            if ((source.Length - this.HeaderStart) < 0x200L)
            {
                throw new ArgumentException();
            }
            BinaryReader reader = new BinaryReader(new BufferedStream(source, 0x200));
            byte[] buffer = reader.ReadBytes(8);
            if (((((buffer[0] != 0xd0) || (buffer[1] != 0xcf)) || ((buffer[2] != 0x11) || (buffer[3] != 0xe0))) || (((buffer[4] != 0xa1) || (buffer[5] != 0xb1)) || (buffer[6] != 0x1a))) || (buffer[7] != 0xe1))
            {
                throw new CompoundFileCorruptException("Invalid signature");
            }
            reader.ReadBytes(0x10);
            ushort minor = reader.ReadUInt16();
            ushort major = reader.ReadUInt16();
            this.Version = new System.Version(major, minor);
            if (reader.ReadUInt16() != 0xfffe)
            {
                throw new CompoundFileCorruptException("Invalid byte order value (only intel byte-ordering supported)");
            }
            this._uSectorShift = reader.ReadUInt16();
            this._uMiniSectorShift = reader.ReadUInt16();
            this.SectorSize = ((int) 1) << this._uSectorShift;
            if (this._uSectorShift < 7)
            {
                throw new CompoundFileCorruptException("Invalid sector size");
            }
            reader.ReadUInt16();
            reader.ReadUInt32();
            reader.ReadUInt32();
            uint num4 = reader.ReadUInt32();
            uint startSector = reader.ReadUInt32();
            reader.ReadUInt32();
            this._ulMiniSectorCutoff = reader.ReadUInt32();
            uint num6 = reader.ReadUInt32();
            uint sectorCount = reader.ReadUInt32();
            uint num8 = reader.ReadUInt32();
            uint num9 = reader.ReadUInt32();
            this.Fat = new uint[(this.SectorSize / 4) * num4];
            uint fatPosition = 0;
            for (int i = 0; i < Math.Min(num4, 0x6d); i++)
            {
                this.ReadFatSector(reader.ReadUInt32(), ref fatPosition);
            }
            if (num8 != 0xfffffffe)
            {
                uint sector = num9;
                List<uint> list = new List<uint> {
                    sector
                };
                while (sector != 0xfffffffe)
                {
                    this.ReadFatSector(sector, ref fatPosition);
                    fatPosition--;
                    sector = this.Fat[fatPosition];
                    if (list.Contains(sector))
                    {
                        throw new CyclicStreamChainException("DIFAT chain is cycled");
                    }
                    list.Add(sector);
                }
            }
            this.ReadMiniFat(num6, sectorCount);
            this.ReadDirectory(startSector);
        }

        public CompoundFile(string fileName) : this(new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
        }

        public void Close()
        {
            if (this.Source != null)
            {
                this.Source.Close();
            }
            this.Source = null;
            this.Fat = null;
            this.MiniFat = null;
            this.MiniStream = null;
        }

        public void Dispose()
        {
            this.Close();
        }

        public IEnumerable<CompoundEntry> GetAllEntries()
        {
            return new <GetAllEntries>d__0(-2) { <>4__this = this };
        }

        internal Stream GetCompoundStream(CompoundFileDirectoryEntry entry)
        {
            if (entry._mse != STGTY.STGTY_STREAM)
            {
                throw new ArgumentException();
            }
            if (this.Source == null)
            {
                throw new ObjectDisposedException("CompoundFile");
            }
            if (entry._ulSizeLow < this._ulMiniSectorCutoff)
            {
                if (this.MiniStream == null)
                {
                    CompoundFileDirectoryEntry entry2 = this.Directory[0];
                    this.MiniStream = new VirtualStream(this.Source, this.Fat, entry2._sectStart, this.SectorSize, GetEntrySize(entry2), new GetSectorOffsetHandler(this.GetSectorOffset));
                }
                return new VirtualStream(this.MiniStream, this.MiniFat, entry._sectStart, ((int) 1) << this._uMiniSectorShift, (long) ((entry._ulSizeHigh << 0x20) | entry._ulSizeLow), new GetSectorOffsetHandler(this.GetMiniSectorOffset));
            }
            return new VirtualStream(this.Source, this.Fat, entry._sectStart, this.SectorSize, GetEntrySize(entry), new GetSectorOffsetHandler(this.GetSectorOffset));
        }

        internal static long GetEntrySize(CompoundFileDirectoryEntry entry)
        {
            return (long) ((entry._ulSizeHigh << 0x20) | entry._ulSizeLow);
        }

        private long GetMiniSectorOffset(uint sector)
        {
            return (long) (sector << this._uMiniSectorShift);
        }

        private long GetSectorOffset(uint sector)
        {
            return (long) ((this.HeaderStart + ((ulong) (sector + 1))) << this._uSectorShift);
        }

        private void ReadDirectory(uint startSector)
        {
            Stream stream = new BufferedStream(new SimpleVirtualStream(this.Source, this.Fat, startSector, this.SectorSize, new GetSectorOffsetHandler(this.GetSectorOffset)), this.SectorSize);
            List<CompoundFileDirectoryEntry> list = new List<CompoundFileDirectoryEntry>();
            byte[] buffer = new byte[0x80];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = handle.AddrOfPinnedObject();
                for (int i = stream.Read(buffer, 0, buffer.Length); i > 0; i = stream.Read(buffer, 0, buffer.Length))
                {
                    CompoundFileDirectoryEntry item = (CompoundFileDirectoryEntry) Marshal.PtrToStructure(ptr, typeof(CompoundFileDirectoryEntry));
                    if (!Enum.IsDefined(typeof(STGTY), item._mse))
                    {
                        throw new CompoundFileCorruptException("Unknown entry type");
                    }
                    if (!Enum.IsDefined(typeof(DECOLOR), item._bflags))
                    {
                        throw new CompoundFileCorruptException("Unknown entry color");
                    }
                    list.Add(item);
                }
            }
            finally
            {
                handle.Free();
            }
            this.Directory = list.ToArray();
        }

        private void ReadFatSector(uint sector, ref uint fatPosition)
        {
            this.Source.Seek(this.GetSectorOffset(sector), SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(new BufferedStream(this.Source, this.SectorSize));
            int num = this.SectorSize / 4;
            for (int i = 0; i < num; i++)
            {
                this.Fat[fatPosition++] = reader.ReadUInt32();
            }
        }

        private void ReadMiniFat(uint startSector, uint sectorCount)
        {
            if (startSector != 0xfffffffe)
            {
                this.MiniFat = new uint[(this.SectorSize / 4) * sectorCount];
                BinaryReader reader = new BinaryReader(new BufferedStream(new SimpleVirtualStream(this.Source, this.Fat, startSector, this.SectorSize, new GetSectorOffsetHandler(this.GetSectorOffset)), this.SectorSize));
                int num = 0;
                while (num < this.MiniFat.Length)
                {
                    this.MiniFat[num++] = reader.ReadUInt32();
                }
            }
        }

        public CompoundStorageEntry Root
        {
            get
            {
                if (this.RootEntry == null)
                {
                    this.RootEntry = (CompoundStorageEntry) CompoundEntry.CreateEntry(this, this.Directory, 0);
                }
                return this.RootEntry;
            }
        }

        [CompilerGenerated]
        private sealed class <GetAllEntries>d__0 : IEnumerable<CompoundEntry>, IEnumerable, IEnumerator<CompoundEntry>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private CompoundEntry <>2__current;
            public CompoundFile <>4__this;
            private int <>l__initialThreadId;
            public CompoundEntry <Entry>5__2;
            public uint <I>5__1;

            [DebuggerHidden]
            public <GetAllEntries>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        if (this.<>4__this.Directory == null)
                        {
                            throw new ObjectDisposedException("CompoundFile");
                        }
                        this.<I>5__1 = 1;
                        while (this.<I>5__1 < this.<>4__this.Directory.Length)
                        {
                            this.<Entry>5__2 = CompoundEntry.CreateEntry(this.<>4__this, this.<>4__this.Directory, this.<I>5__1);
                            if (this.<Entry>5__2 == null)
                            {
                                goto Label_009F;
                            }
                            this.<>2__current = this.<Entry>5__2;
                            this.<>1__state = 1;
                            return true;
                        Label_0098:
                            this.<>1__state = -1;
                        Label_009F:
                            this.<I>5__1++;
                        }
                        break;

                    case 1:
                        goto Label_0098;
                }
                return false;
            }

            [DebuggerHidden]
            IEnumerator<CompoundEntry> IEnumerable<CompoundEntry>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new CompoundFile.<GetAllEntries>d__0(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.Commons.IO.CompoundEntry>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            CompoundEntry IEnumerator<CompoundEntry>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

