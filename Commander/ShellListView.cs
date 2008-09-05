using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using ShellDll;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.ComponentModel;

namespace Commander
{
    
    internal class ShellListView : ListView
    {

        private ArrayList selectedOrder;

        private ContextMenu columnHeaderContextMenu;
        private bool suspendHeaderContextMenu;
        private int columnHeight = 0;

        private BrowserListSorter sorter;

        public ShellListView()
        {
            OwnerDraw = true;

            HandleCreated += new EventHandler(BrowserListView_HandleCreated);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            DrawItem += new DrawListViewItemEventHandler(BrowserListView_DrawItem);
            DrawSubItem += new DrawListViewSubItemEventHandler(BrowserListView_DrawSubItem);
            DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(BrowserListView_DrawColumnHeader);

            this.Alignment = ListViewAlignment.Left;
        }

        void BrowserListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        void BrowserListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        void BrowserListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
            columnHeight = e.Bounds.Height;
        }

        public new View View
        {
            get
            {
                return base.View;
            }
            set
            {
                base.View = value;

                if (value == View.Details)
                {
                    foreach (ColumnHeader col in Columns)
                        if (col.Width == 0)
                            col.Width = 120;
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (this.View == View.Details && columnHeaderContextMenu != null &&
                (int)m.Msg == (int)ShellAPI.WM.CONTEXTMENU)
            {
                if (suspendHeaderContextMenu)
                    suspendHeaderContextMenu = false;
                else
                {
                    int x = (int)ShellHelper.LoWord(m.LParam);
                    int y = (int)ShellHelper.HiWord(m.LParam);
                    Point clientPoint = PointToClient(new Point(x, y));

                    if (clientPoint.Y <= columnHeight)
                        columnHeaderContextMenu.Show(this, clientPoint);
                }

                return;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Once the handle is created we can assign the image lists to the ListView
        /// </summary>
        void BrowserListView_HandleCreated(object sender, EventArgs e)
        {
            ShellImageList.SetSmallImageList(this);
            ShellImageList.SetLargeImageList(this);
        }

        [Browsable(false)]
        public ArrayList SelectedOrder
        {
            get { return selectedOrder; }
        }

        [Browsable(false)]
        public bool SuspendHeaderContextMenu
        {
            get { return suspendHeaderContextMenu; }
            set { suspendHeaderContextMenu = value; }
        }

        [Browsable(true)]
        public ContextMenu ColumnHeaderContextMenu
        {
            get { return columnHeaderContextMenu; }
            set { columnHeaderContextMenu = value; }
        }

        public void SetSorting(bool sorting)
        {
            if (sorting)
                this.ListViewItemSorter = sorter;
            else
                this.ListViewItemSorter = null;
        }

        public void ClearSelections()
        {
            selectedOrder.Clear();
            selectedOrder.Capacity = 0;
        }

        public bool GetListItem(ShellItem shellItem, out ListViewItem listItem)
        {
            listItem = null;

            foreach (ListViewItem item in Items)
            {
                if (shellItem.Equals(item.Tag))
                {
                    listItem = item;
                    return true;
                }
            }

            return false;
        }
    }
}
