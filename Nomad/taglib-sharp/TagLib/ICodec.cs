namespace TagLib
{
    using System;

    public interface ICodec
    {
        string Description { get; }

        TimeSpan Duration { get; }

        TagLib.MediaTypes MediaTypes { get; }
    }
}

