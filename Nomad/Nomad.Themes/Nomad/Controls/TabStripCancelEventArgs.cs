namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class TabStripCancelEventArgs : CancelEventArgs
    {
        public TabStripCancelEventArgs(Nomad.Controls.Tab tab, bool cancel) : base(cancel)
        {
            this.Tab = tab;
        }

        public Nomad.Controls.Tab Tab { get; private set; }
    }
}

