namespace Nomad.FileSystem.Virtual
{
    using System;
    using System.IO;

    public interface IGetVirtualVolume : IGetVirtualRoot
    {
        uint ClusterSize { get; }

        string FileSystem { get; }

        string Location { get; }

        DriveType VolumeType { get; }
    }
}

