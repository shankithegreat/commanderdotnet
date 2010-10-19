namespace TagLib
{
    using System;

    public interface IAudioCodec : ICodec
    {
        int AudioBitrate { get; }

        int AudioChannels { get; }

        int AudioSampleRate { get; }
    }
}

