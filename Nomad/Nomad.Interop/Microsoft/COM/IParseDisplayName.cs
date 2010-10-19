namespace Microsoft.COM
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000011A-0000-0000-C000-000000000046")]
    public interface IParseDisplayName
    {
        void ParseDisplayName([MarshalAs(UnmanagedType.Interface)] object pbc, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, out int pchEaten, out IMoniker ppmkOut);
    }
}

