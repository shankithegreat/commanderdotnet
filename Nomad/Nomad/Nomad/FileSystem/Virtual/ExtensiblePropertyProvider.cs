namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Property.Providers;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class ExtensiblePropertyProvider : CustomPropertyProvider, IGetVirtualProperty, IExtendGetVirtualProperty
    {
        private bool FInitialized;
        private List<IGetVirtualProperty> FProviders;

        protected ExtensiblePropertyProvider()
        {
        }

        public void AddPropertyProvider(IGetVirtualProperty propertyProvider)
        {
            if (propertyProvider == null)
            {
                throw new ArgumentNullException();
            }
            if (this.FProviders == null)
            {
                this.FProviders = new List<IGetVirtualProperty>();
            }
            this.FProviders.Add(propertyProvider);
            CustomPropertyProvider provider = propertyProvider as CustomPropertyProvider;
            if (provider != null)
            {
                provider.AvailablePropertiesChanged += new EventHandler(this.ExtendedAvailableChanged);
            }
        }

        public virtual bool CanSetProperty(int propertyId)
        {
            foreach (IGetVirtualProperty property in this.Providers)
            {
                ISetVirtualProperty property2 = property as ISetVirtualProperty;
                if ((property2 != null) && property2.CanSetProperty(propertyId))
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            this.FInitialized = false;
            this.FProviders = null;
        }

        private void ExtendedAvailableChanged(object sender, EventArgs e)
        {
            base.ResetAvailableSet();
        }

        public virtual object GetProperty(int propertyId)
        {
            foreach (IGetVirtualProperty property in this.GetProviders(propertyId))
            {
                object obj2 = property[propertyId];
                if (obj2 != null)
                {
                    return obj2;
                }
            }
            return null;
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            foreach (IGetVirtualProperty property in this.Providers)
            {
                PropertyAvailability propertyAvailability = property.GetPropertyAvailability(propertyId);
                if (propertyAvailability != PropertyAvailability.None)
                {
                    return propertyAvailability;
                }
            }
            return base.GetPropertyAvailability(propertyId);
        }

        public IEnumerable<IGetVirtualProperty> GetProviders(int propertyId)
        {
            return this.Providers.Where<IGetVirtualProperty>(delegate (IGetVirtualProperty x) {
                return x.IsPropertyAvailable(propertyId);
            });
        }

        public virtual void SetProperty(int propertyId, object value)
        {
            foreach (IGetVirtualProperty property in this.Providers)
            {
                ISetVirtualProperty property2 = property as ISetVirtualProperty;
                if ((property2 != null) && property2.CanSetProperty(propertyId))
                {
                    property2[propertyId] = value;
                    return;
                }
            }
            throw new InvalidOperationException("Provider that can set this property was not found.");
        }

        public override VirtualPropertySet AvailableProperties
        {
            get
            {
                if (!base.HasAvailableSet)
                {
                    VirtualPropertySet availableProperties = base.AvailableProperties;
                    foreach (IGetVirtualProperty property in this.Providers)
                    {
                        availableProperties.Or(property.AvailableProperties);
                    }
                }
                return base.AvailableProperties;
            }
        }

        protected bool IsProvidersCreated
        {
            get
            {
                return this.FInitialized;
            }
        }

        public virtual object this[int propertyId]
        {
            get
            {
                return this.GetProperty(propertyId);
            }
            set
            {
                this.SetProperty(propertyId, value);
            }
        }

        protected IEnumerable<IGetVirtualProperty> Providers
        {
            get
            {
                return new <get_Providers>d__0(-2) { <>4__this = this };
            }
        }

        [CompilerGenerated]
        private sealed class <get_Providers>d__0 : IEnumerable<IGetVirtualProperty>, IEnumerable, IEnumerator<IGetVirtualProperty>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IGetVirtualProperty <>2__current;
            public ExtensiblePropertyProvider <>4__this;
            private int <>l__initialThreadId;
            public int <I>5__1;

            [DebuggerHidden]
            public <get_Providers>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        if (!this.<>4__this.FInitialized)
                        {
                            PropertyProviderManager.AddProperties(this.<>4__this);
                            this.<>4__this.FInitialized = true;
                        }
                        if (this.<>4__this.FProviders != null)
                        {
                            this.<I>5__1 = this.<>4__this.FProviders.Count - 1;
                            while (this.<I>5__1 >= 0)
                            {
                                this.<>2__current = this.<>4__this.FProviders[this.<I>5__1];
                                this.<>1__state = 1;
                                return true;
                            Label_00A7:
                                this.<>1__state = -1;
                                this.<I>5__1--;
                            }
                        }
                        break;

                    case 1:
                        goto Label_00A7;
                }
                return false;
            }

            [DebuggerHidden]
            IEnumerator<IGetVirtualProperty> IEnumerable<IGetVirtualProperty>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new ExtensiblePropertyProvider.<get_Providers>d__0(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Property.IGetVirtualProperty>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            IGetVirtualProperty IEnumerator<IGetVirtualProperty>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

