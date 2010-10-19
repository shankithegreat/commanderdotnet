namespace TagLib
{
    using System;

    public interface IPicture
    {
        ByteVector Data { get; set; }

        string Description { get; set; }

        string MimeType { get; set; }

        PictureType Type { get; set; }
    }
}

