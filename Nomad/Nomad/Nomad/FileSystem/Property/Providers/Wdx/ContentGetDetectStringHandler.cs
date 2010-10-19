namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int ContentGetDetectStringHandler([Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder DetectString, int maxlen);
}

