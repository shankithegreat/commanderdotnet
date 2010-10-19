namespace Nomad.FileSystem.Property
{
    using System;
    using System.Reflection;

    public interface IGetVirtualProperty
    {
        PropertyAvailability GetPropertyAvailability(int propertyId);

        VirtualPropertySet AvailableProperties { get; }

        object this[int propertyId] { get; }
    }
}

