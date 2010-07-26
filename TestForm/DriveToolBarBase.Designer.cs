namespace Commander
{
    partial class DriveToolBarBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DriveToolBarBase));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "flopydrive.png");
            this.imageList.Images.SetKeyName(1, "Drive.png");
            this.imageList.Images.SetKeyName(2, "cddrive.png");
            this.imageList.Images.SetKeyName(3, "removabledrive.png");
            this.imageList.Images.SetKeyName(4, "net.png");
            this.imageList.Images.SetKeyName(5, "star");
            this.imageList.Images.SetKeyName(6, "downList");
            // 
            // DrivaToolBar
            // 
            this.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.AutoSize = false;
            this.ButtonSize = new System.Drawing.Size(36, 21);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ImageList = this.imageList;
            this.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
    }
}
