namespace Nomad.Controls
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ListViewHitTestInfo2 : ListViewHitTestInfo
    {
        public ListViewHitTestInfo2(ListViewItem hitItem, ListViewItem.ListViewSubItem hitSubItem, int columnIndex, ListViewHitTestLocations hitLocation) : base(hitItem, hitSubItem, hitLocation)
        {
            this.ColumnIndex = columnIndex;
        }

        public int ColumnIndex { get; private set; }
    }
}

