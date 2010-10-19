namespace Microsoft.IO
{
    using Microsoft;
    using Microsoft.Win32;
    using Microsoft.Win32.IOCTL;
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    public class VolumeInfo
    {
        private const string CategoryVolume = "Volume";
        private Microsoft.IO.BusType FBusType;
        private VolumeCapabilities FCapabilities;
        private uint FClusterSize;
        private Microsoft.IO.DeviceCapabilities FDeviceCapabilities;
        private string FDeviceSerialNumber;
        private string FDriveFormat;
        private System.IO.DriveType FDriveType;
        private static Regex FFlashRegex;
        private string FLocation;
        private int FMaxComponentLen;
        private static Regex FNetworkShareRegex;
        private string FProduct;
        private string FProductRevision;
        private uint FSectorSize;
        private uint FSerialNumber;
        private string FUniqueVolumeName;
        private string FVendor;
        private Guid FVolumeGuid;
        private string FVolumeLabel;
        private Microsoft.IO.VolumeType FVolumeType;
        private Retrieved HasRetrieved;

        public VolumeInfo(Guid volumeGuid)
        {
            if (!OS.IsWin2k)
            {
                throw new PlatformNotSupportedException();
            }
            this.FVolumeGuid = volumeGuid;
            this.FUniqueVolumeName = GetUniqueVolumeName(this.FVolumeGuid);
            if (GetDriveType(this.FUniqueVolumeName) == System.IO.DriveType.NoRootDirectory)
            {
                throw new ArgumentException("Volume with such guid was not found");
            }
        }

        public VolumeInfo(string driveName)
        {
            if (driveName == null)
            {
                throw new ArgumentNullException("driveName");
            }
            if ((driveName == string.Empty) || (driveName.Length < 3))
            {
                throw new ArgumentException("driveName is empty or malformed");
            }
            if (OS.IsWinNT && (driveName.Length == 3))
            {
                driveName = ExpandDosDevice(driveName);
            }
            switch (GetDriveType(driveName))
            {
                case System.IO.DriveType.NoRootDirectory:
                    throw new NotSupportedException("NoRootDirectory drives not supported");

                case System.IO.DriveType.Network:
                    break;

                default:
                    if (OS.IsWin2k)
                    {
                        try
                        {
                            this.FVolumeGuid = GetVolumeGuid(driveName);
                            this.FUniqueVolumeName = GetUniqueVolumeName(this.FVolumeGuid);
                        }
                        catch
                        {
                        }
                        return;
                    }
                    break;
            }
            this.FUniqueVolumeName = driveName;
        }

        private VolumeInfo(Guid volumeGuid, bool check)
        {
            this.FVolumeGuid = volumeGuid;
            this.FUniqueVolumeName = GetUniqueVolumeName(this.FVolumeGuid);
        }

        private bool CheckRetrieved(Retrieved check)
        {
            return ((this.HasRetrieved & check) == check);
        }

        public bool Eject()
        {
            DESIRED_ACCESS desired_access;
            System.IO.DriveType driveType = this.DriveType;
            if (driveType != System.IO.DriveType.Removable)
            {
                if (driveType != System.IO.DriveType.CDRom)
                {
                    throw new InvalidOperationException();
                }
                desired_access = (DESIRED_ACCESS) (-2147483648);
            }
            else
            {
                desired_access = (DESIRED_ACCESS) (-1073741824);
            }
            using (SafeFileHandle handle = GetVolumeHandle(this.Name, desired_access, true))
            {
                uint num;
                bool flag = false;
                for (int i = 0; i < 1; i++)
                {
                    if (Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, FSCTL.FSCTL_LOCK_VOLUME, IntPtr.Zero, 0, IntPtr.Zero, 0, out num, IntPtr.Zero))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    throw new TimeoutException(new Win32Exception().Message);
                }
                if (!Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, FSCTL.FSCTL_DISMOUNT_VOLUME, IntPtr.Zero, 0, IntPtr.Zero, 0, out num, IntPtr.Zero))
                {
                    throw GetException(new Win32Exception());
                }
                IntPtr ptr = Marshal.AllocHGlobal(1);
                try
                {
                    Marshal.WriteByte(ptr, 0);
                    if (Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, FSCTL.IOCTL_STORAGE_MEDIA_REMOVAL, ptr, 1, IntPtr.Zero, 0, out num, IntPtr.Zero))
                    {
                        Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, FSCTL.IOCTL_STORAGE_EJECT_MEDIA, IntPtr.Zero, 0, IntPtr.Zero, 0, out num, IntPtr.Zero);
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
                return true;
            }
        }

        private static string ExpandDosDevice(string driveName)
        {
            do
            {
                string path = ParseDosDevice(QueryDosDevice(driveName.Substring(0, driveName.Length - 1)));
                if (!(path != string.Empty))
                {
                    return driveName;
                }
                driveName = Path.GetPathRoot(path);
                if (driveName[driveName.Length - 1] != Path.DirectorySeparatorChar)
                {
                    driveName = driveName + Path.DirectorySeparatorChar;
                }
            }
            while (driveName.Length == 3);
            return driveName;
        }

        public static VolumeInfo FromPath(string path)
        {
            if (!OS.IsWin2k)
            {
                throw new PlatformNotSupportedException();
            }
            StringBuilder lpszVolumePathName = new StringBuilder(0x400);
            if (!Windows.GetVolumePathName(path, lpszVolumePathName, lpszVolumePathName.Capacity))
            {
                throw GetException(new Win32Exception());
            }
            return new VolumeInfo(GetVolumeGuid(lpszVolumePathName.ToString()), false);
        }

        public string GetDefaultPath()
        {
            int num;
            StringBuilder lpszVolumePathNames = new StringBuilder(0x400);
            if (!Windows.GetVolumePathNamesForVolumeName(this.Name, lpszVolumePathNames, lpszVolumePathNames.Capacity, out num))
            {
                throw GetException(new Win32Exception());
            }
            return lpszVolumePathNames.ToString();
        }

        public static System.IO.DriveType GetDriveType(string rootPathName)
        {
            System.IO.DriveType driveType = Windows.GetDriveType(rootPathName);
            switch (driveType)
            {
                case System.IO.DriveType.NoRootDirectory:
                    if (!NetworkShareRegex.IsMatch(rootPathName))
                    {
                        return driveType;
                    }
                    return System.IO.DriveType.Network;

                case System.IO.DriveType.Removable:
                    return driveType;

                case System.IO.DriveType.Fixed:
                {
                    Match match = NetworkShareRegex.Match(rootPathName);
                    if (!match.Success || (match.Groups["server"].Value[0] != '.'))
                    {
                        return driveType;
                    }
                    return System.IO.DriveType.Network;
                }
            }
            return driveType;
        }

        private static Exception GetException(Win32Exception e)
        {
            switch (e.NativeErrorCode)
            {
                case 1:
                    return new InvalidOperationException(e.Message, e);

                case 2:
                case 3:
                    return new DirectoryNotFoundException(e.Message, e);

                case 5:
                    return new UnauthorizedAccessException(e.Message, e);

                case 0x15:
                    return new DeviceNotReadyException(e.Message, e);

                case 0x7b:
                    return new ArgumentException(e.Message, e);
            }
            return new IOException(e.Message, e);
        }

        private string GetLocation()
        {
            string name = this.Name;
            switch (this.DriveType)
            {
                case System.IO.DriveType.Removable:
                case System.IO.DriveType.Fixed:
                    if (OS.IsWinNT)
                    {
                        using (SafeFileHandle handle = GetVolumeHandle(name, DESIRED_ACCESS.FILE_READ_ATTRIBUTES, false))
                        {
                            if (!handle.IsInvalid)
                            {
                                IntPtr lpOutBuffer = Marshal.AllocHGlobal(0x180);
                                try
                                {
                                    uint num;
                                    if (Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, FSCTL.IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, IntPtr.Zero, 0, lpOutBuffer, 0x180, out num, IntPtr.Zero))
                                    {
                                        StringBuilder builder = new StringBuilder();
                                        int num2 = Marshal.ReadInt32(lpOutBuffer);
                                        for (int i = 0; i < num2; i++)
                                        {
                                            if (builder.Length > 0)
                                            {
                                                builder.Append(',');
                                            }
                                            int num4 = Marshal.ReadInt32(lpOutBuffer, 4 + (Marshal.SizeOf(typeof(DISK_EXTENT)) * i));
                                            builder.Append(@"\\.\PhysicalDrive");
                                            builder.Append(num4);
                                        }
                                        return builder.ToString();
                                    }
                                }
                                finally
                                {
                                    Marshal.FreeHGlobal(lpOutBuffer);
                                }
                            }
                        }
                    }
                    if (this.DriveType == System.IO.DriveType.Fixed)
                    {
                        return "HardDisk";
                    }
                    return name;

                case System.IO.DriveType.Network:
                {
                    Match match = NetworkShareRegex.Match(name);
                    if (!match.Success)
                    {
                        return name;
                    }
                    return (@"\\" + match.Groups["server"]);
                }
            }
            return name;
        }

        public static string GetUniqueVolumeName(Guid volumeGuid)
        {
            return (@"\\?\Volume" + volumeGuid.ToString("B") + '\\');
        }

        public static Guid GetVolumeGuid(string volumeMountPoint)
        {
            StringBuilder lpszVolumeName = new StringBuilder(0x400);
            if (!Windows.GetVolumeNameForVolumeMountPoint(volumeMountPoint, lpszVolumeName, lpszVolumeName.Capacity))
            {
                throw GetException(new Win32Exception());
            }
            if (lpszVolumeName.ToString(0, 10) != @"\\?\Volume")
            {
                throw new InvalidDataException();
            }
            return new Guid(lpszVolumeName.ToString(10, 0x26));
        }

        private static SafeFileHandle GetVolumeHandle(string uniqueVolumeName, DESIRED_ACCESS access, bool throwException)
        {
            uniqueVolumeName = uniqueVolumeName.Substring(0, uniqueVolumeName.Length - 1);
            if (uniqueVolumeName.Length == 2)
            {
                uniqueVolumeName = @"\\.\" + uniqueVolumeName;
            }
            SafeFileHandle handle = Windows.CreateFile(uniqueVolumeName, (FileAccess) access, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, FileOptions.None, IntPtr.Zero);
            if (handle.IsInvalid && throwException)
            {
                throw GetException(new Win32Exception());
            }
            return handle;
        }

        public static IEnumerable<string> GetVolumeMountPoints(Guid volumeGuid)
        {
            return GetVolumeMountPoints(GetUniqueVolumeName(volumeGuid));
        }

        public static IEnumerable<string> GetVolumeMountPoints(string uniqueVolumeName)
        {
            return new <GetVolumeMountPoints>d__8(-2) { <>3__uniqueVolumeName = uniqueVolumeName };
        }

        private static IEnumerable<string> GetVolumeNames()
        {
            return new <GetVolumeNames>d__0(-2);
        }

        public static string[] GetVolumePathNames(string uniqueVolumeName)
        {
            int num;
            StringBuilder lpszVolumePathNames = new StringBuilder(0x180);
            if (!Windows.GetVolumePathNamesForVolumeName(uniqueVolumeName, lpszVolumePathNames, lpszVolumePathNames.Capacity, out num))
            {
                int error = Marshal.GetLastWin32Error();
                if (error != 0xea)
                {
                    throw GetException(new Win32Exception(error));
                }
                lpszVolumePathNames = new StringBuilder(num);
                if (!Windows.GetVolumePathNamesForVolumeName(uniqueVolumeName, lpszVolumePathNames, lpszVolumePathNames.Capacity, out num))
                {
                    throw new Win32Exception(error);
                }
            }
            lpszVolumePathNames.Length = num;
            List<string> list = new List<string>();
            int startIndex = 0;
            for (int i = 0; i < lpszVolumePathNames.Length; i++)
            {
                if (lpszVolumePathNames[i] == '\0')
                {
                    if (startIndex == i)
                    {
                        break;
                    }
                    list.Add(lpszVolumePathNames.ToString(startIndex, i - startIndex));
                    startIndex = i + 1;
                }
            }
            return list.ToArray();
        }

        public static VolumeInfo[] GetVolumes()
        {
            if (!OS.IsWin2k)
            {
                throw new PlatformNotSupportedException();
            }
            List<VolumeInfo> list = new List<VolumeInfo>();
            foreach (string str in GetVolumeNames())
            {
                list.Add(new VolumeInfo(GetVolumeGuid(str), false));
            }
            return list.ToArray();
        }

        public static Microsoft.IO.VolumeType GetVolumeType(string uniqueVolumeName)
        {
            if (uniqueVolumeName == null)
            {
                throw new ArgumentNullException("uniqueVolumeName");
            }
            if (uniqueVolumeName == string.Empty)
            {
                throw new ArgumentException("uniqueVolumeName is empty");
            }
            return GetVolumeType(uniqueVolumeName, GetDriveType(uniqueVolumeName));
        }

        public static Microsoft.IO.VolumeType GetVolumeType(string uniqueVolumeName, System.IO.DriveType driveType)
        {
            Debug.WriteLine(string.Format("GetVolumeType({0})", uniqueVolumeName), "Volume");
            if (!OS.IsWinNT || ((driveType != System.IO.DriveType.Removable) && (driveType != System.IO.DriveType.CDRom)))
            {
                goto Label_029F;
            }
            STORAGE_MEDIA_TYPE unknown = STORAGE_MEDIA_TYPE.Unknown;
            using (SafeFileHandle handle = GetVolumeHandle(uniqueVolumeName, DESIRED_ACCESS.FILE_READ_ATTRIBUTES, false))
            {
                if (!handle.IsInvalid)
                {
                    IntPtr lpOutBuffer = Marshal.AllocHGlobal(0x400);
                    try
                    {
                        bool flag;
                        GET_MEDIA_TYPES get_media_types;
                        DISK_GEOMETRY disk_geometry;
                        FSCTL dwIoControlCode = (FSCTL) 0;
                        List<FSCTL> list = new List<FSCTL>();
                        if (OS.IsWinXP)
                        {
                            list.Add(FSCTL.IOCTL_STORAGE_GET_MEDIA_TYPES_EX);
                        }
                        if (OS.IsWin2k)
                        {
                            list.Add(FSCTL.IOCTL_STORAGE_GET_MEDIA_TYPES);
                        }
                        list.Add(FSCTL.IOCTL_DISK_GET_MEDIA_TYPES);
                        list.Add(FSCTL.IOCTL_DISK_GET_DRIVE_GEOMETRY);
                        int num = 0;
                        do
                        {
                            uint num2;
                            dwIoControlCode = list[num++];
                            flag = Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, dwIoControlCode, IntPtr.Zero, 0, lpOutBuffer, 0x400, out num2, IntPtr.Zero);
                            Debug.WriteLine(string.Format("{0} = {1}", dwIoControlCode, flag), "Volume");
                        }
                        while (!flag && (num < list.Count));
                        if (flag)
                        {
                            if (dwIoControlCode != FSCTL.IOCTL_STORAGE_GET_MEDIA_TYPES_EX)
                            {
                                goto Label_01D2;
                            }
                            get_media_types = (GET_MEDIA_TYPES) Marshal.PtrToStructure(lpOutBuffer, typeof(GET_MEDIA_TYPES));
                            switch (get_media_types.DeviceType)
                            {
                                case DEVICE_TYPE.FILE_DEVICE_TAPE:
                                    return Microsoft.IO.VolumeType.Tape;

                                case DEVICE_TYPE.FILE_DEVICE_DVD:
                                    return Microsoft.IO.VolumeType.DVDRom;

                                case DEVICE_TYPE.FILE_DEVICE_CD_ROM:
                                    return Microsoft.IO.VolumeType.CDRom;

                                case DEVICE_TYPE.FILE_DEVICE_DISK:
                                    goto Label_0189;
                            }
                        }
                        goto Label_0216;
                    Label_0189:
                        if (get_media_types.MediaInfoCount > 0)
                        {
                            unknown = get_media_types.MediaInfo[0].DiskInfo.MediaType;
                        }
                        goto Label_0216;
                    Label_01D2:
                        disk_geometry = (DISK_GEOMETRY) Marshal.PtrToStructure(lpOutBuffer, typeof(DISK_GEOMETRY));
                        unknown = (STORAGE_MEDIA_TYPE) disk_geometry.MediaType;
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(lpOutBuffer);
                    }
                }
            }
        Label_0216:
            switch (unknown)
            {
                case STORAGE_MEDIA_TYPE.F5_1Pt2_512:
                case STORAGE_MEDIA_TYPE.F5_360_512:
                case STORAGE_MEDIA_TYPE.F5_320_512:
                case STORAGE_MEDIA_TYPE.F5_320_1024:
                case STORAGE_MEDIA_TYPE.F5_180_512:
                case STORAGE_MEDIA_TYPE.F5_160_512:
                case STORAGE_MEDIA_TYPE.F5_640_512:
                case STORAGE_MEDIA_TYPE.F5_720_512:
                case STORAGE_MEDIA_TYPE.F5_1Pt23_1024:
                    return Microsoft.IO.VolumeType.Floppy5;

                case STORAGE_MEDIA_TYPE.F3_1Pt44_512:
                case STORAGE_MEDIA_TYPE.F3_2Pt88_512:
                case STORAGE_MEDIA_TYPE.F3_20Pt8_512:
                case STORAGE_MEDIA_TYPE.F3_720_512:
                case STORAGE_MEDIA_TYPE.F3_120M_512:
                case STORAGE_MEDIA_TYPE.F3_640_512:
                case STORAGE_MEDIA_TYPE.F3_1Pt2_512:
                case STORAGE_MEDIA_TYPE.F3_1Pt23_1024:
                case STORAGE_MEDIA_TYPE.F3_128Mb_512:
                case STORAGE_MEDIA_TYPE.F3_230Mb_512:
                case STORAGE_MEDIA_TYPE.F3_200Mb_512:
                case STORAGE_MEDIA_TYPE.F3_240M_512:
                case STORAGE_MEDIA_TYPE.F3_32M_512:
                    return Microsoft.IO.VolumeType.Floppy3;

                case STORAGE_MEDIA_TYPE.RemovableMedia:
                    return Microsoft.IO.VolumeType.Removable;

                case STORAGE_MEDIA_TYPE.FixedMedia:
                    return Microsoft.IO.VolumeType.Fixed;

                case STORAGE_MEDIA_TYPE.F8_256_128:
                    goto Label_029F;
            }
        Label_029F:
            return (Microsoft.IO.VolumeType) driveType;
        }

        private void HotPlugNeeded(SafeFileHandle volumeHandle)
        {
            if (!this.CheckRetrieved(Retrieved.HasHotPlugInfo))
            {
                Debug.WriteLine("IOCTL_STORAGE_GET_HOTPLUG_INFO Needed", "Volume");
                int cb = Marshal.SizeOf(typeof(STORAGE_HOTPLUG_INFO));
                IntPtr lpOutBuffer = Marshal.AllocHGlobal(cb);
                try
                {
                    uint num2;
                    if (Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(volumeHandle, FSCTL.IOCTL_STORAGE_GET_HOTPLUG_INFO, IntPtr.Zero, 0, lpOutBuffer, cb, out num2, IntPtr.Zero))
                    {
                        STORAGE_HOTPLUG_INFO storage_hotplug_info = (STORAGE_HOTPLUG_INFO) Marshal.PtrToStructure(lpOutBuffer, typeof(STORAGE_HOTPLUG_INFO));
                        if (storage_hotplug_info.DeviceHotplug)
                        {
                            this.FDeviceCapabilities |= Microsoft.IO.DeviceCapabilities.DeviceHotPlug;
                        }
                        if (storage_hotplug_info.MediaHotplug)
                        {
                            this.FDeviceCapabilities |= Microsoft.IO.DeviceCapabilities.MediaHotPlug;
                        }
                        if (storage_hotplug_info.MediaRemovable)
                        {
                            this.FDeviceCapabilities |= Microsoft.IO.DeviceCapabilities.MediaRemovable;
                        }
                        Debug.WriteLine("HOTPLUG Successful", "Volume");
                    }
                    else
                    {
                        Debug.WriteLine("HOTPLUG Failed. Error = " + Marshal.GetLastWin32Error(), "Volume");
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(lpOutBuffer);
                }
                this.SetRetrieved(Retrieved.HasHotPlugInfo, true);
            }
        }

        private void InitializeVolumeInfo(string driveName)
        {
            FILE_SYSTEM_FLAGS file_system_flags;
            StringBuilder lpVolumeNameBuffer = new StringBuilder(0x105);
            StringBuilder lpFileSystemNameBuffer = new StringBuilder(0x105);
            if (Windows.GetVolumeInformation(driveName, lpVolumeNameBuffer, lpVolumeNameBuffer.Capacity, out this.FSerialNumber, out this.FMaxComponentLen, out file_system_flags, lpFileSystemNameBuffer, lpFileSystemNameBuffer.Capacity))
            {
                this.FCapabilities = (VolumeCapabilities) file_system_flags;
                this.FVolumeLabel = lpVolumeNameBuffer.ToString();
                this.FDriveFormat = lpFileSystemNameBuffer.ToString();
            }
            else
            {
                this.FCapabilities = 0;
                this.FVolumeLabel = string.Empty;
                this.FDriveFormat = string.Empty;
            }
        }

        public static string ParseDosDevice(string target)
        {
            if (target.StartsWith(@"\??\"))
            {
                target = target.Substring(4);
                if (target.StartsWith("UNC"))
                {
                    target = Path.DirectorySeparatorChar + target.Substring(3);
                }
                return target;
            }
            return string.Empty;
        }

        public static string QueryDosDevice(string driveName)
        {
            if (driveName == null)
            {
                throw new ArgumentNullException("driveName");
            }
            if (driveName == string.Empty)
            {
                throw new ArgumentException("driveName is empty");
            }
            if (!OS.IsWinNT)
            {
                throw new PlatformNotSupportedException();
            }
            StringBuilder lpTargetPath = new StringBuilder(0x105);
            if (Windows.QueryDosDevice(driveName, lpTargetPath, lpTargetPath.Capacity) <= 0)
            {
                throw GetException(new Win32Exception());
            }
            return lpTargetPath.ToString();
        }

        private void QueryPropertyNeeded()
        {
            using (SafeFileHandle handle = GetVolumeHandle(this.Name, DESIRED_ACCESS.FILE_READ_ATTRIBUTES, false))
            {
                this.QueryPropertyNeeded(handle);
            }
        }

        private void QueryPropertyNeeded(SafeFileHandle volumeHandle)
        {
            if (!this.CheckRetrieved(Retrieved.HasQueryProperty))
            {
                Debug.WriteLine("IOCTL_STORAGE_QUERY_PROPERTY Needed", "Volume");
                this.FBusType = Microsoft.IO.BusType.Unknown;
                this.FVendor = null;
                this.FProduct = null;
                this.FProductRevision = null;
                this.FDeviceSerialNumber = null;
                if (!volumeHandle.IsInvalid)
                {
                    STORAGE_PROPERTY_QUERY storage_property_query = new STORAGE_PROPERTY_QUERY {
                        PropertyId = STORAGE_PROPERTY_ID.StorageDeviceProperty,
                        QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery
                    };
                    GCHandle handle = GCHandle.Alloc(storage_property_query, GCHandleType.Pinned);
                    try
                    {
                        int cb = 0x180;
                        IntPtr lpOutBuffer = Marshal.AllocHGlobal(cb);
                        try
                        {
                            uint num2;
                            if (Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(volumeHandle, FSCTL.IOCTL_STORAGE_QUERY_PROPERTY, handle.AddrOfPinnedObject(), Marshal.SizeOf(storage_property_query), lpOutBuffer, cb, out num2, IntPtr.Zero))
                            {
                                STORAGE_DEVICE_DESCRIPTOR storage_device_descriptor = (STORAGE_DEVICE_DESCRIPTOR) Marshal.PtrToStructure(lpOutBuffer, typeof(STORAGE_DEVICE_DESCRIPTOR));
                                Microsoft.IO.BusType busType = (Microsoft.IO.BusType) storage_device_descriptor.BusType;
                                if (Enum.IsDefined(typeof(Microsoft.IO.BusType), busType))
                                {
                                    this.FBusType = busType;
                                }
                                if (storage_device_descriptor.RemovableMedia)
                                {
                                    this.FDeviceCapabilities |= Microsoft.IO.DeviceCapabilities.MediaRemovable;
                                }
                                long num3 = lpOutBuffer.ToInt64();
                                if ((storage_device_descriptor.VendorIdOffset > 0) && (storage_device_descriptor.VendorIdOffset != uint.MaxValue))
                                {
                                    this.FVendor = Marshal.PtrToStringAnsi((IntPtr) (num3 + storage_device_descriptor.VendorIdOffset)).TrimEnd(new char[0]);
                                }
                                if ((storage_device_descriptor.ProductIdOffset > 0) && (storage_device_descriptor.ProductIdOffset != uint.MaxValue))
                                {
                                    this.FProduct = Marshal.PtrToStringAnsi((IntPtr) (num3 + storage_device_descriptor.ProductIdOffset)).TrimEnd(new char[0]);
                                }
                                if ((storage_device_descriptor.ProductRevisionOffset > 0) && (storage_device_descriptor.ProductRevisionOffset != uint.MaxValue))
                                {
                                    this.FProductRevision = Marshal.PtrToStringAnsi((IntPtr) (num3 + storage_device_descriptor.ProductRevisionOffset)).TrimEnd(new char[0]);
                                }
                                if ((storage_device_descriptor.SerialNumberOffset > 0) && (storage_device_descriptor.SerialNumberOffset != uint.MaxValue))
                                {
                                    this.FDeviceSerialNumber = Marshal.PtrToStringAnsi((IntPtr) (num3 + storage_device_descriptor.SerialNumberOffset)).TrimEnd(new char[0]);
                                }
                                Debug.WriteLine("QUERY_PROPERTY Successful", "Volume");
                            }
                            else
                            {
                                Debug.WriteLine("QUERY_PROPERTY Failed. Error = " + Marshal.GetLastWin32Error(), "Volume");
                            }
                        }
                        finally
                        {
                            Marshal.FreeHGlobal(lpOutBuffer);
                        }
                    }
                    finally
                    {
                        handle.Free();
                    }
                }
                this.SetRetrieved(Retrieved.HasQueryProperty, true);
            }
        }

        public void Refresh()
        {
            this.SetRetrieved(Retrieved.HasSectorInfo | Retrieved.HasVolumeInfo, false);
        }

        private void SectorInfoNeeded()
        {
            if (!this.CheckRetrieved(Retrieved.HasSectorInfo))
            {
                uint num;
                uint num2;
                if (!Windows.GetDiskFreeSpace(this.Name, out this.FClusterSize, out this.FSectorSize, out num, out num2))
                {
                    Win32Exception innerException = new Win32Exception();
                    throw new IOException(innerException.Message, innerException);
                }
                this.FClusterSize *= this.FSectorSize;
                this.SetRetrieved(Retrieved.HasSectorInfo, true);
            }
        }

        private void SetRetrieved(Retrieved capabilities, bool value)
        {
            if (value)
            {
                this.HasRetrieved |= capabilities;
            }
            else
            {
                this.HasRetrieved &= ~capabilities;
            }
        }

        public override string ToString()
        {
            return this.FUniqueVolumeName;
        }

        private void VolumeInfoNeeded()
        {
            if (!this.CheckRetrieved(Retrieved.HasVolumeInfo))
            {
                this.InitializeVolumeInfo(this.Name);
                this.SetRetrieved(Retrieved.HasVolumeInfo, true);
            }
        }

        public Microsoft.IO.BusType BusType
        {
            get
            {
                this.QueryPropertyNeeded();
                return this.FBusType;
            }
        }

        public bool CanEject
        {
            get
            {
                switch (this.DriveType)
                {
                    case System.IO.DriveType.Removable:
                    case System.IO.DriveType.CDRom:
                        return true;

                    case System.IO.DriveType.Fixed:
                        return ((this.DeviceCapabilities & (Microsoft.IO.DeviceCapabilities.MediaRemovable | Microsoft.IO.DeviceCapabilities.DeviceHotPlug)) > 0);
                }
                return false;
            }
        }

        public VolumeCapabilities Capabilities
        {
            get
            {
                this.VolumeInfoNeeded();
                return this.FCapabilities;
            }
        }

        public uint ClusterSize
        {
            get
            {
                this.SectorInfoNeeded();
                return this.FClusterSize;
            }
        }

        public Microsoft.IO.DeviceCapabilities DeviceCapabilities
        {
            get
            {
                if (!this.CheckRetrieved(Retrieved.HasQueryProperty) || !this.CheckRetrieved(Retrieved.HasHotPlugInfo))
                {
                    using (SafeFileHandle handle = GetVolumeHandle(this.Name, DESIRED_ACCESS.FILE_READ_ATTRIBUTES, false))
                    {
                        if (handle.IsInvalid)
                        {
                            this.FDeviceCapabilities = 0;
                            this.SetRetrieved(Retrieved.HasHotPlugInfo, true);
                        }
                        else
                        {
                            this.HotPlugNeeded(handle);
                        }
                        this.QueryPropertyNeeded(handle);
                    }
                }
                switch (this.DriveType)
                {
                    case System.IO.DriveType.Removable:
                    case System.IO.DriveType.CDRom:
                        this.FDeviceCapabilities |= Microsoft.IO.DeviceCapabilities.MediaRemovable;
                        break;
                }
                Debug.WriteLine(string.Format("DeviceCapabilities = {0}", this.FDeviceCapabilities), "Volume");
                return this.FDeviceCapabilities;
            }
        }

        public string DriveFormat
        {
            get
            {
                this.VolumeInfoNeeded();
                return this.FDriveFormat;
            }
        }

        public System.IO.DriveType DriveType
        {
            get
            {
                if (!this.CheckRetrieved(Retrieved.HasDriveType))
                {
                    this.FDriveType = GetDriveType(this.Name);
                    this.SetRetrieved(Retrieved.HasDriveType, true);
                }
                return this.FDriveType;
            }
        }

        private static Regex FlashRegex
        {
            get
            {
                if (FFlashRegex == null)
                {
                    FFlashRegex = new Regex("^USB (SD|CF|SM|MS) Reader$", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
                }
                return FFlashRegex;
            }
        }

        public bool IsLocalDrive
        {
            get
            {
                switch (this.DriveType)
                {
                    case System.IO.DriveType.Removable:
                    case System.IO.DriveType.Fixed:
                    case System.IO.DriveType.CDRom:
                    case System.IO.DriveType.Ram:
                        return true;
                }
                return false;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((this.DriveType == System.IO.DriveType.CDRom) || ((this.Capabilities & VolumeCapabilities.ReadOnlyVolume) > ((VolumeCapabilities) 0)));
            }
        }

        public bool IsReady
        {
            get
            {
                uint num;
                Win32Exception exception;
                bool flag;
                FSCTL dwIoControlCode = FSCTL.IOCTL_DISK_CHECK_VERIFY;
                if (!OS.IsWin2k)
                {
                    goto Label_00EA;
                }
                SafeFileHandle hDevice = GetVolumeHandle(this.Name, DESIRED_ACCESS.FILE_READ_ATTRIBUTES, false);
                if (hDevice.IsInvalid)
                {
                    exception = new Win32Exception();
                    if (exception.NativeErrorCode != 0x35)
                    {
                        throw new IOException(exception.Message, exception);
                    }
                    return false;
                }
                using (hDevice)
                {
                    if (Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(hDevice, FSCTL.IOCTL_STORAGE_CHECK_VERIFY2, IntPtr.Zero, 0, IntPtr.Zero, 0, out num, IntPtr.Zero))
                    {
                        return true;
                    }
                    exception = new Win32Exception();
                    switch (exception.NativeErrorCode)
                    {
                        case 1:
                            goto Label_00E2;

                        case 0x15:
                            return false;
                    }
                    throw new IOException(exception.Message, exception);
                }
            Label_00E2:
                dwIoControlCode = FSCTL.IOCTL_STORAGE_CHECK_VERIFY;
            Label_00EA:
                using (hDevice = GetVolumeHandle(this.Name, DESIRED_ACCESS.FILE_READ_DATA, true))
                {
                    if (Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(hDevice, dwIoControlCode, IntPtr.Zero, 0, IntPtr.Zero, 0, out num, IntPtr.Zero))
                    {
                        return true;
                    }
                    exception = new Win32Exception();
                    int nativeErrorCode = exception.NativeErrorCode;
                    if (nativeErrorCode == 1)
                    {
                        throw new InvalidOperationException(exception.Message, exception);
                    }
                    if (nativeErrorCode != 0x15)
                    {
                        throw new IOException(exception.Message, exception);
                    }
                    return false;
                }
                return flag;
            }
        }

        public bool IsSlowDrive
        {
            get
            {
                switch (this.DriveType)
                {
                    case System.IO.DriveType.Fixed:
                    case System.IO.DriveType.Ram:
                        return false;
                }
                return true;
            }
        }

        public string Location
        {
            get
            {
                if (this.FLocation == null)
                {
                    this.FLocation = this.GetLocation();
                }
                return this.FLocation;
            }
        }

        public int MaxComponentLength
        {
            get
            {
                this.VolumeInfoNeeded();
                return this.FMaxComponentLen;
            }
        }

        public bool Mounted
        {
            get
            {
                using (SafeFileHandle handle = GetVolumeHandle(this.Name, DESIRED_ACCESS.FILE_READ_ATTRIBUTES, true))
                {
                    uint num;
                    return Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, FSCTL.FSCTL_IS_VOLUME_MOUNTED, IntPtr.Zero, 0, IntPtr.Zero, 0, out num, IntPtr.Zero);
                }
            }
        }

        public string Name
        {
            get
            {
                return this.FUniqueVolumeName;
            }
        }

        private static Regex NetworkShareRegex
        {
            get
            {
                if (FNetworkShareRegex == null)
                {
                    FNetworkShareRegex = new Regex(@"^\\\\(?<server>[^\p{C}\\:><\|?]+)\\(?<share>[^\p{C}\\:><\|?]+)\\?$", RegexOptions.Singleline | RegexOptions.Compiled);
                }
                return FNetworkShareRegex;
            }
        }

        public bool Offline
        {
            get
            {
                using (SafeFileHandle handle = GetVolumeHandle(this.Name, DESIRED_ACCESS.FILE_READ_ATTRIBUTES, true))
                {
                    uint num;
                    return Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, FSCTL.IOCTL_VOLUME_IS_OFFLINE, IntPtr.Zero, 0, IntPtr.Zero, 0, out num, IntPtr.Zero);
                }
            }
            set
            {
                SafeFileHandle handle;
                uint num;
                FSCTL dwIoControlCode = FSCTL.IOCTL_VOLUME_OFFLINE;
                using (handle = GetVolumeHandle(this.Name, DESIRED_ACCESS.FILE_READ_ATTRIBUTES, true))
                {
                    if (!Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, FSCTL.IOCTL_VOLUME_SUPPORTS_ONLINE_OFFLINE, IntPtr.Zero, 0, IntPtr.Zero, 0, out num, IntPtr.Zero))
                    {
                        throw new NotSupportedException();
                    }
                    bool flag = Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, FSCTL.IOCTL_VOLUME_IS_OFFLINE, IntPtr.Zero, 0, IntPtr.Zero, 0, out num, IntPtr.Zero);
                    if (flag == value)
                    {
                        return;
                    }
                    dwIoControlCode = flag ? FSCTL.IOCTL_VOLUME_ONLINE : FSCTL.IOCTL_VOLUME_OFFLINE;
                }
                using (handle = GetVolumeHandle(this.Name, (DESIRED_ACCESS) (-1073741824), true))
                {
                    bool flag2 = Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, dwIoControlCode, IntPtr.Zero, 0, IntPtr.Zero, 0, out num, IntPtr.Zero);
                }
            }
        }

        public string[] PathNames
        {
            get
            {
                if (OS.IsWin2k)
                {
                    return GetVolumePathNames(this.Name);
                }
                return new string[] { this.Name };
            }
        }

        public string Product
        {
            get
            {
                this.QueryPropertyNeeded();
                return this.FProduct;
            }
        }

        public string ProductRevision
        {
            get
            {
                this.QueryPropertyNeeded();
                return this.FProductRevision;
            }
        }

        public uint SectorSize
        {
            get
            {
                this.SectorInfoNeeded();
                return this.FSectorSize;
            }
        }

        public uint SerialNumber
        {
            get
            {
                this.VolumeInfoNeeded();
                return this.FSerialNumber;
            }
        }

        public string Vendor
        {
            get
            {
                this.QueryPropertyNeeded();
                return this.FVendor;
            }
        }

        public Guid VolumeGuid
        {
            get
            {
                return this.FVolumeGuid;
            }
        }

        public string VolumeLabel
        {
            get
            {
                this.VolumeInfoNeeded();
                return this.FVolumeLabel;
            }
        }

        public Microsoft.IO.VolumeType VolumeType
        {
            get
            {
                if (!this.CheckRetrieved(Retrieved.HasVolumeType))
                {
                    this.FVolumeType = GetVolumeType(this.Name, this.DriveType);
                    if (((this.FVolumeType == Microsoft.IO.VolumeType.Removable) && !string.IsNullOrEmpty(this.Product)) && FlashRegex.IsMatch(this.Product))
                    {
                        this.FVolumeType = Microsoft.IO.VolumeType.Flash;
                    }
                    this.SetRetrieved(Retrieved.HasVolumeType, true);
                }
                return this.FVolumeType;
            }
        }

        [CompilerGenerated]
        private sealed class <GetVolumeMountPoints>d__8 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private string <>2__current;
            public string <>3__uniqueVolumeName;
            public SafeFindVolumeMountPointHandle <>7__wrapc;
            private int <>l__initialThreadId;
            public int <ErrorCode>5__b;
            public SafeFindVolumeMountPointHandle <FindHandle>5__a;
            public StringBuilder <MountPointBuilder>5__9;
            public string uniqueVolumeName;

            [DebuggerHidden]
            public <GetVolumeMountPoints>d__8(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finallyd()
            {
                this.<>1__state = -1;
                if (this.<>7__wrapc != null)
                {
                    this.<>7__wrapc.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    Win32Exception exception;
                    switch (this.<>1__state)
                    {
                        case 0:
                            break;

                        case 2:
                            this.<>1__state = 1;
                            if (Windows.FindNextVolumeMountPoint(this.<FindHandle>5__a, this.<MountPointBuilder>5__9, this.<MountPointBuilder>5__9.Capacity))
                            {
                                goto Label_00EA;
                            }
                            this.<ErrorCode>5__b = Marshal.GetLastWin32Error();
                            if (this.<ErrorCode>5__b != 0x12)
                            {
                                exception = new Win32Exception(this.<ErrorCode>5__b);
                                throw new IOException(exception.Message, exception);
                            }
                            this.<>m__Finallyd();
                            goto Label_016B;

                        default:
                            goto Label_016B;
                    }
                    this.<>1__state = -1;
                    if (this.uniqueVolumeName == null)
                    {
                        throw new ArgumentNullException();
                    }
                    if (this.uniqueVolumeName == string.Empty)
                    {
                        throw new ArgumentException();
                    }
                    if (!OS.IsWin2k)
                    {
                        throw new PlatformNotSupportedException();
                    }
                    this.<MountPointBuilder>5__9 = new StringBuilder(0x400);
                    this.<FindHandle>5__a = Windows.FindFirstVolumeMountPoint(this.uniqueVolumeName, this.<MountPointBuilder>5__9, this.<MountPointBuilder>5__9.Capacity);
                    if (this.<FindHandle>5__a.IsInvalid)
                    {
                        exception = new Win32Exception();
                        if (exception.NativeErrorCode != 0x12)
                        {
                            throw VolumeInfo.GetException(exception);
                        }
                        goto Label_016B;
                    }
                    this.<>7__wrapc = this.<FindHandle>5__a;
                    this.<>1__state = 1;
                Label_00EA:
                    this.<>2__current = this.<MountPointBuilder>5__9.ToString();
                    this.<>1__state = 2;
                    return true;
                Label_016B:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                VolumeInfo.<GetVolumeMountPoints>d__8 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new VolumeInfo.<GetVolumeMountPoints>d__8(0);
                }
                d__.uniqueVolumeName = this.<>3__uniqueVolumeName;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.String>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finallyd();
                        }
                        break;
                }
            }

            string IEnumerator<string>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetVolumeNames>d__0 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private string <>2__current;
            public SafeFindVolumeHandle <>7__wrap4;
            private int <>l__initialThreadId;
            public int <ErrorCode>5__3;
            public SafeFindVolumeHandle <FindHandle>5__2;
            public StringBuilder <VolumeBuilder>5__1;

            [DebuggerHidden]
            public <GetVolumeNames>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally5()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap4 != null)
                {
                    this.<>7__wrap4.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    Win32Exception exception;
                    switch (this.<>1__state)
                    {
                        case 0:
                            break;

                        case 2:
                            this.<>1__state = 1;
                            if (Windows.FindNextVolume(this.<FindHandle>5__2, this.<VolumeBuilder>5__1, this.<VolumeBuilder>5__1.Capacity))
                            {
                                goto Label_00A4;
                            }
                            this.<ErrorCode>5__3 = Marshal.GetLastWin32Error();
                            if (this.<ErrorCode>5__3 != 0x12)
                            {
                                exception = new Win32Exception(this.<ErrorCode>5__3);
                                throw new IOException(exception.Message, exception);
                            }
                            this.<>m__Finally5();
                            goto Label_0125;

                        default:
                            goto Label_0125;
                    }
                    this.<>1__state = -1;
                    if (!OS.IsWin2k)
                    {
                        throw new PlatformNotSupportedException();
                    }
                    this.<VolumeBuilder>5__1 = new StringBuilder(0x400);
                    this.<FindHandle>5__2 = Windows.FindFirstVolume(this.<VolumeBuilder>5__1, this.<VolumeBuilder>5__1.Capacity);
                    if (this.<FindHandle>5__2.IsInvalid)
                    {
                        exception = new Win32Exception();
                        throw new IOException(exception.Message, exception);
                    }
                    this.<>7__wrap4 = this.<FindHandle>5__2;
                    this.<>1__state = 1;
                Label_00A4:
                    this.<>2__current = this.<VolumeBuilder>5__1.ToString();
                    this.<>1__state = 2;
                    return true;
                Label_0125:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new VolumeInfo.<GetVolumeNames>d__0(0);
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.String>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally5();
                        }
                        break;
                }
            }

            string IEnumerator<string>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [Flags]
        private enum Retrieved
        {
            HasDriveType = 1,
            HasHotPlugInfo = 0x10,
            HasQueryProperty = 0x20,
            HasSectorInfo = 8,
            HasVolumeInfo = 4,
            HasVolumeType = 2
        }
    }
}

