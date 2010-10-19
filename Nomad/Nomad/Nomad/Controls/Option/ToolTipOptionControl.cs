namespace Nomad.Controls.Option
{
    using Nomad.Commons.Controls;
    using Nomad.Controls;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class ToolTipOptionControl : UserControl, IPersistComponentSettings
    {
        private CheckBox chkShowItemTooltips;
        private CheckBox chkShowItemTooltipsKbd;
        private ComboBoxEx cmbToolTipImage;
        private ComboBoxEx cmbTooltipOpacityDelay;
        private IContainer components = null;
        private Label lblToolTipImage;
        private Label lblTooltipOpacityDelay;
        private TableLayoutPanel tlpBack;
        private PropertyValuesWatcher ValuesWatcher;

        public ToolTipOptionControl()
        {
            this.InitializeComponent();
            this.cmbTooltipOpacityDelay.Enabled = OSFeature.Feature.IsPresent(OSFeature.LayeredWindows);
            this.lblTooltipOpacityDelay.Enabled = this.cmbTooltipOpacityDelay.Enabled;
            this.cmbToolTipImage.SelectedIndex = 0;
        }

        private void chkShowItemTooltips_CheckedChanged(object sender, EventArgs e)
        {
            this.chkShowItemTooltipsKbd.Enabled = this.chkShowItemTooltips.Checked;
            this.cmbToolTipImage.Enabled = this.chkShowItemTooltips.Checked;
            this.lblToolTipImage.Enabled = this.cmbToolTipImage.Enabled;
            this.cmbTooltipOpacityDelay.Enabled = this.chkShowItemTooltips.Checked;
            this.lblTooltipOpacityDelay.Enabled = this.cmbTooltipOpacityDelay.Enabled;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ToolTipOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            PropertyValue value5 = new PropertyValue();
            this.chkShowItemTooltips = new CheckBox();
            this.chkShowItemTooltipsKbd = new CheckBox();
            this.cmbToolTipImage = new ComboBoxEx();
            this.cmbTooltipOpacityDelay = new ComboBoxEx();
            this.tlpBack = new TableLayoutPanel();
            this.lblToolTipImage = new Label();
            this.lblTooltipOpacityDelay = new Label();
            this.ValuesWatcher = new PropertyValuesWatcher();
            this.tlpBack.SuspendLayout();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(this.chkShowItemTooltips, "chkShowItemTooltips");
            this.tlpBack.SetColumnSpan(this.chkShowItemTooltips, 2);
            this.chkShowItemTooltips.Name = "chkShowItemTooltips";
            this.chkShowItemTooltips.UseVisualStyleBackColor = true;
            this.chkShowItemTooltips.CheckedChanged += new EventHandler(this.chkShowItemTooltips_CheckedChanged);
            manager.ApplyResources(this.chkShowItemTooltipsKbd, "chkShowItemTooltipsKbd");
            this.tlpBack.SetColumnSpan(this.chkShowItemTooltipsKbd, 2);
            this.chkShowItemTooltipsKbd.Name = "chkShowItemTooltipsKbd";
            this.chkShowItemTooltipsKbd.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.cmbToolTipImage, "cmbToolTipImage");
            this.cmbToolTipImage.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbToolTipImage.FormattingEnabled = true;
            this.cmbToolTipImage.Items.AddRange(new object[] { manager.GetString("cmbToolTipImage.Items"), manager.GetString("cmbToolTipImage.Items1"), manager.GetString("cmbToolTipImage.Items2") });
            this.cmbToolTipImage.MinimumSize = new Size(110, 0);
            this.cmbToolTipImage.Name = "cmbToolTipImage";
            manager.ApplyResources(this.cmbTooltipOpacityDelay, "cmbTooltipOpacityDelay");
            this.cmbTooltipOpacityDelay.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTooltipOpacityDelay.FormattingEnabled = true;
            this.cmbTooltipOpacityDelay.Items.AddRange(new object[] { manager.GetString("cmbTooltipOpacityDelay.Items"), manager.GetString("cmbTooltipOpacityDelay.Items1"), manager.GetString("cmbTooltipOpacityDelay.Items2"), manager.GetString("cmbTooltipOpacityDelay.Items3"), manager.GetString("cmbTooltipOpacityDelay.Items4") });
            this.cmbTooltipOpacityDelay.MinimumSize = new Size(110, 0);
            this.cmbTooltipOpacityDelay.Name = "cmbTooltipOpacityDelay";
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.chkShowItemTooltips, 0, 0);
            this.tlpBack.Controls.Add(this.chkShowItemTooltipsKbd, 0, 1);
            this.tlpBack.Controls.Add(this.lblToolTipImage, 0, 2);
            this.tlpBack.Controls.Add(this.cmbToolTipImage, 1, 2);
            this.tlpBack.Controls.Add(this.lblTooltipOpacityDelay, 0, 3);
            this.tlpBack.Controls.Add(this.cmbTooltipOpacityDelay, 1, 3);
            this.tlpBack.Name = "tlpBack";
            manager.ApplyResources(this.lblToolTipImage, "lblToolTipImage");
            this.lblToolTipImage.Name = "lblToolTipImage";
            manager.ApplyResources(this.lblTooltipOpacityDelay, "lblTooltipOpacityDelay");
            this.lblTooltipOpacityDelay.Name = "lblTooltipOpacityDelay";
            value2.DataObject = this.chkShowItemTooltips;
            value2.PropertyName = "Checked";
            value3.DataObject = this.chkShowItemTooltipsKbd;
            value3.PropertyName = "Checked";
            value4.DataObject = this.cmbToolTipImage;
            value4.PropertyName = "SelectedIndex";
            value5.DataObject = this.cmbTooltipOpacityDelay;
            value5.PropertyName = "SelectedIndex";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4, value5 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tlpBack);
            base.Name = "ToolTipOptionControl";
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.chkShowItemTooltips.Checked = Settings.Default.ShowItemToolTips;
            this.chkShowItemTooltipsKbd.Checked = Settings.Default.ShowItemTooltipsKbd;
            if (Settings.Default.ShowThumbnailInTooltip)
            {
                this.cmbToolTipImage.SelectedIndex = 2;
            }
            else if (Settings.Default.ShowIconInTooltip)
            {
                this.cmbToolTipImage.SelectedIndex = 1;
            }
            else
            {
                this.cmbToolTipImage.SelectedIndex = 0;
            }
            int tooltipOpacityDelay = Settings.Default.TooltipOpacityDelay;
            if (tooltipOpacityDelay < 0)
            {
                this.cmbTooltipOpacityDelay.SelectedIndex = 0;
            }
            else if (tooltipOpacityDelay == 0)
            {
                this.cmbTooltipOpacityDelay.SelectedIndex = 4;
            }
            else if (tooltipOpacityDelay <= 0x3e8)
            {
                this.cmbTooltipOpacityDelay.SelectedIndex = 1;
            }
            else if (tooltipOpacityDelay <= 0x7d0)
            {
                this.cmbTooltipOpacityDelay.SelectedIndex = 2;
            }
            else
            {
                this.cmbTooltipOpacityDelay.SelectedIndex = 3;
            }
            this.chkShowItemTooltips_CheckedChanged(this.chkShowItemTooltips, EventArgs.Empty);
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            Settings.Default.ShowItemToolTips = this.chkShowItemTooltips.Checked;
            Settings.Default.ShowItemTooltipsKbd = this.chkShowItemTooltipsKbd.Checked;
            Settings.Default.ShowIconInTooltip = this.cmbToolTipImage.SelectedIndex > 0;
            Settings.Default.ShowThumbnailInTooltip = this.cmbToolTipImage.SelectedIndex > 1;
            switch (this.cmbTooltipOpacityDelay.SelectedIndex)
            {
                case 1:
                case 2:
                case 3:
                    Settings.Default.TooltipOpacityDelay = this.cmbTooltipOpacityDelay.SelectedIndex * 0x3e8;
                    break;

                case 4:
                    Settings.Default.TooltipOpacityDelay = 0;
                    break;

                default:
                    Settings.Default.TooltipOpacityDelay = -1;
                    break;
            }
        }

        public bool SaveSettings
        {
            get
            {
                return this.ValuesWatcher.AnyValueChanged;
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

