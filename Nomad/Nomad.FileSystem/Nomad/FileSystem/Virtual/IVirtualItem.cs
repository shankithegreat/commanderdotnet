namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;
    using System.IO;

    public interface IVirtualItem : ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        FileAttributes Attributes { get; }

        string FullName { get; }

        IVirtualFolder Parent { get; }

        string ShortName { get; }
    }
}

