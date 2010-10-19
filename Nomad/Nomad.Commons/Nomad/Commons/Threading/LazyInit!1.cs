namespace Nomad.Commons.Threading
{
    using System;
    using System.Threading;

    public sealed class LazyInit<T> where T: class
    {
        private Initializer<T> m_init;
        private T m_value;

        public LazyInit(Initializer<T> init)
        {
            this.m_init = init;
            this.m_value = default(T);
        }

        public T Value
        {
            get
            {
                if (this.m_value == null)
                {
                    T local = this.m_init();
                    if (Interlocked.CompareExchange<T>(ref this.m_value, local, default(T)) != null)
                    {
                        IDisposable disposable = local as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                    this.m_init = null;
                }
                return this.m_value;
            }
        }
    }
}

