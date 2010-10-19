namespace Nomad.Controls
{
    using System;
    using System.Windows.Forms;

    public class ListViewItemStateEventArgs : ListViewItemEventArgs
    {
        private ListViewItemStates FState;

        internal ListViewItemStateEventArgs(ListView owner, int itemIndex, ListViewItemStates state) : base(owner, itemIndex)
        {
            this.FState = state;
        }

        public ListViewItemStates State
        {
            get
            {
                return this.FState;
            }
        }
    }
}

