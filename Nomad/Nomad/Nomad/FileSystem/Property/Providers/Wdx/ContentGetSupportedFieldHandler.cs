namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int ContentGetSupportedFieldHandler(int FieldIndex, [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder FieldName, [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder Units, int maxlen);
}

