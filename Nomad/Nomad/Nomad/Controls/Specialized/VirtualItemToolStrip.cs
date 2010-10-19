namespace Nomad.Controls.Specialized
{
    using Nomad.Commons.Drawing;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.Windows.Forms.Layout;

    [DesignerCategory("Code")]
    public class VirtualItemToolStrip : ToolStrip
    {
        private IContainer components;
        private System.Windows.Forms.Layout.LayoutEngine Engine;

        public VirtualItemToolStrip(IContainer container)
        {
            this.components = container;
        }

        public void Add(IVirtualItem item)
        {
            ToolStripLabel label = this.CreateItemLabel(item);
            if (this.Items.Count == 0)
            {
                base.Renderer = new ItemRenderer();
                base.LayoutStyle = ToolStripLayoutStyle.Table;
                TableLayoutSettings layoutSettings = (TableLayoutSettings) base.LayoutSettings;
                layoutSettings.ColumnCount = 1;
                layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                label.Text = PathHelper.ExcludeTrailingDirectorySeparator(item.FullName);
                label.AutoSize = false;
                label.Dock = DockStyle.Fill;
                if (Settings.Default.ShowItemToolTips)
                {
                    label.MouseHover += new EventHandler(this.tslItem_MouseHover);
                }
            }
            else
            {
                if (this.Items.Count == 1)
                {
                    ToolStripItem item2 = this.Items[0];
                    item2.AutoSize = true;
                    item2.Dock = DockStyle.None;
                }
                base.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
                if (Settings.Default.ShowItemToolTips)
                {
                    label.MouseHover += new EventHandler(this.tslItem_MouseHover);
                }
            }
            this.Items.Add(label);
            if (this.components != null)
            {
                this.components.Add(label);
            }
        }

        public void AddRange(IEnumerable<IVirtualItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException();
            }
            base.SuspendLayout();
            foreach (IVirtualItem item in items)
            {
                this.Add(item);
            }
            base.ResumeLayout();
        }

        private ToolStripLabel CreateItemLabel(IVirtualItem item)
        {
            ToolStripLabel label = new ToolStripLabel {
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 3, 0, 3),
                Tag = item
            };
            label.MouseUp += new MouseEventHandler(VirtualItemToolStripEvents.MouseUp);
            label.MouseLeave += new EventHandler(VirtualItemToolStripEvents.MouseLeave);
            Color foreColor = VirtualItemHelper.GetForeColor(item, Color.Empty);
            if (!foreColor.IsEmpty)
            {
                label.ForeColor = foreColor;
                label.Paint += new PaintEventHandler(VirtualItemToolStripEvents.PaintForeColor);
            }
            if (Settings.Default.IsShowIcons)
            {
                label.ImageAlign = ContentAlignment.MiddleLeft;
                label.Image = VirtualIcon.GetIcon(item, ImageHelper.DefaultSmallIconSize);
            }
            return label;
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.BackColor = base.Parent.BackColor;
            base.OnParentBackColorChanged(e);
        }

        private void tslItem_MouseHover(object sender, EventArgs e)
        {
            Form activeForm = Form.ActiveForm;
            if ((Settings.Default.ShowItemToolTips && (activeForm != null)) && (activeForm == base.FindForm()))
            {
                ToolStripItem item = (ToolStripItem) sender;
                IVirtualItem tag = (IVirtualItem) item.Tag;
                if ((tag != null) && (item.Owner != null))
                {
                    IVirtualItemUI mui;
                    if (base.LayoutStyle == ToolStripLayoutStyle.Table)
                    {
                        StringBuilder builder = new StringBuilder();
                        Size proposedSize = item.ContentRectangle.Size;
                        if (item.Image != null)
                        {
                            proposedSize.Width -= item.Image.Width;
                        }
                        if (TextRenderer.MeasureText(PathHelper.ExcludeTrailingDirectorySeparator(tag.FullName), item.Font, proposedSize, TextFormatFlags.NoPrefix).Width > proposedSize.Width)
                        {
                            builder.Append(tag.FullName);
                        }
                        mui = tag as IVirtualItemUI;
                        if (mui != null)
                        {
                            string toolTip = mui.ToolTip;
                            if (!string.IsNullOrEmpty(toolTip))
                            {
                                if (builder.Length > 0)
                                {
                                    builder.AppendLine();
                                }
                                builder.Append(toolTip);
                            }
                        }
                        if (builder.Length > 0)
                        {
                            Point position = Cursor.Position;
                            VirtualToolTip.Default.ShowTooltip(tag, builder.ToString(), null, position.X, position.Y + item.Owner.Cursor.GetRealHeight());
                        }
                    }
                    else
                    {
                        mui = tag as IVirtualItemUI;
                        if (mui != null)
                        {
                            VirtualToolTip.Default.ShowTooltip(mui);
                        }
                    }
                }
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {
                if (!((base.Parent == null) || this.IsFontSet()))
                {
                    return base.Parent.Font;
                }
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        public override System.Windows.Forms.Layout.LayoutEngine LayoutEngine
        {
            get
            {
                if ((base.LayoutStyle == ToolStripLayoutStyle.Table) || base.DesignMode)
                {
                    return base.LayoutEngine;
                }
                if (this.Engine == null)
                {
                    this.Engine = new ItemLayoutEngine();
                }
                return this.Engine;
            }
        }

        private class ItemLayoutEngine : LayoutEngine
        {
            public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
            {
                VirtualItemToolStrip strip = (VirtualItemToolStrip) container;
                if (!((strip.Items.Count != 0) && strip.Visible))
                {
                    return false;
                }
                Rectangle displayRectangle = strip.DisplayRectangle;
                Point location = displayRectangle.Location;
                int num = 0;
                while (num < strip.Items.Count)
                {
                    ToolStripItem item = strip.Items[num];
                    item.Visible = true;
                    item.Text = ((IVirtualItem) item.Tag).Name.Replace("&", "&&") + ",";
                    item.Size = item.GetPreferredSize(displayRectangle.Size);
                    if ((location.X + item.Width) > displayRectangle.Width)
                    {
                        break;
                    }
                    location.Y = (displayRectangle.Height - item.Height) / 2;
                    strip.SetItemLocation(item, location);
                    location.X += item.Width;
                    num++;
                }
                while ((num > 0) && (num < strip.Items.Count))
                {
                    ToolStripItem item2 = strip.Items[num - 1];
                    item2.Text = item2.Text + " ...";
                    item2.Size = item2.GetPreferredSize(displayRectangle.Size);
                    if (item2.Bounds.Right <= displayRectangle.Width)
                    {
                        break;
                    }
                    num--;
                }
                while (num < strip.Items.Count)
                {
                    strip.Items[num].Visible = false;
                    num++;
                }
                return strip.AutoSize;
            }
        }

        private class ItemRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
            {
                if ((e.Image != null) && (e.ImageRectangle.Width < e.Image.Width))
                {
                    e = new ToolStripItemImageRenderEventArgs(e.Graphics, e.Item, e.Image, new Rectangle(e.ImageRectangle.Location, new Size(e.Image.Width, e.Image.Height)));
                }
                base.OnRenderItemImage(e);
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextFormat |= TextFormatFlags.PathEllipsis | TextFormatFlags.NoPrefix;
                Rectangle contentRectangle = e.Item.ContentRectangle;
                contentRectangle.Y = e.TextRectangle.Y;
                contentRectangle.Height = e.TextRectangle.Height;
                if (e.Item.Image != null)
                {
                    contentRectangle.X += e.Item.Image.Width;
                    contentRectangle.Width -= e.Item.Image.Width;
                }
                e.TextRectangle = contentRectangle;
                base.OnRenderItemText(e);
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDown)
                {
                    base.OnRenderToolStripBackground(e);
                }
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDown)
                {
                    base.OnRenderToolStripBorder(e);
                }
            }
        }
    }
}

