namespace Microsoft.Win32
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate PROGRESS_RESULT CopyProgressRoutine(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred, uint dwStreamNumber, CALLBACK_REASON dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);
}

