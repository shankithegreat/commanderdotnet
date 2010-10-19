namespace TagLib
{
    using System;

    public interface IVideoCodec : ICodec
    {
        int VideoHeight { get; }

        int VideoWidth { get; }
    }
}

