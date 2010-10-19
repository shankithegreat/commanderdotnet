namespace Nomad.Controls
{
    using System;

    public abstract class LunaColorTable : ToolStripMapColorTable
    {
        protected LunaColorTable()
        {
        }

        public override KnownToolStripColor MapKnownColor(KnownToolStripColor knownColor)
        {
            switch (knownColor)
            {
                case KnownToolStripColor.ButtonPressedHighlightBorder:
                case KnownToolStripColor.ButtonCheckedHighlightBorder:
                    return KnownToolStripColor.ButtonPressedHighlight;

                case KnownToolStripColor.ButtonSelectedBorder:
                case KnownToolStripColor.ButtonSelectedHighlightBorder:
                    return KnownToolStripColor.ButtonPressedBorder;

                case KnownToolStripColor.ButtonSelectedGradientBegin:
                case KnownToolStripColor.ButtonSelectedGradientEnd:
                case KnownToolStripColor.ButtonSelectedGradientMiddle:
                case KnownToolStripColor.CheckBackground:
                case KnownToolStripColor.CheckPressedBackground:
                case KnownToolStripColor.GripDark:
                case KnownToolStripColor.GripLight:
                    return knownColor;

                case KnownToolStripColor.ButtonSelectedHighlight:
                    return KnownToolStripColor.ButtonCheckedHighlight;

                case KnownToolStripColor.CheckSelectedBackground:
                    return KnownToolStripColor.CheckPressedBackground;

                case KnownToolStripColor.ImageMarginGradientBegin:
                    return KnownToolStripColor.ToolStripGradientBegin;

                case KnownToolStripColor.ImageMarginGradientEnd:
                    return KnownToolStripColor.ToolStripGradientEnd;

                case KnownToolStripColor.ImageMarginGradientMiddle:
                    return KnownToolStripColor.ToolStripGradientMiddle;

                case KnownToolStripColor.MenuItemPressedGradientMiddle:
                    return KnownToolStripColor.ImageMarginRevealedGradientMiddle;

                case KnownToolStripColor.MenuItemSelected:
                    return knownColor;

                case KnownToolStripColor.MenuItemSelectedGradientBegin:
                    return KnownToolStripColor.ButtonSelectedGradientBegin;

                case KnownToolStripColor.MenuItemSelectedGradientEnd:
                    return KnownToolStripColor.ButtonSelectedGradientEnd;

                case KnownToolStripColor.RaftingContainerGradientBegin:
                case KnownToolStripColor.StatusStripGradientBegin:
                case KnownToolStripColor.ToolStripContentPanelGradientBegin:
                case KnownToolStripColor.ToolStripPanelGradientBegin:
                    return KnownToolStripColor.MenuStripGradientBegin;

                case KnownToolStripColor.RaftingContainerGradientEnd:
                case KnownToolStripColor.StatusStripGradientEnd:
                case KnownToolStripColor.ToolStripContentPanelGradientEnd:
                case KnownToolStripColor.ToolStripPanelGradientEnd:
                    return KnownToolStripColor.MenuStripGradientEnd;

                case KnownToolStripColor.SeparatorDark:
                case KnownToolStripColor.SeparatorLight:
                case KnownToolStripColor.ToolStripBorder:
                case KnownToolStripColor.ToolStripDropDownBackground:
                case KnownToolStripColor.ToolStripGradientBegin:
                case KnownToolStripColor.ToolStripGradientEnd:
                case KnownToolStripColor.ToolStripGradientMiddle:
                    return knownColor;
            }
            return knownColor;
        }
    }
}

