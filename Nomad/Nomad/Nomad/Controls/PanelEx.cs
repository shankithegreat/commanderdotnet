namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    [DesignerCategory("Code")]
    public class PanelEx : Panel
    {
        private Color FBorderColor;
        private AnchorStyles FFlatBorder = (AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top);
        private bool IsBorderColorSet;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.FFlatBorder != AnchorStyles.None)
            {
                Rectangle clientRectangle = base.ClientRectangle;
                using (Pen pen = new Pen(this.BorderColor))
                {
                    if ((this.FFlatBorder & AnchorStyles.Top) > AnchorStyles.None)
                    {
                        e.Graphics.DrawLine(pen, base.ClientRectangle.Left, base.ClientRectangle.Top, base.ClientRectangle.Right - 1, base.ClientRectangle.Top);
                        clientRectangle.X++;
                        clientRectangle.Width--;
                    }
                    if ((this.FFlatBorder & AnchorStyles.Left) > AnchorStyles.None)
                    {
                        e.Graphics.DrawLine(pen, base.ClientRectangle.Left, base.ClientRectangle.Top, base.ClientRectangle.Left, base.ClientRectangle.Bottom - 1);
                        clientRectangle.Y++;
                        clientRectangle.Height--;
                    }
                    if ((this.FFlatBorder & AnchorStyles.Right) > AnchorStyles.None)
                    {
                        e.Graphics.DrawLine(pen, base.ClientRectangle.Right - 1, base.ClientRectangle.Top, base.ClientRectangle.Right - 1, base.ClientRectangle.Bottom - 1);
                        clientRectangle.Width--;
                    }
                    if ((this.FFlatBorder & AnchorStyles.Bottom) > AnchorStyles.None)
                    {
                        e.Graphics.DrawLine(pen, base.ClientRectangle.Left, base.ClientRectangle.Bottom - 1, base.ClientRectangle.Right - 1, base.ClientRectangle.Bottom - 1);
                        clientRectangle.Height--;
                    }
                }
                using (Brush brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillRectangle(brush, clientRectangle);
                }
            }
            base.OnPaint(e);
        }

        public void ResetBorderColor()
        {
            this.IsBorderColorSet = false;
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get
            {
                if (this.IsBorderColorSet)
                {
                    return this.FBorderColor;
                }
                return DefaultBorderColor;
            }
            set
            {
                if (!this.IsBorderColorSet || !(this.FBorderColor == value))
                {
                    this.FBorderColor = value;
                    this.IsBorderColorSet = true;
                    if (this.FlatBorder != AnchorStyles.None)
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        public static Color DefaultBorderColor
        {
            get
            {
                return (VisualStyleRenderer.IsSupported ? VisualStyleInformation.TextControlBorder : Color.DarkGray);
            }
        }

        [Category("Appearance"), DefaultValue(15)]
        public AnchorStyles FlatBorder
        {
            get
            {
                return this.FFlatBorder;
            }
            set
            {
                if (this.FFlatBorder != value)
                {
                    this.FFlatBorder = value;
                    base.SetStyle(ControlStyles.Opaque, this.FFlatBorder != AnchorStyles.None);
                    base.Invalidate();
                }
            }
        }

        private bool ShouldSerializeBorderColor
        {
            get
            {
                return this.IsBorderColorSet;
            }
        }
    }
}

