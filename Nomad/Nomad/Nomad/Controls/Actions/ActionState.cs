namespace Nomad.Controls.Actions
{
    using System;

    [Flags]
    public enum ActionState
    {
        Checked = 4,
        Enabled = 1,
        Indeterminate = 8,
        None = 0,
        Visible = 2
    }
}

