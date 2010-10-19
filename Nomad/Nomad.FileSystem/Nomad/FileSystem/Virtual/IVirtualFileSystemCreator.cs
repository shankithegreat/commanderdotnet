namespace Nomad.FileSystem.Virtual
{
    using System;

    public interface IVirtualFileSystemCreator
    {
        IVirtualItem FromUri(Uri uri, VirtualItemType type, IVirtualFolder parent);

        string UriScheme { get; }
    }
}

