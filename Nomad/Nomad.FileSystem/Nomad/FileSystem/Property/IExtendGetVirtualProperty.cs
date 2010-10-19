namespace Nomad.FileSystem.Property
{
    using System;

    public interface IExtendGetVirtualProperty
    {
        void AddPropertyProvider(IGetVirtualProperty propertyProvider);
    }
}

