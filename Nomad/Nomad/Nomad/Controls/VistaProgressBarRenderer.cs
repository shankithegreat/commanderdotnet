namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class VistaProgressBarRenderer : IProgressBarRenderer
    {
        public Color BackgroundColor;
        private Color Black20;
        private Color Black30;
        private Color Black40;
        private Color Color178;
        private Color FEndColor;
        private Color FHighlightColor;
        private ColorBlend FMarqueeBlend;
        private Color FStartColor;
        private ColorBlend ShadowBlend1;
        private ColorBlend ShadowBlend2;
        private bool StartEndColorEqual;
        private Color White100;
        private Color White128;

        public VistaProgressBarRenderer()
        {
            this.Black20 = Color.FromArgb(20, Color.Black);
            this.Black30 = Color.FromArgb(30, Color.Black);
            this.Black40 = Color.FromArgb(40, Color.Black);
            this.White100 = Color.FromArgb(100, Color.White);
            this.White128 = Color.FromArgb(0x80, Color.White);
            this.Color178 = Color.FromArgb(0xb2, 0xb2, 0xb2);
            this.BackgroundColor = Color.FromArgb(0xc9, 0xc9, 0xc9);
            this.FStartColor = Color.LimeGreen;
            this.FEndColor = Color.LimeGreen;
            this.Initialize();
        }

        public VistaProgressBarRenderer(Color startColor, Color endColor)
        {
            this.Black20 = Color.FromArgb(20, Color.Black);
            this.Black30 = Color.FromArgb(30, Color.Black);
            this.Black40 = Color.FromArgb(40, Color.Black);
            this.White100 = Color.FromArgb(100, Color.White);
            this.White128 = Color.FromArgb(0x80, Color.White);
            this.Color178 = Color.FromArgb(0xb2, 0xb2, 0xb2);
            this.BackgroundColor = Color.FromArgb(0xc9, 0xc9, 0xc9);
            this.FStartColor = startColor;
            this.FEndColor = endColor;
            this.Initialize();
        }

        public void DrawBackground(ProgressBarRenderEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            this.DrawBackground(e.Graphics, e.Bounds);
            this.DrawBackgroundShadows(e.Graphics, e.Bounds);
        }

        private void DrawBackground(Graphics g, Rectangle rect)
        {
            rect.Width--;
            rect.Height--;
            using (GraphicsPath path = RoundRect(rect, 2f, 2f, 2f, 2f))
            {
                using (Brush brush = new SolidBrush(this.BackgroundColor))
                {
                    g.FillPath(brush, path);
                }
            }
        }

        private void DrawBackgroundShadows(Graphics g, Rectangle rect)
        {
            Rectangle rectangle = new Rectangle(rect.Left + 2, rect.Top + 2, 10, rect.Height - 5);
            using (Brush brush = new LinearGradientBrush(rectangle, this.Black30, Color.Transparent, LinearGradientMode.Horizontal))
            {
                rectangle.X--;
                g.FillRectangle(brush, rectangle);
            }
            Rectangle rectangle2 = new Rectangle(rect.Right - 12, rect.Top + 2, 10, rect.Height - 5);
            using (Brush brush2 = new LinearGradientBrush(rectangle2, Color.Transparent, this.Black20, LinearGradientMode.Horizontal))
            {
                g.FillRectangle(brush2, rectangle2);
            }
        }

        private void DrawBar(Graphics g, Rectangle rect, float value)
        {
            Rectangle rectangle = new Rectangle(rect.Left + 1, rect.Top + 2, rect.Width - 3, rect.Height - 3) {
                Width = (int) (value * rect.Width)
            };
            using (Brush brush = new SolidBrush(this.StartEndColorEqual ? this.FStartColor : this.GetIntermediateColor(value)))
            {
                g.FillRectangle(brush, rectangle);
            }
        }

        private void DrawBarHighlight(Graphics g, Rectangle rect)
        {
            Rectangle r = new Rectangle(rect.Left + 1, rect.Top + 1, rect.Width - 1, 6);
            using (GraphicsPath path = RoundRect(r, 2f, 2f, 0f, 0f))
            {
                g.SetClip(path);
                using (Brush brush = new LinearGradientBrush(r, Color.WhiteSmoke, this.White128, LinearGradientMode.Vertical))
                {
                    g.FillPath(brush, path);
                }
                g.ResetClip();
            }
            Rectangle rectangle2 = new Rectangle(rect.Left + 1, rect.Bottom - 8, rect.Width - 1, 6);
            using (GraphicsPath path2 = RoundRect(rectangle2, 0f, 0f, 2f, 2f))
            {
                g.SetClip(path2);
                using (Brush brush2 = new LinearGradientBrush(rectangle2, Color.Transparent, this.FHighlightColor, LinearGradientMode.Vertical))
                {
                    g.FillPath(brush2, path2);
                }
                g.ResetClip();
            }
        }

        private void DrawBarShadows(Graphics g, Rectangle rect, float value)
        {
            Rectangle rectangle = new Rectangle(rect.Left + 1, rect.Top + 2, 15, rect.Height - 3);
            using (LinearGradientBrush brush = new LinearGradientBrush(rectangle, Color.White, Color.White, LinearGradientMode.Horizontal))
            {
                brush.InterpolationColors = this.ShadowBlend1;
                rectangle.X--;
                g.FillRectangle(brush, rectangle);
            }
            Rectangle rectangle2 = new Rectangle(rect.Right - 3, rect.Top + 2, 15, rect.Height - 3) {
                X = ((int) (value * rect.Width)) - 14
            };
            using (LinearGradientBrush brush2 = new LinearGradientBrush(rectangle2, Color.Black, Color.Black, LinearGradientMode.Horizontal))
            {
                brush2.InterpolationColors = this.ShadowBlend2;
                g.FillRectangle(brush2, rectangle2);
            }
        }

        public void DrawBarValue(ProgressBarValueRenderEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            float num = (e.Value * 1f) / ((float) (e.Maximum - e.Minimum));
            this.DrawBar(e.Graphics, e.Bounds, num);
            this.DrawBarShadows(e.Graphics, e.Bounds, num);
            this.DrawBarHighlight(e.Graphics, e.Bounds);
            this.DrawInnerStroke(e.Graphics, e.Bounds);
            this.DrawOuterStroke(e.Graphics, e.Bounds);
        }

        private void DrawInnerStroke(Graphics g, Rectangle rect)
        {
            rect.X++;
            rect.Y++;
            rect.Width -= 3;
            rect.Height -= 3;
            using (GraphicsPath path = RoundRect(rect, 2f, 2f, 2f, 2f))
            {
                using (Pen pen = new Pen(this.White100))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        public void DrawMarquee(ProgressBarMarqueeRenderEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            int position = (e.MarqueeTag is int) ? ((int) e.MarqueeTag) : -120;
            this.DrawMarquee(e.Graphics, e.Bounds, position);
            this.DrawBarHighlight(e.Graphics, e.Bounds);
            this.DrawInnerStroke(e.Graphics, e.Bounds);
            this.DrawOuterStroke(e.Graphics, e.Bounds);
        }

        private void DrawMarquee(Graphics g, Rectangle rect, int position)
        {
            Rectangle rectangle = new Rectangle(rect.Left + position, rect.Top, 100, rect.Height);
            using (LinearGradientBrush brush = new LinearGradientBrush(rectangle, Color.White, Color.White, LinearGradientMode.Horizontal))
            {
                brush.InterpolationColors = this.MarqueeBlend;
                rect.Width--;
                rect.Height--;
                using (GraphicsPath path = RoundRect(rect, 2f, 2f, 2f, 2f))
                {
                    g.SetClip(path);
                    g.FillRectangle(brush, rectangle);
                    g.ResetClip();
                }
            }
        }

        private void DrawOuterStroke(Graphics g, Rectangle rect)
        {
            rect.Width--;
            rect.Height--;
            using (GraphicsPath path = RoundRect(rect, 2f, 2f, 2f, 2f))
            {
                using (Pen pen = new Pen(this.Color178))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private Color GetIntermediateColor(float value)
        {
            int a = this.FStartColor.A;
            int r = this.FStartColor.R;
            int g = this.FStartColor.G;
            int b = this.FStartColor.B;
            int num5 = this.FEndColor.A;
            int num6 = this.FEndColor.R;
            int num7 = this.FEndColor.G;
            int num8 = this.FEndColor.B;
            int num9 = (int) Math.Abs((float) (a + ((a - num5) * value)));
            int num10 = (int) Math.Abs((float) (r - ((r - num6) * value)));
            int num11 = (int) Math.Abs((float) (g - ((g - num7) * value)));
            int num12 = (int) Math.Abs((float) (b - ((b - num8) * value)));
            num9 = Math.Min(0xff, num9);
            num10 = Math.Min(0xff, num10);
            num11 = Math.Min(0xff, num11);
            num12 = Math.Min(0xff, num12);
            return Color.FromArgb(num9, num10, num11, num12);
        }

        private void Initialize()
        {
            this.HighlightColor = Color.White;
            this.ShadowBlend1 = new ColorBlend(3);
            Color[] colorArray = new Color[] { Color.Transparent, this.Black40, Color.Transparent };
            this.ShadowBlend1.Colors = colorArray;
            float[] numArray = new float[3];
            numArray[1] = 0.2f;
            numArray[2] = 1f;
            this.ShadowBlend1.Positions = numArray;
            this.ShadowBlend2 = new ColorBlend(3);
            colorArray = new Color[] { Color.Transparent, this.Black40, Color.Transparent };
            this.ShadowBlend2.Colors = colorArray;
            numArray = new float[3];
            numArray[1] = 0.8f;
            numArray[2] = 1f;
            this.ShadowBlend2.Positions = numArray;
            this.StartEndColorEqual = this.FStartColor == this.FEndColor;
        }

        private static GraphicsPath RoundRect(RectangleF r, float r1, float r2, float r3, float r4)
        {
            float x = r.X;
            float y = r.Y;
            float width = r.Width;
            float height = r.Height;
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

        public bool UpdateMarquee(ProgressBarMarqueeEventArgs e)
        {
            if (e.MarqueeTag is int)
            {
                int marqueeTag = (int) e.MarqueeTag;
                marqueeTag += 4;
                if (marqueeTag > e.Bounds.Width)
                {
                    marqueeTag = -120;
                }
                e.MarqueeTag = marqueeTag;
            }
            else
            {
                e.MarqueeTag = -120;
            }
            return true;
        }

        public Color EndColor
        {
            get
            {
                return this.FEndColor;
            }
            set
            {
                this.FEndColor = value;
                this.FMarqueeBlend = null;
                this.StartEndColorEqual = this.FStartColor == this.FEndColor;
            }
        }

        public Color HighlightColor
        {
            get
            {
                return Color.FromArgb(0xff, this.FHighlightColor);
            }
            set
            {
                this.FHighlightColor = Color.FromArgb(100, value);
            }
        }

        private ColorBlend MarqueeBlend
        {
            get
            {
                if (this.FMarqueeBlend == null)
                {
                    Color color = Color.FromArgb((this.FStartColor.R + this.FEndColor.R) / 2, (this.FStartColor.G + this.FEndColor.G) / 2, (this.FStartColor.B + this.FEndColor.B) / 2);
                    this.FMarqueeBlend = new ColorBlend(3);
                    this.FMarqueeBlend.Colors = new Color[] { Color.Transparent, color, Color.Transparent };
                    float[] numArray = new float[3];
                    numArray[1] = 0.5f;
                    numArray[2] = 1f;
                    this.FMarqueeBlend.Positions = numArray;
                }
                return this.FMarqueeBlend;
            }
        }

        public Color StartColor
        {
            get
            {
                return this.FStartColor;
            }
            set
            {
                this.FStartColor = value;
                this.FMarqueeBlend = null;
                this.StartEndColorEqual = this.FStartColor == this.FEndColor;
            }
        }
    }
}

