namespace Nomad.FileSystem.Property.Providers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [DebuggerDisplay("PropertyProviderInfo: {DisplayName}")]
    public class PropertyProviderInfo : IDisposable
    {
        public readonly IPropertyProviderFactory Factory;
        public readonly string Key;

        public PropertyProviderInfo(string key, IPropertyProviderFactory factory)
        {
            this.Key = key;
            this.Factory = factory;
        }

        public void Dispose()
        {
            this.Unload();
        }

        public void Load()
        {
            if (this.Provider == null)
            {
                this.Provider = this.Factory.CreateProvider(this.Key);
            }
        }

        public void Unload()
        {
            IDisposable provider = this.Provider as IDisposable;
            if (provider != null)
            {
                provider.Dispose();
            }
            this.Provider = null;
        }

        public string DisplayName
        {
            get
            {
                if (this.Provider != null)
                {
                    DisplayNameAttribute attribute = TypeDescriptor.GetAttributes(this.Provider).OfType<DisplayNameAttribute>().FirstOrDefault<DisplayNameAttribute>();
                    if (attribute != null)
                    {
                        return attribute.DisplayName;
                    }
                }
                return this.Factory.GetDisplayName(this.Key);
            }
        }

        public IPropertyProvider Provider { get; private set; }
    }
}

