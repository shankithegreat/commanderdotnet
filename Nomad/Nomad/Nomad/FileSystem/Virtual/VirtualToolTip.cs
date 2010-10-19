namespace Nomad.FileSystem.Virtual
{
    using Microsoft;
    using Microsoft.Win32;
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.Commons.Threading;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public sealed class VirtualToolTip
    {
        private static VirtualToolTip FDefault;
        private PopupForm PopupPanel;

        private static void ExtractThumbnail(object state)
        {
            object[] objArray = (object[]) state;
            IVirtualItem item = (IVirtualItem) objArray[0];
            PopupForm form = (PopupForm) objArray[1];
            int num = -1;
            Size iconSize = (Size) objArray[3];
            bool extractThumbnail = (bool) objArray[4];
            bool extractIcon = (bool) objArray[5];
            Image image = ExtractThumbnail(item, iconSize, extractThumbnail, extractIcon);
            lock (objArray)
            {
                num = (int) objArray[2];
            }
            if ((num >= 0) && form.InvokeRequired)
            {
                form.Invoke(new Action<Image, int>(form.SetIcon), new object[] { image, num });
            }
            objArray[0] = image;
        }

        private static Image ExtractThumbnail(IVirtualItem item, Size iconSize, bool extractThumbnail, bool extractIcon)
        {
            Image original = null;
            if (extractThumbnail)
            {
                original = VirtualIcon.GetThumbnail(item, Settings.Default.TooltipMaxThumbnailSize);
                if (original != null)
                {
                    Size tooltipMaxThumbnailSize = Settings.Default.TooltipMaxThumbnailSize;
                    if (!(tooltipMaxThumbnailSize.IsEmpty || ((original.Width <= tooltipMaxThumbnailSize.Width) && (original.Height <= tooltipMaxThumbnailSize.Height))))
                    {
                        original = new Bitmap(original, ImageHelper.GetThumbnailSize(original.Size, tooltipMaxThumbnailSize));
                    }
                }
            }
            if ((original == null) && extractIcon)
            {
                original = VirtualIcon.GetIcon(item, iconSize);
            }
            return original;
        }

        private static void GetToolTipIcon(object state)
        {
            object[] objArray = (object[]) state;
            objArray[0] = VirtualIcon.GetIcon((IVirtualItem) objArray[0], (Size) objArray[1]);
        }

        private static void GetToolTipThumbnail(object state)
        {
            object[] objArray = (object[]) state;
            Image thumbnail = VirtualIcon.GetThumbnail((IVirtualItem) objArray[0], Settings.Default.TooltipMaxThumbnailSize);
            if (thumbnail != null)
            {
                Size tooltipMaxThumbnailSize = Settings.Default.TooltipMaxThumbnailSize;
                if (!(tooltipMaxThumbnailSize.IsEmpty || ((thumbnail.Width <= tooltipMaxThumbnailSize.Width) && (thumbnail.Height <= tooltipMaxThumbnailSize.Height))))
                {
                    thumbnail = new Bitmap(thumbnail, ImageHelper.GetThumbnailSize(thumbnail.Size, tooltipMaxThumbnailSize));
                }
            }
            objArray[0] = thumbnail;
        }

        public void HideTooltip()
        {
            if ((((this.PopupPanel != null) && this.PopupPanel.IsHandleCreated) && !this.PopupPanel.IsDisposed) && !this.PopupPanel.Disposing)
            {
                if (this.PopupPanel.InvokeRequired)
                {
                    this.PopupPanel.BeginInvoke(new MethodInvoker(this.PopupPanel.Hide));
                }
                else
                {
                    this.PopupPanel.Hide();
                }
            }
        }

        public void ShowTooltip(IVirtualItemUI item)
        {
            Point position = Cursor.Position;
            this.ShowTooltip(item, item.ToolTip, null, position.X, position.Y + Cursor.Current.GetPrefferedHeight());
        }

        public void ShowTooltip(IVirtualItem item, string text)
        {
            Point position = Cursor.Position;
            this.ShowTooltip(item, text, null, position.X, position.Y + Cursor.Current.GetPrefferedHeight());
        }

        private void ShowTooltip(IVirtualItem item, string text, Point popupPosition)
        {
            Image image = null;
            if (Settings.Default.ShowThumbnailInTooltip || Settings.Default.ShowIconInTooltip)
            {
                int tooltipThumbnailTimeout = Settings.Default.TooltipThumbnailTimeout;
                if (tooltipThumbnailTimeout == -1)
                {
                    image = ExtractThumbnail(item, Settings.Default.ToolTipIconSize, Settings.Default.ShowThumbnailInTooltip, Settings.Default.ShowIconInTooltip);
                }
                else
                {
                    object[] state = new object[] { item, this.PopupPanel, -1, Settings.Default.ToolTipIconSize, Settings.Default.ShowThumbnailInTooltip, Settings.Default.ShowIconInTooltip };
                    Task task = Task.Create<object[]>(new Action<object[]>(VirtualToolTip.ExtractThumbnail), state);
                    task.Start(ApartmentState.STA);
                    if (task.Wait(tooltipThumbnailTimeout))
                    {
                        image = (Image) state[0];
                    }
                    else
                    {
                        lock (state)
                        {
                            state[2] = this.PopupPanel.IconTicket;
                        }
                        image = VirtualIcon.GetIcon(item, Settings.Default.ToolTipIconSize, IconStyle.DefaultIcon);
                    }
                }
            }
            if (image != null)
            {
                lock (image)
                {
                    this.PopupPanel.IconBox.Image = image;
                }
                this.PopupPanel.IconBox.Visible = true;
            }
            else
            {
                this.PopupPanel.IconBox.Visible = false;
            }
            this.PopupPanel.ItemLabel.Text = text;
            Rectangle bounds = Screen.FromPoint(popupPosition).Bounds;
            Size preferredSize = this.PopupPanel.PreferredSize;
            if ((popupPosition.X + preferredSize.Width) > bounds.Right)
            {
                popupPosition.X = (bounds.Right - preferredSize.Width) - 1;
            }
            if (popupPosition.X < bounds.Left)
            {
                popupPosition.X = bounds.Left;
            }
            if ((popupPosition.Y + preferredSize.Height) > bounds.Bottom)
            {
                popupPosition.Y = ((popupPosition.Y - Cursor.Current.GetRealHeight()) - preferredSize.Height) - 8;
            }
            this.PopupPanel.Show(popupPosition);
        }

        public void ShowTooltip(IVirtualItemUI item, Control control, int x, int y)
        {
            this.ShowTooltip(item, item.ToolTip, control, x, y);
        }

        public void ShowTooltip(IVirtualItem item, string text, Control control, int x, int y)
        {
            EventHandler handler = null;
            ThreadStart start = null;
            this.HideTooltip();
            if (!string.IsNullOrEmpty(text))
            {
                if (this.PopupPanel == null)
                {
                    this.PopupPanel = new PopupForm();
                    this.PopupPanel.SetBounds(-32767, 0, 1, 1);
                    if (handler == null)
                    {
                        handler = delegate (object sender, EventArgs e) {
                            if (!this.PopupPanel.Visible)
                            {
                                this.PopupPanel.IconBox.Image = null;
                            }
                        };
                    }
                    this.PopupPanel.VisibleChanged += handler;
                }
                else if (this.PopupPanel.IsDisposed || this.PopupPanel.Disposing)
                {
                    return;
                }
                Point p = new Point(x, y);
                if (control != null)
                {
                    p = control.PointToScreen(p);
                }
                if (!this.PopupPanel.InvokeRequired)
                {
                    if (start == null)
                    {
                        start = delegate {
                            Program.SetupApplicationExceptionHandler();
                            Application.Run(this.PopupPanel);
                        };
                    }
                    Thread thread = new Thread(start) {
                        Name = "Tooltip"
                    };
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.IsBackground = true;
                    ErrorReport.RegisterThread(thread);
                    thread.Start();
                    while (!this.PopupPanel.InvokeRequired)
                    {
                        Thread.SpinWait(100);
                    }
                }
                this.PopupPanel.BeginInvoke(new Action<IVirtualItem, string, Point>(this.ShowTooltip), new object[] { item, text, p });
            }
        }

        public static VirtualToolTip Default
        {
            get
            {
                if (FDefault == null)
                {
                }
                return (FDefault = new VirtualToolTip());
            }
        }

        private class PopupForm : Form
        {
            private EventHandler AnimateCallback;
            private VisualStyleRenderer BackgroundRenderer;
            private double InitialOpacity;
            private bool IsInShowWindow = true;
            private const int ShowDelay = 200;
            private const int ShowStepCount = 5;

            public PopupForm()
            {
                this.InitializeComponent();
                base.SetStyle(ControlStyles.Selectable, false);
            }

            private void InitializeBackgroundRenderer()
            {
                if (this.CanDrawToolTipBackground)
                {
                    this.BackgroundRenderer = new VisualStyleRenderer(VisualStyleElement.ToolTip.Standard.Normal);
                    this.TablePanel.BackColor = System.Drawing.Color.Transparent;
                    this.ItemLabel.ForeColor = this.BackgroundRenderer.GetColor(ColorProperty.TextColor);
                }
                else
                {
                    this.BackgroundRenderer = null;
                    this.TablePanel.BackColor = SystemColors.Info;
                    this.ItemLabel.ForeColor = SystemColors.InfoText;
                }
                if (base.IsHandleCreated)
                {
                    int windowLong = (int) Windows.GetWindowLong(base.Handle, -16);
                    if (this.BackgroundRenderer != null)
                    {
                        windowLong &= -8388609;
                    }
                    else
                    {
                        windowLong |= 0x800000;
                    }
                    Windows.SetWindowLong(base.Handle, -16, windowLong);
                }
            }

            private void InitializeComponent()
            {
                this.TablePanel = new TableLayoutPanel();
                this.IconBox = new PictureBox();
                this.ItemLabel = new Label();
                base.SuspendLayout();
                this.TablePanel.SuspendLayout();
                base.FormBorderStyle = FormBorderStyle.None;
                base.StartPosition = FormStartPosition.Manual;
                base.ShowInTaskbar = false;
                this.BackColor = SystemColors.Info;
                this.AutoSize = true;
                base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                this.TablePanel.AutoSize = true;
                this.TablePanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                this.TablePanel.Margin = Padding.Empty;
                this.TablePanel.ColumnCount = 2;
                this.TablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                this.TablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                this.TablePanel.RowCount = 1;
                this.TablePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                base.Controls.Add(this.TablePanel);
                this.IconBox.Margin = new Padding(3, 3, 2, 3);
                this.IconBox.SizeMode = PictureBoxSizeMode.AutoSize;
                this.TablePanel.Controls.Add(this.IconBox, 0, 0);
                this.ItemLabel.AutoSize = true;
                this.ItemLabel.UseMnemonic = false;
                this.ItemLabel.ForeColor = SystemColors.InfoText;
                this.ItemLabel.Margin = new Padding(2, 3, 3, 3);
                this.ItemLabel.Font = SystemFonts.IconTitleFont;
                this.TablePanel.Controls.Add(this.ItemLabel, 1, 0);
                this.TablePanel.ResumeLayout();
                base.ResumeLayout();
                this.InitializeBackgroundRenderer();
            }

            private void InternalSetOpacity(double value)
            {
                if (((byte) ((base.Opacity - value) * 255.0)) != 0)
                {
                    try
                    {
                        base.Opacity = value;
                    }
                    catch (Win32Exception exception)
                    {
                        if (exception.NativeErrorCode != 0x57)
                        {
                            throw;
                        }
                    }
                }
            }

            private void OnAnimate(object sender, EventArgs e)
            {
                this.IconBox.Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                if ((this.BackgroundRenderer != null) && this.CanDrawToolTipBackground)
                {
                    this.BackgroundRenderer.DrawBackground(e.Graphics, base.ClientRectangle);
                }
                else
                {
                    base.OnPaint(e);
                }
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                if (this.BackgroundRenderer != null)
                {
                    Region region = new Region(new Rectangle(0, 1, base.Width, base.Height - 2));
                    region.Union(new Rectangle(1, 0, base.Width - 2, base.Height));
                    base.Region = region;
                }
            }

            protected override void OnVisibleChanged(EventArgs e)
            {
                if (base.Visible)
                {
                    if ((this.IconBox.Visible && (this.IconBox.Image != null)) && ImageAnimator.CanAnimate(this.IconBox.Image))
                    {
                        this.AnimateCallback = new EventHandler(this.OnAnimate);
                        ImageAnimator.Animate(this.IconBox.Image, this.AnimateCallback);
                    }
                    this.StartOpacityTimer();
                }
                else
                {
                    this.IconTicket++;
                    Windows.KillTimer(base.Handle, IntPtr.Zero);
                    if (this.AnimateCallback != null)
                    {
                        ImageAnimator.StopAnimate(this.IconBox.Image, this.AnimateCallback);
                        this.AnimateCallback = null;
                    }
                }
                base.OnVisibleChanged(e);
            }

            public void SetIcon(Image icon, int ticket)
            {
                if (this.IconTicket == ticket)
                {
                    Windows.KillTimer(base.Handle, IntPtr.Zero);
                    lock (icon)
                    {
                        this.IconBox.Image = icon;
                    }
                    if (Settings.Default.TooltipOpacityDelay > 0)
                    {
                        this.InternalSetOpacity(1.0);
                        this.StartOpacityTimer();
                    }
                }
            }

            public void SetOpacity(double value)
            {
                if (OSFeature.Feature.IsPresent(OSFeature.LayeredWindows) && base.Visible)
                {
                    if (SystemInformation.IsToolTipAnimationEnabled)
                    {
                        double num = (value - base.Opacity) / 5.0;
                        if (num != 0.0)
                        {
                            int num2 = 5;
                            while (num2-- > 0)
                            {
                                this.InternalSetOpacity(base.Opacity + num);
                                Application.DoEvents();
                                Thread.Sleep(40);
                            }
                        }
                    }
                    else
                    {
                        this.InternalSetOpacity(value);
                    }
                }
            }

            public void Show()
            {
                Windows.KillTimer(base.Handle, IntPtr.Zero);
                Windows.SetWindowPos(base.Handle, Windows.HWND_TOPMOST, 0, 0, 0, 0, SWP.SWP_NOMOVE | SWP.SWP_NOSIZE | SWP.SWP_NOACTIVATE);
                if (this.CanDrawToolTipBackground ^ (this.BackgroundRenderer != null))
                {
                    this.InitializeBackgroundRenderer();
                    base.Region = null;
                }
                if (OSFeature.Feature.IsPresent(OSFeature.LayeredWindows))
                {
                    this.InternalSetOpacity(1.0);
                    this.InitialOpacity = (Settings.Default.TooltipOpacityDelay == 0) ? Settings.Default.TooltipOpacityValue : 1.0;
                }
                base.Show();
            }

            public void Show(Point p)
            {
                base.Location = p;
                this.Show();
            }

            private void StartOpacityTimer()
            {
                int tooltipOpacityDelay = Settings.Default.TooltipOpacityDelay;
                if (OSFeature.Feature.IsPresent(OSFeature.LayeredWindows) && (tooltipOpacityDelay > 0))
                {
                    Windows.SetTimer(base.Handle, IntPtr.Zero, tooltipOpacityDelay, IntPtr.Zero);
                }
            }

            private void WmShowWindow(ref Message m)
            {
                if (!this.IsInShowWindow && SystemInformation.IsToolTipAnimationEnabled)
                {
                    bool flag = Convert.ToBoolean((int) m.WParam);
                    if (OS.IsWinXP && (!flag || (this.InitialOpacity == 1.0)))
                    {
                        this.IsInShowWindow = true;
                        Windows.AnimateWindow(base.Handle, 200, AW.AW_BLEND | (flag ? ((AW) 0) : AW.AW_HIDE));
                        this.IsInShowWindow = false;
                    }
                    else if (OSFeature.Feature.IsPresent(OSFeature.LayeredWindows))
                    {
                        this.InternalSetOpacity(0.0);
                        base.BeginInvoke(new Action<double>(this.SetOpacity), new object[] { this.InitialOpacity });
                    }
                }
            }

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case 0x18:
                        this.WmShowWindow(ref m);
                        this.IsInShowWindow = false;
                        break;

                    case 0x113:
                        Windows.KillTimer(base.Handle, m.WParam);
                        this.SetOpacity(Settings.Default.TooltipOpacityValue);
                        break;

                    case 0x31a:
                        this.InitializeBackgroundRenderer();
                        base.Region = null;
                        break;
                }
                base.WndProc(ref m);
            }

            private bool CanDrawToolTipBackground
            {
                get
                {
                    return (VisualStyleRenderer.IsSupported && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ToolTip.Standard.Normal));
                }
            }

            protected override System.Windows.Forms.CreateParams CreateParams
            {
                get
                {
                    System.Windows.Forms.CreateParams createParams = base.CreateParams;
                    createParams.Style |= -2147483648;
                    if (this.BackgroundRenderer == null)
                    {
                        createParams.Style |= 0x800000;
                    }
                    createParams.ExStyle |= 0x8000008;
                    if (OS.IsWinXP && SystemInformation.IsDropShadowEnabled)
                    {
                        createParams.ClassStyle |= 0x20000;
                    }
                    return createParams;
                }
            }

            public PictureBox IconBox { get; private set; }

            public int IconTicket { get; private set; }

            public Label ItemLabel { get; private set; }

            protected override bool ShowWithoutActivation
            {
                get
                {
                    return true;
                }
            }

            public TableLayoutPanel TablePanel { get; private set; }
        }
    }
}

