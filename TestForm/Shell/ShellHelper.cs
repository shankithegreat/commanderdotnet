using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ShellDll
{
    internal static class ShellHelper
    {
        #region Low/High Word

        /// <summary>
        /// Retrieves the High Word of a WParam of a WindowMessage
        /// </summary>
        /// <param name="ptr">The pointer to the WParam</param>
        /// <returns>The unsigned integer for the High Word</returns>
        public static uint HiWord(IntPtr ptr)
        {
            if (((uint) ptr & 0x80000000) == 0x80000000)
            {
                return ((uint) ptr >> 16);
            }
            else
            {
                return ((uint) ptr >> 16) & 0xffff;
            }
        }

        /// <summary>
        /// Retrieves the Low Word of a WParam of a WindowMessage
        /// </summary>
        /// <param name="ptr">The pointer to the WParam</param>
        /// <returns>The unsigned integer for the Low Word</returns>
        public static uint LoWord(IntPtr ptr)
        {
            return (uint) ptr & 0xffff;
        }

        #endregion

        #region IStream/IStorage

        public static bool GetIStream(ShellItem item, out IntPtr streamPtr, out IStream stream)
        {
            if (item.ParentItem.ShellFolder.BindToStorage(item.PIDLRel.Ptr, IntPtr.Zero, ref ShellGuids.IStream, out streamPtr) == 0)
            {
                stream = (IStream) Marshal.GetTypedObjectForIUnknown(streamPtr, typeof (IStream));
                return true;
            }
            else
            {
                stream = null;
                streamPtr = IntPtr.Zero;
                return false;
            }
        }

        public static bool GetIStorage(ShellItem item, out IntPtr storagePtr, out IStorage storage)
        {
            if (item.ParentItem.ShellFolder.BindToStorage(item.PIDLRel.Ptr, IntPtr.Zero, ref ShellGuids.IStorage, out storagePtr) == 0)
            {
                storage = (IStorage) Marshal.GetTypedObjectForIUnknown(storagePtr, typeof (IStorage));
                return true;
            }
            else
            {
                storage = null;
                storagePtr = IntPtr.Zero;
                return false;
            }
        }

        #endregion

        #region Drag/Drop

        /// <summary>
        /// This method will use the GetUIObjectOf method of IShellFolder to obtain the IDataObject of a
        /// ShellItem. 
        /// </summary>
        /// <param name="item">The item for which to obtain the IDataObject</param>
        /// <param name="dataObjectPtr">A pointer to the returned IDataObject</param>
        /// <returns>the IDataObject the ShellItem</returns>
        public static IntPtr GetIDataObject(ShellItem[] items)
        {
            ShellItem parent = items[0].ParentItem != null ? items[0].ParentItem : items[0];

            IntPtr[] pidls = new IntPtr[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                pidls[i] = items[i].PIDLRel.Ptr;
            }

            IntPtr dataObjectPtr;
            if (parent.ShellFolder.GetUIObjectOf(IntPtr.Zero, (uint)pidls.Length, pidls, ref ShellGuids.IDataObject, IntPtr.Zero, out dataObjectPtr) == 0)
            {
                return dataObjectPtr;
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// This method will use the GetUIObjectOf method of IShellFolder to obtain the IDataObject of a
        /// ShellItem. 
        /// </summary>
        /// <returns>the IDataObject the ShellItem</returns>
        public static IntPtr GetIDataObject(IntPtr[] pidls, IShellFolder parent)
        {
            IntPtr dataObjectPtr;
            if (parent.GetUIObjectOf(IntPtr.Zero, (uint)pidls.Length, pidls, ref ShellGuids.IDataObject, IntPtr.Zero, out dataObjectPtr) == 0)
            {
                return dataObjectPtr;
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// This method will use the GetUIObjectOf method of IShellFolder to obtain the IDropTarget of a
        /// ShellItem. 
        /// </summary>
        /// <param name="item">The item for which to obtain the IDropTarget</param>
        /// <param name="dropTargetPtr">A pointer to the returned IDropTarget</param>
        /// <returns>the IDropTarget from the ShellItem</returns>
        public static bool GetIDropTarget(ShellItem item, out IntPtr dropTargetPtr, out ShellDll.IDropTarget dropTarget)
        {
            ShellItem parent = item.ParentItem != null ? item.ParentItem : item;

            if (parent.ShellFolder.GetUIObjectOf(IntPtr.Zero, 1, new IntPtr[] { item.PIDLRel.Ptr }, ref ShellGuids.IDropTarget, IntPtr.Zero, out dropTargetPtr) == 0)
            {
                dropTarget = (ShellDll.IDropTarget) Marshal.GetTypedObjectForIUnknown(dropTargetPtr, typeof (ShellDll.IDropTarget));

                return true;
            }
            else
            {
                dropTarget = null;
                dropTargetPtr = IntPtr.Zero;
                return false;
            }
        }

        /// <summary>
        /// This method will use the GetUIObjectOf method of IShellFolder to obtain the IDropTarget of a
        /// ShellItem. 
        /// </summary>
        /// <param name="item">The item for which to obtain the IDropTarget</param>
        /// <param name="dropTargetPtr">A pointer to the returned IDropTarget</param>
        /// <returns>the IDropTarget from the ShellItem</returns>
        public static bool GetIDropTarget(IntPtr[] pidls, IShellFolder parent, out IntPtr dropTargetPtr, out ShellDll.IDropTarget dropTarget)
        {
            if (parent.GetUIObjectOf(IntPtr.Zero, 1, pidls, ref ShellGuids.IDropTarget, IntPtr.Zero, out dropTargetPtr) == 0)
            {
                dropTarget = (ShellDll.IDropTarget) Marshal.GetTypedObjectForIUnknown(dropTargetPtr, typeof (ShellDll.IDropTarget));

                return true;
            }
            else
            {
                dropTarget = null;
                dropTargetPtr = IntPtr.Zero;
                return false;
            }
        }

        public static bool GetIDropTargetHelper(out IntPtr helperPtr, out IDropTargetHelper dropHelper)
        {
            if (ShellApi.CoCreateInstance(ref ShellGuids.DragDropHelper, IntPtr.Zero, CLSCTX.INPROC_SERVER, ref ShellGuids.IDropTargetHelper, out helperPtr) == 0)
            {
                dropHelper = (IDropTargetHelper) Marshal.GetTypedObjectForIUnknown(helperPtr, typeof (IDropTargetHelper));

                return true;
            }
            else
            {
                dropHelper = null;
                helperPtr = IntPtr.Zero;
                return false;
            }
        }

        public static DragDropEffects CanDropClipboard(ShellItem item)
        {
            IntPtr dataObject;
            ShellApi.OleGetClipboard(out dataObject);

            IntPtr targetPtr;
            ShellDll.IDropTarget target;

            DragDropEffects retVal = DragDropEffects.None;
            if (GetIDropTarget(item, out targetPtr, out target))
            {
                #region Check Copy

                DragDropEffects effects = DragDropEffects.Copy;
                if (target.DragEnter(dataObject, MK.CONTROL, new POINT(0, 0), ref effects) == 0)
                {
                    if (effects == DragDropEffects.Copy)
                    {
                        retVal |= DragDropEffects.Copy;
                    }

                    target.DragLeave();
                }

                #endregion

                #region Check Move

                effects = DragDropEffects.Move;
                if (target.DragEnter(dataObject, MK.SHIFT, new POINT(0, 0), ref effects) == 0)
                {
                    if (effects == DragDropEffects.Move)
                    {
                        retVal |= DragDropEffects.Move;
                    }

                    target.DragLeave();
                }

                #endregion

                #region Check Lick

                effects = DragDropEffects.Link;
                if (target.DragEnter(dataObject, MK.ALT, new POINT(0, 0), ref effects) == 0)
                {
                    if (effects == DragDropEffects.Link)
                    {
                        retVal |= DragDropEffects.Link;
                    }

                    target.DragLeave();
                }

                #endregion

                Marshal.ReleaseComObject(target);
                Marshal.Release(targetPtr);
            }

            return retVal;
        }

        #endregion

        #region QueryInfo

        public static bool GetIQueryInfo(ShellItem item, out IntPtr iQueryInfoPtr, out IQueryInfo iQueryInfo)
        {
            ShellItem parent = item.ParentItem != null ? item.ParentItem : item;

            if (parent.ShellFolder.GetUIObjectOf(IntPtr.Zero, 1, new IntPtr[] { item.PIDLRel.Ptr }, ref ShellGuids.IQueryInfo, IntPtr.Zero, out iQueryInfoPtr) == 0)
            {
                iQueryInfo = (IQueryInfo) Marshal.GetTypedObjectForIUnknown(iQueryInfoPtr, typeof (IQueryInfo));

                return true;
            }
            else
            {
                iQueryInfo = null;
                iQueryInfoPtr = IntPtr.Zero;
                return false;
            }
        }

        #endregion
    }
}