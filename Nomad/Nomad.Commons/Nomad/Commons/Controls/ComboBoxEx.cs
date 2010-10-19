namespace Nomad.Commons.Controls
{
    using Microsoft;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class ComboBoxEx : System.Windows.Forms.ComboBox
    {
        private PushButtonState _ButtonState;
        private bool SkipDrawItem;

        private void AdjustSize()
        {
            if (this.AutoSize && (this.DropDownStyle == ComboBoxStyle.DropDownList))
            {
                base.Size = base.PreferredSize;
            }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize = base.GetPreferredSize(proposedSize);
            int num = 0;
            if (base.Items.Count == 0)
            {
                num = 80;
            }
            else
            {
                using (Graphics graphics = Graphics.FromHwndInternal(base.Handle))
                {
                    for (int i = 0; i < base.Items.Count; i++)
                    {
                        if (this.DrawMode == System.Windows.Forms.DrawMode.Normal)
                        {
                            Size size2 = TextRenderer.MeasureText(graphics, base.GetItemText(base.Items[i]), this.Font);
                            num = Math.Max(num, size2.Width);
                        }
                        else
                        {
                            MeasureItemEventArgs e = new MeasureItemEventArgs(graphics, i);
                            this.OnMeasureItem(e);
                            num = Math.Max(num, e.ItemWidth);
                        }
                    }
                }
            }
            preferredSize.Width = Math.Max(this.MinimumSize.Width, (num + (SystemInformation.Border3DSize.Width * 2)) + SystemInformation.VerticalScrollBarWidth);
            if (this.MaximumSize.Width > 0)
            {
                preferredSize.Width = Math.Min(this.MaximumSize.Width, preferredSize.Width);
            }
            return preferredSize;
        }

        protected override void OnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);
            this.AdjustSize();
        }

        protected override void OnDisplayMemberChanged(EventArgs e)
        {
            base.OnDisplayMemberChanged(e);
            this.AdjustSize();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (this.SkipDrawItem)
            {
                base.Invalidate(this.ItemClientRectangle);
            }
            else
            {
                base.OnDrawItem(e);
            }
        }

        protected override void OnDropDown(EventArgs e)
        {
            this.ButtonState = PushButtonState.Pressed;
            base.OnDropDown(e);
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            this.ButtonState = base.ClientRectangle.Contains(base.PointToClient(Cursor.Position)) ? PushButtonState.Hot : PushButtonState.Normal;
            base.OnDropDownClosed(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            this.SkipDrawItem = this.ExtendedPainting;
            base.OnEnter(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.AdjustSize();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.ButtonState = PushButtonState.Hot;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.ButtonState = PushButtonState.Normal;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
            {
                this.ButtonState = base.ClientRectangle.Contains(e.Location) ? PushButtonState.Hot : PushButtonState.Normal;
            }
            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.ExtendedPainting)
            {
                if (VisualStyleRenderer.IsSupported)
                {
                    PushButtonState state = base.Enabled ? (base.DroppedDown ? PushButtonState.Pressed : this.ButtonState) : PushButtonState.Disabled;
                    Rectangle bounds = new Rectangle(base.ClientRectangle.Left, base.ClientRectangle.Top, base.ClientRectangle.Width, base.ClientRectangle.Height + 1);
                    if (ButtonRenderer.IsBackgroundPartiallyTransparent(state))
                    {
                        ButtonRenderer.DrawParentBackground(e.Graphics, bounds, this);
                    }
                    ButtonRenderer.DrawButton(e.Graphics, bounds, state);
                    int x = bounds.Right - 6;
                    Point point = new Point(x - 7, (bounds.Y + (bounds.Height / 2)) - 2);
                    Point point2 = new Point(x, (bounds.Y + (bounds.Height / 2)) - 2);
                    Point point3 = new Point(x - 4, (bounds.Y + (bounds.Height / 2)) + 2);
                    Point[] points = new Point[] { point, point2, point3 };
                    e.Graphics.FillPolygon(SystemBrushes.ControlText, points);
                    DrawItemState state2 = DrawItemState.Checked | DrawItemState.Selected;
                    if (!(!this.Focused || base.DroppedDown))
                    {
                        state2 |= DrawItemState.Focus;
                    }
                    if (!base.Enabled)
                    {
                        state2 |= DrawItemState.Disabled;
                    }
                    if (state == PushButtonState.Hot)
                    {
                        state2 |= DrawItemState.HotLight;
                    }
                    this.SkipDrawItem = false;
                    this.OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font, this.ItemClientRectangle, this.SelectedIndex, state2));
                }
                else
                {
                    base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, false);
                }
            }
            base.OnPaint(e);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            this.AdjustSize();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x201)
            {
                this.ButtonState = PushButtonState.Pressed;
                if (this.ExtendedPainting)
                {
                    base.Update();
                }
            }
            base.WndProc(ref m);
        }

        [EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                if (base.AutoSize != value)
                {
                    base.AutoSize = value;
                    this.AdjustSize();
                }
            }
        }

        private PushButtonState ButtonState
        {
            get
            {
                return this._ButtonState;
            }
            set
            {
                if (this._ButtonState != value)
                {
                    this._ButtonState = value;
                    if (this.ExtendedPainting)
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        public System.Windows.Forms.DrawMode DrawMode
        {
            get
            {
                return base.DrawMode;
            }
            set
            {
                if (base.DrawMode != value)
                {
                    base.DrawMode = value;
                    if (OS.IsWinVista)
                    {
                        base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, ((this.DropDownStyle == ComboBoxStyle.DropDownList) && (this.DrawMode != System.Windows.Forms.DrawMode.Normal)) && VisualStyleRenderer.IsSupported);
                        base.Invalidate();
                    }
                }
            }
        }

        public ComboBoxStyle DropDownStyle
        {
            get
            {
                return base.DropDownStyle;
            }
            set
            {
                if (base.DropDownStyle != value)
                {
                    base.DropDownStyle = value;
                    if (OS.IsWinVista)
                    {
                        base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, ((this.DropDownStyle == ComboBoxStyle.DropDownList) && (this.DrawMode != System.Windows.Forms.DrawMode.Normal)) && VisualStyleRenderer.IsSupported);
                        base.Invalidate();
                    }
                }
            }
        }

        private bool ExtendedPainting
        {
            get
            {
                return base.GetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint);
            }
        }

        private Rectangle ItemClientRectangle
        {
            get
            {
                return Rectangle.FromLTRB(base.ClientRectangle.Left + 3, base.ClientRectangle.Top + 3, (base.ClientRectangle.Right - SystemInformation.VerticalScrollBarWidth) - 3, base.ClientRectangle.Bottom - 3);
            }
        }
    }
}

