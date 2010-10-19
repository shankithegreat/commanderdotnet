namespace Nomad.Commons
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public static class ImageListCache
    {
        private static List<ImageList> Cache = new List<ImageList>();

        public static ImageList Get(Size imageSize)
        {
            foreach (ImageList list in Cache)
            {
                if (list.ImageSize == imageSize)
                {
                    return list;
                }
            }
            ImageList item = new ImageList {
                ImageSize = imageSize
            };
            Cache.Add(item);
            return item;
        }

        public static ImageList Get(int imageWidth, int imageHeight)
        {
            return Get(new Size(imageWidth, imageHeight));
        }
    }
}

