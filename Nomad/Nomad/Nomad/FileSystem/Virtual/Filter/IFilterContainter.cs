namespace Nomad.FileSystem.Virtual.Filter
{
    using System;

    public interface IFilterContainter : IEquatable<IFilterContainter>
    {
        IVirtualItemFilter Filter { get; }
    }
}

