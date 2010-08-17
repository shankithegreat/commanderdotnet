using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Shell;

namespace TestForm
{
    /// <summary>
    /// This class provides static methods which are being used to retrieve IContextMenu's for specific list
    /// and to invoke certain commands.
    /// </summary>
    public static class ContextMenuHelper
    {

        public static string GetCommandString(IContextMenu iContextMenu, uint idcmd, bool executeString)
        {
            string command = GetCommandStringW(iContextMenu, idcmd, executeString);

            if (string.IsNullOrEmpty(command))
            {
                command = GetCommandStringA(iContextMenu, idcmd, executeString);
            }

            return command;
        }

        /// <summary>
        /// Retrieves the command string for a specific item from an iContextMenu (Ansi)
        /// </summary>
        /// <param name="iContextMenu">the IContextMenu to receive the string from</param>
        /// <param name="idcmd">the id of the specific item</param>
        /// <param name="executeString">indicating whether it should return an execute string or not</param>
        /// <returns>if executeString is true it will return the executeString for the item, 
        /// otherwise it will return the help info string</returns>
        public static string GetCommandStringA(IContextMenu iContextMenu, uint idcmd, bool executeString)
        {
            string info = string.Empty;
            byte[] bytes = new byte[256];

            iContextMenu.GetCommandString(
                                          idcmd,
                                          (executeString ? GCS.VerbA : GCS.HelpTextA),
                                          0,
                                          bytes,
                                          ShellApi.MaxPath);

            int index = 0;
            while (index < bytes.Length && bytes[index] != 0)
            {
                index++;
            }

            if (index < bytes.Length)
            {
                info = Encoding.Default.GetString(bytes, 0, index);
            }

            return info;
        }

        /// <summary>
        /// Retrieves the command string for a specific item from an iContextMenu (Unicode)
        /// </summary>
        /// <param name="iContextMenu">the IContextMenu to receive the string from</param>
        /// <param name="idcmd">the id of the specific item</param>
        /// <param name="executeString">indicating whether it should return an execute string or not</param>
        /// <returns>if executeString is true it will return the executeString for the item, 
        /// otherwise it will return the help info string</returns>
        public static string GetCommandStringW(IContextMenu iContextMenu, uint idcmd, bool executeString)
        {
            string info = string.Empty;
            byte[] bytes = new byte[256];

            iContextMenu.GetCommandString(
                                            idcmd,
                                            (executeString ? GCS.VerbW : GCS.HelpTextW),
                                            0,
                                            bytes,
                                            ShellApi.MaxPath);

            int index = 0;
            while (index < bytes.Length - 1 && (bytes[index] != 0 || bytes[index + 1] != 0))
            {
                index += 2;
            }

            if (index < bytes.Length - 1)
            {
                info = Encoding.Unicode.GetString(bytes, 0, index + 1);
            }

            return info;
        }



        /// <summary>
        /// Invokes a specific command from an IContextMenu
        /// </summary>
        /// <param name="iContextMenu">the IContextMenu containing the item</param>
        /// <param name="cmd">the index of the command to invoke</param>
        /// <param name="parentDir">the parent node from where to invoke</param>
        /// <param name="ptInvoke">the point (in screen coцrdinates) from which to invoke</param>
        public static void InvokeCommand(IContextMenu iContextMenu, uint cmd, string parentDir, Point ptInvoke)
        {
            CMINVOKECOMMANDINFOEX invoke = new CMINVOKECOMMANDINFOEX();
            invoke.cbSize = Marshal.SizeOf(typeof(CMINVOKECOMMANDINFOEX));
            invoke.lpVerb = (IntPtr)cmd;
            invoke.lpDirectory = parentDir;
            invoke.lpVerbW = (IntPtr)cmd;
            invoke.lpDirectoryW = parentDir;
            invoke.fMask = CMIC.UNICODE | CMIC.PTINVOKE |
                ((Control.ModifierKeys & Keys.Control) != 0 ? CMIC.CONTROL_DOWN : 0) |
                ((Control.ModifierKeys & Keys.Shift) != 0 ? CMIC.SHIFT_DOWN : 0);
            invoke.ptInvoke = new POINT(ptInvoke);
            invoke.nShow = SW.SHOWNORMAL;

            iContextMenu.InvokeCommand(ref invoke);
        }

        /// <summary>
        /// Invokes a specific command from an IContextMenu
        /// </summary>
        /// <param name="iContextMenu">the IContextMenu containing the item</param>
        /// <param name="cmd">the execute string to invoke</param>
        /// <param name="parentDir">the parent node from where to invoke</param>
        /// <param name="ptInvoke">the point (in screen coцrdinates) from which to invoke</param>
        public static void InvokeCommand(IContextMenu iContextMenu, string cmd, string parentDir, Point ptInvoke)
        {
            CMINVOKECOMMANDINFOEX invoke = new CMINVOKECOMMANDINFOEX();
            invoke.cbSize = Marshal.SizeOf(typeof(CMINVOKECOMMANDINFOEX));
            invoke.lpVerb = Marshal.StringToHGlobalAnsi(cmd);
            invoke.lpDirectory = parentDir;
            invoke.lpVerbW = Marshal.StringToHGlobalUni(cmd);
            invoke.lpDirectoryW = parentDir;
            invoke.fMask = CMIC.UNICODE | CMIC.PTINVOKE |
                ((Control.ModifierKeys & Keys.Control) != 0 ? CMIC.CONTROL_DOWN : 0) |
                ((Control.ModifierKeys & Keys.Shift) != 0 ? CMIC.SHIFT_DOWN : 0);
            invoke.ptInvoke = new POINT(ptInvoke);
            invoke.nShow = SW.SHOWNORMAL;

            iContextMenu.InvokeCommand(ref invoke);
        }

        /// <summary>
        /// Invokes a specific command for a set of pidls
        /// </summary>
        /// <param name="parent">the parent IShellFolder which contains the pidls</param>
        /// <param name="parentPath">the parent path</param>
        /// <param name="pidls">the pidls from the list for which to invoke</param>
        /// <param name="cmd">the execute string from the command to invoke</param>
        /// <param name="ptInvoke">the point (in screen coцrdinates) from which to invoke</param>
        public static void InvokeCommand(IShellFolder parent, string parentPath, IntPtr[] pidls, string cmd, Point ptInvoke)
        {
            IntPtr icontextMenuPtr;
            IContextMenu iContextMenu;

            if (GetIContextMenu(parent, pidls, out icontextMenuPtr, out iContextMenu))
            {
                try
                {
                    InvokeCommand(iContextMenu, cmd, parentPath, ptInvoke);
                }
                catch
                {
                }
                finally
                {
                    if (iContextMenu != null)
                    {
                        Marshal.ReleaseComObject(iContextMenu);
                    }

                    if (icontextMenuPtr != IntPtr.Zero)
                    {
                        Marshal.Release(icontextMenuPtr);
                    }
                }
            }
        }

        /// <summary>
        /// Invokes a specific command for a set of pidls
        /// </summary>
        /// <param name="parent">the parent ShellItem which contains the pidls</param>
        /// <param name="pidls">the pidls from the list for which to invoke</param>
        /// <param name="cmd">the execute string from the command to invoke</param>
        /// <param name="ptInvoke">the point (in screen coцrdinates) from which to invoke</param>
        public static void InvokeCommand(ShellNode parent, IntPtr[] pidls, string cmd, Point ptInvoke)
        {
            IntPtr icontextMenuPtr;
            IContextMenu iContextMenu;

            if (GetIContextMenu(parent.ShellFolder, pidls, out icontextMenuPtr, out iContextMenu))
            {
                try
                {
                    InvokeCommand(
                                    iContextMenu,
                                    cmd,
                                    ShellNode.GetRealPath(parent),
                                    ptInvoke);
                }
                catch
                {
                }
                finally
                {
                    if (iContextMenu != null)
                        Marshal.ReleaseComObject(iContextMenu);

                    if (icontextMenuPtr != IntPtr.Zero)
                        Marshal.Release(icontextMenuPtr);
                }
            }
        }


        /// <summary>
        /// Retrieves the IContextMenu for specific list
        /// </summary>
        /// <param name="parent">the parent IShellFolder which contains the list</param>
        /// <param name="pidls">the pidls of the list for which to retrieve the IContextMenu</param>
        /// <param name="iContextMenuPtr">the pointer to the IContextMenu</param>
        /// <param name="iContextMenu">the IContextMenu for the list</param>
        /// <returns>true if the IContextMenu has been retrieved succesfully, false otherwise</returns>
        public static bool GetIContextMenu(
            IShellFolder parent,
            IntPtr[] pidls,
            out IntPtr iContextMenuPtr,
            out IContextMenu iContextMenu)
        {
            if (parent.GetUIObjectOf(
                                     IntPtr.Zero,
                                     (uint)pidls.Length,
                                     pidls,
                                     ref ShellGuids.IContextMenu,
                                     IntPtr.Zero,
                                     out iContextMenuPtr) == 0)
            {
                iContextMenu = (IContextMenu)Marshal.GetTypedObjectForIUnknown(iContextMenuPtr, typeof(IContextMenu));

                return true;
            }
            else
            {
                iContextMenuPtr = IntPtr.Zero;
                iContextMenu = null;

                return false;
            }
        }

        public static bool GetNewContextMenu(ShellNode item, out IntPtr iContextMenuPtr, out IContextMenu iContextMenu)
        {
            if (Ole32.CoCreateInstance(
                    ref ShellGuids.NewMenu,
                    IntPtr.Zero,
                    CLSCTX.INPROC_SERVER,
                    ref ShellGuids.IContextMenu,
                    out iContextMenuPtr) == 0)
            {
                iContextMenu = Marshal.GetTypedObjectForIUnknown(iContextMenuPtr, typeof(IContextMenu)) as IContextMenu;

                IntPtr iShellExtInitPtr;
                if (Marshal.QueryInterface(
                    iContextMenuPtr,
                    ref ShellGuids.IShellExtInit,
                    out iShellExtInitPtr) == 0)
                {
                    IShellExtInit iShellExtInit = Marshal.GetTypedObjectForIUnknown(iShellExtInitPtr, typeof(IShellExtInit)) as IShellExtInit;

                    Pidl pidlFull = item.PIDLFull;
                    if (iShellExtInit != null)
                    {
                        iShellExtInit.Initialize(pidlFull.Ptr, IntPtr.Zero, 0);
                        Marshal.ReleaseComObject(iShellExtInit);
                    }
                    Marshal.Release(iShellExtInitPtr);
                    pidlFull.Free();

                    return true;
                }
                else
                {
                    if (iContextMenu != null)
                    {
                        Marshal.ReleaseComObject(iContextMenu);
                        iContextMenu = null;
                    }

                    if (iContextMenuPtr != IntPtr.Zero)
                    {
                        Marshal.Release(iContextMenuPtr);
                        iContextMenuPtr = IntPtr.Zero;
                    }

                    return false;
                }
            }
            else
            {
                iContextMenuPtr = IntPtr.Zero;
                iContextMenu = null;
                return false;
            }
        }
    }
}
