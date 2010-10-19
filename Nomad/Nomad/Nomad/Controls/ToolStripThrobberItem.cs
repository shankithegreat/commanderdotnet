namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class ToolStripThrobberItem : ToolStripItem
    {
        private int Position;
        private ThrobberRenderer Renderer = new ThrobberRenderer();
        private System.Windows.Forms.Timer Timer;

        public ToolStripThrobberItem()
        {
            this.Renderer.Color = this.ForeColor;
            this.Timer = new System.Windows.Forms.Timer();
            this.Timer.Tick += new EventHandler(this.Timer_Tick);
            this.Timer.Enabled = this.Enabled;
        }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            constrainingSize = this.Renderer.GetPreferredSize(constrainingSize);
            constrainingSize.Width += this.Padding.Horizontal;
            constrainingSize.Height += this.Padding.Vertical;
            return constrainingSize;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            this.Timer.Enabled = this.Enabled;
            base.Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            this.Renderer.Color = this.ForeColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (e)
            {
                ToolStripItemRenderEventArgs args = new ToolStripItemRenderEventArgs(e.Graphics, this);
                base.Parent.Renderer.DrawLabelBackground(args);
                Rectangle bounds = new Rectangle(this.Padding.Left, this.Padding.Top, base.Width - this.Padding.Horizontal, base.Height - this.Padding.Vertical);
                ThrobberRenderEventArgs args2 = new ThrobberRenderEventArgs(e.Graphics, bounds, this.Position, this.Enabled);
                this.Renderer.DrawThrobber(args2);
                this.Position = args2.Position;
            }
        }

        private void PerformAutoSize()
        {
            if (base.AutoSize)
            {
                base.Width = 0;
            }
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

        [DefaultValue(100), Category("Behavior")]
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

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoToolTip
        {
            get
            {
                return base.AutoToolTip;
            }
            set
            {
                base.AutoToolTip = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ToolStripItemDisplayStyle DisplayStyle
        {
            get
            {
                return base.DisplayStyle;
            }
            set
            {
                base.DisplayStyle = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
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

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override System.Drawing.Image Image
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

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public ToolStripItemImageScaling ImageScaling
        {
            get
            {
                return base.ImageScaling;
            }
            set
            {
                base.ImageScaling = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public Color ImageTransparentColor
        {
            get
            {
                return base.ImageTransparentColor;
            }
            set
            {
                base.ImageTransparentColor = value;
            }
        }

        [Category("Throbber"), RefreshProperties(RefreshProperties.Repaint)]
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

        [RefreshProperties(RefreshProperties.Repaint), Category("Throbber")]
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

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool RightToLeftAutoMirrorImage
        {
            get
            {
                return base.RightToLeftAutoMirrorImage;
            }
            set
            {
                base.RightToLeftAutoMirrorImage = value;
            }
        }

        [Category("Throbber"), RefreshProperties(RefreshProperties.Repaint)]
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

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public override ToolStripTextDirection TextDirection
        {
            get
            {
                return base.TextDirection;
            }
            set
            {
                base.TextDirection = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    }
}

