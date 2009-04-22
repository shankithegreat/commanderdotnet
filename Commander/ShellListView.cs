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
        private int columnHeight = 0;
        private BrowserListSorter sorter;

        public ShellListView()
        {
            OwnerDraw = true;

            HandleCreated += new EventHandler(BrowserListView_HandleCreated);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            DrawItem += BrowserListView_DrawItem;
            DrawSubItem += BrowserListView_DrawSubItem;
            DrawColumnHeader += BrowserListView_DrawColumnHeader;

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
                    foreach (ColumnHeader column in Columns)
                    {
                        if (column.Width == 0)
                        {
                            column.Width = 120;
                        }
                    }
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (this.View == View.Details && ColumnHeaderContextMenu != null &&
                m.Msg == (int)ShellAPI.WM.CONTEXTMENU)
            {
                if (SuspendHeaderContextMenu)
                {
                    SuspendHeaderContextMenu = false;
                }
                else
                {
                    int x = (int)ShellHelper.LoWord(m.LParam);
                    int y = (int)ShellHelper.HiWord(m.LParam);
                    Point clientPoint = PointToClient(new Point(x, y));

                    if (clientPoint.Y <= columnHeight)
                    {
                        ColumnHeaderContextMenu.Show(this, clientPoint);
                    }
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
            get;
            private set;
        }

        [Browsable(false)]
        public bool SuspendHeaderContextMenu
        {
            get;
            set;
        }

        [Browsable(true)]
        public ContextMenu ColumnHeaderContextMenu
        {
            get;
            set;
        }

        public void SetSorting(bool sorting)
        {
            this.ListViewItemSorter = sorting ? sorter : null;
        }

        public void ClearSelections()
        {
            SelectedOrder.Clear();
            SelectedOrder.Capacity = 0;
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
