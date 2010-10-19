namespace Nomad.FileSystem.Network
{
    using Microsoft;
    using Microsoft.Shell;
    using Microsoft.Win32.Network;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class NetworkFileSystemCreator
    {
        public static readonly IVirtualFolder NetworkRoot = new NetworkFolder();

        internal static int AddConnection(IWin32Window owner, NETRESOURCE resource)
        {
            if (owner == null)
            {
                return Winnetwk.WNetAddConnection2(ref resource, null, null, CONNECT.CONNECT_PROMPT | CONNECT.CONNECT_INTERACTIVE);
            }
            Control control = owner as Control;
            if ((control != null) && control.InvokeRequired)
            {
                return (int) control.Invoke(new AddConnectionHanlder(NetworkFileSystemCreator.AddConnection), new object[] { owner, resource });
            }
            return Winnetwk.WNetAddConnection3(owner.Handle, ref resource, null, null, CONNECT.CONNECT_PROMPT | CONNECT.CONNECT_INTERACTIVE);
        }

        public static IVirtualFolder FromFullName(string fullName, IVirtualFolder parent)
        {
            if (fullName == null)
            {
                throw new ArgumentNullException("fullName");
            }
            if (fullName.StartsWith(PathHelper.UncPrefix))
            {
                return new NetworkFolder(fullName, parent);
            }
            Uri uri = new Uri(fullName);
            if (uri.Scheme != UriScheme)
            {
                throw new ArgumentException(string.Format(Resources.sErrorAnotherSchemeExpected, UriScheme, uri.Scheme));
            }
            if (uri.Host != ".")
            {
                throw new ArgumentException(string.Format(Resources.sErrorOnlyDotHostAccepted, uri.Host));
            }
            string[] strArray = uri.LocalPath.Split(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            NETRESOURCE resource = new NETRESOURCE {
                dwType = RESOURCETYPE.RESOURCETYPE_ANY,
                dwUsage = RESOURCEUSAGE.RESOURCEUSAGE_CONTAINER
            };
            switch (strArray.Length)
            {
                case 0:
                    return NetworkRoot;

                case 1:
                    resource.lpProvider = strArray[0];
                    resource.lpRemoteName = strArray[0];
                    resource.dwUsage |= (RESOURCEUSAGE) (-2147483648);
                    resource.dwDisplayType = RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_NETWORK;
                    return new NetworkFolder(resource, parent ?? NetworkRoot);

                case 2:
                    resource.lpProvider = strArray[0];
                    resource.lpRemoteName = strArray[1];
                    resource.dwDisplayType = RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_DOMAIN;
                    return new NetworkFolder(resource, parent);
            }
            throw new ArgumentException(string.Format("'{0}' is not valid network folder name", fullName));
        }

        public static string GetRemoteName(string localName)
        {
            string lpUniversalName;
            int cb = 0x400;
            IntPtr lpBuffer = Marshal.AllocHGlobal(cb);
            try
            {
                if (Winnetwk.WNetGetUniversalName(localName, NAME_INFO.UNIVERSAL_NAME_INFO_LEVEL, lpBuffer, ref cb) != 0)
                {
                    return null;
                }
                UNIVERSAL_NAME_INFO universal_name_info = (UNIVERSAL_NAME_INFO) Marshal.PtrToStructure(lpBuffer, typeof(UNIVERSAL_NAME_INFO));
                lpUniversalName = universal_name_info.lpUniversalName;
            }
            finally
            {
                Marshal.FreeHGlobal(lpBuffer);
            }
            return lpUniversalName;
        }

        public static IResolveLink ResolveShellLink(ShellLink link)
        {
            if (link == null)
            {
                throw new ArgumentNullException("link");
            }
            string path = null;
            IShellFolder desktopFolder = ShellItem.GetDesktopFolder();
            try
            {
                IntPtr idList = link.IdList;
                if (idList == IntPtr.Zero)
                {
                    return null;
                }
                path = desktopFolder.GetDisplayNameOf(idList, SHGNO.SHGDN_FORPARSING);
                if (path != null)
                {
                    switch (PathHelper.GetPathType(path))
                    {
                        case PathType.NetworkServer:
                        case PathType.NetworkShare:
                            return new Nomad.FileSystem.Virtual.ResolveShellLink(link, path);
                    }
                    if (OS.IsWinVista && string.Equals(path, Microsoft.Shell.Shell32.GetClsidFolderParseName(CLSID.CLSID_NETWORK), StringComparison.OrdinalIgnoreCase))
                    {
                        return new Nomad.FileSystem.Virtual.ResolveShellLink(link, UriScheme + Uri.SchemeDelimiter + "./");
                    }
                }
                StringBuilder builder = new StringBuilder();
                ITEMIDLIST itemidlist = ITEMIDLIST.FromPidl(idList);
                IntPtr pidl = Marshal.AllocCoTaskMem(itemidlist.Size);
                try
                {
                    for (int i = 0; i < itemidlist.mkid.Length; i++)
                    {
                        itemidlist.ToPidl(pidl, i + 1);
                        string a = desktopFolder.GetDisplayNameOf(pidl, SHGNO.SHGDN_FORPARSING);
                        switch (i)
                        {
                            case 0:
                                if (string.Equals(a, Microsoft.Shell.Shell32.GetClsidFolderParseName(CLSID.CLSID_NETWORK_NEIGHBORHOOD), StringComparison.OrdinalIgnoreCase))
                                {
                                    break;
                                }
                                return null;

                            case 1:
                                if (!(a != "EntireNetwork"))
                                {
                                    continue;
                                }
                                return null;

                            case 2:
                            {
                                builder.Append(a);
                                builder.Append('/');
                                continue;
                            }
                            case 3:
                            {
                                builder.Append(a);
                                builder.Append('/');
                                continue;
                            }
                            default:
                                return null;
                        }
                        builder.Append(UriScheme);
                        builder.Append(Uri.SchemeDelimiter);
                        builder.Append('.');
                        builder.Append('/');
                    }
                    path = builder.ToString();
                    if (path.Length > 0)
                    {
                        return new Nomad.FileSystem.Virtual.ResolveShellLink(link, path);
                    }
                }
                finally
                {
                    Marshal.FreeCoTaskMem(pidl);
                }
            }
            catch (ArgumentException)
            {
            }
            finally
            {
                Marshal.ReleaseComObject(desktopFolder);
            }
            return null;
        }

        public static string UriScheme
        {
            get
            {
                return "network";
            }
        }

        private delegate int AddConnectionHanlder(IWin32Window owner, NETRESOURCE resource);
    }
}

