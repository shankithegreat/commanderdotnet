namespace Microsoft.IE.MLang
{
    using System;

    [Flags]
    public enum MLCPF : uint
    {
        MLDETECTF_BROWSER = 2,
        MLDETECTF_EURO_UTF8 = 0x80,
        MLDETECTF_FILTER_SPECIALCHAR = 0x40,
        MLDETECTF_MAILNEWS = 1,
        MLDETECTF_PREFERRED_ONLY = 0x20,
        MLDETECTF_PRESERVE_ORDER = 0x10,
        MLDETECTF_VALID = 4,
        MLDETECTF_VALID_NLS = 8
    }
}

