namespace TestForm
{
    partial class FileListViewBase
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
            this.largeImageList = new System.Windows.Forms.ImageList(this.components);
            this.nameColumn = new System.Windows.Forms.ColumnHeader();
            this.typeColumn = new System.Windows.Forms.ColumnHeader();
            this.sizeColumn = new System.Windows.Forms.ColumnHeader();
            this.dateColumn = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // largeImageList
            // 
            this.largeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.largeImageList.ImageSize = new System.Drawing.Size(108, 108);
            this.largeImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 163;
            // 
            // typeColumn
            // 
            this.typeColumn.Text = "Type";
            this.typeColumn.Width = 34;
            // 
            // sizeColumn
            // 
            this.sizeColumn.Text = "Size";
            this.sizeColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.sizeColumn.Width = 115;
            // 
            // dateColumn
            // 
            this.dateColumn.Text = "Date";
            this.dateColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dateColumn.Width = 114;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 400;
            // 
            // FileListViewBase
            // 
            this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.View = System.Windows.Forms.View.Details;
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.ImageList largeImageList;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader typeColumn;
        private System.Windows.Forms.ColumnHeader sizeColumn;
        private System.Windows.Forms.ColumnHeader dateColumn;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
