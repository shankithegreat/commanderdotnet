namespace Nomad.Controls
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ToolStripColorTable : ProfessionalColorTable
    {
        public virtual Color FromKnownColor(KnownToolStripColor knownColor)
        {
            Color color = this.GetKnownColor(this.MapKnownColor(knownColor));
            if (color == Color.Empty)
            {
                throw new InvalidEnumArgumentException();
            }
            return color;
        }

        protected virtual Color GetKnownColor(KnownToolStripColor knownColor)
        {
            switch (knownColor)
            {
                case KnownToolStripColor.ButtonCheckedGradientBegin:
                    return base.ButtonCheckedGradientBegin;

                case KnownToolStripColor.ButtonCheckedGradientEnd:
                    return base.ButtonCheckedGradientEnd;

                case KnownToolStripColor.ButtonCheckedGradientMiddle:
                    return base.ButtonCheckedGradientMiddle;

                case KnownToolStripColor.ButtonCheckedHighlight:
                    return base.ButtonCheckedHighlight;

                case KnownToolStripColor.ButtonCheckedHighlightBorder:
                    return base.ButtonCheckedHighlightBorder;

                case KnownToolStripColor.ButtonPressedBorder:
                    return base.ButtonPressedBorder;

                case KnownToolStripColor.ButtonPressedGradientBegin:
                    return base.ButtonPressedGradientBegin;

                case KnownToolStripColor.ButtonPressedGradientEnd:
                    return base.ButtonPressedGradientEnd;

                case KnownToolStripColor.ButtonPressedGradientMiddle:
                    return base.ButtonPressedGradientMiddle;

                case KnownToolStripColor.ButtonPressedHighlight:
                    return base.ButtonPressedHighlight;

                case KnownToolStripColor.ButtonPressedHighlightBorder:
                    return base.ButtonPressedHighlightBorder;

                case KnownToolStripColor.ButtonSelectedBorder:
                    return base.ButtonSelectedBorder;

                case KnownToolStripColor.ButtonSelectedGradientBegin:
                    return base.ButtonSelectedGradientBegin;

                case KnownToolStripColor.ButtonSelectedGradientEnd:
                    return base.ButtonSelectedGradientEnd;

                case KnownToolStripColor.ButtonSelectedGradientMiddle:
                    return base.ButtonSelectedGradientMiddle;

                case KnownToolStripColor.ButtonSelectedHighlight:
                    return base.ButtonSelectedHighlight;

                case KnownToolStripColor.ButtonSelectedHighlightBorder:
                    return base.ButtonSelectedHighlightBorder;

                case KnownToolStripColor.CheckBackground:
                    return base.CheckBackground;

                case KnownToolStripColor.CheckPressedBackground:
                    return base.CheckPressedBackground;

                case KnownToolStripColor.CheckSelectedBackground:
                    return base.CheckSelectedBackground;

                case KnownToolStripColor.GripDark:
                    return base.GripDark;

                case KnownToolStripColor.GripLight:
                    return base.GripLight;

                case KnownToolStripColor.ImageMarginGradientBegin:
                    return base.ImageMarginGradientBegin;

                case KnownToolStripColor.ImageMarginGradientEnd:
                    return base.ImageMarginGradientEnd;

                case KnownToolStripColor.ImageMarginGradientMiddle:
                    return base.ImageMarginGradientMiddle;

                case KnownToolStripColor.ImageMarginRevealedGradientBegin:
                    return base.ImageMarginRevealedGradientBegin;

                case KnownToolStripColor.ImageMarginRevealedGradientEnd:
                    return base.ImageMarginRevealedGradientEnd;

                case KnownToolStripColor.ImageMarginRevealedGradientMiddle:
                    return base.ImageMarginRevealedGradientMiddle;

                case KnownToolStripColor.MenuBorder:
                    return base.MenuBorder;

                case KnownToolStripColor.MenuItemBorder:
                    return base.MenuItemBorder;

                case KnownToolStripColor.MenuItemPressedGradientBegin:
                    return base.MenuItemPressedGradientBegin;

                case KnownToolStripColor.MenuItemPressedGradientEnd:
                    return base.MenuItemPressedGradientEnd;

                case KnownToolStripColor.MenuItemPressedGradientMiddle:
                    return base.MenuItemPressedGradientMiddle;

                case KnownToolStripColor.MenuItemSelected:
                    return base.MenuItemSelected;

                case KnownToolStripColor.MenuItemSelectedGradientBegin:
                    return base.MenuItemSelectedGradientBegin;

                case KnownToolStripColor.MenuItemSelectedGradientEnd:
                    return base.MenuItemSelectedGradientEnd;

                case KnownToolStripColor.MenuStripGradientBegin:
                    return base.MenuStripGradientBegin;

                case KnownToolStripColor.MenuStripGradientEnd:
                    return base.MenuStripGradientEnd;

                case KnownToolStripColor.OverflowButtonGradientBegin:
                    return base.OverflowButtonGradientBegin;

                case KnownToolStripColor.OverflowButtonGradientEnd:
                    return base.OverflowButtonGradientEnd;

                case KnownToolStripColor.OverflowButtonGradientMiddle:
                    return base.OverflowButtonGradientMiddle;

                case KnownToolStripColor.RaftingContainerGradientBegin:
                    return base.RaftingContainerGradientBegin;

                case KnownToolStripColor.RaftingContainerGradientEnd:
                    return base.RaftingContainerGradientEnd;

                case KnownToolStripColor.SeparatorDark:
                    return base.SeparatorDark;

                case KnownToolStripColor.SeparatorLight:
                    return base.SeparatorLight;

                case KnownToolStripColor.StatusStripGradientBegin:
                    return base.StatusStripGradientBegin;

                case KnownToolStripColor.StatusStripGradientEnd:
                    return base.StatusStripGradientEnd;

                case KnownToolStripColor.ToolStripBorder:
                    return base.ToolStripBorder;

                case KnownToolStripColor.ToolStripContentPanelGradientBegin:
                    return base.ToolStripContentPanelGradientBegin;

                case KnownToolStripColor.ToolStripContentPanelGradientEnd:
                    return base.ToolStripContentPanelGradientEnd;

                case KnownToolStripColor.ToolStripDropDownBackground:
                    return base.ToolStripDropDownBackground;

                case KnownToolStripColor.ToolStripGradientBegin:
                    return base.ToolStripGradientBegin;

                case KnownToolStripColor.ToolStripGradientEnd:
                    return base.ToolStripGradientEnd;

                case KnownToolStripColor.ToolStripGradientMiddle:
                    return base.ToolStripGradientMiddle;

                case KnownToolStripColor.ToolStripPanelGradientBegin:
                    return base.ToolStripPanelGradientBegin;

                case KnownToolStripColor.ToolStripPanelGradientEnd:
                    return base.ToolStripPanelGradientEnd;
            }
            return Color.Empty;
        }

        public virtual KnownToolStripColor MapKnownColor(KnownToolStripColor knownColor)
        {
            return knownColor;
        }

        public override Color ButtonCheckedGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonCheckedGradientBegin);
            }
        }

        public override Color ButtonCheckedGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonCheckedGradientBegin);
            }
        }

        public override Color ButtonCheckedGradientMiddle
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonCheckedGradientMiddle);
            }
        }

        public override Color ButtonCheckedHighlight
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonCheckedHighlight);
            }
        }

        public override Color ButtonCheckedHighlightBorder
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonCheckedHighlightBorder);
            }
        }

        public override Color ButtonPressedBorder
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonPressedBorder);
            }
        }

        public override Color ButtonPressedGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonPressedGradientBegin);
            }
        }

        public override Color ButtonPressedGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonPressedGradientEnd);
            }
        }

        public override Color ButtonPressedGradientMiddle
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonPressedGradientMiddle);
            }
        }

        public override Color ButtonPressedHighlight
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonPressedHighlight);
            }
        }

        public override Color ButtonPressedHighlightBorder
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonPressedHighlightBorder);
            }
        }

        public override Color ButtonSelectedBorder
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonSelectedBorder);
            }
        }

        public override Color ButtonSelectedGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonSelectedGradientBegin);
            }
        }

        public override Color ButtonSelectedGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonSelectedGradientEnd);
            }
        }

        public override Color ButtonSelectedGradientMiddle
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonSelectedGradientMiddle);
            }
        }

        public override Color ButtonSelectedHighlight
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonSelectedHighlight);
            }
        }

        public override Color ButtonSelectedHighlightBorder
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ButtonSelectedHighlightBorder);
            }
        }

        public override Color CheckBackground
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.CheckBackground);
            }
        }

        public override Color CheckPressedBackground
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.CheckPressedBackground);
            }
        }

        public override Color CheckSelectedBackground
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.CheckSelectedBackground);
            }
        }

        public override Color GripDark
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.GripDark);
            }
        }

        public override Color GripLight
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.GripLight);
            }
        }

        public override Color ImageMarginGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ImageMarginGradientBegin);
            }
        }

        public override Color ImageMarginGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ImageMarginGradientEnd);
            }
        }

        public override Color ImageMarginGradientMiddle
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ImageMarginGradientMiddle);
            }
        }

        public override Color ImageMarginRevealedGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ImageMarginRevealedGradientBegin);
            }
        }

        public override Color ImageMarginRevealedGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ImageMarginRevealedGradientEnd);
            }
        }

        public override Color ImageMarginRevealedGradientMiddle
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ImageMarginRevealedGradientMiddle);
            }
        }

        public override Color MenuBorder
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.MenuBorder);
            }
        }

        public override Color MenuItemBorder
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.MenuItemBorder);
            }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.MenuItemPressedGradientBegin);
            }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.MenuItemPressedGradientEnd);
            }
        }

        public override Color MenuItemPressedGradientMiddle
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.MenuItemPressedGradientMiddle);
            }
        }

        public override Color MenuItemSelected
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.MenuItemSelected);
            }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.MenuItemSelectedGradientBegin);
            }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.MenuItemSelectedGradientEnd);
            }
        }

        public override Color MenuStripGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.MenuStripGradientBegin);
            }
        }

        public override Color MenuStripGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.MenuStripGradientEnd);
            }
        }

        public override Color OverflowButtonGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.OverflowButtonGradientBegin);
            }
        }

        public override Color OverflowButtonGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.OverflowButtonGradientEnd);
            }
        }

        public override Color OverflowButtonGradientMiddle
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.OverflowButtonGradientMiddle);
            }
        }

        public override Color RaftingContainerGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.RaftingContainerGradientBegin);
            }
        }

        public override Color RaftingContainerGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.RaftingContainerGradientEnd);
            }
        }

        public override Color SeparatorDark
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.SeparatorDark);
            }
        }

        public override Color SeparatorLight
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.SeparatorLight);
            }
        }

        public override Color StatusStripGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.StatusStripGradientBegin);
            }
        }

        public override Color StatusStripGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.StatusStripGradientEnd);
            }
        }

        public override Color ToolStripBorder
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ToolStripBorder);
            }
        }

        public override Color ToolStripContentPanelGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ToolStripContentPanelGradientBegin);
            }
        }

        public override Color ToolStripContentPanelGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ToolStripContentPanelGradientEnd);
            }
        }

        public override Color ToolStripDropDownBackground
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ToolStripDropDownBackground);
            }
        }

        public override Color ToolStripGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ToolStripGradientBegin);
            }
        }

        public override Color ToolStripGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ToolStripGradientEnd);
            }
        }

        public override Color ToolStripGradientMiddle
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ToolStripGradientMiddle);
            }
        }

        public override Color ToolStripPanelGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ToolStripPanelGradientBegin);
            }
        }

        public override Color ToolStripPanelGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownToolStripColor.ToolStripPanelGradientEnd);
            }
        }
    }
}

