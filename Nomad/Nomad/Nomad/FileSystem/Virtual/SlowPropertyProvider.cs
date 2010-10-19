namespace Nomad.FileSystem.Virtual
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Threading;
    using Nomad.FileSystem.Property;
    using System;
    using System.Diagnostics;

    public abstract class SlowPropertyProvider : CachedPropertyProvider
    {
        private Action<Tuple<IGetVirtualProperty, int>> GetSlowPropertyCallback;
        protected static readonly LazyInit<WorkQueue> SlowPropertyQueue;

        static SlowPropertyProvider()
        {
            SlowPropertyQueue = new LazyInit<WorkQueue>(delegate {
                ThreadPoolQueue queue = new ThreadPoolQueue();
                queue.Error += delegate (object sender, ExceptionEventArgs e) {
                    Nomad.Trace.Error.TraceException(TraceEventType.Critical, e.ErrorException);
                };
                return queue;
            });
        }

        protected SlowPropertyProvider()
        {
        }

        public virtual void CancelGetSlowProperty()
        {
            this.GetSlowPropertyCallback = null;
            if (base.IsProvidersCreated)
            {
                foreach (IGetVirtualProperty property in base.Providers)
                {
                    IDisposable disposable = property as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        public override object GetProperty(int propertyId)
        {
            object slowProperty = null;
            foreach (IGetVirtualProperty property in base.Providers)
            {
                switch (property.GetPropertyAvailability(propertyId))
                {
                    case PropertyAvailability.Normal:
                        slowProperty = property[propertyId];
                        break;

                    case PropertyAvailability.Slow:
                    case PropertyAvailability.OnDemand:
                        slowProperty = this.GetSlowProperty(property, propertyId);
                        break;
                }
                if (slowProperty != null)
                {
                    return slowProperty;
                }
            }
            return slowProperty;
        }

        private void GetSlowProperty(Tuple<IGetVirtualProperty, int> state)
        {
            if (this.GetSlowPropertyCallback != null)
            {
                this.OnSlowPropertyComplete(state.Item2, state.Item1[state.Item2]);
            }
        }

        protected object GetSlowProperty(IGetVirtualProperty provider, int propertyId)
        {
            object obj2 = '∑';
            base.AddPropertyToCache(propertyId, obj2);
            if (this.GetSlowPropertyCallback == null)
            {
                this.GetSlowPropertyCallback = new Action<Tuple<IGetVirtualProperty, int>>(this.GetSlowProperty);
            }
            SlowPropertyQueue.Value.QueueWeakWorkItem<Tuple<IGetVirtualProperty, int>>(this.GetSlowPropertyCallback, Tuple.Create<IGetVirtualProperty, int>(provider, propertyId));
            return obj2;
        }

        protected virtual void OnSlowPropertyComplete(int propertyId, object value)
        {
            base.AddPropertyToCache(propertyId, value);
        }
    }
}

