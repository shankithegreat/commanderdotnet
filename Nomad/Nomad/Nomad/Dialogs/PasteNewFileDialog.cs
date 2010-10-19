namespace Nomad.Dialogs
{
    using Nomad.Commons;
    using Nomad.Commons.Collections;
    using Nomad.Commons.Controls;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class PasteNewFileDialog : BasicDialog
    {
        private Button btnCancel;
        private Button btnOk;
        private RadioButton[] ButtonArray;
        private Bevel bvlButtons;
        private ComboBox cmbImageFormat;
        private ComboBox cmbNewName;
        private ComboBox cmbTextEncoding;
        private IContainer components = null;
        private Label lblFolder;
        private Label lblNewName;
        private RadioButton rbFormatCsv;
        private RadioButton rbFormatHtml;
        private RadioButton rbFormatImage;
        private RadioButton rbFormatRtf;
        private RadioButton rbFormatText;
        private VirtualItemToolStrip tsFolder;
        private ValidatorProvider Validator;

        public PasteNewFileDialog()
        {
            this.InitializeComponent();
            this.Validator.TooltipTitle = Resources.sInvalidName;
            this.rbFormatImage.Tag = new string[] { DataFormats.Bitmap, DataFormats.Dib };
            this.rbFormatText.Tag = new string[] { DataFormats.UnicodeText, DataFormats.Text, DataFormats.OemText };
            this.rbFormatRtf.Tag = DataFormats.Rtf;
            this.rbFormatHtml.Tag = DataFormats.Html;
            this.rbFormatCsv.Tag = DataFormats.CommaSeparatedValue;
            this.ButtonArray = new RadioButton[] { this.rbFormatImage, this.rbFormatText, this.rbFormatRtf, this.rbFormatHtml, this.rbFormatCsv };
            EncodingInfo[] encodings = Encoding.GetEncodings();
            Array.Sort<EncodingInfo>(encodings, delegate (EncodingInfo x, EncodingInfo y) {
                return string.Compare(x.DisplayName, y.DisplayName, StringComparison.CurrentCultureIgnoreCase);
            });
            this.cmbTextEncoding.DataSource = encodings;
            foreach (EncodingInfo info in encodings)
            {
                if (info.CodePage == Encoding.UTF8.CodePage)
                {
                    this.cmbTextEncoding.SelectedItem = info;
                    break;
                }
            }
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            Array.Sort<ImageCodecInfo>(imageEncoders, delegate (ImageCodecInfo x, ImageCodecInfo y) {
                return string.Compare(x.CodecName, y.CodecName, StringComparison.CurrentCultureIgnoreCase);
            });
            this.cmbImageFormat.DataSource = imageEncoders;
            foreach (ImageCodecInfo info2 in imageEncoders)
            {
                if (info2.FormatID == ImageFormat.Png.Guid)
                {
                    this.cmbImageFormat.SelectedItem = info2;
                    break;
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                base.DialogResult = DialogResult.None;
            }
        }

        public static string ChangeImageExtension(string imageName, ImageCodecInfo codec)
        {
            if (imageName == null)
            {
                throw new ArgumentNullException();
            }
            if (imageName == string.Empty)
            {
                throw new ArgumentException();
            }
            foreach (string str in StringHelper.SplitString(codec.FilenameExtension, new char[] { ';' }))
            {
                if (str.StartsWith("*.", StringComparison.Ordinal))
                {
                    return Path.ChangeExtension(imageName, str.Substring(1).ToLower());
                }
            }
            return imageName;
        }

        private void cmbImageFormat_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string newName = this.NewName;
            if (!string.IsNullOrEmpty(newName))
            {
                this.cmbNewName.Text = ChangeImageExtension(newName, (ImageCodecInfo) this.cmbImageFormat.SelectedItem);
            }
        }

        private void cmbNewName_Enter(object sender, EventArgs e)
        {
            if (Settings.Default.SelectNameWithoutExt && this.Validator.GetIsValid(this.cmbNewName))
            {
                int Len = this.cmbNewName.Text.LastIndexOf('.');
                if (Len > 0)
                {
                    base.BeginInvoke(delegate {
                        this.cmbNewName.SelectionLength = Len;
                    });
                }
            }
        }

        private void cmbNewName_Validating(object sender, CancelEventArgs e)
        {
            string newName = this.NewName;
            if (string.IsNullOrEmpty(newName))
            {
                e.Cancel = true;
                this.Validator.RemoveValidateError((Control) sender);
            }
            else
            {
                e.Cancel = newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
                if (e.Cancel)
                {
                    this.Validator.SetValidateError((Control) sender, Resources.sInvalidCharsInFileName);
                }
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

        public bool Execute(IVirtualFolder destFolder, string[] formats)
        {
            this.tsFolder.Add(destFolder);
            this.InitializeFormats(new HashSet<string>(formats));
            return (base.ShowDialog() == DialogResult.OK);
        }

        public static string GetDefaultFormatExtension(string format)
        {
            if (((format == DataFormats.UnicodeText) || (format == DataFormats.Text)) || (format == DataFormats.OemText))
            {
                return ".txt";
            }
            if (format == DataFormats.Rtf)
            {
                return ".rtf";
            }
            if (format == DataFormats.Html)
            {
                return ".htm";
            }
            if (format == DataFormats.CommaSeparatedValue)
            {
                return ".csv";
            }
            return null;
        }

        public static bool HasSupportedFormats(IDataObject dataObject)
        {
            if (dataObject == null)
            {
                throw new ArgumentNullException();
            }
            foreach (string str in SupportedFormats)
            {
                if (dataObject.GetDataPresent(str))
                {
                    return true;
                }
            }
            return false;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PasteNewFileDialog));
            this.cmbImageFormat = new ComboBox();
            this.rbFormatImage = new RadioButton();
            this.cmbTextEncoding = new ComboBox();
            this.rbFormatHtml = new RadioButton();
            this.rbFormatText = new RadioButton();
            this.rbFormatRtf = new RadioButton();
            this.rbFormatCsv = new RadioButton();
            this.lblNewName = new Label();
            this.cmbNewName = new ComboBox();
            this.lblFolder = new Label();
            this.tsFolder = new VirtualItemToolStrip(this.components);
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.Validator = new ValidatorProvider();
            this.bvlButtons = new Bevel();
            GroupBox box = new GroupBox();
            TableLayoutPanel panel = new TableLayoutPanel();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            TableLayoutPanel panel3 = new TableLayoutPanel();
            box.SuspendLayout();
            panel.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(box, "grpDataFormat");
            panel2.SetColumnSpan(box, 3);
            box.Controls.Add(panel);
            box.Name = "grpDataFormat";
            box.TabStop = false;
            manager.ApplyResources(panel, "tlpPasteFormat");
            panel.Controls.Add(this.cmbImageFormat, 1, 0);
            panel.Controls.Add(this.rbFormatImage, 0, 0);
            panel.Controls.Add(this.cmbTextEncoding, 1, 1);
            panel.Controls.Add(this.rbFormatHtml, 0, 3);
            panel.Controls.Add(this.rbFormatText, 0, 1);
            panel.Controls.Add(this.rbFormatRtf, 0, 2);
            panel.Controls.Add(this.rbFormatCsv, 0, 4);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpPasteFormat";
            this.cmbImageFormat.DisplayMember = "CodecName";
            manager.ApplyResources(this.cmbImageFormat, "cmbImageFormat");
            this.cmbImageFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbImageFormat.FormattingEnabled = true;
            this.cmbImageFormat.Name = "cmbImageFormat";
            this.cmbImageFormat.SelectionChangeCommitted += new EventHandler(this.cmbImageFormat_SelectionChangeCommitted);
            manager.ApplyResources(this.rbFormatImage, "rbFormatImage");
            this.rbFormatImage.Name = "rbFormatImage";
            this.rbFormatImage.TabStop = true;
            this.rbFormatImage.UseVisualStyleBackColor = true;
            this.rbFormatImage.CheckedChanged += new EventHandler(this.rbPasteAsImage_CheckedChanged);
            this.cmbTextEncoding.DisplayMember = "DisplayName";
            manager.ApplyResources(this.cmbTextEncoding, "cmbTextEncoding");
            this.cmbTextEncoding.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTextEncoding.FormattingEnabled = true;
            this.cmbTextEncoding.Name = "cmbTextEncoding";
            manager.ApplyResources(this.rbFormatHtml, "rbFormatHtml");
            panel.SetColumnSpan(this.rbFormatHtml, 2);
            this.rbFormatHtml.Name = "rbFormatHtml";
            this.rbFormatHtml.TabStop = true;
            this.rbFormatHtml.UseVisualStyleBackColor = true;
            this.rbFormatHtml.CheckedChanged += new EventHandler(this.rbPasteAsText_CheckedChanged);
            manager.ApplyResources(this.rbFormatText, "rbFormatText");
            this.rbFormatText.Name = "rbFormatText";
            this.rbFormatText.TabStop = true;
            this.rbFormatText.UseVisualStyleBackColor = true;
            this.rbFormatText.CheckedChanged += new EventHandler(this.rbPasteAsText_CheckedChanged);
            manager.ApplyResources(this.rbFormatRtf, "rbFormatRtf");
            panel.SetColumnSpan(this.rbFormatRtf, 2);
            this.rbFormatRtf.Name = "rbFormatRtf";
            this.rbFormatRtf.TabStop = true;
            this.rbFormatRtf.UseVisualStyleBackColor = true;
            this.rbFormatRtf.CheckedChanged += new EventHandler(this.rbPasteAsText_CheckedChanged);
            manager.ApplyResources(this.rbFormatCsv, "rbFormatCsv");
            panel.SetColumnSpan(this.rbFormatCsv, 2);
            this.rbFormatCsv.Name = "rbFormatCsv";
            this.rbFormatCsv.TabStop = true;
            this.rbFormatCsv.UseVisualStyleBackColor = true;
            this.rbFormatCsv.CheckedChanged += new EventHandler(this.rbPasteAsText_CheckedChanged);
            manager.ApplyResources(panel2, "tlpBack");
            panel2.Controls.Add(box, 0, 4);
            panel2.Controls.Add(this.lblNewName, 0, 2);
            panel2.Controls.Add(this.cmbNewName, 0, 3);
            panel2.Controls.Add(this.lblFolder, 0, 0);
            panel2.Controls.Add(this.tsFolder, 0, 1);
            panel2.Name = "tlpBack";
            manager.ApplyResources(this.lblNewName, "lblNewName");
            panel2.SetColumnSpan(this.lblNewName, 3);
            this.lblNewName.Name = "lblNewName";
            panel2.SetColumnSpan(this.cmbNewName, 3);
            manager.ApplyResources(this.cmbNewName, "cmbNewName");
            this.cmbNewName.Name = "cmbNewName";
            this.Validator.SetValidateOn(this.cmbNewName, ValidateOn.TextChanged);
            this.cmbNewName.Validating += new CancelEventHandler(this.cmbNewName_Validating);
            this.cmbNewName.Enter += new EventHandler(this.cmbNewName_Enter);
            manager.ApplyResources(this.lblFolder, "lblFolder");
            panel2.SetColumnSpan(this.lblFolder, 3);
            this.lblFolder.Name = "lblFolder";
            this.tsFolder.BackColor = SystemColors.ButtonFace;
            panel2.SetColumnSpan(this.tsFolder, 3);
            this.tsFolder.GripStyle = ToolStripGripStyle.Hidden;
            manager.ApplyResources(this.tsFolder, "tsFolder");
            this.tsFolder.Name = "tsFolder";
            manager.ApplyResources(panel3, "tlpButtons");
            panel3.Controls.Add(this.btnOk, 1, 0);
            panel3.Controls.Add(this.btnCancel, 2, 0);
            panel3.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel3.Name = "tlpButtons";
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.Validator.Owner = this;
            this.Validator.OwnerFormValidate = FormValidate.DisableAcceptButton;
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Top;
            this.bvlButtons.Style = Border3DStyle.Flat;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(panel3);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(panel2);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "PasteNewFileDialog";
            base.ShowInTaskbar = false;
            base.Activated += new EventHandler(this.PasteNewFileDialog_Activated);
            box.ResumeLayout(false);
            box.PerformLayout();
            panel.ResumeLayout(false);
            panel.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeFormats(HashSet<string> formats)
        {
            bool flag = false;
            foreach (RadioButton button in this.ButtonArray)
            {
                bool flag2 = false;
                string[] tag = button.Tag as string[];
                if (tag != null)
                {
                    foreach (string str in tag)
                    {
                        flag2 |= formats.Contains(str);
                    }
                }
                else
                {
                    flag2 = formats.Contains((string) button.Tag);
                }
                if (!(!flag2 || flag))
                {
                    button.Checked = true;
                    flag = true;
                }
                button.Enabled = flag2;
            }
            if (!flag)
            {
                this.cmbNewName.Enabled = false;
                this.lblNewName.Enabled = false;
                this.btnOk.Enabled = false;
            }
        }

        private void PasteNewFileDialog_Activated(object sender, EventArgs e)
        {
            if (this.cmbNewName.Focused)
            {
                this.cmbNewName_Enter(sender, e);
            }
        }

        private void rbPasteAsImage_CheckedChanged(object sender, EventArgs e)
        {
            this.cmbTextEncoding.Enabled = false;
            this.cmbImageFormat.Enabled = true;
            this.cmbImageFormat_SelectionChangeCommitted(sender, e);
        }

        private void rbPasteAsText_CheckedChanged(object sender, EventArgs e)
        {
            this.cmbTextEncoding.Enabled = this.rbFormatText.Checked;
            this.cmbImageFormat.Enabled = false;
            string newName = this.NewName;
            string defaultFormatExtension = GetDefaultFormatExtension(this.DataFormat);
            if (!(string.IsNullOrEmpty(this.NewName) || string.IsNullOrEmpty(defaultFormatExtension)))
            {
                this.cmbNewName.Text = Path.ChangeExtension(newName, defaultFormatExtension);
            }
        }

        public string DataFormat
        {
            get
            {
                foreach (RadioButton button in this.ButtonArray)
                {
                    if (button.Checked)
                    {
                        string[] tag = button.Tag as string[];
                        if (tag != null)
                        {
                            return tag[0];
                        }
                        return (string) button.Tag;
                    }
                }
                return null;
            }
        }

        public static ImageCodecInfo DefaultImageCodec
        {
            get
            {
                foreach (ImageCodecInfo info in ImageCodecInfo.GetImageEncoders())
                {
                    if (info.FormatID == ImageFormat.Png.Guid)
                    {
                        return info;
                    }
                }
                return null;
            }
        }

        public ImageCodecInfo ImageCodec
        {
            get
            {
                return (ImageCodecInfo) this.cmbImageFormat.SelectedItem;
            }
        }

        public string NewName
        {
            get
            {
                return this.cmbNewName.Text.Trim();
            }
            set
            {
                this.cmbNewName.Text = value;
            }
        }

        public static string[] SupportedFormats
        {
            get
            {
                return new string[] { DataFormats.Bitmap, DataFormats.Dib, DataFormats.UnicodeText, DataFormats.Text, DataFormats.OemText, DataFormats.Rtf, DataFormats.Html, DataFormats.CommaSeparatedValue };
            }
        }

        public Encoding TextEncoding
        {
            get
            {
                return ((EncodingInfo) this.cmbTextEncoding.SelectedItem).GetEncoding();
            }
        }
    }
}

