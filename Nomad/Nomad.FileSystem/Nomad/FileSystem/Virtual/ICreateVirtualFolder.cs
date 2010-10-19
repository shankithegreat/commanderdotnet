namespace Nomad.FileSystem.Virtual
{
    using System;

    public interface ICreateVirtualFolder
    {
        IVirtualFolder CreateFolder(string name);
    }
}

