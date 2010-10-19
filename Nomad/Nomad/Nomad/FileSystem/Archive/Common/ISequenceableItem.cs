namespace Nomad.FileSystem.Archive.Common
{
    public interface ISequenceableItem
    {
        ISequenceContext SequenceContext { get; }
    }
}

