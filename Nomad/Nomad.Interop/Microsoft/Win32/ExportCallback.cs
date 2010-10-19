namespace Microsoft.Win32
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public delegate int ExportCallback([MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] byte[] pbData, IntPtr pvCallbackContext, uint ulLength);
}

