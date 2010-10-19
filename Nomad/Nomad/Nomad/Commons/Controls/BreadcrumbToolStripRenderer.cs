namespace Nomad.Commons.Controls
{
    using Nomad.Controls;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class BreadcrumbToolStripRenderer : ToolStripWrapperRenderer
    {
        public BreadcrumbToolStripRenderer()
        {
        }

        public BreadcrumbToolStripRenderer(ToolStripRenderer baseRenderer) : base(baseRenderer)
        {
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            ToolStripDropDownItem item = e.Item as ToolStripDropDownItem;
            if (!(((item == null) || (e.Direction != ArrowDirection.Down)) || item.DropDown.Visible))
            {
                e.Direction = ArrowDirection.Right;
            }
            base.OnRenderArrow(e);
        }

        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripSplitButton item = (ToolStripSplitButton) e.Item;
            if ((e.Item.Selected || item.ButtonPressed) || item.DropDownButtonPressed)
            {
                if (!item.DropDownButtonPressed)
                {
                    Rectangle dropDownButtonBounds = item.DropDownButtonBounds;
                    dropDownButtonBounds = new Rectangle(dropDownButtonBounds.Left + 1, dropDownButtonBounds.Top + 5, dropDownButtonBounds.Width - 3, dropDownButtonBounds.Height - 10);
                    using (Region region = new Region(new Rectangle(Point.Empty, e.Item.Size)))
                    {
                        region.Xor(dropDownButtonBounds);
                        e.Graphics.SetClip(region, CombineMode.Replace);
                        base.OnRenderSplitButtonBackground(e);
                        e.Graphics.SetClip(dropDownButtonBounds);
                        e.Graphics.TranslateTransform((float) item.DropDownButtonBounds.Width, 0f);
                        if (item.ButtonPressed && (base.BaseRenderer is ToolStripProfessionalRenderer))
                        {
                            using (ToolStripButton button2 = new ToolStripButton())
                            {
                                button2.Select();
                                button2.Size = e.Item.Size;
                                base.OnRenderButtonBackground(new ToolStripItemRenderEventArgs(e.Graphics, button2));
                            }
                        }
                        else
                        {
                            base.OnRenderSplitButtonBackground(e);
                        }
                        e.Graphics.ResetTransform();
                        e.Graphics.ResetClip();
                    }
                }
                else
                {
                    base.OnRenderSplitButtonBackground(e);
                }
            }
            if (!item.DropDownButtonPressed)
            {
                this.OnRenderArrow(new ToolStripArrowRenderEventArgs(e.Graphics, e.Item, item.DropDownButtonBounds, Color.Black, ArrowDirection.Right));
            }
        }
    }
}

