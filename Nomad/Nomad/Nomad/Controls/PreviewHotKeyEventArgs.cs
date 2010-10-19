namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class PreviewHotKeyEventArgs : CancelEventArgs
    {
        public Keys HotKey;

        public PreviewHotKeyEventArgs(Keys hotKey)
        {
            this.HotKey = hotKey;
        }
    }
}

