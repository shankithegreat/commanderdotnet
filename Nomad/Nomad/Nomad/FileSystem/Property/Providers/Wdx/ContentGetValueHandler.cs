namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int ContentGetValueHandler([MarshalAs(UnmanagedType.LPStr)] string FileName, int FieldIndex, int UnitIndex, IntPtr FieldValue, int maxlen, int flags);
}

