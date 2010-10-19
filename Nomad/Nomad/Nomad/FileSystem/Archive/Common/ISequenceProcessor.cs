namespace Nomad.FileSystem.Archive.Common
{
    using System;

    public interface ISequenceProcessor
    {
        void Add(ISequenceableItem item, object userState);
        void Process(ProcessItemHandler handler);

        ProcessorState State { get; }
    }
}

