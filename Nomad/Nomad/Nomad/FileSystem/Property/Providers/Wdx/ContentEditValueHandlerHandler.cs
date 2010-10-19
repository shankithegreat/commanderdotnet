namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int ContentEditValueHandlerHandler(IntPtr ParentWin, int FieldIndex, int UnitIndex, int FieldType, IntPtr FieldValue, int maxlen, int flags, [MarshalAs(UnmanagedType.LPStr)] string langidentifier);
}

