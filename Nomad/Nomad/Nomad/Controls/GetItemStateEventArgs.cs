namespace Nomad.Controls
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GetItemStateEventArgs : ListViewItemEventArgs
    {
        internal GetItemStateEventArgs(ListView owner, int itemIndex) : base(owner, itemIndex)
        {
        }

        public bool Cut { get; set; }

        public bool DropHilited { get; set; }
    }
}

