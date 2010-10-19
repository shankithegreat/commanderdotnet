namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public interface IVirtualAlternateStreams : IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        void Delete(string streamName);
        IEnumerable<string> GetStreamNames();
        Stream Open(string streamName, FileMode mode, FileAccess access, FileShare share, FileOptions options);

        bool HasAlternateStreams { get; }

        bool IsSupported { get; }
    }
}

