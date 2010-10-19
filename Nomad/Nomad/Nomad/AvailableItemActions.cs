namespace Nomad
{
    using System;

    [Flags]
    public enum AvailableItemActions
    {
        CanElevate = 4,
        CanIgnore = 2,
        CanRetry = 1,
        CanRetryOrElevate = 5,
        CanRetryOrIgnore = 3,
        CanUndoDestination = 8,
        None = 0
    }
}

