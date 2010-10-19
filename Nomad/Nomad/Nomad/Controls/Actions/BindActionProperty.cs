namespace Nomad.Controls.Actions
{
    using System;

    [Flags]
    public enum BindActionProperty
    {
        All = 0xff,
        CanClick = 0x40,
        CanUpdate = 0x80,
        Checked = 8,
        Enabled = 1,
        Image = 0x10,
        None = 0,
        Shortcuts = 0x20,
        Text = 2,
        Visible = 4
    }
}

