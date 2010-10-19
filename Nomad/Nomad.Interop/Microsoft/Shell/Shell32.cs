namespace Microsoft.Shell
{
    using Microsoft.Win32;
    using Microsoft.Win32.Network;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class Shell32
    {
        public const string Shell32Dll = "shell32.dll";

        [DllImport("shell32.dll")]
        public static extern int DllGetVersion(ref DLLVERSIONINFO pdvi);
        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern uint ExtractIconEx([MarshalAs(UnmanagedType.LPTStr)] string szFileName, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);
        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern uint ExtractIconEx([MarshalAs(UnmanagedType.LPTStr)] string szFileName, int nIconIndex, out IntPtr phiconLarge, out IntPtr phiconSmall, uint nIcons);
        public static string GetClsidFolderParseName(Guid clsid)
        {
            return ("::" + clsid.ToString("B"));
        }

        [DllImport("shell32.dll")]
        public static extern int GetCurrentProcessExplicitAppUserModelID(IntPtr AppID);
        [DllImport("shell32.dll")]
        public static extern int PickIconDlg(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, ref int piIconIndex);
        [DllImport("shell32.dll")]
        public static extern int SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);
        [DllImport("shell32.dll")]
        public static extern void SHAddToRecentDocs(SHARD uFlags, [In] ref SHARDAPPIDINFO pv);
        [DllImport("shell32.dll")]
        public static extern void SHAddToRecentDocs(SHARD uFlags, [In] ref SHARDAPPIDINFOIDLIST pv);
        [DllImport("shell32.dll")]
        public static extern void SHAddToRecentDocs(SHARD uFlags, [In] ref SHARDAPPIDINFOLINK pv);
        [DllImport("shell32.dll")]
        public static extern void SHAddToRecentDocs(SHARD uFlags, IShellLinkW pv);
        [DllImport("shell32.dll")]
        public static extern void SHAddToRecentDocs(SHARD uFlags, IntPtr pv);
        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern void SHAddToRecentDocs(SHARD uFlags, [MarshalAs(UnmanagedType.LPTStr)] string pv);
        [DllImport("shell32.dll")]
        public static extern int SHBindToFolderIDListParent(IShellFolder psfRoot, IntPtr pidl, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, out object ppv, out IntPtr ppidlLast);
        [DllImport("shell32.dll")]
        public static extern int SHCreateItemFromIDList(IntPtr pidl, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
        [DllImport("shell32.dll")]
        public static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IntPtr pbc, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
        [DllImport("shell32.dll")]
        public static extern int SHCreateShellItem(IntPtr pidlParent, IShellFolder psfParent, IntPtr pidl, [MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern bool ShellExecuteEx([In] ref SHELLEXECUTEINFO lpExecInfo);
        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern int SHFileOperation([In] ref SHFILEOPSTRUCT lpFileOp);
        public static int SHGetDataFromIDList(IShellFolder psf, IntPtr pidl, ref NETRESOURCE pv)
        {
            return SHGetDataFromIDList(psf, pidl, SHGetDataFromIDListFormat.SHGDFIL_NETRESOURCE, ref pv, Marshal.SizeOf((NETRESOURCE) pv));
        }

        public static int SHGetDataFromIDList(IShellFolder psf, IntPtr pidl, ref Microsoft.Win32.WIN32_FIND_DATA pv)
        {
            return SHGetDataFromIDList(psf, pidl, SHGetDataFromIDListFormat.SHGDFIL_FINDDATA, ref pv, Marshal.SizeOf((Microsoft.Win32.WIN32_FIND_DATA) pv));
        }

        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern int SHGetDataFromIDList(IShellFolder psf, IntPtr pidl, SHGetDataFromIDListFormat nFormat, ref NETRESOURCE pv, int cb);
        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern int SHGetDataFromIDList(IShellFolder psf, IntPtr pidl, SHGetDataFromIDListFormat nFormat, ref Microsoft.Win32.WIN32_FIND_DATA pv, int cb);
        [DllImport("shell32.dll")]
        public static extern int SHGetDesktopFolder([MarshalAs(UnmanagedType.Interface)] out IShellFolder ppshf);
        public static IntPtr SHGetFileInfo(IntPtr pszPath, FileAttributes dwFileAttributes, ref SHFILEINFO psfi, SHGFI Flags)
        {
            return SHGetFileInfo(pszPath, dwFileAttributes, ref psfi, (uint) Marshal.SizeOf(((SHFILEINFO) psfi).GetType()), (uint) Flags);
        }

        public static IntPtr SHGetFileInfo(string pszPath, FileAttributes dwFileAttributes, ref SHFILEINFO psfi, SHGFI Flags)
        {
            return SHGetFileInfo(pszPath, dwFileAttributes, ref psfi, (uint) Marshal.SizeOf(((SHFILEINFO) psfi).GetType()), (uint) Flags);
        }

        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(IntPtr pszPath, FileAttributes dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);
        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, FileAttributes dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);
        [DllImport("shell32.dll")]
        public static extern int SHGetMalloc([MarshalAs(UnmanagedType.Interface)] out IMalloc ppMalloc);
        [DllImport("shell32.dll")]
        public static extern int SHGetRealIDL(IShellFolder psf, IntPtr pidlSimple, out IntPtr pidlReal);
        [DllImport("shell32.dll")]
        public static extern int SHGetSpecialFolderLocation(IntPtr hwndOwner, CSIDL nFolder, out IntPtr ppidl);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszPath, CSIDL nFolder, [MarshalAs(UnmanagedType.Bool)] bool fCreate);
        [DllImport("shell32.dll")]
        public static extern int SHGetStockIconInfo(SHSTOCKICONID siid, SHGSI uFlags, ref SHSTOCKICONINFO psii);
        [DllImport("shell32.dll")]
        public static extern int SHParseDisplayName([MarshalAs(UnmanagedType.LPWStr)] string pszName, IntPtr pbc, out IntPtr ppidl, SFGAO sfgaoIn, out SFGAO psfgaoOut);
        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern int SHQueryRecycleBin([MarshalAs(UnmanagedType.LPTStr)] string pszRootPath, ref SHQUERYRBINFO pSHQueryRBInfo);
        [DllImport("shlwapi.dll")]
        public static extern int StrRetToBSTR(ref STRRET pstr, IntPtr pidl, [MarshalAs(UnmanagedType.BStr)] out string pbstr);

        public static Version ShellDllVersion
        {
            get
            {
                DLLVERSIONINFO dllversioninfo;
                dllversioninfo = new DLLVERSIONINFO {
                    cbSize = (uint) Marshal.SizeOf(dllversioninfo)
                };
                DllGetVersion(ref dllversioninfo);
                return dllversioninfo.Version;
            }
        }
    }
}

