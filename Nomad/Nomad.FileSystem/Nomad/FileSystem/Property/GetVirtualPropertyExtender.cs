namespace Nomad.FileSystem.Property
{
    using System;
    using System.Runtime.CompilerServices;

    public static class GetVirtualPropertyExtender
    {
        public static bool IsPropertyAvailable(this IGetVirtualProperty source, int propertyId)
        {
            return (source.GetPropertyAvailability(propertyId) != PropertyAvailability.None);
        }
    }
}

