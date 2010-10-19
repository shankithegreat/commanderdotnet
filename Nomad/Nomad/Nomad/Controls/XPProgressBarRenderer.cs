namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class XPProgressBarRenderer : IProgressBarRenderer
    {
        public Color BarBackgroundColor;
        private Blend BarBlend;
        public Color BarColor;
        public int ChunkWidth;
        public int MarqueeChunks;
        public Nomad.Controls.MarqueeStyle MarqueeStyle;

        public XPProgressBarRenderer() : this(Color.LimeGreen, 8)
        {
        }

        public XPProgressBarRenderer(Color barColor) : this(barColor, 8)
        {
        }

        public XPProgressBarRenderer(Color barColor, int chunkWidth)
        {
            this.BarBackgroundColor = Color.White;
            this.MarqueeChunks = 4;
            this.MarqueeStyle = Nomad.Controls.MarqueeStyle.LeftRight;
            this.BarColor = barColor;
            this.ChunkWidth = chunkWidth;
            float[] numArray = new float[] { 0.1f, 1f, 1f, 1f, 1f, 0.85f, 0.1f };
            float[] numArray2 = new float[] { 0f, 0.2f, 0.5f, 0.5f, 0.5f, 0.8f, 1f };
            this.BarBlend = new Blend();
            this.BarBlend.Factors = numArray;
            this.BarBlend.Positions = numArray2;
        }

        public void DrawBackground(ProgressBarRenderEventArgs e)
        {
            Point[] points = new Point[] { new Point(e.Bounds.Left + 3, e.Bounds.Top + 1), new Point(e.Bounds.Right - 3, e.Bounds.Top + 1), new Point(e.Bounds.Right - 2, e.Bounds.Top + 2), new Point(e.Bounds.Right - 1, e.Bounds.Top + 3), new Point(e.Bounds.Right - 1, e.Bounds.Bottom - 4), new Point(e.Bounds.Right - 2, e.Bounds.Bottom - 3), new Point(e.Bounds.Right - 3, e.Bounds.Bottom - 2), new Point(e.Bounds.Left + 3, e.Bounds.Bottom - 1), new Point(e.Bounds.Left + 2, e.Bounds.Bottom - 3), new Point(e.Bounds.Left + 1, e.Bounds.Bottom - 4), new Point(e.Bounds.Left + 1, e.Bounds.Top + 3), new Point(e.Bounds.Left + 2, e.Bounds.Top + 2) };
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddLines(points);
                using (Brush brush = new SolidBrush(this.BarBackgroundColor))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }
            DrawBorder(e.Graphics, e.Bounds);
        }

        public void DrawBarValue(ProgressBarValueRenderEventArgs e)
        {
            if ((e.Maximum != e.Minimum) && (e.Value != 0))
            {
                int num = e.Bounds.Width - 7;
                int width = (num * e.Value) / (e.Maximum - e.Minimum);
                if (width != 0)
                {
                    if ((e.ProgressBar.Style == ProgressBarStyle.Blocks) && ((width % this.ChunkWidth) != 0))
                    {
                        int num3 = width % this.ChunkWidth;
                        width += this.ChunkWidth - num3;
                        if (width > num)
                        {
                            width = num;
                        }
                    }
                    Rectangle rect = new Rectangle(e.Bounds.Left + 3, e.Bounds.Top + 2, width, e.Bounds.Bottom - 4);
                    using (LinearGradientBrush brush = new LinearGradientBrush(rect, this.BarBackgroundColor, this.BarColor, 90f))
                    {
                        brush.Blend = this.BarBlend;
                        e.Graphics.FillRectangle(brush, rect);
                    }
                    if (e.ProgressBar.Style == ProgressBarStyle.Blocks)
                    {
                        int num4 = width / this.ChunkWidth;
                        using (Pen pen = new Pen(this.BarBackgroundColor, 2f))
                        {
                            for (int i = 0; i <= num4; i++)
                            {
                                e.Graphics.DrawLine(pen, (int) ((e.Bounds.Left + (this.ChunkWidth * i)) + 3), (int) (e.Bounds.Top + 2), (int) ((e.Bounds.Left + (this.ChunkWidth * i)) + 3), (int) (e.Bounds.Bottom - 2));
                            }
                        }
                    }
                }
            }
        }

        private static void DrawBorder(Graphics g, Rectangle rect)
        {
            Point[] pointArray3 = new Point[] { new Point(rect.Left + 1, rect.Top + 2), new Point(rect.Left + 2, rect.Top + 1), new Point(rect.Left + 3, rect.Top), new Point(rect.Right - 4, rect.Top), new Point(rect.Right - 3, rect.Top + 1), new Point(rect.Right - 2, rect.Top + 2), new Point(rect.Right - 2, rect.Bottom - 3), new Point(rect.Right - 3, rect.Bottom - 2), new Point(rect.Right - 4, rect.Bottom - 1), new Point(rect.Left + 3, rect.Bottom - 1), new Point(rect.Left + 2, rect.Bottom - 2), new Point(rect.Left + 1, rect.Bottom - 3), new Point(rect.Left + 1, 2) };
            Point[] points = pointArray3;
            pointArray3 = new Point[] { new Point(rect.Left + 2, rect.Top + 2), new Point(rect.Left + 3, rect.Top + 1), new Point(rect.Left + 4, rect.Top + 1), new Point(rect.Right - 5, rect.Top + 1), new Point(rect.Right - 4, rect.Top + 1), new Point(rect.Right - 3, rect.Top + 2) };
            Point[] pointArray2 = pointArray3;
            g.DrawCurve(Pens.Gray, points, 0f);
            g.DrawCurve(Pens.LightGray, pointArray2, 0f);
            g.DrawLine(Pens.LightGray, rect.Left + 2, rect.Top + 2, 2, rect.Bottom - 3);
        }

        public void DrawMarquee(ProgressBarMarqueeRenderEventArgs e)
        {
            int num2;
            int num3;
            int num = e.Bounds.Width - 7;
            MarqueeData marqueeData = this.GetMarqueeData(e.MarqueeTag);
            if (marqueeData.StartChunk < 0)
            {
                num2 = 0;
                num3 = (this.MarqueeChunks + Math.Max(-this.MarqueeChunks, marqueeData.StartChunk)) * this.ChunkWidth;
            }
            else
            {
                num2 = marqueeData.StartChunk * this.ChunkWidth;
                num3 = this.ChunkWidth * this.MarqueeChunks;
            }
            if ((num2 + num3) > num)
            {
                num3 = num - num2;
            }
            Rectangle rect = new Rectangle((e.Bounds.Left + 3) + num2, e.Bounds.Top + 2, num3, e.Bounds.Bottom - 4);
            if (rect.Width > 0)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(rect, this.BarBackgroundColor, this.BarColor, 90f))
                {
                    brush.Blend = this.BarBlend;
                    e.Graphics.FillRectangle(brush, rect);
                }
                using (Pen pen = new Pen(this.BarBackgroundColor, 2f))
                {
                    for (int i = 0; (i < this.MarqueeChunks) && ((num2 + (this.ChunkWidth * i)) < num); i++)
                    {
                        int num5 = ((e.Bounds.Left + num2) + (this.ChunkWidth * i)) + 3;
                        e.Graphics.DrawLine(pen, num5, e.Bounds.Top + 2, num5, e.Bounds.Bottom - 2);
                    }
                }
            }
        }

        private MarqueeData GetMarqueeData(object marqueeTag)
        {
            if (marqueeTag is MarqueeData)
            {
                return (MarqueeData) marqueeTag;
            }
            return new MarqueeData { StartChunk = -this.MarqueeChunks };
        }

        public bool UpdateMarquee(ProgressBarMarqueeEventArgs e)
        {
            MarqueeData marqueeData = this.GetMarqueeData(e.MarqueeTag);
            int num = (e.Bounds.Width - 7) / this.ChunkWidth;
            if (this.MarqueeStyle == Nomad.Controls.MarqueeStyle.LeftRight)
            {
                marqueeData.StartChunk += marqueeData.Reverse ? -1 : 1;
                if (marqueeData.StartChunk > num)
                {
                    marqueeData.Reverse = true;
                }
                else if (marqueeData.StartChunk < -this.MarqueeChunks)
                {
                    marqueeData.Reverse = false;
                }
            }
            else
            {
                marqueeData.StartChunk++;
                if (marqueeData.StartChunk > num)
                {
                    marqueeData.StartChunk = -this.MarqueeChunks;
                }
            }
            e.MarqueeTag = marqueeData;
            return true;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MarqueeData
        {
            public int StartChunk;
            public bool Reverse;
        }
    }
}

