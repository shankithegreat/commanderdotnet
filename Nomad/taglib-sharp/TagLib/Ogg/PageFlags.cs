namespace TagLib.Ogg
{
    using System;

    [Flags]
    public enum PageFlags : byte
    {
        FirstPacketContinued = 1,
        FirstPageOfStream = 2,
        LastPageOfStream = 4,
        None = 0
    }
}

