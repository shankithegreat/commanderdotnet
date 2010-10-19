namespace Nomad.Workers
{
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class PackWorkerDialog : CustomWorkerDialog
    {
        private string AddingFileToTheArchiveStr;
        private IContainer components = null;
        private string CopyProgressSizeWithTimeStr = Resources.sCopyProgressSizeWithTime;
        private string CopySpeedStr = Resources.sCopySpeed;
        private PackStage FCurrentStage;
        private bool FShowTotal;
        private string FSourceFileName;
        private int FTotalProgress = -1;
        private long FVisibleProcessedSize;
        private Label lblArchiveAction;
        private Label lblArchiveName;
        private Label lblElapsedTime;
        private Label lblFileAction;
        private Label lblFileName;
        private Label lblFiles;
        private Label lblProcessedSize;
        private Label lblRatio;
        private Label lblRemainingTime;
        private Label lblSpeed;
        private Label lblStarted;
        private Label lblTotalSize;
        private bool RejectAll;
        private Queue<PackProgressSnapshot> SnapshotQueue = new Queue<PackProgressSnapshot>(11);
        private TableLayoutPanel tblPackDetails;
        private Timer tmrUpdateProgress;

        public PackWorkerDialog()
        {
            this.InitializeComponent();
            base.LocalizeForm();
            this.lblArchiveName.Text = string.Empty;
            this.lblFileName.Text = string.Empty;
            this.AddingFileToTheArchiveStr = this.lblFileAction.Text;
            base.SaveSettings = true;
        }

        protected override void AttachEvents()
        {
            base.AttachEvents();
            if (base.FWorker != null)
            {
                Nomad.Workers.PackWorker packWorker = this.PackWorker;
                packWorker.OnBeforePackItem = (EventHandler<VirtualItemEventArgs>) Delegate.Combine(packWorker.OnBeforePackItem, new EventHandler<VirtualItemEventArgs>(this.BeforePackItem));
                Nomad.Workers.PackWorker worker2 = this.PackWorker;
                worker2.OnConfirmAction = (EventHandler<ConfirmArchiveActionArgs>) Delegate.Combine(worker2.OnConfirmAction, new EventHandler<ConfirmArchiveActionArgs>(this.ConfirmArchiveAction));
                Nomad.Workers.PackWorker worker3 = this.PackWorker;
                worker3.OnRejectItem = (EventHandler<CancelVirtualItemEventArgs>) Delegate.Combine(worker3.OnRejectItem, new EventHandler<CancelVirtualItemEventArgs>(this.RejectItem));
                Nomad.Workers.PackWorker worker4 = this.PackWorker;
                worker4.OnItemError = (EventHandler<ChangeItemErrorEventArgs>) Delegate.Combine(worker4.OnItemError, new EventHandler<ChangeItemErrorEventArgs>(this.ItemError));
            }
        }

        private void BeforePackItem(object sender, VirtualItemEventArgs e)
        {
            lock (this.tblPackDetails)
            {
                TextFormatFlags formatFlags = TextFormatFlags.PathEllipsis | TextFormatFlags.NoPrefix;
                this.FSourceFileName = StringHelper.CompactString(e.Item.FullName, this.lblFileName.Width, this.lblFileName.Font, formatFlags);
            }
        }

        private void ConfirmArchiveAction(object sender, ConfirmArchiveActionArgs e)
        {
            string actionTag = e.ActionTag;
            if (actionTag != null)
            {
                string sArchiveActionFileExistsNoTemp;
                if (!(actionTag == "FileExistsCreate"))
                {
                    if (actionTag != "FileExistsNotArchive")
                    {
                        if (actionTag != "FileExistsNoTemp")
                        {
                            return;
                        }
                        sArchiveActionFileExistsNoTemp = Resources.sArchiveActionFileExistsNoTemp;
                    }
                    else
                    {
                        sArchiveActionFileExistsNoTemp = Resources.sArchiveActionFileExistsNotArchive;
                    }
                }
                else
                {
                    sArchiveActionFileExistsNoTemp = Resources.sArchiveActionFileExistsCreate;
                }
                sArchiveActionFileExistsNoTemp = string.Format(sArchiveActionFileExistsNoTemp, e.DestArchiveFile.FullName, e.ArchiveFormat.Name);
                if (base.InvokeRequired)
                {
                    e.Cancel = !((bool) base.Invoke(new Func<string, bool>(this.ShowConfirmDialog), new object[] { sArchiveActionFileExistsNoTemp }));
                }
                else
                {
                    e.Cancel = !this.ShowConfirmDialog(sArchiveActionFileExistsNoTemp);
                }
            }
        }

        private void CopyWorkerDialog_Shown(object sender, EventArgs e)
        {
            this.lblArchiveAction.Text = string.Format(this.lblArchiveAction.Text, this.PackWorker.ArchiveFormat.Name);
            this.lblArchiveName.Text = StringHelper.CompactString(this.PackWorker.DestFile.FullName, this.lblArchiveName.Width, this.lblArchiveName.Font, TextFormatFlags.PathEllipsis | TextFormatFlags.NoPrefix);
            this.lblStarted.Text = DateTime.Now.ToString("g");
            this.ShowCopyProgress(false);
        }

        protected override void DetachEvents()
        {
            if (base.FWorker != null)
            {
                Nomad.Workers.PackWorker packWorker = this.PackWorker;
                packWorker.OnBeforePackItem = (EventHandler<VirtualItemEventArgs>) Delegate.Remove(packWorker.OnBeforePackItem, new EventHandler<VirtualItemEventArgs>(this.BeforePackItem));
                Nomad.Workers.PackWorker worker2 = this.PackWorker;
                worker2.OnConfirmAction = (EventHandler<ConfirmArchiveActionArgs>) Delegate.Remove(worker2.OnConfirmAction, new EventHandler<ConfirmArchiveActionArgs>(this.ConfirmArchiveAction));
                Nomad.Workers.PackWorker worker3 = this.PackWorker;
                worker3.OnRejectItem = (EventHandler<CancelVirtualItemEventArgs>) Delegate.Remove(worker3.OnRejectItem, new EventHandler<CancelVirtualItemEventArgs>(this.RejectItem));
                Nomad.Workers.PackWorker worker4 = this.PackWorker;
                worker4.OnItemError = (EventHandler<ChangeItemErrorEventArgs>) Delegate.Remove(worker4.OnItemError, new EventHandler<ChangeItemErrorEventArgs>(this.ItemError));
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

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PackWorkerDialog));
            this.lblElapsedTime = new Label();
            this.lblRemainingTime = new Label();
            this.lblRatio = new Label();
            this.lblStarted = new Label();
            this.lblFiles = new Label();
            this.lblTotalSize = new Label();
            this.lblProcessedSize = new Label();
            this.lblSpeed = new Label();
            this.lblFileAction = new Label();
            this.lblFileName = new Label();
            this.lblArchiveName = new Label();
            this.lblArchiveAction = new Label();
            this.tmrUpdateProgress = new Timer(this.components);
            this.tblPackDetails = new TableLayoutPanel();
            Bevel control = new Bevel();
            Label label = new Label();
            Label label2 = new Label();
            Label label3 = new Label();
            Label label4 = new Label();
            Label label5 = new Label();
            Label label6 = new Label();
            Label label7 = new Label();
            Label label8 = new Label();
            Panel panel = new Panel();
            Panel panel2 = new Panel();
            Panel panel3 = new Panel();
            Panel panel4 = new Panel();
            Panel panel5 = new Panel();
            Bevel bevel2 = new Bevel();
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
            this.tblPackDetails.SuspendLayout();
            base.SuspendLayout();
            this.tblPackDetails.SetColumnSpan(control, 3);
            manager.ApplyResources(control, "bvlSeparator1");
            control.Name = "bvlSeparator1";
            control.Sides = Border3DSide.Top;
            manager.ApplyResources(label, "lblStartedCaption");
            label.Name = "lblStartedCaption";
            manager.ApplyResources(label2, "lblElapsedCaption");
            label2.Name = "lblElapsedCaption";
            manager.ApplyResources(label3, "lblRemainingCaption");
            label3.Name = "lblRemainingCaption";
            manager.ApplyResources(label4, "lblRatioCaption");
            label4.Name = "lblRatioCaption";
            manager.ApplyResources(label5, "lblTotalSizeCaption");
            label5.Name = "lblTotalSizeCaption";
            manager.ApplyResources(label6, "lblProcessedSizeCaption");
            label6.Name = "lblProcessedSizeCaption";
            manager.ApplyResources(label7, "lblSpeedCaption");
            label7.Name = "lblSpeedCaption";
            manager.ApplyResources(label8, "lblFilesCaption");
            label8.Name = "lblFilesCaption";
            panel.Controls.Add(this.lblElapsedTime);
            panel.Controls.Add(label2);
            manager.ApplyResources(panel, "pnlElapsedTime");
            panel.Name = "pnlElapsedTime";
            manager.ApplyResources(this.lblElapsedTime, "lblElapsedTime");
            this.lblElapsedTime.Name = "lblElapsedTime";
            panel2.Controls.Add(this.lblRemainingTime);
            panel2.Controls.Add(label3);
            manager.ApplyResources(panel2, "pnlRemainingTime");
            panel2.Name = "pnlRemainingTime";
            manager.ApplyResources(this.lblRemainingTime, "lblRemainingTime");
            this.lblRemainingTime.Name = "lblRemainingTime";
            panel3.Controls.Add(this.lblRatio);
            panel3.Controls.Add(label4);
            manager.ApplyResources(panel3, "pnlCopyMode");
            panel3.Name = "pnlCopyMode";
            manager.ApplyResources(this.lblRatio, "lblRatio");
            this.lblRatio.Name = "lblRatio";
            this.lblRatio.UseMnemonic = false;
            panel4.Controls.Add(this.lblStarted);
            panel4.Controls.Add(label);
            manager.ApplyResources(panel4, "pnlStarted");
            panel4.Name = "pnlStarted";
            manager.ApplyResources(this.lblStarted, "lblStarted");
            this.lblStarted.Name = "lblStarted";
            manager.ApplyResources(panel5, "pnlFiles");
            panel5.Controls.Add(this.lblFiles);
            panel5.Controls.Add(label8);
            panel5.Name = "pnlFiles";
            manager.ApplyResources(this.lblFiles, "lblFiles");
            this.lblFiles.Name = "lblFiles";
            this.tblPackDetails.SetColumnSpan(bevel2, 3);
            manager.ApplyResources(bevel2, "bvlSeparator2");
            bevel2.Name = "bvlSeparator2";
            bevel2.Sides = Border3DSide.Top;
            manager.ApplyResources(panel6, "pnlTotalSize");
            panel6.Controls.Add(this.lblTotalSize);
            panel6.Controls.Add(label5);
            panel6.Name = "pnlTotalSize";
            manager.ApplyResources(this.lblTotalSize, "lblTotalSize");
            this.lblTotalSize.Name = "lblTotalSize";
            manager.ApplyResources(panel7, "pnlProcessedSize");
            panel7.Controls.Add(this.lblProcessedSize);
            panel7.Controls.Add(label6);
            panel7.Name = "pnlProcessedSize";
            manager.ApplyResources(this.lblProcessedSize, "lblProcessedSize");
            this.lblProcessedSize.Name = "lblProcessedSize";
            manager.ApplyResources(panel8, "pnlSpeed");
            panel8.Controls.Add(this.lblSpeed);
            panel8.Controls.Add(label7);
            panel8.Name = "pnlSpeed";
            manager.ApplyResources(this.lblSpeed, "lblSpeed");
            this.lblSpeed.Name = "lblSpeed";
            manager.ApplyResources(this.lblFileAction, "lblFileAction");
            this.lblFileAction.Name = "lblFileAction";
            this.lblFileAction.UseMnemonic = false;
            this.tblPackDetails.SetColumnSpan(this.lblFileName, 3);
            manager.ApplyResources(this.lblFileName, "lblFileName");
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.UseMnemonic = false;
            this.tblPackDetails.SetColumnSpan(this.lblArchiveName, 3);
            manager.ApplyResources(this.lblArchiveName, "lblArchiveName");
            this.lblArchiveName.Name = "lblArchiveName";
            this.lblArchiveName.UseMnemonic = false;
            manager.ApplyResources(this.lblArchiveAction, "lblArchiveAction");
            this.lblArchiveAction.Name = "lblArchiveAction";
            this.lblArchiveAction.UseMnemonic = false;
            this.tmrUpdateProgress.Enabled = true;
            this.tmrUpdateProgress.Interval = 350;
            this.tmrUpdateProgress.Tick += new EventHandler(this.tmrUpdateProgress_Tick);
            manager.ApplyResources(this.tblPackDetails, "tblPackDetails");
            this.tblPackDetails.Controls.Add(bevel2, 0, 9);
            this.tblPackDetails.Controls.Add(this.lblArchiveAction, 0, 0);
            this.tblPackDetails.Controls.Add(control, 0, 4);
            this.tblPackDetails.Controls.Add(this.lblArchiveName, 0, 1);
            this.tblPackDetails.Controls.Add(this.lblFileAction, 0, 2);
            this.tblPackDetails.Controls.Add(this.lblFileName, 0, 3);
            this.tblPackDetails.Controls.Add(panel, 0, 6);
            this.tblPackDetails.Controls.Add(panel2, 0, 7);
            this.tblPackDetails.Controls.Add(panel3, 0, 8);
            this.tblPackDetails.Controls.Add(panel4, 0, 5);
            this.tblPackDetails.Controls.Add(panel5, 2, 5);
            this.tblPackDetails.Controls.Add(panel6, 2, 6);
            this.tblPackDetails.Controls.Add(panel7, 2, 7);
            this.tblPackDetails.Controls.Add(panel8, 2, 8);
            this.tblPackDetails.Name = "tblPackDetails";
            manager.ApplyResources(this, "$this");
            base.Controls.Add(this.tblPackDetails);
            base.Name = "PackWorkerDialog";
            base.Shown += new EventHandler(this.CopyWorkerDialog_Shown);
            base.Controls.SetChildIndex(this.tblPackDetails, 0);
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
            this.tblPackDetails.ResumeLayout(false);
            this.tblPackDetails.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void pnlCopyDetails_VisibleChanged(object sender, EventArgs e)
        {
            this.ShowCopyProgress(true);
        }

        protected override void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.FTotalProgress = e.ProgressPercentage;
            if (this.FTotalProgress == 0)
            {
                PackProgressSnapshot userState = (PackProgressSnapshot) e.UserState;
                if (!this.tblPackDetails.Visible)
                {
                    base.TotalProgressText = string.Format(Resources.sCopyProgressSize, 0, SizeTypeConverter.FormatSize<long>(userState.TotalSize, SizeFormat.Dynamic));
                }
            }
            this.FShowTotal = true;
            this.lblFiles.Tag = null;
        }

        private void RejectItem(object sender, CancelVirtualItemEventArgs e)
        {
            if (!this.RejectAll)
            {
                if (base.InvokeRequired)
                {
                    base.Invoke(new EventHandler<CancelVirtualItemEventArgs>(this.RejectItem), new object[] { sender, e });
                }
                else
                {
                    switch (MessageDialog.Show(this, "Cannot pack this item", Resources.sWarning, CustomWorkerDialog.ButtonsSkipCancel, MessageBoxIcon.Exclamation))
                    {
                        case MessageDialogResult.Skip:
                            return;

                        case MessageDialogResult.SkipAll:
                            this.RejectAll = true;
                            return;
                    }
                    e.Cancel = true;
                }
            }
        }

        public static PackWorkerDialog ShowAsync(Nomad.Workers.PackWorker worker)
        {
            return (PackWorkerDialog) CustomWorkerDialog.ShowAsync(typeof(PackWorkerDialog), worker);
        }

        private bool ShowConfirmDialog(string message)
        {
            return (MessageDialog.Show(this, message, Resources.sConfirmArchiveAction, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question) == MessageDialogResult.Yes);
        }

        private void ShowCopyProgress(bool force)
        {
            int fTotalProgress = this.FTotalProgress;
            PackProgressSnapshot progressSnapshot = this.PackWorker.GetProgressSnapshot();
            this.SnapshotQueue.Enqueue(progressSnapshot);
            PackProgressSnapshot snapshot2 = (this.SnapshotQueue.Count > 10) ? this.SnapshotQueue.Dequeue() : this.SnapshotQueue.Peek();
            TimeSpan span = progressSnapshot.Duration - snapshot2.Duration;
            long num2 = progressSnapshot.ProcessedSize - snapshot2.ProcessedSize;
            double size = (num2 > 0L) ? (((double) num2) / span.TotalSeconds) : 0.0;
            if (this.tblPackDetails.Visible)
            {
                TableLayoutPanel panel;
                this.tblPackDetails.SuspendLayout();
                if (this.FCurrentStage != progressSnapshot.Stage)
                {
                    this.FCurrentStage = progressSnapshot.Stage;
                    if (this.FCurrentStage == PackStage.PackingNewItems)
                    {
                        this.lblFileAction.Text = this.AddingFileToTheArchiveStr;
                    }
                    else
                    {
                        this.lblFileAction.Text = Resources.sPerformingAction;
                        string str = TypeDescriptor.GetConverter(typeof(PackStage)).ConvertToString(this.FCurrentStage);
                        lock ((panel = this.tblPackDetails))
                        {
                            this.FSourceFileName = str;
                        }
                    }
                }
                lock ((panel = this.tblPackDetails))
                {
                    this.lblFileName.Text = this.FSourceFileName;
                }
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
                this.lblRatio.Text = string.Format("{0}%", progressSnapshot.CompressionRatio);
                if ((this.lblFiles.Tag == null) || (Convert.ToInt32(this.lblFiles.Tag) != progressSnapshot.ProcessedCount))
                {
                    this.lblFiles.Text = string.Format("{0} / {1}", progressSnapshot.ProcessedCount, this.FShowTotal ? ((object) progressSnapshot.TotalCount) : ((object) "?"));
                    this.lblFiles.Tag = progressSnapshot.ProcessedCount;
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
                this.tblPackDetails.ResumeLayout();
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

        private void tmrUpdateProgress_Tick(object sender, EventArgs e)
        {
            if ((base.FWorker != null) && !base.FWorker.CancellationPending)
            {
                this.ShowCopyProgress(false);
            }
        }

        public override string OperationName
        {
            get
            {
                return Resources.sCaptionPack;
            }
        }

        public Nomad.Workers.PackWorker PackWorker
        {
            get
            {
                return (Nomad.Workers.PackWorker) base.FWorker;
            }
        }
    }
}

