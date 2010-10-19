namespace Nomad
{
    using Microsoft.Win32;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Windows.Forms;

    internal class VirtualItemDataObject : DataObject
    {
        public const string CleanupClipboardDataFormat = "Virtual Item Operation";
        public const string VirtualItemList = "Virtual Items";

        public VirtualItemDataObject(IVirtualItem item, bool move)
        {
            FileSystemItem item2 = item as FileSystemItem;
            if (item2 != null)
            {
                StringCollection filePaths = new StringCollection();
                filePaths.Add(item2.ComparableName);
                this.SetFileDropList(filePaths);
            }
            else
            {
                this.SetData("Virtual Items", new IVirtualItem[] { item });
            }
            if (move)
            {
                this.SetData("Preferred DropEffect", true, 2);
            }
        }

        public VirtualItemDataObject(IEnumerable<IVirtualItem> items, bool move)
        {
            bool flag = false;
            StringCollection filePaths = new StringCollection();
            foreach (IVirtualItem item in items)
            {
                FileSystemItem item2 = item as FileSystemItem;
                if (item2 != null)
                {
                    filePaths.Add(item2.ComparableName);
                }
                else
                {
                    flag = true;
                }
            }
            if (filePaths.Count > 0)
            {
                this.SetFileDropList(filePaths);
            }
            if (flag)
            {
                this.SetData("Virtual Items", new List<IVirtualItem>(items));
            }
            if (move)
            {
                this.SetData("Preferred DropEffect", true, 2);
            }
        }

        public static int ReadInt32FromClipboard(string format)
        {
            STGMEDIUM stgmedium;
            if (!Clipboard.ContainsData(format))
            {
                return -1;
            }
            object data = Clipboard.GetData(format);
            if (data is int)
            {
                return (int) data;
            }
            System.Runtime.InteropServices.ComTypes.IDataObject dataObject = Clipboard.GetDataObject() as System.Runtime.InteropServices.ComTypes.IDataObject;
            if (dataObject == null)
            {
                return -1;
            }
            DataFormats.Format format2 = DataFormats.GetFormat(format);
            if (format2 == null)
            {
                return -1;
            }
            FORMATETC formatetc = new FORMATETC {
                cfFormat = (short) format2.Id,
                dwAspect = DVASPECT.DVASPECT_CONTENT,
                lindex = -1,
                tymed = TYMED.TYMED_HGLOBAL
            };
            dataObject.GetData(ref formatetc, out stgmedium);
            return ReadInt32FromSTGMEDIUM(stgmedium);
        }

        private static int ReadInt32FromSTGMEDIUM(STGMEDIUM pmedium)
        {
            if (pmedium.tymed == TYMED.TYMED_HGLOBAL)
            {
                IntPtr ptr = Windows.GlobalLock(pmedium.unionmember);
                try
                {
                    return Marshal.ReadInt32(ptr);
                }
                finally
                {
                    Windows.GlobalUnlock(pmedium.unionmember);
                }
            }
            return 0;
        }
    }
}

