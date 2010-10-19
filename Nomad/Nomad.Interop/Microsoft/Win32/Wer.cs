namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    public static class Wer
    {
        private const string WerDll = "Wer.dll";

        [DllImport("Wer.dll")]
        public static extern int WerAddExcludedApplication([MarshalAs(UnmanagedType.BStr)] string pwzExeName, [MarshalAs(UnmanagedType.Bool)] bool bAllUsers);
    }
}

