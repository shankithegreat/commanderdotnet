namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int ContentSetValueHandler([MarshalAs(UnmanagedType.LPStr)] string FileName, int FieldIndex, int UnitIndex, int FieldType, IntPtr FieldValue, int flags);
}

