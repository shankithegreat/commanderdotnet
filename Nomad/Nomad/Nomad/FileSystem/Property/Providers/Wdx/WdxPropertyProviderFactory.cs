namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.FileSystem.Property.Providers;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;

    public class WdxPropertyProviderFactory : IPropertyProviderFactory, IEnumerable<string>, IEnumerable
    {
        public IPropertyProvider CreateProvider(string providerKey)
        {
            IPropertyProvider instance = WdxPropertyProvider.Create(providerKey);
            if (instance != null)
            {
                TypeDescriptor.AddAttributes(instance, new Attribute[] { new DisplayNameAttribute(this.GetDisplayName(providerKey)) });
            }
            return instance;
        }

        public string GetDisplayName(string providerKey)
        {
            return Path.GetFileName(providerKey);
        }

        public IEnumerator<string> GetEnumerator()
        {
            string[] source = new string[0];
            try
            {
                source = Directory.GetFiles(WdxPluginsPath, "*.wdx", SearchOption.AllDirectories);
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (Exception exception)
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
            }
            return source.Cast<string>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public static string WdxPluginsPath
        {
            get
            {
                return Path.Combine(SettingsManager.SpecialFolders.Plugins, "Wdx");
            }
        }
    }
}

