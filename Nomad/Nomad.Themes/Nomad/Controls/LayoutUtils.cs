namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal static class LayoutUtils
    {
        public static Rectangle DeflateRect(Rectangle rect, Padding padding)
        {
            rect.X += padding.Left;
            rect.Y += padding.Top;
            rect.Width -= padding.Horizontal;
            rect.Height -= padding.Vertical;
            return rect;
        }

        public static Rectangle InflateRect(Rectangle rect, Padding padding)
        {
            rect.X -= padding.Left;
            rect.Y -= padding.Top;
            rect.Width += padding.Horizontal;
            rect.Height += padding.Vertical;
            return rect;
        }

        public static Size UnionSizes(Size a, Size b)
        {
            return new Size(Math.Max(a.Width, b.Width), Math.Max(a.Height, b.Height));
        }
    }
}

