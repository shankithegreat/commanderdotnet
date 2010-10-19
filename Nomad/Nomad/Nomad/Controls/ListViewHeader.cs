namespace Nomad.Controls
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    internal class ListViewHeader : NativeWindow
    {
        private IntPtr HeaderItemPtr;
        private ColumnHeader HoverColumn;
        private ListViewEx Parent;
        private Microsoft.Win32.MSG RelayMsg = new Microsoft.Win32.MSG();
        private Dictionary<int, ListSortDirection> SortDirection;
        private IntPtr TooltipWnd;

        public ListViewHeader(ListViewEx parent, IntPtr headerWnd)
        {
            this.Parent = parent;
            base.AssignHandle(headerWnd);
            this.TooltipWnd = Windows.SendMessage(this.Parent.Handle, 0x104e, IntPtr.Zero, IntPtr.Zero);
            this.AddTool();
        }

        private void AddTool()
        {
            TOOLINFO tool = new TOOLINFO {
                lpszText = CommCtrl.LPSTR_TEXTCALLBACK
            };
            tool = this.ExecuteTool(CommCtrl.TTM_ADDTOOL, ref tool);
        }

        private void DeleteTool()
        {
            TOOLINFO tool = new TOOLINFO();
            tool = this.ExecuteTool(CommCtrl.TTM_DELTOOL, ref tool);
        }

        private TOOLINFO ExecuteTool(int message, ref TOOLINFO Tool)
        {
            Tool.cbSize = Marshal.SizeOf((TOOLINFO) Tool);
            Tool.uFlags = TTF.TTF_IDISHWND;
            Tool.hwnd = base.Handle;
            Tool.uId = base.Handle;
            GCHandle handle = GCHandle.Alloc((TOOLINFO) Tool, GCHandleType.Pinned);
            try
            {
                Windows.SendMessage(this.TooltipWnd, CommCtrl.TTM_ADDTOOL, IntPtr.Zero, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
            return Tool;
        }

        private void InternalSortColumn(int column, ListSortDirection? direction)
        {
            HDF hdf;
            if (this.ReadColumnFormat(column, out hdf))
            {
                hdf &= ~(HDF.HDF_SORTUP | HDF.HDF_SORTDOWN);
                if (direction.HasValue)
                {
                    hdf |= (((ListSortDirection) direction.Value) == ListSortDirection.Descending) ? 0x200 : 0x400;
                }
                HDITEM hditem = new HDITEM {
                    mask = HDI.HDI_FORMAT,
                    fmt = hdf
                };
                GCHandle handle = GCHandle.Alloc(hditem, GCHandleType.Pinned);
                try
                {
                    this.HeaderItemPtr = handle.AddrOfPinnedObject();
                    Windows.SendMessage(base.Handle, 0x120c, (IntPtr) column, this.HeaderItemPtr);
                }
                finally
                {
                    handle.Free();
                    this.HeaderItemPtr = IntPtr.Zero;
                }
            }
        }

        private bool ReadColumnFormat(int column, out HDF format)
        {
            bool flag;
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(HDITEM)));
            try
            {
                HDITEM.SetMask(ptr, HDI.HDI_FORMAT);
                if (Windows.SendMessage(base.Handle, 0x120b, (IntPtr) column, ptr) == IntPtr.Zero)
                {
                    format = HDF.HDF_LEFT;
                    return false;
                }
                format = HDITEM.GetFormat(ptr);
                flag = true;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return flag;
        }

        private void RelayTooltipMessage(ref Message m)
        {
            this.RelayMsg.hwnd = m.HWnd;
            this.RelayMsg.message = m.Msg;
            this.RelayMsg.wParam = m.WParam;
            this.RelayMsg.lParam = m.LParam;
            GCHandle handle = GCHandle.Alloc(this.RelayMsg, GCHandleType.Pinned);
            try
            {
                Windows.SendMessage(this.TooltipWnd, 0x407, IntPtr.Zero, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        public void ResetSortColumn(int column)
        {
            if ((this.SortDirection != null) && this.SortDirection.Remove(column))
            {
                this.InternalSortColumn(column, null);
            }
        }

        public void SetImageList(ImageList headerImageList)
        {
            if (headerImageList != null)
            {
                Windows.SendMessage(base.Handle, 0x1208, IntPtr.Zero, headerImageList.Handle);
            }
        }

        public void SetSortColumn(int column, ListSortDirection direction)
        {
            if (this.SortDirection == null)
            {
                this.SortDirection = new Dictionary<int, ListSortDirection>();
            }
            else
            {
                ListSortDirection direction2;
                if (this.SortDirection.TryGetValue(column, out direction2) && (direction2 == direction))
                {
                    return;
                }
            }
            this.SortDirection[column] = direction;
            this.InternalSortColumn(column, new ListSortDirection?(direction));
        }

        protected override void WndProc(ref Message m)
        {
            int msg = m.Msg;
            switch (msg)
            {
                case 2:
                    this.DeleteTool();
                    break;

                case 0x20:
                    if (this.Parent.CanResizeColumns)
                    {
                        break;
                    }
                    return;

                case 0x4e:
                    msg = Microsoft.Win32.NMHDR.GetNotifyCode(m.LParam);
                    if ((((msg == -530) || (msg == -520)) && (this.HoverColumn != null)) && (TextRenderer.MeasureText(this.HoverColumn.Text, this.Parent.Font).Width > this.HoverColumn.Width))
                    {
                        string text = this.HoverColumn.Text;
                        if (text.Length > 0x4f)
                        {
                            text = text.Substring(0, 0x4f);
                        }
                        text = text + '\0';
                        byte[] bytes = ((m.Msg == -520) ? Encoding.Default : Encoding.Unicode).GetBytes(text);
                        IntPtr destination = (IntPtr) ((m.LParam.ToInt64() + Marshal.SizeOf(typeof(Microsoft.Win32.NMHDR))) + IntPtr.Size);
                        Marshal.Copy(bytes, 0, destination, bytes.Length);
                    }
                    break;

                case 0x120a:
                case 0x120c:
                    ListSortDirection direction;
                    if (((this.HeaderItemPtr == IntPtr.Zero) && (this.SortDirection != null)) && this.SortDirection.TryGetValue((int) m.WParam, out direction))
                    {
                        HDF format;
                        HDI mask = HDITEM.GetMask(m.LParam);
                        if ((mask & HDI.HDI_FORMAT) > ((HDI) 0))
                        {
                            format = HDITEM.GetFormat(m.LParam);
                        }
                        else if (!this.ReadColumnFormat((int) m.WParam, out format))
                        {
                            break;
                        }
                        format &= ~(HDF.HDF_SORTUP | HDF.HDF_SORTDOWN);
                        format |= (direction == ListSortDirection.Descending) ? 0x200 : 0x400;
                        HDITEM.SetMask(m.LParam, mask | HDI.HDI_FORMAT);
                        HDITEM.SetFormat(m.LParam, format);
                        break;
                    }
                    break;

                case 0x1202:
                    if (this.SortDirection != null)
                    {
                        this.SortDirection.Remove((int) m.WParam);
                    }
                    break;

                case 0x200:
                {
                    int num2 = CommCtrl.Header_GetColumnAt(base.Handle, new Point(m.LParam.ToInt32()));
                    if (this.HoverColumn == null)
                    {
                        if (num2 >= 0)
                        {
                            this.HoverColumn = this.Parent.Columns[num2];
                        }
                    }
                    else if (this.HoverColumn.Index != num2)
                    {
                        Windows.PostMessage(this.TooltipWnd, 0x41c, IntPtr.Zero, IntPtr.Zero);
                        this.HoverColumn = null;
                    }
                    this.RelayTooltipMessage(ref m);
                    break;
                }
                case 0x2a3:
                    Windows.PostMessage(this.TooltipWnd, 0x41c, IntPtr.Zero, IntPtr.Zero);
                    this.HoverColumn = null;
                    break;
            }
            base.WndProc(ref m);
        }
    }
}

