using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Commander
{
    public class DataGridViewAutoSizeCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        public DataGridViewAutoSizeCheckBoxColumn()
        {
            this.CellTemplate = new DataGridViewAutoSizeCheckBoxCell();
        }
    }

    public class DataGridViewAutoSizeTextBoxColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewAutoSizeTextBoxColumn()
        {
            this.CellTemplate = new DataGridViewAutoSizeTextBoxCell();
        }
    }

    public class DataGridViewAutoSizeTextBoxCell : DataGridViewTextBoxCell
    {
        private Rectangle borders = new Rectangle(1, 1, 2, 1);


        public DataGridViewAutoSizeTextBoxCell()
        {
            this.Style.WrapMode = DataGridViewTriState.True;
        }


        protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            if (rowIndex == this.RowIndex)
            {
                return DataGridViewUtilities.GetAutoSize(base.DataGridView, graphics, (this.Value as string), cellStyle, constraintSize, borders);
            }

            return base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
        }
    }

    public class DataGridViewAutoSizeCheckBoxCell : DataGridViewCheckBoxCell
    {
        private string text = string.Empty;
        private Rectangle borders = new Rectangle(20, 1, 2, 1);


        public DataGridViewAutoSizeCheckBoxCell()
        {
            this.Style.WrapMode = DataGridViewTriState.True;
            this.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.Value = false;
        }


        public String Text
        {
            get
            {
                return text;
            }
            set
            {
                this.text = value;

                if (this.DataGridView != null)
                {
                    base.RaiseCellValueChanged(new DataGridViewCellEventArgs(this.ColumnIndex, this.RowIndex));
                }
            }
        }


        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            int horizontalPadding = (borders.Left + borders.Width) + cellStyle.Padding.Horizontal;
            int verticalPadding = (borders.Top + borders.Height) + cellStyle.Padding.Vertical;

            cellBounds.X += borders.Left;
            cellBounds.Y += borders.Top;
            cellBounds.Width -= horizontalPadding;
            cellBounds.Height -= verticalPadding;

            int y = (cellStyle.WrapMode == DataGridViewTriState.True ? 1 : 2);
            cellBounds.Offset(0, y);
            cellBounds.Width = cellBounds.Width;
            cellBounds.Height -= y + 1;

            TextFormatFlags flags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeft == RightToLeft.Yes, cellStyle.Alignment, cellStyle.WrapMode);
            Color color = ((elementState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None ? cellStyle.SelectionForeColor : cellStyle.ForeColor);

            TextRenderer.DrawText(graphics, this.Text, cellStyle.Font, cellBounds, color, flags);
        }

        protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
        {
            if (((base.DataGridView == null) || (rowIndex < 0)) || (base.OwningColumn == null))
            {
                return Rectangle.Empty;
            }

            return new Rectangle(Point.Empty, this.Size);
        }

        protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            if (rowIndex == this.RowIndex)
            {
                return DataGridViewUtilities.GetAutoSize(base.DataGridView, graphics, this.Text, cellStyle, constraintSize, borders);
            }

            return base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
        }

        protected override void OnContentClick(DataGridViewCellEventArgs e)
        {
            if (base.DataGridView != null)
            {
                if (((e.ColumnIndex == this.ColumnIndex) && (e.RowIndex == this.RowIndex)) && base.DataGridView.IsCurrentCellInEditMode)
                {
                    this.Value = !(bool)this.Value;
                }
            }

            base.OnContentClick(e);
        }

        protected override void OnContentDoubleClick(DataGridViewCellEventArgs e)
        {
            if (base.DataGridView != null)
            {
                if (((e.ColumnIndex == this.ColumnIndex) && (e.RowIndex == this.RowIndex)) && base.DataGridView.IsCurrentCellInEditMode)
                {
                    this.Value = !(bool)this.Value;
                }
            }

            base.OnContentDoubleClick(e);
        }

    }

    public class CustomDataGridViewImageColumn : DataGridViewImageColumn
    {
        public CustomDataGridViewImageColumn()
        {
            this.CellTemplate = new CustomDataGridViewImageCell();
            this.ImageWidth = 32;
        }

        public int ImageWidth { get; set; }
    }

    public class CustomDataGridViewImageCell : DataGridViewImageCell
    {
        private string text = string.Empty;
        private Image image;
        private static Rectangle defaultBorders = new Rectangle(1, 1, 2, 1);
        private Rectangle borders = new Rectangle(1, 1, 2, 1);

        private int GetImageWidth(object value)
        {
            if (value != null)
            {
                if (value is Image)
                {
                    return ((Image)value).Width;
                }

                if (value is DataItem)
                {
                    return ((DataItem)value).Image.Width;
                }
            }

            return 0;
        }

        private Rectangle GetBorders(object value)
        {
            Rectangle result = defaultBorders;
            result.X += GetImageWidth(value);
            return result;
        }

        private Rectangle GetBorders(Image image)
        {
            if (image != this.image && image != null)
            {
                this.image = image;

                Rectangle result = defaultBorders;
                result.X += image.Width;
                this.borders = result;
            }

            return borders;
        }

        public CustomDataGridViewImageCell()
        {
            this.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }


        public String Text
        {
            get
            {
                return text;
            }
            set
            {
                this.text = value;

                if (this.DataGridView != null)
                {
                    base.RaiseCellValueChanged(new DataGridViewCellEventArgs(this.ColumnIndex, this.RowIndex));
                }
            }
        }



        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            Rectangle borders = GetBorders((Image)formattedValue);
            int horizontalPadding = (borders.Left + borders.Width) + cellStyle.Padding.Horizontal;
            int verticalPadding = (borders.Top + borders.Height) + cellStyle.Padding.Vertical;

            cellBounds.X += borders.Left;
            cellBounds.Y += borders.Top;
            cellBounds.Width -= horizontalPadding;
            cellBounds.Height -= verticalPadding;

            int y = (cellStyle.WrapMode == DataGridViewTriState.True ? 1 : 2);
            cellBounds.Offset(0, y);
            cellBounds.Width = cellBounds.Width;
            cellBounds.Height -= y + 1;

            TextFormatFlags flags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeft == RightToLeft.Yes, cellStyle.Alignment, cellStyle.WrapMode);
            Color color = ((elementState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None ? cellStyle.SelectionForeColor : cellStyle.ForeColor);

            TextRenderer.DrawText(graphics, this.Text, cellStyle.Font, cellBounds, color, flags);
        }

        protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            if (rowIndex == this.RowIndex)
            {
                return DataGridViewUtilities.GetAutoSize(base.DataGridView, graphics, this.Text, cellStyle, constraintSize, borders);
            }

            return base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
        }

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            if (value is DataItem)
            {
                DataItem item = (DataItem)value;

                this.text = item.Text;
                return base.GetFormattedValue(item.Image, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
            }

            return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
        }
    }


    public class DataItem
    {
        public DataItem(Image image, string text)
        {
            this.Image = image;
            this.Text = text;
        }

        public Image Image { get; set; }
        public string Text { get; set; }
    }

}
