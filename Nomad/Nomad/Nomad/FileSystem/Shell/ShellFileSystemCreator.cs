namespace Nomad.FileSystem.Shell
{
    using Microsoft.Shell;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;

    public class ShellFileSystemCreator
    {
        public static IVirtualItem FromUri(Uri shellUri, VirtualItemType type)
        {
            if (shellUri == null)
            {
                throw new ArgumentNullException("shellUri");
            }
            if (shellUri.Scheme != UriScheme)
            {
                throw new ArgumentException("Unknown shellUri scheme (shell expected)");
            }
            string localPath = shellUri.LocalPath;
            if (localPath.StartsWith("/::", StringComparison.Ordinal))
            {
                localPath = localPath.Substring(1).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }
            SafeShellItem item = new SafeShellItem(IntPtr.Zero, localPath);
            if (type == VirtualItemType.Folder)
            {
                return new ShellFolder(item);
            }
            return ShellFolder.CreateShellItem(item, item.GetAttributesOf(SFGAO.SFGAO_FOLDER | SFGAO.SFGAO_STREAM), null);
        }

        public static IResolveLink ResolveShellLink(ShellLink link)
        {
            if (link == null)
            {
                throw new ArgumentNullException("link");
            }
            IntPtr idList = link.IdList;
            if (idList != IntPtr.Zero)
            {
                try
                {
                    SafeShellItem item = new SafeShellItem(idList);
                    IVirtualItem target = ShellFolder.CreateShellItem(item, item.GetAttributesOf(SFGAO.SFGAO_FOLDER | SFGAO.SFGAO_STREAM), null);
                    if (target != null)
                    {
                        return new Nomad.FileSystem.Virtual.ResolveShellLink(link, target);
                    }
                }
                catch (ArgumentException)
                {
                }
            }
            return null;
        }

        public static string UriScheme
        {
            get
            {
                return "shell";
            }
        }
    }
}

