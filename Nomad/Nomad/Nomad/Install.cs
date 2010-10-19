namespace Nomad
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static class Install
    {
        [DllImport("mscoree.dll")]
        private static extern int GetCORSystemDirectory([MarshalAs(UnmanagedType.LPWStr)] StringBuilder buffer, int bufferLength, out int length);
        public static bool RunNGen(string command, string assembly)
        {
            try
            {
                Process process = Process.Start(new ProcessStartInfo(Path.Combine(CORSystemDirectory, "ngen.exe"), string.Concat(new object[] { command, " \"", assembly, '"' })) { WindowStyle = ProcessWindowStyle.Hidden });
                process.WaitForExit();
                return (process.ExitCode == 0);
            }
            catch
            {
                return false;
            }
        }

        public static string CORSystemDirectory
        {
            get
            {
                int num;
                StringBuilder buffer = new StringBuilder(0x400);
                int errorCode = GetCORSystemDirectory(buffer, buffer.Capacity, out num);
                if (errorCode != 0)
                {
                    throw Marshal.GetExceptionForHR(errorCode);
                }
                return buffer.ToString();
            }
        }
    }
}

