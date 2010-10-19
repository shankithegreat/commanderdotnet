namespace Nomad.Dialogs
{
    using Microsoft;
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    public class MessageDialog : BasicDialog
    {
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        protected Button[] Buttons;
        public static readonly MessageDialogResult[] ButtonsAbortRetryIgnore = new MessageDialogResult[] { MessageDialogResult.Retry, MessageDialogResult.Ignore, MessageDialogResult.Abort };
        public static readonly MessageDialogResult[] ButtonsOk = new MessageDialogResult[] { MessageDialogResult.OK };
        public static readonly MessageDialogResult[] ButtonsOkCancel = new MessageDialogResult[] { MessageDialogResult.OK, MessageDialogResult.Cancel };
        public static readonly MessageDialogResult[] ButtonsRetryIgnoreSkipCancel = new MessageDialogResult[] { MessageDialogResult.Retry, MessageDialogResult.Ignore, MessageDialogResult.Skip, MessageDialogResult.Cancel };
        public static readonly MessageDialogResult[] ButtonsRetrySkipCancel = new MessageDialogResult[] { MessageDialogResult.Retry, MessageDialogResult.Skip, MessageDialogResult.Cancel };
        public static readonly MessageDialogResult[] ButtonsSkipCancel = new MessageDialogResult[] { MessageDialogResult.Skip, MessageDialogResult.Cancel };
        public static readonly MessageDialogResult[] ButtonsYesNo = new MessageDialogResult[] { MessageDialogResult.Yes, MessageDialogResult.No };
        public static readonly MessageDialogResult[] ButtonsYesNoCancel = new MessageDialogResult[] { MessageDialogResult.Yes, MessageDialogResult.No, MessageDialogResult.Cancel };
        private Bevel bvlButtons;
        private CheckBox checkBox;
        private ContextMenuStrip cmsLabel;
        private IContainer components = null;
        private Label label;
        private PictureBox pictureBox;
        private TableLayoutPanel tlpBack;
        private TableLayoutPanel tlpButtons;
        private ToolStripMenuItem tsmiCopy;

        public MessageDialog()
        {
            this.InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            this.Buttons = new Button[] { this.button1, this.button2, this.button3, this.button4 };
        }

        private void button_Click(object sender, EventArgs e)
        {
            base.Tag = ((Button) sender).Tag;
            base.DialogResult = DialogResult.OK;
        }

        public static DialogResult ConvertDialogResult(MessageDialogResult button)
        {
            switch (button)
            {
                case MessageDialogResult.OK:
                    return DialogResult.OK;

                case MessageDialogResult.Cancel:
                    return DialogResult.Cancel;

                case MessageDialogResult.Yes:
                    return DialogResult.Yes;

                case MessageDialogResult.No:
                    return DialogResult.No;

                case MessageDialogResult.Abort:
                    return DialogResult.Abort;

                case MessageDialogResult.Retry:
                    return DialogResult.Retry;

                case MessageDialogResult.Ignore:
                    return DialogResult.Ignore;
            }
            throw new InvalidEnumArgumentException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private static MessageDialogResult FindDefaultButton(MessageDialogResult[] buttons)
        {
            foreach (MessageDialogResult result in buttons)
            {
                switch (result)
                {
                    case MessageDialogResult.OK:
                    case MessageDialogResult.Yes:
                        return result;

                    case MessageDialogResult.Skip:
                        return result;

                    case MessageDialogResult.Retry:
                        return result;
                }
            }
            return MessageDialogResult.OK;
        }

        public static Icon FromMessageBoxIcon(MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Exclamation:
                    return SystemIcons.Warning;

                case MessageBoxIcon.Asterisk:
                    return SystemIcons.Information;

                case MessageBoxIcon.Hand:
                    return SystemIcons.Error;

                case MessageBoxIcon.Question:
                    return SystemIcons.Question;
            }
            throw new InvalidEnumArgumentException();
        }

        public static string GetMessageButtonText(MessageDialogResult button)
        {
            return GetMessageButtonText(button, null);
        }

        public static string GetMessageButtonText(MessageDialogResult button, IDictionary<MessageDialogResult, string> buttonTextMap)
        {
            string str;
            if ((buttonTextMap != null) && buttonTextMap.TryGetValue(button, out str))
            {
                return str;
            }
            return Resources.ResourceManager.GetString("sMessageButton" + button.ToString());
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize = base.GetPreferredSize(proposedSize);
            int width = this.tlpButtons.GetPreferredSize(proposedSize).Width;
            if (this.tlpButtons.Width < width)
            {
                preferredSize.Width += width - this.tlpButtons.Width;
            }
            return preferredSize;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.bvlButtons = new Bevel();
            this.button1 = new Button();
            this.button2 = new Button();
            this.button4 = new Button();
            this.button3 = new Button();
            this.pictureBox = new PictureBox();
            this.label = new Label();
            this.cmsLabel = new ContextMenuStrip(this.components);
            this.tsmiCopy = new ToolStripMenuItem();
            this.checkBox = new CheckBox();
            this.tlpBack = new TableLayoutPanel();
            this.tlpButtons = new TableLayoutPanel();
            ((ISupportInitialize) this.pictureBox).BeginInit();
            this.cmsLabel.SuspendLayout();
            this.tlpBack.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            base.SuspendLayout();
            this.bvlButtons.Dock = DockStyle.Top;
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Location = new Point(0, 0x43);
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Top;
            this.bvlButtons.Size = new Size(0x17a, 1);
            this.bvlButtons.Style = Border3DStyle.Flat;
            this.bvlButtons.TabIndex = 1;
            this.button1.AutoSize = true;
            this.button1.FlatStyle = FlatStyle.System;
            this.button1.Location = new Point(0x33, 7);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new EventHandler(this.button_Click);
            this.button2.AutoSize = true;
            this.button2.FlatStyle = FlatStyle.System;
            this.button2.Location = new Point(0x84, 7);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x4b, 0x17);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new EventHandler(this.button_Click);
            this.button4.AutoSize = true;
            this.button4.FlatStyle = FlatStyle.System;
            this.button4.Location = new Point(0x126, 7);
            this.button4.Name = "button4";
            this.button4.Size = new Size(0x4b, 0x17);
            this.button4.TabIndex = 3;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new EventHandler(this.button_Click);
            this.button3.AutoSize = true;
            this.button3.FlatStyle = FlatStyle.System;
            this.button3.Location = new Point(0xd5, 7);
            this.button3.Name = "button3";
            this.button3.Size = new Size(0x4b, 0x17);
            this.button3.TabIndex = 2;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new EventHandler(this.button_Click);
            this.pictureBox.Location = new Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new Size(0x20, 0x20);
            this.pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.label.AutoSize = true;
            this.label.ContextMenuStrip = this.cmsLabel;
            this.label.Dock = DockStyle.Top;
            this.label.Location = new Point(50, 9);
            this.label.Margin = new Padding(3, 0, 3, 6);
            this.label.MinimumSize = new Size(0, 0x30);
            this.label.Name = "label";
            this.label.Size = new Size(0x13c, 0x30);
            this.label.TabIndex = 0;
            this.label.Text = "label";
            this.label.UseMnemonic = false;
            this.cmsLabel.Items.AddRange(new ToolStripItem[] { this.tsmiCopy });
            this.cmsLabel.Name = "cmsLabel";
            this.cmsLabel.Size = new Size(0x91, 0x1a);
            this.tsmiCopy.Name = "tsmiCopy";
            this.tsmiCopy.ShortcutKeys = Keys.Control | Keys.C;
            this.tsmiCopy.Size = new Size(0x90, 0x16);
            this.tsmiCopy.Text = "&Copy";
            this.tsmiCopy.Click += new EventHandler(this.tsmiCopy_Click);
            this.tlpButtons.SetColumnSpan(this.checkBox, 2);
            this.checkBox.Dock = DockStyle.Left;
            this.checkBox.FlatStyle = FlatStyle.System;
            this.checkBox.Location = new Point(9, 7);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new Size(0x23, 0x17);
            this.checkBox.TabIndex = 4;
            this.checkBox.Text = "checkBox";
            this.checkBox.UseCompatibleTextRendering = true;
            this.checkBox.UseVisualStyleBackColor = true;
            this.tlpBack.AutoSize = true;
            this.tlpBack.ColumnCount = 2;
            this.tlpBack.ColumnStyles.Add(new ColumnStyle());
            this.tlpBack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tlpBack.Controls.Add(this.label, 1, 0);
            this.tlpBack.Controls.Add(this.pictureBox, 0, 0);
            this.tlpBack.Dock = DockStyle.Top;
            this.tlpBack.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpBack.Location = new Point(0, 0);
            this.tlpBack.Name = "tlpBack";
            this.tlpBack.Padding = new Padding(9, 9, 9, 4);
            this.tlpBack.RowCount = 1;
            this.tlpBack.RowStyles.Add(new RowStyle());
            this.tlpBack.RowStyles.Add(new RowStyle(SizeType.Absolute, 54f));
            this.tlpBack.Size = new Size(0x17a, 0x43);
            this.tlpBack.TabIndex = 0;
            this.tlpButtons.AutoSize = true;
            this.tlpButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.tlpButtons.BackColor = Color.Gainsboro;
            this.tlpButtons.ColumnCount = 6;
            this.tlpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 41f));
            this.tlpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tlpButtons.ColumnStyles.Add(new ColumnStyle());
            this.tlpButtons.ColumnStyles.Add(new ColumnStyle());
            this.tlpButtons.ColumnStyles.Add(new ColumnStyle());
            this.tlpButtons.ColumnStyles.Add(new ColumnStyle());
            this.tlpButtons.Controls.Add(this.button1, 2, 0);
            this.tlpButtons.Controls.Add(this.button2, 3, 0);
            this.tlpButtons.Controls.Add(this.button3, 4, 0);
            this.tlpButtons.Controls.Add(this.button4, 5, 0);
            this.tlpButtons.Controls.Add(this.checkBox, 0, 0);
            this.tlpButtons.Dock = DockStyle.Top;
            this.tlpButtons.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpButtons.Location = new Point(0, 0x44);
            this.tlpButtons.Name = "tlpButtons";
            this.tlpButtons.Padding = new Padding(6, 4, 6, 4);
            this.tlpButtons.RowCount = 2;
            this.tlpButtons.RowStyles.Add(new RowStyle());
            this.tlpButtons.RowStyles.Add(new RowStyle());
            this.tlpButtons.Size = new Size(0x17a, 0x25);
            this.tlpButtons.TabIndex = 2;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            base.ClientSize = new Size(0x17a, 0x69);
            base.Controls.Add(this.tlpButtons);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(this.tlpBack);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x180, 0);
            base.Name = "MessageDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "MessageDialog";
            ((ISupportInitialize) this.pictureBox).EndInit();
            this.cmsLabel.ResumeLayout(false);
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            this.tlpButtons.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected static MessageDialogResult IntenalShow(IWin32Window owner, string text, string caption, string checkBox, ref bool checkBoxChecked, MessageDialogResult[] buttons, IDictionary<MessageDialogResult, string> buttonTextMap, Icon icon, MessageDialogResult defaultButton)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            if (caption == null)
            {
                throw new ArgumentNullException("caption");
            }
            if (buttons == null)
            {
                throw new ArgumentNullException("buttons");
            }
            if ((buttons.Length > 4) || (buttons.Length == 0))
            {
                throw new ArgumentOutOfRangeException("buttons");
            }
            using (MessageDialog dialog = new MessageDialog())
            {
                DialogResult result;
                Form form = owner as Form;
                if (form != null)
                {
                    form.AddOwnedForm(dialog);
                    if (form.WindowState == FormWindowState.Minimized)
                    {
                        dialog.StartPosition = FormStartPosition.CenterScreen;
                    }
                }
                dialog.SuspendLayout();
                dialog.tlpBack.SuspendLayout();
                dialog.Text = caption;
                dialog.label.Text = text;
                if (icon != null)
                {
                    dialog.pictureBox.Image = icon.ToBitmap();
                }
                else
                {
                    dialog.pictureBox.Visible = false;
                }
                dialog.tlpBack.ResumeLayout();
                dialog.tlpButtons.SuspendLayout();
                if (string.IsNullOrEmpty(checkBox))
                {
                    dialog.checkBox.Visible = false;
                }
                else
                {
                    dialog.checkBox.Text = checkBox;
                    dialog.checkBox.Checked = checkBoxChecked;
                }
                int index = 3;
                for (int i = buttons.Length - 1; i >= 0; i--)
                {
                    Button btn = dialog.Buttons[index];
                    btn.Text = GetMessageButtonText(buttons[i], buttonTextMap);
                    btn.Visible = true;
                    btn.Tag = buttons[i];
                    if ((buttons[i] == MessageDialogResult.Shield) && OS.IsWinVista)
                    {
                        btn.Padding = new Padding(8, 0, 8, 0);
                        btn.SetElevationRequiredState(true);
                    }
                    if (buttons[i] == defaultButton)
                    {
                        dialog.AcceptButton = btn;
                        dialog.ActiveControl = btn;
                    }
                    switch (buttons[i])
                    {
                        case MessageDialogResult.Cancel:
                            dialog.CancelButton = btn;
                            break;

                        case MessageDialogResult.No:
                        case MessageDialogResult.Abort:
                            if (dialog.CancelButton == null)
                            {
                                dialog.CancelButton = btn;
                            }
                            break;
                    }
                    index--;
                }
                if ((((dialog.CancelButton == null) && (dialog.AcceptButton != null)) && (buttons.Length == 1)) && (buttons[0] == MessageDialogResult.OK))
                {
                    dialog.CancelButton = dialog.AcceptButton;
                }
                dialog.tlpButtons.ResumeLayout();
                if (!string.IsNullOrEmpty(checkBox))
                {
                    int[] columnWidths = dialog.tlpButtons.GetColumnWidths();
                    if ((dialog.checkBox.PreferredSize.Width + dialog.checkBox.Margin.Horizontal) >= (columnWidths[0] + columnWidths[1]))
                    {
                        dialog.tlpButtons.SetRow(dialog.checkBox, 1);
                        dialog.tlpButtons.SetColumnSpan(dialog.checkBox, 5);
                    }
                    dialog.checkBox.AutoSize = true;
                }
                dialog.ResumeLayout();
                if (dialog.AcceptButton == null)
                {
                    throw new ArgumentOutOfRangeException("defaultButton");
                }
                if (owner != null)
                {
                    result = dialog.ShowDialog(owner);
                }
                else
                {
                    Form activeForm = Form.ActiveForm;
                    if (activeForm != null)
                    {
                        result = dialog.ShowDialog(activeForm);
                    }
                    else
                    {
                        dialog.ShowInTaskbar = true;
                        dialog.StartPosition = FormStartPosition.CenterScreen;
                        result = dialog.ShowDialog();
                    }
                }
                if (checkBox != null)
                {
                    checkBoxChecked = dialog.checkBox.Checked;
                }
                return ((result == DialogResult.OK) ? ((MessageDialogResult) dialog.Tag) : MessageDialogResult.Cancel);
            }
        }

        public static MessageDialogResult Show(IWin32Window owner, string text, string caption, MessageDialogResult[] buttons)
        {
            return Show(owner, text, caption, buttons, (Icon) null, FindDefaultButton(buttons));
        }

        public static MessageDialogResult Show(IWin32Window owner, string text, string caption, MessageDialogResult[] buttons, MessageDialogResult defaultButton)
        {
            return Show(owner, text, caption, buttons, (Icon) null, defaultButton);
        }

        public static MessageDialogResult Show(IWin32Window owner, string text, string caption, MessageDialogResult[] buttons, MessageBoxIcon icon)
        {
            return Show(owner, text, caption, buttons, FromMessageBoxIcon(icon), FindDefaultButton(buttons));
        }

        public static MessageDialogResult Show(IWin32Window owner, string text, string caption, MessageDialogResult[] buttons, Icon icon, MessageDialogResult defaultButton)
        {
            bool checkBoxChecked = false;
            return IntenalShow(owner, text, caption, null, ref checkBoxChecked, buttons, null, icon, defaultButton);
        }

        public static MessageDialogResult Show(IWin32Window owner, string text, string caption, MessageDialogResult[] buttons, MessageBoxIcon icon, MessageDialogResult defaultButton)
        {
            return Show(owner, text, caption, buttons, FromMessageBoxIcon(icon), defaultButton);
        }

        public static MessageDialogResult Show(IWin32Window owner, string text, string caption, MessageDialogResult[] buttons, IDictionary<MessageDialogResult, string> buttonTextMap, Icon icon, MessageDialogResult defaultButton)
        {
            bool checkBoxChecked = false;
            return IntenalShow(owner, text, caption, null, ref checkBoxChecked, buttons, buttonTextMap, icon, defaultButton);
        }

        public static MessageDialogResult Show(IWin32Window owner, string text, string caption, MessageDialogResult[] buttons, IDictionary<MessageDialogResult, string> buttonTextMap, MessageBoxIcon icon, MessageDialogResult defaultButton)
        {
            bool checkBoxChecked = false;
            return IntenalShow(owner, text, caption, null, ref checkBoxChecked, buttons, buttonTextMap, FromMessageBoxIcon(icon), defaultButton);
        }

        public static MessageDialogResult Show(IWin32Window owner, string text, string caption, string checkBox, ref bool checkBoxChecked, MessageDialogResult[] buttons, Icon icon)
        {
            return Show(owner, text, caption, checkBox, ref checkBoxChecked, buttons, icon, FindDefaultButton(buttons));
        }

        public static MessageDialogResult Show(IWin32Window owner, string text, string caption, string checkBox, ref bool checkBoxChecked, MessageDialogResult[] buttons, MessageBoxIcon icon)
        {
            return Show(owner, text, caption, checkBox, ref checkBoxChecked, buttons, FromMessageBoxIcon(icon), FindDefaultButton(buttons));
        }

        public static MessageDialogResult Show(IWin32Window owner, string text, string caption, string checkBox, ref bool checkBoxChecked, MessageDialogResult[] buttons, Icon icon, MessageDialogResult defaultButton)
        {
            if (checkBox == null)
            {
                throw new ArgumentNullException("checkBox");
            }
            return IntenalShow(owner, text, caption, checkBox, ref checkBoxChecked, buttons, null, icon, defaultButton);
        }

        public static MessageDialogResult Show(IWin32Window owner, string text, string caption, string checkBox, ref bool checkBoxChecked, MessageDialogResult[] buttons, MessageBoxIcon icon, MessageDialogResult defaultButton)
        {
            return Show(owner, text, caption, checkBox, ref checkBoxChecked, buttons, FromMessageBoxIcon(icon), defaultButton);
        }

        public static MessageDialogResult Show(IWin32Window owner, string text, string caption, string checkBox, ref bool checkBoxChecked, MessageDialogResult[] buttons, IDictionary<MessageDialogResult, string> buttonTextMap, Icon icon, MessageDialogResult defaultButton)
        {
            if (checkBox == null)
            {
                throw new ArgumentNullException("checkBox");
            }
            return IntenalShow(owner, text, caption, checkBox, ref checkBoxChecked, buttons, buttonTextMap, icon, defaultButton);
        }

        public static MessageDialogResult Show(IWin32Window owner, string text, string caption, string checkBox, ref bool checkBoxChecked, MessageDialogResult[] buttons, IDictionary<MessageDialogResult, string> buttonTextMap, MessageBoxIcon icon, MessageDialogResult defaultButton)
        {
            if (checkBox == null)
            {
                throw new ArgumentNullException("checkBox");
            }
            return IntenalShow(owner, text, caption, checkBox, ref checkBoxChecked, buttons, buttonTextMap, FromMessageBoxIcon(icon), defaultButton);
        }

        public static void ShowException(IWin32Window owner, Exception error)
        {
            ShowException(owner, error, error is WarningException);
        }

        public static void ShowException(IWin32Window owner, Exception error, bool isWarning)
        {
            ShowException(owner, error, isWarning ? Resources.sWarning : Resources.sError, isWarning);
        }

        public static void ShowException(IWin32Window owner, Exception error, string caption)
        {
            ShowException(owner, error, caption, error is WarningException);
        }

        public static void ShowException(IWin32Window owner, Exception error, string caption, bool isWarning)
        {
            Nomad.Trace.Error.TraceException(isWarning ? TraceEventType.Warning : TraceEventType.Error, error);
            Nomad.Trace.Error.Flush();
            Show(owner, error.Message, caption, ButtonsOk, isWarning ? MessageBoxIcon.Exclamation : MessageBoxIcon.Hand);
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.label.Text);
        }
    }
}

