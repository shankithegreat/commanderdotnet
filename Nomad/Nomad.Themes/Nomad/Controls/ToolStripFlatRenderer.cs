namespace Nomad.Controls
{
    using Microsoft;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class ToolStripFlatRenderer : ToolStripSystemRenderer
    {
        private static Brush FCheckBrush;

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            ToolStripSplitButton item = e.Item as ToolStripSplitButton;
            if (!((e.Item is ToolStripMenuItem) || !((item != null) ? item.ButtonPressed : e.Item.Pressed)))
            {
                Rectangle arrowRectangle = e.ArrowRectangle;
                arrowRectangle.Offset(1, 1);
                e.ArrowRectangle = arrowRectangle;
            }
            if (!e.Item.Enabled)
            {
                e.ArrowColor = SystemColors.GrayText;
            }
            base.OnRenderArrow(e);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripButton item = (ToolStripButton) e.Item;
            Rectangle rectangle = new Rectangle(Point.Empty, e.Item.Size);
            if (e.Item.Pressed || item.Checked)
            {
                ControlPaint.DrawBorder3D(e.Graphics, rectangle, Border3DStyle.SunkenOuter);
            }
            else if (e.Item.Selected)
            {
                ControlPaint.DrawBorder3D(e.Graphics, rectangle, Border3DStyle.RaisedInner);
            }
            if (!(!item.Checked || e.Item.Selected))
            {
                rectangle.Inflate(-1, -1);
                e.Graphics.FillRectangle(CheckBrush, rectangle);
            }
        }

        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rectangle = new Rectangle(Point.Empty, e.Item.Size);
            if (e.Item.Pressed)
            {
                ControlPaint.DrawBorder3D(e.Graphics, rectangle, Border3DStyle.SunkenOuter);
            }
            else if (e.Item.Selected)
            {
                ControlPaint.DrawBorder3D(e.Graphics, rectangle, Border3DStyle.RaisedInner);
            }
        }

        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            ToolStripSplitButton item = e.Item as ToolStripSplitButton;
            if (!((e.Item is ToolStripMenuItem) || !((item != null) ? item.ButtonPressed : e.Item.Pressed)))
            {
                Rectangle imageRectangle = e.ImageRectangle;
                imageRectangle.Offset(1, 1);
                e = new ToolStripItemImageRenderEventArgs(e.Graphics, e.Item, e.Image, imageRectangle);
            }
            base.OnRenderItemImage(e);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            Rectangle textRectangle = e.TextRectangle;
            if (((e.ToolStrip is ToolStripDropDown) && (e.Item is ToolStripMenuItem)) && (textRectangle.Left < 40))
            {
                textRectangle = Rectangle.FromLTRB(0x18, textRectangle.Top, textRectangle.Right, textRectangle.Bottom);
            }
            if (!e.Item.Enabled)
            {
                textRectangle.Offset(1, 1);
                TextRenderer.DrawText(e.Graphics, e.Text, e.TextFont, textRectangle, SystemColors.ControlLightLight, e.TextFormat);
                textRectangle.Offset(-1, -1);
                TextRenderer.DrawText(e.Graphics, e.Text, e.TextFont, textRectangle, SystemColors.GrayText, e.TextFormat);
            }
            else
            {
                ToolStripSplitButton button = e.Item as ToolStripSplitButton;
                if (!((e.Item is ToolStripMenuItem) || !((button != null) ? button.ButtonPressed : e.Item.Pressed)))
                {
                    textRectangle.Offset(1, 1);
                }
                e.TextRectangle = textRectangle;
                ToolStripMenuItem item = e.Item as ToolStripMenuItem;
                if ((item != null) && (item.Selected || item.DropDown.Visible))
                {
                    e.TextColor = SystemColors.HighlightText;
                }
                base.OnRenderItemText(e);
            }
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) e.Item;
            if (e.ToolStrip is ToolStripDropDown)
            {
                bool flag = item.Checked;
                Rectangle rect = new Rectangle(3, 1, 20, 20);
                if (flag)
                {
                    if (!e.Item.Selected)
                    {
                        e.Graphics.FillRectangle(CheckBrush, rect);
                    }
                    ControlPaint.DrawBorder3D(e.Graphics, rect, Border3DStyle.SunkenOuter);
                }
                if ((e.Item.Selected || item.DropDown.Visible) && e.Item.Enabled)
                {
                    bool flag2 = (e.Item.Image != null) && (e.Item.DisplayStyle == ToolStripItemDisplayStyle.ImageAndText);
                    Rectangle rectangle2 = Rectangle.FromLTRB((flag || flag2) ? 0x18 : 3, 1, e.Item.Size.Width - 2, e.Item.Size.Height - 1);
                    e.Graphics.FillRectangle(SystemBrushes.MenuHighlight, rectangle2);
                    if (!(!flag2 || flag))
                    {
                        ControlPaint.DrawBorder3D(e.Graphics, rect, Border3DStyle.RaisedInner);
                    }
                }
            }
            else if (e.Item.Selected || item.DropDown.Visible)
            {
                e.Graphics.FillRectangle(SystemBrushes.MenuHighlight, 0, 0, e.Item.Size.Width, e.Item.Height - 1);
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            Rectangle rectangle;
            if (e.Vertical)
            {
                rectangle = new Rectangle((e.Item.Size.Width - 2) / 2, 1, 2, e.Item.Size.Height - 4);
            }
            else
            {
                rectangle = new Rectangle(2, (e.Item.Size.Height - 2) / 2, e.Item.Size.Width - 5, 2);
            }
            ControlPaint.DrawBorder3D(e.Graphics, rectangle, Border3DStyle.Etched, e.Vertical ? Border3DSide.Left : Border3DSide.Top);
        }

        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripSplitButton item = (ToolStripSplitButton) e.Item;
            if (item.ButtonPressed)
            {
                ControlPaint.DrawBorder3D(e.Graphics, new Rectangle(Point.Empty, e.Item.Size), Border3DStyle.SunkenOuter);
            }
            else
            {
                base.DrawDropDownButtonBackground(e);
            }
            if (!(item.ButtonPressed || (!item.DropDownButtonPressed && !e.Item.Selected)))
            {
                Rectangle splitterBounds = item.SplitterBounds;
                splitterBounds.Inflate(1, -2);
                splitterBounds.Offset(1, 0);
                ControlPaint.DrawBorder3D(e.Graphics, splitterBounds, Border3DStyle.Etched, Border3DSide.Left);
            }
            base.DrawArrow(new ToolStripArrowRenderEventArgs(e.Graphics, e.Item, item.DropDownButtonBounds, SystemColors.MenuText, ArrowDirection.Down));
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDown)
            {
                e.Graphics.FillRectangle(OS.IsWinXP ? SystemBrushes.MenuBar : SystemBrushes.Menu, e.AffectedBounds);
            }
            else
            {
                base.OnRenderToolStripBackground(e);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDown)
            {
                ControlPaint.DrawBorder3D(e.Graphics, e.AffectedBounds, Border3DStyle.Raised);
            }
            else
            {
                ControlPaint.DrawBorder3D(e.Graphics, e.AffectedBounds, Border3DStyle.Etched, Border3DSide.Bottom);
            }
        }

        protected static Brush CheckBrush
        {
            get
            {
                if (FCheckBrush == null)
                {
                    using (Bitmap bitmap = new Bitmap(2, 2))
                    {
                        bitmap.SetPixel(0, 0, SystemColors.ControlLightLight);
                        bitmap.SetPixel(1, 0, SystemColors.ButtonFace);
                        bitmap.SetPixel(1, 1, SystemColors.ControlLightLight);
                        bitmap.SetPixel(0, 1, SystemColors.ButtonFace);
                        FCheckBrush = new TextureBrush(bitmap);
                    }
                }
                return FCheckBrush;
            }
        }
    }
}

