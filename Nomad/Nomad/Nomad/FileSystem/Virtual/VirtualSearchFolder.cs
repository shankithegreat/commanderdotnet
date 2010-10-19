namespace Nomad.FileSystem.Virtual
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Archive;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public class VirtualSearchFolder : CustomAsyncFolder, IVirtualItemUI, IAsyncVirtualFolder, IVirtualCachedFolder, IVirtualFolderUI, IVirtualFolder, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IDisposable, ISerializable, IGetVirtualRoot, ICreateVirtualLink
    {
        private CheckItemHandler AsyncCheckItem;
        private const string EntryDuplicateOptions = "DuplicateOptions";
        private const string EntryFilter = "Filter";
        protected const string EntryFolder = "Folder";
        private const string EntrySearchOptions = "SearchOptions";
        private FindDuplicateOptions FDuplicateOptions;
        private IVirtualItemFilter FFilter;
        private IVirtualFolder FFolder;
        private IVirtualFolder FParent;
        private SearchFolderOptions FSearchOptions;
        public object Tag;

        private event EventHandler<VirtualItemChangedEventArgs> FChanged;

        public event EventHandler<VirtualItemChangedEventArgs> OnChanged
        {
            add
            {
                if ((this.FChanged == null) && this.CheckOption(SearchFolderOptions.DetectChanges))
                {
                    VirtualItem.VirtualItemChanged = (EventHandler<VirtualItemChangedEventArgs>) Delegate.Combine(VirtualItem.VirtualItemChanged, new EventHandler<VirtualItemChangedEventArgs>(this.VirtualItemChanged));
                }
                this.FChanged = (EventHandler<VirtualItemChangedEventArgs>) Delegate.Combine(this.FChanged, value);
            }
            remove
            {
                this.FChanged = (EventHandler<VirtualItemChangedEventArgs>) Delegate.Remove(this.FChanged, value);
                if (this.FChanged == null)
                {
                    VirtualItem.VirtualItemChanged = (EventHandler<VirtualItemChangedEventArgs>) Delegate.Remove(VirtualItem.VirtualItemChanged, new EventHandler<VirtualItemChangedEventArgs>(this.VirtualItemChanged));
                }
            }
        }

        public event EventHandler<SearchErrorEventArgs> SearchError;

        protected VirtualSearchFolder(SerializationInfo info, StreamingContext context)
        {
            this.FFolder = (IVirtualFolder) info.GetValue("Folder", typeof(IVirtualFolder));
            this.FSearchOptions = (SearchFolderOptions) info.GetInt32("SearchOptions");
            this.FFilter = (IVirtualItemFilter) info.GetValue("Filter", typeof(IVirtualItemFilter));
            this.FDuplicateOptions = (FindDuplicateOptions) info.GetInt32("DuplicateOptions");
            if (!(this.FFolder is AggregatedVirtualFolder))
            {
                this.FParent = this.FFolder;
            }
        }

        public VirtualSearchFolder(IVirtualFolder folder, IVirtualItemFilter filter, SearchFolderOptions searchOptions) : this(folder, filter, searchOptions, 0)
        {
        }

        public VirtualSearchFolder(IVirtualFolder folder, IVirtualItemFilter filter, SearchFolderOptions searchOptions, FindDuplicateOptions duplicateOptions)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }
            ICloneable cloneable = folder as ICloneable;
            if (cloneable != null)
            {
                this.FFolder = (IVirtualFolder) cloneable.Clone();
            }
            else
            {
                this.FFolder = folder;
            }
            if (!(folder is AggregatedVirtualFolder))
            {
                this.FParent = folder;
            }
            this.FFilter = filter;
            this.FSearchOptions = searchOptions;
            this.FDuplicateOptions = duplicateOptions;
        }

        private IAsyncResult BeginCheckItem(IVirtualItem item, IVirtualItem folder, Stack<IVirtualFolder> folderStack, Dictionary<string, Dictionary<long, Dictionary<string, IVirtualItem>>> duplicates)
        {
            if (this.AsyncCheckItem == null)
            {
                this.AsyncCheckItem = new CheckItemHandler(this.CheckItem);
            }
            return this.AsyncCheckItem.BeginInvoke(item, folder, folderStack, duplicates, null, null);
        }

        public LinkType CanCreateLinkIn(IVirtualFolder destFolder)
        {
            return ((destFolder is CustomFileSystemFolder) ? LinkType.Default : LinkType.None);
        }

        private IVirtualItem[] CheckItem(IVirtualItem item, IVirtualItem folder, Stack<IVirtualFolder> folderStack, Dictionary<string, Dictionary<long, Dictionary<string, IVirtualItem>>> duplicates)
        {
            Stack<IVirtualFolder> stack;
            bool flag = (this.FFilter == null) || this.FFilter.IsMatch(item);
            IVirtualFolder folder2 = item as IVirtualFolder;
            if ((((folder2 != null) && (this.CheckOption(SearchFolderOptions.ProcessSubfolders) || ((item is ArchiveFolder) && this.CheckOption(SearchFolderOptions.ProcessArchives)))) && (flag || !this.CheckOption(SearchFolderOptions.SkipUnmatchedSubfolders))) && (((item.Attributes & FileAttributes.ReparsePoint) == 0) || !this.CheckOption(SearchFolderOptions.SkipReparsePoints)))
            {
                lock ((stack = folderStack))
                {
                    folderStack.Push(folder2);
                }
            }
            IChangeVirtualFile archiveFile = item as IChangeVirtualFile;
            if (((archiveFile != null) && archiveFile.CanSeek) && this.CheckOption(SearchFolderOptions.ProcessArchives))
            {
                IVirtualFolder folder3 = VirtualItem.OpenArchive(archiveFile, null);
                if (folder3 != null)
                {
                    lock ((stack = folderStack))
                    {
                        folderStack.Push(folder3);
                    }
                }
            }
            if (flag)
            {
                SimplePropertyBag propertyProvider = null;
                IExtendGetVirtualProperty property = item as IExtendGetVirtualProperty;
                if (property != null)
                {
                    propertyProvider = new SimplePropertyBag(12, folder.FullName);
                    property.AddPropertyProvider(propertyProvider);
                }
                List<IVirtualItem> list = new List<IVirtualItem>(2);
                if (this.FDuplicateOptions != 0)
                {
                    if (folder2 != null)
                    {
                        return null;
                    }
                    string key = string.Empty;
                    if ((this.FDuplicateOptions & FindDuplicateOptions.SameName) > 0)
                    {
                        key = item.Name;
                    }
                    long num = 0L;
                    if ((this.FDuplicateOptions & FindDuplicateOptions.SameSize) > 0)
                    {
                        object obj2 = item[3];
                        if (obj2 == null)
                        {
                            return null;
                        }
                        num = Convert.ToInt64(obj2);
                    }
                    string str2 = string.Empty;
                    if ((this.FDuplicateOptions & FindDuplicateOptions.SameContent) > 0)
                    {
                        byte[] hash;
                        if (archiveFile == null)
                        {
                            return null;
                        }
                        if (archiveFile.IsPropertyAvailable(0x19))
                        {
                            hash = (byte[]) archiveFile[0x19];
                        }
                        else
                        {
                            using (Stream stream = archiveFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.SequentialScan, 0L))
                            {
                                using (HashAlgorithm algorithm = MD5.Create())
                                {
                                    using (Stream stream2 = new CryptoStream(stream, algorithm, CryptoStreamMode.Read))
                                    {
                                        byte[] buffer = new byte[0xf000];
                                        while (stream2.Read(buffer, 0, buffer.Length) > 0)
                                        {
                                            if (base.CancellationPending)
                                            {
                                                return null;
                                            }
                                        }
                                    }
                                    hash = algorithm.Hash;
                                }
                            }
                        }
                        if (propertyProvider != null)
                        {
                            propertyProvider[0x19] = hash;
                        }
                        str2 = ByteArrayHelper.ToString(hash);
                    }
                    lock (duplicates)
                    {
                        Dictionary<long, Dictionary<string, IVirtualItem>> dictionary;
                        Dictionary<string, IVirtualItem> dictionary2;
                        if (duplicates.TryGetValue(key, out dictionary))
                        {
                            if (dictionary.TryGetValue(num, out dictionary2))
                            {
                                IVirtualItem item2;
                                if (dictionary2.TryGetValue(str2, out item2))
                                {
                                    if (item2 != null)
                                    {
                                        list.Add(item2);
                                        dictionary2[str2] = null;
                                    }
                                    list.Add(item);
                                }
                                else
                                {
                                    dictionary2.Add(str2, item);
                                }
                            }
                            else
                            {
                                dictionary2 = new Dictionary<string, IVirtualItem>();
                                dictionary2.Add(str2, item);
                                dictionary.Add(num, dictionary2);
                            }
                        }
                        else
                        {
                            dictionary2 = new Dictionary<string, IVirtualItem>();
                            dictionary2.Add(str2, item);
                            dictionary = new Dictionary<long, Dictionary<string, IVirtualItem>>();
                            dictionary.Add(num, dictionary2);
                            duplicates.Add(key, dictionary);
                        }
                    }
                }
                else
                {
                    list.Add(item);
                }
                if (list.Count > 0)
                {
                    return list.ToArray();
                }
            }
            return null;
        }

        private bool CheckOption(SearchFolderOptions option)
        {
            return ((this.FSearchOptions & option) > 0);
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            return null;
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, IEnumerable<IVirtualItem> items, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            return null;
        }

        public IVirtualItem CreateLink(IVirtualFolder destFolder, string name, LinkType type)
        {
            if (type != LinkType.Default)
            {
                throw new ArgumentException("Unsupported link type", "type");
            }
            return VirtualItem.CreateVirtualShellLink(destFolder, name, this);
        }

        private IVirtualItem[] EndCheckItem(IAsyncResult asyncResult)
        {
            if (this.AsyncCheckItem == null)
            {
                throw new InvalidOperationException();
            }
            return this.AsyncCheckItem.EndInvoke(asyncResult);
        }

        public virtual bool Equals(IVirtualItem other)
        {
            VirtualSearchFolder folder = other as VirtualSearchFolder;
            return ((other == this) || (((((folder != null) && (folder.GetType() == typeof(VirtualSearchFolder))) && (this.FFolder.Equals(folder.Folder) && (this.FSearchOptions == folder.FSearchOptions))) && (this.FDuplicateOptions == folder.FDuplicateOptions)) && FilterHelper.FilterEquals(this.FFilter, folder.FFilter)));
        }

        public bool ExecuteVerb(IWin32Window owner, string verb)
        {
            return false;
        }

        private void FilterProgress(object sender, ProgressEventArgs e)
        {
            e.Cancel = base.CancellationPending;
        }

        public IEnumerable<IVirtualFolder> GetFolders()
        {
            return new <GetFolders>d__1(-2) { <>4__this = this };
        }

        public Image GetIcon(Size size, IconStyle style)
        {
            return ImageProvider.Default.GetDefaultIcon(DefaultIcon.SearchFolder, size);
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Folder", this.FFolder);
            info.AddValue("SearchOptions", (int) this.FSearchOptions);
            info.AddValue("Filter", this.FFilter);
            info.AddValue("DuplicateOptions", (int) this.FDuplicateOptions);
        }

        public string GetPrefferedLinkName(LinkType type)
        {
            return ((type == LinkType.Default) ? (this.Folder.Name + ".lnk") : null);
        }

        public PropertyAvailability GetPropertyAvailability(int property)
        {
            return (this.AvailableProperties[property] ? PropertyAvailability.Normal : PropertyAvailability.None);
        }

        public virtual bool IsChild(IVirtualItem Item)
        {
            return false;
        }

        private void NewItem(IVirtualItem[] newItems)
        {
            if (newItems != null)
            {
                foreach (IVirtualItem item in newItems)
                {
                    base.NewItem(item);
                }
            }
        }

        protected bool OnSearchError(IVirtualItem item, Exception error)
        {
            if (this.SearchError != null)
            {
                SearchErrorEventArgs e = new SearchErrorEventArgs(item, error);
                this.SearchError(this, e);
                return e.Continue;
            }
            return false;
        }

        protected override void PopulateContent()
        {
            WaitHandle[] handleArray;
            int num3;
            Stack<IVirtualFolder> folderStack = new Stack<IVirtualFolder>();
            if ((this.FFolder is AggregatedVirtualFolder) && this.CheckOption(SearchFolderOptions.ExpandAggregatedRoot))
            {
                foreach (IVirtualFolder folder in this.FFolder.GetFolders())
                {
                    folderStack.Push(folder);
                }
            }
            else
            {
                folderStack.Push(this.FFolder);
            }
            Dictionary<string, Dictionary<long, Dictionary<string, IVirtualItem>>> duplicates = null;
            if (this.FDuplicateOptions != 0)
            {
                duplicates = new Dictionary<string, Dictionary<long, Dictionary<string, IVirtualItem>>>();
            }
            List<AsyncCheckInfo> list = null;
            IChangeProgress fFilter = this.FFilter as IChangeProgress;
            if (fFilter != null)
            {
                fFilter.Progress += new EventHandler<ProgressEventArgs>(this.FilterProgress);
            }
            try
            {
                bool flag = this.CheckOption(SearchFolderOptions.AsyncSearch);
                if (this.CheckOption(SearchFolderOptions.AutoAsyncSearch))
                {
                    flag = FilterHelper.HasContentFilter(this.FFilter) || ((this.FDuplicateOptions & FindDuplicateOptions.SameContent) > 0);
                }
                if (flag)
                {
                    int processorCount = Environment.ProcessorCount;
                    if (processorCount > 1)
                    {
                        list = new List<AsyncCheckInfo>(processorCount);
                    }
                }
            Label_03E6:
                while (!base.CancellationPending)
                {
                    IVirtualFolder userState = null;
                    lock (folderStack)
                    {
                        if (folderStack.Count > 0)
                        {
                            userState = folderStack.Pop();
                        }
                    }
                    if (userState == null)
                    {
                        return;
                    }
                    using (userState)
                    {
                        SystemException exception;
                        base.RaiseProgressChanged(0, userState);
                        try
                        {
                            lock (userState)
                            {
                                foreach (IVirtualItem item in userState.GetContent())
                                {
                                    if (list == null)
                                    {
                                        try
                                        {
                                            this.NewItem(this.CheckItem(item, userState, folderStack, duplicates));
                                        }
                                        catch (SystemException exception1)
                                        {
                                            exception = exception1;
                                            if (this.SearchError == null)
                                            {
                                                throw;
                                            }
                                            if (!this.OnSearchError(item, exception))
                                            {
                                                return;
                                            }
                                        }
                                        goto Label_0355;
                                    }
                                    if (list.Count >= list.Capacity)
                                    {
                                        int millisecondsTimeout = -1;
                                        do
                                        {
                                            handleArray = new WaitHandle[list.Count + 1];
                                            handleArray[0] = base.ExitThreadEvent;
                                            for (num3 = 0; num3 < list.Count; num3++)
                                            {
                                                handleArray[num3 + 1] = list[num3].CheckResult.AsyncWaitHandle;
                                            }
                                            int index = WaitHandle.WaitAny(handleArray, millisecondsTimeout, false);
                                            switch (index)
                                            {
                                                case 0:
                                                    return;

                                                case 0x102:
                                                    goto Label_0328;
                                            }
                                            AsyncCheckInfo info = list[--index];
                                            list.RemoveAt(index);
                                            try
                                            {
                                                this.NewItem(this.EndCheckItem(info.CheckResult));
                                            }
                                            catch (SystemException exception2)
                                            {
                                                exception = exception2;
                                                if (this.SearchError == null)
                                                {
                                                    throw;
                                                }
                                                if (!this.OnSearchError(info.CheckItem, exception))
                                                {
                                                    return;
                                                }
                                            }
                                            millisecondsTimeout = 0;
                                        }
                                        while (list.Count > 0);
                                    }
                                Label_0328:;
                                    list.Add(new AsyncCheckInfo { CheckItem = item, CheckResult = this.BeginCheckItem(item, userState, folderStack, duplicates) });
                                Label_0355:
                                    if (base.CancellationPending)
                                    {
                                        goto Label_03E6;
                                    }
                                }
                            }
                            continue;
                        }
                        catch (SystemException exception3)
                        {
                            exception = exception3;
                            if (this.SearchError == null)
                            {
                                throw;
                            }
                            if (!this.OnSearchError(userState, exception))
                            {
                                return;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (fFilter != null)
                {
                    fFilter.Progress -= new EventHandler<ProgressEventArgs>(this.FilterProgress);
                }
                if ((list != null) && (list.Count > 0))
                {
                    handleArray = new WaitHandle[list.Count];
                    for (num3 = 0; num3 < list.Count; num3++)
                    {
                        handleArray[num3] = list[num3].CheckResult.AsyncWaitHandle;
                    }
                    WaitHandle.WaitAll(handleArray);
                    foreach (AsyncCheckInfo info3 in list)
                    {
                        try
                        {
                            IVirtualItem[] newItems = this.EndCheckItem(info3.CheckResult);
                            if (!base.CancellationPending)
                            {
                                this.NewItem(newItems);
                            }
                        }
                        catch (SystemException)
                        {
                        }
                    }
                }
            }
        }

        protected override void RaiseChanged(VirtualItemChangedEventArgs e)
        {
            if (this.FChanged != null)
            {
                this.FChanged(this, e);
            }
        }

        public void ShowProperties(IWin32Window owner)
        {
        }

        public void ShowProperties(IWin32Window owner, IEnumerable<IVirtualItem> items)
        {
            using (PropertiesDialog dialog = new PropertiesDialog())
            {
                dialog.Execute(owner, items);
            }
        }

        private void VirtualItemChanged(object sender, VirtualItemChangedEventArgs e)
        {
            bool flag = false;
            lock (base.ContentLock)
            {
                IList<IVirtualItem> content = base.Content;
                if (content == null)
                {
                    return;
                }
                switch (e.ChangeType)
                {
                    case WatcherChangeTypes.Deleted:
                        flag = content.Remove(e.Item);
                        break;

                    case WatcherChangeTypes.Changed:
                        flag = content.Contains(e.Item);
                        break;
                }
            }
            if (flag)
            {
                this.RaiseChanged(e);
            }
        }

        public FileAttributes Attributes
        {
            get
            {
                return (FileAttributes.Offline | FileAttributes.Directory);
            }
        }

        public VirtualPropertySet AvailableProperties
        {
            get
            {
                return DefaultProperty.NameAttrPropertySet;
            }
        }

        public FindDuplicateOptions DuplicateOptions
        {
            get
            {
                return this.FDuplicateOptions;
            }
        }

        protected IVirtualFolder Folder
        {
            get
            {
                return this.FFolder;
            }
        }

        public string FullName
        {
            get
            {
                return ((this.FParent != null) ? this.FParent.FullName : this.Name);
            }
        }

        public VirtualHighligher Highlighter
        {
            get
            {
                return null;
            }
        }

        public object this[int property]
        {
            get
            {
                switch (property)
                {
                    case 0:
                        return this.Name;

                    case 6:
                        return FileAttributes.Directory;
                }
                return null;
            }
        }

        public override string Name
        {
            get
            {
                return Resources.sFolderSearch;
            }
        }

        public IVirtualFolder Parent
        {
            get
            {
                return this.FParent;
            }
            set
            {
                this.FParent = value;
            }
        }

        public IVirtualFolder Root
        {
            get
            {
                if (this.FParent != null)
                {
                    return VirtualItemHelper.GetFolderRoot(this.FParent);
                }
                return null;
            }
        }

        public IVirtualItemFilter SearchFilter
        {
            get
            {
                return this.FFilter;
            }
        }

        public IVirtualFolder SearchFolder
        {
            get
            {
                return this.FFolder;
            }
        }

        public SearchFolderOptions SearchOptions
        {
            get
            {
                return this.FSearchOptions;
            }
        }

        public string ShortName
        {
            get
            {
                return this.Name;
            }
        }

        public string ToolTip
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(VirtualProperty.Get(12).LocalizedName);
                builder.Append(": ");
                if ((this.FFolder is AggregatedVirtualFolder) && this.CheckOption(SearchFolderOptions.ExpandAggregatedRoot))
                {
                    foreach (IVirtualFolder folder in this.FFolder.GetFolders())
                    {
                        builder.AppendLine();
                        builder.Append(folder.FullName);
                    }
                }
                else
                {
                    builder.Append(this.FFolder.FullName);
                }
                return builder.ToString();
            }
        }

        [CompilerGenerated]
        private sealed class <GetFolders>d__1 : IEnumerable<IVirtualFolder>, IEnumerable, IEnumerator<IVirtualFolder>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualFolder <>2__current;
            public VirtualSearchFolder <>4__this;
            public object <>7__wrap6;
            private int <>l__initialThreadId;
            public int <Count>5__2;
            public IList<IVirtualItem> <FolderContent>5__3;
            public int <I>5__4;
            public IVirtualFolder <NextFolder>5__5;

            [DebuggerHidden]
            public <GetFolders>d__1(int <>1__state)
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
                        this.<Count>5__2 = 0;
                        lock ((this.<>7__wrap6 = this.<>4__this.ContentLock))
                        {
                            this.<FolderContent>5__3 = this.<>4__this.Content;
                            if (this.<FolderContent>5__3 != null)
                            {
                                this.<Count>5__2 = this.<FolderContent>5__3.Count;
                            }
                        }
                        this.<I>5__4 = 0;
                        while (this.<I>5__4 < this.<Count>5__2)
                        {
                            this.<NextFolder>5__5 = this.<FolderContent>5__3[this.<I>5__4] as IVirtualFolder;
                            if (this.<NextFolder>5__5 == null)
                            {
                                goto Label_00E2;
                            }
                            this.<>2__current = this.<NextFolder>5__5;
                            this.<>1__state = 2;
                            return true;
                        Label_00DB:
                            this.<>1__state = -1;
                        Label_00E2:
                            this.<I>5__4++;
                        }
                        break;

                    case 2:
                        goto Label_00DB;
                }
                return false;
            }

            [DebuggerHidden]
            IEnumerator<IVirtualFolder> IEnumerable<IVirtualFolder>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new VirtualSearchFolder.<GetFolders>d__1(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Virtual.IVirtualFolder>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            IVirtualFolder IEnumerator<IVirtualFolder>.Current
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

        private class AsyncCheckInfo
        {
            public IVirtualItem CheckItem;
            public IAsyncResult CheckResult;
        }

        private delegate IVirtualItem[] CheckItemHandler(IVirtualItem item, IVirtualItem folder, Stack<IVirtualFolder> folderStack, Dictionary<string, Dictionary<long, Dictionary<string, IVirtualItem>>> duplicates);
    }
}

