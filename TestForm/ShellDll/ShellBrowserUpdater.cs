using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ShellDll
{
    public class ShellBrowserUpdater : NativeWindow
    {
        private ShellBrowser browser;
        private uint notifyId;


        public ShellBrowserUpdater(ShellBrowser browser)
        {
            this.browser = browser;
            CreateHandle(new CreateParams());

            SHChangeNotifyEntry entry = new SHChangeNotifyEntry();
            entry.pIdl = browser.DesktopItem.PIDLRel.Ptr;
            entry.Recursively = true;

            notifyId = RegisterShellNotify(this.Handle, entry);
        }

        ~ShellBrowserUpdater()
        {
            if (notifyId > 0)
            {
                ShellApi.SHChangeNotifyDeregister(notifyId);
                GC.SuppressFinalize(this);
            }
        }


        public static uint RegisterShellNotify(IntPtr handle, SHChangeNotifyEntry entry)
        {
            return ShellApi.SHChangeNotifyRegister(handle, SHCNRF.InterruptLevel | SHCNRF.ShellLevel, SHCNE.ALLEVENTS | SHCNE.INTERRUPT, WM.SH_NOTIFY, 1, new SHChangeNotifyEntry[] { entry });
        }

        public static uint RegisterShellNotify(IntPtr handle)
        {
            SHChangeNotifyEntry entry = new SHChangeNotifyEntry();
            entry.pIdl = ShellBrowser.GetDesctopPidl();
            entry.Recursively = true;

            return RegisterShellNotify(handle, entry);
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WM.SH_NOTIFY)
            {
                SHNOTIFYSTRUCT shNotify = (SHNOTIFYSTRUCT)Marshal.PtrToStructure(m.WParam, typeof(SHNOTIFYSTRUCT));

                //Console.Out.WriteLine("Event: {0}", (SHCNE)m.LParam);
                //if (shNotify.dwItem1 != IntPtr.Zero)
                //PIDL.Write(shNotify.dwItem1);
                //if (shNotify.dwItem2 != IntPtr.Zero)
                //PIDL.Write(shNotify.dwItem2);

                switch ((SHCNE)m.LParam)
                {
                    #region File Changes

                    case SHCNE.CREATE:

                        #region Create Item

                        {
                            if (!Pidl.IsEmpty(shNotify.dwItem1))
                            {
                                IntPtr parent, child, relative;
                                Pidl.SplitPidl(shNotify.dwItem1, out parent, out child);

                                Pidl parentPIDL = new Pidl(parent, false);
                                ShellNode parentItem = browser.GetShellItem(parentPIDL);
                                if (parentItem != null && parentItem.FilesExpanded && !parentItem.SubFiles.Contains(child))
                                {
                                    ShellApi.SHGetRealIDL(parentItem.ShellFolder, child, out relative);
                                    parentItem.AddItem(new ShellNode(browser, parentItem, relative));
                                }

                                Marshal.FreeCoTaskMem(child);
                                parentPIDL.Free();
                            }
                        }

                        #endregion

                        break;

                    case SHCNE.RENAMEITEM:

                        #region Rename Item

                        {
                            if (!Pidl.IsEmpty(shNotify.dwItem1) && !Pidl.IsEmpty(shNotify.dwItem2))
                            {
                                ShellNode item = browser.GetShellItem(new Pidl(shNotify.dwItem1, true));
                                if (item != null)
                                {
                                    item.Update(shNotify.dwItem2, ShellItemUpdateType.Renamed);
                                }
                            }
                        }

                        #endregion

                        break;

                    case SHCNE.DELETE:

                        #region Delete Item

                        {
                            if (!Pidl.IsEmpty(shNotify.dwItem1))
                            {
                                IntPtr parent, child;
                                Pidl.SplitPidl(shNotify.dwItem1, out parent, out child);

                                Pidl parentPIDL = new Pidl(parent, false);
                                ShellNode parentItem = browser.GetShellItem(parentPIDL);
                                if (parentItem != null && parentItem.SubFiles.Contains(child))
                                {
                                    parentItem.RemoveItem(parentItem.SubFiles[child]);
                                }

                                Marshal.FreeCoTaskMem(child);
                                parentPIDL.Free();
                            }
                        }

                        #endregion

                        break;

                    case SHCNE.UPDATEITEM:

                        #region Update Item

                        {
                            if (!Pidl.IsEmpty(shNotify.dwItem1))
                            {
                                ShellNode item = browser.GetShellItem(new Pidl(shNotify.dwItem1, true));
                                if (item != null)
                                {
                                    Console.Out.WriteLine("Item: {0}", item);
                                    item.Update(IntPtr.Zero, ShellItemUpdateType.Updated);
                                    item.Update(IntPtr.Zero, ShellItemUpdateType.IconChange);
                                }
                            }
                        }

                        #endregion

                        break;

                    #endregion

                    #region Folder Changes

                    case SHCNE.MKDIR:
                    case SHCNE.DRIVEADD:
                    case SHCNE.DRIVEADDGUI:

                        #region Make Directory

                        {
                            if (!Pidl.IsEmpty(shNotify.dwItem1))
                            {
                                IntPtr parent, child, relative;
                                Pidl.SplitPidl(shNotify.dwItem1, out parent, out child);

                                Pidl parentPIDL = new Pidl(parent, false);
                                ShellNode parentItem = browser.GetShellItem(parentPIDL);
                                if (parentItem != null && parentItem.FoldersExpanded && !parentItem.SubFolders.Contains(child))
                                {
                                    ShellApi.SHGetRealIDL(parentItem.ShellFolder, child, out relative);

                                    IntPtr shellFolderPtr;
                                    if (parentItem.ShellFolder.BindToObject(relative, IntPtr.Zero, ref ShellGuids.IShellFolder, out shellFolderPtr) == 0)
                                    {
                                        parentItem.AddItem(new ShellNode(browser, parentItem, relative, shellFolderPtr));
                                    }
                                    else
                                    {
                                        Marshal.FreeCoTaskMem(relative);
                                    }
                                }

                                Marshal.FreeCoTaskMem(child);
                                parentPIDL.Free();
                            }
                        }

                        #endregion

                        break;

                    case SHCNE.RENAMEFOLDER:

                        #region Rename Directory

                        {
                            if (!Pidl.IsEmpty(shNotify.dwItem1) && !Pidl.IsEmpty(shNotify.dwItem2))
                            {
                                ShellNode item = browser.GetShellItem(new Pidl(shNotify.dwItem1, false));
                                if (item != null)
                                {
                                    //Console.Out.WriteLine("Update: {0}", item);
                                    item.Update(shNotify.dwItem2, ShellItemUpdateType.Renamed);
                                }
                            }
                        }

                        #endregion

                        break;

                    case SHCNE.RMDIR:
                    case SHCNE.DRIVEREMOVED:

                        #region Remove Directory

                        {
                            if (!Pidl.IsEmpty(shNotify.dwItem1))
                            {
                                IntPtr parent, child;
                                Pidl.SplitPidl(shNotify.dwItem1, out parent, out child);

                                Pidl parentPIDL = new Pidl(parent, false);
                                ShellNode parentItem = browser.GetShellItem(parentPIDL);
                                if (parentItem != null && parentItem.SubFolders.Contains(child))
                                {
                                    parentItem.RemoveItem(parentItem.SubFolders[child]);
                                }

                                Marshal.FreeCoTaskMem(child);
                                parentPIDL.Free();
                            }
                        }

                        #endregion

                        break;

                    case SHCNE.UPDATEDIR:
                    case SHCNE.ATTRIBUTES:

                        #region Update Directory

                        {
                            if (!Pidl.IsEmpty(shNotify.dwItem1))
                            {
                                ShellNode item = browser.GetShellItem(new Pidl(shNotify.dwItem1, true));
                                if (item != null)
                                {
                                    item.Update(IntPtr.Zero, ShellItemUpdateType.Updated);
                                    item.Update(IntPtr.Zero, ShellItemUpdateType.IconChange);
                                }
                            }
                        }

                        #endregion

                        break;

                    case SHCNE.MEDIAINSERTED:
                    case SHCNE.MEDIAREMOVED:

                        #region Media Change

                        {
                            if (!Pidl.IsEmpty(shNotify.dwItem1))
                            {
                                ShellNode item = browser.GetShellItem(new Pidl(shNotify.dwItem1, true));
                                if (item != null)
                                {
                                    item.Update(IntPtr.Zero, ShellItemUpdateType.MediaChange);
                                }
                            }
                        }

                        #endregion

                        break;

                    #endregion

                    #region Other Changes

                    case SHCNE.ASSOCCHANGED:

                        #region Update Images

                        {
                        }

                        #endregion

                        break;

                    case SHCNE.NETSHARE:
                    case SHCNE.NETUNSHARE:
                        break;

                    case SHCNE.UPDATEIMAGE:
                        UpdateRecycleBin();
                        break;

                    #endregion
                }
            }

            base.WndProc(ref m);
        }


        private void UpdateRecycleBin()
        {
            ShellNode recycleBin = browser.DesktopItem["Recycle Bin"];
            if (recycleBin != null)
            {
                recycleBin.Update(IntPtr.Zero, ShellItemUpdateType.IconChange);
            }
        }
    }
}