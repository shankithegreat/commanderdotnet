namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    [DesignerCategory("Code")]
    public class GradientPanel : Panel
    {
        private Color FStartColor = Color.Black;

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.SetStyle(ControlStyles.Opaque, (this.FStartColor != Color.Transparent) && (this.BackColor != Color.Transparent));
            base.OnBackColorChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush brush = new LinearGradientBrush(base.ClientRectangle, this.FStartColor, this.BackColor, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, base.ClientRectangle);
            }
            base.OnPaint(e);
        }

        [DefaultValue(typeof(Color), "Black"), Category("Appearance")]
        public Color StartColor
        {
            get
            {
                return this.FStartColor;
            }
            set
            {
                if (!(this.FStartColor == value))
                {
                    this.FStartColor = value;
                    base.SetStyle(ControlStyles.Opaque, (this.FStartColor != Color.Transparent) && (this.BackColor != Color.Transparent));
                    base.Invalidate();
                }
            }
        }
    }
}

