using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Commander
{
    public class DatagridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        private Rectangle checkBox;
        private Rectangle checkRectangle;
        private bool check;
        private bool mouseInCheckBox;
        private ButtonState checkBoxState = ButtonState.Normal;


        public bool Check
        {
            get
            {
                return check;
            }
            set
            {
                if (this.check != value)
                {
                    this.check = value;
                    if (this.DataGridView != null)
                    {
                        this.DataGridView.InvalidateCell(this);
                    }
                }
            }
        }


        public event CheckBoxClickEventHandler CheckBoxClick;
        

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            this.checkBox.Size = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal);
            this.checkBox.Location = new Point(2, cellBounds.Height / 2 - this.checkBox.Height / 2);
            this.checkRectangle.Size = new Size(this.checkBox.Right + 1, clipBounds.Height);

            Point realLocation = this.checkBox.Location + (Size)cellBounds.Location;
            ButtonState state = GetCheckBoxState();

            Padding newPadding = cellStyle.Padding;
            newPadding.Left = checkRectangle.Width;
            cellStyle.Padding = newPadding;

            GraphicsState gstate = graphics.Save();
            graphics.ExcludeClip(new Rectangle(realLocation, this.checkBox.Size));
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            graphics.Restore(gstate);

            CheckBoxRenderer.DrawCheckBox(graphics, realLocation, ConvertFromButtonState(state, false, this.mouseInCheckBox));
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (MouseInCheckBox(e))
            {
                OnCheckBoxClick();
            }
            else
            {
                base.OnMouseClick(e);
            }
        }

        protected override void OnMouseDoubleClick(DataGridViewCellMouseEventArgs e)
        {
            if (MouseInCheckBox(e))
            {
                OnCheckBoxClick();
            }
            else
            {
                base.OnMouseDoubleClick(e);
            }
        }

        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            if (base.DataGridView != null)
            {
                if (((e.ColumnIndex == this.ColumnIndex) && (e.RowIndex == this.RowIndex)))
                {
                    this.mouseInCheckBox = MouseInCheckBox(e);
                    base.DataGridView.InvalidateCell(base.ColumnIndex, e.RowIndex);
                }
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            if (base.DataGridView != null)
            {
                if (mouseInCheckBox)
                {
                    mouseInCheckBox = false;
                    if (((base.ColumnIndex >= 0) && (rowIndex >= 0)))
                    {
                        base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
                    }
                }
            }

            base.OnMouseLeave(rowIndex);
        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if ((base.DataGridView != null) && ((e.Button == MouseButtons.Left) && mouseInCheckBox))
            {
                this.UpdateCheckBoxState(this.checkBoxState | ButtonState.Pushed, e.RowIndex);
            }
            else
            {
                base.OnMouseDown(e);
            }
        }

        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            if ((base.DataGridView != null) && (e.Button == MouseButtons.Left))
            {
                this.UpdateCheckBoxState(this.checkBoxState & ~ButtonState.Pushed, e.RowIndex);
            }

            base.OnMouseUp(e);
        }

        protected virtual void OnCheckBoxClick(bool check)
        {
            if (CheckBoxClick != null)
            {
                CheckBoxClick(check);

                this.DataGridView.InvalidateCell(this);
            }
        }


        private static CheckBoxState ConvertFromButtonState(ButtonState state, bool isMixed, bool isHot)
        {
            if (isMixed)
            {
                if ((state & ButtonState.Pushed) == ButtonState.Pushed)
                {
                    return CheckBoxState.MixedPressed;
                }
                if ((state & ButtonState.Inactive) == ButtonState.Inactive)
                {
                    return CheckBoxState.MixedDisabled;
                }
                if (isHot)
                {
                    return CheckBoxState.MixedHot;
                }
                return CheckBoxState.MixedNormal;
            }
            if ((state & ButtonState.Checked) == ButtonState.Checked)
            {
                if ((state & ButtonState.Pushed) == ButtonState.Pushed)
                {
                    return CheckBoxState.CheckedPressed;
                }
                if ((state & ButtonState.Inactive) == ButtonState.Inactive)
                {
                    return CheckBoxState.CheckedDisabled;
                }
                if (isHot)
                {
                    return CheckBoxState.CheckedHot;
                }
                return CheckBoxState.CheckedNormal;
            }
            if ((state & ButtonState.Pushed) == ButtonState.Pushed)
            {
                return CheckBoxState.UncheckedPressed;
            }
            if ((state & ButtonState.Inactive) == ButtonState.Inactive)
            {
                return CheckBoxState.UncheckedDisabled;
            }
            if (isHot)
            {
                return CheckBoxState.UncheckedHot;
            }
            return CheckBoxState.UncheckedNormal;
        }

        private bool MouseInCheckBox(DataGridViewCellMouseEventArgs e)
        {
            return (this.checkRectangle.Contains(e.Location));
        }

        private void OnCheckBoxClick()
        {
            this.check = !this.check;
            this.DataGridView.InvalidateCell(this);

            OnCheckBoxClick(this.check);
        }

        private void UpdateCheckBoxState(ButtonState newState, int rowIndex)
        {
            this.checkBoxState = newState;
            base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
        }

        private ButtonState GetCheckBoxState()
        {
            if (this.check)
            {
                return this.checkBoxState | ButtonState.Checked;
            }
            else
            {
                return this.checkBoxState & ~ButtonState.Checked;
            }
        }

    }

    public delegate void CheckBoxClickEventHandler(bool check);
}
