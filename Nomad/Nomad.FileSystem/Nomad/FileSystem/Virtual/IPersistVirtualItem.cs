namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;

    public interface IPersistVirtualItem : IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        bool Exists { get; }
    }
}

