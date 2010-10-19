namespace Nomad.Workers
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Plugin;
    using Nomad.Commons.Resources;
    using Nomad.Controls;
    using Nomad.Dialogs;
    using Nomad.Properties;
    using Nomad.Workers.Configuration;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class CustomWorkerDialog : FormEx
    {
        private IPluginProcess AutoElevateProcess;
        private VistaProgressBar barTotalProgress;
        private Button btnCancel;
        private Button btnPause;
        private TaskbarProgress ButtonProgress;
        protected static readonly MessageDialogResult[] ButtonsRetrySkipCancel = new MessageDialogResult[] { MessageDialogResult.Retry, MessageDialogResult.Skip, MessageDialogResult.SkipAll, MessageDialogResult.Cancel };
        protected static readonly MessageDialogResult[] ButtonsSkipCancel = new MessageDialogResult[] { MessageDialogResult.Skip, MessageDialogResult.SkipAll, MessageDialogResult.Cancel };
        private EventWaitHandle CompletedEvent;
        private IContainer components;
        protected Dictionary<System.Type, ChangeItemAction> DefaultErrorAction;
        private string FOperationName;
        private WorkerDialogSettings FSettings;
        private string FTotalProgressText;
        private bool FTotalProgressTextChanged;
        protected EventBackgroundWorker FWorker;
        private ImageList imageList;
        private bool IsFormLocalized;
        private Label lblProgress;
        private Label lblTotalProgress;
        protected TableLayoutPanel pnlTotalProgress;
        private IAsyncResult ProgressChangedResult;
        protected bool SaveSettings;
        private ToolStrip toolStrip;
        private ToolStripButton tsbShowDetails;
        private ToolStripButton tsbTopMost;
        protected readonly TraceSource WorkerTrace;

        public CustomWorkerDialog()
        {
            EventHandler handler = null;
            this.CompletedEvent = new ManualResetEvent(false);
            this.WorkerTrace = new TraceSource("Worker");
            this.FOperationName = "Operation";
            this.components = null;
            this.InitializeComponent();
            if (!base.DesignMode)
            {
                this.btnPause.Text = Resources.sPauseButton;
                this.toolStrip.Renderer = BorderLessToolStripRenderer.Default;
                this.toolStrip.ImageList = this.imageList;
                this.imageList.Images.Add(Resources.Pin);
                this.imageList.Images.Add(Resources.Pin2);
                this.imageList.Images.Add(Resources.ShowDetail);
                this.imageList.Images.Add(Resources.HideDetail);
                this.ButtonProgress = TaskbarProgress.TryCreate(this);
                if (this.ButtonProgress != null)
                {
                    this.ButtonProgress.Maximum = this.barTotalProgress.Maximum;
                    this.ButtonProgress.State = TaskbarProgressState.Marquee;
                    if (handler == null)
                    {
                        handler = delegate (object sender, EventArgs e) {
                            this.ButtonProgress.Dispose();
                        };
                    }
                    base.Disposed += handler;
                }
                base.Disposed += new EventHandler(this.Form_Disposed);
            }
        }

        protected virtual void AttachEvents()
        {
            if (this.FWorker != null)
            {
                this.FWorker.ProgressChanged += new ProgressChangedEventHandler(this.ProgressChanged);
                this.FWorker.Completed += new AsyncCompletedEventHandler(this.Completed);
                ISetOwnerWindow fWorker = this.FWorker as ISetOwnerWindow;
                if (fWorker != null)
                {
                    fWorker.Owner = this;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (this.FWorker.SuspendingPending)
            {
                this.FWorker.ResumeAsync();
                this.ChangeProgressState(ProgressState.Normal);
                this.btnPause.Text = Resources.sPauseButton;
            }
            else
            {
                this.FWorker.SuspendAsync();
                this.ChangeProgressState(ProgressState.Pause);
                this.btnPause.Text = Resources.sResumeButton;
            }
            this.UpdateText();
        }

        protected virtual void ChangeProgressState(ProgressState newState)
        {
            this.barTotalProgress.State = newState;
            if (this.ButtonProgress != null)
            {
                switch (newState)
                {
                    case ProgressState.Normal:
                        this.ButtonProgress.State = (this.barTotalProgress.Style == ProgressBarStyle.Marquee) ? TaskbarProgressState.Marquee : TaskbarProgressState.Normal;
                        break;

                    case ProgressState.Pause:
                        this.ButtonProgress.State = TaskbarProgressState.Pause;
                        break;

                    case ProgressState.Error:
                        this.ButtonProgress.State = TaskbarProgressState.Error;
                        break;
                }
            }
        }

        public void CloseAsync(DialogResult dialogResult)
        {
            base.DialogResult = dialogResult;
            if (base.Visible)
            {
                if (base.InvokeRequired)
                {
                    base.BeginInvoke(new MethodInvoker(this.Close));
                }
                else if (base.IsHandleCreated)
                {
                    base.Close();
                }
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.WorkerTrace.TraceException(TraceEventType.Critical, e.Error);
                if (base.InvokeRequired)
                {
                    base.Invoke(new AsyncCompletedEventHandler(this.ShowException), new object[] { sender, e });
                }
                else
                {
                    this.ShowException(sender, e);
                }
            }
            this.CloseAsync(!e.Cancelled ? DialogResult.OK : DialogResult.Cancel);
            this.CompletedEvent.Set();
        }

        protected virtual void DetachEvents()
        {
            if (this.FWorker != null)
            {
                ISetOwnerWindow fWorker = this.FWorker as ISetOwnerWindow;
                if ((fWorker != null) && (fWorker.Owner == this))
                {
                    fWorker.Owner = null;
                }
                this.FWorker.ProgressChanged -= new ProgressChangedEventHandler(this.ProgressChanged);
                this.FWorker.Completed -= new AsyncCompletedEventHandler(this.Completed);
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

        private void Form_Disposed(object sender, EventArgs e)
        {
            this.DetachEvents();
            if (this.AutoElevateProcess != null)
            {
                this.AutoElevateProcess.KeepAlive = false;
            }
            this.AutoElevateProcess = null;
            this.WorkerTrace.Close();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(CustomWorkerDialog));
            this.btnPause = new Button();
            this.btnCancel = new Button();
            this.lblTotalProgress = new Label();
            this.lblProgress = new Label();
            this.toolStrip = new ToolStrip();
            this.tsbTopMost = new ToolStripButton();
            this.tsbShowDetails = new ToolStripButton();
            this.imageList = new ImageList(this.components);
            this.pnlTotalProgress = new TableLayoutPanel();
            this.barTotalProgress = new VistaProgressBar();
            this.toolStrip.SuspendLayout();
            this.pnlTotalProgress.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(this.btnPause, "btnPause");
            this.btnPause.Name = "btnPause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new EventHandler(this.btnPause_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.pnlTotalProgress.SetColumnSpan(this.lblTotalProgress, 4);
            manager.ApplyResources(this.lblTotalProgress, "lblTotalProgress");
            this.lblTotalProgress.Name = "lblTotalProgress";
            this.lblTotalProgress.UseMnemonic = false;
            manager.ApplyResources(this.lblProgress, "lblProgress");
            this.lblProgress.Name = "lblProgress";
            this.toolStrip.BackColor = SystemColors.Control;
            manager.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new ToolStripItem[] { this.tsbTopMost, this.tsbShowDetails });
            this.toolStrip.Name = "toolStrip";
            this.tsbTopMost.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbTopMost.Image = Resources.Pin;
            manager.ApplyResources(this.tsbTopMost, "tsbTopMost");
            this.tsbTopMost.Name = "tsbTopMost";
            this.tsbTopMost.Paint += new PaintEventHandler(this.tsbTopMost_Paint);
            this.tsbTopMost.Click += new EventHandler(this.tsbTopMost_Click);
            this.tsbShowDetails.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbShowDetails.Image = Resources.HideDetail;
            manager.ApplyResources(this.tsbShowDetails, "tsbShowDetails");
            this.tsbShowDetails.Name = "tsbShowDetails";
            this.tsbShowDetails.Paint += new PaintEventHandler(this.tsbShowDetails_Paint);
            this.tsbShowDetails.Click += new EventHandler(this.tsbShowDetails_Click);
            this.imageList.ColorDepth = ColorDepth.Depth8Bit;
            manager.ApplyResources(this.imageList, "imageList");
            this.imageList.TransparentColor = Color.Transparent;
            manager.ApplyResources(this.pnlTotalProgress, "pnlTotalProgress");
            this.pnlTotalProgress.Controls.Add(this.lblTotalProgress, 1, 0);
            this.pnlTotalProgress.Controls.Add(this.btnPause, 3, 2);
            this.pnlTotalProgress.Controls.Add(this.toolStrip, 0, 2);
            this.pnlTotalProgress.Controls.Add(this.lblProgress, 0, 0);
            this.pnlTotalProgress.Controls.Add(this.barTotalProgress, 0, 1);
            this.pnlTotalProgress.Controls.Add(this.btnCancel, 4, 2);
            this.pnlTotalProgress.Name = "pnlTotalProgress";
            this.pnlTotalProgress.SetColumnSpan(this.barTotalProgress, 5);
            manager.ApplyResources(this.barTotalProgress, "barTotalProgress");
            this.barTotalProgress.MarqueeAnimationSpeed = 50;
            this.barTotalProgress.Name = "barTotalProgress";
            this.barTotalProgress.RenderMode = ProgressRenderMode.Vista;
            this.barTotalProgress.Style = ProgressBarStyle.Marquee;
            base.AcceptButton = this.btnPause;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(this.pnlTotalProgress);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "CustomWorkerDialog";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.pnlTotalProgress.ResumeLayout(false);
            this.pnlTotalProgress.PerformLayout();
            base.ResumeLayout(false);
        }

        protected void ItemError(object sender, ChangeItemErrorEventArgs e)
        {
            ChangeItemAction action;
            if ((this.DefaultErrorAction != null) && this.DefaultErrorAction.TryGetValue(e.Error.GetType(), out action))
            {
                e.Action = action;
            }
            else if (this.TryAutoElevate(e))
            {
                e.Action = ChangeItemAction.Retry;
            }
            else
            {
                this.WorkerTrace.TraceException(TraceEventType.Error, e.Error);
                if (base.InvokeRequired)
                {
                    base.Invoke(new EventHandler<ChangeItemErrorEventArgs>(this.SyncronizedItemError), new object[] { sender, e });
                }
                else
                {
                    this.SyncronizedItemError(sender, e);
                }
            }
        }

        protected void LocalizeForm()
        {
            if (!this.IsFormLocalized)
            {
                BasicFormLocalizer argument = SettingsManager.GetArgument<BasicFormLocalizer>(ArgumentKey.FormLocalizer);
                if (argument != null)
                {
                    argument.Localize(this);
                }
                this.IsFormLocalized = true;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (this.SaveSettings)
            {
                this.FSettings.DetailsVisible = this.DetailsVisible;
                this.FSettings.TopMost = base.TopMost;
                SettingsManager.RegisterSettings(this.FSettings);
            }
            base.Dispose();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if ((this.FWorker != null) && !this.FWorker.IsCompleted)
            {
                bool flag = this.FWorker.SuspendingPending || this.FWorker.CancellationPending;
                if (!flag)
                {
                    this.FWorker.SuspendAsync();
                    this.ChangeProgressState(ProgressState.Pause);
                    this.UpdateText();
                }
                MessageDialogResult none = MessageDialogResult.None;
                switch (e.CloseReason)
                {
                    case CloseReason.TaskManagerClosing:
                    case CloseReason.ApplicationExitCall:
                    case CloseReason.WindowsShutDown:
                        break;

                    default:
                        if (!this.FWorker.CancellationPending)
                        {
                            none = MessageDialog.Show(this, Resources.sAskCancelWorkerDialog, Resources.sConfirmCancel, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question);
                        }
                        else
                        {
                            none = MessageDialog.Show(this, Resources.sAskAbortWorkerDialog, Resources.sConfirmCancel, new MessageDialogResult[] { MessageDialogResult.Abort, MessageDialogResult.Cancel }, MessageBoxIcon.Exclamation, MessageDialogResult.Abort);
                        }
                        break;
                }
                MessageDialogResult result2 = none;
                if (result2 == MessageDialogResult.Yes)
                {
                    this.barTotalProgress.Style = ProgressBarStyle.Marquee;
                    if (this.ButtonProgress != null)
                    {
                        this.ButtonProgress.State = TaskbarProgressState.Marquee;
                    }
                    this.Text = this.OperationName + " - " + Resources.sCancellingText;
                    this.btnPause.Text = Resources.sPauseButton;
                    this.btnPause.Enabled = false;
                    this.btnCancel.Enabled = false;
                    this.FWorker.CancelAsync();
                    e.Cancel = true;
                }
                else if (result2 == MessageDialogResult.Abort)
                {
                    this.FWorker.AbortAsync();
                }
                else
                {
                    if (!flag)
                    {
                        this.FWorker.ResumeAsync();
                        this.ChangeProgressState(ProgressState.Normal);
                        this.UpdateText();
                    }
                    e.Cancel = true;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!base.DesignMode)
            {
                base.SuspendLayout();
                this.OnThemeChanged(EventArgs.Empty);
                this.LocalizeForm();
                this.Font = SystemFonts.DialogFont;
                bool flag = false;
                foreach (Control control in base.Controls)
                {
                    if (control != this.pnlTotalProgress)
                    {
                        flag = true;
                        break;
                    }
                }
                this.tsbShowDetails.Visible = flag;
                this.FSettings = SettingsManager.GetSettings<WorkerDialogSettings>(base.Name);
                if (this.FSettings == null)
                {
                    this.FSettings = new WorkerDialogSettings();
                    this.FSettings.SettingsKey = base.Name;
                    this.FSettings.Reload();
                }
                base.Controls.SetChildIndex(this.pnlTotalProgress, 0);
                this.DetailsVisible = this.FSettings.DetailsVisible;
                base.TopMost = this.FSettings.TopMost;
                base.ResumeLayout();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Text = this.OperationName;
        }

        protected override void OnThemeChanged(EventArgs e)
        {
            this.barTotalProgress.RenderMode = Application.RenderWithVisualStyles ? ProgressRenderMode.Vista : ProgressRenderMode.System;
            base.OnThemeChanged(e);
        }

        protected MessageDialogResult ProcessUnauthorizedAccessException(ChangeItemErrorEventArgs e)
        {
            MessageDialogResult none = MessageDialogResult.None;
            if ((e.Error is UnauthorizedAccessException) && e.CanElevate)
            {
                IElevatable item = e.Item as IElevatable;
                if ((item == null) || !item.CanElevate)
                {
                    return none;
                }
                bool checkBoxChecked = true;
                none = MessageDialog.Show(this, string.Format(Resources.sAskElevateOperationPermissions, e.Item.FullName), Resources.sWarning, Resources.sDoThisForAll, ref checkBoxChecked, new MessageDialogResult[] { MessageDialogResult.Shield, MessageDialogResult.Skip, MessageDialogResult.Cancel }, MessageBoxIcon.Exclamation, MessageDialogResult.Shield);
                if (none == MessageDialogResult.Shield)
                {
                    if (checkBoxChecked)
                    {
                        this.AutoElevateProcess = new ElevatedProcess(true, TimeSpan.FromSeconds(9.0));
                    }
                    if (item.Elevate(this.AutoElevateProcess ?? new ElevatedProcess()))
                    {
                        return MessageDialogResult.Retry;
                    }
                    if (this.AutoElevateProcess != null)
                    {
                        this.AutoElevateProcess.Shutdown();
                    }
                    this.AutoElevateProcess = null;
                    return MessageDialogResult.None;
                }
                if ((none == MessageDialogResult.Retry) && checkBoxChecked)
                {
                    none = MessageDialogResult.SkipAll;
                }
            }
            return none;
        }

        protected virtual void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if ((this.FWorker != null) && !this.FWorker.CancellationPending)
            {
                if (base.InvokeRequired)
                {
                    if ((this.ProgressChangedResult == null) || this.ProgressChangedResult.IsCompleted)
                    {
                        this.ProgressChangedResult = base.BeginInvoke(new ProgressChangedEventHandler(this.ProgressChanged), new object[] { sender, e });
                    }
                }
                else
                {
                    this.TotalProgress = e.ProgressPercentage;
                }
            }
        }

        public bool Run(IWin32Window owner, EventBackgroundWorker woker)
        {
            return this.Run(owner, woker, ThreadPriority.Normal);
        }

        public bool Run(IWin32Window owner, EventBackgroundWorker worker, ThreadPriority priority)
        {
            Debug.Assert(!this.CompletedEvent.WaitOne(0, false));
            base.ShowInTaskbar = false;
            this.tsbTopMost.Visible = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.FWorker = worker;
            this.AttachEvents();
            this.CreateHandle();
            worker.RunAsync(priority);
            if (this.CompletedEvent.WaitOne(500, false))
            {
                return (base.DialogResult == DialogResult.OK);
            }
            return (base.ShowDialog(owner) == DialogResult.OK);
        }

        private static void RunDialog(object obj)
        {
            AsyncWorkerDialog dialog = (AsyncWorkerDialog) obj;
            Program.SetupApplicationExceptionHandler();
            Thread.CurrentThread.CurrentUICulture = dialog.UICulture;
            dialog.Dialog = (CustomWorkerDialog) Activator.CreateInstance(dialog.DialogType);
            dialog.Dialog.FWorker = dialog.Worker;
            dialog.Dialog.AttachEvents();
            dialog.Dialog.CreateHandle();
            dialog.AsyncWaitHandle.Set();
            if (!dialog.Dialog.CompletedEvent.WaitOne(500, false))
            {
                dialog.Dialog.ShowDialog();
            }
            else
            {
                dialog.Dialog.Dispose();
            }
        }

        protected static CustomWorkerDialog ShowAsync(System.Type workerDialogType, EventBackgroundWorker worker)
        {
            AsyncWorkerDialog parameter = new AsyncWorkerDialog {
                UICulture = Thread.CurrentThread.CurrentUICulture,
                DialogType = workerDialogType,
                Worker = worker
            };
            Thread thread = new Thread(new ParameterizedThreadStart(CustomWorkerDialog.RunDialog));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(parameter);
            parameter.AsyncWaitHandle.WaitOne();
            return parameter.Dialog;
        }

        private void ShowException(object sender, AsyncCompletedEventArgs e)
        {
            MessageDialog.ShowException(this, e.Error);
        }

        private void SyncronizedItemError(object sender, ChangeItemErrorEventArgs e)
        {
            this.ChangeProgressState(ProgressState.Error);
            MessageDialogResult result = this.ProcessUnauthorizedAccessException(e);
            switch (result)
            {
                case MessageDialogResult.None:
                {
                    MessageDialogResult[] buttons = e.CanRetry ? ButtonsRetrySkipCancel : ButtonsSkipCancel;
                    MessageDialogResult defaultButton = e.CanRetry ? MessageDialogResult.Retry : MessageDialogResult.Skip;
                    result = MessageDialog.Show(this, e.Error.Message, this.ItemErrorCaption, buttons, MessageBoxIcon.Hand, defaultButton);
                    break;
                }
                case MessageDialogResult.SkipAll:
                    if (this.DefaultErrorAction == null)
                    {
                        this.DefaultErrorAction = new Dictionary<System.Type, ChangeItemAction>();
                    }
                    this.DefaultErrorAction.Add(e.Error.GetType(), ChangeItemAction.Skip);
                    e.Action = ChangeItemAction.Skip;
                    goto Label_00AF;
            }
            e.FromMessageDialogResult(result);
        Label_00AF:
            if (e.Action != ChangeItemAction.Cancel)
            {
                this.ChangeProgressState(ProgressState.Normal);
            }
        }

        protected bool TryAutoElevate(ChangeItemErrorEventArgs e)
        {
            if (((e.Error is UnauthorizedAccessException) && e.CanElevate) && (this.AutoElevateProcess != null))
            {
                IElevatable item = e.Item as IElevatable;
                if (((item != null) && item.CanElevate) && item.Elevate(this.AutoElevateProcess))
                {
                    return true;
                }
                e.CanElevate = false;
            }
            return false;
        }

        private void tsbShowDetails_Click(object sender, EventArgs e)
        {
            bool topMost = base.TopMost;
            this.DetailsVisible = !this.DetailsVisible;
            base.TopMost = topMost;
        }

        private void tsbShowDetails_Paint(object sender, PaintEventArgs e)
        {
            int num = this.DetailsVisible ? 3 : 2;
            if (num != this.tsbShowDetails.ImageIndex)
            {
                this.tsbShowDetails.ImageIndex = num;
                if (num == 2)
                {
                    this.tsbShowDetails.Text = Resources.sShowDetail;
                }
                else
                {
                    this.tsbShowDetails.Text = Resources.sHideDetail;
                }
            }
        }

        private void tsbTopMost_Click(object sender, EventArgs e)
        {
            base.TopMost = !base.TopMost;
            this.tsbTopMost_Paint(sender, null);
        }

        private void tsbTopMost_Paint(object sender, PaintEventArgs e)
        {
            int num = 0;
            if (base.TopMost)
            {
                num = 1;
            }
            if (num != this.tsbTopMost.ImageIndex)
            {
                this.tsbTopMost.ImageIndex = num;
            }
            this.tsbTopMost.Checked = base.TopMost;
        }

        protected void UpdateText()
        {
            StringBuilder builder = new StringBuilder();
            if (this.lblTotalProgress.Visible)
            {
                builder.AppendFormat("{0}% - ", this.barTotalProgress.Value);
            }
            builder.Append(this.OperationName);
            if (this.FWorker.SuspendingPending)
            {
                builder.Append(" - ");
                builder.Append(Resources.sPaused);
            }
            this.Text = builder.ToString();
        }

        protected void UpdateTotalProgressText()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(this.TotalProgressText))
            {
                builder.Append(this.TotalProgressText);
                builder.Append(", ");
            }
            builder.Append(this.barTotalProgress.Value);
            builder.Append('%');
            this.lblTotalProgress.Text = builder.ToString();
            this.lblTotalProgress.Visible = true;
        }

        protected bool DetailsVisible
        {
            get
            {
                foreach (Control control in base.Controls)
                {
                    if (control != this.pnlTotalProgress)
                    {
                        return control.Visible;
                    }
                }
                return false;
            }
            set
            {
                foreach (Control control in base.Controls)
                {
                    if (control != this.pnlTotalProgress)
                    {
                        control.Visible = value;
                    }
                }
            }
        }

        protected virtual string ItemErrorCaption
        {
            get
            {
                return this.OperationName;
            }
        }

        public virtual string OperationName
        {
            get
            {
                return this.FOperationName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.FOperationName = value;
                }
            }
        }

        public int TotalProgress
        {
            get
            {
                return ((this.barTotalProgress.Style == ProgressBarStyle.Marquee) ? -1 : this.barTotalProgress.Value);
            }
            set
            {
                if (((this.barTotalProgress.Value != value) || (this.barTotalProgress.Style != ProgressBarStyle.Blocks)) || this.FTotalProgressTextChanged)
                {
                    if (value < 0)
                    {
                        this.barTotalProgress.Style = ProgressBarStyle.Marquee;
                        this.lblTotalProgress.Visible = false;
                        if (this.ButtonProgress != null)
                        {
                            this.ButtonProgress.State = TaskbarProgressState.Marquee;
                        }
                    }
                    else
                    {
                        if (this.barTotalProgress.Style != ProgressBarStyle.Blocks)
                        {
                            this.barTotalProgress.Style = ProgressBarStyle.Blocks;
                        }
                        this.barTotalProgress.Value = value;
                        if (this.ButtonProgress != null)
                        {
                            this.ButtonProgress.Value = this.barTotalProgress.Value;
                        }
                        this.UpdateTotalProgressText();
                        this.FTotalProgressTextChanged = false;
                    }
                    this.UpdateText();
                }
            }
        }

        public string TotalProgressText
        {
            get
            {
                return this.FTotalProgressText;
            }
            set
            {
                if (this.FTotalProgressText != value)
                {
                    this.FTotalProgressText = value;
                    this.FTotalProgressTextChanged = true;
                }
            }
        }

        private class AsyncWorkerDialog
        {
            public readonly ManualResetEvent AsyncWaitHandle = new ManualResetEvent(false);
            public CustomWorkerDialog Dialog;
            public System.Type DialogType;
            public CultureInfo UICulture;
            public EventBackgroundWorker Worker;
        }
    }
}

