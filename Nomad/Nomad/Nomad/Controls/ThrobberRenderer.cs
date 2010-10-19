namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class ThrobberRenderer
    {
        private System.Drawing.Color FColor = System.Drawing.Color.Gray;
        private int FInnerCircleRadius = 8;
        private const int FireFoxInnerCircleRadius = 6;
        private const int FireFoxNumberOfSpoke = 9;
        private const int FireFoxOuterCircleRadius = 7;
        private const int FireFoxSpokeThickness = 4;
        private int FNumberOfSpoke = 10;
        private int FOuterCircleRadius = 10;
        private System.Drawing.Color[] FPalette;
        private double[] FSpokeAngles;
        private int FSpokeThickness = 4;
        private ThrobberStyle FStyle;
        private const int IE7InnerCircleRadius = 8;
        private const int IE7NumberOfSpoke = 0x18;
        private const int IE7OuterCircleRadius = 9;
        private const int IE7SpokeThickness = 4;
        private const int MacOSXInnerCircleRadius = 5;
        private const int MacOSXNumberOfSpoke = 12;
        private const int MacOSXOuterCircleRadius = 11;
        private const int MacOSXSpokeThickness = 2;
        private const double NumberOfDegreesInCircle = 360.0;
        private const double NumberOfDegreesInHalfCircle = 180.0;

        public ThrobberRenderer()
        {
            this.GeneratePallete();
            this.GenerateSpokeAngles();
        }

        private static System.Drawing.Color Darken(System.Drawing.Color color, int percent)
        {
            int r = color.R;
            int g = color.G;
            int b = color.B;
            return System.Drawing.Color.FromArgb(percent, Math.Min(r, 0xff), Math.Min(g, 0xff), Math.Min(b, 0xff));
        }

        private static void DrawLine(Graphics graphics, PointF pt1, PointF pt2, System.Drawing.Color color, int lineThickness)
        {
            using (Pen pen = new Pen(color, (float) lineThickness))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                graphics.DrawLine(pen, pt1, pt2);
            }
        }

        public void DrawThrobber(ThrobberRenderEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            PointF center = new PointF((float) (e.Bounds.Left + (e.Bounds.Width / 2)), (float) ((e.Bounds.Top + (e.Bounds.Height / 2)) - 1));
            for (int i = 0; i < this.FNumberOfSpoke; i++)
            {
                e.Position = e.Position % this.FNumberOfSpoke;
                DrawLine(e.Graphics, GetCoordinate(center, this.FInnerCircleRadius, this.FSpokeAngles[e.Position]), GetCoordinate(center, this.FOuterCircleRadius, this.FSpokeAngles[e.Position]), e.Enabled ? this.FPalette[i] : this.FColor, this.FSpokeThickness);
                e.Position++;
            }
        }

        private void GeneratePallete()
        {
            this.FPalette = this.GeneratePallete(this.FColor, this.FNumberOfSpoke);
        }

        private System.Drawing.Color[] GeneratePallete(System.Drawing.Color color, int spokeNumber)
        {
            System.Drawing.Color[] colorArray = new System.Drawing.Color[this.NumberOfSpoke];
            byte num = (byte) (0xff / this.NumberOfSpoke);
            byte percent = 0;
            for (int i = 0; i < this.NumberOfSpoke; i++)
            {
                if ((i == 0) || (i < (this.NumberOfSpoke - spokeNumber)))
                {
                    colorArray[i] = color;
                }
                else
                {
                    percent = (byte) (percent + num);
                    if (percent > 0xff)
                    {
                        percent = 0xff;
                    }
                    colorArray[i] = Darken(color, percent);
                }
            }
            return colorArray;
        }

        private void GenerateSpokeAngles()
        {
            this.FSpokeAngles = GenerateSpokeAngles(this.NumberOfSpoke);
        }

        private static double[] GenerateSpokeAngles(int numberOfSpoke)
        {
            double[] numArray = new double[numberOfSpoke];
            double num = 360.0 / ((double) numberOfSpoke);
            for (int i = 0; i < numberOfSpoke; i++)
            {
                numArray[i] = (i == 0) ? num : (numArray[i - 1] + num);
            }
            return numArray;
        }

        private static PointF GetCoordinate(PointF center, int radius, double angle)
        {
            double d = (3.1415926535897931 * angle) / 180.0;
            return new PointF(center.X + (radius * ((float) Math.Cos(d))), center.Y + (radius * ((float) Math.Sin(d))));
        }

        public Size GetPreferredSize(Size proposedSize)
        {
            proposedSize.Width = (this.FOuterCircleRadius + this.FSpokeThickness) * 2;
            proposedSize.Height = proposedSize.Width;
            return proposedSize;
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this.FColor;
            }
            set
            {
                if (!(this.FColor == value))
                {
                    this.FColor = value;
                    this.GeneratePallete();
                }
            }
        }

        public int InnerCircleRadius
        {
            get
            {
                return this.FInnerCircleRadius;
            }
            set
            {
                if ((this.FInnerCircleRadius != value) && (value >= 1))
                {
                    this.FInnerCircleRadius = value;
                    this.FStyle = ThrobberStyle.Custom;
                }
            }
        }

        public int NumberOfSpoke
        {
            get
            {
                return this.FNumberOfSpoke;
            }
            set
            {
                if ((this.FNumberOfSpoke != value) && (value >= 1))
                {
                    this.FNumberOfSpoke = value;
                    this.FStyle = ThrobberStyle.Custom;
                    this.GeneratePallete();
                    this.GenerateSpokeAngles();
                }
            }
        }

        public int OuterCircleRadius
        {
            get
            {
                return this.FOuterCircleRadius;
            }
            set
            {
                if ((this.FOuterCircleRadius != value) && (value >= 1))
                {
                    this.FOuterCircleRadius = value;
                    this.FStyle = ThrobberStyle.Custom;
                }
            }
        }

        public int SpokeThickness
        {
            get
            {
                return this.FSpokeThickness;
            }
            set
            {
                if ((this.FSpokeThickness != value) && (value >= 1))
                {
                    this.FSpokeThickness = value;
                    this.FStyle = ThrobberStyle.Custom;
                }
            }
        }

        public ThrobberStyle Style
        {
            get
            {
                return this.FStyle;
            }
            set
            {
                if (this.FStyle != value)
                {
                    this.FStyle = value;
                    switch (this.FStyle)
                    {
                        case ThrobberStyle.MacOSX:
                            this.FInnerCircleRadius = 5;
                            this.FOuterCircleRadius = 11;
                            this.FNumberOfSpoke = 12;
                            this.FSpokeThickness = 2;
                            break;

                        case ThrobberStyle.Firefox:
                            this.FInnerCircleRadius = 6;
                            this.FOuterCircleRadius = 7;
                            this.FNumberOfSpoke = 9;
                            this.FSpokeThickness = 4;
                            break;

                        case ThrobberStyle.IE7:
                            this.FInnerCircleRadius = 8;
                            this.FOuterCircleRadius = 9;
                            this.FNumberOfSpoke = 0x18;
                            this.FSpokeThickness = 4;
                            break;
                    }
                    this.GeneratePallete();
                    this.GenerateSpokeAngles();
                }
            }
        }
    }
}

