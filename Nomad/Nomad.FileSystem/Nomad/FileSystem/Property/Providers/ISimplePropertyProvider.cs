namespace Nomad.FileSystem.Property.Providers
{
    using System;
    using System.Collections;

    public interface ISimplePropertyProvider : IPropertyProvider
    {
        bool Register(Hashtable options);
    }
}

