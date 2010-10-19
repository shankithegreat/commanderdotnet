namespace Nomad.FileSystem.Archive
{
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Property;
    using System;

    public static class ArchiveProperty
    {
        public static int ArchiveFormat;

        public static void Initialize()
        {
            int groupId = VirtualProperty.RegisterGroup("Default");
            ArchiveFormat = DefaultProperty.RegisterProperty("ArchiveFormat", groupId, typeof(ArchiveFormatInfo), 6, new ArchiveFormatConverter(), 0);
        }
    }
}

