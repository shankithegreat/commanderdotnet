namespace Nomad.FileSystem.Archive.Common
{
    public interface ISequenceContext
    {
        ISequenceProcessor CreateProcessor(SequenseProcessorType type);
    }
}

