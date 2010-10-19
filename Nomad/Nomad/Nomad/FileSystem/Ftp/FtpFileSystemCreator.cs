namespace Nomad.FileSystem.Ftp
{
    using Nomad.FileSystem.Virtual;
    using System;

    public class FtpFileSystemCreator
    {
        public static IVirtualItem FromUri(Uri absoluteUri, VirtualItemType type, IVirtualFolder parent)
        {
            return FtpItem.FromUri(new FtpContext(), absoluteUri, type, parent);
        }

        public static IResolveLink ResolveLink(string urlPath)
        {
            return ResolveUrlLink.Create(urlPath);
        }

        public static string UriScheme
        {
            get
            {
                return Uri.UriSchemeFtp;
            }
        }
    }
}

