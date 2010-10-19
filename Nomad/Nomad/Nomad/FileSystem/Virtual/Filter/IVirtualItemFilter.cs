namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad.FileSystem.Virtual;
    using System;

    public interface IVirtualItemFilter : IEquatable<IVirtualItemFilter>
    {
        bool IsMatch(IVirtualItem item);
    }
}

