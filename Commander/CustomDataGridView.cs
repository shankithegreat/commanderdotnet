using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Commander
{
    [Flags]
    public enum CustomCellBorderStyle
    {
        None = 0,
        Top = 1,
        Bottom = 2
    }

    public class CustomDataGridView : DataGridView
    {
        private bool hideSelection = true;
        private Pen gridPen = new Pen(Color.Black);

        public CustomDataGridView()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the selected item in the control 
        /// remains highlighted when the control loses focus.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]        
        public bool HideSelection
        {
            get
            {
                return hideSelection;
            }
            set
            {
                hideSelection = value;
            }
        }

        /// <summary>
        /// Adds a new row to the collection, and populates the cells with the specified objects.
        /// </summary>
        /// <param name="list">A variable number of objects that populate the cells of the new DataGridViewRow.</param>
        /// <returns>The instance of the new row.</returns>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewRow AddRow(params object[] list)
        {
            int index = Rows.Add(list);
            return Rows[index];
        }

        /// <summary>
        /// Adds a new top separated row to the collection, and populates the cells with the specified objects.
        /// </summary>
        /// <param name="list">A variable number of objects that populate the cells of the new DataGridViewRow.</param>
        /// <returns>The instance of the new row.</returns>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewRow AddTopSeparatedRow(params object[] list)
        {
            int index = Rows.Add(list);
            DataGridViewRow row = Rows[index];
            SetSeparator(row, CustomCellBorderStyle.Top);
            return row;
        }

        /// <summary>
        /// Adds a new bottom separated row to the collection, and populates the cells with the specified objects.
        /// </summary>
        /// <param name="list">A variable number of objects that populate the cells of the new DataGridViewRow.</param>
        /// <returns>The instance of the new row.</returns>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewRow AddBottomSeparatedRow(params object[] list)
        {
            int index = Rows.Add(list);
            DataGridViewRow row = Rows[index];
            SetSeparator(row, CustomCellBorderStyle.Bottom);
            return row;
        }

        public void SetSeparator(int index)
        {
            SetSeparator(index, CustomCellBorderStyle.Top);
        }

        public void SetSeparator(int index, CustomCellBorderStyle separator)
        {
            SetSeparator(Rows[index], separator);
        }


        public void SetSeparator(DataGridViewRow row)
        {
            SetSeparator(row, CustomCellBorderStyle.Top);
        }

        public void SetSeparator(DataGridViewRow row, CustomCellBorderStyle separator)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell is DataGridViewBorderedTextBoxCell)
                {
                    ((DataGridViewBorderedTextBoxCell)cell).Border = separator;
                }
            }
        }


        protected override void PaintBackground(Graphics graphics, Rectangle clipBounds, Rectangle gridBounds)
        {
            base.PaintBackground(graphics, clipBounds, gridBounds);  
          
            this.gridPen.Color = this.GridColor;

            if (ColumnHeadersVisible)
            {
                graphics.DrawLine(gridPen, 0, ColumnHeadersHeight, this.ClientRectangle.Width, ColumnHeadersHeight);
            }
            foreach (DataGridViewRow row in Rows)
            {
                if (row.Visible)
                {
                    Rectangle r = this.GetRowDisplayRectangle(row.Index, false);
                    graphics.DrawLine(gridPen, 0, r.Bottom - 1, r.Width, r.Bottom - 1);
                }
            }
            int yOffset = Rows.GetRowsHeight(DataGridViewElementStates.Visible);
            if (ColumnHeadersVisible)
            {
                yOffset += ColumnHeadersHeight;
            }
            for (int y = yOffset + RowTemplate.Height; y < this.ClientRectangle.Height; y += RowTemplate.Height)
            {
                graphics.DrawLine(gridPen, 0, y, this.ClientRectangle.Width, y);
            }


            int x = -HorizontalScrollBar.Value;
            if (RowHeadersVisible)
            {
                x += RowHeadersWidth;
                graphics.DrawLine(gridPen, RowHeadersWidth, 0, RowHeadersWidth, this.ClientRectangle.Height);
            }
            foreach (DataGridViewColumn column in this.Columns)
            {
                x += column.Width;
                graphics.DrawLine(gridPen, x + 1, 0, x + 1, this.ClientRectangle.Height);
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (hideSelection)
            {
                this.ClearSelection();
            }
        }
    }

    public class DataGridViewBorderedTextBoxCell : DataGridViewTextBoxCell
    {
        private CustomCellBorderStyle border = CustomCellBorderStyle.None;
        private Pen borderPen = new Pen(Color.Black, 1);


        public CustomCellBorderStyle Border
        {
            get
            {
                return border;
            }
            set
            {
                border = value;
                if (this.DataGridView != null)
                {
                    this.DataGridView.InvalidateCell(this);
                }
            }
        }

        public Color BorderColor
        {
            get
            {
                return borderPen.Color;
            }
            set
            {
                borderPen.Color = value;
                if (this.DataGridView != null)
                {
                    this.DataGridView.InvalidateCell(this);
                }
            }
        }


        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            PaintAdvancedBorder(graphics, clipBounds, cellBounds);
        }

        protected virtual void PaintAdvancedBorder(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds)
        {
            // Top Border
            if ((border & CustomCellBorderStyle.Top) == CustomCellBorderStyle.Top)
            {
                graphics.DrawLine(borderPen, cellBounds.Left, cellBounds.Top - borderPen.Width, cellBounds.Right, cellBounds.Top - borderPen.Width);
            }
            // Bottom Border
            if ((border & CustomCellBorderStyle.Bottom) == CustomCellBorderStyle.Bottom)
            {
                graphics.DrawLine(borderPen, cellBounds.Left, cellBounds.Bottom - borderPen.Width, cellBounds.Right, cellBounds.Bottom - borderPen.Width);
            }
        }
    }
    
    public class DataGridViewBorderedTextBoxColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewBorderedTextBoxColumn()
        {
            this.CellTemplate = new DataGridViewBorderedTextBoxCell();
        }
    }


   
}
