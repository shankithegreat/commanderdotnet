namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class ToolStripThrobberButton : ToolStripButton
    {
        private int Position;
        private ThrobberRenderer Renderer = new ThrobberRenderer();
        private System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();

        public ToolStripThrobberButton()
        {
            this.Timer.Tick += new EventHandler(this.Timer_Tick);
            this.Timer.Enabled = this.Enabled;
        }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            Size preferredSize;
            Size size2;
            switch (this.DisplayStyle)
            {
                case ToolStripItemDisplayStyle.Image:
                    constrainingSize = this.Renderer.GetPreferredSize(constrainingSize);
                    goto Label_013D;

                case ToolStripItemDisplayStyle.ImageAndText:
                    preferredSize = this.Renderer.GetPreferredSize(constrainingSize);
                    size2 = base.GetPreferredSize(constrainingSize);
                    size2.Width -= this.Padding.Horizontal;
                    size2.Height -= this.Padding.Vertical;
                    switch (base.TextImageRelation)
                    {
                        case TextImageRelation.ImageAboveText:
                        case TextImageRelation.TextAboveImage:
                            constrainingSize = new Size(Math.Max(preferredSize.Width, size2.Width), size2.Height + preferredSize.Height);
                            goto Label_013D;

                        case TextImageRelation.ImageBeforeText:
                        case TextImageRelation.TextBeforeImage:
                            constrainingSize = new Size(size2.Width + preferredSize.Width, Math.Max(preferredSize.Height, size2.Height));
                            goto Label_013D;
                    }
                    break;

                default:
                    return base.GetPreferredSize(constrainingSize);
            }
            constrainingSize = new Size(Math.Max(preferredSize.Width, size2.Width), Math.Max(preferredSize.Height, size2.Height));
        Label_013D:
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

        protected override void OnPaint(PaintEventArgs e)
        {
            using (e)
            {
                Rectangle rectangle;
                Rectangle rectangle2;
                Size preferredSize;
                ToolStripItemRenderEventArgs args;
                switch (this.DisplayStyle)
                {
                    case ToolStripItemDisplayStyle.Image:
                    case ToolStripItemDisplayStyle.ImageAndText:
                        rectangle = new Rectangle(this.Padding.Left, this.Padding.Top, base.Width - this.Padding.Horizontal, base.Height - this.Padding.Vertical);
                        if (this.DisplayStyle != ToolStripItemDisplayStyle.ImageAndText)
                        {
                            goto Label_01C9;
                        }
                        preferredSize = this.Renderer.GetPreferredSize(this.Size);
                        rectangle2 = rectangle;
                        switch (base.TextImageRelation)
                        {
                            case TextImageRelation.TextAboveImage:
                                goto Label_010D;

                            case (TextImageRelation.TextAboveImage | TextImageRelation.ImageAboveText):
                                goto Label_01CF;

                            case TextImageRelation.ImageBeforeText:
                                goto Label_014B;

                            case TextImageRelation.TextBeforeImage:
                                goto Label_0188;
                        }
                        goto Label_01CF;

                    default:
                        goto Label_026E;
                }
                rectangle.Height = preferredSize.Height;
                rectangle2.Y += preferredSize.Height;
                rectangle2.Height -= preferredSize.Height;
                goto Label_01CF;
            Label_010D:
                rectangle.Y = rectangle.Bottom - preferredSize.Height;
                rectangle.Height = preferredSize.Height;
                rectangle2.Height -= preferredSize.Height;
                goto Label_01CF;
            Label_014B:
                rectangle.Width = preferredSize.Width;
                rectangle2.X += preferredSize.Width;
                rectangle2.Width -= preferredSize.Width;
                goto Label_01CF;
            Label_0188:
                rectangle.X = rectangle.Right - preferredSize.Width;
                rectangle.Width = preferredSize.Width;
                rectangle2.Width -= preferredSize.Width;
                goto Label_01CF;
            Label_01C9:
                rectangle2 = Rectangle.Empty;
            Label_01CF:
                args = new ToolStripItemRenderEventArgs(e.Graphics, this);
                base.Parent.Renderer.DrawButtonBackground(args);
                ThrobberRenderEventArgs args2 = new ThrobberRenderEventArgs(e.Graphics, rectangle, this.Position, this.Enabled);
                this.Renderer.DrawThrobber(args2);
                this.Position = args2.Position;
                if (!rectangle2.IsEmpty)
                {
                    ToolStripItemTextRenderEventArgs args3 = new ToolStripItemTextRenderEventArgs(e.Graphics, this, this.Text, rectangle2, this.ForeColor, this.Font, this.TextAlign);
                    base.Parent.Renderer.DrawItemText(args3);
                }
                return;
            Label_026E:
                base.OnPaint(e);
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
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

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
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

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [DefaultValue(typeof(Color), "Gray"), Category("Throbber")]
        public Color ThrobberColor
        {
            get
            {
                return this.Renderer.Color;
            }
            set
            {
                if (this.Renderer.Color != value)
                {
                    this.Renderer.Color = value;
                    base.Invalidate();
                }
            }
        }
    }
}

