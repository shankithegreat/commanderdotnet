namespace Microsoft.COM
{
    using System;

    [Flags]
    public enum OLECONTF : uint
    {
        OLECONTF_EMBEDDINGS = 1,
        OLECONTF_LINKS = 2,
        OLECONTF_ONLYIFRUNNING = 0x10,
        OLECONTF_ONLYUSER = 8,
        OLECONTF_OTHERS = 4
    }
}

