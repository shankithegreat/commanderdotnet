namespace Nomad.FileSystem.Property
{
    using System;
    using System.Reflection;

    public interface ISetVirtualProperty : IGetVirtualProperty
    {
        bool CanSetProperty(int propertyId);

        object this[int propertyId] { get; set; }
    }
}

