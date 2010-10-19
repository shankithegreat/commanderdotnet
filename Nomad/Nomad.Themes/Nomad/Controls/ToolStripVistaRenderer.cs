namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class ToolStripVistaRenderer : ToolStripProfessionalRenderer
    {
        private Color MenuItemSubBorder;
        private Color ToolStripDropDownBorder;

        public ToolStripVistaRenderer() : base(new VistaColorTable())
        {
            this.MenuItemSubBorder = Color.FromArgb(0x40, 0xff, 0xff, 0xff);
            this.ToolStripDropDownBorder = Color.FromArgb(0xf5, 0xf5, 0xf5);
            base.RoundedEdges = false;
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            using (GraphicsPath path = RoundRect(Rectangle.FromLTRB(e.ImageRectangle.Left, e.ImageRectangle.Top - 1, e.ImageRectangle.Right + 2, e.ImageRectangle.Bottom), 2f, 2f, 2f, 2f))
            {
                Color color = e.Item.Selected ? base.ColorTable.CheckSelectedBackground : base.ColorTable.CheckBackground;
                using (Brush brush = new SolidBrush(color))
                {
                    e.Graphics.FillPath(brush, path);
                }
                using (Pen pen = new Pen(base.ColorTable.ButtonCheckedHighlightBorder))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            if (e.Item is ToolStripMenuItem)
            {
                Rectangle imageRectangle = new Rectangle(e.ImageRectangle.Left + 2, e.ImageRectangle.Top, e.ImageRectangle.Width, e.ImageRectangle.Height);
                e = new ToolStripItemImageRenderEventArgs(e.Graphics, e.Item, e.Image, imageRectangle);
            }
            base.OnRenderItemImage(e);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) e.Item;
            if (e.Item.Selected || item.DropDown.Visible)
            {
                Pen pen;
                Brush brush;
                Rectangle rect = new Rectangle(0, 0, e.Item.Width, e.Item.Height);
                bool flag = false;
                if (item.DropDown.Visible && (item.Owner is MenuStrip))
                {
                    rect = Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right - 1, rect.Bottom);
                    brush = new SolidBrush(base.ColorTable.ToolStripDropDownBackground);
                    pen = new Pen(base.ColorTable.MenuBorder);
                }
                else
                {
                    if (e.ToolStrip is ToolStripDropDown)
                    {
                        rect = Rectangle.FromLTRB(rect.Left + 3, rect.Top, rect.Right - 3, rect.Bottom - 1);
                    }
                    else
                    {
                        rect = Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right - 1, rect.Bottom - 1);
                    }
                    brush = new LinearGradientBrush(rect, base.ColorTable.MenuItemSelectedGradientBegin, base.ColorTable.MenuItemSelectedGradientEnd, LinearGradientMode.Vertical);
                    pen = new Pen(base.ColorTable.MenuItemBorder);
                    flag = true;
                }
                using (GraphicsPath path = RoundRect(rect, 3f, 3f, 3f, 3f))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                    if (flag)
                    {
                        rect.Inflate(-1, -1);
                        using (GraphicsPath path2 = RoundRect(rect, 2f, 2f, 2f, 2f))
                        {
                            using (Pen pen2 = new Pen(this.MenuItemSubBorder))
                            {
                                e.Graphics.DrawPath(pen2, path2);
                            }
                        }
                    }
                }
                brush.Dispose();
                pen.Dispose();
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDown)
            {
                Rectangle rectangle = new Rectangle(0, 0, e.Item.Width, e.Item.Height);
                int num = ((rectangle.Height / 2) + rectangle.Top) - 1;
                using (Pen pen = new Pen(base.ColorTable.SeparatorDark))
                {
                    e.Graphics.DrawLine(pen, rectangle.Left + 30, num, rectangle.Right - 2, num);
                }
                using (Pen pen2 = new Pen(base.ColorTable.SeparatorLight))
                {
                    e.Graphics.DrawLine(pen2, (int) (rectangle.Left + 30), (int) (num + 1), (int) (rectangle.Right - 2), (int) (num + 1));
                }
            }
            else
            {
                base.OnRenderSeparator(e);
            }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDown)
            {
                Rectangle affectedBounds = e.AffectedBounds;
                using (Brush brush = new SolidBrush(this.ToolStripDropDownBorder))
                {
                    e.Graphics.FillRectangle(brush, affectedBounds);
                }
                affectedBounds.Inflate(-3, -3);
                using (Brush brush2 = new SolidBrush(base.ColorTable.ToolStripDropDownBackground))
                {
                    e.Graphics.FillRectangle(brush2, affectedBounds);
                }
                using (Pen pen = new Pen(base.ColorTable.SeparatorDark))
                {
                    e.Graphics.DrawLine(pen, affectedBounds.Left + 0x1b, affectedBounds.Top, affectedBounds.Left + 0x1b, affectedBounds.Bottom - 1);
                }
                using (Pen pen2 = new Pen(base.ColorTable.SeparatorLight))
                {
                    e.Graphics.DrawLine(pen2, affectedBounds.Left + 0x1c, affectedBounds.Top, affectedBounds.Left + 0x1c, affectedBounds.Bottom - 1);
                }
            }
            else
            {
                base.OnRenderToolStripBackground(e);
            }
        }

        private static GraphicsPath RoundRect(RectangleF r, float r1, float r2, float r3, float r4)
        {
            float x = r.X;
            float y = r.Y;
            float width = r.Width;
            float height = r.Height;
            GraphicsPath path = new GraphicsPath();
            path.AddBezier(x, y + r1, x, y, x + r1, y, x + r1, y);
            path.AddLine(x + r1, y, (x + width) - r2, y);
            path.AddBezier((x + width) - r2, y, x + width, y, x + width, y + r2, x + width, y + r2);
            path.AddLine((float) (x + width), (float) (y + r2), (float) (x + width), (float) ((y + height) - r3));
            path.AddBezier((float) (x + width), (float) ((y + height) - r3), (float) (x + width), (float) (y + height), (float) ((x + width) - r3), (float) (y + height), (float) ((x + width) - r3), (float) (y + height));
            path.AddLine((float) ((x + width) - r3), (float) (y + height), (float) (x + r4), (float) (y + height));
            path.AddBezier(x + r4, y + height, x, y + height, x, (y + height) - r4, x, (y + height) - r4);
            path.AddLine(x, (y + height) - r4, x, y + r1);
            return path;
        }
    }
}

