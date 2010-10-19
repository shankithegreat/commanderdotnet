namespace Microsoft.IE.MLang
{
    using System;

    public enum MLDETECTCP : uint
    {
        MLDETECTCP_7BIT = 1,
        MLDETECTCP_8BIT = 2,
        MLDETECTCP_DBCS = 4,
        MLDETECTCP_HTML = 8,
        MLDETECTCP_MASK = 0x1f,
        MLDETECTCP_NONE = 0,
        MLDETECTCP_NUMBER = 0x10
    }
}

