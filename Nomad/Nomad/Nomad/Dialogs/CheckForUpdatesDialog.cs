namespace Nomad.Dialogs
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Commons.IO;
    using Nomad.Configuration;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Windows.Forms;

    public class CheckForUpdatesDialog : BasicDialog
    {
        private Button btnOk;
        private Bevel bvlButtons;
        private IContainer components = null;
        private LinkLabel lblCheckResult;
        private PictureBox pictureBox;
        private TableLayoutPanel tlpBack;

        public CheckForUpdatesDialog()
        {
            this.InitializeComponent();
            base.LocalizeForm();
            this.lblCheckResult.ParseLinks();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private static void CheckForUpdateCallback(IAsyncResult ar)
        {
            object[] asyncState = (object[]) ar.AsyncState;
            HttpWebRequest request = (HttpWebRequest) asyncState[0];
            SynchronizationContext context = (SynchronizationContext) asyncState[1];
            Ini ini = null;
            Exception exception = null;
            bool flag = false;
            try
            {
                HttpWebResponse response = (HttpWebResponse) request.EndGetResponse(ar);
                ini = new Ini();
                using (TextReader reader = new StreamReader(response.GetResponseStream()))
                {
                    ini.Read(reader);
                }
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
                ini.Set("Version", "CurrentVersion", versionInfo.ProductVersion);
            }
            catch (WebException exception2)
            {
                exception = exception2;
                flag = true;
            }
            catch (Exception exception3)
            {
                exception = exception3;
            }
            context.Send(new SendOrPostCallback(CheckForUpdatesDialog.ShowCheckResult), new object[] { exception, flag, ini });
        }

        public static void CheckForUpdates()
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Settings.Default.CheckForUpdatesUrl);
            request.BeginGetResponse(new AsyncCallback(CheckForUpdatesDialog.CheckForUpdateCallback), new object[] { request, SynchronizationContext.Current });
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(CheckForUpdatesDialog));
            this.btnOk = new Button();
            this.tlpBack = new TableLayoutPanel();
            this.pictureBox = new PictureBox();
            this.lblCheckResult = new LinkLabel();
            this.bvlButtons = new Bevel();
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.SuspendLayout();
            this.tlpBack.SuspendLayout();
            ((ISupportInitialize) this.pictureBox).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpButtons");
            panel.BackColor = Color.Gainsboro;
            panel.Controls.Add(this.btnOk, 1, 0);
            panel.Name = "tlpButtons";
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.Cancel;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.pictureBox, 0, 0);
            this.tlpBack.Controls.Add(this.lblCheckResult, 1, 0);
            this.tlpBack.Name = "tlpBack";
            manager.ApplyResources(this.pictureBox, "pictureBox");
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.TabStop = false;
            manager.ApplyResources(this.lblCheckResult, "lblCheckResult");
            this.lblCheckResult.Name = "lblCheckResult";
            this.lblCheckResult.Tag = "http://www.nomad-net.info";
            this.lblCheckResult.UseMnemonic = false;
            this.lblCheckResult.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lblCheckResult_LinkClicked);
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Top;
            this.bvlButtons.Style = Border3DStyle.Flat;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnOk;
            base.Controls.Add(panel);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(this.tlpBack);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "CheckForUpdatesDialog";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            ((ISupportInitialize) this.pictureBox).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lblCheckResult_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start((string) ((Control) sender).Tag);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                base.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private static void ShowCheckResult(object state)
        {
            object[] objArray = (object[]) state;
            Exception e = (Exception) objArray[0];
            CheckForUpdatesDialog dialog = new CheckForUpdatesDialog();
            Icon information = null;
            if (e == null)
            {
                try
                {
                    Ini ini = (Ini) objArray[2];
                    Version version = new Version(ini.Get("Version", "LatestVersion"));
                    ReleaseType type = (ReleaseType) System.Enum.Parse(typeof(ReleaseType), ini.Get("Version", "LatestRelease"));
                    Version version2 = new Version(ini.Get("Version", "CurrentVersion"));
                    string str = ini.Get("Download", "DownloadUrl." + CultureInfo.CurrentUICulture.Name);
                    if ((string.IsNullOrEmpty(str) && (CultureInfo.CurrentUICulture.Parent != null)) && (CultureInfo.CurrentUICulture.Parent != CultureInfo.InvariantCulture))
                    {
                        str = ini.Get("Download", "DownloadUrl." + CultureInfo.CurrentUICulture.Parent.Name);
                    }
                    if (string.IsNullOrEmpty(str))
                    {
                        str = ini.Get("Download", "DownloadUrl");
                    }
                    if (!string.IsNullOrEmpty(str))
                    {
                        dialog.lblCheckResult.Tag = str;
                    }
                    information = SystemIcons.Information;
                    if (version > version2)
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(ReleaseType));
                        dialog.lblCheckResult.Text = string.Format(dialog.lblCheckResult.Text, new object[] { version2, converter.ConvertToString(ReleaseType.RC), version, converter.ConvertToString(type) });
                    }
                    else
                    {
                        dialog.lblCheckResult.Text = Resources.sNewVersionNotAvailable;
                        dialog.lblCheckResult.LinkArea = new LinkArea(0, 0);
                    }
                }
                catch (Exception exception2)
                {
                    e = exception2;
                }
            }
            if (e != null)
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Error, e);
                dialog.lblCheckResult.Text = e.Message;
                dialog.lblCheckResult.LinkArea = new LinkArea(0, 0);
                information = ((bool) objArray[1]) ? SystemIcons.Warning : SystemIcons.Error;
            }
            dialog.pictureBox.Image = information.ToBitmap();
            dialog.Show();
        }
    }
}

