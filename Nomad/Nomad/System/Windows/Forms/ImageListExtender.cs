namespace System.Windows.Forms
{
    using Microsoft;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;

    public static class ImageListExtender
    {
        public static void AddAspected(this ImageList list, Image img)
        {
            if (list.ImageSize.Height != list.ImageSize.Width)
            {
                using (Bitmap bitmap = new Bitmap(list.ImageSize.Width, list.ImageSize.Height, img.PixelFormat))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.Clear((list.ColorDepth == ColorDepth.Depth32Bit) ? Color.Transparent : list.TransparentColor);
                        int num = list.ImageSize.Height - img.Height;
                        graphics.DrawImage(img, (int) ((list.ImageSize.Width - img.Width) / 2), (int) ((num / 2) + (num % 2)));
                    }
                    list.CreateHandle();
                    list.Images.Add(bitmap);
                }
            }
            else
            {
                list.Images.Add(img);
            }
        }

        public static int AddNormalized(this ImageList list, Image img, Color backColor)
        {
            PixelFormat pixelFormat;
            bool flag = false;
            ColorDepth colorDepth = list.ColorDepth;
            switch (colorDepth)
            {
                case ColorDepth.Depth16Bit:
                case ColorDepth.Depth24Bit:
                    pixelFormat = img.PixelFormat;
                    if (pixelFormat <= PixelFormat.Format64bppPArgb)
                    {
                        switch (pixelFormat)
                        {
                            case PixelFormat.Format32bppPArgb:
                            case PixelFormat.Format64bppPArgb:
                                goto Label_00A5;
                        }
                        goto Label_00AB;
                    }
                    if ((pixelFormat != PixelFormat.Format32bppArgb) && (pixelFormat != PixelFormat.Format64bppArgb))
                    {
                        goto Label_00AB;
                    }
                    goto Label_00A5;

                default:
                    if ((colorDepth != ColorDepth.Depth32Bit) || OS.IsWinXP)
                    {
                        goto Label_00AB;
                    }
                    pixelFormat = img.PixelFormat;
                    if (pixelFormat <= PixelFormat.Format64bppPArgb)
                    {
                        goto Label_00AB;
                    }
                    if ((pixelFormat != PixelFormat.Format32bppArgb) && (pixelFormat != PixelFormat.Format64bppArgb))
                    {
                        goto Label_00AB;
                    }
                    break;
            }
            flag = true;
            goto Label_00AB;
        Label_00A5:
            flag = true;
        Label_00AB:
            if (flag)
            {
                using (Bitmap bitmap = new Bitmap(list.ImageSize.Width, list.ImageSize.Height, PixelFormat.Format24bppRgb))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.Clear(backColor);
                        int num = list.ImageSize.Height - img.Height;
                        graphics.DrawImage(img, (int) ((list.ImageSize.Width - img.Width) / 2), (int) ((num / 2) + (num % 2)));
                    }
                    list.CreateHandle();
                    list.Images.Add(bitmap, backColor);
                }
            }
            else
            {
                list.AddAspected(img);
            }
            return (list.Images.Count - 1);
        }

        public static void CreateHandle(this ImageList list)
        {
            if (!list.HandleCreated)
            {
                IntPtr handle = list.Handle;
            }
        }
    }
}

