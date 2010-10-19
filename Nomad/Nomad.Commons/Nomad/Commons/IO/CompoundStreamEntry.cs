namespace Nomad.Commons.IO
{
    using System;
    using System.IO;

    public class CompoundStreamEntry : CompoundEntry
    {
        public Stream OpenRead()
        {
            return base.Owner.GetCompoundStream(base.Entry);
        }

        public long Size
        {
            get
            {
                return CompoundFile.GetEntrySize(base.Entry);
            }
        }

        public CompoundStorageEntry Storage
        {
            get
            {
                return base.GetParentStorage();
            }
        }
    }
}

