namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(ProgressBar))]
    public class VistaProgressBar : ProgressBar
    {
        private IProgressBarRenderer FRenderer;
        private ProgressRenderMode FRenderMode;
        private ProgressState FState;
        private object MarqueeTag;
        private Timer MarqueeTimer = new Timer();
        private const int PBM_GETSTATE = 0x411;
        private const int PBM_SETSTATE = 0x410;
        private const int PBST_ERROR = 2;
        private const int PBST_NORMAL = 1;
        private const int PBST_PAUSED = 3;
        private const int WM_PAINT = 15;
        private const int WM_USER = 0x400;

        public VistaProgressBar()
        {
            this.MarqueeTimer.Interval = base.MarqueeAnimationSpeed;
            this.MarqueeTimer.Tick += new EventHandler(this.AnimateTimer_OnTick);
            this.MarqueeTimer.Enabled = false;
        }

        private void AnimateTimer_OnTick(object sender, EventArgs e)
        {
            if (this.MarqueeTimer.Interval != base.MarqueeAnimationSpeed)
            {
                this.MarqueeTimer.Interval = base.MarqueeAnimationSpeed;
            }
            if (base.Style == ProgressBarStyle.Marquee)
            {
                ProgressBarMarqueeEventArgs args = new ProgressBarMarqueeEventArgs(this, this.MarqueeTag);
                if (this.FRenderer.UpdateMarquee(args))
                {
                    base.Invalidate();
                }
                this.MarqueeTag = args.MarqueeTag;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (IsWindowsVista && (this.RenderMode != ProgressRenderMode.Custom))
            {
                this.SetVistaState(this.FState);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (e)
            {
                ProgressBarRenderEventArgs args = new ProgressBarRenderEventArgs(e.Graphics, this);
                this.FRenderer.DrawBackground(args);
                if (base.Style == ProgressBarStyle.Marquee)
                {
                    ProgressBarMarqueeRenderEventArgs args2 = new ProgressBarMarqueeRenderEventArgs(e.Graphics, this, this.MarqueeTag);
                    this.FRenderer.DrawMarquee(args2);
                }
                else
                {
                    ProgressBarValueRenderEventArgs args3 = new ProgressBarValueRenderEventArgs(e.Graphics, this);
                    this.FRenderer.DrawBarValue(args3);
                }
            }
        }

        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        private void SetSystemState(ProgressState state)
        {
            switch (state)
            {
                case ProgressState.Pause:
                    this.ForeColor = Color.Olive;
                    break;

                case ProgressState.Error:
                    this.ForeColor = Color.Maroon;
                    break;

                default:
                    this.ResetForeColor();
                    break;
            }
        }

        private static void SetVistaRendererState(VistaProgressBarRenderer renderer, ProgressState state)
        {
            switch (state)
            {
                case ProgressState.Pause:
                    renderer.StartColor = Color.FromArgb(0xc0, 0xc0, 0);
                    break;

                case ProgressState.Error:
                    renderer.StartColor = Color.FromArgb(0xc0, 0, 0);
                    break;

                default:
                    renderer.StartColor = Color.LimeGreen;
                    break;
            }
            renderer.EndColor = renderer.StartColor;
        }

        private void SetVistaState(ProgressState state)
        {
            SendMessage(base.Handle, 0x410, (IntPtr) 1, IntPtr.Zero);
            switch (state)
            {
                case ProgressState.Pause:
                    SendMessage(base.Handle, 0x410, (IntPtr) 3, IntPtr.Zero);
                    break;

                case ProgressState.Error:
                    SendMessage(base.Handle, 0x410, (IntPtr) 2, IntPtr.Zero);
                    break;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if ((((m.Msg == 15) && IsWindowsVista) && ((this.FState != ProgressState.Normal) && (base.Style != ProgressBarStyle.Marquee))) && (this.RenderMode != ProgressRenderMode.Custom))
            {
                this.SetVistaState(this.FState);
            }
            base.WndProc(ref m);
        }

        private static bool IsWindowsVista
        {
            get
            {
                return ((Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major > 5));
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IProgressBarRenderer Renderer
        {
            get
            {
                return ((this.FRenderMode == ProgressRenderMode.Custom) ? this.FRenderer : null);
            }
            set
            {
                if (this.FRenderer != value)
                {
                    this.FRenderer = value;
                    base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, this.FRenderer != null);
                    this.FRenderMode = (this.FRenderer != null) ? ProgressRenderMode.Custom : ProgressRenderMode.System;
                    this.MarqueeTimer.Enabled = (this.FRenderer != null) && !base.DesignMode;
                    base.Invalidate();
                }
            }
        }

        [Category("Appearance"), DefaultValue(typeof(ProgressRenderMode), "System")]
        public ProgressRenderMode RenderMode
        {
            get
            {
                return this.FRenderMode;
            }
            set
            {
                if (this.FRenderMode != value)
                {
                    if (!Enum.IsDefined(typeof(ProgressRenderMode), value))
                    {
                        throw new InvalidEnumArgumentException();
                    }
                    switch (value)
                    {
                        case ProgressRenderMode.System:
                            this.Renderer = null;
                            if (!IsWindowsVista)
                            {
                                this.SetSystemState(this.FState);
                                break;
                            }
                            this.SetVistaState(this.FState);
                            break;

                        case ProgressRenderMode.Vista:
                            if (!IsWindowsVista)
                            {
                                this.Renderer = new VistaProgressBarRenderer();
                                SetVistaRendererState((VistaProgressBarRenderer) this.Renderer, this.FState);
                                break;
                            }
                            this.Renderer = null;
                            this.SetVistaState(this.FState);
                            break;

                        default:
                            throw new NotSupportedException();
                    }
                    this.FRenderMode = value;
                }
            }
        }

        [DefaultValue(typeof(ProgressState), "Normal"), Category("Behavior")]
        public ProgressState State
        {
            get
            {
                if (IsWindowsVista && (this.RenderMode != ProgressRenderMode.Custom))
                {
                    switch (((int) SendMessage(base.Handle, 0x411, IntPtr.Zero, IntPtr.Zero)))
                    {
                        case 2:
                            return ProgressState.Error;

                        case 3:
                            return ProgressState.Pause;
                    }
                    return ProgressState.Normal;
                }
                return this.FState;
            }
            set
            {
                if ((base.Style == ProgressBarStyle.Marquee) || (this.RenderMode == ProgressRenderMode.Custom))
                {
                    value = ProgressState.Normal;
                }
                if (this.FState != value)
                {
                    this.FState = value;
                    if (!Enum.IsDefined(typeof(ProgressState), value))
                    {
                        throw new InvalidEnumArgumentException();
                    }
                    if ((this.FRenderMode != ProgressRenderMode.Custom) && IsWindowsVista)
                    {
                        this.SetVistaState(this.FState);
                    }
                    else if (this.FRenderMode == ProgressRenderMode.System)
                    {
                        this.SetSystemState(this.FState);
                    }
                    else if (this.FRenderMode == ProgressRenderMode.Vista)
                    {
                        SetVistaRendererState((VistaProgressBarRenderer) this.FRenderer, this.FState);
                        base.Invalidate();
                    }
                }
            }
        }
    }
}

