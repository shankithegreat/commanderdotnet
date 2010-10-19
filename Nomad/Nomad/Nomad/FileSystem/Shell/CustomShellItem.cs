namespace Nomad.FileSystem.Shell
{
    using Microsoft.Shell;
    using Microsoft.Win32;
    using Nomad;
    using Nomad.Commons.Drawing;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Windows.Forms;

    [DebuggerDisplay("{GetType().Name}, {FullName}")]
    public class CustomShellItem : CustomPropertyProvider, IVirtualItemUI, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, ISerializable
    {
        private DateTime? CreationTime;
        private const string EntryItemInfo = "ItemInfo";
        private VirtualItemVisualExtender FExtender;
        private IDictionary<System.Drawing.Size, Image> FIcons;
        private FileAttributes? FileAttr;
        private int FStoredChangeVector;
        private bool HasItemData;
        private bool HasParent;
        private SFGAO ItemAttr;
        private SFGAO ItemAttrMask;
        private string ItemFullName;
        protected internal readonly SafeShellItem ItemInfo;
        private string ItemName;
        private IVirtualFolder ItemParent;
        private string ItemType;
        private DateTime? LastAccessTime;
        private DateTime? LastWriteTime;
        private long? Size;

        protected CustomShellItem(SafeShellItem item)
        {
            this.ItemInfo = item;
            this.Initialize();
        }

        protected CustomShellItem(SerializationInfo info, StreamingContext context)
        {
            this.ItemInfo = (SafeShellItem) info.GetValue("ItemInfo", typeof(SafeShellItem));
            this.Initialize();
        }

        protected CustomShellItem(SafeShellItem item, SFGAO attributes, IVirtualFolder parent)
        {
            this.ItemInfo = item;
            this.ItemAttr = attributes;
            this.ItemAttrMask = attributes;
            this.ItemParent = parent;
            this.HasParent = this.ItemParent != null;
            this.Initialize();
        }

        protected bool CheckItemAttribute(SFGAO attribute)
        {
            if ((this.ItemAttrMask & attribute) != attribute)
            {
                this.ItemAttr = (this.ItemAttr & ~attribute) | this.ItemInfo.GetAttributesOf(attribute);
                this.ItemAttrMask |= attribute;
            }
            return ((this.ItemAttr & attribute) == attribute);
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = new VirtualPropertySet();
            set[0] = true;
            set[1] = true;
            set[6] = true;
            set[2] = true;
            bool flag = !this.HasItemData && this.CheckItemAttribute(SFGAO.SFGAO_FILESYSTEM);
            set[7] = flag || this.CreationTime.HasValue;
            set[9] = flag || this.LastAccessTime.HasValue;
            set[8] = flag || this.LastWriteTime.HasValue;
            set[3] = flag || this.Size.HasValue;
            return set;
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            return ShellContextMenuHelper.CreateContextMenu(owner, this.ItemInfo.GetUIObjectOf<IContextMenu>(owner.Handle), options, onExecuteVerb);
        }

        public bool Equals(IVirtualItem other)
        {
            CustomShellItem item = other as CustomShellItem;
            return ((item != null) && ITEMIDLIST.Equals(this.ItemInfo.AbsolutePidl, item.ItemInfo.AbsolutePidl));
        }

        public bool ExecuteVerb(IWin32Window owner, string verb)
        {
            SHELLEXECUTEINFO shellexecuteinfo;
            shellexecuteinfo = new SHELLEXECUTEINFO {
                cbSize = Marshal.SizeOf(shellexecuteinfo),
                fMask = SEE_MASK.SEE_MASK_IDLIST,
                lpIDList = this.ItemInfo.AbsolutePidl,
                lpVerb = verb,
                hwnd = (owner != null) ? owner.Handle : IntPtr.Zero,
                nShow = SW.SW_SHOW
            };
            return Microsoft.Shell.Shell32.ShellExecuteEx(ref shellexecuteinfo);
        }

        public Image GetIcon(System.Drawing.Size size, IconStyle style)
        {
            if ((style & IconStyle.DefaultIcon) > 0)
            {
                return this.GetItemIcon(size, true);
            }
            Image itemIcon = null;
            bool flag = !ChangeVector.Equals(this.FStoredChangeVector, ChangeVector.Icon);
            if (flag)
            {
                ChangeVector.CopyTo(ref this.FStoredChangeVector, ChangeVector.Icon);
            }
            if (((this.FIcons == null) || flag) || !this.FIcons.TryGetValue(size, out itemIcon))
            {
                bool flag2 = VirtualIcon.CheckIconOption(IconOptions.ExtractIcons) || !this.CheckItemAttribute(SFGAO.SFGAO_FILESYSTEM);
                if (flag2 && VirtualIcon.CheckIconOption(IconOptions.DisableExtractSlowIcons))
                {
                    flag2 = !this.CheckItemAttribute(SFGAO.SFGAO_ISSLOW) && !this.CheckItemAttribute(SFGAO.SFGAO_REMOVABLE);
                }
                itemIcon = this.GetItemIcon(size, !flag2);
                if ((this.FIcons == null) || flag)
                {
                    this.FIcons = IconCollection.Create();
                }
                this.FIcons.Add(size, itemIcon);
            }
            return itemIcon;
        }

        private Image GetItemIcon(System.Drawing.Size size, bool defaultIcon)
        {
            object uIObjectOf;
            try
            {
                uIObjectOf = this.ItemInfo.GetUIObjectOf<IExtractIconW>(IntPtr.Zero);
            }
            catch
            {
                try
                {
                    uIObjectOf = this.ItemInfo.GetUIObjectOf<IExtractIconA>(IntPtr.Zero);
                }
                catch
                {
                    uIObjectOf = null;
                }
            }
            IntPtr zero = IntPtr.Zero;
            if (uIObjectOf != null)
            {
                int num;
                GIL_OUT gil_out;
                uint num2;
                IntPtr ptr4;
                StringBuilder szIconFile = new StringBuilder(0x400);
                GIL_IN uFlags = (GIL_IN.GIL_FORSHELL | (defaultIcon ? GIL_IN.GIL_DEFAULTICON : ((GIL_IN) 0))) | (this.CheckItemAttribute(SFGAO.SFGAO_LINK) ? GIL_IN.GIL_FORSHORTCUT : ((GIL_IN) 0));
                IntPtr phiconSmall = IntPtr.Zero;
                IntPtr phiconLarge = IntPtr.Zero;
                if (size.Height >= 0x20)
                {
                    num2 = (uint) (size.Height | 0x100000);
                }
                else
                {
                    num2 = (uint) (0x20 | (size.Height << 0x10));
                }
                IExtractIconW nw = uIObjectOf as IExtractIconW;
                if (nw != null)
                {
                    if (HRESULT.SUCCEEDED(nw.GetIconLocation(uFlags, szIconFile, (uint) szIconFile.Capacity, out num, out gil_out)))
                    {
                        int num3 = nw.Extract(szIconFile.ToString(), num, out phiconLarge, out phiconSmall, num2);
                    }
                }
                else
                {
                    IExtractIconA na = uIObjectOf as IExtractIconA;
                    if (na == null)
                    {
                        throw new InvalidOperationException();
                    }
                    if (HRESULT.SUCCEEDED(na.GetIconLocation(uFlags, szIconFile, (uint) szIconFile.Capacity, out num, out gil_out)))
                    {
                        na.Extract(szIconFile.ToString(), num, out phiconLarge, out phiconSmall, num2);
                    }
                }
                if (size.Height >= 0x20)
                {
                    zero = phiconLarge;
                    ptr4 = phiconSmall;
                }
                else
                {
                    zero = phiconSmall;
                    ptr4 = phiconLarge;
                }
                if (ptr4 != IntPtr.Zero)
                {
                    Windows.DestroyIcon(ptr4);
                }
            }
            if (zero == IntPtr.Zero)
            {
                SHGFI shgfi = (SHGFI.SHGFI_LARGEICON | SHGFI.SHGFI_ICON) | ((size.Height < 0x20) ? SHGFI.SHGFI_SMALLICON : SHGFI.SHGFI_LARGEICON);
                SHFILEINFO psfi = new SHFILEINFO();
                if (defaultIcon)
                {
                    Microsoft.Shell.Shell32.SHGetFileInfo(this.ItemInfo.GetDisplayNameOf(SHGNO.SHGDN_FORPARSING | SHGNO.SHGDN_INFOLDER), this.Attributes, ref psfi, shgfi | (SHGFI.SHGFI_LARGEICON | SHGFI.SHGFI_USEFILEATTRIBUTES));
                }
                else
                {
                    if (VirtualIcon.CheckIconOption(IconOptions.ShowOverlayIcons))
                    {
                        shgfi |= SHGFI.SHGFI_LARGEICON | SHGFI.SHGFI_ADDOVERLAYS;
                    }
                    Microsoft.Shell.Shell32.SHGetFileInfo(this.ItemInfo.AbsolutePidl, 0, ref psfi, shgfi | (SHGFI.SHGFI_LARGEICON | SHGFI.SHGFI_PIDL));
                }
                zero = psfi.hIcon;
            }
            if (zero != IntPtr.Zero)
            {
                Image image = ImageHelper.IconToBitmap(zero);
                Windows.DestroyIcon(zero);
                return image;
            }
            return null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ItemInfo", this.ItemInfo);
        }

        protected static T? GetShellProperty<T>(SafeShellItem item, Guid fmt, uint id) where T: struct
        {
            object obj2 = GetShellProperty(item, fmt, id);
            if (obj2 is T)
            {
                return new T?((T) obj2);
            }
            return null;
        }

        protected static object GetShellProperty(SafeShellItem item, Guid fmt, uint id)
        {
            IShellFolder2 folder = item.Folder as IShellFolder2;
            if (folder != null)
            {
                PropVariant variant;
                SHCOLUMNID pscid = new SHCOLUMNID(fmt, id);
                if (HRESULT.SUCCEEDED(folder.GetDetailsEx(item.RelativePidl, ref pscid, out variant)))
                {
                    return variant.Value;
                }
            }
            return null;
        }

        private void Initialize()
        {
            this.FStoredChangeVector = ChangeVector.Value;
        }

        private void PopulateItemData()
        {
            if (!this.HasItemData)
            {
                bool flag = false;
                IShellFolder psf = this.ItemInfo.Folder;
                if (this.CheckItemAttribute(SFGAO.SFGAO_FILESYSTEM))
                {
                    Microsoft.Win32.WIN32_FIND_DATA pv = new Microsoft.Win32.WIN32_FIND_DATA();
                    if (HRESULT.SUCCEEDED(Microsoft.Shell.Shell32.SHGetDataFromIDList(psf, this.ItemInfo.RelativePidl, ref pv)))
                    {
                        this.FileAttr = new FileAttributes?(pv.dwFileAttributes);
                        this.CreationTime = new DateTime?(DateTime.FromFileTime((pv.ftCreationTime.dwHighDateTime << 0x20) & pv.ftCreationTime.dwLowDateTime));
                        this.LastAccessTime = new DateTime?(DateTime.FromFileTime((pv.ftLastAccessTime.dwHighDateTime << 0x20) & pv.ftLastAccessTime.dwLowDateTime));
                        this.LastWriteTime = new DateTime?(DateTime.FromFileTime((pv.ftLastWriteTime.dwHighDateTime << 0x20) & pv.ftLastWriteTime.dwLowDateTime));
                        this.Size = new long?((long) ((pv.nFileSizeHigh << 0x20) | pv.nFileSizeLow));
                        flag = true;
                    }
                }
                if (!flag)
                {
                    IShellFolder2 folder2 = psf as IShellFolder2;
                    if (folder2 != null)
                    {
                        PropVariant variant;
                        this.CreationTime = GetShellProperty<DateTime>(this.ItemInfo, SHCOLUMNID.PropertySetStorage, 15);
                        this.LastAccessTime = GetShellProperty<DateTime>(this.ItemInfo, SHCOLUMNID.PropertySetStorage, 0x10);
                        this.LastWriteTime = GetShellProperty<DateTime>(this.ItemInfo, SHCOLUMNID.PropertySetStorage, 14);
                        SHCOLUMNID pscid = new SHCOLUMNID(SHCOLUMNID.PropertySetStorage, 12);
                        if (HRESULT.SUCCEEDED(folder2.GetDetailsEx(this.ItemInfo.RelativePidl, ref pscid, out variant)))
                        {
                            this.Size = new long?(Convert.ToInt64(variant.Value));
                        }
                        flag = true;
                    }
                }
                if (flag)
                {
                    base.ResetAvailableSet();
                }
                this.HasItemData = true;
            }
        }

        public void ShowProperties(IWin32Window owner)
        {
            ShellContextMenuHelper.ExecuteVerb(owner, "properties", null, this.ItemInfo.GetUIObjectOf<IContextMenu>(owner.Handle));
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", base.GetType().Name, this.FullName);
        }

        public FileAttributes Attributes
        {
            get
            {
                this.PopulateItemData();
                if (this.FileAttr.HasValue)
                {
                    return this.FileAttr.Value;
                }
                return ((FileAttributes.Normal | (this.CheckItemAttribute(SFGAO.SFGAO_FOLDER) ? FileAttributes.Directory : ((FileAttributes) 0))) | (this.CheckItemAttribute(SFGAO.SFGAO_HIDDEN) ? FileAttributes.Hidden : ((FileAttributes) 0)));
            }
        }

        protected VirtualItemVisualExtender Extender
        {
            get
            {
                if (this.FExtender == null)
                {
                    this.FExtender = new VirtualItemVisualExtender(this);
                }
                return this.FExtender;
            }
        }

        public string Extension
        {
            get
            {
                return Path.GetExtension(this.Name);
            }
        }

        public string FullName
        {
            get
            {
                if (this.ItemFullName == null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(ShellFileSystemCreator.UriScheme);
                    builder.Append(Uri.SchemeDelimiter);
                    string displayNameOf = this.ItemInfo.GetDisplayNameOf(SHGNO.SHGDN_FORPARSING);
                    if (displayNameOf.StartsWith("::", StringComparison.Ordinal))
                    {
                        builder.Append(Path.AltDirectorySeparatorChar);
                    }
                    builder.Append(displayNameOf);
                    if (this.CheckItemAttribute(SFGAO.SFGAO_FOLDER) && (builder[builder.Length - 1] != Path.DirectorySeparatorChar))
                    {
                        builder.Append(Path.DirectorySeparatorChar);
                    }
                    this.ItemFullName = builder.ToString();
                }
                return this.ItemFullName;
            }
        }

        public VirtualHighligher Highlighter
        {
            get
            {
                return this.Extender.Highlighter;
            }
        }

        public virtual object this[int property]
        {
            get
            {
                this.PopulateItemData();
                switch (property)
                {
                    case 0:
                        return this.Name;

                    case 1:
                        return this.Extension;

                    case 2:
                        return this.Type;

                    case 3:
                        return this.Size;

                    case 6:
                        return this.Attributes;

                    case 7:
                        return this.CreationTime;

                    case 8:
                        return this.LastWriteTime;

                    case 9:
                        return this.LastAccessTime;
                }
                return null;
            }
        }

        public string Name
        {
            get
            {
                if (this.ItemName == null)
                {
                    this.ItemName = this.ItemInfo.GetDisplayNameOf(SHGNO.SHGDN_FORADDRESSBAR | SHGNO.SHGDN_INFOLDER);
                }
                return this.ItemName;
            }
        }

        public IVirtualFolder Parent
        {
            get
            {
                if (!this.HasParent)
                {
                    SafeShellItem parent = this.ItemInfo.GetParent();
                    if (parent != null)
                    {
                        this.ItemParent = new ShellFolder(parent);
                    }
                    this.HasParent = true;
                }
                return this.ItemParent;
            }
        }

        public string ShortName
        {
            get
            {
                return this.Name;
            }
        }

        public virtual string ToolTip
        {
            get
            {
                return this.Extender.ToolTip;
            }
        }

        public string Type
        {
            get
            {
                if (this.ItemType == null)
                {
                    SHFILEINFO psfi = new SHFILEINFO();
                    Microsoft.Shell.Shell32.SHGetFileInfo(this.ItemInfo.AbsolutePidl, 0, ref psfi, SHGFI.SHGFI_LARGEICON | SHGFI.SHGFI_PIDL | SHGFI.SHGFI_TYPENAME);
                    this.ItemType = psfi.szTypeName;
                }
                return this.ItemType;
            }
        }
    }
}

