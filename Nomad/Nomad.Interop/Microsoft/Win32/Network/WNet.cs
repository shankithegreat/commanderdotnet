namespace Microsoft.Win32.Network
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public static class WNet
    {
        public static void CheckWNetError(int errorCode)
        {
            if (errorCode == 0x4b8)
            {
                int num;
                StringBuilder lpErrorBuf = new StringBuilder(0x400);
                StringBuilder lpNameBuf = new StringBuilder(0x400);
                Winnetwk.WNetGetLastError(out num, lpErrorBuf, lpErrorBuf.Capacity, lpNameBuf, lpNameBuf.Capacity);
                throw new NetworkException(lpErrorBuf.ToString(), num, lpNameBuf.ToString());
            }
            if (errorCode != 0)
            {
                Exception innerException = new Win32Exception(errorCode);
                throw new IOException(innerException.Message, innerException);
            }
        }

        public static bool DisconnectNetworkDrive(IWin32Window window)
        {
            if (window == null)
            {
                throw new ArgumentNullException();
            }
            int errorCode = Winnetwk.WNetDisconnectDialog(window.Handle, RESOURCETYPE.RESOURCETYPE_DISK);
            switch (errorCode)
            {
                case -1:
                    return false;

                case 0:
                    return true;
            }
            CheckWNetError(errorCode);
            return false;
        }

        public static bool MapNetworkDrive(IWin32Window window)
        {
            if (window == null)
            {
                throw new ArgumentNullException();
            }
            int errorCode = Winnetwk.WNetConnectionDialog(window.Handle, RESOURCETYPE.RESOURCETYPE_DISK);
            switch (errorCode)
            {
                case -1:
                    return false;

                case 0:
                    return true;
            }
            CheckWNetError(errorCode);
            return false;
        }
    }
}

