using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Commander
{
    public static class DataGridViewUtilities
    {
        public static DataGridViewFreeDimension GetFreeDimensionFromConstraint(Size constraintSize)
        {
            if (constraintSize.Width == 0)
            {
                if (constraintSize.Height == 0)
                {
                    return DataGridViewFreeDimension.Both;
                }
                return DataGridViewFreeDimension.Width;
            }
            return DataGridViewFreeDimension.Height;
        }

        public static TextFormatFlags ComputeTextFormatFlagsForCellStyleAlignment(bool rightToLeft, DataGridViewContentAlignment alignment, DataGridViewTriState wrapMode)
        {
            TextFormatFlags result;
            switch (alignment)
            {
                case DataGridViewContentAlignment.TopLeft:
                    {
                        result = TextFormatFlags.GlyphOverhangPadding;
                        if (rightToLeft)
                        {
                            result |= TextFormatFlags.Right;
                        }
                        break;
                    }
                case DataGridViewContentAlignment.TopCenter:
                    {
                        result = TextFormatFlags.HorizontalCenter;
                        break;
                    }
                case DataGridViewContentAlignment.TopRight:
                    {
                        result = TextFormatFlags.GlyphOverhangPadding;
                        if (!rightToLeft)
                        {
                            result |= TextFormatFlags.Right;
                        }
                        break;
                    }

                case DataGridViewContentAlignment.MiddleLeft:
                    {
                        result = TextFormatFlags.VerticalCenter;
                        if (rightToLeft)
                        {
                            result |= TextFormatFlags.Right;
                        }
                        break;
                    }
                case DataGridViewContentAlignment.MiddleCenter:
                    {
                        result = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                        break;
                    }

                case DataGridViewContentAlignment.BottomCenter:
                    {
                        result = TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                        break;
                    }
                case DataGridViewContentAlignment.BottomRight:
                    {
                        result = TextFormatFlags.Bottom;
                        if (!rightToLeft)
                        {
                            result |= TextFormatFlags.Right;
                        }
                        break;
                    }
                case DataGridViewContentAlignment.MiddleRight:
                    {
                        result = TextFormatFlags.VerticalCenter;
                        if (!rightToLeft)
                        {
                            result |= TextFormatFlags.Right;
                        }
                        break;
                    }
                case DataGridViewContentAlignment.BottomLeft:
                    {
                        result = TextFormatFlags.Bottom;
                        if (rightToLeft)
                        {
                            result |= TextFormatFlags.Right;
                        }
                        break;
                    }
                default:
                    {
                        result = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                        break;
                    }
            }

            if (wrapMode == DataGridViewTriState.False)
            {
                result |= TextFormatFlags.SingleLine;
            }
            else
            {
                result |= TextFormatFlags.WordBreak;
            }

            result |= TextFormatFlags.NoPrefix;
            result |= TextFormatFlags.PreserveGraphicsClipping;

            if (rightToLeft)
            {
                result |= TextFormatFlags.RightToLeft;
            }

            return result;
        }

        internal static Size GetAutoSize(DataGridView parent, Graphics graphics, string text, DataGridViewCellStyle cellStyle, Size constraintSize, Rectangle borders)
        {
            if (parent == null)
            {
                return new Size(-1, -1);
            }
            if (cellStyle == null)
            {
                throw new ArgumentNullException("cellStyle");
            }

            int horizontalPadding = (borders.Left + borders.Width) + cellStyle.Padding.Horizontal;
            int verticalPadding = (borders.Top + borders.Height) + cellStyle.Padding.Vertical;

            DataGridViewFreeDimension freeDimensionFromConstraint = GetFreeDimensionFromConstraint(constraintSize);
            TextFormatFlags flags = ComputeTextFormatFlagsForCellStyleAlignment(parent.RightToLeft == RightToLeft.Yes, cellStyle.Alignment, cellStyle.WrapMode);

            if (string.IsNullOrEmpty(text))
            {
                text = " ";
            }

            Size size;
            if ((cellStyle.WrapMode == DataGridViewTriState.True) && (text.Length > 1))
            {
                switch (freeDimensionFromConstraint)
                {
                    case DataGridViewFreeDimension.Height:
                        {
                            size = new Size(0, DataGridViewCell.MeasureTextHeight(graphics, text, cellStyle.Font, Math.Max(1, constraintSize.Width - horizontalPadding), flags));
                            break;
                        }
                    case DataGridViewFreeDimension.Width:
                        {
                            size = new Size(DataGridViewCell.MeasureTextWidth(graphics, text, cellStyle.Font, Math.Max(1, ((constraintSize.Height - verticalPadding) - 1) - 1), flags), 0);
                            break;
                        }
                    default:
                        {
                            size = DataGridViewCell.MeasureTextPreferredSize(graphics, text, cellStyle.Font, 5f, flags);
                            break;
                        }
                }
            }
            else
            {
                switch (freeDimensionFromConstraint)
                {
                    case DataGridViewFreeDimension.Height:
                        {
                            size = new Size(0, DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags).Height);
                            break;
                        }

                    case DataGridViewFreeDimension.Width:
                        {
                            size = new Size(DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags).Width, 0);
                            break;
                        }
                    default:
                        {
                            size = DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags);
                            break;
                        }
                }
            }

            if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
            {
                size.Width += horizontalPadding;
                if (parent.ShowCellErrors)
                {
                    size.Width = Math.Max(size.Width, (horizontalPadding + 8) + 12);
                }
            }
            if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
            {
                int num = (cellStyle.WrapMode == DataGridViewTriState.True ? 1 : 2);
                size.Height += (num + 1) + verticalPadding;
                if (parent.ShowCellErrors)
                {
                    size.Height = Math.Max(size.Height, (verticalPadding + 8) + 11);
                }
            }

            return size;
        }
    }

    public enum DataGridViewFreeDimension
    {
        Both,
        Height,
        Width
    }
}
