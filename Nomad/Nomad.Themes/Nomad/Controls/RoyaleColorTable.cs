namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class RoyaleColorTable : LunaColorTable
    {
        public static readonly ProfessionalColorTable Default = new RoyaleColorTable();

        protected override void InitializeMap(Dictionary<KnownToolStripColor, Color> colorMap)
        {
            colorMap.Add(KnownToolStripColor.ButtonCheckedGradientBegin, Color.FromArgb(0xe2, 0xe5, 0xee));
            colorMap.Add(KnownToolStripColor.ButtonPressedBorder, Color.FromArgb(0x33, 0x5e, 0xa8));
            colorMap.Add(KnownToolStripColor.ButtonPressedGradientBegin, Color.FromArgb(0x99, 0xaf, 0xd4));
            colorMap.Add(KnownToolStripColor.ButtonSelectedGradientEnd, Color.FromArgb(0xc2, 0xcf, 0xe5));
            colorMap.Add(KnownToolStripColor.CheckBackground, Color.FromArgb(0xe2, 0xe5, 0xee));
            colorMap.Add(KnownToolStripColor.CheckPressedBackground, Color.FromArgb(0x33, 0x5e, 0xa8));
            colorMap.Add(KnownToolStripColor.GripDark, Color.FromArgb(0xbd, 0xbc, 0xbf));
            colorMap.Add(KnownToolStripColor.GripLight, Color.White);
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientBegin, Color.FromArgb(0xf7, 0xf6, 0xf8));
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientMiddle, Color.FromArgb(0xf1, 240, 0xf2));
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientEnd, Color.FromArgb(0xe4, 0xe2, 230));
            colorMap.Add(KnownToolStripColor.MenuBorder, Color.FromArgb(0x86, 0x85, 0x88));
            colorMap.Add(KnownToolStripColor.MenuItemBorder, Color.FromArgb(0x33, 0x5e, 0xa8));
            colorMap.Add(KnownToolStripColor.MenuItemPressedGradientBegin, Color.FromArgb(0xfc, 0xfc, 0xfc));
            colorMap.Add(KnownToolStripColor.MenuItemPressedGradientEnd, Color.FromArgb(0xf5, 0xf4, 0xf6));
            colorMap.Add(KnownToolStripColor.MenuItemSelected, Color.FromArgb(0xc2, 0xcf, 0xe5));
            colorMap.Add(KnownToolStripColor.MenuStripGradientBegin, Color.FromArgb(0xeb, 0xe9, 0xed));
            colorMap.Add(KnownToolStripColor.MenuStripGradientEnd, Color.FromArgb(0xfb, 250, 0xfb));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientBegin, Color.FromArgb(0xf2, 0xf2, 0xf2));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientMiddle, Color.FromArgb(0xe0, 0xe0, 0xe1));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientEnd, Color.FromArgb(0xa7, 0xa6, 170));
            colorMap.Add(KnownToolStripColor.SeparatorDark, Color.FromArgb(0xc1, 0xc1, 0xc4));
            colorMap.Add(KnownToolStripColor.SeparatorLight, Color.White);
            colorMap.Add(KnownToolStripColor.ToolStripBorder, Color.FromArgb(0xee, 0xed, 240));
            colorMap.Add(KnownToolStripColor.ToolStripDropDownBackground, Color.FromArgb(0xfc, 0xfc, 0xfc));
            colorMap.Add(KnownToolStripColor.ToolStripGradientBegin, Color.FromArgb(0xfc, 0xfc, 0xfc));
            colorMap.Add(KnownToolStripColor.ToolStripGradientMiddle, Color.FromArgb(0xf5, 0xf4, 0xf6));
            colorMap.Add(KnownToolStripColor.ToolStripGradientEnd, Color.FromArgb(0xeb, 0xe9, 0xed));
            colorMap.Add(KnownToolStripColor.ButtonPressedHighlight, Color.FromArgb(0x33, 0x5e, 0xa8));
            colorMap.Add(KnownToolStripColor.ButtonCheckedHighlight, Color.FromArgb(220, 0xdf, 0xe4));
        }

        public override KnownToolStripColor MapKnownColor(KnownToolStripColor knownColor)
        {
            switch (knownColor)
            {
                case KnownToolStripColor.ButtonCheckedGradientEnd:
                case KnownToolStripColor.ButtonCheckedGradientMiddle:
                    return KnownToolStripColor.ButtonCheckedGradientBegin;

                case KnownToolStripColor.ButtonPressedGradientEnd:
                case KnownToolStripColor.ButtonPressedGradientMiddle:
                    return KnownToolStripColor.ButtonPressedGradientBegin;

                case KnownToolStripColor.ButtonSelectedGradientBegin:
                    return KnownToolStripColor.ButtonCheckedGradientBegin;

                case KnownToolStripColor.ButtonSelectedGradientMiddle:
                    return KnownToolStripColor.ButtonSelectedGradientEnd;
            }
            return base.MapKnownColor(knownColor);
        }
    }
}

