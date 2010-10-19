namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void ContentSendStateInformationWHandler(int state, [MarshalAs(UnmanagedType.LPWStr)] string path);
}

