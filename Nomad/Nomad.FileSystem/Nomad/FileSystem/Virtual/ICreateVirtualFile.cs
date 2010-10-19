namespace Nomad.FileSystem.Virtual
{
    using System;

    public interface ICreateVirtualFile
    {
        IChangeVirtualFile CreateFile(string name);
    }
}

