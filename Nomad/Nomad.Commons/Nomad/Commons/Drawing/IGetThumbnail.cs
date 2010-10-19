namespace Nomad.Commons.Drawing
{
    using System.Drawing;

    public interface IGetThumbnail
    {
        Image GetThumbnail(Size thumbSize);
    }
}

