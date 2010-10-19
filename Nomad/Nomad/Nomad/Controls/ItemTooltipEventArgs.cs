namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ItemTooltipEventArgs : CancelEventArgs
    {
        public ItemTooltipEventArgs(ListViewItem item, string tooltip)
        {
            this.Item = item;
            this.Tooltip = tooltip;
        }

        public ListViewItem Item { get; private set; }

        public string Tooltip { get; set; }
    }
}

