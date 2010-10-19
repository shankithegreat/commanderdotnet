namespace Nomad.Dialogs
{
    using Microsoft.Shell;
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Configuration;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class NewFileDialog : BasicDialog
    {
        private Button btnCancel;
        private Button btnOk;
        private Bevel bvlButtons;
        private ComboBox cmbNewName;
        private ComboBoxEx cmbNewType;
        private IContainer components = null;
        private ImageList imgNewType;
        private Label lblItem;
        private Label lblNewName;
        private Label lblNewType;
        private VirtualItemToolStrip tsCurrentFolder;
        private ValidatorProvider Validator;

        public NewFileDialog()
        {
            this.InitializeComponent();
            this.Validator.TooltipTitle = Resources.sInvalidName;
            this.cmbNewType.Items.Add(new ShellNew(ShellNewCommand.NullFile, string.Empty, null));
            foreach (ShellNew new2 in ShellNew.All)
            {
                switch (new2.CommandType)
                {
                    case ShellNewCommand.NullFile:
                    case ShellNewCommand.Data:
                    case ShellNewCommand.FileName:
                        this.cmbNewType.Items.Add(new2);
                        break;
                }
            }
            if (this.cmbNewType.Items.Count == 0)
            {
                this.cmbNewType.Enabled = false;
                this.lblNewType.Enabled = false;
                this.cmbNewName.Enabled = false;
                this.lblNewName.Enabled = false;
                this.btnOk.Enabled = false;
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

        private void cmbNewType_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color windowText;
            if ((e.State & DrawItemState.Checked) > DrawItemState.None)
            {
                windowText = SystemColors.WindowText;
            }
            else
            {
                e.DrawBackground();
                windowText = e.ForeColor;
            }
            if (e.Index >= 0)
            {
                string sCreateNewUntypedFile;
                ShellNew new2 = (ShellNew) this.cmbNewType.Items[e.Index];
                int index = this.imgNewType.Images.IndexOfKey(new2.Extension);
                if (index < 0)
                {
                    Image defaultIcon;
                    if (string.IsNullOrEmpty(new2.Extension))
                    {
                        defaultIcon = ImageProvider.Default.GetDefaultIcon(DefaultIcon.UnknownFile, this.imgNewType.ImageSize);
                    }
                    else
                    {
                        defaultIcon = ImageProvider.Default.GetDefaultFileIcon("test" + new2.Extension, this.imgNewType.ImageSize);
                    }
                    this.imgNewType.Images.Add(new2.Extension, defaultIcon);
                    index = this.imgNewType.Images.Count - 1;
                }
                int num2 = e.Bounds.Height - this.imgNewType.ImageSize.Height;
                this.imgNewType.Draw(e.Graphics, e.Bounds.Left + 4, (e.Bounds.Top + (num2 / 2)) + (num2 % 2), index);
                if (string.IsNullOrEmpty(new2.Extension))
                {
                    sCreateNewUntypedFile = Resources.sCreateNewUntypedFile;
                }
                else
                {
                    sCreateNewUntypedFile = string.Format("{0} ({1})", new2.Name, new2.Extension);
                }
                Rectangle bounds = Rectangle.FromLTRB(e.Bounds.Left + 20, e.Bounds.Top, e.Bounds.Right, e.Bounds.Bottom);
                TextRenderer.DrawText(e.Graphics, sCreateNewUntypedFile, e.Font, bounds, windowText, TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter);
                if ((e.State & DrawItemState.Focus) > DrawItemState.None)
                {
                    e.DrawFocusRectangle();
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

        public bool Execute(IVirtualFolder folder)
        {
            this.tsCurrentFolder.Add(folder);
            HistorySettings.PopulateComboBox(this.cmbNewName, HistorySettings.Default.NewFile);
            if (this.cmbNewType.SelectedIndex < 0)
            {
                this.cmbNewType.SelectedIndex = 0;
                for (int i = 0; i < this.cmbNewType.Items.Count; i++)
                {
                    if (string.Equals(((ShellNew) this.cmbNewType.Items[i]).Extension, Settings.Default.NewFileLastExt, StringComparison.OrdinalIgnoreCase))
                    {
                        this.cmbNewType.SelectedIndex = i;
                        break;
                    }
                }
            }
            bool flag = base.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                HistorySettings.Default.AddStringToNewFile(this.cmbNewName.Text);
                Settings.Default.NewFileLastExt = this.Command.Extension;
            }
            return flag;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(NewFileDialog));
            this.lblNewType = new Label();
            this.cmbNewType = new ComboBoxEx();
            this.cmbNewName = new ComboBox();
            this.lblNewName = new Label();
            this.lblItem = new Label();
            this.tsCurrentFolder = new VirtualItemToolStrip(this.components);
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.imgNewType = new ImageList(this.components);
            this.Validator = new ValidatorProvider();
            this.bvlButtons = new Bevel();
            TableLayoutPanel panel = new TableLayoutPanel();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            panel.SuspendLayout();
            panel2.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.lblNewType, 0, 2);
            panel.Controls.Add(this.cmbNewType, 0, 3);
            panel.Controls.Add(this.cmbNewName, 0, 5);
            panel.Controls.Add(this.lblNewName, 0, 4);
            panel.Controls.Add(this.lblItem, 0, 0);
            panel.Controls.Add(this.tsCurrentFolder, 0, 1);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpBack";
            manager.ApplyResources(this.lblNewType, "lblNewType");
            this.lblNewType.Name = "lblNewType";
            manager.ApplyResources(this.cmbNewType, "cmbNewType");
            this.cmbNewType.DrawMode = DrawMode.OwnerDrawFixed;
            this.cmbNewType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbNewType.Name = "cmbNewType";
            this.cmbNewType.DrawItem += new DrawItemEventHandler(this.cmbNewType_DrawItem);
            manager.ApplyResources(this.cmbNewName, "cmbNewName");
            this.cmbNewName.Name = "cmbNewName";
            this.Validator.SetValidateOn(this.cmbNewName, ValidateOn.TextChanged);
            this.cmbNewName.Validating += new CancelEventHandler(this.cmbNewName_Validating);
            manager.ApplyResources(this.lblNewName, "lblNewName");
            this.lblNewName.Name = "lblNewName";
            manager.ApplyResources(this.lblItem, "lblItem");
            this.lblItem.Name = "lblItem";
            this.tsCurrentFolder.BackColor = SystemColors.ButtonFace;
            this.tsCurrentFolder.GripStyle = ToolStripGripStyle.Hidden;
            manager.ApplyResources(this.tsCurrentFolder, "tsCurrentFolder");
            this.tsCurrentFolder.Name = "tsCurrentFolder";
            manager.ApplyResources(panel2, "tlpButtons");
            panel2.Controls.Add(this.btnOk, 1, 0);
            panel2.Controls.Add(this.btnCancel, 2, 0);
            panel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel2.Name = "tlpButtons";
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.imgNewType.ColorDepth = ColorDepth.Depth32Bit;
            manager.ApplyResources(this.imgNewType, "imgNewType");
            this.imgNewType.TransparentColor = Color.Transparent;
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
            base.Controls.Add(panel2);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(panel);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "NewFileDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.NewFileDialog_Shown);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void NewFileDialog_Shown(object sender, EventArgs e)
        {
            this.cmbNewName.Select();
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            if (((specified & BoundsSpecified.Height) > BoundsSpecified.None) && (factor.Height != 1.0))
            {
                this.cmbNewType.ItemHeight = (int) Math.Round((double) (this.cmbNewType.ItemHeight * factor.Height));
            }
        }

        public ShellNew Command
        {
            get
            {
                return (this.cmbNewType.SelectedItem as ShellNew);
            }
            set
            {
                this.cmbNewType.SelectedItem = value;
            }
        }

        public string NewName
        {
            get
            {
                return this.cmbNewName.Text.Trim();
            }
        }
    }
}

