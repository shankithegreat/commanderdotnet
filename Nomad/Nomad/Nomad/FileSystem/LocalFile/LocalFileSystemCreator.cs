namespace Nomad.FileSystem.LocalFile
{
    using Nomad.Commons.Plugin;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class LocalFileSystemCreator
    {
        private static WeakClientSponsor _Sponsor;

        public static  event FileSystemEventHandler GlobalFileChanged;

        public static Process Execute(IWin32Window owner, ProcessStartInfo startInfo)
        {
            Process process;
            try
            {
                if (owner != null)
                {
                    startInfo.ErrorDialog = true;
                    startInfo.ErrorDialogParentHandle = owner.Handle;
                }
                process = Process.Start(startInfo);
            }
            catch (Win32Exception exception)
            {
                switch (exception.NativeErrorCode)
                {
                    case 0xc1:
                    case 0x4c7:
                        return null;

                    case 0x483:
                        throw new WarningException(exception.Message, exception);

                    case 2:
                        throw new FileNotFoundException(exception.Message, exception);

                    case 5:
                        throw new UnauthorizedAccessException(exception.Message, exception);

                    case 0x20:
                        throw new IOException(exception.Message, exception);
                }
                throw;
            }
            return process;
        }

        public static void ExecuteVerb(IWin32Window owner, string fileName, string verb)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName) {
                Verb = verb
            };
            Execute(owner, startInfo);
        }

        public static FileSystemItem FromFileSystemInfo(FileSystemInfo info, IVirtualFolder parent)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            DirectoryInfo info2 = info as DirectoryInfo;
            if (info2 != null)
            {
                return new FileSystemFolder(info2, parent);
            }
            FileInfo info3 = (FileInfo) info;
            if (VirtualItem.IsLink(info3.FullName))
            {
                return new FileSystemShellLink(info3, parent);
            }
            return new FileSystemFile(info3, parent);
        }

        public static FileSystemItem FromFullName(string fullName, VirtualItemType type, IVirtualFolder parent)
        {
            if (fullName == null)
            {
                throw new ArgumentNullException("fullName");
            }
            if (type == VirtualItemType.Unknown)
            {
                if (!Directory.Exists(fullName))
                {
                    if (!File.Exists(fullName))
                    {
                        throw new FileNotFoundException(string.Format("Cannot discover '{0}' item type because no such file or folder found.", fullName), fullName);
                    }
                    type = VirtualItemType.File;
                }
                else
                {
                    type = VirtualItemType.Folder;
                }
            }
            fullName = fullName.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (type == VirtualItemType.Folder)
            {
                return new FileSystemFolder(fullName, parent);
            }
            if (VirtualItem.IsLink(fullName))
            {
                return new FileSystemShellLink(fullName, parent);
            }
            return new FileSystemFile(fullName, parent);
        }

        public static void RaiseFileChangedEvent(FileSystemEventArgs e)
        {
            if (GlobalFileChanged != null)
            {
                GlobalFileChanged(null, e);
            }
        }

        public static void RaiseFileChangedEvent(WatcherChangeTypes changeType, string fullName)
        {
            if (fullName == null)
            {
                throw new ArgumentNullException();
            }
            if (fullName == string.Empty)
            {
                throw new ArgumentException();
            }
            switch (changeType)
            {
                case WatcherChangeTypes.Created:
                case WatcherChangeTypes.Deleted:
                case WatcherChangeTypes.Changed:
                    RaiseFileChangedEvent(new FileSystemEventArgs(changeType, Path.GetDirectoryName(fullName), Path.GetFileName(fullName)));
                    return;
            }
            throw new InvalidEnumArgumentException();
        }

        public static void RaiseFileChangedEvent(string fullName, string newName)
        {
            if (fullName == null)
            {
                throw new ArgumentNullException("fullName");
            }
            if (fullName == string.Empty)
            {
                throw new ArgumentException("fullName");
            }
            if (newName == null)
            {
                throw new ArgumentNullException("newName");
            }
            if (newName == string.Empty)
            {
                throw new ArgumentException("newName");
            }
            RaiseFileChangedEvent(new RenamedEventArgs(WatcherChangeTypes.Renamed, Path.GetDirectoryName(fullName), newName, Path.GetFileName(fullName)));
        }

        public static WeakClientSponsor Sponsor
        {
            get
            {
                if (_Sponsor == null)
                {
                    WeakClientSponsor sponsor1 = _Sponsor;
                }
                return (_Sponsor = new WeakClientSponsor(TimeSpan.FromSeconds(9.0)));
            }
        }
    }
}

