namespace Nomad.Controls.Option
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Resources;
    using Nomad.Controls;
    using Nomad.FileSystem.Properties;
    using Nomad.FileSystem.Property;
    using Nomad.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class FormatsOptionControl : UserControl, IPersistComponentSettings, IUpdateCulture
    {
        private Button btnDateTimeFormat;
        private ComboBoxEx cmbFileNameCasing;
        private ComboBoxEx cmbFolderNameCasing;
        private ComboBoxEx cmbFolderNameTemplate;
        private ComboBoxEx cmbSizeFormat;
        private ContextMenuStrip cmsDateTimeFormat;
        private IContainer components = null;
        private Label lblDateTimeFormat;
        private Label lblDateTimeSample;
        private Label lblFileNameCasing;
        private Label lblFileNameSample;
        private Label lblFolderNameCasing;
        private Label lblFolderNameSample;
        private Label lblFolderNameTemplate;
        private Label lblSizeFormat;
        private Label lblSizeSample;
        private string SampleFileName;
        private string SampleFolderName;
        private TextBox txtDateTimeFormat;
        private PropertyValuesWatcher ValuesWatcher;

        public FormatsOptionControl()
        {
            this.InitializeComponent();
            this.UpdateCulture();
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>((string) this.cmbFolderNameTemplate.Items[0], "{0}")
            };
            Hashtable section = ConfigurationManager.GetSection("folderNameTemplates") as Hashtable;
            if (section != null)
            {
                foreach (DictionaryEntry entry in section)
                {
                    string str = (string) entry.Value;
                    list.Add(new KeyValuePair<string, string>(string.Format(str, Resources.sFolder), str));
                }
            }
            this.cmbFolderNameTemplate.DataSource = list;
            this.cmbSizeFormat.DataSource = Enum.GetValues(typeof(SizeFormat));
            this.cmbFileNameCasing.DataSource = Enum.GetValues(typeof(CharacterCasing));
            this.cmbFolderNameCasing.DataSource = Enum.GetValues(typeof(CharacterCasing));
            if (!Application.RenderWithVisualStyles)
            {
                this.btnDateTimeFormat.BackColor = SystemColors.Control;
            }
        }

        private void btnDateTimeFormat_Click(object sender, EventArgs e)
        {
            this.cmsDateTimeFormat.Show(this.btnDateTimeFormat, 0, this.btnDateTimeFormat.Height + 1);
        }

        private void cmbFileNameCasing_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblFileNameSample.Text = StringHelper.ApplyCharacterCasing(this.SampleFileName, (CharacterCasing) this.cmbFileNameCasing.SelectedItem);
        }

        private void cmbFolderNameCasing_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.cmbFolderNameCasing.SelectedItem != null) && (this.cmbFolderNameTemplate.SelectedValue != null))
            {
                this.lblFolderNameSample.Text = string.Format((string) this.cmbFolderNameTemplate.SelectedValue, StringHelper.ApplyCharacterCasing(this.SampleFolderName, (CharacterCasing) this.cmbFolderNameCasing.SelectedItem));
            }
        }

        private void cmbSizeFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblSizeSample.Text = SizeTypeConverter.FormatSize<int>(0xbc614e, (SizeFormat) this.cmbSizeFormat.SelectedItem);
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
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FormatsOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            PropertyValue value5 = new PropertyValue();
            PropertyValue value6 = new PropertyValue();
            this.lblDateTimeFormat = new Label();
            this.lblSizeFormat = new Label();
            this.lblFolderNameSample = new Label();
            this.lblDateTimeSample = new Label();
            this.lblFileNameSample = new Label();
            this.btnDateTimeFormat = new Button();
            this.lblSizeSample = new Label();
            this.lblFileNameCasing = new Label();
            this.lblFolderNameCasing = new Label();
            this.txtDateTimeFormat = new TextBox();
            this.cmbFolderNameCasing = new ComboBoxEx();
            this.cmbSizeFormat = new ComboBoxEx();
            this.cmbFileNameCasing = new ComboBoxEx();
            this.lblFolderNameTemplate = new Label();
            this.cmbFolderNameTemplate = new ComboBoxEx();
            this.cmsDateTimeFormat = new ContextMenuStrip(this.components);
            this.ValuesWatcher = new PropertyValuesWatcher();
            ToolStripMenuItem item = new ToolStripMenuItem();
            ToolStripMenuItem item2 = new ToolStripMenuItem();
            ToolStripSeparator separator = new ToolStripSeparator();
            ToolStripMenuItem item3 = new ToolStripMenuItem();
            ToolStripMenuItem item4 = new ToolStripMenuItem();
            ToolStripSeparator separator2 = new ToolStripSeparator();
            ToolStripMenuItem item5 = new ToolStripMenuItem();
            ToolStripMenuItem item6 = new ToolStripMenuItem();
            ToolStripSeparator separator3 = new ToolStripSeparator();
            ToolStripMenuItem item7 = new ToolStripMenuItem();
            ToolStripMenuItem item8 = new ToolStripMenuItem();
            ToolStripSeparator separator4 = new ToolStripSeparator();
            ToolStripMenuItem item9 = new ToolStripMenuItem();
            ToolStripMenuItem item10 = new ToolStripMenuItem();
            ToolStripMenuItem item11 = new ToolStripMenuItem();
            ToolStripMenuItem item12 = new ToolStripMenuItem();
            ToolStripSeparator separator5 = new ToolStripSeparator();
            ToolStripMenuItem item13 = new ToolStripMenuItem();
            ToolStripMenuItem item14 = new ToolStripMenuItem();
            ToolStripMenuItem item15 = new ToolStripMenuItem();
            ToolStripMenuItem item16 = new ToolStripMenuItem();
            ToolStripSeparator separator6 = new ToolStripSeparator();
            ToolStripMenuItem item17 = new ToolStripMenuItem();
            ToolStripMenuItem item18 = new ToolStripMenuItem();
            ToolStripMenuItem item19 = new ToolStripMenuItem();
            ToolStripMenuItem item20 = new ToolStripMenuItem();
            ToolStripMenuItem item21 = new ToolStripMenuItem();
            ToolStripSeparator separator7 = new ToolStripSeparator();
            ToolStripMenuItem item22 = new ToolStripMenuItem();
            ToolStripMenuItem item23 = new ToolStripMenuItem();
            ToolStripMenuItem item24 = new ToolStripMenuItem();
            ToolStripMenuItem item25 = new ToolStripMenuItem();
            ToolStripSeparator separator8 = new ToolStripSeparator();
            ToolStripMenuItem item26 = new ToolStripMenuItem();
            ToolStripMenuItem item27 = new ToolStripMenuItem();
            ToolStripSeparator separator9 = new ToolStripSeparator();
            ToolStripMenuItem item28 = new ToolStripMenuItem();
            ToolStripMenuItem item29 = new ToolStripMenuItem();
            ToolStripSeparator separator10 = new ToolStripSeparator();
            ToolStripMenuItem item30 = new ToolStripMenuItem();
            ToolStripMenuItem item31 = new ToolStripMenuItem();
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.SuspendLayout();
            this.cmsDateTimeFormat.SuspendLayout();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            item.Name = "tsmiPatternGeneralDateShortTime";
            manager.ApplyResources(item, "tsmiPatternGeneralDateShortTime");
            item.Tag = "g";
            item.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item.Click += new EventHandler(this.tsmiStandardDatePattern_Click);
            item2.Name = "tsmiPatternGeneralDateLongTime";
            manager.ApplyResources(item2, "tsmiPatternGeneralDateLongTime");
            item2.Tag = "G";
            item2.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item2.Click += new EventHandler(this.tsmiStandardDatePattern_Click);
            separator.Name = "tssStandardPattern1";
            manager.ApplyResources(separator, "tssStandardPattern1");
            separator.Click += new EventHandler(this.tsmiStandardDatePattern_Click);
            item3.Name = "tsmiPatternFullDateShortTime";
            manager.ApplyResources(item3, "tsmiPatternFullDateShortTime");
            item3.Tag = "f";
            item3.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item3.Click += new EventHandler(this.tsmiStandardDatePattern_Click);
            item4.Name = "tsmiPatternFullDateLongTime";
            manager.ApplyResources(item4, "tsmiPatternFullDateLongTime");
            item4.Tag = "F";
            item4.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item4.Click += new EventHandler(this.tsmiStandardDatePattern_Click);
            separator2.Name = "tssStandardPattern2";
            manager.ApplyResources(separator2, "tssStandardPattern2");
            separator2.Click += new EventHandler(this.tsmiStandardDatePattern_Click);
            item5.Name = "tsmiPatternShortDate";
            manager.ApplyResources(item5, "tsmiPatternShortDate");
            item5.Tag = "d";
            item5.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item5.Click += new EventHandler(this.tsmiStandardDatePattern_Click);
            item6.Name = "tsmiPatternLongDate";
            manager.ApplyResources(item6, "tsmiPatternLongDate");
            item6.Tag = "D";
            item6.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item6.Click += new EventHandler(this.tsmiStandardDatePattern_Click);
            separator3.Name = "tssStandardPattern3";
            manager.ApplyResources(separator3, "tssStandardPattern3");
            item7.DropDownItems.AddRange(new ToolStripItem[] { item8, separator4, item9, item10, item11, item12, separator5, item13, item14, item15, item16, separator6, item17, item18, item19 });
            item7.Name = "tsmiCustomDateFormat";
            manager.ApplyResources(item7, "tsmiCustomDateFormat");
            item8.Name = "tsmiPatternDateSeparator";
            manager.ApplyResources(item8, "tsmiPatternDateSeparator");
            item8.Tag = "/";
            item8.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item8.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            separator4.Name = "tssCustomDatePattern1";
            manager.ApplyResources(separator4, "tssCustomDatePattern1");
            item9.Name = "tsmiPatternDay1";
            manager.ApplyResources(item9, "tsmiPatternDay1");
            item9.Tag = "d";
            item9.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item9.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item10.Name = "tsmiPatternDay2";
            manager.ApplyResources(item10, "tsmiPatternDay2");
            item10.Tag = "dd";
            item10.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item10.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item11.Name = "tsmiPatternAbbreviatedDayName";
            manager.ApplyResources(item11, "tsmiPatternAbbreviatedDayName");
            item11.Tag = "ddd";
            item11.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item11.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item12.Name = "tsmiPatternFullDayName";
            manager.ApplyResources(item12, "tsmiPatternFullDayName");
            item12.Tag = "dddd";
            item12.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item12.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            separator5.Name = "tssCustomDatePattern2";
            manager.ApplyResources(separator5, "tssCustomDatePattern2");
            item13.Name = "tsmiPatternMonth1";
            manager.ApplyResources(item13, "tsmiPatternMonth1");
            item13.Tag = "M";
            item13.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item13.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item14.Name = "tsmiPatternMonth2";
            manager.ApplyResources(item14, "tsmiPatternMonth2");
            item14.Tag = "MM";
            item14.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item14.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item15.Name = "tsmiPatternAbbreviatedMonthName";
            manager.ApplyResources(item15, "tsmiPatternAbbreviatedMonthName");
            item15.Tag = "MMM";
            item15.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item15.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item16.Name = "tsmiPatternFullMonthName";
            manager.ApplyResources(item16, "tsmiPatternFullMonthName");
            item16.Tag = "MMMM";
            item16.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item16.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            separator6.Name = "tssCustomDatePattern3";
            manager.ApplyResources(separator6, "tssCustomDatePattern3");
            item17.Name = "tsmiPatternYear1";
            manager.ApplyResources(item17, "tsmiPatternYear1");
            item17.Tag = "y";
            item17.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item17.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item18.Name = "tsmiPatternYear2";
            manager.ApplyResources(item18, "tsmiPatternYear2");
            item18.Tag = "yy";
            item18.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item18.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item19.Name = "tsmiPatternYear3";
            manager.ApplyResources(item19, "tsmiPatternYear3");
            item19.Tag = "yyyy";
            item19.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item19.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item20.DropDownItems.AddRange(new ToolStripItem[] { item21, separator7, item22, item23, item24, item25, separator8, item26, item27, separator9, item28, item29, separator10, item30, item31 });
            item20.Name = "tsmiCustomTimeFormat";
            manager.ApplyResources(item20, "tsmiCustomTimeFormat");
            item21.Name = "tsmiPatternTimeSeparator";
            manager.ApplyResources(item21, "tsmiPatternTimeSeparator");
            item21.Tag = ":";
            item21.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item21.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            separator7.Name = "tssCustomTimePattern1";
            manager.ApplyResources(separator7, "tssCustomTimePattern1");
            item22.Name = "tsmiPatternHour1";
            manager.ApplyResources(item22, "tsmiPatternHour1");
            item22.Tag = "h";
            item22.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item22.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item23.Name = "tsmiPatternHour2";
            manager.ApplyResources(item23, "tsmiPatternHour2");
            item23.Tag = "hh";
            item23.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item23.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item24.Name = "tsmiPatternHour3";
            manager.ApplyResources(item24, "tsmiPatternHour3");
            item24.Tag = "H";
            item24.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item24.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item25.Name = "tsmiPatternHour4";
            manager.ApplyResources(item25, "tsmiPatternHour4");
            item25.Tag = "HH";
            item25.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item25.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            separator8.Name = "tssCustomTimePattern2";
            manager.ApplyResources(separator8, "tssCustomTimePattern2");
            item26.Name = "tsmiPatternAMPM";
            manager.ApplyResources(item26, "tsmiPatternAMPM");
            item26.Tag = "tt";
            item26.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item26.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item27.Name = "tsmiPatternShortAMPM";
            manager.ApplyResources(item27, "tsmiPatternShortAMPM");
            item27.Tag = "t";
            item27.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item27.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            separator9.Name = "tssCustomTimePattern3";
            manager.ApplyResources(separator9, "tssCustomTimePattern3");
            item28.Name = "tsmiPatternMinute1";
            manager.ApplyResources(item28, "tsmiPatternMinute1");
            item28.Tag = "m";
            item28.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item28.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item29.Name = "tsmiPatternMinute2";
            manager.ApplyResources(item29, "tsmiPatternMinute2");
            item29.Tag = "mm";
            item29.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item29.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            separator10.Name = "tssCustomTimePattern4";
            manager.ApplyResources(separator10, "tssCustomTimePattern4");
            item30.Name = "tsmiPatternSeconds1";
            manager.ApplyResources(item30, "tsmiPatternSeconds1");
            item30.Tag = "s";
            item30.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item30.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            item31.Name = "tsmiPatternSeconds2";
            manager.ApplyResources(item31, "tsmiPatternSeconds2");
            item31.Tag = "ss";
            item31.Paint += new PaintEventHandler(this.tsmiStandardDatePattern_Paint);
            item31.Click += new EventHandler(this.tsmiCustomDatePattern_Click);
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.lblDateTimeFormat, 0, 0);
            panel.Controls.Add(this.lblSizeFormat, 0, 1);
            panel.Controls.Add(this.lblFolderNameSample, 2, 3);
            panel.Controls.Add(this.lblDateTimeSample, 3, 0);
            panel.Controls.Add(this.lblFileNameSample, 2, 2);
            panel.Controls.Add(this.btnDateTimeFormat, 2, 0);
            panel.Controls.Add(this.lblSizeSample, 2, 1);
            panel.Controls.Add(this.lblFileNameCasing, 0, 2);
            panel.Controls.Add(this.lblFolderNameCasing, 0, 3);
            panel.Controls.Add(this.txtDateTimeFormat, 1, 0);
            panel.Controls.Add(this.cmbFolderNameCasing, 1, 3);
            panel.Controls.Add(this.cmbSizeFormat, 1, 1);
            panel.Controls.Add(this.cmbFileNameCasing, 1, 2);
            panel.Controls.Add(this.lblFolderNameTemplate, 0, 4);
            panel.Controls.Add(this.cmbFolderNameTemplate, 1, 4);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.lblDateTimeFormat, "lblDateTimeFormat");
            this.lblDateTimeFormat.Name = "lblDateTimeFormat";
            manager.ApplyResources(this.lblSizeFormat, "lblSizeFormat");
            this.lblSizeFormat.Name = "lblSizeFormat";
            manager.ApplyResources(this.lblFolderNameSample, "lblFolderNameSample");
            panel.SetColumnSpan(this.lblFolderNameSample, 2);
            this.lblFolderNameSample.Name = "lblFolderNameSample";
            panel.SetRowSpan(this.lblFolderNameSample, 2);
            manager.ApplyResources(this.lblDateTimeSample, "lblDateTimeSample");
            this.lblDateTimeSample.Name = "lblDateTimeSample";
            manager.ApplyResources(this.lblFileNameSample, "lblFileNameSample");
            panel.SetColumnSpan(this.lblFileNameSample, 2);
            this.lblFileNameSample.Name = "lblFileNameSample";
            this.btnDateTimeFormat.Image = Resources.SmallDownArrow;
            manager.ApplyResources(this.btnDateTimeFormat, "btnDateTimeFormat");
            this.btnDateTimeFormat.Name = "btnDateTimeFormat";
            this.btnDateTimeFormat.UseVisualStyleBackColor = true;
            this.btnDateTimeFormat.Click += new EventHandler(this.btnDateTimeFormat_Click);
            manager.ApplyResources(this.lblSizeSample, "lblSizeSample");
            panel.SetColumnSpan(this.lblSizeSample, 2);
            this.lblSizeSample.Name = "lblSizeSample";
            manager.ApplyResources(this.lblFileNameCasing, "lblFileNameCasing");
            this.lblFileNameCasing.Name = "lblFileNameCasing";
            manager.ApplyResources(this.lblFolderNameCasing, "lblFolderNameCasing");
            this.lblFolderNameCasing.Name = "lblFolderNameCasing";
            manager.ApplyResources(this.txtDateTimeFormat, "txtDateTimeFormat");
            this.txtDateTimeFormat.Name = "txtDateTimeFormat";
            this.txtDateTimeFormat.TextChanged += new EventHandler(this.txtDateTimeFormat_TextChanged);
            manager.ApplyResources(this.cmbFolderNameCasing, "cmbFolderNameCasing");
            this.cmbFolderNameCasing.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFolderNameCasing.FormattingEnabled = true;
            this.cmbFolderNameCasing.Name = "cmbFolderNameCasing";
            this.cmbFolderNameCasing.SelectedIndexChanged += new EventHandler(this.cmbFolderNameCasing_SelectedIndexChanged);
            manager.ApplyResources(this.cmbSizeFormat, "cmbSizeFormat");
            this.cmbSizeFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSizeFormat.FormattingEnabled = true;
            this.cmbSizeFormat.Name = "cmbSizeFormat";
            this.cmbSizeFormat.SelectedIndexChanged += new EventHandler(this.cmbSizeFormat_SelectedIndexChanged);
            manager.ApplyResources(this.cmbFileNameCasing, "cmbFileNameCasing");
            this.cmbFileNameCasing.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFileNameCasing.FormattingEnabled = true;
            this.cmbFileNameCasing.Name = "cmbFileNameCasing";
            this.cmbFileNameCasing.SelectedIndexChanged += new EventHandler(this.cmbFileNameCasing_SelectedIndexChanged);
            manager.ApplyResources(this.lblFolderNameTemplate, "lblFolderNameTemplate");
            this.lblFolderNameTemplate.Name = "lblFolderNameTemplate";
            manager.ApplyResources(this.cmbFolderNameTemplate, "cmbFolderNameTemplate");
            this.cmbFolderNameTemplate.DisplayMember = "Key";
            this.cmbFolderNameTemplate.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFolderNameTemplate.FormattingEnabled = true;
            this.cmbFolderNameTemplate.Items.AddRange(new object[] { manager.GetString("cmbFolderNameTemplate.Items") });
            this.cmbFolderNameTemplate.Name = "cmbFolderNameTemplate";
            this.cmbFolderNameTemplate.ValueMember = "Value";
            this.cmbFolderNameTemplate.SelectedIndexChanged += new EventHandler(this.cmbFolderNameCasing_SelectedIndexChanged);
            this.cmsDateTimeFormat.Items.AddRange(new ToolStripItem[] { item, item2, separator, item3, item4, separator2, item5, item6, separator3, item7, item20 });
            this.cmsDateTimeFormat.Name = "cmsDateTimeFormat";
            manager.ApplyResources(this.cmsDateTimeFormat, "cmsDateTimeFormat");
            value2.DataObject = this.txtDateTimeFormat;
            value2.PropertyName = "Text";
            value3.DataObject = this.cmbSizeFormat;
            value3.PropertyName = "SelectedItem";
            value4.DataObject = this.cmbFileNameCasing;
            value4.PropertyName = "SelectedItem";
            value5.DataObject = this.cmbFolderNameCasing;
            value5.PropertyName = "SelectedItem";
            value6.DataObject = this.cmbFolderNameTemplate;
            value6.PropertyName = "SelectedIndex";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4, value5, value6 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(panel);
            base.Name = "FormatsOptionControl";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.cmsDateTimeFormat.ResumeLayout(false);
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.txtDateTimeFormat.Text = Nomad.FileSystem.Properties.Settings.Default.DateTimeFormat;
            this.txtDateTimeFormat_TextChanged(this.txtDateTimeFormat, EventArgs.Empty);
            this.cmbSizeFormat.SelectedItem = Nomad.FileSystem.Properties.Settings.Default.SizeFormat;
            this.cmbFileNameCasing.SelectedItem = VirtualFilePanelSettings.Default.FileNameCasing;
            this.cmbFolderNameCasing.SelectedItem = VirtualFilePanelSettings.Default.FolderNameCasing;
            this.cmbFolderNameTemplate.SelectedValue = VirtualFilePanelSettings.Default.FolderNameTemplate;
            if (this.cmbFolderNameTemplate.SelectedIndex < 0)
            {
                this.cmbFolderNameTemplate.SelectedIndex = 0;
            }
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            try
            {
                DateTime.Now.ToString(this.txtDateTimeFormat.Text);
                Nomad.FileSystem.Properties.Settings.Default.DateTimeFormat = this.txtDateTimeFormat.Text;
            }
            catch (FormatException)
            {
            }
            Nomad.FileSystem.Properties.Settings.Default.SizeFormat = (SizeFormat) this.cmbSizeFormat.SelectedItem;
            VirtualFilePanelSettings.Default.FileNameCasing = (CharacterCasing) this.cmbFileNameCasing.SelectedItem;
            VirtualFilePanelSettings.Default.FolderNameCasing = (CharacterCasing) this.cmbFolderNameCasing.SelectedItem;
            VirtualFilePanelSettings.Default.FolderNameTemplate = (string) this.cmbFolderNameTemplate.SelectedValue;
        }

        private void tsmiCustomDatePattern_Click(object sender, EventArgs e)
        {
            this.txtDateTimeFormat.SelectedText = (string) ((ToolStripItem) sender).Tag;
            this.txtDateTimeFormat.Focus();
        }

        private void tsmiStandardDatePattern_Click(object sender, EventArgs e)
        {
            this.txtDateTimeFormat.Text = (string) ((ToolStripItem) sender).Tag;
            this.txtDateTimeFormat.SelectAll();
            this.txtDateTimeFormat.Focus();
        }

        private void tsmiStandardDatePattern_Paint(object sender, PaintEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            if (string.IsNullOrEmpty(item.ShortcutKeyDisplayString))
            {
                item.ShortcutKeyDisplayString = (string) item.Tag;
            }
        }

        private void txtDateTimeFormat_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblDateTimeSample.Text = DateTime.Now.ToString(this.txtDateTimeFormat.Text);
                this.txtDateTimeFormat.ResetBackColor();
                this.txtDateTimeFormat.ResetForeColor();
            }
            catch (FormatException)
            {
                this.lblDateTimeSample.Text = Resources.sError;
                this.txtDateTimeFormat.BackColor = Nomad.Properties.Settings.TextBoxError;
                this.txtDateTimeFormat.ForeColor = SystemColors.HighlightText;
            }
        }

        public void UpdateCulture()
        {
            this.SampleFileName = this.lblFileNameSample.Text;
            this.SampleFolderName = this.lblFolderNameSample.Text;
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

