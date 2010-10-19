namespace Nomad.Dialogs
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Configuration;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class AboutDialog : BasicDialog
    {
        private VisualStyleRenderer BackgroundRenderer;
        private System.Windows.Forms.Button btnCheckForUpdates;
        private System.Windows.Forms.Button btnClose;
        private Bevel bvlButtons;
        private IContainer components = null;
        private PictureBox imgLicense;
        private Label lblCopyright;
        private LinkLabel lblEMail;
        private LinkLabel lblHomePage;
        private LinkLabel lblLicense;
        private Label lblNomad;
        private Label lblTarget;
        private Label lblVersion;
        private TableLayoutPanel tlpLicense;

        public AboutDialog()
        {
            this.InitializeComponent();
            base.LocalizeForm();
            this.imgLicense.Image = IconSet.Information;
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            Module module = entryAssembly.GetModule(Path.GetFileName(entryAssembly.Location));
            if (module != null)
            {
                PortableExecutableKinds kinds;
                ImageFileMachine machine;
                module.GetPEKind(out kinds, out machine);
                if ((kinds & PortableExecutableKinds.Required32Bit) > PortableExecutableKinds.NotAPortableExecutableImage)
                {
                    this.lblTarget.Text = "x86";
                }
                else if ((kinds & PortableExecutableKinds.PE32Plus) > PortableExecutableKinds.NotAPortableExecutableImage)
                {
                    this.lblTarget.Text = "x64";
                }
            }
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(ReleaseType));
            this.lblVersion.Text = string.Format(this.lblVersion.Text, entryAssembly.GetName().Version, converter.ConvertToString(ReleaseType.RC));
            this.lblLicense.ParseLinks();
        }

        private void btnCheckForUpdates_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is CheckForUpdatesDialog)
                {
                    form.Close();
                    break;
                }
            }
            CheckForUpdatesDialog.CheckForUpdates();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(AboutDialog));
            this.lblLicense = new LinkLabel();
            this.lblNomad = new Label();
            this.lblVersion = new Label();
            this.lblCopyright = new Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblHomePage = new LinkLabel();
            this.lblEMail = new LinkLabel();
            this.btnCheckForUpdates = new System.Windows.Forms.Button();
            this.lblTarget = new Label();
            this.tlpLicense = new TableLayoutPanel();
            this.imgLicense = new PictureBox();
            this.bvlButtons = new Bevel();
            PictureBox box = new PictureBox();
            TableLayoutPanel panel = new TableLayoutPanel();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            ((ISupportInitialize) box).BeginInit();
            panel.SuspendLayout();
            this.tlpLicense.SuspendLayout();
            ((ISupportInitialize) this.imgLicense).BeginInit();
            panel2.SuspendLayout();
            base.SuspendLayout();
            box.Image = Resources.NomadAboutLogo;
            manager.ApplyResources(box, "imgNomad");
            box.Name = "imgNomad";
            panel.SetRowSpan(box, 6);
            box.TabStop = false;
            manager.ApplyResources(this.lblLicense, "lblLicense");
            this.lblLicense.Name = "lblLicense";
            this.lblLicense.TabStop = true;
            this.lblLicense.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lblLicense_LinkClicked);
            manager.ApplyResources(this.lblNomad, "lblNomad");
            this.lblNomad.Name = "lblNomad";
            manager.ApplyResources(this.lblVersion, "lblVersion");
            panel.SetColumnSpan(this.lblVersion, 2);
            this.lblVersion.Name = "lblVersion";
            manager.ApplyResources(this.lblCopyright, "lblCopyright");
            panel.SetColumnSpan(this.lblCopyright, 2);
            this.lblCopyright.Name = "lblCopyright";
            manager.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.lblHomePage, "lblHomePage");
            panel.SetColumnSpan(this.lblHomePage, 2);
            this.lblHomePage.Name = "lblHomePage";
            this.lblHomePage.TabStop = true;
            this.lblHomePage.Tag = "http://www.nomad-net.info";
            this.lblHomePage.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lblHomePage_LinkClicked);
            manager.ApplyResources(this.lblEMail, "lblEMail");
            panel.SetColumnSpan(this.lblEMail, 2);
            this.lblEMail.Name = "lblEMail";
            this.lblEMail.TabStop = true;
            this.lblEMail.Tag = "mailto:support@nomad-net.info";
            this.lblEMail.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lblHomePage_LinkClicked);
            manager.ApplyResources(this.btnCheckForUpdates, "btnCheckForUpdates");
            this.btnCheckForUpdates.Name = "btnCheckForUpdates";
            this.btnCheckForUpdates.UseVisualStyleBackColor = true;
            this.btnCheckForUpdates.Click += new EventHandler(this.btnCheckForUpdates_Click);
            manager.ApplyResources(this.lblTarget, "lblTarget");
            this.lblTarget.ForeColor = SystemColors.GrayText;
            this.lblTarget.Name = "lblTarget";
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.lblNomad, 1, 0);
            panel.Controls.Add(this.lblTarget, 2, 0);
            panel.Controls.Add(this.lblVersion, 1, 1);
            panel.Controls.Add(this.lblCopyright, 1, 2);
            panel.Controls.Add(this.lblHomePage, 1, 3);
            panel.Controls.Add(this.lblEMail, 1, 4);
            panel.Controls.Add(box, 0, 0);
            panel.Controls.Add(this.tlpLicense, 0, 6);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpBack";
            manager.ApplyResources(this.tlpLicense, "tlpLicense");
            panel.SetColumnSpan(this.tlpLicense, 3);
            this.tlpLicense.Controls.Add(this.lblLicense, 1, 0);
            this.tlpLicense.Controls.Add(this.imgLicense, 0, 0);
            this.tlpLicense.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpLicense.Name = "tlpLicense";
            this.tlpLicense.Paint += new PaintEventHandler(this.tlpLicense_Paint);
            manager.ApplyResources(this.imgLicense, "imgLicense");
            this.imgLicense.Name = "imgLicense";
            this.imgLicense.TabStop = false;
            manager.ApplyResources(panel2, "tlpButtons");
            panel2.Controls.Add(this.btnCheckForUpdates, 0, 0);
            panel2.Controls.Add(this.btnClose, 2, 0);
            panel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel2.Name = "tlpButtons";
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Top;
            this.bvlButtons.Style = Border3DStyle.Flat;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnClose;
            base.Controls.Add(panel2);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(panel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AboutDialog";
            base.ShowInTaskbar = false;
            ((ISupportInitialize) box).EndInit();
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.tlpLicense.ResumeLayout(false);
            this.tlpLicense.PerformLayout();
            ((ISupportInitialize) this.imgLicense).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lblHomePage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start((string) ((Control) sender).Tag);
            }
            catch (Win32Exception exception)
            {
                MessageDialog.ShowException(this, exception);
            }
        }

        private void lblLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SettingsManager.ShowLicenseInformation();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.lblLicense.Font = new Font(SystemFonts.IconTitleFont.FontFamily, this.lblLicense.Font.SizeInPoints);
            this.lblTarget.Font = new Font(this.Font.FontFamily, this.lblTarget.Font.SizeInPoints);
        }

        protected override void OnThemeChanged(EventArgs e)
        {
            if (VisualStyleRenderer.IsSupported && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ToolTip.Standard.Normal))
            {
                this.BackgroundRenderer = new VisualStyleRenderer(VisualStyleElement.ToolTip.Standard.Normal);
                this.tlpLicense.ResetBackColor();
                this.imgLicense.BackColor = Color.Transparent;
                this.lblLicense.BackColor = Color.Transparent;
                this.lblLicense.ForeColor = this.BackgroundRenderer.GetColor(ColorProperty.TextColor);
            }
            else
            {
                this.BackgroundRenderer = null;
                this.tlpLicense.BackColor = SystemColors.Info;
                this.imgLicense.ResetBackColor();
                this.lblLicense.ResetBackColor();
                this.lblLicense.ForeColor = SystemColors.InfoText;
            }
            base.OnThemeChanged(e);
        }

        private void tlpLicense_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control) sender;
            if (this.BackgroundRenderer != null)
            {
                this.BackgroundRenderer.DrawBackground(e.Graphics, control.ClientRectangle);
            }
            else
            {
                e.Graphics.DrawRectangle(Pens.Black, 0, 0, control.ClientSize.Width - 1, control.ClientSize.Height - 1);
            }
        }
    }
}

