namespace Nomad.FileSystem.Property.Providers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class SimplePropertyProviderFactory : IPropertyProviderFactory, IEnumerable<string>, IEnumerable
    {
        private NameValueCollection ProviderCollection;

        public IPropertyProvider CreateProvider(string providerKey)
        {
            Type type = Type.GetType(providerKey);
            ISimplePropertyProvider instance = Activator.CreateInstance(type) as ISimplePropertyProvider;
            if ((instance != null) && instance.Register(ConfigurationManager.GetSection("propertyProviders/" + type.Name) as Hashtable))
            {
                TypeDescriptor.AddAttributes(instance, new Attribute[] { new DisplayNameAttribute(this.GetDisplayName(providerKey)) });
                return instance;
            }
            IDisposable disposable = instance as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
            return null;
        }

        public string GetDisplayName(string providerKey)
        {
            this.ProviderCollectionNeeded();
            if (this.ProviderCollection != null)
            {
                return this.ProviderCollection[providerKey];
            }
            return providerKey;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new <GetEnumerator>d__0(0) { <>4__this = this };
        }

        private void ProviderCollectionNeeded()
        {
            if (this.ProviderCollection == null)
            {
                this.ProviderCollection = ConfigurationManager.GetSection("propertyProviders/simpleProviders") as NameValueCollection;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__0 : IEnumerator<string>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private string <>2__current;
            public SimplePropertyProviderFactory <>4__this;
            public IEnumerator <>7__wrap2;
            public IDisposable <>7__wrap3;
            public string <NextKey>5__1;

            [DebuggerHidden]
            public <GetEnumerator>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private void <>m__Finally4()
            {
                this.<>1__state = -1;
                this.<>7__wrap3 = this.<>7__wrap2 as IDisposable;
                if (this.<>7__wrap3 != null)
                {
                    this.<>7__wrap3.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>4__this.ProviderCollectionNeeded();
                            if (this.<>4__this.ProviderCollection != null)
                            {
                                this.<>7__wrap2 = this.<>4__this.ProviderCollection.Keys.GetEnumerator();
                                this.<>1__state = 1;
                                while (this.<>7__wrap2.MoveNext())
                                {
                                    this.<NextKey>5__1 = (string) this.<>7__wrap2.Current;
                                    this.<>2__current = this.<NextKey>5__1;
                                    this.<>1__state = 2;
                                    return true;
                                Label_009C:
                                    this.<>1__state = 1;
                                }
                                this.<>m__Finally4();
                            }
                            break;

                        case 2:
                            goto Label_009C;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally4();
                        }
                        break;
                }
            }

            string IEnumerator<string>.Current
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

