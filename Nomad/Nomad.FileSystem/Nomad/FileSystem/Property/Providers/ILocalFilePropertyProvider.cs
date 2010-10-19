namespace Nomad.FileSystem.Property.Providers
{
    using Nomad.FileSystem.Property;
    using System.IO;

    public interface ILocalFilePropertyProvider : IPropertyProvider
    {
        IGetVirtualProperty AddProperties(FileSystemInfo info);
    }
}

