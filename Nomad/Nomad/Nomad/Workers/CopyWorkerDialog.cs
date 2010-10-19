namespace Nomad.Workers
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Archive;
    using Nomad.FileSystem.Ftp;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Forms;

    public class CopyWorkerDialog : CustomWorkerDialog
    {
        private VistaProgressBar barFileProgress;
        private Button btnSkipFile;
        private IContainer components = null;
        private string CopyProgressSizeWithTimeStr = Resources.sCopyProgressSizeWithTime;
        private string CopySpeedStr = Resources.sCopySpeed;
        private MessageDialogResult DefaultCopyStreamsAction;
        public List<IOverwriteRule> DefaultOverwriteRules;
        private string FDestFileName;
        private string FFileAction;
        private int FFileProgress;
        private string FOperationName;
        private bool FShowTotal;
        private string FSourceFileName;
        private int FTotalProgress = -1;
        private long FVisibleProcessedSize;
        private Label lblAction;
        private Label lblCopyMode;
        private Label lblElapsedTime;
        private Label lblFileProgress;
        private Label lblFiles;
        private Label lblFromName;
        private Label lblProcessedSize;
        private Label lblRemainingTime;
        private Label lblSpeed;
        private Label lblStarted;
        private Label lblToName;
        private Label lblTotalSize;
        private OverwritePromtDialog OverwriteDialog;
        private int SkipFileCount;
        private bool SkipFileRequested;
        private Queue<CopyProgressSnapshot> SnapshotQueue = new Queue<CopyProgressSnapshot>(11);
        private TableLayoutPanel tblCopyDetails;
        private System.Windows.Forms.Timer tmrUpdateProgress;

        public CopyWorkerDialog()
        {
            this.InitializeComponent();
            base.pnlTotalProgress.Controls.Add(this.btnSkipFile, 2, 2);
            base.LocalizeForm();
            if (!ConfirmationSettings.Default.CopyAlternateDataStreams)
            {
                this.DefaultCopyStreamsAction = MessageDialogResult.Yes;
            }
            this.lblFromName.Text = string.Empty;
            this.lblToName.Text = string.Empty;
            base.SaveSettings = true;
        }

        protected override void AttachEvents()
        {
            base.AttachEvents();
            if (base.FWorker != null)
            {
                this.CopyWorker.FileProgressChanged += new EventHandler<ProgressEventArgs>(this.FileProgressChanged);
                this.CopyWorker.OnBeforeCopyItem += new EventHandler<BeforeCopyItemEventArgs>(this.BeforeCopyItem);
                this.CopyWorker.OnCopyItemError += new EventHandler<CopyItemErrorEventArgs>(this.CopyItemError);
                this.CopyWorker.OnCreateFolderError += new EventHandler<ChangeItemErrorEventArgs>(this.CreateFolderError);
                this.CopyWorker.OnDeleteItemError += new EventHandler<ChangeItemErrorEventArgs>(this.DeleteItemError);
                this.CopyWorker.OnMoveItemError += new EventHandler<ChangeItemErrorEventArgs>(this.MoveItemError);
                if (!((base.FWorker != null) && this.CopyWorker.CheckOption(CopyWorkerOptions.DeleteSource)))
                {
                    this.FOperationName = Resources.sCaptionCopy;
                    this.FFileAction = Resources.sFileActionCopy;
                }
                else
                {
                    this.FOperationName = Resources.sCaptionRenameOrMove;
                    this.FFileAction = Resources.sFileActionMove;
                }
            }
        }

        private void BeforeCopyItem(object sender, BeforeCopyItemEventArgs e)
        {
            TableLayoutPanel panel;
            bool flag = false;
            bool flag2 = false;
            IPersistVirtualItem dest = e.Dest as IPersistVirtualItem;
            bool flag3 = (dest != null) && dest.Exists;
            if (flag3)
            {
                OverwriteDialogResult defaultOverwriteResult = this.GetDefaultOverwriteResult(e.Source, e.Dest);
                if (defaultOverwriteResult != OverwriteDialogResult.None)
                {
                    e.OverwriteResult = defaultOverwriteResult;
                }
                else
                {
                    flag = true;
                }
            }
            IVirtualAlternateStreams source = e.Source as IVirtualAlternateStreams;
            IVirtualAlternateStreams streams2 = e.Dest as IVirtualAlternateStreams;
            bool flag4 = this.DefaultCopyStreamsAction != MessageDialogResult.Yes;
            if (!flag4)
            {
                goto Label_0159;
            }
        Label_008C:;
            try
            {
                flag4 = (((source != null) && source.IsSupported) && source.GetStreamNames().Any<string>(delegate (string x) {
                    return (x != ":Zone.Identifier:$DATA");
                })) && ((streams2 == null) || !streams2.IsSupported);
            }
            catch (UnauthorizedAccessException exception)
            {
                CopyItemErrorEventArgs args = new CopyItemErrorEventArgs(e.Source, e.Source, e.Dest, AvailableItemActions.CanElevate, exception);
                this.CopyItemError(sender, args);
                switch (args.Action)
                {
                    case ChangeItemAction.Retry:
                        goto Label_008C;

                    case ChangeItemAction.Skip:
                        e.OverwriteResult = OverwriteDialogResult.Skip;
                        break;

                    case ChangeItemAction.Cancel:
                        e.OverwriteResult = OverwriteDialogResult.Abort;
                        break;

                    default:
                        throw new InvalidEnumArgumentException();
                }
                flag4 = false;
                flag = false;
            }
        Label_0159:
            if (flag4)
            {
                switch (this.DefaultCopyStreamsAction)
                {
                    case MessageDialogResult.Yes:
                        goto Label_0190;

                    case MessageDialogResult.No:
                        e.OverwriteResult = OverwriteDialogResult.Skip;
                        flag = false;
                        goto Label_0190;
                }
                flag2 = true;
            }
        Label_0190:
            Monitor.Enter(panel = this.tblCopyDetails);
            try
            {
                if (((Nomad.Workers.CopyWorker) sender).CheckOption(CopyWorkerOptions.DeleteSource) && (e.Source is IChangeVirtualItem))
                {
                    if (!(!flag3 && ((IChangeVirtualItem) e.Source).CanMoveTo(e.Dest.Parent)))
                    {
                        this.FFileAction = Resources.sFileActionMove;
                    }
                    else
                    {
                        this.FFileAction = Resources.sFileActionRename;
                    }
                }
                else
                {
                    this.FFileAction = GetCopyAction(e.Source, e.Dest);
                }
                TextFormatFlags formatFlags = TextFormatFlags.PathEllipsis | TextFormatFlags.NoPrefix;
                this.FSourceFileName = StringHelper.CompactString(e.Source.FullName, this.lblFromName.Width, this.lblFromName.Font, formatFlags);
                this.FDestFileName = StringHelper.CompactString(e.Dest.FullName, this.lblToName.Width, this.lblToName.Font, formatFlags);
            }
            finally
            {
                Monitor.Exit(panel);
            }
            this.SkipFileCount = 0;
            this.SkipFileRequested = false;
            this.FFileProgress = 0;
            if (flag)
            {
                if (base.InvokeRequired)
                {
                    base.Invoke(new EventHandler<BeforeCopyItemEventArgs>(this.BeforeCopyOverwritePrompt), new object[] { sender, e });
                }
                else
                {
                    this.BeforeCopyOverwritePrompt(sender, e);
                }
            }
            if (flag2)
            {
                if (base.InvokeRequired)
                {
                    base.Invoke(new EventHandler<BeforeCopyItemEventArgs>(this.BeforeCopyStreamsPrompt), new object[] { sender, e });
                }
                else
                {
                    this.BeforeCopyStreamsPrompt(sender, e);
                }
            }
        }

        private void BeforeCopyOverwritePrompt(object sender, BeforeCopyItemEventArgs e)
        {
            this.ChangeProgressState(ProgressState.Pause);
            this.ShowCopyItem();
            if (this.OverwriteDialog == null)
            {
                this.OverwriteDialog = new OverwritePromtDialog();
            }
            this.OverwriteDialog.Execute(this, e.Source, e.Dest);
            IOverwriteRule overwriteRule = this.OverwriteDialog.OverwriteRule;
            if (overwriteRule != null)
            {
                if (this.DefaultOverwriteRules == null)
                {
                    this.DefaultOverwriteRules = new List<IOverwriteRule>();
                }
                this.DefaultOverwriteRules.Add(overwriteRule);
            }
            e.OverwriteResult = this.OverwriteDialog.OverwriteResult;
            e.NewName = this.OverwriteDialog.NewName;
            if (e.OverwriteResult != OverwriteDialogResult.Abort)
            {
                this.ChangeProgressState(ProgressState.Normal);
            }
        }

        private void BeforeCopyStreamsPrompt(object sender, BeforeCopyItemEventArgs e)
        {
            this.ChangeProgressState(ProgressState.Pause);
            this.ShowCopyItem();
            MessageDialogResult[] buttons = new MessageDialogResult[] { MessageDialogResult.Yes, MessageDialogResult.YesToAll, MessageDialogResult.No, MessageDialogResult.Cancel };
            bool checkBoxChecked = false;
            switch (MessageDialog.Show(this, string.Format(Resources.sWarningCopyAlternateDataStreams, e.Source.FullName), Resources.sCaptionErrorCopingFile, Resources.sDoNotAskAgain, ref checkBoxChecked, buttons, MessageBoxIcon.Exclamation))
            {
                case MessageDialogResult.Yes:
                    if (checkBoxChecked)
                    {
                        this.DefaultCopyStreamsAction = MessageDialogResult.Yes;
                    }
                    break;

                case MessageDialogResult.No:
                    e.OverwriteResult = OverwriteDialogResult.Skip;
                    if ((Control.ModifierKeys == Keys.Shift) || checkBoxChecked)
                    {
                        this.DefaultCopyStreamsAction = MessageDialogResult.No;
                    }
                    break;

                case MessageDialogResult.YesToAll:
                    this.DefaultCopyStreamsAction = MessageDialogResult.Yes;
                    break;

                default:
                    e.OverwriteResult = OverwriteDialogResult.Abort;
                    break;
            }
            if (checkBoxChecked)
            {
                ConfirmationSettings.Default.CopyAlternateDataStreams = false;
            }
            this.ChangeProgressState(ProgressState.Normal);
        }

        private void btnSkipFile_Click(object sender, EventArgs e)
        {
            this.SkipFileRequested = this.SkipFileCount > 10;
            this.btnSkipFile.Enabled = false;
        }

        protected override void ChangeProgressState(ProgressState newState)
        {
            this.barFileProgress.State = newState;
            base.ChangeProgressState(newState);
        }

        private void CopyItemError(object sender, CopyItemErrorEventArgs e)
        {
            ChangeItemAction action;
            if ((base.DefaultErrorAction != null) && base.DefaultErrorAction.TryGetValue(e.Error.GetType(), out action))
            {
                e.Action = action;
            }
            else if (base.TryAutoElevate(e))
            {
                e.Action = ChangeItemAction.Retry;
            }
            else
            {
                base.WorkerTrace.TraceException(TraceEventType.Error, e.Error);
                if (base.InvokeRequired)
                {
                    base.Invoke(new EventHandler<CopyItemErrorEventArgs>(this.SyncronizedCopyItemError), new object[] { sender, e });
                }
                else
                {
                    this.SyncronizedCopyItemError(sender, e);
                }
            }
        }

        private void CopyWorkerDialog_Shown(object sender, EventArgs e)
        {
            this.lblStarted.Text = DateTime.Now.ToString("g");
        }

        private void CreateFolderError(object sender, ChangeItemErrorEventArgs e)
        {
            if (base.TryAutoElevate(e))
            {
                e.Action = ChangeItemAction.Retry;
            }
            else
            {
                base.WorkerTrace.TraceException(TraceEventType.Error, e.Error);
                if (base.InvokeRequired)
                {
                    base.Invoke(new EventHandler<ChangeItemErrorEventArgs>(this.SyncronizedCreateFolderError), new object[] { sender, e });
                }
                else
                {
                    this.SyncronizedCreateFolderError(sender, e);
                }
            }
        }

        private void DeleteItemError(object sender, ChangeItemErrorEventArgs e)
        {
            if (base.TryAutoElevate(e))
            {
                e.Action = ChangeItemAction.Retry;
            }
            else
            {
                base.WorkerTrace.TraceException(TraceEventType.Error, e.Error);
                if (base.InvokeRequired)
                {
                    base.Invoke(new EventHandler<ChangeItemErrorEventArgs>(this.SyncronizedDeleteItemError), new object[] { sender, e });
                }
                else
                {
                    this.SyncronizedDeleteItemError(sender, e);
                }
            }
        }

        protected override void DetachEvents()
        {
            if (base.FWorker != null)
            {
                this.CopyWorker.FileProgressChanged -= new EventHandler<ProgressEventArgs>(this.FileProgressChanged);
                this.CopyWorker.OnBeforeCopyItem -= new EventHandler<BeforeCopyItemEventArgs>(this.BeforeCopyItem);
                this.CopyWorker.OnCopyItemError -= new EventHandler<CopyItemErrorEventArgs>(this.CopyItemError);
                this.CopyWorker.OnCreateFolderError -= new EventHandler<ChangeItemErrorEventArgs>(this.CreateFolderError);
                this.CopyWorker.OnDeleteItemError -= new EventHandler<ChangeItemErrorEventArgs>(this.DeleteItemError);
                this.CopyWorker.OnMoveItemError -= new EventHandler<ChangeItemErrorEventArgs>(this.MoveItemError);
            }
            base.DetachEvents();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FileProgressChanged(object sender, ProgressEventArgs e)
        {
            this.FFileProgress = e.ProgressPercentage;
            e.Cancel = this.SkipFileRequested;
        }

        private static string GetCopyAction(IVirtualItem source, IVirtualItem dest)
        {
            if (source is ArchiveFile)
            {
                return Resources.sFileActionExtract;
            }
            if ((source is FtpFile) && (dest is FileSystemFile))
            {
                return Resources.sFileActionDownload;
            }
            if ((source is FileSystemFile) && (dest is FtpFile))
            {
                return Resources.sFileActionUpload;
            }
            return Resources.sFileActionCopy;
        }

        private OverwriteDialogResult GetDefaultOverwriteResult(IVirtualItem source, IVirtualItem dest)
        {
            if (this.DefaultOverwriteRules != null)
            {
                foreach (IOverwriteRule rule in this.DefaultOverwriteRules)
                {
                    OverwriteDialogResult overwrite = rule.GetOverwrite(source, dest);
                    if (overwrite != OverwriteDialogResult.None)
                    {
                        return overwrite;
                    }
                }
            }
            return OverwriteDialogResult.None;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(CopyWorkerDialog));
            this.lblElapsedTime = new Label();
            this.lblRemainingTime = new Label();
            this.lblCopyMode = new Label();
            this.lblStarted = new Label();
            this.lblFiles = new Label();
            this.lblTotalSize = new Label();
            this.lblProcessedSize = new Label();
            this.lblSpeed = new Label();
            this.lblFileProgress = new Label();
            this.barFileProgress = new VistaProgressBar();
            this.lblToName = new Label();
            this.lblFromName = new Label();
            this.lblAction = new Label();
            this.tmrUpdateProgress = new System.Windows.Forms.Timer(this.components);
            this.tblCopyDetails = new TableLayoutPanel();
            this.btnSkipFile = new Button();
            Label label = new Label();
            Label label2 = new Label();
            Bevel control = new Bevel();
            Bevel bevel2 = new Bevel();
            Label label3 = new Label();
            Label label4 = new Label();
            Label label5 = new Label();
            Label label6 = new Label();
            Label label7 = new Label();
            Label label8 = new Label();
            Label label9 = new Label();
            Label label10 = new Label();
            Panel panel = new Panel();
            Panel panel2 = new Panel();
            Panel panel3 = new Panel();
            Panel panel4 = new Panel();
            Panel panel5 = new Panel();
            Panel panel6 = new Panel();
            Panel panel7 = new Panel();
            Panel panel8 = new Panel();
            panel.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            panel7.SuspendLayout();
            panel8.SuspendLayout();
            this.tblCopyDetails.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(label, "lblProgress2");
            label.Name = "lblProgress2";
            label.UseMnemonic = false;
            manager.ApplyResources(label2, "lblTo");
            label2.Name = "lblTo";
            label2.UseMnemonic = false;
            this.tblCopyDetails.SetColumnSpan(control, 3);
            manager.ApplyResources(control, "bvlSeparator1");
            control.Name = "bvlSeparator1";
            control.Sides = Border3DSide.Top;
            this.tblCopyDetails.SetColumnSpan(bevel2, 3);
            manager.ApplyResources(bevel2, "bvlSeparator2");
            bevel2.Name = "bvlSeparator2";
            bevel2.Sides = Border3DSide.Top;
            manager.ApplyResources(label3, "lblStartedCaption");
            label3.Name = "lblStartedCaption";
            manager.ApplyResources(label4, "lblElapsedCaption");
            label4.Name = "lblElapsedCaption";
            manager.ApplyResources(label5, "lblRemainingCaption");
            label5.Name = "lblRemainingCaption";
            manager.ApplyResources(label6, "lblCopyModeCaption");
            label6.Name = "lblCopyModeCaption";
            manager.ApplyResources(label7, "lblTotalSizeCaption");
            label7.Name = "lblTotalSizeCaption";
            manager.ApplyResources(label8, "lblProcessedSizeCaption");
            label8.Name = "lblProcessedSizeCaption";
            manager.ApplyResources(label9, "lblSpeedCaption");
            label9.Name = "lblSpeedCaption";
            manager.ApplyResources(label10, "lblFilesCaption");
            label10.Name = "lblFilesCaption";
            panel.Controls.Add(this.lblElapsedTime);
            panel.Controls.Add(label4);
            manager.ApplyResources(panel, "pnlElapsedTime");
            panel.Name = "pnlElapsedTime";
            manager.ApplyResources(this.lblElapsedTime, "lblElapsedTime");
            this.lblElapsedTime.Name = "lblElapsedTime";
            panel2.Controls.Add(this.lblRemainingTime);
            panel2.Controls.Add(label5);
            manager.ApplyResources(panel2, "pnlRemainingTime");
            panel2.Name = "pnlRemainingTime";
            manager.ApplyResources(this.lblRemainingTime, "lblRemainingTime");
            this.lblRemainingTime.Name = "lblRemainingTime";
            panel3.Controls.Add(this.lblCopyMode);
            panel3.Controls.Add(label6);
            manager.ApplyResources(panel3, "pnlCopyMode");
            panel3.Name = "pnlCopyMode";
            manager.ApplyResources(this.lblCopyMode, "lblCopyMode");
            this.lblCopyMode.Name = "lblCopyMode";
            this.lblCopyMode.UseMnemonic = false;
            panel4.Controls.Add(this.lblStarted);
            panel4.Controls.Add(label3);
            manager.ApplyResources(panel4, "pnlStarted");
            panel4.Name = "pnlStarted";
            manager.ApplyResources(this.lblStarted, "lblStarted");
            this.lblStarted.Name = "lblStarted";
            manager.ApplyResources(panel5, "pnlFiles");
            panel5.Controls.Add(this.lblFiles);
            panel5.Controls.Add(label10);
            panel5.Name = "pnlFiles";
            manager.ApplyResources(this.lblFiles, "lblFiles");
            this.lblFiles.Name = "lblFiles";
            manager.ApplyResources(panel6, "pnlTotalSize");
            panel6.Controls.Add(this.lblTotalSize);
            panel6.Controls.Add(label7);
            panel6.Name = "pnlTotalSize";
            manager.ApplyResources(this.lblTotalSize, "lblTotalSize");
            this.lblTotalSize.Name = "lblTotalSize";
            manager.ApplyResources(panel7, "pnlProcessedSize");
            panel7.Controls.Add(this.lblProcessedSize);
            panel7.Controls.Add(label8);
            panel7.Name = "pnlProcessedSize";
            manager.ApplyResources(this.lblProcessedSize, "lblProcessedSize");
            this.lblProcessedSize.Name = "lblProcessedSize";
            manager.ApplyResources(panel8, "pnlSpeed");
            panel8.Controls.Add(this.lblSpeed);
            panel8.Controls.Add(label9);
            panel8.Name = "pnlSpeed";
            manager.ApplyResources(this.lblSpeed, "lblSpeed");
            this.lblSpeed.Name = "lblSpeed";
            manager.ApplyResources(this.lblFileProgress, "lblFileProgress");
            this.lblFileProgress.Name = "lblFileProgress";
            this.lblFileProgress.UseMnemonic = false;
            this.tblCopyDetails.SetColumnSpan(this.barFileProgress, 3);
            manager.ApplyResources(this.barFileProgress, "barFileProgress");
            this.barFileProgress.Name = "barFileProgress";
            this.barFileProgress.RenderMode = ProgressRenderMode.Vista;
            this.tblCopyDetails.SetColumnSpan(this.lblToName, 3);
            manager.ApplyResources(this.lblToName, "lblToName");
            this.lblToName.Name = "lblToName";
            this.lblToName.UseMnemonic = false;
            this.tblCopyDetails.SetColumnSpan(this.lblFromName, 3);
            manager.ApplyResources(this.lblFromName, "lblFromName");
            this.lblFromName.Name = "lblFromName";
            this.lblFromName.UseMnemonic = false;
            manager.ApplyResources(this.lblAction, "lblAction");
            this.tblCopyDetails.SetColumnSpan(this.lblAction, 3);
            this.lblAction.Name = "lblAction";
            this.lblAction.UseMnemonic = false;
            this.tmrUpdateProgress.Enabled = true;
            this.tmrUpdateProgress.Interval = 350;
            this.tmrUpdateProgress.Tick += new EventHandler(this.tmrUpdateProgress_Tick);
            manager.ApplyResources(this.tblCopyDetails, "tblCopyDetails");
            this.tblCopyDetails.Controls.Add(bevel2, 0, 9);
            this.tblCopyDetails.Controls.Add(this.barFileProgress, 0, 11);
            this.tblCopyDetails.Controls.Add(this.lblAction, 0, 0);
            this.tblCopyDetails.Controls.Add(label, 0, 10);
            this.tblCopyDetails.Controls.Add(control, 0, 4);
            this.tblCopyDetails.Controls.Add(this.lblFromName, 0, 1);
            this.tblCopyDetails.Controls.Add(label2, 0, 2);
            this.tblCopyDetails.Controls.Add(this.lblToName, 0, 3);
            this.tblCopyDetails.Controls.Add(panel, 0, 6);
            this.tblCopyDetails.Controls.Add(panel2, 0, 7);
            this.tblCopyDetails.Controls.Add(panel3, 0, 8);
            this.tblCopyDetails.Controls.Add(panel4, 0, 5);
            this.tblCopyDetails.Controls.Add(panel5, 2, 5);
            this.tblCopyDetails.Controls.Add(panel6, 2, 6);
            this.tblCopyDetails.Controls.Add(panel7, 2, 7);
            this.tblCopyDetails.Controls.Add(panel8, 2, 8);
            this.tblCopyDetails.Controls.Add(this.lblFileProgress, 2, 10);
            this.tblCopyDetails.Name = "tblCopyDetails";
            this.tblCopyDetails.VisibleChanged += new EventHandler(this.tlbCopyDetails_VisibleChanged);
            manager.ApplyResources(this.btnSkipFile, "btnSkipFile");
            this.btnSkipFile.Name = "btnSkipFile";
            this.btnSkipFile.UseVisualStyleBackColor = true;
            this.btnSkipFile.Click += new EventHandler(this.btnSkipFile_Click);
            manager.ApplyResources(this, "$this");
            base.Controls.Add(this.tblCopyDetails);
            base.Name = "CopyWorkerDialog";
            base.Shown += new EventHandler(this.CopyWorkerDialog_Shown);
            base.Controls.SetChildIndex(this.tblCopyDetails, 0);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            this.tblCopyDetails.ResumeLayout(false);
            this.tblCopyDetails.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void MoveItemError(object sender, ChangeItemErrorEventArgs e)
        {
            if (base.TryAutoElevate(e))
            {
                e.Action = ChangeItemAction.Retry;
            }
            else
            {
                base.WorkerTrace.TraceException(TraceEventType.Error, e.Error);
                if (base.InvokeRequired)
                {
                    base.Invoke(new EventHandler<ChangeItemErrorEventArgs>(this.SyncronizedMoveItemError), new object[] { sender, e });
                }
                else
                {
                    this.SyncronizedMoveItemError(sender, e);
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (this.OverwriteDialog != null)
            {
                this.OverwriteDialog.Dispose();
            }
            this.OverwriteDialog = null;
            base.OnFormClosed(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.Cancel && ((base.FWorker == null) || base.FWorker.CancellationPending))
            {
                this.btnSkipFile.Enabled = false;
            }
        }

        protected override void OnThemeChanged(EventArgs e)
        {
            this.barFileProgress.RenderMode = Application.RenderWithVisualStyles ? ProgressRenderMode.Vista : ProgressRenderMode.System;
            base.OnThemeChanged(e);
        }

        protected override void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!base.Disposing && !base.IsDisposed)
            {
                this.FTotalProgress = e.ProgressPercentage;
                if (this.FTotalProgress == 0)
                {
                    CopyProgressSnapshot userState = (CopyProgressSnapshot) e.UserState;
                    if (!this.tblCopyDetails.Visible)
                    {
                        base.TotalProgressText = string.Format(Resources.sCopyProgressSize, 0, SizeTypeConverter.FormatSize<long>(userState.TotalSize, SizeFormat.Dynamic));
                    }
                }
                this.FShowTotal = true;
                this.lblFiles.Tag = null;
            }
        }

        public static CopyWorkerDialog ShowAsync(Nomad.Workers.CopyWorker worker)
        {
            return (CopyWorkerDialog) CustomWorkerDialog.ShowAsync(typeof(CopyWorkerDialog), worker);
        }

        private void ShowCopyItem()
        {
            lock (this.tblCopyDetails)
            {
                this.lblAction.Text = this.FFileAction;
                this.lblFromName.Text = this.FSourceFileName;
                this.lblToName.Text = this.FDestFileName;
            }
            this.ShowFileProgress(this.FFileProgress);
        }

        private void ShowCopyProgress(bool force)
        {
            if (this.CopyWorker != null)
            {
                int fTotalProgress = this.FTotalProgress;
                CopyProgressSnapshot progressSnapshot = this.CopyWorker.GetProgressSnapshot();
                this.SnapshotQueue.Enqueue(progressSnapshot);
                CopyProgressSnapshot snapshot2 = (this.SnapshotQueue.Count > 10) ? this.SnapshotQueue.Dequeue() : this.SnapshotQueue.Peek();
                TimeSpan span = progressSnapshot.Duration - snapshot2.Duration;
                long num2 = progressSnapshot.ProcessedSize - snapshot2.ProcessedSize;
                double size = (num2 > 0L) ? (((double) num2) / span.TotalSeconds) : 0.0;
                if (this.tblCopyDetails.Visible)
                {
                    this.tblCopyDetails.SuspendLayout();
                    this.ShowCopyItem();
                    if (this.FShowTotal && (this.lblTotalSize.Tag == null))
                    {
                        this.lblTotalSize.Text = SizeTypeConverter.FormatSize<long>(progressSnapshot.TotalSize, SizeFormat.Dynamic);
                        this.lblTotalSize.Tag = true;
                    }
                    if (Convert.ToInt32(this.lblElapsedTime.Tag) != progressSnapshot.Duration.Seconds)
                    {
                        this.lblElapsedTime.Text = progressSnapshot.Duration.ToString().Substring(0, 8);
                        this.lblElapsedTime.Tag = progressSnapshot.Duration.Seconds;
                    }
                    if ((this.lblCopyMode.Tag == null) || (((CopyMode) this.lblCopyMode.Tag) != progressSnapshot.CopyMode))
                    {
                        this.lblCopyMode.Text = TypeDescriptor.GetConverter(typeof(CopyMode)).ConvertToString(progressSnapshot.CopyMode);
                        this.lblCopyMode.Tag = progressSnapshot.CopyMode;
                    }
                    if ((this.lblFiles.Tag == null) || (Convert.ToInt32(this.lblFiles.Tag) != progressSnapshot.TotalProcessedCount))
                    {
                        this.lblFiles.Text = string.Format("{0} / {1} / {2}", progressSnapshot.ProcessedCount, progressSnapshot.SkippedCount, this.FShowTotal ? ((object) progressSnapshot.TotalCount) : ((object) "?"));
                        this.lblFiles.Tag = progressSnapshot.TotalProcessedCount;
                    }
                    if (Convert.ToInt64(this.lblProcessedSize.Tag) != progressSnapshot.ProcessedSize)
                    {
                        this.lblProcessedSize.Text = SizeTypeConverter.FormatSize<long>(progressSnapshot.ProcessedSize, SizeFormat.Dynamic);
                        this.lblProcessedSize.Tag = progressSnapshot.ProcessedSize;
                    }
                    if (span.Seconds >= 1)
                    {
                        this.lblSpeed.Text = string.Format(this.CopySpeedStr, SizeTypeConverter.FormatSize<double>(size, SizeFormat.Dynamic));
                    }
                    else
                    {
                        this.lblSpeed.Text = "?";
                    }
                    if (this.FShowTotal && (span.Seconds >= 2))
                    {
                        if (num2 == 0L)
                        {
                            this.lblRemainingTime.Text = "∞";
                            this.lblRemainingTime.Tag = null;
                        }
                        else
                        {
                            TimeSpan span2 = new TimeSpan(((progressSnapshot.TotalSize - progressSnapshot.ProcessedSize) * span.Ticks) / num2);
                            if (Convert.ToInt32(this.lblRemainingTime.Tag) != span2.Seconds)
                            {
                                this.lblRemainingTime.Text = span2.ToString().Substring(0, 8);
                                this.lblRemainingTime.Tag = span2.Seconds;
                            }
                        }
                    }
                    else
                    {
                        this.lblRemainingTime.Text = "?";
                        this.lblRemainingTime.Tag = null;
                    }
                    this.btnSkipFile.Enabled = (this.SkipFileCount > 10) && !this.SkipFileRequested;
                    this.tblCopyDetails.ResumeLayout();
                    base.TotalProgressText = null;
                }
                else if (this.FShowTotal && ((progressSnapshot.ProcessedSize != this.FVisibleProcessedSize) || force))
                {
                    base.TotalProgressText = string.Format(this.CopyProgressSizeWithTimeStr, SizeTypeConverter.FormatSize<long>(progressSnapshot.ProcessedSize, SizeFormat.Dynamic), SizeTypeConverter.FormatSize<long>(progressSnapshot.TotalSize, SizeFormat.Dynamic), SizeTypeConverter.FormatSize<double>(size, SizeFormat.Dynamic));
                    this.FVisibleProcessedSize = progressSnapshot.ProcessedSize;
                }
                if (this.FShowTotal)
                {
                    base.TotalProgress = fTotalProgress;
                }
            }
        }

        private void ShowFileProgress(int progress)
        {
            if (Convert.ToInt32(this.barFileProgress.Tag) != progress)
            {
                this.barFileProgress.Value = progress;
                this.lblFileProgress.Text = progress.ToString() + "%";
                this.barFileProgress.Tag = progress;
            }
        }

        private void SyncronizedCopyItemError(object sender, CopyItemErrorEventArgs e)
        {
            MessageDialogResult[] buttonsRetryIgnoreSkipCancel;
            this.ChangeProgressState(ProgressState.Error);
            MessageDialogResult retry = MessageDialogResult.Retry;
            if (e.CanRetry && e.CanIgnore)
            {
                buttonsRetryIgnoreSkipCancel = MessageDialog.ButtonsRetryIgnoreSkipCancel;
            }
            else if (e.CanRetry)
            {
                buttonsRetryIgnoreSkipCancel = CustomWorkerDialog.ButtonsRetrySkipCancel;
            }
            else
            {
                buttonsRetryIgnoreSkipCancel = CustomWorkerDialog.ButtonsSkipCancel;
                retry = MessageDialogResult.Skip;
            }
            MessageDialogResult result = base.ProcessUnauthorizedAccessException(e);
            if (result == MessageDialogResult.None)
            {
                MessageBoxIcon exclamation;
                if (e.Error is WarningException)
                {
                    exclamation = MessageBoxIcon.Exclamation;
                }
                else
                {
                    exclamation = MessageBoxIcon.Hand;
                }
                if (e.CanUndoDestination)
                {
                    result = MessageDialog.Show(this, e.Error.Message, Resources.sCaptionErrorCopingFile, Resources.sUndoDestFile, ref e.UndoDest, buttonsRetryIgnoreSkipCancel, exclamation, retry);
                }
                else
                {
                    result = MessageDialog.Show(this, e.Error.Message, Resources.sCaptionErrorCopingFile, buttonsRetryIgnoreSkipCancel, exclamation, retry);
                }
            }
            if ((result == MessageDialogResult.SkipAll) || ((result == MessageDialogResult.Skip) && ((Control.ModifierKeys & Keys.Shift) > Keys.None)))
            {
                if (base.DefaultErrorAction == null)
                {
                    base.DefaultErrorAction = new Dictionary<System.Type, ChangeItemAction>();
                }
                base.DefaultErrorAction.Add(e.Error.GetType(), ChangeItemAction.Skip);
                e.Action = ChangeItemAction.Skip;
            }
            else
            {
                e.FromMessageDialogResult(result);
            }
            if (e.Action != ChangeItemAction.Cancel)
            {
                this.ChangeProgressState(ProgressState.Normal);
            }
        }

        private void SyncronizedCreateFolderError(object sender, ChangeItemErrorEventArgs e)
        {
            this.ChangeProgressState(ProgressState.Error);
            MessageDialogResult result = base.ProcessUnauthorizedAccessException(e);
            if (result == MessageDialogResult.None)
            {
                result = MessageDialog.Show(this, e.Error.Message, Resources.sCaptionErrorCreatingFolder, MessageDialog.ButtonsRetrySkipCancel, MessageBoxIcon.Hand, MessageDialogResult.Retry);
            }
            e.FromMessageDialogResult(result);
            if (e.Action != ChangeItemAction.Cancel)
            {
                this.ChangeProgressState(ProgressState.Normal);
            }
        }

        private void SyncronizedDeleteItemError(object sender, ChangeItemErrorEventArgs e)
        {
            this.ChangeProgressState(ProgressState.Error);
            MessageDialogResult result = base.ProcessUnauthorizedAccessException(e);
            if (result == MessageDialogResult.None)
            {
                result = MessageDialog.Show(this, e.Error.Message, Resources.sCaptionErrorDeletingFile, MessageDialog.ButtonsRetrySkipCancel, MessageBoxIcon.Hand, MessageDialogResult.Retry);
            }
            e.FromMessageDialogResult(result);
            if (e.Action != ChangeItemAction.Cancel)
            {
                this.ChangeProgressState(ProgressState.Normal);
            }
        }

        private void SyncronizedMoveItemError(object sender, ChangeItemErrorEventArgs e)
        {
            this.ChangeProgressState(ProgressState.Error);
            MessageDialogResult result = base.ProcessUnauthorizedAccessException(e);
            if (result == MessageDialogResult.None)
            {
                result = MessageDialog.Show(this, e.Error.Message, Resources.sCaptionErrorMovingFile, MessageDialog.ButtonsRetrySkipCancel, MessageBoxIcon.Hand, MessageDialogResult.Retry);
            }
            e.FromMessageDialogResult(result);
            if (e.Action != ChangeItemAction.Cancel)
            {
                this.ChangeProgressState(ProgressState.Normal);
            }
        }

        private void tlbCopyDetails_VisibleChanged(object sender, EventArgs e)
        {
            this.btnSkipFile.Visible = ((Control) sender).Visible;
            this.ShowCopyProgress(true);
        }

        private void tmrUpdateProgress_Tick(object sender, EventArgs e)
        {
            if ((base.FWorker != null) && !base.FWorker.CancellationPending)
            {
                this.SkipFileCount++;
                this.ShowCopyProgress(false);
            }
        }

        public Nomad.Workers.CopyWorker CopyWorker
        {
            get
            {
                return (Nomad.Workers.CopyWorker) base.FWorker;
            }
        }

        public override string OperationName
        {
            get
            {
                return this.FOperationName;
            }
        }
    }
}

