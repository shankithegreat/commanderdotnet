namespace Nomad.FileSystem.Virtual
{
    using Microsoft;
    using Microsoft.IO;
    using Microsoft.Shell;
    using Microsoft.Win32;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Archive;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Ftp;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Network;
    using Nomad.FileSystem.Null;
    using Nomad.FileSystem.Shell;
    using Nomad.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading;

    public static class VirtualItem
    {
        public static readonly IDictionary<string, string> DefaultDrivePath = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public const uint SL_Base = 0xefff0000;
        private const uint SL_VirtualItem = 0xefff0000;
        public static EventHandler<VirtualItemChangedEventArgs> VirtualItemChanged;

        public static IVirtualItem CreateVirtualShellLink(IVirtualFolder destFolder, string name, IVirtualItem target)
        {
            if (destFolder == null)
            {
                throw new ArgumentNullException("destFolder");
            }
            CustomFileSystemFolder folder = destFolder as CustomFileSystemFolder;
            if (folder == null)
            {
                throw new ArgumentException("destFolder is not CustomFileSystemFolder");
            }
            using (ShellLink link = new ShellLink())
            {
                if (target is FileSystemItem)
                {
                    link.Path = target.FullName;
                }
                byte[] data = Serialize(target);
                link.AddBlock(0xefff0000, data, 0, data.Length);
                IChangeVirtualFile file = folder.CreateFile(name);
                using (Stream stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None, FileOptions.SequentialScan, 0L))
                {
                    link.Save(stream);
                }
                return (IChangeVirtualFile) FromFullName(file.FullName, VirtualItemType.File);
            }
        }

        public static T Deserialize<T>(byte[] value) where T: IVirtualItem
        {
            using (MemoryStream stream = new MemoryStream(value))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T) formatter.Deserialize(stream);
            }
        }

        public static bool Equals(IVirtualItem x, IVirtualItem y)
        {
            if ((x == null) ^ (y == null))
            {
                return false;
            }
            return ((x == null) || x.Equals(y));
        }

        public static IVirtualItem FromFullName(string fullName, VirtualItemType type)
        {
            return FromFullName(fullName, type, null, null);
        }

        public static IVirtualItem FromFullName(string fullName, VirtualItemType type, IVirtualFolder current)
        {
            return FromFullName(fullName, type, current, null);
        }

        public static IVirtualItem FromFullName(string fullName, VirtualItemType type, IVirtualFolder current, IVirtualFolder parent)
        {
            PathType pathType = PathHelper.GetPathType(fullName);
            if ((pathType == ~PathType.Unknown) && fullName.StartsWith("shell:", StringComparison.OrdinalIgnoreCase))
            {
                if (type == VirtualItemType.File)
                {
                    throw new ArgumentException("Cannot resolve known shell folder to file.");
                }
                return FromKnownShellFolder(fullName.Substring(6));
            }
            if (pathType == ~PathType.Unknown)
            {
                throw new ArgumentException(Resources.sErrorInvalidPath);
            }
            if ((type == VirtualItemType.File) && ((pathType & PathType.Folder) > PathType.Unknown))
            {
                throw new ArgumentException("Cannot return file while fullName represents folder.");
            }
            if ((type == VirtualItemType.Unknown) && ((pathType & PathType.Folder) > PathType.Unknown))
            {
                type = VirtualItemType.Folder;
            }
            if ((pathType & PathType.Relative) > PathType.Unknown)
            {
                if (!(current is CustomFileSystemFolder))
                {
                    throw new ArgumentException(Resources.sErrorRelativeFileName);
                }
                System.IO.Directory.SetCurrentDirectory(current.FullName);
                fullName = Path.GetFullPath(fullName);
                pathType = PathHelper.GetPathType(fullName);
            }
            if ((pathType & PathType.Uri) > PathType.Unknown)
            {
                Uri absoluteUri = new Uri(fullName);
                if (!string.IsNullOrEmpty(absoluteUri.Query))
                {
                    throw new ArgumentException(Resources.sErrorQueryInPathUri);
                }
                if (!string.IsNullOrEmpty(absoluteUri.Fragment))
                {
                    IVirtualItem item;
                    if (absoluteUri.Scheme == Uri.UriSchemeFtp)
                    {
                        item = FtpFileSystemCreator.FromUri(new Uri(absoluteUri.GetComponents(UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped)), VirtualItemType.File, null);
                    }
                    else
                    {
                        if (absoluteUri.Scheme != Uri.UriSchemeFile)
                        {
                            throw new ArgumentException(Resources.sErrorUnsupportedUriScheme);
                        }
                        item = LocalFileSystemCreator.FromFullName(absoluteUri.LocalPath, VirtualItemType.File, null);
                    }
                    IPersistVirtualItem item2 = item as IPersistVirtualItem;
                    if (!((item2 == null) || item2.Exists))
                    {
                        throw new FileNotFoundException(string.Format(Resources.sErrorArchiveNotFound, item.FullName), item.FullName);
                    }
                    IVirtualFolder folder = OpenArchive((IChangeVirtualFile) item, parent);
                    if (folder == null)
                    {
                        throw new WarningException(string.Format(Resources.sErrorUnknownArchiveFormat, item.FullName));
                    }
                    ArchiveFolder folder2 = folder as ArchiveFolder;
                    if (folder2 != null)
                    {
                        return folder2.FromName(absoluteUri.GetComponents(UriComponents.Fragment, UriFormat.Unescaped));
                    }
                    return folder;
                }
                if (absoluteUri.Scheme == Uri.UriSchemeFtp)
                {
                    return FtpFileSystemCreator.FromUri(absoluteUri, type, parent);
                }
                if (absoluteUri.Scheme == NetworkFileSystemCreator.UriScheme)
                {
                    return NetworkFileSystemCreator.FromFullName(fullName, parent);
                }
                if (absoluteUri.Scheme == NullFileSystemCreator.UriScheme)
                {
                    return NullFileSystemCreator.FromUri(absoluteUri, type);
                }
                if (absoluteUri.Scheme == ShellFileSystemCreator.UriScheme)
                {
                    return ShellFileSystemCreator.FromUri(absoluteUri, type);
                }
                if (absoluteUri.Scheme != Uri.UriSchemeFile)
                {
                    throw new ArgumentException(Resources.sErrorUnsupportedUriScheme);
                }
                fullName = absoluteUri.LocalPath;
                pathType &= ~PathType.Uri;
            }
            if (type == VirtualItemType.File)
            {
                switch (pathType)
                {
                    case PathType.Volume:
                    case PathType.NetworkServer:
                    case PathType.NetworkShare:
                        throw new ArgumentException("Cannot return file while fullName represents folder.");
                }
            }
            PathType type3 = pathType;
            if (type3 != PathType.Volume)
            {
                if (type3 == PathType.NetworkServer)
                {
                    return NetworkFileSystemCreator.FromFullName(fullName, parent);
                }
                if (type3 == PathType.NetworkShare)
                {
                    return new NetworkShareFolder(fullName, parent);
                }
            }
            else
            {
                return FileSystemDrive.Create(fullName);
            }
            return LocalFileSystemCreator.FromFullName(fullName, type, parent);
        }

        public static IVirtualFolder FromKnownShellFolder(CSIDL folderId)
        {
            IntPtr ptr;
            StringBuilder lpszPath = new StringBuilder(260);
            if (Shell32.SHGetSpecialFolderPath(IntPtr.Zero, lpszPath, folderId, false))
            {
                return (IVirtualFolder) LocalFileSystemCreator.FromFullName(lpszPath.ToString(), VirtualItemType.Folder, null);
            }
            if (HRESULT.SUCCEEDED(Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, folderId, out ptr)))
            {
                try
                {
                    return new ShellFolder(new SafeShellItem(ptr));
                }
                finally
                {
                    Marshal.FreeCoTaskMem(ptr);
                }
            }
            throw new ArgumentException(string.Format("Unable to resolve CSIDL ({0}).", folderId));
        }

        private static IVirtualFolder FromKnownShellFolder(IKnownFolder knownFolder)
        {
            IntPtr ptr2;
            IVirtualFolder folder;
            if (knownFolder.GetCategory() == KF_CATEGORY.KF_CATEGORY_VIRTUAL)
            {
                IntPtr ptr;
                knownFolder.GetIDList(KF_FLAG.KF_FLAG_DONT_VERIFY, out ptr);
                try
                {
                    return new ShellFolder(new SafeShellItem(ptr));
                }
                catch
                {
                    Marshal.FreeCoTaskMem(ptr);
                    throw;
                }
            }
            knownFolder.GetPath(KF_FLAG.KF_FLAG_DONT_VERIFY, out ptr2);
            try
            {
                folder = (IVirtualFolder) LocalFileSystemCreator.FromFullName(Marshal.PtrToStringUni(ptr2), VirtualItemType.Folder, null);
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr2);
            }
            return folder;
        }

        public static IVirtualFolder FromKnownShellFolder(Guid folderId)
        {
            IVirtualFolder folder2;
            if (!OS.IsWinVista)
            {
                throw new PlatformNotSupportedException();
            }
            IKnownFolderManager o = (IKnownFolderManager) new CoKnownFolderManager();
            try
            {
                IKnownFolder folder;
                o.GetFolder(folderId, out folder);
                try
                {
                    folder2 = FromKnownShellFolder(folder);
                }
                finally
                {
                    Marshal.FinalReleaseComObject(folder);
                }
            }
            catch (FileNotFoundException exception)
            {
                throw new ArgumentException(string.Format("'{0}' is not valid known shell folder guid.", folderId), exception);
            }
            finally
            {
                Marshal.FinalReleaseComObject(o);
            }
            return folder2;
        }

        public static IVirtualFolder FromKnownShellFolder(string knownName)
        {
            if (OS.IsWinVista)
            {
                IKnownFolderManager o = (IKnownFolderManager) new CoKnownFolderManager();
                try
                {
                    IKnownFolder folder;
                    o.GetFolderByName(knownName, out folder);
                    try
                    {
                        return FromKnownShellFolder(folder);
                    }
                    finally
                    {
                        Marshal.FinalReleaseComObject(folder);
                    }
                }
                catch (FileNotFoundException exception)
                {
                    throw new ArgumentException(string.Format("'{0}' is not valid known shell folder.", knownName), exception);
                }
                finally
                {
                    Marshal.FinalReleaseComObject(o);
                }
            }
            return FromKnownShellFolder(KnownFolder.FolderNameToCSIDL(knownName));
        }

        public static DesktopIni GetDesktopIni(IVirtualFolder folder, bool mustExists)
        {
            if (((folder != null) && !(folder is VirtualSearchFolder)) && !(folder is ArchiveFolder))
            {
                if (folder is CustomFileSystemFolder)
                {
                    string path = Path.Combine(folder.FullName, "desktop.ini");
                    if (System.IO.File.Exists(path))
                    {
                        return new DesktopIni(path);
                    }
                    if (!((mustExists || DesktopIniCache.Contains(folder.FullName)) || Settings.Default.ForceDesktopIniCache))
                    {
                        return new FallbackDesktopIni(path);
                    }
                }
                if (!(mustExists && !DesktopIniCache.Contains(folder.FullName)))
                {
                    return DesktopIniCache.Get(folder.FullName);
                }
            }
            return null;
        }

        public static DesktopIni GetInheritedDesktopIni(IVirtualFolder folder)
        {
            if (((folder != null) && !(folder is VirtualSearchFolder)) && !(folder is ArchiveFolder))
            {
                string path;
                DesktopIni desktopIni = GetDesktopIni(folder, true);
                if (desktopIni != null)
                {
                    return desktopIni;
                }
                if (folder is CustomFileSystemFolder)
                {
                    for (string str = Path.GetDirectoryName(folder.FullName); !string.IsNullOrEmpty(str); str = Path.GetDirectoryName(str))
                    {
                        path = Path.Combine(str, "desktop.ini");
                        if (!System.IO.File.Exists(path))
                        {
                            path = DesktopIniCache.GetPath(str);
                        }
                        if (!(string.IsNullOrEmpty(path) || !DesktopIni.CheckApplyToChildren(path)))
                        {
                            return new DesktopIni(path);
                        }
                    }
                }
                else
                {
                    for (IVirtualFolder folder2 = folder.Parent; folder2 != null; folder2 = folder2.Parent)
                    {
                        path = DesktopIniCache.GetPath(folder2.FullName);
                        if (!(string.IsNullOrEmpty(path) || !DesktopIni.CheckApplyToChildren(path)))
                        {
                            return new DesktopIni(path);
                        }
                    }
                }
            }
            return null;
        }

        public static IEnumerable<IVirtualFolder> GetRootFolders()
        {
            return new <GetRootFolders>d__0(-2);
        }

        public static bool IsFolderInaccessibleException(Exception e)
        {
            if (((e is DirectoryNotFoundException) || (e is UnauthorizedAccessException)) || (e is DeviceNotReadyException))
            {
                return true;
            }
            int hRForException = Marshal.GetHRForException(e);
            if ((hRForException & -2147024896) == -2147024896)
            {
                switch ((hRForException & 0xffff))
                {
                    case 3:
                    case 5:
                    case 0x35:
                    case 0x40:
                    case 0x41:
                    case 0x43:
                        return true;

                    case 4:
                    case 0x42:
                        goto Label_00BC;
                }
            }
            else
            {
                switch (hRForException)
                {
                    case -2147287037:
                    case -2147287035:
                    case -2147024891:
                    case -2147024875:
                        return true;

                    case -2147287036:
                        goto Label_00BC;
                }
            }
        Label_00BC:
            return false;
        }

        public static bool IsLink(string name)
        {
            return (((name != null) && name.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase)) || ResolveUrlLink.IsUrlLink(name));
        }

        public static bool IsWarningIOException(Exception e)
        {
            return ((((e is WarningException) || (e is IOException)) || ((e is UnauthorizedAccessException) || (e is WebException))) || (e is COMException));
        }

        public static IVirtualFolder OpenArchive(IChangeVirtualFile archiveFile, IVirtualFolder parent)
        {
            return OpenArchive(archiveFile, parent, false, true);
        }

        public static IVirtualFolder OpenArchive(IChangeVirtualFile archiveFile, IVirtualFolder parent, bool useHideFormat)
        {
            return OpenArchive(archiveFile, parent, useHideFormat, false);
        }

        private static IVirtualFolder OpenArchive(IChangeVirtualFile archiveFile, IVirtualFolder parent, bool useHideFormat, bool forceOpen)
        {
            NameFilter hideMaskFilter;
            string fullName = archiveFile.FullName;
            string extension = Path.GetExtension(fullName);
            if (!forceOpen && useHideFormat)
            {
                hideMaskFilter = ArchiveFormatSettings.Default.HideMaskFilter;
                if ((hideMaskFilter != null) && hideMaskFilter.MatchName(Path.GetFileName(fullName)))
                {
                    return null;
                }
                bool hideFormat = false;
                foreach (FindFormatResult result in ArchiveFormatManager.FindFormat(extension))
                {
                    hideFormat = result.Format.HideFormat;
                    if (!hideFormat)
                    {
                        break;
                    }
                }
                if (hideFormat)
                {
                    return null;
                }
            }
            Stream stream = archiveFile.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite, FileOptions.RandomAccess, 0L);
            ICollection<FindFormatResult> formatList = ArchiveFormatManager.FindFormat(stream, extension);
            if (formatList.Count != 1)
            {
                if (formatList.Count > 1)
                {
                    stream.Close();
                    return new ArchiveFormatRoot(formatList, archiveFile, parent);
                }
            }
            else
            {
                ArchiveFormatInfo format = null;
                foreach (FindFormatResult result in formatList)
                {
                    format = result.Format;
                    break;
                }
                bool flag2 = format.HideFormat;
                if (!((forceOpen || useHideFormat) || flag2))
                {
                    hideMaskFilter = ArchiveFormatSettings.Default.HideMaskFilter;
                    flag2 = (hideMaskFilter != null) && hideMaskFilter.MatchName(Path.GetFileName(fullName));
                }
                if (forceOpen || (useHideFormat ^ flag2))
                {
                    IEnumerable<ISimpleItem> archiveContent = format.OpenArchiveContent(stream, fullName);
                    if (archiveContent != null)
                    {
                        return new ArchiveFolder(string.Empty, new Uri(fullName), archiveContent, parent);
                    }
                }
            }
            stream.Close();
            return null;
        }

        public static void RaiseVirtualItemChangedEvent(VirtualItemChangedEventArgs e)
        {
            if (VirtualItemChanged != null)
            {
                VirtualItemChanged(null, e);
            }
        }

        public static void RaiseVirtualItemChangedEvent(WatcherChangeTypes changeType, IVirtualItem item)
        {
            if (VirtualItemChanged != null)
            {
                VirtualItemChanged(null, new VirtualItemChangedEventArgs(changeType, item));
            }
        }

        public static IResolveLink ResolveShellLink(ShellLink link)
        {
            if (link == null)
            {
                throw new ArgumentNullException("link");
            }
            byte[] block = link.GetBlock(0xefff0000);
            if (block != null)
            {
                IVirtualItem target = Deserialize<IVirtualItem>(block);
                if (target != null)
                {
                    return new Nomad.FileSystem.Virtual.ResolveShellLink(link, target);
                }
            }
            return null;
        }

        public static byte[] Serialize(IVirtualItem item)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, item);
                return stream.GetBuffer();
            }
        }

        [CompilerGenerated]
        private sealed class <GetRootFolders>d__0 : IEnumerable<IVirtualFolder>, IEnumerable, IEnumerator<IVirtualFolder>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualFolder <>2__current;
            public DriveInfo[] <>7__wrap3;
            public int <>7__wrap4;
            private int <>l__initialThreadId;
            public DriveInfo <Drive>5__1;

            [DebuggerHidden]
            public <GetRootFolders>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally2()
            {
                this.<>1__state = -1;
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>1__state = 1;
                            this.<>7__wrap3 = DriveInfo.GetDrives();
                            this.<>7__wrap4 = 0;
                            while (this.<>7__wrap4 < this.<>7__wrap3.Length)
                            {
                                this.<Drive>5__1 = this.<>7__wrap3[this.<>7__wrap4];
                                this.<>2__current = FileSystemDrive.Create(this.<Drive>5__1.Name);
                                this.<>1__state = 2;
                                return true;
                            Label_0085:
                                this.<>1__state = 1;
                                this.<>7__wrap4++;
                            }
                            this.<>m__Finally2();
                            this.<>2__current = NetworkFileSystemCreator.NetworkRoot;
                            this.<>1__state = 3;
                            return true;

                        case 2:
                            goto Label_0085;

                        case 3:
                            this.<>1__state = -1;
                            break;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<IVirtualFolder> IEnumerable<IVirtualFolder>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new VirtualItem.<GetRootFolders>d__0(0);
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Virtual.IVirtualFolder>.GetEnumerator();
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
                        this.<>m__Finally2();
                        break;
                }
            }

            IVirtualFolder IEnumerator<IVirtualFolder>.Current
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

        private class FallbackDesktopIni : DesktopIni
        {
            public FallbackDesktopIni(string iniPath) : base(iniPath)
            {
            }

            public override void Write()
            {
                bool flag = false;
                try
                {
                    base.Write();
                }
                catch (UnauthorizedAccessException)
                {
                    flag = true;
                }
                if (flag)
                {
                    DesktopIni dest = DesktopIniCache.Get(PathHelper.IncludeTrailingDirectorySeparator(Path.GetDirectoryName(base.FileName)));
                    dest.Set(this, this.UpdatableParts & dest.UpdatableParts);
                    dest.Write();
                }
            }
        }
    }
}

