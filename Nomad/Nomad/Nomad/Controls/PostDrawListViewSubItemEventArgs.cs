namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class PostDrawListViewSubItemEventArgs : PostDrawListViewItemEventArgs
    {
        private int FSubItemIndex;

        internal PostDrawListViewSubItemEventArgs(Graphics g, ListView owner, int itemIndex, int subItemIndex, Rectangle bounds, ListViewItemStates state) : base(g, owner, itemIndex, bounds, state)
        {
            this.FSubItemIndex = subItemIndex;
        }

        public int ColumnIndex
        {
            get
            {
                return this.FSubItemIndex;
            }
        }

        public ColumnHeader Header
        {
            get
            {
                return ((this.FSubItemIndex < base.Owner.Columns.Count) ? base.Owner.Columns[this.FSubItemIndex] : null);
            }
        }

        public ListViewItem.ListViewSubItem SubItem
        {
            get
            {
                return ((this.FSubItemIndex < base.Item.SubItems.Count) ? base.Item.SubItems[this.FSubItemIndex] : null);
            }
        }
    }
}

