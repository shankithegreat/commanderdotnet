namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [Designer(typeof(AnimatedThrobberDesigner)), DefaultProperty("Style")]
    public class AnimatedThrobber : Control
    {
        private System.Windows.Forms.BorderStyle FBorderStyle;
        private int Position;
        private ThrobberRenderer Renderer;
        private System.Windows.Forms.Timer Timer;

        public AnimatedThrobber()
        {
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            this.Renderer = new ThrobberRenderer();
            this.Renderer.Color = this.ForeColor;
            this.Timer = new System.Windows.Forms.Timer();
            this.Timer.Tick += new EventHandler(this.Timer_Tick);
            this.Timer.Enabled = base.Enabled;
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            proposedSize = this.Renderer.GetPreferredSize(proposedSize);
            proposedSize.Width += base.Padding.Horizontal;
            proposedSize.Height += base.Padding.Vertical;
            return proposedSize;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            this.Timer.Enabled = base.Enabled;
            base.Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            this.Renderer.Color = this.ForeColor;
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            this.PerformAutoSize();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (e)
            {
                Rectangle bounds = Rectangle.FromLTRB(base.Padding.Left, base.Padding.Top, base.ClientSize.Width - base.Padding.Right, base.ClientSize.Height - base.Padding.Bottom);
                ThrobberRenderEventArgs args = new ThrobberRenderEventArgs(e.Graphics, bounds, this.Position, base.Enabled);
                this.Renderer.DrawThrobber(args);
                this.Position = args.Position;
            }
        }

        private void PerformAutoSize()
        {
            if (this.AutoSize)
            {
                base.Width = 0;
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.AutoSize & ((specified & BoundsSpecified.Size) > BoundsSpecified.None))
            {
                Size preferredSize = base.PreferredSize;
                width = preferredSize.Width;
                height = preferredSize.Height;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        private bool ShouldSerializeInnerCircleRadius()
        {
            return (this.Renderer.Style == ThrobberStyle.Custom);
        }

        private bool ShouldSerializeNumberOfSpoke()
        {
            return (this.Renderer.Style == ThrobberStyle.Custom);
        }

        private bool ShouldSerializeOuterCircleRadius()
        {
            return (this.Renderer.Style == ThrobberStyle.Custom);
        }

        private bool ShouldSerializeSpokeThickness()
        {
            return (this.Renderer.Style == ThrobberStyle.Custom);
        }

        private bool ShouldSerializeStyle()
        {
            return (this.Renderer.Style != ThrobberStyle.Custom);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Position = ++this.Position % this.NumberOfSpoke;
            base.Invalidate();
        }

        [Category("Behavior"), DefaultValue(100)]
        public int AnimationSpeed
        {
            get
            {
                return this.Timer.Interval;
            }
            set
            {
                this.Timer.Interval = value;
            }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Always)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
                this.PerformAutoSize();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(System.Windows.Forms.BorderStyle), "None")]
        public System.Windows.Forms.BorderStyle BorderStyle
        {
            get
            {
                return this.FBorderStyle;
            }
            set
            {
                if (this.FBorderStyle != value)
                {
                    if (!Enum.IsDefined(typeof(System.Windows.Forms.BorderStyle), value))
                    {
                        throw new InvalidEnumArgumentException("value", (int) value, typeof(System.Windows.Forms.BorderStyle));
                    }
                    this.FBorderStyle = value;
                    base.UpdateStyles();
                }
            }
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                createParams.ExStyle &= -513;
                createParams.Style &= -8388609;
                switch (this.FBorderStyle)
                {
                    case System.Windows.Forms.BorderStyle.FixedSingle:
                        createParams.Style |= 0x800000;
                        return createParams;

                    case System.Windows.Forms.BorderStyle.Fixed3D:
                        createParams.ExStyle |= 0x200;
                        return createParams;
                }
                return createParams;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
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

        [RefreshProperties(RefreshProperties.Repaint), Category("Throbber")]
        public int InnerCircleRadius
        {
            get
            {
                return this.Renderer.InnerCircleRadius;
            }
            set
            {
                if (this.Renderer.InnerCircleRadius != value)
                {
                    this.Renderer.InnerCircleRadius = value;
                    base.Invalidate();
                }
            }
        }

        [RefreshProperties(RefreshProperties.Repaint), Category("Throbber")]
        public int NumberOfSpoke
        {
            get
            {
                return this.Renderer.NumberOfSpoke;
            }
            set
            {
                if (this.Renderer.NumberOfSpoke != value)
                {
                    this.Renderer.NumberOfSpoke = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Throbber"), RefreshProperties(RefreshProperties.Repaint)]
        public int OuterCircleRadius
        {
            get
            {
                return this.Renderer.OuterCircleRadius;
            }
            set
            {
                if (this.Renderer.OuterCircleRadius != value)
                {
                    this.Renderer.OuterCircleRadius = value;
                    this.PerformAutoSize();
                    base.Invalidate();
                }
            }
        }

        [RefreshProperties(RefreshProperties.Repaint), Category("Throbber")]
        public int SpokeThickness
        {
            get
            {
                return this.Renderer.SpokeThickness;
            }
            set
            {
                if (this.Renderer.SpokeThickness != value)
                {
                    this.Renderer.SpokeThickness = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Throbber"), RefreshProperties(RefreshProperties.Repaint)]
        public ThrobberStyle Style
        {
            get
            {
                return this.Renderer.Style;
            }
            set
            {
                if (this.Renderer.Style != value)
                {
                    this.Renderer.Style = value;
                    this.PerformAutoSize();
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(false), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool TabStop
        {
            get
            {
                return base.TabStop;
            }
            set
            {
                base.TabStop = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
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

