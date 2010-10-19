namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [DesignerCategory("Code")]
    public class ColorButton : Button
    {
        private Brush ColorBrush;
        private ColorPickerDropDown ColorPicker;
        private System.Drawing.Color FColor;
        private System.Drawing.Color FDefaultColor = System.Drawing.Color.White;

        public event EventHandler ColorChanged;

        private void ColorPickerColorChanged(object sender, EventArgs e)
        {
            this.Color = this.ColorPicker.Color;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.ColorBrush != null)
                {
                    this.ColorBrush.Dispose();
                }
                if (this.ColorPicker != null)
                {
                    this.ColorPicker.Dispose();
                }
            }
            this.ColorBrush = null;
            this.ColorPicker = null;
            base.Dispose(disposing);
        }

        protected override void OnClick(EventArgs e)
        {
            if (this.ColorPicker == null)
            {
                this.ColorPicker = new ColorPickerDropDown();
                this.ColorPicker.ColorChanged += new EventHandler(this.ColorPickerColorChanged);
            }
            this.ColorPicker.Color = this.Color;
            this.ColorPicker.DefaultColor = this.DefaultColor;
            this.ColorPicker.Show(this, 0, base.Height);
        }

        protected void OnColorChanged(EventArgs e)
        {
            if (this.ColorChanged != null)
            {
                this.ColorChanged(this, e);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.ColorBrush == null)
            {
                this.ColorBrush = new SolidBrush(this.Color);
            }
            Rectangle colorRectangle = this.ColorRectangle;
            if (this.Color.IsEmpty)
            {
                colorRectangle.Y = base.ClientRectangle.Top;
                colorRectangle.Height = base.ClientRectangle.Height;
                TextRenderer.DrawText(e.Graphics, Resource.sColorEmpty, this.Font, colorRectangle, base.Enabled ? this.ForeColor : SystemColors.GrayText, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            }
            else
            {
                e.Graphics.FillRectangle(this.ColorBrush, colorRectangle);
                colorRectangle = Rectangle.FromLTRB(colorRectangle.Left - 1, colorRectangle.Top - 1, colorRectangle.Right, colorRectangle.Bottom);
                e.Graphics.DrawRectangle(base.Enabled ? Pens.Black : SystemPens.GrayText, colorRectangle);
            }
            int x = (this.RightToLeft == RightToLeft.Yes) ? ((base.ClientRectangle.Left + base.Padding.Left) + 2) : ((base.ClientRectangle.Right - base.Padding.Right) - 2);
            int num2 = base.ClientRectangle.Top + (base.ClientRectangle.Height / 2);
            Point[] points = new Point[] { new Point(x - 2, num2 - 1), new Point(x + 3, num2 - 1), new Point(x, num2 + 2) };
            e.Graphics.FillPolygon(base.Enabled ? Brushes.Black : SystemBrushes.GrayText, points);
        }

        public void ResetColor()
        {
            this.FColor = System.Drawing.Color.Empty;
        }

        private bool ShouldSerializeColor()
        {
            return !this.FColor.IsEmpty;
        }

        public override string ToString()
        {
            return ("Color = " + this.Color.ToString());
        }

        [Browsable(false), SettingsBindable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool AutoEllipsis
        {
            get
            {
                return base.AutoEllipsis;
            }
            set
            {
                base.AutoEllipsis = value;
            }
        }

        [SettingsBindable(false), Localizable(false), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Bindable(false)]
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

        [Category("Behavior")]
        public System.Drawing.Color Color
        {
            get
            {
                return (this.FColor.IsEmpty ? this.FDefaultColor : this.FColor);
            }
            set
            {
                bool isEmpty = this.Color.IsEmpty;
                if (this.FColor != value)
                {
                    this.FColor = value;
                    if (this.ColorBrush != null)
                    {
                        this.ColorBrush.Dispose();
                        this.ColorBrush = null;
                    }
                    if (isEmpty || this.FColor.IsEmpty)
                    {
                        base.Invalidate();
                    }
                    else
                    {
                        base.Invalidate(this.ColorRectangle);
                    }
                    this.OnColorChanged(EventArgs.Empty);
                }
            }
        }

        protected Rectangle ColorRectangle
        {
            get
            {
                if (this.RightToLeft == RightToLeft.Yes)
                {
                    return Rectangle.FromLTRB((base.ClientRectangle.Left + base.Padding.Left) + 9, base.ClientRectangle.Top + base.Padding.Top, base.ClientRectangle.Right - base.Padding.Right, base.ClientRectangle.Bottom - base.Padding.Bottom);
                }
                return Rectangle.FromLTRB(base.ClientRectangle.Left + base.Padding.Left, base.ClientRectangle.Top + base.Padding.Top, (base.ClientRectangle.Right - base.Padding.Right) - 9, base.ClientRectangle.Bottom - base.Padding.Bottom);
            }
        }

        [Category("Behavior"), DefaultValue(typeof(System.Drawing.Color), "White")]
        public System.Drawing.Color DefaultColor
        {
            get
            {
                return this.FDefaultColor;
            }
            set
            {
                this.FDefaultColor = value;
            }
        }

        protected override Padding DefaultPadding
        {
            get
            {
                return new Padding(9, 7, 9, 7);
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Bindable(false), SettingsBindable(false), Localizable(false)]
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

        [EditorBrowsable(EditorBrowsableState.Never), Bindable(false), SettingsBindable(false), Browsable(false)]
        public override System.Drawing.Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        [Localizable(true), Bindable(false), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public System.Drawing.Image Image
        {
            get
            {
                return base.Image;
            }
            set
            {
                base.Image = value;
            }
        }

        [Localizable(true), EditorBrowsable(EditorBrowsableState.Never), Bindable(false), SettingsBindable(false), Browsable(false)]
        public ContentAlignment ImageAlign
        {
            get
            {
                return base.ImageAlign;
            }
            set
            {
                base.ImageAlign = value;
            }
        }

        [Browsable(false), Localizable(true), EditorBrowsable(EditorBrowsableState.Never), Bindable(false), SettingsBindable(false)]
        public int ImageIndex
        {
            get
            {
                return base.ImageIndex;
            }
            set
            {
                base.ImageIndex = value;
            }
        }

        [Bindable(false), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), SettingsBindable(false), Localizable(true)]
        public string ImageKey
        {
            get
            {
                return base.ImageKey;
            }
            set
            {
                base.ImageKey = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Bindable(false), Browsable(false)]
        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                return base.ImageList;
            }
            set
            {
                base.ImageList = value;
            }
        }

        [Bindable(false), SettingsBindable(false), Browsable(false), Localizable(false), EditorBrowsable(EditorBrowsableState.Never)]
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

        [EditorBrowsable(EditorBrowsableState.Never), Localizable(false), SettingsBindable(false), Browsable(false), Bindable(false)]
        public override ContentAlignment TextAlign
        {
            get
            {
                return base.TextAlign;
            }
            set
            {
                base.TextAlign = value;
            }
        }

        [SettingsBindable(false), EditorBrowsable(EditorBrowsableState.Never), Bindable(false), Browsable(false), Localizable(false)]
        public System.Windows.Forms.TextImageRelation TextImageRelation
        {
            get
            {
                return base.TextImageRelation;
            }
            set
            {
                base.TextImageRelation = value;
            }
        }

        [Bindable(false), SettingsBindable(false), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public bool UseCompatibleTextRendering
        {
            get
            {
                return base.UseCompatibleTextRendering;
            }
            set
            {
                base.UseCompatibleTextRendering = value;
            }
        }

        [Browsable(false), SettingsBindable(false), EditorBrowsable(EditorBrowsableState.Never), Bindable(false)]
        public bool UseMnemonic
        {
            get
            {
                return base.UseMnemonic;
            }
            set
            {
                base.UseMnemonic = value;
            }
        }
    }
}

