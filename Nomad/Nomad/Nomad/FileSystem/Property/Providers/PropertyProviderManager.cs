namespace Nomad.FileSystem.Property.Providers
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class PropertyProviderManager
    {
        private static List<PropertyProviderInfo> ProviderList;
        public static readonly TraceSource ProviderTrace = new TraceSource("PropertyProvider");

        public static void AddProperties(IExtendGetVirtualProperty item)
        {
            if (((ProviderList != null) && (ProviderList.Count != 0)) && (item != null))
            {
                IVirtualItem item2 = item as IVirtualItem;
                if (item2 != null)
                {
                    FileSystemItem item3 = item2 as FileSystemItem;
                    FileSystemInfo info = (item3 != null) ? item3.Info : null;
                    foreach (IPropertyProvider provider in GetLoadedProviders())
                    {
                        IGetVirtualProperty propertyProvider = null;
                        try
                        {
                            if (info != null)
                            {
                                ILocalFilePropertyProvider provider2 = provider as ILocalFilePropertyProvider;
                                if (provider2 != null)
                                {
                                    propertyProvider = provider2.AddProperties(info);
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            ProviderTrace.TraceException(TraceEventType.Error, exception);
                        }
                        if (propertyProvider != null)
                        {
                            item.AddPropertyProvider(propertyProvider);
                        }
                    }
                }
            }
        }

        public static IEnumerable<PropertyProviderInfo> GetAllProviders()
        {
            return ProviderList;
        }

        public static IEnumerable<IPropertyProvider> GetLoadedProviders()
        {
            return new <GetLoadedProviders>d__0(-2);
        }

        public static void Intitialize(IEnumerable<string> disabledProviders)
        {
            NameValueCollection section = ConfigurationManager.GetSection("propertyProviders/factories") as NameValueCollection;
            if (section != null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                foreach (string str in disabledProviders)
                {
                    dictionary.Add(str, 0);
                }
                ProviderList = new List<PropertyProviderInfo>();
                foreach (string str2 in section.Keys)
                {
                    Exception exception;
                    try
                    {
                        IPropertyProviderFactory factory = Activator.CreateInstance(Type.GetType(str2)) as IPropertyProviderFactory;
                        if (factory != null)
                        {
                            foreach (string str3 in factory)
                            {
                                try
                                {
                                    PropertyProviderInfo item = new PropertyProviderInfo(str3, factory);
                                    if (!dictionary.ContainsKey(str3))
                                    {
                                        item.Load();
                                    }
                                    ProviderList.Add(item);
                                }
                                catch (Exception exception1)
                                {
                                    exception = exception1;
                                    Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                                }
                            }
                        }
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                    }
                }
            }
        }

        public static T ReadOption<T>(Hashtable options, string name, T defaultValue) where T: struct
        {
            if (options == null)
            {
                return defaultValue;
            }
            string str = options[name] as string;
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            try
            {
                return (T) TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(str);
            }
            catch (Exception exception)
            {
                ProviderTrace.TraceException(TraceEventType.Error, exception);
                return defaultValue;
            }
        }

        [CompilerGenerated]
        private sealed class <GetLoadedProviders>d__0 : IEnumerable<IPropertyProvider>, IEnumerable, IEnumerator<IPropertyProvider>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IPropertyProvider <>2__current;
            public List<PropertyProviderInfo>.Enumerator <>7__wrap2;
            private int <>l__initialThreadId;
            public PropertyProviderInfo <NextProviderInfo>5__1;

            [DebuggerHidden]
            public <GetLoadedProviders>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally3()
            {
                this.<>1__state = -1;
                this.<>7__wrap2.Dispose();
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            break;

                        case 2:
                            goto Label_0082;

                        default:
                            goto Label_00A0;
                    }
                    this.<>1__state = -1;
                    this.<>7__wrap2 = PropertyProviderManager.ProviderList.GetEnumerator();
                    this.<>1__state = 1;
                    while (this.<>7__wrap2.MoveNext())
                    {
                        this.<NextProviderInfo>5__1 = this.<>7__wrap2.Current;
                        if (this.<NextProviderInfo>5__1.Provider == null)
                        {
                            continue;
                        }
                        this.<>2__current = this.<NextProviderInfo>5__1.Provider;
                        this.<>1__state = 2;
                        return true;
                    Label_0082:
                        this.<>1__state = 1;
                    }
                    this.<>m__Finally3();
                Label_00A0:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<IPropertyProvider> IEnumerable<IPropertyProvider>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new PropertyProviderManager.<GetLoadedProviders>d__0(0);
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Property.Providers.IPropertyProvider>.GetEnumerator();
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
                            this.<>m__Finally3();
                        }
                        break;
                }
            }

            IPropertyProvider IEnumerator<IPropertyProvider>.Current
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

