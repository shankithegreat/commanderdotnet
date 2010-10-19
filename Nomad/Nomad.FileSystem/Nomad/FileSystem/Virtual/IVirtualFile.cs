namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;

    public interface IVirtualFile : IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        string Extension { get; }
    }
}

