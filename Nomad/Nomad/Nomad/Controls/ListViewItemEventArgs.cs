namespace Nomad.Controls
{
    using System;
    using System.Windows.Forms;

    public class ListViewItemEventArgs : EventArgs
    {
        private ListViewItem FItem;
        private int FItemIndex;
        protected readonly ListView Owner;

        internal ListViewItemEventArgs(ListView owner, int itemIndex)
        {
            this.Owner = owner;
            this.FItemIndex = itemIndex;
        }

        public ListViewItem Item
        {
            get
            {
                if (this.FItem == null)
                {
                    this.FItem = this.Owner.Items[this.FItemIndex];
                }
                return this.FItem;
            }
        }

        public int ItemIndex
        {
            get
            {
                return this.FItemIndex;
            }
        }
    }
}

