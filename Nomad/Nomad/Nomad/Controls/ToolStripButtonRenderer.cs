namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class ToolStripButtonRenderer : ToolStripRenderer
    {
        private static PushButtonState ConvertToolBarStateToPushButtonState(ToolBarState buttonState)
        {
            switch (buttonState)
            {
                case ToolBarState.Hot:
                    return PushButtonState.Hot;

                case ToolBarState.Pressed:
                case ToolBarState.Checked:
                    return PushButtonState.Pressed;

                case ToolBarState.Disabled:
                    return PushButtonState.Disabled;
            }
            return PushButtonState.Normal;
        }

        private static ToolBarState GetToolStripItemState(ToolStripItem item)
        {
            if (item != null)
            {
                if (!item.Enabled)
                {
                    return ToolBarState.Disabled;
                }
                ToolStripButton button = item as ToolStripButton;
                if ((button != null) && button.Checked)
                {
                    return ToolBarState.Checked;
                }
                if (item.Pressed)
                {
                    return ToolBarState.Pressed;
                }
                if (item.Selected)
                {
                    return ToolBarState.Hot;
                }
            }
            return ToolBarState.Normal;
        }

        protected override void InitializeItem(ToolStripItem item)
        {
            base.InitializeItem(item);
            item.Height = Math.Max(0x17, item.Height);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ButtonRenderer.DrawButton(e.Graphics, new Rectangle(Point.Empty, e.Item.Size), ConvertToolBarStateToPushButtonState(GetToolStripItemState(e.Item)));
        }

        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            Rectangle imageRectangle = e.ImageRectangle;
            Image normalImage = e.Image;
            if (!imageRectangle.IsEmpty && (normalImage != null))
            {
                bool flag = false;
                if (!e.Item.Enabled)
                {
                    normalImage = ToolStripRenderer.CreateDisabledImage(normalImage);
                    flag = true;
                }
                if (e.Item.ImageScaling == ToolStripItemImageScaling.None)
                {
                    e.Graphics.DrawImage(normalImage, imageRectangle.Location);
                }
                else
                {
                    e.Graphics.DrawImage(normalImage, imageRectangle);
                }
                if (flag)
                {
                    normalImage.Dispose();
                }
            }
        }
    }
}

