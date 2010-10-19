namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum LOAD_LIBRARY : uint
    {
        DONT_RESOLVE_DLL_REFERENCES = 1,
        LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x10,
        LOAD_LIBRARY_AS_DATAFILE = 2,
        LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x40,
        LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x20,
        LOAD_WITH_ALTERED_SEARCH_PATH = 8
    }
}

