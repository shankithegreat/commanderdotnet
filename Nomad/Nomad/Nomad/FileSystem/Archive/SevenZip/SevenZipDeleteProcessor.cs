namespace Nomad.FileSystem.Archive.SevenZip
{
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.LocalFile;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;

    internal class SevenZipDeleteProcessor : SevenZipProcessor, IArchiveUpdateCallback
    {
        private List<uint> ExistIndexes;
        private IEnumerator<uint> ItemsEnumerator;
        private ulong TotalProcessSize;

        internal SevenZipDeleteProcessor(SevenZipSharedArchiveContext context) : base(context)
        {
        }

        protected override void DoProcess()
        {
            uint numberOfItems = base.Context.GetNumberOfItems();
            this.ExistIndexes = new List<uint>(((int) numberOfItems) - base.Items.Count);
            for (uint i = 0; i < numberOfItems; i++)
            {
                if (!base.Items.ContainsKey(i))
                {
                    this.ExistIndexes.Add(i);
                }
            }
            string path = base.Context.FileName + ".tmp";
            using (Stream stream = base.Context.Proxy.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                base.Context.UpdateItems(new OutStreamWrapper(stream), this.ExistIndexes.Count, this);
            }
            base.Context.Flush();
            File.Delete(base.Context.FileName);
            File.Move(path, base.Context.FileName);
            LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Changed, base.Context.FileName);
            LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Deleted, path);
        }

        public void GetProperty(int index, ItemPropId propID, IntPtr value)
        {
        }

        public void GetStream(int index, out ISequentialInStream inStream)
        {
            inStream = null;
        }

        public void GetUpdateItemInfo(int index, out int newData, out int newProperties, out uint indexInArchive)
        {
            newData = 0;
            newProperties = 0;
            indexInArchive = this.ExistIndexes[index];
        }

        public void SetCompleted(ref ulong completeValue)
        {
            if (this.ItemsEnumerator != null)
            {
                while (this.ItemsEnumerator.MoveNext())
                {
                    ISequenceableItem item = base.Items[this.ItemsEnumerator.Current];
                    base.ItemHandler(new SimpleProcessItemEventArgs(item, base.GetUserState(item)));
                    if (completeValue < this.TotalProcessSize)
                    {
                        break;
                    }
                }
            }
        }

        public void SetOperationResult(int operationResult)
        {
        }

        public void SetTotal(ulong total)
        {
            this.TotalProcessSize = total;
            if (base.ItemHandler != null)
            {
                this.ItemsEnumerator = base.Items.Keys.GetEnumerator();
            }
        }
    }
}

