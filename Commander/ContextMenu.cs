using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ShellDll;

namespace Commander
{
    public class ContextMenu : NativeWindow
    {
        private IContextMenu iContextMenu, newContextMenu;
        private IContextMenu2 iContextMenu2, newContextMenu2;
        private IContextMenu3 iContextMenu3, newContextMenu3;
        private IntPtr newSubmenuPtr;

        public ContextMenu()
        {
            this.CreateHandle(new CreateParams());
        }

        /// <summary>
        /// This method receives WindowMessages. It will make the "Open With" and "Send To" work 
        /// by calling HandleMenuMsg and HandleMenuMsg2. It will also call the OnContextMenuMouseHover 
        /// method of Browser when hovering over a ContextMenu item.
        /// </summary>
        /// <param name="m">the Message of the Browser's WndProc</param>
        /// <returns>true if the message has been handled, false otherwise</returns>
        protected override void WndProc(ref Message m)
        {
            if (iContextMenu2 != null &&
                (m.Msg == (int)ShellAPI.WM.INITMENUPOPUP ||
                 m.Msg == (int)ShellAPI.WM.MEASUREITEM ||
                 m.Msg == (int)ShellAPI.WM.DRAWITEM))
            {
                if (iContextMenu2.HandleMenuMsg(
                    (uint)m.Msg, m.WParam, m.LParam) == ShellAPI.S_OK)
                    return;
            }

            if (newContextMenu2 != null &&
                ((m.Msg == (int)ShellAPI.WM.INITMENUPOPUP && m.WParam == newSubmenuPtr) ||
                 m.Msg == (int)ShellAPI.WM.MEASUREITEM ||
                 m.Msg == (int)ShellAPI.WM.DRAWITEM))
            {
                if (newContextMenu2.HandleMenuMsg(
                    (uint)m.Msg, m.WParam, m.LParam) == ShellAPI.S_OK)
                    return;
            }

            if (iContextMenu3 != null &&
                m.Msg == (int)ShellAPI.WM.MENUCHAR)
            {
                if (iContextMenu3.HandleMenuMsg2(
                    (uint)m.Msg, m.WParam, m.LParam, IntPtr.Zero) == ShellAPI.S_OK)
                    return;
            }

            if (newContextMenu3 != null &&
                m.Msg == (int)ShellAPI.WM.MENUCHAR)
            {
                if (newContextMenu3.HandleMenuMsg2(
                    (uint)m.Msg, m.WParam, m.LParam, IntPtr.Zero) == ShellAPI.S_OK)
                    return;
            }

            base.WndProc(ref m);
        }
        
        

        private static void FreePIDLs(IntPtr[] arrPIDLs)
        {
            if (null != arrPIDLs)
            {
                for (int n = 0; n < arrPIDLs.Length; n++)
                {
                    if (arrPIDLs[n] != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(arrPIDLs[n]);
                        arrPIDLs[n] = IntPtr.Zero;
                    }
                }
            }
        }

        private static IntPtr[] GetPIDLs(FileInfo[] files)
        {
            if (null == files || 0 == files.Length)
            {
                return null;
            }

            IShellFolder parentFolder = ShellFolder.GetShellFolder(files[0].DirectoryName);
            if (null == parentFolder)
            {
                return null;
            }

            List<IntPtr> pidls = new List<IntPtr>(files.Length);
            foreach (FileInfo file in files)
            {
                // Get the file relative to folder
                uint pchEaten = 0;
                ShellAPI.SFGAO pdwAttributes = 0;
                IntPtr pidl = IntPtr.Zero;
                int result = parentFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, file.Name, ref pchEaten, out pidl, ref pdwAttributes);
                if (ShellAPI.S_OK != result)
                {
                    FreePIDLs(pidls.ToArray());
                    return null;
                }
                pidls.Add(pidl);
            }

            return pidls.ToArray();
        }

        public void CreateNormalMenu(Point location, FileInfo[] files)
        {
            IntPtr[] pidls = GetPIDLs(files);
            IShellFolder parentShellFolder = ShellFolder.GetShellFolder(files[0].DirectoryName);

            IntPtr contextMenu = IntPtr.Zero;
            IntPtr iContextMenuPtr = IntPtr.Zero;
            IntPtr iContextMenuPtr2 = IntPtr.Zero;
            IntPtr iContextMenuPtr3 = IntPtr.Zero;

            // Show / Invoke
            try
            {
                if (ContextMenuHelper.GetIContextMenu(parentShellFolder, pidls, out iContextMenuPtr, out iContextMenu))
                {
                    contextMenu = ShellAPI.CreatePopupMenu();
                    iContextMenu.QueryContextMenu(
                        contextMenu,
                        0,
                        ShellAPI.CMD_FIRST,
                        ShellAPI.CMD_LAST,
                        ShellAPI.CMF.EXPLORE |
                        ShellAPI.CMF.CANRENAME |
                        ((Control.ModifierKeys & Keys.Shift) != 0 ? ShellAPI.CMF.EXTENDEDVERBS : 0));

                    Marshal.QueryInterface(iContextMenuPtr, ref ShellAPI.IID_IContextMenu2, out iContextMenuPtr2);
                    Marshal.QueryInterface(iContextMenuPtr, ref ShellAPI.IID_IContextMenu3, out iContextMenuPtr3);

                    try
                    {
                        iContextMenu2 = (IContextMenu2)Marshal.GetTypedObjectForIUnknown(iContextMenuPtr2, typeof(IContextMenu2));

                        iContextMenu3 = (IContextMenu3)Marshal.GetTypedObjectForIUnknown(iContextMenuPtr3, typeof(IContextMenu3));
                    }
                    catch (Exception) { }

                    uint selected = ShellAPI.TrackPopupMenuEx(
                                        contextMenu,
                                        ShellAPI.TPM.RETURNCMD,
                                        location.X,
                                        location.Y,
                                        this.Handle,
                                        IntPtr.Zero);

                    //br.OnContextMenuMouseHover(new ContextMenuMouseHoverEventArgs(string.Empty));

                    if (selected >= ShellAPI.CMD_FIRST)
                    {
                        string command = ContextMenuHelper.GetCommandString(iContextMenu, selected - ShellAPI.CMD_FIRST, true);

                        if (command == "Explore")
                        {
                            /*if (!br.FolderView.SelectedNode.IsExpanded)
                                br.FolderView.SelectedNode.Expand();

                            br.FolderView.SelectedNode = br.FolderView.SelectedNode.Nodes[hitTest.Item.Text];*/
                        }
                        else if (command == "rename")
                        {
                            /*hitTest.Item.BeginEdit();*/
                        }
                        else
                        {
                            ContextMenuHelper.InvokeCommand(
                                iContextMenu,
                                selected - ShellAPI.CMD_FIRST,
                                files[0].FullName,
                                location);
                        }
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                if (iContextMenu != null)
                {
                    Marshal.ReleaseComObject(iContextMenu);
                    iContextMenu = null;
                }

                if (iContextMenu2 != null)
                {
                    Marshal.ReleaseComObject(iContextMenu2);
                    iContextMenu2 = null;
                }

                if (iContextMenu3 != null)
                {
                    Marshal.ReleaseComObject(iContextMenu3);
                    iContextMenu3 = null;
                }

                if (contextMenu != null)
                    ShellAPI.DestroyMenu(contextMenu);

                if (iContextMenuPtr != IntPtr.Zero)
                    Marshal.Release(iContextMenuPtr);

                if (iContextMenuPtr2 != IntPtr.Zero)
                    Marshal.Release(iContextMenuPtr2);

                if (iContextMenuPtr3 != IntPtr.Zero)
                    Marshal.Release(iContextMenuPtr3);
            }
        }
    }
}
