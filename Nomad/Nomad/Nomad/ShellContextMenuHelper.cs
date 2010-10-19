namespace Nomad
{
    using Microsoft;
    using Microsoft.Shell;
    using Microsoft.Win32;
    using Nomad.Commons.Drawing;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ShellContextMenuHelper
    {
        private IContextMenu ContextMenu;
        private IContextMenu2 ContextMenu2;
        private IContextMenu3 ContextMenu3;
        private string[] FileNames;
        private IContainer ItemContainer;
        private IntPtr Menu;
        private EventHandler<ExecuteVerbEventArgs> OnExecuteVerb;
        private CMF Options;
        private IWin32Window Owner;
        private string ParentName;
        private const int SCRATCH_QCM_FIRST = 1;
        private const int SCRATCH_QCM_LAST = 0x7fff;
        public const string VerbOpen = "open";
        public const string VerbProperties = "properties";
        public const string VerbRename = "rename";

        private ShellContextMenuHelper(IWin32Window owner, IContextMenu contextMenu, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            this.ItemContainer = new Container();
            this.OnExecuteVerb = null;
            this.Owner = owner;
            this.ContextMenu = contextMenu;
            this.Options = ((((options & ContextMenuOptions.Explore) > 0) ? (CMF.CMF_NORMAL | CMF.CMF_EXPLORE) : CMF.CMF_NORMAL) | (((options & ContextMenuOptions.CanRename) > 0) ? CMF.CMF_CANRENAME : CMF.CMF_NORMAL)) | (((options & ContextMenuOptions.VerbsOnly) > 0) ? (CMF.CMF_NORMAL | CMF.CMF_VERBSONLY) : CMF.CMF_NORMAL);
            this.OnExecuteVerb = onExecuteVerb;
        }

        private ShellContextMenuHelper(IWin32Window owner, string[] fileNames, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            this.ItemContainer = new Container();
            this.OnExecuteVerb = null;
            this.Owner = owner;
            ParseFileNames(fileNames, out this.ParentName, out this.FileNames);
            this.Options = ((((options & ContextMenuOptions.Explore) > 0) ? (CMF.CMF_NORMAL | CMF.CMF_EXPLORE) : CMF.CMF_NORMAL) | (((options & ContextMenuOptions.CanRename) > 0) ? CMF.CMF_CANRENAME : CMF.CMF_NORMAL)) | (((options & ContextMenuOptions.VerbsOnly) > 0) ? (CMF.CMF_NORMAL | CMF.CMF_VERBSONLY) : CMF.CMF_NORMAL);
            this.OnExecuteVerb = onExecuteVerb;
        }

        private void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            ((Control) sender).BeginInvoke(new MethodInvoker(this.ItemContainer.Dispose));
        }

        private void ContextMenuStrip_Disposed(object sender, EventArgs e)
        {
            if (this.Menu != IntPtr.Zero)
            {
                Windows.DestroyMenu(this.Menu);
            }
            this.Menu = IntPtr.Zero;
            if (this.ContextMenu != null)
            {
                Marshal.ReleaseComObject(this.ContextMenu);
            }
            this.ContextMenu = null;
            this.ContextMenu2 = null;
            this.ContextMenu3 = null;
            this.Owner = null;
            ((Control) sender).Tag = null;
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip strip = (ContextMenuStrip) sender;
            if ((strip.Items.Count <= 1) && (strip.Items[0].Text == string.Empty))
            {
                if ((this.ContextMenu == null) && (this.FileNames != null))
                {
                    this.ContextMenu = GetContextMenu(this.Owner, this.ParentName, this.FileNames);
                }
                if (this.ContextMenu == null)
                {
                    e.Cancel = true;
                }
                else
                {
                    this.ContextMenu3 = this.ContextMenu as IContextMenu3;
                    if (this.ContextMenu3 == null)
                    {
                        this.ContextMenu2 = this.ContextMenu as IContextMenu2;
                    }
                    this.Menu = Windows.CreatePopupMenu();
                    this.ContextMenu.QueryContextMenu(this.Menu, 0, 1, 0x7fff, this.Options | (((Control.ModifierKeys & Keys.Shift) > Keys.None) ? CMF.CMF_EXTENDEDVERBS : CMF.CMF_NORMAL));
                    if (this.ContextMenu3 != null)
                    {
                        this.ContextMenu3.HandleMenuMsg2(0x117, this.Menu, IntPtr.Zero, IntPtr.Zero);
                    }
                    else if (this.ContextMenu2 != null)
                    {
                        this.ContextMenu2.HandleMenuMsg(0x117, this.Menu, IntPtr.Zero);
                    }
                    this.InitializeToolStrip(strip, this.Menu);
                }
            }
        }

        public static ContextMenuStrip CreateContextMenu(IWin32Window owner, IContextMenu contextMenu, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            ShellContextMenuHelper helper = new ShellContextMenuHelper(owner, contextMenu, options, onExecuteVerb);
            ContextMenuStrip component = new ContextMenuStrip {
                Tag = helper
            };
            component.Items.Add(string.Empty);
            component.Closed += new ToolStripDropDownClosedEventHandler(helper.ContextMenuStrip_Closed);
            component.Opening += new CancelEventHandler(helper.ContextMenuStrip_Opening);
            component.ItemClicked += new ToolStripItemClickedEventHandler(helper.ToolStrip_ItemClick);
            component.Disposed += new EventHandler(helper.ContextMenuStrip_Disposed);
            helper.ItemContainer.Add(component);
            return component;
        }

        public static ContextMenuStrip CreateContextMenu(IWin32Window owner, string[] fileNames, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            ShellContextMenuHelper helper = new ShellContextMenuHelper(owner, fileNames, options, onExecuteVerb);
            ContextMenuStrip component = new ContextMenuStrip {
                Tag = helper
            };
            component.Items.Add(string.Empty);
            component.Closed += new ToolStripDropDownClosedEventHandler(helper.ContextMenuStrip_Closed);
            component.Opening += new CancelEventHandler(helper.ContextMenuStrip_Opening);
            component.ItemClicked += new ToolStripItemClickedEventHandler(helper.ToolStrip_ItemClick);
            component.Disposed += new EventHandler(helper.ContextMenuStrip_Disposed);
            helper.ItemContainer.Add(component);
            return component;
        }

        public static bool ExecuteVerb(IWin32Window owner, string verb, params string[] fileNames)
        {
            string str;
            string[] strArray;
            IContextMenu menu;
            ParseFileNames(fileNames, out str, out strArray);
            if (str == null)
            {
                IShellFolder desktopFolder = ShellItem.GetDesktopFolder();
                try
                {
                    menu = GetContextMenu(owner, desktopFolder, strArray);
                }
                finally
                {
                    Marshal.ReleaseComObject(desktopFolder);
                }
            }
            else
            {
                menu = GetContextMenu(owner, str, strArray);
            }
            return ExecuteVerb(owner, verb, str, menu);
        }

        public static bool ExecuteVerb(IWin32Window owner, string verb, string parentName, IContextMenu contextMenu)
        {
            if (contextMenu == null)
            {
                return false;
            }
            CMINVOKECOMMANDINFOEX structure = new CMINVOKECOMMANDINFOEX();
            try
            {
                structure.cbSize = Marshal.SizeOf(structure);
                if (verb != null)
                {
                    structure.lpVerb = Marshal.StringToHGlobalAnsi(verb);
                    structure.lpVerbW = Marshal.StringToHGlobalUni(verb);
                }
                if (!string.IsNullOrEmpty(parentName))
                {
                    structure.lpDirectory = parentName;
                    structure.lpDirectoryW = parentName;
                }
                if (owner != null)
                {
                    structure.hwnd = owner.Handle;
                }
                structure.fMask = (CMIC.UNICODE | (((Control.ModifierKeys & Keys.Control) > Keys.None) ? CMIC.CONTROL_DOWN : ((CMIC) 0))) | (((Control.ModifierKeys & Keys.Shift) > Keys.None) ? CMIC.SHIFT_DOWN : ((CMIC) 0));
                structure.nShow = SW.SW_SHOWNORMAL;
                contextMenu.InvokeCommand(ref structure);
                Marshal.ReleaseComObject(contextMenu);
            }
            finally
            {
                Marshal.FreeHGlobal(structure.lpVerb);
                Marshal.FreeHGlobal(structure.lpVerbW);
            }
            return true;
        }

        private static IContextMenu GetContextMenu(IWin32Window owner, IShellFolder parent, string[] fileNames)
        {
            IContextMenu menu;
            IntPtr[] apidl = new IntPtr[fileNames.Length];
            try
            {
                for (int i = 0; i < fileNames.Length; i++)
                {
                    apidl[i] = parent.ParseDisplayName(owner.Handle, fileNames[i]);
                }
                menu = parent.GetUIObjectOf<IContextMenu>(owner.Handle, apidl);
            }
            finally
            {
                foreach (IntPtr ptr in apidl)
                {
                    if (ptr != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(ptr);
                    }
                }
            }
            return menu;
        }

        private static IContextMenu GetContextMenu(IWin32Window owner, string parentName, string[] fileNames)
        {
            IContextMenu menu;
            IShellFolder desktopFolder = ShellItem.GetDesktopFolder();
            try
            {
                IntPtr pidl = desktopFolder.ParseDisplayName(owner.Handle, parentName);
                try
                {
                    IShellFolder parent = desktopFolder.BindToFolder(pidl);
                    try
                    {
                        menu = GetContextMenu(owner, parent, fileNames);
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(parent);
                    }
                }
                finally
                {
                    Marshal.FreeCoTaskMem(pidl);
                }
            }
            catch (ArgumentException)
            {
                menu = null;
            }
            catch (COMException)
            {
                menu = null;
            }
            catch (IOException)
            {
                menu = null;
            }
            catch (NotImplementedException)
            {
                menu = null;
            }
            finally
            {
                Marshal.ReleaseComObject(desktopFolder);
            }
            return menu;
        }

        private static RootType GetRootType(string[] fileNames)
        {
            if ((fileNames == null) || (fileNames.Length == 0))
            {
                return RootType.None;
            }
            RootType rootType = GetRootType(fileNames[0]);
            for (int i = 1; (i < fileNames.Length) && (rootType != RootType.None); i++)
            {
                if (GetRootType(fileNames[i]) != rootType)
                {
                    return RootType.None;
                }
            }
            return rootType;
        }

        private static RootType GetRootType(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && fileName.Equals(Path.GetPathRoot(fileName), StringComparison.Ordinal))
            {
                if (fileName.StartsWith(@"\\"))
                {
                    return RootType.Network;
                }
                return RootType.Local;
            }
            return RootType.None;
        }

        private void InitializeToolStrip(ToolStrip strip, IntPtr menu)
        {
            strip.SuspendLayout();
            this.InitializeToolStripItems(strip.Items, menu);
            strip.MinimumSize = new Size(this.MeasureOwnerDrawItems(strip.Items), 0);
            strip.ResumeLayout();
            if (strip.MinimumSize.Width > 0)
            {
                strip.SuspendLayout();
                foreach (ToolStripItem item in strip.Items)
                {
                    if (!item.AutoSize)
                    {
                        item.Width = strip.ClientSize.Width - 1;
                    }
                }
                strip.ResumeLayout();
            }
        }

        private void InitializeToolStripItems(ToolStripItemCollection itemCollection, IntPtr menu)
        {
            itemCollection.Clear();
            int menuItemCount = Windows.GetMenuItemCount(menu);
            MENUITEMINFO lpmii = new MENUITEMINFO {
                cbSize = MENUITEMINFO.SizeOf
            };
            for (uint i = 0; i < menuItemCount; i++)
            {
                ToolStripItem item;
                lpmii.fMask = MIIM.MIIM_DATA | MIIM.MIIM_STRING | MIIM.MIIM_FTYPE | MIIM.MIIM_BITMAP | MIIM.MIIM_CHECKMARKS | MIIM.MIIM_STATE | MIIM.MIIM_SUBMENU | MIIM.MIIM_ID;
                lpmii.dwTypeData = null;
                Windows.GetMenuItemInfo(menu, i, true, ref lpmii);
                if ((lpmii.fType & MFT.MFT_SEPARATOR) > MFT.MFT_STRING)
                {
                    item = new ToolStripSeparator();
                }
                else
                {
                    item = new ToolStripMenuItem();
                    if (lpmii.cch > 0)
                    {
                        lpmii.cch++;
                        lpmii.dwTypeData = new string(' ', (int) lpmii.cch);
                        Windows.GetMenuItemInfo(menu, i, true, ref lpmii);
                        item.Text = lpmii.dwTypeData;
                    }
                    Image image = null;
                    if (((lpmii.fMask & MIIM.MIIM_BITMAP) > ((MIIM) 0)) && (lpmii.hbmpItem != IntPtr.Zero))
                    {
                        if (lpmii.hbmpItem == Windows.HBMMENU_CALLBACK)
                        {
                            lpmii.fType |= MFT.MFT_OWNERDRAW;
                        }
                        else
                        {
                            image = ImageHelper.FromHbitmapWithAlpha(lpmii.hbmpItem);
                            image.RotateFlip(RotateFlipType.Rotate180FlipX);
                        }
                    }
                    else if (((lpmii.fMask & MIIM.MIIM_CHECKMARKS) > ((MIIM) 0)) && (lpmii.hbmpUnchecked != IntPtr.Zero))
                    {
                        image = ImageHelper.FromHbitmapWithAlpha(lpmii.hbmpUnchecked);
                        item.ImageTransparentColor = SystemColors.Window;
                    }
                    if (image != null)
                    {
                        item.ImageScaling = ToolStripItemImageScaling.None;
                        item.Image = image;
                    }
                    if (((lpmii.fType & MFT.MFT_OWNERDRAW) > MFT.MFT_STRING) && ((this.ContextMenu3 != null) || (this.ContextMenu2 != null)))
                    {
                        item.Paint += new PaintEventHandler(this.ToolStripMenuItem_Paint);
                    }
                    if (lpmii.hSubMenu != IntPtr.Zero)
                    {
                        ToolStripMenuItem item2 = (ToolStripMenuItem) item;
                        item2.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.ToolStrip_ItemClick);
                        if ((this.ContextMenu3 != null) || (this.ContextMenu2 != null))
                        {
                            item2.DropDownItems.Add(string.Empty);
                            item2.DropDownOpening += new EventHandler(this.ToolStripMenuItem_DropDownOpening);
                        }
                        else
                        {
                            this.InitializeToolStripItems(item2.DropDownItems, lpmii.hSubMenu);
                        }
                    }
                }
                item.Tag = new MenuItemInfo { Info = lpmii, Menu = menu, Index = i };
                itemCollection.Add(item);
                this.ItemContainer.Add(item);
            }
        }

        private int MeasureOwnerDrawItems(ToolStripItemCollection itemCollection)
        {
            int num2;
            if (itemCollection.Count == 0)
            {
                return 0;
            }
            IntPtr zero = IntPtr.Zero;
            try
            {
                int num = 0;
                foreach (ToolStripItem item in itemCollection)
                {
                    MenuItemInfo tag = (MenuItemInfo) item.Tag;
                    if ((tag.Info.fType & MFT.MFT_OWNERDRAW) > MFT.MFT_STRING)
                    {
                        Microsoft.Win32.MEASUREITEMSTRUCT structure = new Microsoft.Win32.MEASUREITEMSTRUCT {
                            CtlType = ODT.ODT_MENU,
                            itemID = tag.Info.wID,
                            itemWidth = 0,
                            itemHeight = 0,
                            itemData = tag.Info.dwItemData
                        };
                        if (zero == IntPtr.Zero)
                        {
                            zero = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Microsoft.Win32.MEASUREITEMSTRUCT)));
                        }
                        Marshal.StructureToPtr(structure, zero, false);
                        if (this.ContextMenu3 != null)
                        {
                            this.ContextMenu3.HandleMenuMsg2(0x2c, IntPtr.Zero, zero, IntPtr.Zero);
                        }
                        else
                        {
                            this.ContextMenu2.HandleMenuMsg(0x2c, IntPtr.Zero, zero);
                        }
                        structure = (Microsoft.Win32.MEASUREITEMSTRUCT) Marshal.PtrToStructure(zero, typeof(Microsoft.Win32.MEASUREITEMSTRUCT));
                        item.AutoSize = false;
                        num = Math.Max(num, (int) structure.itemWidth);
                        item.Height = Math.Max(item.GetPreferredSize(Size.Empty).Height, (int) structure.itemHeight);
                    }
                }
                num2 = num;
            }
            finally
            {
                Marshal.FreeCoTaskMem(zero);
            }
            return num2;
        }

        private static void ParseFileNames(string[] fileNames, out string parentName, out string[] relativeFileNames)
        {
            parentName = null;
            foreach (string str in fileNames)
            {
                string directoryName = Path.GetDirectoryName(PathHelper.ExcludeTrailingDirectorySeparator(str));
                if (parentName == null)
                {
                    parentName = directoryName;
                }
                else if (!((directoryName != null) && directoryName.Equals(parentName, StringComparison.OrdinalIgnoreCase)))
                {
                    parentName = null;
                    break;
                }
            }
            if (parentName != null)
            {
                relativeFileNames = new string[fileNames.Length];
                for (int i = 0; i < fileNames.Length; i++)
                {
                    relativeFileNames[i] = Path.GetFileName(PathHelper.ExcludeTrailingDirectorySeparator(fileNames[i]));
                }
            }
            else
            {
                relativeFileNames = fileNames;
                switch (GetRootType(fileNames))
                {
                    case RootType.Local:
                        parentName = Microsoft.Shell.Shell32.GetClsidFolderParseName(CLSID.CLSID_MYCOMPUTER);
                        return;

                    case RootType.Network:
                        parentName = Microsoft.Shell.Shell32.GetClsidFolderParseName(CLSID.CLSID_NETWORK_NEIGHBORHOOD);
                        return;
                }
            }
        }

        private void ToolStrip_ItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripDropDownItem clickedItem = e.ClickedItem as ToolStripDropDownItem;
            if (((clickedItem != null) && (clickedItem.DropDownItems.Count <= 0)) && (clickedItem.Tag is MenuItemInfo))
            {
                MenuItemInfo tag = (MenuItemInfo) clickedItem.Tag;
                bool handled = false;
                if (this.OnExecuteVerb != null)
                {
                    IntPtr pszName = Marshal.AllocHGlobal(0x100);
                    try
                    {
                        this.ContextMenu.GetCommandString(tag.Info.wID - 1, GCS.VERBA, 0, pszName, 0x100);
                        ExecuteVerbEventArgs args = new ExecuteVerbEventArgs(Marshal.PtrToStringAnsi(pszName));
                        this.OnExecuteVerb(this, args);
                        handled = args.Handled;
                    }
                    catch (SystemException)
                    {
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(pszName);
                    }
                }
                if (!handled)
                {
                    ContextMenuStrip owner = sender as ContextMenuStrip;
                    if (owner == null)
                    {
                        for (ToolStripItem item2 = sender as ToolStripItem; (owner == null) && (item2 != null); item2 = item2.OwnerItem)
                        {
                            owner = item2.Owner as ContextMenuStrip;
                        }
                    }
                    Debug.Assert(owner != null);
                    owner.Closed -= new ToolStripDropDownClosedEventHandler(this.ContextMenuStrip_Closed);
                    owner.Hide();
                    try
                    {
                        CMINVOKECOMMANDINFOEX cminvokecommandinfoex;
                        cminvokecommandinfoex = new CMINVOKECOMMANDINFOEX {
                            cbSize = Marshal.SizeOf(cminvokecommandinfoex),
                            lpVerb = (IntPtr) (tag.Info.wID - 1),
                            lpVerbW = (IntPtr) (tag.Info.wID - 1)
                        };
                        if (!string.IsNullOrEmpty(this.ParentName))
                        {
                            cminvokecommandinfoex.lpDirectory = this.ParentName;
                            cminvokecommandinfoex.lpDirectoryW = this.ParentName;
                        }
                        cminvokecommandinfoex.fMask = ((CMIC.ASYNCOK | CMIC.PTINVOKE | CMIC.UNICODE) | (((Control.ModifierKeys & Keys.Control) > Keys.None) ? CMIC.CONTROL_DOWN : ((CMIC) 0))) | (((Control.ModifierKeys & Keys.Shift) > Keys.None) ? CMIC.SHIFT_DOWN : ((CMIC) 0));
                        cminvokecommandinfoex.ptInvoke = Cursor.Position;
                        cminvokecommandinfoex.nShow = SW.SW_SHOWNORMAL;
                        cminvokecommandinfoex.hwnd = this.Owner.Handle;
                        this.ContextMenu.InvokeCommand(ref cminvokecommandinfoex);
                    }
                    finally
                    {
                        this.ItemContainer.Dispose();
                    }
                }
            }
        }

        private void ToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripDropDownItem item = (ToolStripDropDownItem) sender;
            MenuItemInfo tag = (MenuItemInfo) item.Tag;
            if (this.ContextMenu3 != null)
            {
                this.ContextMenu3.HandleMenuMsg2(0x117, tag.Info.hSubMenu, (IntPtr) tag.Index, IntPtr.Zero);
            }
            else
            {
                this.ContextMenu2.HandleMenuMsg(0x117, tag.Info.hSubMenu, (IntPtr) tag.Index);
            }
            this.InitializeToolStrip(item.DropDown, tag.Info.hSubMenu);
        }

        private void ToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            if ((this.ContextMenu3 != null) || (this.ContextMenu2 != null))
            {
                System.Drawing.Color menu;
                ToolStripItem item = (ToolStripItem) sender;
                MenuItemInfo tag = (MenuItemInfo) item.Tag;
                if (OS.IsWinXP)
                {
                    menu = SystemColors.Menu;
                    if (item.Selected)
                    {
                        ToolStripProfessionalRenderer renderer = item.Owner.Renderer as ToolStripProfessionalRenderer;
                        if (renderer != null)
                        {
                            menu = renderer.ColorTable.MenuItemSelected;
                        }
                        ToolStripSystemRenderer renderer2 = item.Owner.Renderer as ToolStripSystemRenderer;
                        if (renderer2 != null)
                        {
                            menu = SystemColors.MenuHighlight;
                        }
                    }
                }
                else
                {
                    menu = System.Drawing.Color.Fuchsia;
                }
                Microsoft.Win32.DRAWITEMSTRUCT drawitemstruct = new Microsoft.Win32.DRAWITEMSTRUCT {
                    CtlType = ODT.ODT_MENU,
                    itemID = tag.Info.wID,
                    itemAction = ODA.ODA_DRAWENTIRE,
                    itemState = ODS.ODS_DEFAULT,
                    hwndItem = tag.Menu,
                    itemData = tag.Info.dwItemData
                };
                using (Bitmap bitmap = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.Clear(System.Drawing.Color.Transparent);
                        drawitemstruct.rcItem = new Microsoft.Win32.RECT(0, 0, bitmap.Width - 1, bitmap.Height - 1);
                        drawitemstruct.hDC = graphics.GetHdc();
                        try
                        {
                            IntPtr hgdiobj = item.Font.ToHfont();
                            try
                            {
                                Windows.SelectObject(drawitemstruct.hDC, hgdiobj);
                                Windows.SetBkColor(drawitemstruct.hDC, ColorTranslator.ToWin32(menu));
                                if ((item.Selected && (item.Owner.Renderer is ToolStripSystemRenderer)) && OS.IsWinXP)
                                {
                                    Windows.SetTextColor(drawitemstruct.hDC, ColorTranslator.ToWin32(SystemColors.HighlightText));
                                }
                                GCHandle handle = GCHandle.Alloc(drawitemstruct, GCHandleType.Pinned);
                                try
                                {
                                    if (this.ContextMenu3 != null)
                                    {
                                        this.ContextMenu3.HandleMenuMsg2(0x2b, IntPtr.Zero, handle.AddrOfPinnedObject(), IntPtr.Zero);
                                    }
                                    else
                                    {
                                        this.ContextMenu2.HandleMenuMsg(0x2b, IntPtr.Zero, handle.AddrOfPinnedObject());
                                    }
                                }
                                finally
                                {
                                    handle.Free();
                                }
                            }
                            finally
                            {
                                Windows.DeleteObject(hgdiobj);
                            }
                        }
                        finally
                        {
                            graphics.ReleaseHdc(drawitemstruct.hDC);
                        }
                        bitmap.MakeTransparent(menu);
                        e.Graphics.DrawImage(bitmap, e.ClipRectangle.Location);
                    }
                }
            }
        }

        private class MenuItemInfo
        {
            public uint Index;
            public MENUITEMINFO Info;
            public IntPtr Menu;
        }

        private enum RootType
        {
            None,
            Local,
            Network
        }
    }
}

