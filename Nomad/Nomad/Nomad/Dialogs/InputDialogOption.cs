namespace Nomad.Dialogs
{
    using System;

    [Flags]
    public enum InputDialogOption
    {
        AllowEmptyValue = 1,
        AllowSameValue = 2,
        ReadOnly = 4
    }
}

