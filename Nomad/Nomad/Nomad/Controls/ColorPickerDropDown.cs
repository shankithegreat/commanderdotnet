namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [DesignerCategory("Code")]
    public class ColorPickerDropDown : ToolStripDropDown
    {
        public static KnownColor[] AdvancedColors = new KnownColor[] { 
            KnownColor.WhiteSmoke, KnownColor.DarkGray, KnownColor.Crimson, KnownColor.Orange, KnownColor.SpringGreen, KnownColor.Turquoise, KnownColor.RoyalBlue, KnownColor.Violet, KnownColor.Snow, KnownColor.LightGray, KnownColor.OrangeRed, KnownColor.Gold, KnownColor.LightGreen, KnownColor.MediumTurquoise, KnownColor.DodgerBlue, KnownColor.Pink, 
            KnownColor.Ivory, KnownColor.Gainsboro, KnownColor.Tomato, KnownColor.Khaki, KnownColor.PaleGreen, KnownColor.PaleTurquoise, KnownColor.SkyBlue, KnownColor.LightPink
         };
        private List<ToolStripButton> AdvancedColorsItems;
        private ToolStripLabel AdvancedColorsLabel;
        public System.Drawing.Color Color;
        public System.Drawing.Color DefaultColor;
        private ToolStripItem FDefaultColorItem;
        private ToolStripItem FMoreColorsItem;
        private static List<System.Drawing.Color> RecentColors;
        private Dictionary<System.Drawing.Color, ToolStripButton> RecentColorsItems;
        private ToolStripLabel RecentColorsLabel;
        private ToolStripItem SelectedColorButton;
        public static KnownColor[] StandardColors = new KnownColor[] { KnownColor.Black, KnownColor.Gray, KnownColor.Maroon, KnownColor.Olive, KnownColor.Green, KnownColor.Teal, KnownColor.Navy, KnownColor.Purple, KnownColor.White, KnownColor.Silver, KnownColor.Red, KnownColor.Yellow, KnownColor.Lime, KnownColor.Aqua, KnownColor.Blue, KnownColor.Fuchsia };
        private List<ToolStripButton> StandardColorsItems;
        private ToolStripLabel StandardColorsLabel;

        public event EventHandler ColorChanged;

        private void AddColors(KnownColor[] colors, ref List<ToolStripButton> colorsList)
        {
            if (colorsList == null)
            {
                colorsList = new List<ToolStripButton>(colors.Length);
                foreach (KnownColor color in colors)
                {
                    System.Drawing.Color color2 = System.Drawing.Color.FromKnownColor(color);
                    ToolStripButton colorButton = this.CreateColorButton(color2);
                    this.CheckColorButton(colorButton);
                    colorsList.Add(colorButton);
                    this.Items.Add(colorButton);
                }
            }
            else
            {
                foreach (ToolStripButton button in colorsList)
                {
                    this.CheckColorButton(button);
                    this.Items.Add(button);
                }
            }
        }

        private void CheckColorButton(ToolStripButton colorButton)
        {
            if (this.SelectedColorButton == null)
            {
                colorButton.Checked = ((System.Drawing.Color) colorButton.Tag) == this.Color;
                if (colorButton.Checked)
                {
                    this.SelectedColorButton = colorButton;
                }
            }
            else
            {
                colorButton.Checked = false;
            }
        }

        private void ColorItem_Click(object sender, EventArgs e)
        {
            this.Color = (System.Drawing.Color) ((ToolStripItem) sender).Tag;
            this.RaiseColorChanged();
        }

        private void ColorItem_Paint(object sender, PaintEventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            Rectangle rect = new Rectangle(3, (item.Height - base.ImageScalingSize.Height) / 2, base.ImageScalingSize.Width - 1, base.ImageScalingSize.Height - 1);
            System.Drawing.Color tag = (System.Drawing.Color) item.Tag;
            if (!tag.IsEmpty)
            {
                using (Brush brush = new SolidBrush(tag))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
            }
            else
            {
                e.Graphics.DrawLine(Pens.Black, rect.Left, rect.Top, rect.Right, rect.Bottom);
                e.Graphics.DrawLine(Pens.Black, rect.Left, rect.Bottom, rect.Right, rect.Top);
            }
            e.Graphics.DrawRectangle(Pens.Black, rect);
        }

        private ToolStripButton CreateColorButton(System.Drawing.Color color)
        {
            ToolStripButton button = new ToolStripButton(color.Name) {
                AutoSize = false,
                Size = new Size(base.ImageScalingSize.Width + 6, base.ImageScalingSize.Height + 4),
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Tag = color
            };
            button.Click += new EventHandler(this.ColorItem_Click);
            button.Paint += new PaintEventHandler(this.ColorItem_Paint);
            return button;
        }

        private void MoreColorsItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.Color = this.Color;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.Color = dialog.Color;
                    this.RaiseColorChanged();
                }
            }
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            e.Cancel = false;
            base.OnOpening(e);
            if (!e.Cancel)
            {
                base.SuspendLayout();
                this.Items.Clear();
                base.LayoutStyle = ToolStripLayoutStyle.Table;
                TableLayoutSettings layoutSettings = (TableLayoutSettings) base.LayoutSettings;
                layoutSettings.ColumnCount = 8;
                this.SelectedColorButton = null;
                this.DefaultColorItem.Tag = this.DefaultColor;
                this.Items.Add(this.DefaultColorItem);
                layoutSettings.SetColumnSpan(this.DefaultColorItem, layoutSettings.ColumnCount);
                if (this.StandardColorsLabel == null)
                {
                    this.StandardColorsLabel = new ToolStripLabel(Resource.sStandardColors);
                }
                this.Items.Add(this.StandardColorsLabel);
                layoutSettings.SetColumnSpan(this.StandardColorsLabel, layoutSettings.ColumnCount);
                this.AddColors(StandardColors, ref this.StandardColorsItems);
                if (this.AdvancedColorsLabel == null)
                {
                    this.AdvancedColorsLabel = new ToolStripLabel(Resource.sAdvancedColors);
                }
                this.Items.Add(this.AdvancedColorsLabel);
                layoutSettings.SetColumnSpan(this.AdvancedColorsLabel, layoutSettings.ColumnCount);
                this.AddColors(AdvancedColors, ref this.AdvancedColorsItems);
                if (!((this.SelectedColorButton != null) || this.Color.IsEmpty))
                {
                    UpdateRecentColors(this.Color);
                }
                if (RecentColors != null)
                {
                    if (this.RecentColorsLabel == null)
                    {
                        this.RecentColorsLabel = new ToolStripLabel(Resource.sRecentColors);
                    }
                    this.Items.Add(this.RecentColorsLabel);
                    layoutSettings.SetColumnSpan(this.RecentColorsLabel, layoutSettings.ColumnCount);
                    if (this.RecentColorsItems == null)
                    {
                        this.RecentColorsItems = new Dictionary<System.Drawing.Color, ToolStripButton>();
                    }
                    foreach (System.Drawing.Color color in RecentColors)
                    {
                        ToolStripButton button;
                        if (!this.RecentColorsItems.TryGetValue(color, out button))
                        {
                            button = this.CreateColorButton(color);
                            this.RecentColorsItems.Add(color, button);
                        }
                        this.CheckColorButton(button);
                        this.Items.Add(button);
                    }
                }
                this.Items.Add(this.MoreColorsItem);
                layoutSettings.SetColumnSpan(this.MoreColorsItem, layoutSettings.ColumnCount);
                base.ResumeLayout();
            }
        }

        protected void RaiseColorChanged()
        {
            if (this.ColorChanged != null)
            {
                this.ColorChanged(this, EventArgs.Empty);
            }
        }

        private static void UpdateRecentColors(System.Drawing.Color newRecentColor)
        {
            bool flag = true;
            if (RecentColors == null)
            {
                RecentColors = new List<System.Drawing.Color>();
            }
            else
            {
                foreach (System.Drawing.Color color in RecentColors)
                {
                    if (color == newRecentColor)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            if (flag)
            {
                RecentColors.Insert(0, newRecentColor);
            }
            while (RecentColors.Count > 8)
            {
                RecentColors.RemoveAt(8);
            }
        }

        private ToolStripItem DefaultColorItem
        {
            get
            {
                if (this.FDefaultColorItem == null)
                {
                    this.FDefaultColorItem = new ToolStripButton(Resource.sColorDefault);
                    this.FDefaultColorItem.AutoSize = false;
                    this.FDefaultColorItem.AutoToolTip = false;
                    this.FDefaultColorItem.TextAlign = ContentAlignment.MiddleRight;
                    this.FDefaultColorItem.Dock = DockStyle.Fill;
                    this.FDefaultColorItem.Padding = new Padding(0, 2, 0, 2);
                    this.FDefaultColorItem.Paint += new PaintEventHandler(this.ColorItem_Paint);
                    this.FDefaultColorItem.Click += new EventHandler(this.ColorItem_Click);
                }
                return this.FDefaultColorItem;
            }
        }

        protected override Padding DefaultPadding
        {
            get
            {
                return new Padding(2);
            }
        }

        private ToolStripItem MoreColorsItem
        {
            get
            {
                if (this.FMoreColorsItem == null)
                {
                    this.FMoreColorsItem = new ToolStripButton(Resource.sMoreColors);
                    this.FMoreColorsItem.AutoSize = false;
                    this.FMoreColorsItem.AutoToolTip = false;
                    this.FMoreColorsItem.Dock = DockStyle.Fill;
                    this.FMoreColorsItem.Padding = new Padding(0, 2, 0, 2);
                    this.FMoreColorsItem.Click += new EventHandler(this.MoreColorsItem_Click);
                }
                return this.FMoreColorsItem;
            }
        }
    }
}

