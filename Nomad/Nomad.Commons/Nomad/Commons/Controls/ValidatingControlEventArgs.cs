namespace Nomad.Commons.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class ValidatingControlEventArgs : CancelEventArgs
    {
        public readonly Control ValidatingControl;

        public ValidatingControlEventArgs(Control control)
        {
            this.ValidatingControl = control;
        }

        public ValidatingControlEventArgs(Control control, bool cancel) : base(cancel)
        {
            this.ValidatingControl = control;
        }
    }
}

