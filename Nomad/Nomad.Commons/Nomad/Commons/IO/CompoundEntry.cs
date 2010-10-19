namespace Nomad.Commons.IO
{
    using System;

    public abstract class CompoundEntry
    {
        protected uint DID;
        protected CompoundFileDirectoryEntry[] Directory;
        private CompoundFile FOwner;

        protected CompoundEntry()
        {
        }

        internal static CompoundEntry CreateEntry(CompoundFile owner, CompoundFileDirectoryEntry[] directory, uint did)
        {
            CompoundEntry entry = null;
            switch (directory[did]._mse)
            {
                case STGTY.STGTY_STORAGE:
                case STGTY.STGTY_ROOT:
                    entry = new CompoundStorageEntry();
                    break;

                case STGTY.STGTY_STREAM:
                    entry = new CompoundStreamEntry();
                    break;
            }
            if (entry != null)
            {
                entry.FOwner = owner;
                entry.Directory = directory;
                entry.DID = did;
            }
            return entry;
        }

        protected CompoundStorageEntry GetParentStorage()
        {
            if (this.DID == 0)
            {
                return null;
            }
            uint dID = this.DID;
            while (true)
            {
                for (uint i = 0; i < this.Directory.Length; i++)
                {
                    if (this.Directory[i]._sidChild == dID)
                    {
                        return (CompoundStorageEntry) CreateEntry(this.Owner, this.Directory, i);
                    }
                    if ((this.Directory[i]._sidLeftSib == dID) || (this.Directory[i]._sidRightSib == dID))
                    {
                        dID = i;
                        break;
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", this.EntryType, this.Name);
        }

        public DateTime CreationTime
        {
            get
            {
                long fileTime = (this.Entry._Createtime.dwHighDateTime << 0x20) | ((long) ((ulong) this.Entry._Createtime.dwLowDateTime));
                return DateTime.FromFileTime(fileTime);
            }
        }

        protected CompoundFileDirectoryEntry Entry
        {
            get
            {
                return this.Directory[this.DID];
            }
        }

        public Nomad.Commons.IO.EntryType EntryType
        {
            get
            {
                return (Nomad.Commons.IO.EntryType) this.Entry._mse;
            }
        }

        public DateTime LastWriteTime
        {
            get
            {
                long fileTime = (this.Entry._Modifytime.dwHighDateTime << 0x20) | ((long) ((ulong) this.Entry._Modifytime.dwLowDateTime));
                return DateTime.FromFileTime(fileTime);
            }
        }

        public string Name
        {
            get
            {
                return this.Entry._ab;
            }
        }

        public CompoundFile Owner
        {
            get
            {
                return this.FOwner;
            }
        }
    }
}

