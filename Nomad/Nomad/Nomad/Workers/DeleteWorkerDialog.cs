namespace Nomad.Workers
{
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DeleteWorkerDialog : ItemWorkerDialog
    {
        private IContainer components = null;
        private DialogResult DefaultDeleteNonEmptyFolderAction = DialogResult.None;
        private DialogResult DefaultDeleteReadOnlyItemAcion = DialogResult.None;
        private MessageDialogResult[] StandardButtons = new MessageDialogResult[] { MessageDialogResult.Yes, MessageDialogResult.YesToAll, MessageDialogResult.No, MessageDialogResult.Cancel };

        public DeleteWorkerDialog()
        {
            this.InitializeComponent();
            if (!ConfirmationSettings.Default.DeleteNonEmptyFolder)
            {
                this.DefaultDeleteNonEmptyFolderAction = DialogResult.Yes;
            }
            if (!ConfirmationSettings.Default.DeleteReadOnlyFile)
            {
                this.DefaultDeleteReadOnlyItemAcion = DialogResult.Yes;
            }
            base.SaveSettings = true;
        }

        private void AfterDeleteItem(object sender, VirtualItemEventArgs e)
        {
            base.IncreaseProcessedCount();
        }

        protected override void AttachEvents()
        {
            if (base.FWorker != null)
            {
                this.DeleteWorker.OnBeforeDelete += new EventHandler<BeforeDeleteItemEventArgs>(this.BeforeDeleteItem);
                this.DeleteWorker.OnAfterDelete += new EventHandler<VirtualItemEventArgs>(this.AfterDeleteItem);
                this.DeleteWorker.OnDeleteError += new EventHandler<ChangeItemErrorEventArgs>(this.ItemError);
            }
            base.AttachEvents();
        }

        private void BeforeDeleteItem(object sender, BeforeDeleteItemEventArgs e)
        {
            base.ItemFileName = e.Item.FullName;
            bool deleteNonEmptyFolder = false;
            bool deleteReadOnlyItem = false;
            if (e.Item is IVirtualFolder)
            {
                if ((this.DefaultDeleteNonEmptyFolderAction == DialogResult.Yes) || ((e.Item.Attributes & FileAttributes.ReparsePoint) != 0))
                {
                    e.Action = DialogResult.Yes;
                }
                else
                {
                    IVirtualFolder folder;
                    bool flag3 = false;
                    lock (e.Item)
                    {
                        ICloneable cloneable = e.Item as ICloneable;
                        folder = (cloneable != null) ? ((IVirtualFolder) cloneable.Clone()) : ((IVirtualFolder) e.Item);
                    }
                    foreach (IVirtualItem item in folder.GetContent())
                    {
                        flag3 = true;
                        break;
                    }
                    if (flag3)
                    {
                        if (this.DefaultDeleteNonEmptyFolderAction != DialogResult.None)
                        {
                            e.Action = this.DefaultDeleteNonEmptyFolderAction;
                        }
                        else
                        {
                            deleteNonEmptyFolder = true;
                        }
                    }
                }
            }
            else if ((e.Item.Attributes & FileAttributes.ReadOnly) > 0)
            {
                if (this.DefaultDeleteReadOnlyItemAcion != DialogResult.None)
                {
                    e.Action = this.DefaultDeleteReadOnlyItemAcion;
                }
                else
                {
                    deleteReadOnlyItem = true;
                }
            }
            if (deleteNonEmptyFolder || deleteReadOnlyItem)
            {
                if (base.InvokeRequired)
                {
                    base.Invoke(new ShowDeleteItemDelegate(this.ShowDeleteItem), new object[] { sender, deleteNonEmptyFolder, deleteReadOnlyItem, e });
                }
                else
                {
                    this.ShowDeleteItem(sender, deleteNonEmptyFolder, deleteReadOnlyItem, e);
                }
            }
        }

        private void BeforeDeleteNonEmptyFolder(object sender, BeforeDeleteItemEventArgs e)
        {
            bool checkBoxChecked = false;
            MessageDialogResult result = MessageDialog.Show(this, string.Format(Resources.sAskDeleteNonEmptyFolder, e.Item.FullName), Resources.sConfirmFolderDelete, Resources.sDoNotAskAgain, ref checkBoxChecked, this.StandardButtons, MessageBoxIcon.Question, MessageDialogResult.No);
            e.Action = ConvertDialogResult(result, ref this.DefaultDeleteNonEmptyFolderAction);
            if (checkBoxChecked)
            {
                ConfirmationSettings.Default.DeleteNonEmptyFolder = false;
                if (this.DefaultDeleteNonEmptyFolderAction == DialogResult.None)
                {
                    this.DefaultDeleteNonEmptyFolderAction = DialogResult.Yes;
                }
            }
        }

        private void BeforeDeleteReadOnlyItem(object sender, BeforeDeleteItemEventArgs e)
        {
            bool checkBoxChecked = false;
            MessageDialogResult result = MessageDialog.Show(this, string.Format(Resources.sAskDeleteReadOnlyFile, e.Item.FullName), Resources.sConfirmFileDelete, Resources.sDoNotAskAgain, ref checkBoxChecked, this.StandardButtons, MessageBoxIcon.Question, MessageDialogResult.No);
            e.Action = ConvertDialogResult(result, ref this.DefaultDeleteReadOnlyItemAcion);
            if (checkBoxChecked)
            {
                ConfirmationSettings.Default.DeleteReadOnlyFile = false;
                if (this.DefaultDeleteReadOnlyItemAcion == DialogResult.None)
                {
                    this.DefaultDeleteReadOnlyItemAcion = DialogResult.Yes;
                }
            }
        }

        private static DialogResult ConvertDialogResult(MessageDialogResult result, ref DialogResult defaultResult)
        {
            if (result == MessageDialogResult.YesToAll)
            {
                defaultResult = DialogResult.Yes;
                return DialogResult.Yes;
            }
            if ((result == MessageDialogResult.No) && (Control.ModifierKeys == Keys.Shift))
            {
                defaultResult = DialogResult.No;
            }
            return MessageDialog.ConvertDialogResult(result);
        }

        private void DeleteWorkerDialog_Load(object sender, EventArgs e)
        {
            base.lblOperationDescription.Text = Resources.sDeletingFileOrFolder;
        }

        protected override void DetachEvents()
        {
            if (base.FWorker != null)
            {
                this.DeleteWorker.OnBeforeDelete -= new EventHandler<BeforeDeleteItemEventArgs>(this.BeforeDeleteItem);
                this.DeleteWorker.OnAfterDelete -= new EventHandler<VirtualItemEventArgs>(this.AfterDeleteItem);
                this.DeleteWorker.OnDeleteError -= new EventHandler<ChangeItemErrorEventArgs>(this.ItemError);
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
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.ClientSize = new Size(0x176, 0x76);
            base.Name = "DeleteWorkerDialog";
            base.TotalProgressText = "0 items";
            base.Load += new EventHandler(this.DeleteWorkerDialog_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public static DeleteWorkerDialog ShowAsync(Nomad.Workers.DeleteWorker worker)
        {
            return (DeleteWorkerDialog) CustomWorkerDialog.ShowAsync(typeof(DeleteWorkerDialog), worker);
        }

        private void ShowDeleteItem(object sender, bool DeleteNonEmptyFolder, bool DeleteReadOnlyItem, BeforeDeleteItemEventArgs e)
        {
            base.ShowItemName();
            this.ChangeProgressState(ProgressState.Pause);
            if (DeleteNonEmptyFolder)
            {
                this.BeforeDeleteNonEmptyFolder(sender, e);
            }
            if (DeleteReadOnlyItem)
            {
                this.BeforeDeleteReadOnlyItem(sender, e);
            }
            if (e.Action != DialogResult.Cancel)
            {
                this.ChangeProgressState(ProgressState.Normal);
            }
        }

        public Nomad.Workers.DeleteWorker DeleteWorker
        {
            get
            {
                return (Nomad.Workers.DeleteWorker) base.FWorker;
            }
        }

        protected override string ItemErrorCaption
        {
            get
            {
                return Resources.sCaptionErrorDeletingFile;
            }
        }

        public override string OperationName
        {
            get
            {
                return Resources.sCaptionDelete;
            }
        }

        private delegate void ShowDeleteItemDelegate(object sender, bool DeleteNonEmptyFolder, bool DeleteReadOnlyItem, BeforeDeleteItemEventArgs e);
    }
}

