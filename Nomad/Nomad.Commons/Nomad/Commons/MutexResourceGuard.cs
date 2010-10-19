namespace Nomad.Commons
{
    using System;
    using System.Threading;

    public class MutexResourceGuard : IResourceGuard, IDisposable
    {
        private Mutex FResource;

        public MutexResourceGuard(object resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException();
            }
            this.FResource = resource as Mutex;
            if (this.FResource == null)
            {
                throw new ArgumentException();
            }
        }

        public void Enter()
        {
            this.FResource.WaitOne();
        }

        public void Leave()
        {
            this.FResource.ReleaseMutex();
        }

        void IDisposable.Dispose()
        {
            this.Leave();
        }
    }
}

