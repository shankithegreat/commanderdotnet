namespace Nomad.Controls
{
    using Microsoft;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class TabStripSystemRenderer : ToolStripSystemRenderer
    {
        private bool FUseVisualStyles;
        private VisualStyleRenderer PaneRenderer;

        public TabStripSystemRenderer() : this(true)
        {
        }

        public TabStripSystemRenderer(bool useVisualStyles)
        {
            this.FUseVisualStyles = useVisualStyles;
            this.DrawFocusRect = true;
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            TabStrip toolStrip = e.ToolStrip as TabStrip;
            Nomad.Controls.Tab item = e.Item as Nomad.Controls.Tab;
            Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);
            if ((item != null) && (toolStrip != null))
            {
                if (this.UseVisualStyles)
                {
                    TabItemState normal = TabItemState.Normal;
                    if (item.Selected)
                    {
                        normal = TabItemState.Hot;
                    }
                    if (item.Checked)
                    {
                        normal = TabItemState.Selected;
                    }
                    if (toolStrip.EffectiveOrientation == TabOrientation.Top)
                    {
                        TabRenderer.DrawTabItem(e.Graphics, bounds, normal);
                    }
                    else
                    {
                        using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                        {
                            using (Graphics graphics = Graphics.FromImage(bitmap))
                            {
                                TabRenderer.DrawTabItem(graphics, new Rectangle(Point.Empty, bitmap.Size), normal);
                            }
                            bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                            e.Graphics.DrawImage(bitmap, bounds.Location);
                        }
                    }
                }
                else
                {
                    int top = bounds.Top;
                    int num2 = bounds.Bottom - 1;
                    int num3 = 1;
                    if (toolStrip.EffectiveOrientation == TabOrientation.Bottom)
                    {
                        top = bounds.Bottom - 1;
                        num2 = bounds.Top;
                        num3 = -1;
                    }
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, bounds.Left, num2, bounds.Left, top + (2 * num3));
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, bounds.Left, top + (2 * num3), bounds.Left + 2, top);
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, bounds.Left + 2, top, bounds.Right - 3, top);
                    e.Graphics.DrawLine(SystemPens.ControlLight, bounds.Left + 1, num2, bounds.Left + 1, top + (2 * num3));
                    e.Graphics.DrawLine(SystemPens.ControlLight, (int) (bounds.Left + 2), (int) (top + num3), (int) (bounds.Right - 3), (int) (top + num3));
                    e.Graphics.DrawLine(SystemPens.ControlDarkDark, (int) (bounds.Right - 2), (int) (top + num3), (int) (bounds.Right - 1), (int) (top + (2 * num3)));
                    e.Graphics.DrawLine(SystemPens.ControlDarkDark, bounds.Right - 1, top + (2 * num3), bounds.Right - 1, num2);
                    e.Graphics.DrawLine(SystemPens.ControlDark, bounds.Right - 2, top + (2 * num3), bounds.Right - 2, num2);
                }
            }
            else
            {
                base.OnRenderButtonBackground(e);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            Nomad.Controls.Tab item = e.Item as Nomad.Controls.Tab;
            if (item != null)
            {
                if (e.Item.Width < e.Item.GetPreferredSize(Size.Empty).Width)
                {
                    e.TextFormat |= TextFormatFlags.EndEllipsis;
                }
                Rectangle textRectangle = e.TextRectangle;
                textRectangle.Offset(0, item.Checked ? -1 : 1);
                e.TextRectangle = textRectangle;
            }
            base.OnRenderItemText(e);
            if (((item != null) && item.Checked) && this.DrawFocusRect)
            {
                Rectangle contentRectangle = e.Item.ContentRectangle;
                contentRectangle.Inflate(-1, -1);
                ControlPaint.DrawFocusRectangle(e.Graphics, contentRectangle);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            TabStrip toolStrip = e.ToolStrip as TabStrip;
            if (toolStrip == null)
            {
                base.OnRenderToolStripBorder(e);
            }
            else
            {
                Color controlLightLight = SystemColors.ControlLightLight;
                Color controlLight = SystemColors.ControlLight;
                if (this.UseVisualStyles)
                {
                    if (this.PaneRenderer == null)
                    {
                        this.PaneRenderer = new VisualStyleRenderer(VisualStyleElement.Tab.TabItemLeftEdge.Normal);
                    }
                    controlLightLight = this.PaneRenderer.GetColor(ColorProperty.EdgeDarkShadowColor);
                    if (OS.IsWinVista)
                    {
                        controlLight = this.PaneRenderer.GetColor(ColorProperty.GlowColor);
                    }
                    else
                    {
                        controlLight = this.PaneRenderer.GetColor(ColorProperty.FillColor);
                    }
                }
                Rectangle displayRectangle = toolStrip.DisplayRectangle;
                int num = (toolStrip.EffectiveOrientation == TabOrientation.Top) ? displayRectangle.Bottom : displayRectangle.Top;
                int left = e.AffectedBounds.Left;
                int right = e.AffectedBounds.Right;
                int num4 = 0;
                int num5 = 0;
                if (toolStrip.SelectedTab != null)
                {
                    num4 = toolStrip.SelectedTab.Bounds.Left;
                    num5 = toolStrip.SelectedTab.Bounds.Right - 1;
                    if (!this.UseVisualStyles)
                    {
                        num4--;
                        num5++;
                    }
                }
                using (Pen pen = new Pen(controlLightLight))
                {
                    if (toolStrip.SelectedTab != null)
                    {
                        e.Graphics.DrawLine(pen, left, num, num4, num);
                        e.Graphics.DrawLine(pen, num5, num, right, num);
                    }
                    else
                    {
                        e.Graphics.DrawLine(pen, left, num, right, num);
                    }
                }
                num += (toolStrip.EffectiveOrientation == TabOrientation.Top) ? 1 : -1;
                using (Pen pen2 = new Pen(controlLight))
                {
                    if (this.UseVisualStyles || (toolStrip.SelectedTab == null))
                    {
                        e.Graphics.DrawLine(pen2, left, num, right, num);
                    }
                    else
                    {
                        e.Graphics.DrawLine(pen2, left, num, num4, num);
                        e.Graphics.DrawLine(pen2, num5, num, right, num);
                    }
                }
            }
        }

        public bool DrawFocusRect { get; set; }

        public bool UseVisualStyles
        {
            get
            {
                return (this.FUseVisualStyles && VisualStyleRenderer.IsSupported);
            }
            set
            {
                this.FUseVisualStyles = true;
            }
        }
    }
}

