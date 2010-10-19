namespace Nomad.Dialogs
{
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Resources;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Properties;
    using Nomad.Themes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;

    public class OptionsDialog : BasicDialog
    {
        private Font BlockLabelFont;
        private Button btnCancel;
        private Button btnOk;
        private Bevel bvlButtons;
        private IContainer components = null;
        private FlowLayoutPanel flpSection;
        private ToolStripItem InitialSectionItem;
        private Label lblSectionTitle;
        private Label lblWidthController;
        private PanelEx pnlSectionBack;
        private GradientPanel pnlSectionTitle;
        private Nomad.Commons.Controls.SizeGripProvider SizeGripProvider;
        private TableLayoutPanel tlpButtons;
        private ToolStrip tsNavigator;

        public OptionsDialog()
        {
            this.InitializeComponent();
            this.SizeGripProvider = new Nomad.Commons.Controls.SizeGripProvider(this.tlpButtons);
            this.components = new Container();
            this.InitializeColors();
            base.SuspendLayout();
            bool flag = SettingsManager.CheckSafeMode(SafeMode.SkipFormPlacement) || (Control.ModifierKeys == Keys.Shift);
            FormSettings.RegisterForm(this, flag ? FormPlacement.None : (FormPlacement.Size | FormPlacement.Location));
            OptionsSectionGroup sectionGroup = (OptionsSectionGroup) ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSectionGroup("optionsDialog");
            foreach (OptionsSection section in sectionGroup.OrderedSectionList)
            {
                string sectionCaption = Resources.ResourceManager.GetString(section.SectionCaption);
                if (string.IsNullOrEmpty(sectionCaption))
                {
                    sectionCaption = section.SectionCaption;
                }
                string sectionDescription = Resources.ResourceManager.GetString(section.SectionDescription);
                if (string.IsNullOrEmpty(sectionDescription))
                {
                    sectionDescription = section.SectionDescription;
                }
                ToolStripItem item = this.AddSectionButton(sectionCaption, sectionDescription, IconSet.GetImage(section.SectionImage));
                if (this.InitialSectionItem == null)
                {
                    Control[] controlArray = this.CreateSectionBlocks(section.SectionBlocks);
                    foreach (Control control in controlArray)
                    {
                        this.components.Add(control);
                    }
                    this.InitialSectionItem = item;
                    this.InitialSectionItem.Tag = controlArray;
                }
                else
                {
                    item.Tag = section.SectionBlocks;
                }
            }
            base.ResumeLayout();
        }

        public ToolStripItem AddSectionButton(string sectionText, string sectionDescription, Image image)
        {
            ToolStripItem item = new ToolStripButton(sectionText) {
                ToolTipText = sectionDescription,
                Image = image,
                TextAlign = ContentAlignment.MiddleLeft,
                ImageAlign = ContentAlignment.MiddleLeft,
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Padding = new Padding(0, 3, 0, 3)
            };
            item.Click += new EventHandler(this.SectionButton_Click);
            this.tsNavigator.Items.Add(item);
            return item;
        }

        private void BlockLabel_Paint(object sender, PaintEventArgs e)
        {
            Label label = (Label) sender;
            using (Pen pen = new Pen(Theme.Current.ThemeColors.OptionBlockLabelBorder))
            {
                e.Graphics.DrawLine(pen, 0, label.ClientRectangle.Bottom - 1, label.ClientRectangle.Right, label.ClientRectangle.Bottom - 1);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            foreach (ToolStripItem item in this.tsNavigator.Items)
            {
                Control[] tag = item.Tag as Control[];
                foreach (Control control in tag.AsEnumerable<Control>())
                {
                    IPersistComponentSettings settings = control as IPersistComponentSettings;
                    if ((Convert.ToBoolean(control.Tag) && (settings != null)) && settings.SaveSettings)
                    {
                        settings.SaveComponentSettings();
                    }
                }
            }
        }

        private Label CreateBlockLabel(string caption)
        {
            Label component = new Label {
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Theme.Current.ThemeColors.OptionBlockLabelBackground,
                Text = caption,
                Margin = new Padding(12, 3, 12, 3),
                Font = this.BlockLabelFont,
                ForeColor = Theme.Current.ThemeColors.OptionBlockLabelText
            };
            component.Paint += new PaintEventHandler(this.BlockLabel_Paint);
            this.components.Add(component);
            return component;
        }

        private Control[] CreateSectionBlocks(SectionBlockCollection collection)
        {
            BasicFormLocalizer argument = SettingsManager.GetArgument<BasicFormLocalizer>(ArgumentKey.FormLocalizer);
            List<Control> list = new List<Control>();
            foreach (SectionBlockElement element in collection)
            {
                Control parent = (Control) Activator.CreateInstance(System.Type.GetType(element.BlockType));
                UserControl userControl = parent as UserControl;
                if ((userControl != null) && (argument != null))
                {
                    argument.Localize(userControl);
                }
                if (base.FixMouseWheel)
                {
                    base.FixChildrenMouseWheel(parent);
                }
                string blockCaption = Resources.ResourceManager.GetString(element.BlockCaption);
                if (string.IsNullOrEmpty(blockCaption))
                {
                    blockCaption = element.BlockCaption;
                }
                list.Add(SetControlText(parent, blockCaption, element.Dock));
            }
            return list.ToArray();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void flpSection_ClientSizeChanged(object sender, EventArgs e)
        {
            Control SenderControl = (Control) sender;
            if (base.IsHandleCreated)
            {
                base.BeginInvoke(delegate {
                    this.lblWidthController.Width = SenderControl.ClientSize.Width;
                });
            }
            else
            {
                this.lblWidthController.Width = SenderControl.ClientSize.Width;
            }
        }

        private void InitializeColors()
        {
            this.tsNavigator.Renderer = SimpleBorderToolStipRenderer.Create(ToolStripManager.RenderMode, ToolStripManager.Renderer, Theme.Current.ThemeColors.WindowBorder);
            this.tsNavigator.BackColor = Theme.Current.ThemeColors.OptionNavigatorBackground;
            this.tsNavigator.ForeColor = Theme.Current.ThemeColors.OptionNavigatorText;
            this.pnlSectionBack.BackColor = Theme.Current.ThemeColors.OptionSectionBackground;
            this.flpSection.BackColor = Theme.Current.ThemeColors.OptionSectionBackground;
            this.pnlSectionTitle.StartColor = Theme.Current.ThemeColors.OptionSectionGradientBegin;
            this.lblSectionTitle.ForeColor = Theme.Current.ThemeColors.OptionSectionLabelText;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(OptionsDialog));
            this.tsNavigator = new ToolStrip();
            this.pnlSectionBack = new PanelEx();
            this.flpSection = new FlowLayoutPanel();
            this.pnlSectionTitle = new GradientPanel();
            this.lblSectionTitle = new Label();
            this.lblWidthController = new Label();
            this.tlpButtons = new TableLayoutPanel();
            this.btnCancel = new Button();
            this.btnOk = new Button();
            this.bvlButtons = new Bevel();
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.SuspendLayout();
            this.pnlSectionBack.SuspendLayout();
            this.flpSection.SuspendLayout();
            this.pnlSectionTitle.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.tsNavigator, 0, 0);
            panel.Controls.Add(this.pnlSectionBack, 1, 0);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.tsNavigator, "tsNavigator");
            this.tsNavigator.BackColor = SystemColors.Window;
            this.tsNavigator.CanOverflow = false;
            this.tsNavigator.GripStyle = ToolStripGripStyle.Hidden;
            this.tsNavigator.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.tsNavigator.Name = "tsNavigator";
            this.tsNavigator.TabStop = true;
            this.pnlSectionBack.BackColor = SystemColors.Window;
            this.pnlSectionBack.BorderColor = Color.FromArgb(0xa7, 0xa6, 170);
            this.pnlSectionBack.Controls.Add(this.flpSection);
            manager.ApplyResources(this.pnlSectionBack, "pnlSectionBack");
            this.pnlSectionBack.Name = "pnlSectionBack";
            manager.ApplyResources(this.flpSection, "flpSection");
            this.flpSection.Controls.Add(this.pnlSectionTitle);
            this.flpSection.Controls.Add(this.lblWidthController);
            this.flpSection.Name = "flpSection";
            this.flpSection.ClientSizeChanged += new EventHandler(this.flpSection_ClientSizeChanged);
            this.pnlSectionTitle.Controls.Add(this.lblSectionTitle);
            manager.ApplyResources(this.pnlSectionTitle, "pnlSectionTitle");
            this.pnlSectionTitle.Name = "pnlSectionTitle";
            this.pnlSectionTitle.StartColor = Color.LightSkyBlue;
            this.lblSectionTitle.BackColor = Color.Transparent;
            manager.ApplyResources(this.lblSectionTitle, "lblSectionTitle");
            this.lblSectionTitle.ForeColor = Color.Navy;
            this.lblSectionTitle.Name = "lblSectionTitle";
            manager.ApplyResources(this.lblWidthController, "lblWidthController");
            this.lblWidthController.Name = "lblWidthController";
            manager.ApplyResources(this.tlpButtons, "tlpButtons");
            this.tlpButtons.Controls.Add(this.btnCancel, 2, 0);
            this.tlpButtons.Controls.Add(this.btnOk, 1, 0);
            this.tlpButtons.Name = "tlpButtons";
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOk.DialogResult = DialogResult.OK;
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Bottom;
            this.bvlButtons.Style = Border3DStyle.Flat;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(panel);
            base.Controls.Add(this.tlpButtons);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "OptionsDialog";
            base.ShowInTaskbar = false;
            base.ResizeBegin += new EventHandler(this.OptionsDialog_ResizeBegin);
            base.Shown += new EventHandler(this.OptionsDialog_Shown);
            base.ResizeEnd += new EventHandler(this.OptionsDialog_ResizeEnd);
            panel.ResumeLayout(false);
            this.pnlSectionBack.ResumeLayout(false);
            this.flpSection.ResumeLayout(false);
            this.pnlSectionTitle.ResumeLayout(false);
            this.tlpButtons.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeOptionBlock(Control block)
        {
            if (!Convert.ToBoolean(block.Tag))
            {
                IPersistComponentSettings settings = block as IPersistComponentSettings;
                if (settings != null)
                {
                    settings.LoadComponentSettings();
                }
                if (SettingsManager.GetArgument<bool>(ArgumentKey.Debug) && (block.Dock == DockStyle.Fill))
                {
                    BasicForm.CheckForDuplicateMnemonics(block);
                }
                this.InitializeInfoTips(block);
                block.Tag = true;
            }
        }

        protected override bool MouseWheelFixNeeded(Control ctrl)
        {
            return ((ctrl == this.flpSection) || base.MouseWheelFixNeeded(ctrl));
        }

        protected override void OnLoad(EventArgs e)
        {
            float size = this.Font.Size;
            base.OnLoad(e);
            base.DataBindings.RemoveAt(0);
            this.lblSectionTitle.Font = new Font(this.lblSectionTitle.Font.Name, (this.lblSectionTitle.Font.Size * this.Font.Size) / size);
            this.tsNavigator.Font = this.Font;
            this.BlockLabelFont = new Font(this.Font, FontStyle.Bold);
            base.Disposed += delegate (object a, EventArgs b) {
                this.BlockLabelFont.Dispose();
            };
            if (this.InitialSectionItem != null)
            {
                this.InitialSectionItem.PerformClick();
                this.InitialSectionItem = null;
            }
        }

        private void OptionsDialog_ResizeBegin(object sender, EventArgs e)
        {
        }

        private void OptionsDialog_ResizeEnd(object sender, EventArgs e)
        {
            this.flpSection.PerformLayout();
        }

        private void OptionsDialog_Shown(object sender, EventArgs e)
        {
            this.pnlSectionBack.SelectNextControl(null, true, true, true, false);
        }

        private void SectionButton_Click(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton) sender;
            if (!button.Checked)
            {
                foreach (ToolStripButton button2 in this.tsNavigator.Items)
                {
                    button2.Checked = false;
                }
                button.Checked = true;
                try
                {
                    Control[] controlArray;
                    SectionBlockCollection tag = button.Tag as SectionBlockCollection;
                    if (tag != null)
                    {
                        WaitCursor.Show(this.tsNavigator);
                        controlArray = this.CreateSectionBlocks(tag);
                        foreach (Control control in controlArray)
                        {
                            this.components.Add(control);
                        }
                        button.Tag = controlArray;
                    }
                    else
                    {
                        controlArray = (Control[]) button.Tag;
                    }
                    using (new LockWindowRedraw(this.pnlSectionBack, true))
                    {
                        this.flpSection.SuspendLayout();
                        this.pnlSectionBack.SuspendLayout();
                        this.lblSectionTitle.Text = button.ToolTipText;
                        if ((controlArray.Length == 1) && (controlArray[0].Dock == DockStyle.Fill))
                        {
                            this.InitializeOptionBlock(controlArray[0]);
                            this.flpSection.Controls.Clear();
                            this.pnlSectionBack.Controls.Clear();
                            controlArray[0].Padding = new Padding(12, 1, 12, 12);
                            this.pnlSectionBack.Controls.Add(controlArray[0]);
                            this.pnlSectionBack.Controls.Add(this.pnlSectionTitle);
                        }
                        else
                        {
                            if (this.flpSection.Parent == null)
                            {
                                this.pnlSectionBack.Controls.Clear();
                                this.pnlSectionBack.Controls.Add(this.flpSection);
                                this.flpSection.Controls.Add(this.pnlSectionTitle);
                                this.flpSection.Controls.Add(this.lblWidthController);
                            }
                            else
                            {
                                for (int i = this.flpSection.Controls.Count - 1; i > 1; i--)
                                {
                                    this.flpSection.Controls.RemoveAt(i);
                                }
                            }
                            foreach (Control control2 in controlArray)
                            {
                                if (!string.IsNullOrEmpty(control2.Text))
                                {
                                    this.flpSection.Controls.Add(this.CreateBlockLabel(control2.Text));
                                }
                                this.InitializeOptionBlock(control2);
                                control2.Dock = DockStyle.Top;
                                control2.Margin = new Padding(12, 3, 12, 3);
                                control2.Padding = new Padding(0);
                                this.flpSection.Controls.Add(control2);
                            }
                            this.flpSection.ScrollControlIntoView(this.pnlSectionTitle);
                        }
                        this.pnlSectionBack.ResumeLayout();
                        this.flpSection.ResumeLayout();
                    }
                }
                finally
                {
                    WaitCursor.Hide(this.tsNavigator);
                }
            }
        }

        private static Control SetControlText(Control control, string text, DockStyle dock)
        {
            control.Text = text;
            control.Dock = dock;
            return control;
        }

        public bool SetInitialSection(System.Type blockType)
        {
            foreach (ToolStripItem item in this.tsNavigator.Items)
            {
                SectionBlockCollection tag = item.Tag as SectionBlockCollection;
                if (tag != null)
                {
                    foreach (SectionBlockElement element in tag)
                    {
                        if (element.BlockType == blockType.FullName)
                        {
                            this.InitialSectionItem = item;
                            return true;
                        }
                    }
                }
                else
                {
                    foreach (Control control in (Control[]) item.Tag)
                    {
                        if (control.GetType() == blockType)
                        {
                            this.InitialSectionItem = item;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}

