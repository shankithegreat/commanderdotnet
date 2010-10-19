namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int ContentSetValueWHandler([MarshalAs(UnmanagedType.LPWStr)] string FileName, int FieldIndex, int UnitIndex, int FieldType, IntPtr FieldValue, int flags);
}

