namespace Nomad.Commons
{
    using System;

    public interface IResourceGuard : IDisposable
    {
        void Enter();
        void Leave();
    }
}

