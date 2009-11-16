using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Commander
{
    public class GroupDataGridView : DataGridView
    {
        public GroupDataGridView()
        {
            InitializeComponent();
        }


        public void AddGroupRow(string name)
        {
            this.Rows.Add(new DataGridViewGroupHeader(name));
        }

        public bool IsGroupHeader(DataGridViewRow row)
        {
            return (row is DataGridViewGroupHeader);
        }


        protected override void SetSelectedRowCore(int rowIndex, bool selected)
        {
            if (rowIndex > 0 && rowIndex < this.Rows.Count && !IsGroupHeader(this.Rows[rowIndex]))
            {
                base.SetSelectedRowCore(rowIndex, selected);
            }
        }

        protected override void OnRowHeightChanged(DataGridViewRowEventArgs e)
        {
            // Bug 242.
            try
            {
                base.OnRowHeightChanged(e);
            }
            catch (InvalidOperationException)
            {
            }
        }


        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupDataGridView
            // 
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.BackgroundColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.RowHeadersVisible = false;
            this.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }

    public class DataGridViewGroupHeader : DataGridViewRow
    {
        private Font font;
        private Color lineColor = Color.FromArgb(61, 149, 255);
        private string text;

        public DataGridViewGroupHeader(string text)
        {
            this.Text = text;
            this.ReadOnly = true;
        }

        /// <summary>
        /// Gets or sets the name of group.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        protected override void OnDataGridViewChanged()
        {
            if (this.DataGridView != null)
            {
                this.font = new Font(this.DataGridView.Font, FontStyle.Bold);
            }
            base.OnDataGridViewChanged();            
        }
        
        protected override void PaintCells(Graphics graphics, Rectangle clipBounds, Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, bool isFirstDisplayedRow, bool isLastVisibleRow, DataGridViewPaintParts paintParts)
        {
            graphics.FillRectangle(SystemBrushes.Window, rowBounds);
            Rectangle textRectangle = new Rectangle(rowBounds.X + 12, rowBounds.Y, rowBounds.Width - 25, rowBounds.Height);
            TextRenderer.DrawText(graphics, this.Text, font, textRectangle, this.DefaultCellStyle.ForeColor,
                                  TextFormatFlags.Left | TextFormatFlags.WordBreak | TextFormatFlags.VerticalCenter);

            Rectangle gbr = new Rectangle(rowBounds.Location.X + 1, rowBounds.Bottom - 3, 287, 1);
            LinearGradientBrush gb = new LinearGradientBrush(gbr, lineColor, Color.Transparent, LinearGradientMode.Horizontal);
            graphics.FillRectangle(gb, gbr);
        }
    }    
}
