namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TabStripProfessionalRenderer : ToolStripProfessionalRenderer
    {
        private const int BOTTOM_LEFT = 0;
        private const int BOTTOM_RIGHT = 3;
        private bool FUseBoldFont;
        private const int TOP_LEFT = 1;
        private const int TOP_RIGHT = 2;

        public TabStripProfessionalRenderer() : this(new TabStripColorTable(new ProfessionalColorTable()))
        {
        }

        public TabStripProfessionalRenderer(TabStripColorTable colorTable) : base(colorTable.ColorTable)
        {
            this.FUseBoldFont = true;
            this.TabColorTable = colorTable;
            base.RoundedEdges = false;
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            TabStrip toolStrip = e.ToolStrip as TabStrip;
            Tab item = e.Item as Tab;
            if ((toolStrip == null) || (item == null))
            {
                base.OnRenderButtonBackground(e);
            }
            else
            {
                Brush brush;
                Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
                Graphics graphics = e.Graphics;
                Point[] pointArray3 = new Point[] { new Point(rect.Left, rect.Bottom), new Point(rect.Left + item.Height, rect.Top), new Point(rect.Right - 1, rect.Top), new Point(rect.Right - 1, rect.Bottom) };
                Point[] pointArray = pointArray3;
                pointArray3 = new Point[] { pointArray[0], new Point(pointArray[1].X - 2, pointArray[1].Y + 2), new Point(pointArray[1].X + 2, pointArray[1].Y), new Point(pointArray[2].X - 1, pointArray[2].Y), pointArray[3] };
                Point[] points = pointArray3;
                Color tabInactiveGradientBegin = this.TabColorTable.TabInactiveGradientBegin;
                Color tabInactiveGradientEnd = this.TabColorTable.TabInactiveGradientEnd;
                if (item.Checked)
                {
                    tabInactiveGradientBegin = this.TabColorTable.TabActiveGradientBegin;
                    tabInactiveGradientEnd = this.TabColorTable.TabActiveGradientEnd;
                }
                else if (item.Selected)
                {
                    tabInactiveGradientBegin = this.TabColorTable.TabSelectedGradientBegin;
                    tabInactiveGradientEnd = this.TabColorTable.TabSelectedGradientEnd;
                }
                if (tabInactiveGradientBegin != tabInactiveGradientEnd)
                {
                    brush = new LinearGradientBrush(rect, tabInactiveGradientBegin, tabInactiveGradientEnd, LinearGradientMode.Vertical);
                }
                else
                {
                    brush = new SolidBrush(tabInactiveGradientBegin);
                }
                using (brush)
                {
                    graphics.FillPolygon(brush, points);
                }
                Color color = item.Checked ? this.TabColorTable.TabActiveBorderOuter : this.TabColorTable.TabInactiveBorderOuter;
                using (Pen pen = new Pen(color))
                {
                    using (Pen pen2 = new Pen(this.TabColorTable.TabBorderInner))
                    {
                        Brush brush2;
                        Point point = pointArray[0];
                        Point point2 = pointArray[1];
                        point2.Offset(-2, 2);
                        graphics.DrawLine(pen, point, point2);
                        point.Offset(1, 0);
                        point2.Offset(1, 0);
                        graphics.DrawLine(pen2, point, point2);
                        point = pointArray[1];
                        point.Offset(0, 1);
                        point2 = point;
                        point2.X++;
                        graphics.DrawLine(pen, point, point2);
                        point.Offset(-2, 1);
                        point2.Offset(-2, 1);
                        graphics.DrawLine(pen, point, point2);
                        point.Offset(2, 0);
                        point2.Offset(2, 0);
                        graphics.DrawLine(pen2, point, point2);
                        point.Offset(-2, 1);
                        point2.Offset(-2, 1);
                        graphics.DrawLine(pen2, point, point2);
                        point = pointArray[1];
                        point2 = pointArray[2];
                        point.Offset(2, 0);
                        point2.Offset(-2, 0);
                        graphics.DrawLine(pen, point, point2);
                        point.Offset(0, 1);
                        point2.Offset(0, 1);
                        graphics.DrawLine(pen2, point, point2);
                        point = new Point(pointArray[2].X, pointArray[2].Y + 2);
                        point2 = pointArray[3];
                        graphics.DrawLine(pen, point, point2);
                        using (brush2 = new SolidBrush(pen.Color))
                        {
                            graphics.FillRectangle(brush2, new Rectangle(point.X - 1, point.Y - 1, 1, 1));
                        }
                        point.Offset(-1, 0);
                        point2.Offset(-1, 0);
                        graphics.DrawLine(pen2, point, point2);
                        using (brush2 = new SolidBrush(pen2.Color))
                        {
                            graphics.FillRectangle(brush2, new Rectangle(point.X - 1, point.Y - 1, 1, 1));
                        }
                    }
                }
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            Tab item = e.Item as Tab;
            if (item != null)
            {
                e.TextFormat |= TextFormatFlags.EndEllipsis;
                e.TextRectangle = new Rectangle(14, e.Item.ContentRectangle.Top, (e.Item.ContentRectangle.Width + e.Item.ContentRectangle.Left) - 0x10, e.Item.ContentRectangle.Height);
                if (!e.Item.IsForeColorSet() && !e.ToolStrip.IsForeColorSet())
                {
                    if (item.Checked)
                    {
                        e.TextColor = this.TabColorTable.TabActiveText;
                    }
                    else if (item.Selected)
                    {
                        e.TextColor = this.TabColorTable.TabSelectedText;
                    }
                    else
                    {
                        e.TextColor = this.TabColorTable.TabInactiveText;
                    }
                }
                if ((item.Checked && this.UseBoldFont) && !e.TextFont.Bold)
                {
                    using (e.TextFont = new Font(e.TextFont, FontStyle.Bold))
                    {
                        base.OnRenderItemText(e);
                    }
                    return;
                }
            }
            base.OnRenderItemText(e);
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBackground(e);
            Rectangle affectedBounds = e.AffectedBounds;
            affectedBounds.Y = e.ToolStrip.DisplayRectangle.Bottom;
            affectedBounds.Height -= e.ToolStrip.DisplayRectangle.Bottom;
            using (Brush brush = new SolidBrush(this.TabColorTable.TabStripBottomBorder))
            {
                e.Graphics.FillRectangle(brush, affectedBounds);
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
                int bottom = toolStrip.DisplayRectangle.Bottom;
                int num2 = bottom + 1;
                using (Pen pen = new Pen(this.TabColorTable.TabActiveBorderOuter))
                {
                    using (Pen pen2 = new Pen(this.TabColorTable.TabBorderInner))
                    {
                        if (toolStrip.SelectedTab != null)
                        {
                            int left = toolStrip.SelectedTab.Bounds.Left;
                            int num4 = toolStrip.SelectedTab.Bounds.Right - 1;
                            e.Graphics.DrawLine(pen, e.AffectedBounds.Left, bottom, left, bottom);
                            e.Graphics.DrawLine(pen, num4, bottom, e.AffectedBounds.Right, bottom);
                            e.Graphics.DrawLine(pen2, e.AffectedBounds.Left, num2, left, num2);
                            e.Graphics.DrawLine(pen2, num4, num2, e.AffectedBounds.Right, num2);
                        }
                        else
                        {
                            e.Graphics.DrawLine(pen, e.AffectedBounds.Left, bottom, e.AffectedBounds.Right, bottom);
                            e.Graphics.DrawLine(pen2, e.AffectedBounds.Left, num2, e.AffectedBounds.Right, num2);
                        }
                    }
                }
            }
        }

        public TabStripColorTable TabColorTable { get; private set; }

        public bool UseBoldFont
        {
            get
            {
                return this.FUseBoldFont;
            }
            set
            {
                this.FUseBoldFont = value;
            }
        }
    }
}

