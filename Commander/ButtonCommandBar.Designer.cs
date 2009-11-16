namespace Commander
{
    partial class ButtonCommandBar
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.exitButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.creteFolderButton = new System.Windows.Forms.Button();
            this.moveButton = new System.Windows.Forms.Button();
            this.copyButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.viewButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 7;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28204F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28633F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28633F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28633F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28633F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28633F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28633F));
            this.tableLayoutPanel.Controls.Add(this.exitButton, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.deleteButton, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.creteFolderButton, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.moveButton, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.copyButton, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.editButton, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.viewButton, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(851, 27);
            this.tableLayoutPanel.TabIndex = 4;
            // 
            // exitButton
            // 
            this.exitButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exitButton.Location = new System.Drawing.Point(729, 3);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(119, 21);
            this.exitButton.TabIndex = 7;
            this.exitButton.Text = "Alt+F4 Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            // 
            // deleteButton
            // 
            this.deleteButton.AllowDrop = true;
            this.deleteButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deleteButton.Location = new System.Drawing.Point(608, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(115, 21);
            this.deleteButton.TabIndex = 6;
            this.deleteButton.Text = "F8 Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.DragDrop += new System.Windows.Forms.DragEventHandler(this.deleteButton_DragDrop);
            this.deleteButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.deleteButton_MouseUp);
            this.deleteButton.DragEnter += new System.Windows.Forms.DragEventHandler(this.deleteButton_DragEnter);
            // 
            // creteFolderButton
            // 
            this.creteFolderButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.creteFolderButton.Location = new System.Drawing.Point(487, 3);
            this.creteFolderButton.Name = "creteFolderButton";
            this.creteFolderButton.Size = new System.Drawing.Size(115, 21);
            this.creteFolderButton.TabIndex = 5;
            this.creteFolderButton.Text = "F7 Folder";
            this.creteFolderButton.UseVisualStyleBackColor = true;
            // 
            // moveButton
            // 
            this.moveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.moveButton.Location = new System.Drawing.Point(366, 3);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(115, 21);
            this.moveButton.TabIndex = 4;
            this.moveButton.Text = "F6 Move";
            this.moveButton.UseVisualStyleBackColor = true;
            // 
            // copyButton
            // 
            this.copyButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.copyButton.Location = new System.Drawing.Point(245, 3);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(115, 21);
            this.copyButton.TabIndex = 3;
            this.copyButton.Text = "F5 Copy";
            this.copyButton.UseVisualStyleBackColor = true;
            // 
            // editButton
            // 
            this.editButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editButton.Location = new System.Drawing.Point(124, 3);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(115, 21);
            this.editButton.TabIndex = 2;
            this.editButton.Text = "F4 Edit";
            this.editButton.UseVisualStyleBackColor = true;
            // 
            // viewButton
            // 
            this.viewButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewButton.Location = new System.Drawing.Point(3, 3);
            this.viewButton.Name = "viewButton";
            this.viewButton.Size = new System.Drawing.Size(115, 21);
            this.viewButton.TabIndex = 1;
            this.viewButton.Text = "F3 View";
            this.viewButton.UseVisualStyleBackColor = true;
            // 
            // ButtonCommandBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "ButtonCommandBar";
            this.Size = new System.Drawing.Size(851, 27);
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button creteFolderButton;
        private System.Windows.Forms.Button moveButton;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button viewButton;
    }
}
