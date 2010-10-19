namespace Nomad.Controls.Option
{
    using Nomad.Configuration;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class DialogFontOptionControl : UserControl, IPersistComponentSettings
    {
        private CheckBox chkChangeFont;
        private ComboBox cmbFontFamily;
        private ComboBox cmbFontSize;
        private IContainer components = null;
        private Label lblFontFamily;
        private Label lblFontSize;

        public DialogFontOptionControl()
        {
            this.InitializeComponent();
            this.cmbFontFamily.DataSource = FontFamily.Families;
            this.cmbFontFamily.DataBindings.Add(new Binding("SelectedItem", FormSettings.Default.DialogFont, "FontFamily", false, DataSourceUpdateMode.Never));
            this.cmbFontSize.DataBindings.Add(new Binding("Text", FormSettings.Default.DialogFont, "Size", true, DataSourceUpdateMode.Never));
        }

        private void chkChangeFont_CheckedChanged(object sender, EventArgs e)
        {
            this.lblFontFamily.Enabled = this.chkChangeFont.Checked;
            this.cmbFontFamily.Enabled = this.chkChangeFont.Checked;
            this.lblFontSize.Enabled = this.chkChangeFont.Checked;
            this.cmbFontSize.Enabled = this.chkChangeFont.Checked;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DialogFontOptionControl));
            this.chkChangeFont = new CheckBox();
            this.cmbFontSize = new ComboBox();
            this.lblFontSize = new Label();
            this.lblFontFamily = new Label();
            this.cmbFontFamily = new ComboBox();
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.chkChangeFont, 0, 0);
            panel.Controls.Add(this.cmbFontSize, 2, 2);
            panel.Controls.Add(this.lblFontSize, 1, 2);
            panel.Controls.Add(this.lblFontFamily, 1, 1);
            panel.Controls.Add(this.cmbFontFamily, 2, 1);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.chkChangeFont, "chkChangeFont");
            panel.SetColumnSpan(this.chkChangeFont, 3);
            this.chkChangeFont.Name = "chkChangeFont";
            this.chkChangeFont.UseVisualStyleBackColor = true;
            this.chkChangeFont.CheckedChanged += new EventHandler(this.chkChangeFont_CheckedChanged);
            manager.ApplyResources(this.cmbFontSize, "cmbFontSize");
            this.cmbFontSize.FormattingEnabled = true;
            this.cmbFontSize.Items.AddRange(new object[] { manager.GetString("cmbFontSize.Items"), manager.GetString("cmbFontSize.Items1"), manager.GetString("cmbFontSize.Items2"), manager.GetString("cmbFontSize.Items3"), manager.GetString("cmbFontSize.Items4"), manager.GetString("cmbFontSize.Items5"), manager.GetString("cmbFontSize.Items6"), manager.GetString("cmbFontSize.Items7"), manager.GetString("cmbFontSize.Items8"), manager.GetString("cmbFontSize.Items9"), manager.GetString("cmbFontSize.Items10") });
            this.cmbFontSize.Name = "cmbFontSize";
            this.cmbFontSize.Leave += new EventHandler(this.cmbFontSize_Leave);
            this.cmbFontSize.Enter += new EventHandler(this.cmbFontSize_Enter);
            manager.ApplyResources(this.lblFontSize, "lblFontSize");
            this.lblFontSize.Name = "lblFontSize";
            manager.ApplyResources(this.lblFontFamily, "lblFontFamily");
            this.lblFontFamily.Name = "lblFontFamily";
            this.cmbFontFamily.AutoCompleteMode = AutoCompleteMode.Append;
            this.cmbFontFamily.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.cmbFontFamily.DisplayMember = "Name";
            manager.ApplyResources(this.cmbFontFamily, "cmbFontFamily");
            this.cmbFontFamily.Name = "cmbFontFamily";
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(panel);
            base.Name = "DialogFontOptionControl";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.chkChangeFont.Checked = FormSettings.Default.DialogFontEnabled;
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            if (this.chkChangeFont.Checked)
            {
                float num;
                FontFamily selectedItem = (FontFamily) this.cmbFontFamily.SelectedItem;
                if (((selectedItem != null) && float.TryParse(this.cmbFontSize.Text, out num)) && (num > 0f))
                {
                    Font dialogFont = FormSettings.Default.DialogFont;
                    if (((dialogFont == null) || !selectedItem.Equals(dialogFont.FontFamily)) || (dialogFont.Size != num))
                    {
                        try
                        {
                            FormSettings.Default.DialogFont = new Font(selectedItem, num);
                        }
                        catch (ArgumentException)
                        {
                        }
                    }
                }
            }
            else
            {
                FormSettings.Default.DialogFont = null;
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

