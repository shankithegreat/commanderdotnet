namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class WindowStateChangingEventArgs : CancelEventArgs
    {
        public WindowStateChangingEventArgs(FormWindowState currentState, FormWindowState newState)
        {
            this.CurrentWindowState = currentState;
            this.NewWindowState = newState;
        }

        public FormWindowState CurrentWindowState { get; private set; }

        public FormWindowState NewWindowState { get; private set; }
    }
}

