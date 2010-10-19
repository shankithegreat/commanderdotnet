namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;

    public interface IVirtualLink : IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        IVirtualItem Target { get; }
    }
}

