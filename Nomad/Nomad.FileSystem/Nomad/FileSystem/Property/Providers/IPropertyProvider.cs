namespace Nomad.FileSystem.Property.Providers
{
    using Nomad.FileSystem.Property;

    public interface IPropertyProvider
    {
        VirtualPropertySet GetRegisteredProperties();
    }
}

