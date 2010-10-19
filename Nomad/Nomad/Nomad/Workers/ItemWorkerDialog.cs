namespace Nomad.Workers
{
    using Nomad.Commons;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class ItemWorkerDialog : CustomWorkerDialog
    {
        private IContainer components = null;
        private string FItemFileName;
        private int FProcessedItems;
        private int FTotalProgress = -1;
        private string ItemProgressStr = Resources.sItemProgress;
        private Label lblFileName;
        protected Label lblOperationDescription;
        private Label lblSeparator;
        protected TableLayoutPanel pnlItemDetails;
        private System.Windows.Forms.Timer tmrUpdateProgress;

        public ItemWorkerDialog()
        {
            this.InitializeComponent();
            this.lblFileName.Text = string.Empty;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void IncreaseProcessedCount()
        {
            Interlocked.Increment(ref this.FProcessedItems);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.pnlItemDetails = new TableLayoutPanel();
            this.lblSeparator = new Label();
            this.lblOperationDescription = new Label();
            this.lblFileName = new Label();
            this.tmrUpdateProgress = new System.Windows.Forms.Timer(this.components);
            this.pnlItemDetails.SuspendLayout();
            base.SuspendLayout();
            this.pnlItemDetails.AutoSize = true;
            this.pnlItemDetails.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.pnlItemDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.pnlItemDetails.Controls.Add(this.lblSeparator, 0, 2);
            this.pnlItemDetails.Controls.Add(this.lblOperationDescription, 0, 0);
            this.pnlItemDetails.Controls.Add(this.lblFileName, 0, 1);
            this.pnlItemDetails.Dock = DockStyle.Top;
            this.pnlItemDetails.Location = new Point(9, 9);
            this.pnlItemDetails.Name = "pnlItemDetails";
            this.pnlItemDetails.RowCount = 1;
            this.pnlItemDetails.RowStyles.Add(new RowStyle());
            this.pnlItemDetails.RowStyles.Add(new RowStyle());
            this.pnlItemDetails.RowStyles.Add(new RowStyle());
            this.pnlItemDetails.Size = new Size(0x164, 0x21);
            this.pnlItemDetails.TabIndex = 0;
            this.pnlItemDetails.VisibleChanged += new EventHandler(this.pnlItemDetails_VisibleChanged);
            this.lblSeparator.BorderStyle = BorderStyle.Fixed3D;
            this.lblSeparator.Dock = DockStyle.Top;
            this.lblSeparator.Location = new Point(3, 0x1d);
            this.lblSeparator.Margin = new Padding(3);
            this.lblSeparator.Name = "lblSeparator";
            this.lblSeparator.Size = new Size(350, 1);
            this.lblSeparator.TabIndex = 2;
            this.lblOperationDescription.AutoSize = true;
            this.lblOperationDescription.Location = new Point(3, 0);
            this.lblOperationDescription.Name = "lblOperationDescription";
            this.lblOperationDescription.Size = new Size(0x74, 13);
            this.lblOperationDescription.TabIndex = 0;
            this.lblOperationDescription.Text = "lblOperationDescription";
            this.lblFileName.AutoEllipsis = true;
            this.lblFileName.Dock = DockStyle.Fill;
            this.lblFileName.Location = new Point(3, 13);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new Size(350, 13);
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "lblFileName";
            this.lblFileName.UseMnemonic = false;
            this.tmrUpdateProgress.Enabled = true;
            this.tmrUpdateProgress.Interval = 250;
            this.tmrUpdateProgress.Tick += new EventHandler(this.tmrUpdateProgress_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.ClientSize = new Size(0x176, 0x76);
            base.Controls.Add(this.pnlItemDetails);
            base.Name = "ItemWorkerDialog";
            this.Text = "Operation";
            base.Controls.SetChildIndex(this.pnlItemDetails, 0);
            this.pnlItemDetails.ResumeLayout(false);
            this.pnlItemDetails.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void pnlItemDetails_VisibleChanged(object sender, EventArgs e)
        {
            if (this.pnlItemDetails.Visible)
            {
                lock (this.pnlItemDetails)
                {
                    this.ShowItemName();
                }
            }
        }

        protected override void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.FTotalProgress = e.ProgressPercentage;
        }

        protected void ShowItemName()
        {
            this.lblFileName.Text = (this.FItemFileName == null) ? string.Empty : StringHelper.CompactString(this.FItemFileName, this.lblFileName.Width, this.lblFileName.Font, TextFormatFlags.PathEllipsis | TextFormatFlags.NoPrefix);
            if (this.FTotalProgress >= 0)
            {
                base.TotalProgress = this.FTotalProgress;
            }
        }

        private void tmrUpdateProgress_Tick(object sender, EventArgs e)
        {
            base.TotalProgressText = PluralInfo.Format(this.ItemProgressStr, new object[] { this.FProcessedItems });
            if (this.pnlItemDetails.Visible)
            {
                lock (this.pnlItemDetails)
                {
                    this.ShowItemName();
                }
            }
            else if (this.FTotalProgress >= 0)
            {
                base.TotalProgress = this.FTotalProgress;
            }
        }

        protected string ItemFileName
        {
            get
            {
                return this.FItemFileName;
            }
            set
            {
                lock (this.pnlItemDetails)
                {
                    this.FItemFileName = value;
                }
            }
        }
    }
}

