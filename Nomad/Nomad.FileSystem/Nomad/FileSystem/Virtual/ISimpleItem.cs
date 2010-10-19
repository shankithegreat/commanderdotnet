namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;

    public interface ISimpleItem : IGetVirtualProperty
    {
        string Name { get; }
    }
}

