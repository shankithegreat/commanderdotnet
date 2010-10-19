namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GetItemColorsEventArgs : ListViewItemStateEventArgs
    {
        internal GetItemColorsEventArgs(ListView owner, int itemIndex, ListViewItemStates state, Color backColor, Color foreColor) : base(owner, itemIndex, state)
        {
            this.BackColor = backColor;
            this.ForeColor = foreColor;
        }

        public Color BackColor { get; set; }

        public Color ForeColor { get; set; }
    }
}

