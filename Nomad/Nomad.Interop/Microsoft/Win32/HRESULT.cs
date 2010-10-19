namespace Microsoft.Win32
{
    using System;

    public sealed class HRESULT
    {
        public const int COR_E_BADNETPATH = -2147024843;
        public const int COR_E_DIRECTORYNOTFOUND = -2147024893;
        public const int COR_PREFIX_WIN32 = -2147024896;
        public const int E_ABORT = -2147467260;
        public const int E_ACCESSDENIED = -2147024891;
        public const int E_FAIL = -2147467259;
        public const int E_HANDLE = -2147024890;
        public const int E_INVALIDARG = -2147024809;
        public const int E_NOTIMPL = -2147467263;
        public const int E_NOTREADY = -2147024875;
        public const int E_OUTOFMEMORY = -2147024882;
        public const int E_UNEXPECTED = -2147418113;
        public const int FACILITY_WIN32 = 7;
        public const int FILTER_E_ACCESS = -2147215613;
        public const int FILTER_E_EMBEDDING_UNAVAILABLE = -2147215609;
        public const int FILTER_E_END_OF_CHUNKS = -2147215616;
        public const int FILTER_E_LINK_UNAVAILABLE = -2147215608;
        public const int FILTER_E_NO_MORE_TEXT = -2147215615;
        public const int FILTER_E_NO_MORE_VALUES = -2147215614;
        public const int FILTER_E_NO_TEXT = -2147215611;
        public const int FILTER_E_PASSWORD = -2147215605;
        public const int FILTER_E_UNKNOWNFORMAT = -2147215604;
        public const int FILTER_S_LAST_TEXT = 0x41709;
        public const int FILTER_S_LAST_VALUES = 0x4170a;
        public const int FILTER_W_MONIKER_CLIPPED = 0x41704;
        public const int S_FALSE = 1;
        public const int S_OK = 0;
        public const int SEVERITY_ERROR = 1;
        public const int SEVERITY_SUCCESS = 0;
        public const int STG_E_ACCESSDENIED = -2147287035;
        public const int STG_E_FILENOTFOUND = -2147287038;
        public const int STG_E_INVALIDFUNCTION = -2147287039;
        public const int STG_E_PATHNOTFOUND = -2147287037;

        public static bool FAILED(int errorCode)
        {
            return (errorCode < 0);
        }

        public static int HRESULT_FROM_WIN32(int x)
        {
            return ((x <= 0) ? x : (-2147024896 | (x & 0xffff)));
        }

        public static int MAKE_HRESULT(int severity, int facility, int code)
        {
            return (((severity << 0x1f) | (facility << 0x10)) | (code & 0xffff));
        }

        public static bool SUCCEEDED(int errorCode)
        {
            return (errorCode >= 0);
        }
    }
}

