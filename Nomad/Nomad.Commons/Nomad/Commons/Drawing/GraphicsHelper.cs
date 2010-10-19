namespace Nomad.Commons.Drawing
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public static class GraphicsHelper
    {
        public static GraphicsPath RoundRect(Rectangle rect, float radius)
        {
            return RoundRect(rect, radius, radius, radius, radius);
        }

        public static GraphicsPath RoundRect(RectangleF rect, float radius)
        {
            return RoundRect(rect, radius, radius, radius, radius);
        }

        public static GraphicsPath RoundRect(Rectangle rect, float r1, float r2, float r3, float r4)
        {
            int x = rect.X;
            int y = rect.Y;
            int width = rect.Width;
            int height = rect.Height;
            GraphicsPath path = new GraphicsPath();
            path.AddBezier((float) x, y + r1, (float) x, (float) y, x + r1, (float) y, x + r1, (float) y);
            path.AddLine(x + r1, (float) y, (x + width) - r2, (float) y);
            path.AddBezier((x + width) - r2, (float) y, (float) (x + width), (float) y, (float) (x + width), y + r2, (float) (x + width), y + r2);
            path.AddLine((float) (x + width), y + r2, (float) (x + width), (y + height) - r3);
            path.AddBezier((float) (x + width), (y + height) - r3, (float) (x + width), (float) (y + height), (x + width) - r3, (float) (y + height), (x + width) - r3, (float) (y + height));
            path.AddLine((x + width) - r3, (float) (y + height), x + r4, (float) (y + height));
            path.AddBezier(x + r4, (float) (y + height), (float) x, (float) (y + height), (float) x, (y + height) - r4, (float) x, (y + height) - r4);
            path.AddLine((float) x, (y + height) - r4, (float) x, y + r1);
            return path;
        }

        public static GraphicsPath RoundRect(RectangleF rect, float r1, float r2, float r3, float r4)
        {
            float x = rect.X;
            float y = rect.Y;
            float width = rect.Width;
            float height = rect.Height;
            GraphicsPath path = new GraphicsPath();
            path.AddBezier(x, y + r1, x, y, x + r1, y, x + r1, y);
            path.AddLine(x + r1, y, (x + width) - r2, y);
            path.AddBezier((x + width) - r2, y, x + width, y, x + width, y + r2, x + width, y + r2);
            path.AddLine((float) (x + width), (float) (y + r2), (float) (x + width), (float) ((y + height) - r3));
            path.AddBezier((float) (x + width), (float) ((y + height) - r3), (float) (x + width), (float) (y + height), (float) ((x + width) - r3), (float) (y + height), (float) ((x + width) - r3), (float) (y + height));
            path.AddLine((float) ((x + width) - r3), (float) (y + height), (float) (x + r4), (float) (y + height));
            path.AddBezier(x + r4, y + height, x, y + height, x, (y + height) - r4, x, (y + height) - r4);
            path.AddLine(x, (y + height) - r4, x, y + r1);
            return path;
        }
    }
}

