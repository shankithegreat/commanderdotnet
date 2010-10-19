namespace Microsoft.IO
{
    using Microsoft.Win32;
    using Microsoft.Win32.IOCTL;
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class ReparsePoint
    {
        public static void Create(string emptyFolder, string targetFolder)
        {
            if (emptyFolder == null)
            {
                throw new ArgumentNullException("emptyFolder");
            }
            if (emptyFolder == string.Empty)
            {
                throw new ArgumentException("String is empty.", "emptyFolder");
            }
            if (targetFolder == null)
            {
                throw new ArgumentNullException("targetFolder");
            }
            if (targetFolder == string.Empty)
            {
                throw new ArgumentException("String is empty.", "targetFolder");
            }
            if (string.IsNullOrEmpty(Path.GetPathRoot(targetFolder)))
            {
                throw new ArgumentException("targetFolder is not full quilified folder path.", "targetFolder");
            }
            using (SafeFileHandle handle = OpenReparsePoint(emptyFolder, FileAccess.Write))
            {
                REPARSE_DATA_BUFFER structure = new REPARSE_DATA_BUFFER {
                    ReparseTag = 0xa0000003
                };
                StringBuilder builder = new StringBuilder(@"\??\");
                builder.Append(targetFolder);
                if (builder[builder.Length - 1] != Path.DirectorySeparatorChar)
                {
                    builder.Append(Path.DirectorySeparatorChar);
                }
                structure.PathBuffer = new byte[0x3ff0];
                structure.SubstituteNameLength = (ushort) Encoding.Unicode.GetBytes(builder.ToString(), 0, builder.Length, structure.PathBuffer, 0);
                structure.SubstituteNameOffset = 0;
                structure.ReparseDataLength = (ushort) (structure.SubstituteNameLength + 12);
                structure.PrintNameOffset = (ushort) (structure.SubstituteNameLength + 2);
                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
                try
                {
                    uint num;
                    Marshal.StructureToPtr(structure, ptr, false);
                    if (!Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, FSCTL.FSCTL_SET_REPARSE_POINT, ptr, structure.SubstituteNameLength + 20, IntPtr.Zero, 0, out num, IntPtr.Zero))
                    {
                        throw IoHelper.GetIOException();
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        private static ReparsePointInfo GetReparsePointInfo(SafeFileHandle handle)
        {
            ReparsePointInfo info;
            int cb = Marshal.SizeOf(typeof(REPARSE_DATA_BUFFER));
            IntPtr lpOutBuffer = Marshal.AllocHGlobal(cb);
            try
            {
                uint num2;
                if (!Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, FSCTL.FSCTL_GET_REPARSE_POINT, IntPtr.Zero, 0, lpOutBuffer, cb, out num2, IntPtr.Zero))
                {
                    int nativeErrorCode = Marshal.GetLastWin32Error();
                    if (nativeErrorCode != 0x1126)
                    {
                        throw IoHelper.GetIOException(nativeErrorCode);
                    }
                    return null;
                }
                info = new ReparsePointInfo((REPARSE_DATA_BUFFER) Marshal.PtrToStructure(lpOutBuffer, typeof(REPARSE_DATA_BUFFER)));
            }
            finally
            {
                Marshal.FreeHGlobal(lpOutBuffer);
            }
            return info;
        }

        public static ReparsePointInfo GetReparsePointInfo(string reparsePoint)
        {
            if (reparsePoint == null)
            {
                throw new ArgumentNullException("reparsePoint");
            }
            if (reparsePoint == string.Empty)
            {
                throw new ArgumentException("reparsePoint is empty");
            }
            using (SafeFileHandle handle = OpenReparsePoint(reparsePoint, 0x80))
            {
                ReparsePointInfo reparsePointInfo = GetReparsePointInfo(handle);
                if ((reparsePointInfo != null) && (reparsePointInfo.ReparseType != ReparseType.Unknown))
                {
                    return reparsePointInfo;
                }
            }
            return null;
        }

        public static string GetTarget(string reparsePoint)
        {
            ReparsePointInfo reparsePointInfo = GetReparsePointInfo(reparsePoint);
            return ((reparsePointInfo != null) ? reparsePointInfo.Target : null);
        }

        private static SafeFileHandle OpenReparsePoint(string reparsePoint, FileAccess accessMode)
        {
            SafeFileHandle handle = Windows.CreateFile(reparsePoint, accessMode, FileShare.Delete | FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, 0x2200000, IntPtr.Zero);
            if (handle.IsInvalid)
            {
                throw IoHelper.GetIOException();
            }
            return handle;
        }
    }
}

