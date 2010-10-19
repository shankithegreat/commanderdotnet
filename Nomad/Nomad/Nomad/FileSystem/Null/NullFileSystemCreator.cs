namespace Nomad.FileSystem.Null
{
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.IO;

    public class NullFileSystemCreator
    {
        public static IVirtualItem FromUri(Uri nullUri, VirtualItemType type)
        {
            if (nullUri == null)
            {
                throw new ArgumentNullException("nullUri");
            }
            if (nullUri.Scheme != UriScheme)
            {
                throw new ArgumentException(string.Format(Resources.sErrorAnotherSchemeExpected, UriScheme, nullUri.Scheme));
            }
            if (nullUri.Host != ".")
            {
                throw new ArgumentException(string.Format(Resources.sErrorOnlyDotHostAccepted, nullUri.Host));
            }
            switch (type)
            {
                case VirtualItemType.Folder:
                    return new NullFolder(Path.GetFileName(nullUri.AbsolutePath));

                case VirtualItemType.File:
                    return new NullFile(Path.GetFileName(nullUri.AbsolutePath));
            }
            throw new ApplicationException("Cannot detect item type in null file system.");
        }

        public static string UriScheme
        {
            get
            {
                return "null";
            }
        }
    }
}

