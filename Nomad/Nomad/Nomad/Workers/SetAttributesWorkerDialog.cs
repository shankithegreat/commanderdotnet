namespace Nomad.Workers
{
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class SetAttributesWorkerDialog : ItemWorkerDialog
    {
        private IContainer components = null;

        public SetAttributesWorkerDialog()
        {
            this.InitializeComponent();
            base.SaveSettings = true;
        }

        private void AfterSetAttributes(object sender, VirtualItemEventArgs e)
        {
            base.IncreaseProcessedCount();
        }

        protected override void AttachEvents()
        {
            if (base.FWorker != null)
            {
                this.SetAttributesWorker.OnBeforeSetAttributes += new EventHandler<VirtualItemEventArgs>(this.BeforeSetAttributes);
                this.SetAttributesWorker.OnAfterSetAttributes += new EventHandler<VirtualItemEventArgs>(this.AfterSetAttributes);
                this.SetAttributesWorker.OnSetAttributesError += new EventHandler<ChangeItemErrorEventArgs>(this.ItemError);
            }
            base.AttachEvents();
        }

        private void BeforeSetAttributes(object sender, VirtualItemEventArgs e)
        {
            base.ItemFileName = e.Item.FullName;
        }

        protected override void DetachEvents()
        {
            if (base.FWorker != null)
            {
                this.SetAttributesWorker.OnBeforeSetAttributes -= new EventHandler<VirtualItemEventArgs>(this.BeforeSetAttributes);
                this.SetAttributesWorker.OnAfterSetAttributes -= new EventHandler<VirtualItemEventArgs>(this.AfterSetAttributes);
                this.SetAttributesWorker.OnSetAttributesError -= new EventHandler<ChangeItemErrorEventArgs>(this.ItemError);
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
            base.Name = "SetAttributesWorkerDialog";
            base.TotalProgressText = "0 items";
            base.Load += new EventHandler(this.SetAttributesWorkerDialog_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void SetAttributesWorkerDialog_Load(object sender, EventArgs e)
        {
            base.lblOperationDescription.Text = Resources.sSettingAttrForItem;
        }

        public static SetAttributesWorkerDialog ShowAsync(Nomad.Workers.SetAttributesWorker worker)
        {
            return (SetAttributesWorkerDialog) CustomWorkerDialog.ShowAsync(typeof(SetAttributesWorkerDialog), worker);
        }

        protected override string ItemErrorCaption
        {
            get
            {
                return Resources.sCaptionErrorSetAttributes;
            }
        }

        public override string OperationName
        {
            get
            {
                return Resources.sCaptionSetAttributes;
            }
        }

        public Nomad.Workers.SetAttributesWorker SetAttributesWorker
        {
            get
            {
                return (Nomad.Workers.SetAttributesWorker) base.FWorker;
            }
        }
    }
}

