namespace Nomad.FileSystem.Virtual
{
    using System;

    public interface ICreateVirtualLink
    {
        LinkType CanCreateLinkIn(IVirtualFolder destFolder);
        IVirtualItem CreateLink(IVirtualFolder destFolder, string name, LinkType type);
        string GetPrefferedLinkName(LinkType type);
    }
}

