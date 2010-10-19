namespace Nomad.FileSystem.Property
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class PropertyMeasure
    {
        public readonly int DefaultWidth;
        public readonly string MeasureString;

        public PropertyMeasure(int width)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException("width must be greater than zero.");
            }
            this.DefaultWidth = width;
        }

        public PropertyMeasure(string measureString)
        {
            if (measureString == null)
            {
                throw new ArgumentNullException("measureString");
            }
            if (measureString == string.Empty)
            {
                throw new ArgumentException("measureString is empty");
            }
            this.MeasureString = measureString;
            this.DefaultWidth = -1;
        }

        public int MeasureWidth(Font font)
        {
            if (this.DefaultWidth > 0)
            {
                return this.DefaultWidth;
            }
            return TextRenderer.MeasureText(this.MeasureString, font).Width;
        }
    }
}

