namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class TabStripVS2k3Renderer : ToolStripFlatRenderer
    {
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            TabStrip toolStrip = e.ToolStrip as TabStrip;
            Tab item = e.Item as Tab;
            Rectangle rect = new Rectangle(0, 0, e.Item.Size.Width - 1, e.Item.Size.Height);
            if ((toolStrip != null) && (item != null))
            {
                if (item.Checked)
                {
                    e.Graphics.FillRectangle(SystemBrushes.ButtonFace, rect);
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, rect.Left, rect.Top, rect.Left, rect.Bottom - 1);
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, rect.Left, rect.Top, rect.Right, rect.Top);
                    e.Graphics.DrawLine(SystemPens.ControlDarkDark, rect.Right, rect.Top + 1, rect.Right, rect.Bottom - 1);
                }
                else if (item != toolStrip.GetNextTab(toolStrip.SelectedTab, false, false))
                {
                    e.Graphics.DrawLine(SystemPens.ControlDark, rect.Right, rect.Top + 2, rect.Right, rect.Bottom - 3);
                }
            }
            else
            {
                if (e.Item.Selected || e.Item.Pressed)
                {
                    e.Graphics.FillRectangle(SystemBrushes.ButtonFace, rect);
                }
                base.OnRenderButtonBackground(e);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            Tab item = e.Item as Tab;
            if (item != null)
            {
                e.TextRectangle = e.Item.ContentRectangle;
                e.TextFormat |= TextFormatFlags.EndEllipsis;
                if (item.Pressed)
                {
                    Rectangle textRectangle = e.TextRectangle;
                    textRectangle.Offset(-1, -1);
                    e.TextRectangle = textRectangle;
                }
                if (!((item.Checked || e.Item.IsForeColorSet()) || e.ToolStrip.IsForeColorSet()))
                {
                    e.TextColor = SystemColors.GrayText;
                }
                if (item.Checked && !e.TextFont.Bold)
                {
                    using (e.TextFont = new Font(e.TextFont, FontStyle.Bold))
                    {
                        if (TextRenderer.MeasureText(e.Graphics, e.Text, e.TextFont).Width <= e.TextRectangle.Width)
                        {
                            e.TextFormat &= ~TextFormatFlags.EndEllipsis;
                        }
                        base.OnRenderItemText(e);
                    }
                    return;
                }
            }
            base.OnRenderItemText(e);
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(ToolStripFlatRenderer.CheckBrush, e.AffectedBounds);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            TabStrip toolStrip = e.ToolStrip as TabStrip;
            Rectangle rectangle = new Rectangle(e.AffectedBounds.Left, e.AffectedBounds.Top, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1);
            e.Graphics.DrawLine(SystemPens.ButtonFace, rectangle.Left, rectangle.Top, rectangle.Left, rectangle.Bottom);
            e.Graphics.DrawLine(SystemPens.ButtonFace, rectangle.Right, rectangle.Top, rectangle.Right, rectangle.Bottom);
            if ((toolStrip == null) || (toolStrip.SelectedTab == null))
            {
                e.Graphics.DrawLine(SystemPens.ControlLightLight, rectangle.Left + 1, rectangle.Bottom, rectangle.Right - 1, rectangle.Bottom);
            }
            else
            {
                e.Graphics.DrawLine(SystemPens.ControlLightLight, rectangle.Left + 1, rectangle.Bottom, toolStrip.SelectedTab.Bounds.Left - 1, rectangle.Bottom);
                e.Graphics.DrawLine(SystemPens.ControlLightLight, toolStrip.SelectedTab.Bounds.Right, rectangle.Bottom, rectangle.Right - 1, rectangle.Bottom);
            }
        }
    }
}

