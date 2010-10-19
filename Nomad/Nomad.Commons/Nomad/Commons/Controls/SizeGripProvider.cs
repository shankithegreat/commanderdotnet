namespace Nomad.Commons.Controls
{
    using Microsoft.Win32;
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class SizeGripProvider : NativeWindow
    {
        private const int GripWidth = 0x10;
        private VisualStyleRenderer SizeGripRenderer;

        public SizeGripProvider(Control owner)
        {
            owner.Disposed += new EventHandler(this.Control_Disposed);
            owner.HandleCreated += new EventHandler(this.Control_HandleCreated);
            owner.HandleDestroyed += new EventHandler(this.Control_HandleDestroyed);
            owner.Paint += new PaintEventHandler(this.Control_Paint);
            if (owner.IsHandleCreated)
            {
                base.AssignHandle(owner.Handle);
            }
        }

        private void Control_Disposed(object sender, EventArgs e)
        {
            Control control = (Control) sender;
            control.Disposed -= new EventHandler(this.Control_Disposed);
            control.HandleCreated -= new EventHandler(this.Control_HandleCreated);
            control.HandleDestroyed -= new EventHandler(this.Control_HandleDestroyed);
            control.Paint -= new PaintEventHandler(this.Control_Paint);
            this.ReleaseHandle();
        }

        private void Control_HandleCreated(object sender, EventArgs e)
        {
            base.AssignHandle(((Control) sender).Handle);
        }

        private void Control_HandleDestroyed(object sender, EventArgs e)
        {
            this.ReleaseHandle();
        }

        private void Control_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control) sender;
            Rectangle bounds = new Rectangle(control.ClientSize.Width - 0x10, control.ClientSize.Height - 0x10, 0x10, 0x10);
            if (Application.RenderWithVisualStyles)
            {
                (this.SizeGripRenderer ?? (this.SizeGripRenderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal))).DrawBackground(e.Graphics, bounds);
            }
            else
            {
                ControlPaint.DrawSizeGrip(e.Graphics, control.BackColor, bounds);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {
                Microsoft.Win32.RECT rect;
                Windows.GetClientRect(m.HWnd, out rect);
                Size size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
                Point lpPoint = new Point(Macros.GET_X_LPARAM(m.LParam), Macros.GET_Y_LPARAM(m.LParam));
                Windows.ScreenToClient(m.HWnd, ref lpPoint);
                if (((lpPoint.X >= (size.Width - 0x10)) && (lpPoint.Y >= (size.Height - 0x10))) && (size.Height >= 0x10))
                {
                    bool flag = (((int) Windows.GetWindowLong(m.HWnd, -20)) & 0x400000) > 0;
                    m.Result = flag ? ((IntPtr) 0x10L) : ((IntPtr) 0x11L);
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}

