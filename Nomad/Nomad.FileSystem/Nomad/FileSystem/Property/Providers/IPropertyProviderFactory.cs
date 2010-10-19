namespace Nomad.FileSystem.Property.Providers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IPropertyProviderFactory : IEnumerable<string>, IEnumerable
    {
        IPropertyProvider CreateProvider(string providerKey);
        string GetDisplayName(string providerKey);
    }
}

