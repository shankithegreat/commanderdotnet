namespace Nomad.Controls
{
    using Nomad.FileSystem.Virtual;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DuplicateSearchControl : UserControl
    {
        private CheckBox chkSameContent;
        private CheckBox chkSameName;
        private CheckBox chkSameSize;
        private IContainer components = null;
        private Label lblInfo;
        private Panel pnlOptions;

        public DuplicateSearchControl()
        {
            this.InitializeComponent();
        }

        private void chkSameContent_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSameContent.Checked)
            {
                this.chkSameSize.Checked = true;
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

        private void DuplicateSearchControl_ClientSizeChanged(object sender, EventArgs e)
        {
            this.lblInfo.MaximumSize = new Size(base.ClientSize.Width, 0);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DuplicateSearchControl));
            this.chkSameName = new CheckBox();
            this.chkSameSize = new CheckBox();
            this.chkSameContent = new CheckBox();
            this.lblInfo = new Label();
            this.pnlOptions = new Panel();
            this.pnlOptions.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(this.chkSameName, "chkSameName");
            this.chkSameName.Name = "chkSameName";
            this.chkSameName.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkSameSize, "chkSameSize");
            this.chkSameSize.Name = "chkSameSize";
            this.chkSameSize.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkSameContent, "chkSameContent");
            this.chkSameContent.Name = "chkSameContent";
            this.chkSameContent.UseVisualStyleBackColor = true;
            this.chkSameContent.CheckedChanged += new EventHandler(this.chkSameContent_CheckedChanged);
            manager.ApplyResources(this.lblInfo, "lblInfo");
            this.lblInfo.MaximumSize = new Size(300, 0);
            this.lblInfo.Name = "lblInfo";
            this.pnlOptions.Controls.Add(this.chkSameName);
            this.pnlOptions.Controls.Add(this.chkSameSize);
            this.pnlOptions.Controls.Add(this.chkSameContent);
            manager.ApplyResources(this.pnlOptions, "pnlOptions");
            this.pnlOptions.Name = "pnlOptions";
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.pnlOptions);
            base.Controls.Add(this.lblInfo);
            base.Name = "DuplicateSearchControl";
            base.ClientSizeChanged += new EventHandler(this.DuplicateSearchControl_ClientSizeChanged);
            this.pnlOptions.ResumeLayout(false);
            this.pnlOptions.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public FindDuplicateOptions DuplicateOptions
        {
            get
            {
                return (((this.chkSameName.Checked ? FindDuplicateOptions.SameName : ((FindDuplicateOptions) 0)) | (this.chkSameSize.Checked ? FindDuplicateOptions.SameSize : ((FindDuplicateOptions) 0))) | (this.chkSameContent.Checked ? FindDuplicateOptions.SameContent : ((FindDuplicateOptions) 0)));
            }
            set
            {
                this.chkSameName.Checked = (value & FindDuplicateOptions.SameName) > 0;
                this.chkSameSize.Checked = (value & FindDuplicateOptions.SameSize) > 0;
                this.chkSameContent.Checked = (value & FindDuplicateOptions.SameContent) > 0;
            }
        }
    }
}

