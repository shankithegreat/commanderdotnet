namespace Microsoft.Win32
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate int ImportCallback(IntPtr pbData, IntPtr pvCallbackContext, ref uint ulLength);
}

