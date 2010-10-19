namespace Microsoft.Win32
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Security;
    using System.Text;

    [SuppressUnmanagedCodeSecurity]
    public static class Windows
    {
        public const byte AC_SRC_ALPHA = 1;
        public const byte AC_SRC_OVER = 0;
        public const uint ACCESS_SYSTEM_SECURITY = 0x1000000;
        private const string Gdi32Dll = "gdi32.dll";
        public const int GWL_EXSTYLE = -20;
        public const int GWL_STYLE = -16;
        public static readonly IntPtr HBMMENU_CALLBACK = ((IntPtr) (-1));
        public static readonly IntPtr HKL_NEXT = ((IntPtr) 1);
        public static readonly IntPtr HKL_PREV = IntPtr.Zero;
        public static readonly IntPtr HWND_BOTTOM = ((IntPtr) 1);
        public static readonly IntPtr HWND_NOTOPMOST = ((IntPtr) (-2));
        public static readonly IntPtr HWND_TOP = IntPtr.Zero;
        public static readonly IntPtr HWND_TOPMOST = ((IntPtr) (-1));
        public const int INVALID_FILE_ATTRIBUTES = -1;
        public const uint INVALID_FILE_SIZE = uint.MaxValue;
        public static readonly IntPtr INVALID_HANDLE_VALUE = ((IntPtr) (-1));
        public const string Kernel32Dll = "Kernel32.dll";
        public const int MAX_PATH = 260;
        public const int RECOVERY_DEFAULT_PING_INTERVAL = 0x1388;
        public const string User32Dll = "user32.dll";

        [DllImport("user32.dll", SetLastError=true)]
        public static extern IntPtr ActivateKeyboardLayout(IntPtr hkl, KLF Flags);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("gdi32.dll", EntryPoint="GdiAlphaBlend", SetLastError=true)]
        public static extern bool AlphaBlend(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, BLENDFUNCTION blendFunction);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool AnimateWindow(IntPtr hwnd, uint dwTime, AW dwFlags);
        [DllImport("Kernel32.dll")]
        internal static extern void ApplicationRecoveryFinished([MarshalAs(UnmanagedType.Bool)] bool bSuccess);
        [DllImport("Kernel32.dll")]
        internal static extern int ApplicationRecoveryInProgress([MarshalAs(UnmanagedType.Bool)] out bool pbCanceled);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool BackupRead(SafeFileHandle hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, [MarshalAs(UnmanagedType.Bool)] bool bAbort, [MarshalAs(UnmanagedType.Bool)] bool bProcessSecurity, ref IntPtr lpContext);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool BackupRead(SafeFileHandle hFile, [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, [MarshalAs(UnmanagedType.Bool)] bool bAbort, [MarshalAs(UnmanagedType.Bool)] bool bProcessSecurity, ref IntPtr lpContext);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool BackupSeek(SafeFileHandle hFile, uint dwLowBytesToSeek, uint dwHighBytesToSeek, out uint lpdwLowByteSeeked, out uint lpdwHighByteSeeked, ref IntPtr lpContext);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool BackupWrite(SafeFileHandle hFile, IntPtr lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, [MarshalAs(UnmanagedType.Bool)] bool bAbort, [MarshalAs(UnmanagedType.Bool)] bool bProcessSecurity, ref IntPtr lpContext);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool BackupWrite(SafeFileHandle hFile, [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, [MarshalAs(UnmanagedType.Bool)] bool bAbort, [MarshalAs(UnmanagedType.Bool)] bool bProcessSecurity, ref IntPtr lpContext);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern IntPtr BeginPaint(IntPtr hwnd, ref PAINTSTRUCT lpPaint);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ChildWindowFromPoint(IntPtr hWndParent, Point Point);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool CloseClipboard();
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool ConnectNamedPipe(SafePipeHandle hNamedPipe, IntPtr lpOverlapped);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool CopyFileEx([MarshalAs(UnmanagedType.LPTStr)] string lpExistingFileName, [MarshalAs(UnmanagedType.LPTStr)] string lpNewFileName, [MarshalAs(UnmanagedType.FunctionPtr)] CopyProgressRoutine lpProgressRoutine, IntPtr lpData, [In, MarshalAs(UnmanagedType.Bool)] ref bool pbCancel, COPY_FILE dwCopyFlags);
        [DllImport("gdi32.dll", SetLastError=true)]
        public static extern IntPtr CreateBrushIndirect([In] ref LOGBRUSH lplb);
        [DllImport("gdi32.dll", SetLastError=true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern SafeFileHandle CreateFile([MarshalAs(UnmanagedType.LPTStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] FileAccess dwDesiredAccess, [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode, IntPtr lpSecurityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition, [MarshalAs(UnmanagedType.U4)] FileOptions dwFlagsAndAttributes, IntPtr hTemplateFile);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool CreateHardLink([MarshalAs(UnmanagedType.LPTStr)] string lpFileName, [MarshalAs(UnmanagedType.LPTStr)] string lpExistingFileName, IntPtr lpSecurityAttributes);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern IntPtr CreateIconFromResourceEx(IntPtr pbIconBits, uint cbIconBits, [MarshalAs(UnmanagedType.Bool)] bool fIcon, uint dwVersion, int cxDesired, int cyDesired, LR uFlags);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern SafePipeHandle CreateNamedPipe([MarshalAs(UnmanagedType.LPTStr)] string lpName, uint dwOpenMode, uint dwPipeMode, uint nMaxInstances, uint nOutBufferSize, uint nInBufferSize, uint nDefaultTimeOut, IntPtr pipeSecurityDescriptor);
        [DllImport("user32.dll")]
        public static extern IntPtr CreatePopupMenu();
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(uint crColor);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool CreateSymbolicLink([MarshalAs(UnmanagedType.LPTStr)] string lpSymlinkFileName, [MarshalAs(UnmanagedType.LPTStr)] string lpTargetFileName, SYMBOLIC_LINK dwFlags);
        [DllImport("gdi32.dll", SetLastError=true)]
        public static extern bool DeleteDC(IntPtr hdc);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool DeleteFile([MarshalAs(UnmanagedType.LPTStr)] string lpFileName);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr hIcon);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool DestroyMenu(IntPtr hMenu);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool DisconnectNamedPipe(SafePipeHandle hNamedPipe);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr DispatchMessage([In] ref Microsoft.Win32.MSG lpmsg);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool EnableWindow(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bEnable);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, [In] ref PAINTSTRUCT lpPaint);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern int EnumClipboardFormats(int format);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool EnumResourceNames(Microsoft.Win32.SafeHandles.SafeLibraryHandle hModule, IntPtr lpszType, EnumResNameDelegate lpEnumFunc, IntPtr lParam);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool EnumResourceNames(IntPtr hModule, [MarshalAs(UnmanagedType.LPTStr)] string lpszType, EnumResNameDelegate lpEnumFunc, IntPtr lParam);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern Microsoft.Win32.SafeHandles.SafeFindHandle FindFirstFile(string lpFileName, out Microsoft.Win32.WIN32_FIND_DATA lpFindFileData);
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern Microsoft.Win32.SafeHandles.SafeFindHandle FindFirstStreamW([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, STREAM_INFO_LEVELS InfoLevel, out WIN32_FIND_STREAM_DATA lpFindStreamData, uint dwFlags);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern SafeFindVolumeHandle FindFirstVolume([Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszVolumeName, int cchBufferLength);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern SafeFindVolumeMountPointHandle FindFirstVolumeMountPoint([MarshalAs(UnmanagedType.LPTStr)] string lpszRootPathName, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszVolumeMountPoint, int cchBufferLength);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool FindNextFile(Microsoft.Win32.SafeHandles.SafeFindHandle hFindFile, out Microsoft.Win32.WIN32_FIND_DATA lpFindFileData);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool FindNextStreamW(Microsoft.Win32.SafeHandles.SafeFindHandle hFindStream, out WIN32_FIND_STREAM_DATA lpFindStreamData);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool FindNextVolume(SafeFindVolumeHandle hFindVolume, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszVolumeName, int cchBufferLength);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool FindNextVolumeMountPoint(SafeFindVolumeMountPointHandle hFindVolumeMountPoint, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszVolumeMountPoint, int cchBufferLength);
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern IntPtr FindResource(Microsoft.Win32.SafeHandles.SafeLibraryHandle hModule, IntPtr lpName, IntPtr lpType);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern IntPtr FindResource(Microsoft.Win32.SafeHandles.SafeLibraryHandle hModule, [MarshalAs(UnmanagedType.LPTStr)] string lpName, IntPtr lpType);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern IntPtr FindResource(Microsoft.Win32.SafeHandles.SafeLibraryHandle hModule, [MarshalAs(UnmanagedType.LPTStr)] string lpName, [MarshalAs(UnmanagedType.LPTStr)] string lpType);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool FlushFileBuffers(SafeFileHandle handle);
        [DllImport("Kernel32.dll")]
        public static extern int GetACP();
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();
        [DllImport("Kernel32.dll")]
        internal static extern int GetApplicationRecoveryCallback(IntPtr hProcess, out ApplicationRecoveryCallback pRecoveryCallback, out IntPtr ppvParameter, out uint dwPingInterval, out uint dwFlags);
        [DllImport("Kernel32.dll")]
        internal static extern int GetApplicationRestartSettings(IntPtr hProcess, [Out, MarshalAs(UnmanagedType.BStr)] StringBuilder pwzCommandLine, ref uint pcchSize, out RESTART pdwFlags);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern int GetClassName(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpClassName, int nMaxCount);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool GetClientRect(IntPtr hWnd, out Microsoft.Win32.RECT lpRect);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern uint GetCompressedFileSize([MarshalAs(UnmanagedType.LPTStr)] string lpFileName, out uint lpFileSizeHigh);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Point lpPoint);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool GetDiskFreeSpace([MarshalAs(UnmanagedType.LPTStr)] string lpRootPathName, out uint lpSectorsPerCluster, out uint lpBytesPerSector, out uint lpNumberOfFreeClusters, out uint lpTotalNumberOfClusters);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto)]
        public static extern DriveType GetDriveType([MarshalAs(UnmanagedType.LPTStr)] string lpRootPathName);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern FileAttributes GetFileAttributes([MarshalAs(UnmanagedType.LPTStr)] string lpFileName);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool GetFileAttributesEx([MarshalAs(UnmanagedType.LPTStr)] string lpFileName, GET_FILEEX_INFO_LEVELS fInfoLevelId, out Microsoft.Win32.WIN32_FILE_ATTRIBUTE_DATA lpFileInformation);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool GetFileInformationByHandle(SafeFileHandle hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);
        public static bool GetFileInformationByHandleEx(SafeFileHandle hFile, out FILE_BASIC_INFO lpFileInformation)
        {
            return GetFileInformationByHandleEx(hFile, FILE_INFO_BY_HANDLE_CLASS.FileBasicInfo, out lpFileInformation, Marshal.SizeOf(typeof(FILE_BASIC_INFO)));
        }

        public static bool GetFileInformationByHandleEx(SafeFileHandle hFile, out FILE_STANDARD_INFO lpFileInformation)
        {
            return GetFileInformationByHandleEx(hFile, FILE_INFO_BY_HANDLE_CLASS.FileStandardInfo, out lpFileInformation, Marshal.SizeOf(typeof(FILE_STANDARD_INFO)));
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll")]
        private static extern bool GetFileInformationByHandleEx(SafeFileHandle hFile, FILE_INFO_BY_HANDLE_CLASS FileInformationClass, out FILE_BASIC_INFO lpFileInformation, int dwBufferSize);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll")]
        public static extern bool GetFileInformationByHandleEx(SafeFileHandle hFile, FILE_INFO_BY_HANDLE_CLASS FileInformationClass, IntPtr lpFileInformation, uint dwBufferSize);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll")]
        private static extern bool GetFileInformationByHandleEx(SafeFileHandle hFile, FILE_INFO_BY_HANDLE_CLASS FileInformationClass, out FILE_STANDARD_INFO lpFileInformation, int dwBufferSize);
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern uint GetFileSize(SafeFileHandle hFile, out uint lpFileSizeHigh);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool GetFileSizeEx(SafeFileHandle hFile, out long lpFileSizeHigh);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool GetFileTime(SafeFileHandle hFile, out System.Runtime.InteropServices.ComTypes.FILETIME lpCreationTime, out System.Runtime.InteropServices.ComTypes.FILETIME lpLastAccessTime, out System.Runtime.InteropServices.ComTypes.FILETIME lpLastWriteTime);
        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern uint GetLogicalDrives();
        [DllImport("user32.dll", SetLastError=true)]
        public static extern int GetMenuItemCount(IntPtr hMenu);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool GetMenuItemInfo(IntPtr hMenu, uint uItem, [MarshalAs(UnmanagedType.Bool)] bool fByPosition, ref MENUITEMINFO lpmii);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool GetMessage(ref Microsoft.Win32.MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
        [DllImport("user32.dll")]
        public static extern int GetMessagePos();
        [DllImport("gdi32.dll", EntryPoint="GetObject", SetLastError=true)]
        public static extern int GetObjectBitmap(IntPtr hObject, int nCount, ref Microsoft.Win32.BITMAP lpObject);
        [DllImport("gdi32.dll", EntryPoint="GetObject", SetLastError=true)]
        public static extern int GetObjectDIBSection(IntPtr hObject, int nCount, ref DIBSECTION lpObject);
        [DllImport("Kernel32.dll")]
        public static extern int GetOEMCP();
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool GetOverlappedResult(SafeFileHandle hFile, IntPtr lpOverlapped, out uint lpNumberOfBytesTransferred, [MarshalAs(UnmanagedType.Bool)] bool bWait);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("Kernel32.dll", CharSet=CharSet.Ansi, SetLastError=true)]
        public static extern IntPtr GetProcAddress(Microsoft.Win32.SafeHandles.SafeLibraryHandle hModule, [MarshalAs(UnmanagedType.LPStr)] string procName);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool GetScrollInfo(IntPtr hwnd, SB fnBar, ref SCROLLINFO lpsi);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern int GetScrollPos(IntPtr hWnd, SB nBar);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern int GetTempPath(int nBufferLength, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpBuffer);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool GetUpdateRect(IntPtr hWnd, out Microsoft.Win32.RECT lpRect, [MarshalAs(UnmanagedType.Bool)] bool bErase);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern RegionResult GetUpdateRgn(IntPtr hWnd, IntPtr hRgn, [MarshalAs(UnmanagedType.Bool)] bool bErase);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool GetVolumeInformation([MarshalAs(UnmanagedType.LPTStr)] string lpRootPathName, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpVolumeNameBuffer, int nVolumeNameSize, out uint lpVolumeSerialNumber, out int lpMaximumComponentLength, out FILE_SYSTEM_FLAGS lpFileSystemFlags, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpFileSystemNameBuffer, int nFileSystemNameSize);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool GetVolumeNameForVolumeMountPoint([MarshalAs(UnmanagedType.LPTStr)] string lpszVolumeMountPoint, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszVolumeName, int cchBufferLength);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool GetVolumePathName([MarshalAs(UnmanagedType.LPTStr)] string lpszFileName, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszVolumePathName, int cchBufferLength);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool GetVolumePathNamesForVolumeName([MarshalAs(UnmanagedType.LPTStr)] string lpszVolumeName, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszVolumePathNames, int cchBufferLength, out int lpcchReturnLength);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool GetWindowRect(IntPtr hWnd, out Microsoft.Win32.RECT lpRect);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern int GetWindowsDirectory([Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpBuffer, int uSize);
        [DllImport("Kernel32.dll")]
        public static extern IntPtr GlobalAlloc(GMEM uFlags, int dwBytes);
        [DllImport("Kernel32.dll")]
        public static extern IntPtr GlobalLock(IntPtr hMem);
        [DllImport("Kernel32.dll")]
        public static extern IntPtr GlobalSize(IntPtr hMem);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll")]
        public static extern bool GlobalUnlock(IntPtr hMem);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, KEYEVENTF dwFlags, IntPtr dwExtraInfo);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool KillTimer(IntPtr hWnd, IntPtr uIDEvent);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, PredefinedCursor lpCursorName);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, [MarshalAs(UnmanagedType.LPTStr)] string lpCursorName);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr LoadIcon(IntPtr hInstance, PredefinedIcon lpIconName);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr LoadIcon(IntPtr hInstance, [MarshalAs(UnmanagedType.LPTStr)] string lpIconName);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern IntPtr LoadImage(Microsoft.Win32.SafeHandles.SafeLibraryHandle hInst, IntPtr lpszName, IMAGE uType, int cxDesired, int cyDesired, LR fuLoad);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern IntPtr LoadImage(Microsoft.Win32.SafeHandles.SafeLibraryHandle hInst, [MarshalAs(UnmanagedType.LPTStr)] string lpszName, IMAGE uType, int cxDesired, int cyDesired, LR fuLoad);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern IntPtr LoadImage(IntPtr hInst, IntPtr lpszName, IMAGE uType, int cxDesired, int cyDesired, LR fuLoad);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern IntPtr LoadImage(IntPtr hInst, [MarshalAs(UnmanagedType.LPTStr)] string lpszName, IMAGE uType, int cxDesired, int cyDesired, LR fuLoad);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern Microsoft.Win32.SafeHandles.SafeLibraryHandle LoadLibrary([MarshalAs(UnmanagedType.LPTStr)] string lpFileName);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern Microsoft.Win32.SafeHandles.SafeLibraryHandle LoadLibraryEx([MarshalAs(UnmanagedType.LPTStr)] string lpFileName, IntPtr hFile, LOAD_LIBRARY dwFlags);
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern IntPtr LoadResource(Microsoft.Win32.SafeHandles.SafeLibraryHandle hModule, IntPtr hResInfo);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern int LoadString(Microsoft.Win32.SafeHandles.SafeLibraryHandle hInstance, uint uID, out IntPtr lpBuffer, int nBufferMax);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern int LoadString(Microsoft.Win32.SafeHandles.SafeLibraryHandle hInstance, uint uID, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpBuffer, int nBufferMax);
        [DllImport("Kernel32.dll")]
        public static extern IntPtr LocalAlloc(LMEM uFlags, int dwBytes);
        [DllImport("Kernel32.dll")]
        public static extern IntPtr LocalFree(IntPtr hMem);
        [DllImport("Kernel32.dll")]
        public static extern IntPtr LockResource(IntPtr hResData);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern int LookupIconIdFromDirectoryEx(IntPtr presbits, [MarshalAs(UnmanagedType.Bool)] bool fIcon, int cxDesired, int cyDesired, LR Flags);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool MoveFile([MarshalAs(UnmanagedType.LPTStr)] string lpExistingFileName, [MarshalAs(UnmanagedType.LPTStr)] string lpNewFileName);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool PeekNamedPipe(SafePipeHandle hNamedPipe, [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, uint nBufferSize, out uint lpBytesRead, out uint lpTotalBytesAvail, out uint lpBytesLeftThisMessage);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern int QueryDosDevice([MarshalAs(UnmanagedType.LPTStr)] string lpDeviceName, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpTargetPath, int ucchMax);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool ReadDirectoryChangesW(SafeFileHandle hDirectory, IntPtr lpBuffer, uint nBufferLength, [MarshalAs(UnmanagedType.Bool)] bool bWatchSubtree, FILE_NOTIFY dwNotifyFilter, out uint lpBytesReturned, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool ReadFile(SafeFileHandle hFile, [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverLapped);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool ReadFile(SafeFileHandle hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverLapped);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool RedrawWindow(IntPtr hWnd, ref Rectangle lprcUpdate, IntPtr hrgnUpdate, RDW flags);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RDW flags);
        [DllImport("Kernel32.dll")]
        internal static extern int RegisterApplicationRecoveryCallback(ApplicationRecoveryCallback pRecoveryCallback, IntPtr pvParameter, uint dwPingInterval, uint dwFlags);
        [DllImport("Kernel32.dll")]
        internal static extern int RegisterApplicationRestart([MarshalAs(UnmanagedType.BStr)] string pwzCommandLine, RESTART dwFlags);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, MOD fsModifiers, uint vk);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern int RegisterWindowMessage([MarshalAs(UnmanagedType.LPTStr)] string lpString);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("gdi32.dll", SetLastError=true)]
        public static extern uint SetBkColor(IntPtr hdc, int crColor);
        [DllImport("gdi32.dll", SetLastError=true)]
        public static extern int SetBkMode(IntPtr hdc, BkMode iBkMode);
        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr hCursor);
        [DllImport("Kernel32.dll")]
        public static extern SEM SetErrorMode(SEM uMode);
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern uint SetFilePointer(SafeFileHandle hFile, int lDistanceToMove, [In] ref int lpDistanceToMoveHigh, uint dwMoveMethod);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool SetFileTime(SafeFileHandle hFile, [In] ref System.Runtime.InteropServices.ComTypes.FILETIME lpCreationTime, [In] ref System.Runtime.InteropServices.ComTypes.FILETIME lpLastAccessTime, [In] ref System.Runtime.InteropServices.ComTypes.FILETIME lpLastWriteTime);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool SetFileTime(SafeFileHandle hFile, IntPtr lpCreationTime, IntPtr lpLastAccessTime, IntPtr lpLastWriteTime);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hwnd);
        [DllImport("Kernel32.dll")]
        public static extern void SetLastError(uint dwErrCode);
        [DllImport("gdi32.dll", SetLastError=true)]
        public static extern uint SetTextColor(IntPtr hdc, int crColor);
        [DllImport("Kernel32.dll")]
        public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern IntPtr SetTimer(IntPtr hWnd, IntPtr nIDEvent, int uElapse, IntPtr lpTimerFunc);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SWP uFlags);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool SetWindowText(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr)] string lpString);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, SW nCmdShow);
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern uint SizeofResource(Microsoft.Win32.SafeHandles.SafeLibraryHandle hModule, IntPtr hResInfo);
        [DllImport("user32.dll")]
        public static extern uint TrackPopupMenuEx(IntPtr hMenu, TPM flags, int x, int y, IntPtr hwnd, IntPtr lptpm);
        [DllImport("Kernel32.dll")]
        internal static extern int UnregisterApplicationRecoveryCallback();
        [DllImport("Kernel32.dll")]
        internal static extern int UnregisterApplicationRestart();
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool ValidateRect(IntPtr hWnd, [In] ref Microsoft.Win32.RECT lpRect);
        [DllImport("user32.dll", CharSet=CharSet.Unicode)]
        public static extern ushort VkKeyScanExW(char ch, IntPtr dwhkl);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool WaitNamedPipe([MarshalAs(UnmanagedType.LPTStr)] string lpNamedPipeName, int nTimeOut);
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point Point);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool WriteFile(SafeFileHandle hFile, IntPtr lpBuffer, uint nNumberOfBytesToWrite, ref uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
    }
}

