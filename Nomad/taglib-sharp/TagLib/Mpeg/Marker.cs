namespace TagLib.Mpeg
{
    using System;

    public enum Marker
    {
        AudioPacket = 0xc0,
        Corrupt = -1,
        EndOfStream = 0xb9,
        PaddingPacket = 190,
        SystemPacket = 0xbb,
        SystemSyncPacket = 0xba,
        VideoPacket = 0xe0,
        VideoSyncPacket = 0xb3,
        Zero = 0
    }
}

