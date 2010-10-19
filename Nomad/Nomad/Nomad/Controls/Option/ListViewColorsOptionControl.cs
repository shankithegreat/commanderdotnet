namespace Nomad.Controls.Option
{
    using Microsoft;
    using Nomad;
    using Nomad.Commons.Collections;
    using Nomad.Controls;
    using Nomad.Properties;
    using Nomad.Themes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class ListViewColorsOptionControl : UserControl, IPersistComponentSettings
    {
        private Label boxActiveBackColor;
        private Label boxBackColor;
        private Label boxFocusedBackColor;
        private Label boxFocusedForeColor;
        private Label boxForeColor;
        private Label boxOddLineBackColor;
        private Label boxSelectedForeColor;
        private ColorButton btnActiveBackColor;
        private ColorButton btnBackColor;
        private ColorButton btnFocusedBackColor;
        private ColorButton btnFocusedForeColor;
        private ColorButton btnForeColor;
        private ColorButton btnOddLineBackColor;
        private ColorButton btnSelectedForeColor;
        private CheckBox chkChangeFont;
        private CheckBox chkFontBold;
        private ComboBox cmbFontFamily;
        private ComboBox cmbFontSize;
        private Label[] ColorBoxes;
        private Button[] ColorButtons;
        private IContainer components = null;
        private Label lblFontFamily;
        private Label lblFontSize;

        public ListViewColorsOptionControl()
        {
            this.InitializeComponent();
            this.btnBackColor.Tag = KnownListViewColor.Back;
            this.btnOddLineBackColor.Tag = KnownListViewColor.OddLineBack;
            this.btnActiveBackColor.Tag = KnownListViewColor.ActiveBack;
            this.btnForeColor.Tag = KnownListViewColor.Text;
            this.btnFocusedBackColor.Tag = KnownListViewColor.FocusedBack;
            this.btnFocusedForeColor.Tag = KnownListViewColor.FocusedText;
            this.btnSelectedForeColor.Tag = KnownListViewColor.SelectedText;
            this.ColorButtons = new ColorButton[] { this.btnBackColor, this.btnOddLineBackColor, this.btnActiveBackColor, this.btnForeColor, this.btnFocusedBackColor, this.btnFocusedForeColor, this.btnSelectedForeColor };
            this.boxBackColor.Tag = this.btnBackColor;
            this.boxOddLineBackColor.Tag = this.btnOddLineBackColor;
            this.boxActiveBackColor.Tag = this.btnActiveBackColor;
            this.boxForeColor.Tag = this.btnForeColor;
            this.boxFocusedBackColor.Tag = this.btnFocusedBackColor;
            this.boxFocusedForeColor.Tag = this.btnFocusedForeColor;
            this.boxSelectedForeColor.Tag = this.btnSelectedForeColor;
            this.ColorBoxes = new Label[] { this.boxBackColor, this.boxOddLineBackColor, this.boxActiveBackColor, this.boxForeColor, this.boxFocusedBackColor, this.boxFocusedForeColor, this.boxSelectedForeColor };
            foreach (ColorButton button in this.ColorButtons)
            {
                button.DefaultColor = Color.Empty;
                if (!Application.RenderWithVisualStyles)
                {
                    button.BackColor = SystemColors.Control;
                }
            }
            this.cmbFontFamily.DataSource = FontFamily.Families;
            this.cmbFontFamily.DataBindings.Add(new Binding("SelectedItem", VirtualFilePanelSettings.Default.ListFont, "FontFamily", false, DataSourceUpdateMode.Never));
            this.cmbFontSize.DataBindings.Add(new Binding("Text", VirtualFilePanelSettings.Default.ListFont, "Size", true, DataSourceUpdateMode.Never));
            this.chkFontBold.DataBindings.Add(new Binding("Checked", VirtualFilePanelSettings.Default.ListFont, "Bold", false, DataSourceUpdateMode.Never));
        }

        private void chkChangeFont_CheckedChanged(object sender, EventArgs e)
        {
            this.lblFontFamily.Enabled = this.chkChangeFont.Checked;
            this.cmbFontFamily.Enabled = this.chkChangeFont.Checked;
            this.lblFontSize.Enabled = this.chkChangeFont.Checked;
            this.cmbFontSize.Enabled = this.chkChangeFont.Checked;
            this.chkFontBold.Enabled = this.chkChangeFont.Checked;
        }

        private void cmbFontSize_Enter(object sender, EventArgs e)
        {
            this.cmbFontSize.Tag = this.cmbFontSize.Text;
        }

        private void cmbFontSize_Leave(object sender, EventArgs e)
        {
            float num;
            if (!float.TryParse(this.cmbFontSize.Text, out num))
            {
                this.cmbFontSize.Text = (string) this.cmbFontSize.Tag;
            }
        }

        private void ColorButton_ColorChanged(object sender, EventArgs e)
        {
            this.UpdateColorBoxes();
        }

        private void ColorButton_EnabledChanged(object sender, EventArgs e)
        {
            Control ctl = (Control) sender;
            Label nextControl = base.GetNextControl(ctl, false) as Label;
            if (nextControl != null)
            {
                nextControl.Enabled = ctl.Enabled;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ListViewColorsOptionControl));
            this.boxFocusedForeColor = new Label();
            this.boxFocusedBackColor = new Label();
            this.boxSelectedForeColor = new Label();
            this.boxForeColor = new Label();
            this.boxOddLineBackColor = new Label();
            this.boxActiveBackColor = new Label();
            this.chkChangeFont = new CheckBox();
            this.btnOddLineBackColor = new ColorButton();
            this.btnFocusedBackColor = new ColorButton();
            this.lblFontFamily = new Label();
            this.btnForeColor = new ColorButton();
            this.lblFontSize = new Label();
            this.btnBackColor = new ColorButton();
            this.cmbFontFamily = new ComboBox();
            this.cmbFontSize = new ComboBox();
            this.chkFontBold = new CheckBox();
            this.btnActiveBackColor = new ColorButton();
            this.btnFocusedForeColor = new ColorButton();
            this.btnSelectedForeColor = new ColorButton();
            this.boxBackColor = new Label();
            TableLayoutPanel panel = new TableLayoutPanel();
            Label control = new Label();
            Label label2 = new Label();
            Label label3 = new Label();
            Label label4 = new Label();
            Label label5 = new Label();
            Label label6 = new Label();
            Label label7 = new Label();
            panel.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.boxFocusedForeColor, 4, 9);
            panel.Controls.Add(this.boxFocusedBackColor, 4, 8);
            panel.Controls.Add(this.boxSelectedForeColor, 4, 7);
            panel.Controls.Add(this.boxForeColor, 4, 6);
            panel.Controls.Add(this.boxOddLineBackColor, 4, 5);
            panel.Controls.Add(this.boxActiveBackColor, 4, 4);
            panel.Controls.Add(this.chkChangeFont, 0, 0);
            panel.Controls.Add(this.btnOddLineBackColor, 3, 5);
            panel.Controls.Add(this.btnFocusedBackColor, 3, 8);
            panel.Controls.Add(this.lblFontFamily, 1, 1);
            panel.Controls.Add(this.btnForeColor, 3, 6);
            panel.Controls.Add(this.lblFontSize, 1, 2);
            panel.Controls.Add(this.btnBackColor, 3, 3);
            panel.Controls.Add(control, 0, 3);
            panel.Controls.Add(label2, 0, 8);
            panel.Controls.Add(label3, 0, 6);
            panel.Controls.Add(this.cmbFontFamily, 2, 1);
            panel.Controls.Add(this.cmbFontSize, 2, 2);
            panel.Controls.Add(label4, 0, 5);
            panel.Controls.Add(this.chkFontBold, 3, 2);
            panel.Controls.Add(this.btnActiveBackColor, 3, 4);
            panel.Controls.Add(label5, 0, 4);
            panel.Controls.Add(this.btnFocusedForeColor, 3, 9);
            panel.Controls.Add(label6, 0, 9);
            panel.Controls.Add(this.btnSelectedForeColor, 3, 7);
            panel.Controls.Add(label7, 0, 7);
            panel.Controls.Add(this.boxBackColor, 4, 3);
            panel.Name = "tlpBack";
            this.boxFocusedForeColor.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.boxFocusedForeColor, "boxFocusedForeColor");
            this.boxFocusedForeColor.Name = "boxFocusedForeColor";
            this.boxFocusedBackColor.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.boxFocusedBackColor, "boxFocusedBackColor");
            this.boxFocusedBackColor.Name = "boxFocusedBackColor";
            this.boxSelectedForeColor.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.boxSelectedForeColor, "boxSelectedForeColor");
            this.boxSelectedForeColor.Name = "boxSelectedForeColor";
            this.boxForeColor.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.boxForeColor, "boxForeColor");
            this.boxForeColor.Name = "boxForeColor";
            this.boxOddLineBackColor.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.boxOddLineBackColor, "boxOddLineBackColor");
            this.boxOddLineBackColor.Name = "boxOddLineBackColor";
            this.boxActiveBackColor.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.boxActiveBackColor, "boxActiveBackColor");
            this.boxActiveBackColor.Name = "boxActiveBackColor";
            manager.ApplyResources(this.chkChangeFont, "chkChangeFont");
            panel.SetColumnSpan(this.chkChangeFont, 4);
            this.chkChangeFont.Name = "chkChangeFont";
            this.chkChangeFont.UseVisualStyleBackColor = true;
            this.chkChangeFont.CheckedChanged += new EventHandler(this.chkChangeFont_CheckedChanged);
            this.btnOddLineBackColor.Image = null;
            manager.ApplyResources(this.btnOddLineBackColor, "btnOddLineBackColor");
            this.btnOddLineBackColor.Name = "btnOddLineBackColor";
            this.btnOddLineBackColor.UseVisualStyleBackColor = true;
            this.btnOddLineBackColor.ColorChanged += new EventHandler(this.ColorButton_ColorChanged);
            this.btnFocusedBackColor.Image = null;
            manager.ApplyResources(this.btnFocusedBackColor, "btnFocusedBackColor");
            this.btnFocusedBackColor.Name = "btnFocusedBackColor";
            this.btnFocusedBackColor.UseVisualStyleBackColor = true;
            this.btnFocusedBackColor.ColorChanged += new EventHandler(this.ColorButton_ColorChanged);
            this.btnFocusedBackColor.EnabledChanged += new EventHandler(this.ColorButton_EnabledChanged);
            manager.ApplyResources(this.lblFontFamily, "lblFontFamily");
            this.lblFontFamily.Name = "lblFontFamily";
            this.btnForeColor.Image = null;
            manager.ApplyResources(this.btnForeColor, "btnForeColor");
            this.btnForeColor.Name = "btnForeColor";
            this.btnForeColor.UseVisualStyleBackColor = true;
            this.btnForeColor.ColorChanged += new EventHandler(this.ColorButton_ColorChanged);
            manager.ApplyResources(this.lblFontSize, "lblFontSize");
            this.lblFontSize.Name = "lblFontSize";
            this.btnBackColor.DefaultColor = SystemColors.Window;
            this.btnBackColor.Image = null;
            manager.ApplyResources(this.btnBackColor, "btnBackColor");
            this.btnBackColor.Name = "btnBackColor";
            this.btnBackColor.UseVisualStyleBackColor = true;
            this.btnBackColor.ColorChanged += new EventHandler(this.ColorButton_ColorChanged);
            manager.ApplyResources(control, "lblBackColor");
            panel.SetColumnSpan(control, 3);
            control.Name = "lblBackColor";
            manager.ApplyResources(label2, "lblFocusedBackColor");
            panel.SetColumnSpan(label2, 3);
            label2.Name = "lblFocusedBackColor";
            manager.ApplyResources(label3, "lblForeColor");
            panel.SetColumnSpan(label3, 3);
            label3.Name = "lblForeColor";
            this.cmbFontFamily.AutoCompleteMode = AutoCompleteMode.Append;
            this.cmbFontFamily.AutoCompleteSource = AutoCompleteSource.ListItems;
            panel.SetColumnSpan(this.cmbFontFamily, 3);
            this.cmbFontFamily.DisplayMember = "Name";
            manager.ApplyResources(this.cmbFontFamily, "cmbFontFamily");
            this.cmbFontFamily.Name = "cmbFontFamily";
            manager.ApplyResources(this.cmbFontSize, "cmbFontSize");
            this.cmbFontSize.Items.AddRange(new object[] { 
                manager.GetString("cmbFontSize.Items"), manager.GetString("cmbFontSize.Items1"), manager.GetString("cmbFontSize.Items2"), manager.GetString("cmbFontSize.Items3"), manager.GetString("cmbFontSize.Items4"), manager.GetString("cmbFontSize.Items5"), manager.GetString("cmbFontSize.Items6"), manager.GetString("cmbFontSize.Items7"), manager.GetString("cmbFontSize.Items8"), manager.GetString("cmbFontSize.Items9"), manager.GetString("cmbFontSize.Items10"), manager.GetString("cmbFontSize.Items11"), manager.GetString("cmbFontSize.Items12"), manager.GetString("cmbFontSize.Items13"), manager.GetString("cmbFontSize.Items14"), manager.GetString("cmbFontSize.Items15"), 
                manager.GetString("cmbFontSize.Items16"), manager.GetString("cmbFontSize.Items17"), manager.GetString("cmbFontSize.Items18")
             });
            this.cmbFontSize.Name = "cmbFontSize";
            this.cmbFontSize.Leave += new EventHandler(this.cmbFontSize_Leave);
            this.cmbFontSize.Enter += new EventHandler(this.cmbFontSize_Enter);
            manager.ApplyResources(label4, "lblOddLineBackColor");
            panel.SetColumnSpan(label4, 3);
            label4.Name = "lblOddLineBackColor";
            manager.ApplyResources(this.chkFontBold, "chkFontBold");
            panel.SetColumnSpan(this.chkFontBold, 2);
            this.chkFontBold.Name = "chkFontBold";
            this.chkFontBold.UseVisualStyleBackColor = true;
            this.btnActiveBackColor.Image = null;
            manager.ApplyResources(this.btnActiveBackColor, "btnActiveBackColor");
            this.btnActiveBackColor.Name = "btnActiveBackColor";
            this.btnActiveBackColor.UseVisualStyleBackColor = true;
            this.btnActiveBackColor.ColorChanged += new EventHandler(this.ColorButton_ColorChanged);
            manager.ApplyResources(label5, "lblActiveBackColor");
            panel.SetColumnSpan(label5, 3);
            label5.Name = "lblActiveBackColor";
            this.btnFocusedForeColor.Image = null;
            manager.ApplyResources(this.btnFocusedForeColor, "btnFocusedForeColor");
            this.btnFocusedForeColor.Name = "btnFocusedForeColor";
            this.btnFocusedForeColor.UseVisualStyleBackColor = true;
            this.btnFocusedForeColor.ColorChanged += new EventHandler(this.ColorButton_ColorChanged);
            this.btnFocusedForeColor.EnabledChanged += new EventHandler(this.ColorButton_EnabledChanged);
            manager.ApplyResources(label6, "lblFocusedForeColor");
            panel.SetColumnSpan(label6, 3);
            label6.Name = "lblFocusedForeColor";
            this.btnSelectedForeColor.Image = null;
            manager.ApplyResources(this.btnSelectedForeColor, "btnSelectedForeColor");
            this.btnSelectedForeColor.Name = "btnSelectedForeColor";
            this.btnSelectedForeColor.UseVisualStyleBackColor = true;
            this.btnSelectedForeColor.ColorChanged += new EventHandler(this.ColorButton_ColorChanged);
            manager.ApplyResources(label7, "lblSelectedForeColor");
            panel.SetColumnSpan(label7, 3);
            label7.Name = "lblSelectedForeColor";
            this.boxBackColor.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.boxBackColor, "boxBackColor");
            this.boxBackColor.Name = "boxBackColor";
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(panel);
            base.Name = "ListViewColorsOptionControl";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            IDictionary<KnownListViewColor, Color> listColorMap = VirtualFilePanelSettings.Default.ListColorMap;
            if (listColorMap != null)
            {
                foreach (ColorButton button in this.ColorButtons)
                {
                    Color color;
                    if (listColorMap.TryGetValue((KnownListViewColor) button.Tag, out color))
                    {
                        button.Color = color;
                    }
                    else
                    {
                        button.Color = Color.Empty;
                    }
                }
            }
            this.UpdateColorBoxes();
            this.btnFocusedBackColor.Enabled = (!Settings.Default.ExplorerTheme || !OS.IsWinVista) || !Application.RenderWithVisualStyles;
            this.btnFocusedForeColor.Enabled = this.btnFocusedBackColor.Enabled;
            this.chkChangeFont.Checked = VirtualFilePanelSettings.Default.ListFontEnabled;
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            SerializableDictionary<KnownListViewColor, Color> dictionary = new SerializableDictionary<KnownListViewColor, Color>();
            foreach (ColorButton button in this.ColorButtons)
            {
                if (!button.Color.IsEmpty)
                {
                    dictionary.Add((KnownListViewColor) button.Tag, button.Color);
                }
            }
            if (dictionary.Count == 0)
            {
                dictionary = null;
            }
            IDictionary<KnownListViewColor, Color> listColorMap = VirtualFilePanelSettings.Default.ListColorMap;
            bool flag = ((listColorMap == null) ^ (dictionary == null)) || (((listColorMap != null) && (dictionary != null)) && (listColorMap.Count != dictionary.Count));
            if (!flag && (dictionary != null))
            {
                foreach (KeyValuePair<KnownListViewColor, Color> pair in dictionary)
                {
                    Color color;
                    flag |= listColorMap.TryGetValue(pair.Key, out color) && (pair.Value != color);
                }
            }
            if (flag)
            {
                VirtualFilePanelSettings.Default.ListColorMap = dictionary;
            }
            if (this.chkChangeFont.Checked)
            {
                float num;
                FontFamily selectedItem = (FontFamily) this.cmbFontFamily.SelectedItem;
                FontStyle style = this.chkFontBold.Checked ? FontStyle.Bold : FontStyle.Regular;
                if (((selectedItem != null) && float.TryParse(this.cmbFontSize.Text, out num)) && (num > 0f))
                {
                    Font listFont = VirtualFilePanelSettings.Default.ListFont;
                    if ((((listFont == null) || !selectedItem.Equals(listFont.FontFamily)) || (listFont.Size != num)) || (listFont.Style != style))
                    {
                        try
                        {
                            VirtualFilePanelSettings.Default.ListFont = new Font(selectedItem, num, style);
                        }
                        catch (ArgumentException)
                        {
                        }
                    }
                }
            }
            else
            {
                VirtualFilePanelSettings.Default.ListFont = null;
            }
        }

        private void UpdateColorBoxes()
        {
            foreach (Label label in this.ColorBoxes)
            {
                ColorButton tag = (ColorButton) label.Tag;
                if (tag.Color.IsEmpty)
                {
                    KnownListViewColor knownColor = (KnownListViewColor) tag.Tag;
                    Color color = Theme.Current.ListViewColors.FromKnownColor(knownColor);
                    if (color.IsEmpty)
                    {
                        switch (knownColor)
                        {
                            case KnownListViewColor.ActiveBack:
                            case KnownListViewColor.OddLineBack:
                                color = this.btnBackColor.Color;
                                if (color.IsEmpty)
                                {
                                    color = Theme.Current.ListViewColors.FromKnownColor(KnownListViewColor.Back);
                                }
                                break;

                            case KnownListViewColor.FocusedText:
                                color = this.btnForeColor.Color;
                                if (color.IsEmpty)
                                {
                                    color = Theme.Current.ListViewColors.FromKnownColor(KnownListViewColor.Text);
                                }
                                break;
                        }
                    }
                    label.BackColor = color;
                }
                else
                {
                    label.BackColor = tag.Color;
                }
            }
        }

        public bool SaveSettings
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public string SettingsKey
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }
    }
}

