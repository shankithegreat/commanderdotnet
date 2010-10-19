namespace TagLib.Flac
{
    using System;

    public enum BlockType
    {
        StreamInfo,
        Padding,
        Application,
        SeekTable,
        XiphComment,
        CueSheet,
        Picture
    }
}

