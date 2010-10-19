namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TabStripIE7Renderer : ToolStripSystemRenderer
    {
        private Blend ActiveTabBlend;
        private Blend InactiveTabBlend;
        private Blend ShadowBlend;

        public TabStripIE7Renderer() : this(new TabStripIE7ColorTable())
        {
        }

        public TabStripIE7Renderer(TabStripColorTable colorTable)
        {
            this.TabColorTable = colorTable;
            this.InitializeBlends();
        }

        public TabStripIE7Renderer(ProfessionalColorTable colorTable)
        {
            this.TabColorTable = new TabStripIE7ColorTable();
            this.InitializeBlends();
        }

        private LinearGradientBrush GetGradientBackBrush(Tab currentTab)
        {
            Color tabInactiveGradientBegin = this.TabColorTable.TabInactiveGradientBegin;
            Color tabInactiveGradientEnd = this.TabColorTable.TabInactiveGradientEnd;
            if (currentTab.Checked)
            {
                tabInactiveGradientBegin = this.TabColorTable.TabActiveGradientBegin;
                tabInactiveGradientEnd = this.TabColorTable.TabActiveGradientEnd;
            }
            else if (currentTab.Selected)
            {
                tabInactiveGradientBegin = this.TabColorTable.TabSelectedGradientBegin;
                tabInactiveGradientEnd = this.TabColorTable.TabSelectedGradientEnd;
            }
            return new LinearGradientBrush(new Rectangle(Point.Empty, currentTab.Size), tabInactiveGradientBegin, tabInactiveGradientEnd, LinearGradientMode.Vertical) { Blend = currentTab.Checked ? this.ActiveTabBlend : this.InactiveTabBlend };
        }

        private void InitializeBlends()
        {
            this.ActiveTabBlend = new Blend();
            this.ActiveTabBlend.Factors = new float[] { 0.3f, 0.4f, 0.5f, 1f, 1f };
            this.ActiveTabBlend.Positions = new float[] { 0f, 0.2f, 0.35f, 0.35f, 1f };
            this.InactiveTabBlend = new Blend();
            this.InactiveTabBlend.Factors = new float[] { 0.3f, 0.4f, 0.5f, 1f, 0.8f, 0.7f };
            this.InactiveTabBlend.Positions = new float[] { 0f, 0.2f, 0.4f, 0.4f, 0.8f, 1f };
            this.ShadowBlend = new Blend();
            this.ShadowBlend.Factors = new float[] { 0f, 0.1f, 0.3f, 0.4f };
            this.ShadowBlend.Positions = new float[] { 0f, 0.5f, 0.8f, 1f };
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            Tab item = e.Item as Tab;
            if (item == null)
            {
                base.OnRenderButtonBackground(e);
            }
            else
            {
                Point[] pointArray;
                Rectangle rectangle = new Rectangle(Point.Empty, e.Item.Size);
                if (item.Checked || item.IsLastTab)
                {
                    rectangle.Width--;
                }
                using (GraphicsPath path = new GraphicsPath())
                {
                    pointArray = new Point[] { new Point(rectangle.Left, rectangle.Bottom), new Point(rectangle.Left, rectangle.Top + 3), new Point(rectangle.Left + 2, rectangle.Top), new Point(rectangle.Right - 2, rectangle.Top), new Point(rectangle.Right, rectangle.Top + 3), new Point(rectangle.Right, rectangle.Bottom) };
                    path.AddLines(pointArray);
                    using (LinearGradientBrush brush = this.GetGradientBackBrush(item))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    Color color = item.Checked ? this.TabColorTable.TabActiveBorderOuter : this.TabColorTable.TabInactiveBorderOuter;
                    using (Pen pen = new Pen(color))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
                using (GraphicsPath path2 = new GraphicsPath())
                {
                    pointArray = new Point[] { new Point(rectangle.Left + 1, rectangle.Bottom), new Point(rectangle.Left + 1, rectangle.Top + 4), new Point(rectangle.Left + 3, rectangle.Top + 1), new Point(rectangle.Right - 3, rectangle.Top + 1), new Point(rectangle.Right - 1, rectangle.Top + 4), new Point(rectangle.Right - 1, rectangle.Bottom) };
                    path2.AddLines(pointArray);
                    using (Pen pen2 = new Pen(this.TabColorTable.TabBorderInner))
                    {
                        e.Graphics.DrawPath(pen2, path2);
                    }
                }
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            Tab item = e.Item as Tab;
            if (item != null)
            {
                if (e.Item.Width < e.Item.GetPreferredSize(Size.Empty).Width)
                {
                    e.TextFormat |= TextFormatFlags.EndEllipsis;
                }
                if (!(e.Item.IsForeColorSet() || e.ToolStrip.IsForeColorSet()))
                {
                    e.TextColor = item.Checked ? this.TabColorTable.TabActiveText : this.TabColorTable.TabInactiveText;
                }
                Rectangle textRectangle = e.TextRectangle;
                textRectangle.Offset(0, item.Checked ? 0 : 2);
                e.TextRectangle = textRectangle;
            }
            base.OnRenderItemText(e);
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
                Rectangle displayRectangle = toolStrip.DisplayRectangle;
                Rectangle rect = Rectangle.FromLTRB(e.AffectedBounds.Left, displayRectangle.Bottom, e.AffectedBounds.Right, e.AffectedBounds.Bottom);
                e.Graphics.ResetClip();
                using (LinearGradientBrush brush = new LinearGradientBrush(rect, this.TabColorTable.TabActiveGradientEnd, this.TabColorTable.TabStripBottomBorder, LinearGradientMode.Vertical))
                {
                    brush.Blend = this.ShadowBlend;
                    e.Graphics.FillRectangle(brush, rect);
                }
                using (Pen pen = new Pen(this.TabColorTable.TabActiveBorderOuter))
                {
                    if (toolStrip.SelectedTab != null)
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        Point[] points = new Point[] { new Point(e.AffectedBounds.Left, displayRectangle.Bottom), new Point(toolStrip.SelectedTab.Bounds.Left - 3, displayRectangle.Bottom), new Point(toolStrip.SelectedTab.Bounds.Left, displayRectangle.Bottom - 1) };
                        e.Graphics.DrawLines(pen, points);
                        points = new Point[] { new Point(toolStrip.SelectedTab.Bounds.Right, displayRectangle.Bottom - 1), new Point(toolStrip.SelectedTab.Bounds.Right + 2, displayRectangle.Bottom), new Point(e.AffectedBounds.Right, displayRectangle.Bottom) };
                        e.Graphics.DrawLines(pen, points);
                    }
                    else
                    {
                        e.Graphics.DrawLine(pen, e.AffectedBounds.Left, displayRectangle.Bottom, e.AffectedBounds.Right, displayRectangle.Bottom);
                    }
                }
            }
        }

        public TabStripColorTable TabColorTable { get; private set; }
    }
}

