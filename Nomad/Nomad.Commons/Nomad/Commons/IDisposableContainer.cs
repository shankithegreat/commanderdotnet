namespace Nomad.Commons
{
    using System;

    public interface IDisposableContainer : IDisposable
    {
        void Add(IDisposable item);
        void Remove(IDisposable item);
    }
}

