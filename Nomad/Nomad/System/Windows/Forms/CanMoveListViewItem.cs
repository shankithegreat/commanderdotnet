namespace System.Windows.Forms
{
    using System;

    [Flags]
    public enum CanMoveListViewItem
    {
        Down = 2,
        DownInGroup = 8,
        Up = 1,
        UpInGroup = 4
    }
}

