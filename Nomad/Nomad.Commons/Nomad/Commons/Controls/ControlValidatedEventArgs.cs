namespace Nomad.Commons.Controls
{
    using System;
    using System.Windows.Forms;

    public class ControlValidatedEventArgs : EventArgs
    {
        public readonly Control ValidatedControl;

        public ControlValidatedEventArgs(Control control)
        {
            this.ValidatedControl = control;
        }
    }
}

