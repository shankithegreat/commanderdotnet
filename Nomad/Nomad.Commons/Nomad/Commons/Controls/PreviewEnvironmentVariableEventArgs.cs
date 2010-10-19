namespace Nomad.Commons.Controls
{
    using System;
    using System.Windows.Forms;

    public class PreviewEnvironmentVariableEventArgs : PreviewValueEventArgs<string>
    {
        internal PreviewEnvironmentVariableEventArgs(Control target) : base(target)
        {
        }

        public PreviewEnvironmentVariableEventArgs(Control target, string value) : base(target, value)
        {
        }
    }
}

