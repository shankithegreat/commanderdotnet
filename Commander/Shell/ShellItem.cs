using System;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ShellDll
{
    public sealed class ShellItem : IEnumerable, IDisposable, IComparable
    {
        private IShellFolder shellFolder;
        private IntPtr shellFolderPtr;
        private bool disposed;


        public ShellItem(ShellBrowser browser, IntPtr pidl, IntPtr shellFolderPtr)
        {
            this.Browser = browser;

            this.shellFolderPtr = shellFolderPtr;
            this.shellFolder = (IShellFolder)Marshal.GetTypedObjectForIUnknown(shellFolderPtr, typeof(IShellFolder));
            SubFiles = new ShellItemCollection(this);
            SubFolders = new ShellItemCollection(this);

            PIDLRel = new PIDL(pidl, false);

            Text = "Desktop";
            Path = "Desktop";

            SetAttributesDesktop(this);

            ShellAPI.SHFILEINFO info = new ShellAPI.SHFILEINFO();
            ShellAPI.SHGetFileInfo(PIDLRel.Ptr, 0, ref info, ShellAPI.cbFileInfo, ShellAPI.SHGFI.PIDL | ShellAPI.SHGFI.TYPENAME | ShellAPI.SHGFI.SYSICONINDEX);

            Type = info.szTypeName;

            ShellImageList.SetIconIndex(this, info.iIcon, false);
            ShellImageList.SetIconIndex(this, info.iIcon, true);

            SortFlag = 1;
        }

        public ShellItem(ShellBrowser browser, ShellItem parentItem, IntPtr pidl, IntPtr shellFolderPtr)
        {
            this.Browser = browser;

            this.ParentItem = parentItem;
            this.shellFolderPtr = shellFolderPtr;
            this.shellFolder = (IShellFolder)Marshal.GetTypedObjectForIUnknown(shellFolderPtr, typeof(IShellFolder));
            SubFiles = new ShellItemCollection(this);
            SubFolders = new ShellItemCollection(this);

            PIDLRel = new PIDL(pidl, false);

            SetText(this);
            SetPath(this);
            SetAttributesFolder(this);
            SetInfo(this);

            SortFlag = MakeSortFlag(this);
        }

        public ShellItem(ShellBrowser browser, ShellItem parentItem, IntPtr pidl)
        {
            this.Browser = browser;

            this.ParentItem = parentItem;

            PIDLRel = new PIDL(pidl, false);

            SetText(this);
            SetPath(this);
            SetAttributesFile(this);
            SetInfo(this);

            SortFlag = MakeSortFlag(this);
        }
     


        public ShellBrowser Browser { get; private set; }

        public ShellItem ParentItem { get; private set; }

        public ShellItemCollection SubFiles { get; private set; }

        public ShellItemCollection SubFolders { get; private set; }

        public IShellFolder ShellFolder
        {
            get
            {
                if (UpdateShellFolder)
                {
                    Marshal.ReleaseComObject(shellFolder);
                    Marshal.Release(shellFolderPtr);

                    if (ParentItem.ShellFolder.BindToObject(PIDLRel.Ptr, IntPtr.Zero, ref ShellAPI.IID_IShellFolder, out shellFolderPtr) == ShellAPI.S_OK)
                    {
                        shellFolder = (IShellFolder)Marshal.GetTypedObjectForIUnknown(shellFolderPtr, typeof(IShellFolder));
                    }

                    UpdateShellFolder = false;
                }

                return shellFolder;
            }
        }

        public int ImageIndex { get; set; }

        public int SelectedImageIndex { get; set; }

        public PIDL PIDLRel { get; private set; }

        public PIDL PIDLFull
        {
            get
            {
                PIDL pidlFull = new PIDL(PIDLRel.Ptr, true);
                ShellItem current = ParentItem;
                while (current != null)
                {
                    pidlFull.Insert(current.PIDLRel.Ptr);
                    current = current.ParentItem;
                }
                return pidlFull;
            }
        }

        public string Text { get; private set; }

        public string Path { get; private set; }

        public string Type { get; private set; }

        public short SortFlag { get; private set; }

        public bool FilesExpanded { get; private set; }

        public bool FoldersExpanded { get; private set; }

        public bool IsSystemFolder { get { return Type == Browser.SystemFolderName; } }

        public bool IsHidden { get; private set; }

        public bool IsFolder { get; private set; }

        public bool IsLink { get; private set; }

        public bool IsShared { get; private set; }

        public bool IsFileSystem { get; private set; }

        public bool IsBrowsable { get; private set; }

        public bool HasSubfolder { get; private set; }

        public bool IsDisk { get; private set; }

        public bool CanRename { get; private set; }

        public bool CanRead { get; private set; }

        public bool UpdateShellFolder { get; set; }

        public int Count { get { return SubFolders.Count + SubFiles.Count; } }


        public void Dispose()
        {
            if (!disposed)
            {
                DisposeShellItem();
                GC.SuppressFinalize(this);
            }
        }

        public override string ToString()
        {
            return Text;
        }

        public bool Expand(bool expandFiles, bool expandFolders, IntPtr winHandle)
        {
            if (((expandFiles && !FilesExpanded) || !expandFiles) && ((expandFolders && !FoldersExpanded) || !expandFolders) && (expandFiles || expandFolders) && ShellFolder != null && !disposed)
            {
                #region Fields

                IntPtr fileEnumPtr = IntPtr.Zero, folderEnumPtr = IntPtr.Zero;
                IEnumIDList fileEnum = null, folderEnum = null;
                IntPtr pidlSubItem;
                int celtFetched;

                ShellAPI.SHCONTF fileFlag = ShellAPI.SHCONTF.NONFOLDERS | ShellAPI.SHCONTF.INCLUDEHIDDEN;

                ShellAPI.SHCONTF folderFlag = ShellAPI.SHCONTF.FOLDERS | ShellAPI.SHCONTF.INCLUDEHIDDEN;

                #endregion

                try
                {
                    #region Add Files

                    if (expandFiles)
                    {
                        if (this.Equals(Browser.DesktopItem) || ParentItem.Equals(Browser.DesktopItem))
                        {
                            if (ShellFolder.EnumObjects(winHandle, fileFlag, out fileEnumPtr) == ShellAPI.S_OK)
                            {
                                fileEnum = (IEnumIDList)Marshal.GetTypedObjectForIUnknown(fileEnumPtr, typeof(IEnumIDList));
                                ShellAPI.SFGAO attribs = ShellAPI.SFGAO.FOLDER;
                                while (fileEnum.Next(1, out pidlSubItem, out celtFetched) == ShellAPI.S_OK && celtFetched == 1)
                                {
                                    ShellFolder.GetAttributesOf(1, new IntPtr[] { pidlSubItem }, ref attribs);

                                    if ((attribs & ShellAPI.SFGAO.FOLDER) == 0)
                                    {
                                        ShellItem newItem = new ShellItem(Browser, this, pidlSubItem);

                                        if (!SubFolders.Contains(newItem.Text))
                                        {
                                            SubFiles.Add(newItem);
                                        }
                                    }
                                    else
                                    {
                                        Marshal.FreeCoTaskMem(pidlSubItem);
                                    }
                                }

                                SubFiles.Sort();
                                FilesExpanded = true;
                            }
                        }
                        else
                        {
                            if (ShellFolder.EnumObjects(winHandle, fileFlag, out fileEnumPtr) == ShellAPI.S_OK)
                            {
                                fileEnum = (IEnumIDList)Marshal.GetTypedObjectForIUnknown(fileEnumPtr, typeof(IEnumIDList));
                                while (fileEnum.Next(1, out pidlSubItem, out celtFetched) == ShellAPI.S_OK && celtFetched == 1)
                                {
                                    ShellItem newItem = new ShellItem(Browser, this, pidlSubItem);
                                    SubFiles.Add(newItem);
                                }

                                SubFiles.Sort();
                                FilesExpanded = true;
                            }
                        }
                    }

                    #endregion

                    #region Add Folders

                    if (expandFolders)
                    {
                        if (ShellFolder.EnumObjects(winHandle, folderFlag, out folderEnumPtr) == ShellAPI.S_OK)
                        {
                            folderEnum = (IEnumIDList)Marshal.GetTypedObjectForIUnknown(folderEnumPtr, typeof(IEnumIDList));
                            while (folderEnum.Next(1, out pidlSubItem, out celtFetched) == ShellAPI.S_OK && celtFetched == 1)
                            {
                                IntPtr shellFolderPtr;
                                if (ShellFolder.BindToObject(pidlSubItem, IntPtr.Zero, ref ShellAPI.IID_IShellFolder, out shellFolderPtr) == ShellAPI.S_OK)
                                {
                                    ShellItem newItem = new ShellItem(Browser, this, pidlSubItem, shellFolderPtr);
                                    SubFolders.Add(newItem);
                                }
                            }

                            SubFolders.Sort();
                            FoldersExpanded = true;
                        }
                    }

                    #endregion
                }
                catch (Exception)
                {
                }
                finally
                {
                    #region Free

                    if (folderEnum != null)
                    {
                        Marshal.ReleaseComObject(folderEnum);
                        Marshal.Release(folderEnumPtr);
                    }

                    if (fileEnum != null)
                    {
                        Marshal.ReleaseComObject(fileEnum);
                        Marshal.Release(fileEnumPtr);
                    }

                    #endregion
                }
            }

            return ((expandFiles == FilesExpanded || !expandFiles) && (expandFolders == FoldersExpanded || !expandFolders));
        }

        public void Clear(bool clearFiles, bool clearFolders)
        {
            if (((clearFiles && FilesExpanded) || !clearFiles) && ((clearFolders && FoldersExpanded) || !clearFolders) && (clearFiles || clearFolders) && ShellFolder != null && !disposed)
            {
                lock (Browser)
                {
                    try
                    {
                        #region Reset Files

                        if (clearFiles)
                        {
                            foreach (IDisposable item in SubFiles)
                            {
                                item.Dispose();
                            }

                            SubFiles.Clear();
                            FilesExpanded = false;
                        }

                        #endregion

                        #region Reset Folders

                        if (clearFolders)
                        {
                            foreach (IDisposable item in SubFolders)
                            {
                                item.Dispose();
                            }

                            SubFolders.Clear();
                            FoldersExpanded = false;
                        }

                        #endregion
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public void Update(bool updateFiles, bool updateFolders)
        {
            if (Browser.UpdateCondition.ContinueUpdate && (updateFiles || updateFolders) && ShellFolder != null && !disposed)
            {
                lock (Browser)
                {
                    #region Fields

                    IntPtr fileEnumPtr = IntPtr.Zero, folderEnumPtr = IntPtr.Zero;
                    IEnumIDList fileEnum = null, folderEnum = null;
                    IntPtr pidlSubItem;
                    int celtFetched;

                    ShellAPI.SHCONTF fileFlag = ShellAPI.SHCONTF.NONFOLDERS | ShellAPI.SHCONTF.INCLUDEHIDDEN;

                    ShellAPI.SHCONTF folderFlag = ShellAPI.SHCONTF.FOLDERS | ShellAPI.SHCONTF.INCLUDEHIDDEN;

                    bool[] fileExists;
                    fileExists = new bool[SubFiles.Count];

                    bool[] folderExists;
                    folderExists = new bool[SubFolders.Count];

                    int index;

                    #endregion

                    try
                    {
                        #region Update Files

                        if (Browser.UpdateCondition.ContinueUpdate && updateFiles)
                        {
                            ShellItemCollection add = new ShellItemCollection(this);
                            ShellItemCollection remove = new ShellItemCollection(this);

                            bool fileEnumCompleted = false;

                            #region Add Files

                            if (this.Equals(Browser.DesktopItem) || ParentItem.Equals(Browser.DesktopItem))
                            {
                                if (ShellFolder.EnumObjects(IntPtr.Zero, fileFlag, out fileEnumPtr) == ShellAPI.S_OK)
                                {
                                    fileEnum = (IEnumIDList)Marshal.GetTypedObjectForIUnknown(fileEnumPtr, typeof(IEnumIDList));
                                    ShellAPI.SFGAO attribs = ShellAPI.SFGAO.FOLDER;
                                    while (Browser.UpdateCondition.ContinueUpdate && fileEnum.Next(1, out pidlSubItem, out celtFetched) == ShellAPI.S_OK && celtFetched == 1)
                                    {
                                        ShellFolder.GetAttributesOf(1, new IntPtr[] { pidlSubItem }, ref attribs);

                                        if ((attribs & ShellAPI.SFGAO.FOLDER) == 0)
                                        {
                                            if ((index = SubFiles.IndexOf(pidlSubItem)) == -1)
                                            {
                                                ShellItem newItem = new ShellItem(Browser, this, pidlSubItem);

                                                if (!SubFolders.Contains(newItem.Text))
                                                {
                                                    add.Add(newItem);
                                                }
                                            }
                                            else if (index < fileExists.Length)
                                            {
                                                fileExists[index] = true;
                                                Marshal.FreeCoTaskMem(pidlSubItem);
                                            }
                                        }
                                        else
                                        {
                                            Marshal.FreeCoTaskMem(pidlSubItem);
                                        }
                                    }

                                    fileEnumCompleted = true;
                                }
                            }
                            else
                            {
                                if (ShellFolder.EnumObjects(IntPtr.Zero, fileFlag, out fileEnumPtr) == ShellAPI.S_OK)
                                {
                                    fileEnum = (IEnumIDList)Marshal.GetTypedObjectForIUnknown(fileEnumPtr, typeof(IEnumIDList));
                                    while (Browser.UpdateCondition.ContinueUpdate && fileEnum.Next(1, out pidlSubItem, out celtFetched) == ShellAPI.S_OK && celtFetched == 1)
                                    {
                                        if ((index = SubFiles.IndexOf(pidlSubItem)) == -1)
                                        {
                                            add.Add(new ShellItem(Browser, this, pidlSubItem));
                                        }
                                        else if (index < fileExists.Length)
                                        {
                                            fileExists[index] = true;
                                            Marshal.FreeCoTaskMem(pidlSubItem);
                                        }
                                    }

                                    fileEnumCompleted = true;
                                }
                            }

                            #endregion

                            #region Remove Files

                            for (int i = 0; fileEnumCompleted && Browser.UpdateCondition.ContinueUpdate && i < fileExists.Length; i++)
                            {
                                if (!fileExists[i] && SubFiles[i] != null)
                                {
                                    remove.Add(SubFiles[i]);
                                }
                            }

                            #endregion

                            #region Do Events

                            if (fileEnumCompleted && Browser.UpdateCondition.ContinueUpdate)
                            {
                                int newIndex;
                                foreach (ShellItem oldItem in remove)
                                {
                                    if ((newIndex = add.IndexOf(oldItem.Text)) > -1)
                                    {
                                        ShellItem newItem = add[newIndex];
                                        add.Remove(newItem);

                                        oldItem.PIDLRel.Free();
                                        oldItem.PIDLRel = new PIDL(newItem.PIDLRel.Ptr, true);

                                        oldItem.shellFolder = newItem.shellFolder;
                                        oldItem.shellFolderPtr = newItem.shellFolderPtr;

                                        ((IDisposable)newItem).Dispose();

                                        Browser.OnShellItemUpdate(this, new ShellItemUpdateEventArgs(oldItem, oldItem, ShellItemUpdateType.Updated));
                                    }
                                    else
                                    {
                                        SubFiles.Remove(oldItem);
                                        Browser.OnShellItemUpdate(this, new ShellItemUpdateEventArgs(oldItem, null, ShellItemUpdateType.Deleted));
                                        ((IDisposable)oldItem).Dispose();
                                    }
                                }

                                foreach (ShellItem newItem in add)
                                {
                                    SubFiles.Add(newItem);
                                    Browser.OnShellItemUpdate(this, new ShellItemUpdateEventArgs(null, newItem, ShellItemUpdateType.Created));
                                }

                                SubFiles.Capacity = SubFiles.Count;
                                SubFiles.Sort();

                                FilesExpanded = true;
                            }

                            #endregion
                        }

                        #endregion

                        #region Update Folders

                        if (Browser.UpdateCondition.ContinueUpdate && updateFolders)
                        {
                            ShellItemCollection add = new ShellItemCollection(this);
                            ShellItemCollection remove = new ShellItemCollection(this);

                            bool folderEnumCompleted = false;

                            #region Add Folders

                            if (ShellFolder.EnumObjects(IntPtr.Zero, folderFlag, out folderEnumPtr) == ShellAPI.S_OK)
                            {
                                folderEnum = (IEnumIDList)Marshal.GetTypedObjectForIUnknown(folderEnumPtr, typeof(IEnumIDList));
                                while (Browser.UpdateCondition.ContinueUpdate && folderEnum.Next(1, out pidlSubItem, out celtFetched) == ShellAPI.S_OK && celtFetched == 1)
                                {
                                    if ((index = SubFolders.IndexOf(pidlSubItem)) == -1)
                                    {
                                        IntPtr shellFolderPtr;
                                        if (ShellFolder.BindToObject(pidlSubItem, IntPtr.Zero, ref ShellAPI.IID_IShellFolder, out shellFolderPtr) == ShellAPI.S_OK)
                                        {
                                            add.Add(new ShellItem(Browser, this, pidlSubItem, shellFolderPtr));
                                        }
                                    }
                                    else if (index < folderExists.Length)
                                    {
                                        folderExists[index] = true;
                                        Marshal.FreeCoTaskMem(pidlSubItem);
                                    }
                                }

                                folderEnumCompleted = true;
                            }

                            #endregion

                            #region Remove Folders

                            for (int i = 0; folderEnumCompleted && Browser.UpdateCondition.ContinueUpdate && i < folderExists.Length; i++)
                            {
                                if (!folderExists[i] && SubFolders[i] != null)
                                {
                                    remove.Add(SubFolders[i]);
                                }
                            }

                            #endregion

                            #region Do Events

                            if (folderEnumCompleted && Browser.UpdateCondition.ContinueUpdate)
                            {
                                int newIndex;
                                foreach (ShellItem oldItem in remove)
                                {
                                    if ((newIndex = add.IndexOf(oldItem.Text)) > -1)
                                    {
                                        ShellItem newItem = add[newIndex];
                                        add.Remove(newItem);

                                        oldItem.PIDLRel.Free();
                                        oldItem.PIDLRel = new PIDL(newItem.PIDLRel, true);

                                        Marshal.ReleaseComObject(oldItem.shellFolder);
                                        Marshal.Release(oldItem.shellFolderPtr);

                                        oldItem.shellFolder = newItem.shellFolder;
                                        oldItem.shellFolderPtr = newItem.shellFolderPtr;

                                        newItem.shellFolder = null;
                                        newItem.shellFolderPtr = IntPtr.Zero;
                                        ((IDisposable)newItem).Dispose();

                                        Browser.OnShellItemUpdate(this, new ShellItemUpdateEventArgs(oldItem, oldItem, ShellItemUpdateType.Updated));
                                    }
                                    else
                                    {
                                        SubFolders.Remove(oldItem);
                                        Browser.OnShellItemUpdate(this, new ShellItemUpdateEventArgs(oldItem, null, ShellItemUpdateType.Deleted));
                                        ((IDisposable)oldItem).Dispose();
                                    }
                                }

                                foreach (ShellItem newItem in add)
                                {
                                    SubFolders.Add(newItem);

                                    Browser.OnShellItemUpdate(this, new ShellItemUpdateEventArgs(null, newItem, ShellItemUpdateType.Created));
                                }

                                SubFolders.Capacity = SubFolders.Count;
                                SubFolders.Sort();
                                FoldersExpanded = true;
                            }

                            #endregion
                        }

                        #endregion
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        #region Free

                        if (folderEnum != null)
                        {
                            Marshal.ReleaseComObject(folderEnum);
                            Marshal.Release(folderEnumPtr);
                        }

                        if (fileEnum != null)
                        {
                            Marshal.ReleaseComObject(fileEnum);

                            if (!(Type == Browser.SystemFolderName && string.Compare(Text, "Control Panel", true) == 0))
                            {
                                Marshal.Release(fileEnumPtr);
                            }
                        }

                        #endregion
                    }
                }
            }
        }

        public void AddItem(ShellItem item)
        {
            Browser.UpdateCondition.ContinueUpdate = false;
            lock (Browser)
            {
                try
                {
                    if (item.IsFolder)
                    {
                        SubFolders.Add(item);
                    }
                    else
                    {
                        SubFiles.Add(item);
                    }

                    Browser.OnShellItemUpdate(this, new ShellItemUpdateEventArgs(null, item, ShellItemUpdateType.Created));
                }
                catch (Exception)
                {
                }
            }
        }

        public void Update(IntPtr newPidlFull, ShellItemUpdateType changeType)
        {
            Browser.UpdateCondition.ContinueUpdate = false;

            lock (Browser)
            {
                #region Change Pidl and ShellFolder

                if (newPidlFull != IntPtr.Zero)
                {
                    IntPtr tempPidl = PIDL.ILClone(PIDL.ILFindLastID(newPidlFull)), newPidlRel, newShellFolderPtr;
                    ShellAPI.SHGetRealIDL(ParentItem.ShellFolder, tempPidl, out newPidlRel);

                    if (IsFolder && ParentItem.ShellFolder.BindToObject(newPidlRel, IntPtr.Zero, ref ShellAPI.IID_IShellFolder, out newShellFolderPtr) == ShellAPI.S_OK)
                    {
                        Marshal.ReleaseComObject(shellFolder);
                        Marshal.Release(shellFolderPtr);
                        PIDLRel.Free();

                        shellFolderPtr = newShellFolderPtr;
                        shellFolder = (IShellFolder)Marshal.GetTypedObjectForIUnknown(shellFolderPtr, typeof(IShellFolder));
                        PIDLRel = new PIDL(newPidlRel, false);

                        foreach (ShellItem child in SubFolders)
                        {
                            UpdateShellFolders(child);
                        }
                    }
                    else
                    {
                        PIDLRel.Free();
                        PIDLRel = new PIDL(newPidlRel, false);
                    }

                    Marshal.FreeCoTaskMem(tempPidl);
                    Marshal.FreeCoTaskMem(newPidlFull);
                }

                #endregion

                #region Make Other Changes

                switch (changeType)
                {
                    case ShellItemUpdateType.Renamed:
                        SetText(this);
                        SetPath(this);
                        break;

                    case ShellItemUpdateType.Updated:
                        if (IsFolder)
                        {
                            SetAttributesFolder(this);
                        }
                        else
                        {
                            SetAttributesFile(this);
                        }
                        break;

                    case ShellItemUpdateType.MediaChange:
                        SetInfo(this);
                        Clear(true, true);
                        break;

                    case ShellItemUpdateType.IconChange:
                        SetInfo(this);
                        break;
                }

                #endregion
            }

            Browser.OnShellItemUpdate(ParentItem, new ShellItemUpdateEventArgs(this, this, changeType));
        }

        public void RemoveItem(ShellItem item)
        {
            Browser.UpdateCondition.ContinueUpdate = false;

            lock (Browser)
            {
                try
                {
                    if (item.IsFolder)
                    {
                        SubFolders.Remove(item);
                    }
                    else
                    {
                        SubFiles.Remove(item);
                    }

                    Browser.OnShellItemUpdate(this, new ShellItemUpdateEventArgs(item, null, ShellItemUpdateType.Deleted));
                    ((IDisposable)item).Dispose();
                }
                catch (Exception)
                {
                }
            }
        }

        public static string GetRealPath(ShellItem item)
        {
            if (item.Equals(item.Browser.DesktopItem))
            {
                return SpecialFolderPath.MyDocuments;
            }
            else if (item.Type == item.Browser.SystemFolderName)
            {
                IntPtr strr = Marshal.AllocCoTaskMem(ShellAPI.MAX_PATH * 2 + 4);
                Marshal.WriteInt32(strr, 0, 0);
                StringBuilder buf = new StringBuilder(ShellAPI.MAX_PATH);

                if (item.ParentItem.ShellFolder.GetDisplayNameOf(item.PIDLRel.Ptr, ShellAPI.SHGNO.FORPARSING, strr) == ShellAPI.S_OK)
                {
                    ShellAPI.StrRetToBuf(strr, item.PIDLRel.Ptr, buf, ShellAPI.MAX_PATH);
                }

                Marshal.FreeCoTaskMem(strr);

                return buf.ToString();
            }
            else
            {
                return item.Path;
            }
        }

        public static void UpdateShellFolders(ShellItem item)
        {
            item.UpdateShellFolder = true;

            foreach (ShellItem child in item.SubFolders)
            {
                UpdateShellFolders(child);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new ShellItemEnumerator(this);
        }

        public int CompareTo(object obj)
        {
            ShellItem other = (ShellItem)obj;

            if (SortFlag != other.SortFlag)
            {
                return ((SortFlag > other.SortFlag) ? 1 : -1);
            }
            else if (IsDisk)
            {
                return string.Compare(Path, other.Path);
            }
            else
            {
                return string.Compare(Text, other.Text);
            }
        }

        public bool Contains(ShellItem value)
        {
            return (SubFolders.Contains(value) || SubFiles.Contains(value));
        }

        public bool Contains(string name)
        {
            return (SubFolders.Contains(name) || SubFiles.Contains(name));
        }

        public bool Contains(IntPtr pidl)
        {
            return (SubFolders.Contains(pidl) || SubFiles.Contains(pidl));
        }

        public int IndexOf(ShellItem value)
        {
            int index;
            index = SubFolders.IndexOf(value);

            if (index > -1)
            {
                return index;
            }

            index = SubFiles.IndexOf(value);

            if (index > -1)
            {
                return SubFolders.Count + index;
            }

            return -1;
        }

        public int IndexOf(string name)
        {
            int index;
            index = SubFolders.IndexOf(name);

            if (index > -1)
            {
                return index;
            }

            index = SubFiles.IndexOf(name);

            if (index > -1)
            {
                return SubFolders.Count + index;
            }

            return -1;
        }

        public int IndexOf(IntPtr pidl)
        {
            int index;
            index = SubFolders.IndexOf(pidl);

            if (index > -1)
            {
                return index;
            }

            index = SubFiles.IndexOf(pidl);

            if (index > -1)
            {
                return SubFolders.Count + index;
            }

            return -1;
        }

        public ShellItem this[int index]
        {
            get
            {
                if (index >= 0 && index < SubFolders.Count)
                {
                    return SubFolders[index];
                }
                else if (index >= 0 && index - SubFolders.Count < SubFiles.Count)
                {
                    return SubFiles[index - SubFolders.Count];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (index >= 0 && index < SubFolders.Count)
                {
                    SubFolders[index] = value;
                }
                else if (index >= 0 && index - SubFolders.Count < SubFiles.Count)
                {
                    SubFiles[index - SubFolders.Count] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public ShellItem this[string name]
        {
            get
            {
                ShellItem temp = SubFolders[name];

                if (temp != null)
                {
                    return temp;
                }
                else
                {
                    return SubFiles[name];
                }
            }
            set
            {
                ShellItem temp = SubFolders[name];

                if (temp != null)
                {
                    SubFolders[name] = value;
                }
                else
                {
                    SubFiles[name] = value;
                }
            }
        }

        public ShellItem this[IntPtr pidl]
        {
            get
            {
                ShellItem temp = SubFolders[pidl];

                if (temp != null)
                {
                    return temp;
                }
                else
                {
                    return SubFiles[pidl];
                }
            }
            set
            {
                ShellItem temp = SubFolders[pidl];

                if (temp != null)
                {
                    SubFolders[pidl] = value;
                }
                else
                {
                    SubFiles[pidl] = value;
                }
            }
        }


        private static short MakeSortFlag(ShellItem item)
        {
            if (item.IsFolder)
            {
                if (item.IsDisk)
                {
                    return 1;
                }
                if (item.Text == item.Browser.MyDocumentsName && item.Type == item.Browser.SystemFolderName)
                {
                    return 2;
                }
                else if (item.Text == item.Browser.MyComputerName)
                {
                    return 3;
                }
                else if (item.Type == item.Browser.SystemFolderName)
                {
                    if (!item.IsBrowsable)
                    {
                        return 4;
                    }
                    else
                    {
                        return 5;
                    }
                }
                else if (item.IsFolder && !item.IsBrowsable)
                {
                    return 6;
                }
                else
                {
                    return 7;
                }
            }
            else
            {
                return 8;
            }
        }

        private static void SetText(ShellItem item)
        {
            IntPtr strr = Marshal.AllocCoTaskMem(ShellAPI.MAX_PATH * 2 + 4);
            Marshal.WriteInt32(strr, 0, 0);
            StringBuilder buf = new StringBuilder(ShellAPI.MAX_PATH);

            if (item.ParentItem.ShellFolder.GetDisplayNameOf(item.PIDLRel.Ptr, ShellAPI.SHGNO.INFOLDER, strr) == ShellAPI.S_OK)
            {
                ShellAPI.StrRetToBuf(strr, item.PIDLRel.Ptr, buf, ShellAPI.MAX_PATH);
                item.Text = buf.ToString();
            }

            Marshal.FreeCoTaskMem(strr);
        }

        private static void SetPath(ShellItem item)
        {
            IntPtr strr = Marshal.AllocCoTaskMem(ShellAPI.MAX_PATH * 2 + 4);
            Marshal.WriteInt32(strr, 0, 0);
            StringBuilder buf = new StringBuilder(ShellAPI.MAX_PATH);

            if (item.ParentItem.ShellFolder.GetDisplayNameOf(item.PIDLRel.Ptr, ShellAPI.SHGNO.FORADDRESSBAR | ShellAPI.SHGNO.FORPARSING, strr) == ShellAPI.S_OK)
            {
                ShellAPI.StrRetToBuf(strr, item.PIDLRel.Ptr, buf, ShellAPI.MAX_PATH);
                item.Path = buf.ToString();
            }

            Marshal.FreeCoTaskMem(strr);
        }

        private static void SetInfo(ShellItem item)
        {
            PIDL pidlFull = item.PIDLFull;

            ShellAPI.SHFILEINFO info = new ShellAPI.SHFILEINFO();
            ShellAPI.SHGetFileInfo(pidlFull.Ptr, 0, ref info, ShellAPI.cbFileInfo, ShellAPI.SHGFI.PIDL | ShellAPI.SHGFI.TYPENAME | ShellAPI.SHGFI.SYSICONINDEX);

            pidlFull.Free();

            ShellImageList.SetIconIndex(item, info.iIcon, false);
            ShellImageList.SetIconIndex(item, info.iIcon, true);

            item.Type = info.szTypeName;
        }

        private static void SetAttributesDesktop(ShellItem item)
        {
            item.IsFolder = true;
            item.IsLink = false;
            item.IsShared = false;
            item.IsFileSystem = true;
            item.IsHidden = false;
            item.HasSubfolder = true;
            item.IsBrowsable = true;
            item.CanRename = false;
            item.CanRead = true;
        }

        private static void SetAttributesFolder(ShellItem item)
        {
            // file/folder attributes
            ShellAPI.SFGAO attribs = ShellAPI.SFGAO.SHARE | ShellAPI.SFGAO.FILESYSTEM | ShellAPI.SFGAO.HIDDEN | ShellAPI.SFGAO.HASSUBFOLDER | ShellAPI.SFGAO.BROWSABLE | ShellAPI.SFGAO.CANRENAME | ShellAPI.SFGAO.STORAGE;
            item.ParentItem.ShellFolder.GetAttributesOf(1, new IntPtr[] { item.PIDLRel.Ptr }, ref attribs);

            item.IsFolder = true;
            item.IsLink = false;
            item.IsShared = (attribs & ShellAPI.SFGAO.SHARE) != 0;
            item.IsFileSystem = (attribs & ShellAPI.SFGAO.FILESYSTEM) != 0;
            item.IsHidden = (attribs & ShellAPI.SFGAO.HIDDEN) != 0;
            item.HasSubfolder = (attribs & ShellAPI.SFGAO.HASSUBFOLDER) != 0;
            item.IsBrowsable = (attribs & ShellAPI.SFGAO.BROWSABLE) != 0;
            item.CanRename = (attribs & ShellAPI.SFGAO.CANRENAME) != 0;
            item.CanRead = (attribs & ShellAPI.SFGAO.STORAGE) != 0;

            item.IsDisk = (item.Path.Length == 3 && item.Path.EndsWith(":\\"));
        }

        private static void SetAttributesFile(ShellItem item)
        {
            // file/folder attributes
            ShellAPI.SFGAO attribs = ShellAPI.SFGAO.LINK | ShellAPI.SFGAO.SHARE | ShellAPI.SFGAO.FILESYSTEM | ShellAPI.SFGAO.HIDDEN | ShellAPI.SFGAO.CANRENAME | ShellAPI.SFGAO.STREAM;
            item.ParentItem.ShellFolder.GetAttributesOf(1, new IntPtr[] { item.PIDLRel.Ptr }, ref attribs);

            item.IsFolder = false;
            item.IsLink = (attribs & ShellAPI.SFGAO.LINK) != 0;
            item.IsShared = (attribs & ShellAPI.SFGAO.SHARE) != 0;
            item.IsFileSystem = (attribs & ShellAPI.SFGAO.FILESYSTEM) != 0;
            item.IsHidden = (attribs & ShellAPI.SFGAO.HIDDEN) != 0;
            item.HasSubfolder = false;
            item.IsBrowsable = false;
            item.CanRename = (attribs & ShellAPI.SFGAO.CANRENAME) != 0;
            item.CanRead = (attribs & ShellAPI.SFGAO.STREAM) != 0;

            item.IsDisk = false;
        }

        private void DisposeShellItem()
        {
            disposed = true;

            if (ShellFolder != null)
            {
                Marshal.ReleaseComObject(ShellFolder);
                shellFolder = null;
            }

            if (shellFolderPtr != IntPtr.Zero)
            {
                try
                {
                    Marshal.Release(shellFolderPtr);
                }
                catch (Exception)
                {
                }
                finally
                {
                    shellFolderPtr = IntPtr.Zero;
                }
            }

            PIDLRel.Free();
        }
    }


    public class ShellItemUpdateCondition
    {
        public ShellItemUpdateCondition()
        {
            this.ContinueUpdate = true;
        }


        public bool ContinueUpdate { get; set; }
    }

    public class ShellItemEnumerator : IEnumerator
    {
        private ShellItem parent;
        private int index = -1;


        public ShellItemEnumerator(ShellItem parent)
        {
            this.parent = parent;
        }


        public object Current { get { return parent[index]; } }


        public bool MoveNext()
        {
            index++;
            return (index < parent.Count);
        }

        public void Reset()
        {
            index = -1;
        }
    }

    public class ShellItemCollection : IEnumerable
    {
        private ArrayList items;
        private ShellItem shellItem;

        public ShellItemCollection(ShellItem shellItem)
        {
            this.shellItem = shellItem;
            items = new ArrayList();
        }

        public ShellItem ShellItem { get { return shellItem; } }

        #region ArrayList Members

        public int Count { get { return items.Count; } }

        public void Sort()
        {
            items.Sort();
        }

        internal int Capacity { get { return items.Capacity; } set { items.Capacity = value; } }

        #endregion

        #region IList Members

        internal int Add(ShellItem value)
        {
            return items.Add(value);
        }

        internal void Clear()
        {
            items.Clear();
        }

        public bool Contains(ShellItem value)
        {
            return items.Contains(value);
        }

        public bool Contains(string name)
        {
            foreach (ShellItem item in this)
            {
                if (string.Compare(item.Text, name, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Contains(IntPtr pidl)
        {
            foreach (ShellItem item in this)
            {
                if (item.PIDLRel.Equals(pidl))
                {
                    return true;
                }
            }

            return false;
        }

        public int IndexOf(ShellItem value)
        {
            return items.IndexOf(value);
        }

        public int IndexOf(string name)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (string.Compare(this[i].Text, name, true) == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        public int IndexOf(IntPtr pidl)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (this[i].PIDLRel.Equals(pidl))
                {
                    return i;
                }
            }

            return -1;
        }

        internal void Insert(int index, ShellItem value)
        {
            items.Insert(index, value);
        }

        public bool IsFixedSize { get { return items.IsFixedSize; } }

        public bool IsReadOnly { get { return items.IsReadOnly; } }

        internal void Remove(ShellItem value)
        {
            items.Remove(value);
        }

        internal void Remove(string name)
        {
            int index;

            if ((index = IndexOf(name)) > -1)
            {
                RemoveAt(index);
            }
        }

        internal void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        public ShellItem this[int index]
        {
            get
            {
                try
                {
                    return (ShellItem)items[index];
                }
                catch (ArgumentOutOfRangeException)
                {
                    return null;
                }
            }
            set { items[index] = value; }
        }

        public ShellItem this[string name]
        {
            get
            {
                int index;
                if ((index = IndexOf(name)) > -1)
                {
                    return (ShellItem)items[index];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                int index;
                if ((index = IndexOf(name)) > -1)
                {
                    items[index] = value;
                }
            }
        }

        public ShellItem this[IntPtr pidl]
        {
            get
            {
                int index;
                if ((index = IndexOf(pidl)) > -1)
                {
                    return (ShellItem)items[index];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                int index;
                if ((index = IndexOf(pidl)) > -1)
                {
                    items[index] = value;
                }
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion
    }
}