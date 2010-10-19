namespace Microsoft.COM
{
    using System;

    public enum OLEGETMONIKER : uint
    {
        OLEGETMONIKER_FORCEASSIGN = 2,
        OLEGETMONIKER_ONLYIFTHERE = 1,
        OLEGETMONIKER_TEMPFORUSER = 4,
        OLEGETMONIKER_UNASSIGN = 3
    }
}

