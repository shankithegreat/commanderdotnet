namespace Nomad.Commons.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Bevel : Control
    {
        private Border3DSide FSides = Border3DSide.All;
        private Border3DStyle FStyle = Border3DStyle.Etched;

        public Bevel()
        {
            base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.Selectable, false);
            base.AutoSize = true;
            base.CausesValidation = false;
            base.TabStop = false;
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize = base.GetPreferredSize(proposedSize);
            switch (this.FSides)
            {
                case Border3DSide.Left:
                case Border3DSide.Right:
                    preferredSize.Width = this.BorderThickness + base.Padding.Horizontal;
                    return preferredSize;

                case Border3DSide.Top:
                case Border3DSide.Bottom:
                    preferredSize.Height = this.BorderThickness + base.Padding.Vertical;
                    return preferredSize;

                case (Border3DSide.Top | Border3DSide.Left):
                    return preferredSize;
            }
            return preferredSize;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle clientRectangle = base.ClientRectangle;
            clientRectangle = Rectangle.FromLTRB(clientRectangle.Left + base.Padding.Left, clientRectangle.Top + base.Padding.Top, clientRectangle.Right - base.Padding.Right, clientRectangle.Bottom - base.Padding.Bottom);
            if (this.FStyle == Border3DStyle.Flat)
            {
                ControlPaint.DrawBorder(e.Graphics, clientRectangle, this.ForeColor, 1, ((this.FSides & Border3DSide.Left) > 0) ? ButtonBorderStyle.Solid : ButtonBorderStyle.None, this.ForeColor, 1, ((this.FSides & Border3DSide.Top) > 0) ? ButtonBorderStyle.Solid : ButtonBorderStyle.None, this.ForeColor, 1, ((this.FSides & Border3DSide.Right) > 0) ? ButtonBorderStyle.Solid : ButtonBorderStyle.None, this.ForeColor, 1, ((this.FSides & Border3DSide.Bottom) > 0) ? ButtonBorderStyle.Solid : ButtonBorderStyle.None);
            }
            else
            {
                ControlPaint.DrawBorder3D(e.Graphics, clientRectangle, this.FStyle, this.FSides);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Localizable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true), Browsable(true)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
            }
        }

        protected int BorderThickness
        {
            get
            {
                return ((this.FStyle == Border3DStyle.Flat) ? 1 : 2);
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CausesValidation
        {
            get
            {
                return base.CausesValidation;
            }
            set
            {
                base.CausesValidation = false;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public System.Windows.Forms.ImeMode ImeMode
        {
            get
            {
                return base.ImeMode;
            }
            set
            {
                base.ImeMode = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override System.Windows.Forms.RightToLeft RightToLeft
        {
            get
            {
                return base.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
            }
        }

        [Category("Appearance"), DefaultValue(0x80f)]
        public Border3DSide Sides
        {
            get
            {
                return this.FSides;
            }
            set
            {
                if (this.FSides != value)
                {
                    this.FSides = value;
                    if (this.AutoSize)
                    {
                        base.Size = base.PreferredSize;
                    }
                    base.Invalidate();
                }
            }
        }

        [Category("Appearance"), DefaultValue(6)]
        public Border3DStyle Style
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
                    if (this.AutoSize)
                    {
                        base.Size = base.PreferredSize;
                    }
                    base.Invalidate();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TabStop
        {
            get
            {
                return base.TabStop;
            }
            set
            {
                base.TabStop = false;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
    }
}

