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

    public class ShellListView : ListView
    {
        private int columnHeight;
        private BrowserListSorter sorter = new BrowserListSorter();


        public ShellListView()
        {   
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.OwnerDraw = true;
            this.Alignment = ListViewAlignment.Left;
        }
        
        /// <summary>
        /// Gets or sets how items are displayed in the control.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(View.LargeIcon)]
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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ArrayList SelectedOrder { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SuspendHeaderContextMenu { get; set; }
        
        [Browsable(true)]
        public ContextMenu ColumnHeaderContextMenu { get; set; }

        
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

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            
            // Assign the image lists to the ListView
            ShellImageList.SetSmallImageList(this);
            ShellImageList.SetLargeImageList(this);
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;

            base.OnDrawItem(e);
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;

            base.OnDrawSubItem(e);
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
            columnHeight = e.Bounds.Height;

            base.OnDrawColumnHeader(e);
        }
    }
}
