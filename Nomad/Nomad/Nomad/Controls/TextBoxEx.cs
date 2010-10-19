namespace Nomad.Controls
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class TextBoxEx : System.Windows.Forms.TextBox
    {
        private Rectangle CapsRect;
        private bool CurrentCapsState;
        private IntPtr CurrentInputLayout;
        private System.Windows.Forms.Padding FPadding;
        private bool FShowCapsLock;
        private bool FShowInputLanguage;
        private Rectangle InputLayoutRect;

        private void CalculateDrawRects()
        {
            int top = VisualStyleRenderer.IsSupported ? 0 : 1;
            if (this.ShowInputLanguage && (InputLanguage.InstalledInputLanguages.Count > 1))
            {
                Size size = TextRenderer.MeasureText(InputLanguage.CurrentInputLanguage.LayoutName, this.Font);
                this.InputLayoutRect = Rectangle.FromLTRB(base.ClientSize.Width - size.Width, top, (base.ClientSize.Width - 1) - top, (base.ClientSize.Height - 1) - top);
            }
            else
            {
                this.InputLayoutRect = Rectangle.Empty;
            }
            if (this.ShowCapsLock)
            {
                Size size2 = TextRenderer.MeasureText("A", this.Font);
                if (this.InputLayoutRect.IsEmpty)
                {
                    this.CapsRect = Rectangle.FromLTRB(base.ClientSize.Width - size2.Width, top, (base.ClientSize.Width - 1) - top, (base.ClientSize.Height - 1) - top);
                }
                else
                {
                    this.CapsRect = Rectangle.FromLTRB((this.InputLayoutRect.Left - size2.Width) - 1, top, this.InputLayoutRect.Left - 2, (base.ClientSize.Height - 1) - top);
                }
            }
            else
            {
                this.CapsRect = Rectangle.Empty;
            }
        }

        private void DrawTextRect(Graphics g, string text, ref Rectangle drawRect)
        {
            g.FillRectangle(SystemBrushes.Info, drawRect);
            Rectangle bounds = Rectangle.FromLTRB(drawRect.Left + 2, drawRect.Top, drawRect.Right, drawRect.Bottom);
            TextRenderer.DrawText(g, text, this.Font, bounds, SystemColors.InfoText, SystemColors.Info, TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            if (VisualStyleRenderer.IsSupported)
            {
                using (Pen pen = new Pen(VisualStyleInformation.TextControlBorder))
                {
                    g.DrawRectangle(pen, drawRect);
                }
            }
            else
            {
                g.DrawRectangle(SystemPens.InfoText, drawRect);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            this.CurrentInputLayout = InputLanguage.CurrentInputLanguage.Handle;
            this.CurrentCapsState = Control.IsKeyLocked(Keys.Capital);
            this.CalculateDrawRects();
            this.UpdatePadding();
            base.OnGotFocus(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Padding = this.FPadding;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            bool flag = Control.IsKeyLocked(Keys.Capital);
            if (!(this.CapsRect.IsEmpty || (this.CurrentCapsState == flag)))
            {
                this.CurrentCapsState = flag;
                base.Invalidate(this.CapsRect);
            }
            base.OnKeyUp(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            this.CurrentInputLayout = IntPtr.Zero;
            this.InputLayoutRect = Rectangle.Empty;
            this.CapsRect = Rectangle.Empty;
            this.Padding = System.Windows.Forms.Padding.Empty;
            base.OnLostFocus(e);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (this.CapsRect.Contains(mevent.Location))
            {
                Windows.keybd_event(20, 0, KEYEVENTF.KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
                Windows.keybd_event(20, 0, KEYEVENTF.KEYEVENTF_KEYUP | KEYEVENTF.KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
            }
            if (this.InputLayoutRect.Contains(mevent.Location))
            {
                Windows.ActivateKeyboardLayout(Windows.HKL_NEXT, (KLF) 0);
            }
            base.OnMouseUp(mevent);
        }

        private bool UpdatePadding()
        {
            int right = 0;
            if (!this.CapsRect.IsEmpty)
            {
                right = base.ClientSize.Width - this.CapsRect.Left;
            }
            else if (!this.InputLayoutRect.IsEmpty)
            {
                right = base.ClientSize.Width - this.InputLayoutRect.Left;
            }
            if (right != 0)
            {
                this.Padding = new System.Windows.Forms.Padding(0, 0, right, 0);
                return true;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x20:
                {
                    Cursor iBeam = Cursors.IBeam;
                    Point point = base.PointToClient(WindowsWrapper.GetMessagePos());
                    if ((!this.CapsRect.IsEmpty && (point.X >= this.CapsRect.Left)) || (!this.InputLayoutRect.IsEmpty && (point.X >= this.InputLayoutRect.Left)))
                    {
                        iBeam = Cursors.Arrow;
                    }
                    Windows.SetCursor(iBeam.Handle);
                    m.Result = (IntPtr) 1;
                    return;
                }
                case 0x51:
                    this.CurrentInputLayout = InputLanguage.CurrentInputLanguage.Handle;
                    this.CalculateDrawRects();
                    this.UpdatePadding();
                    break;
            }
            base.WndProc(ref m);
            if ((((m.Msg == 15) && base.Enabled) && this.Focused) && !base.ReadOnly)
            {
                bool flag = !this.InputLayoutRect.IsEmpty;
                bool flag2 = !this.CapsRect.IsEmpty;
                if (flag || flag2)
                {
                    using (Graphics graphics = Graphics.FromHwnd(base.Handle))
                    {
                        if (flag)
                        {
                            this.DrawTextRect(graphics, InputLanguage.CurrentInputLanguage.LayoutName, ref this.InputLayoutRect);
                        }
                        if (flag2)
                        {
                            this.DrawTextRect(graphics, Control.IsKeyLocked(Keys.Capital) ? "A" : "a", ref this.CapsRect);
                        }
                    }
                }
            }
        }

        public System.Windows.Forms.Padding Padding
        {
            get
            {
                if (this.Multiline)
                {
                    return System.Windows.Forms.Padding.Empty;
                }
                if (!base.IsHandleCreated)
                {
                    return this.FPadding;
                }
                IntPtr ptr = Windows.SendMessage(base.Handle, 0xd4, IntPtr.Zero, IntPtr.Zero);
                return new System.Windows.Forms.Padding(((int) ptr) & 0xffff, 0, (((int) ptr) >> 0x10) & 0xffff, 0);
            }
            set
            {
                if (!this.Multiline)
                {
                    if (base.IsHandleCreated)
                    {
                        int num = value.Left & 0xffff;
                        int num2 = (value.Right & 0xffff) << 0x10;
                        IntPtr ptr = Windows.SendMessage(base.Handle, 0xd4, IntPtr.Zero, IntPtr.Zero);
                        EC ec = 0;
                        if ((((int) ptr) & 0xffff) != num)
                        {
                            ec = (EC) ((ushort) (ec | EC.EC_LEFTMARGIN));
                        }
                        if ((((int) ptr) & 0xffff0000L) != num2)
                        {
                            ec = (EC) ((ushort) (ec | EC.EC_RIGHTMARGIN));
                        }
                        if (((int) ec) != 0)
                        {
                            Windows.SendMessage(base.Handle, 0xd3, (IntPtr) ((ulong) ec), (IntPtr) (num | num2));
                        }
                    }
                    this.FPadding = value;
                    this.FPadding.Top = 0;
                    this.FPadding.Bottom = 0;
                }
            }
        }

        [Category("Behavior"), DefaultValue(false)]
        public bool ShowCapsLock
        {
            get
            {
                return this.FShowCapsLock;
            }
            set
            {
                this.FShowCapsLock = value;
            }
        }

        [DefaultValue(false), Category("Behavior")]
        public bool ShowInputLanguage
        {
            get
            {
                return this.FShowInputLanguage;
            }
            set
            {
                this.FShowInputLanguage = value;
            }
        }
    }
}

