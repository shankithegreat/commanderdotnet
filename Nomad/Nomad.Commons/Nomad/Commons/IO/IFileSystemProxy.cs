namespace Nomad.Commons.IO
{
    using Microsoft.IO;
    using System;
    using System.Collections.Generic;

    public interface IFileSystemProxy
    {
        IEnumerable<AlternateDataStreamInfo> EnumerateAlternateStreams(string fileName);
        AlternateDataStreamInfo GetAlternateStream(string fileName, string streamName);
        void MoveToRecycleBin(string fileName);
    }
}

