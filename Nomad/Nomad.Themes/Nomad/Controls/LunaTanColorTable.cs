namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class LunaTanColorTable : LunaColorTable
    {
        public static readonly ProfessionalColorTable Default = new LunaTanColorTable();

        protected override void InitializeMap(Dictionary<KnownToolStripColor, Color> colorMap)
        {
            colorMap.Add(KnownToolStripColor.GripDark, Color.FromArgb(0xc1, 190, 0xb3));
            colorMap.Add(KnownToolStripColor.SeparatorDark, Color.FromArgb(0xc5, 0xc2, 0xb8));
            colorMap.Add(KnownToolStripColor.MenuItemSelected, Color.FromArgb(0xc1, 210, 0xee));
            colorMap.Add(KnownToolStripColor.ButtonPressedBorder, Color.FromArgb(0x31, 0x6a, 0xc5));
            colorMap.Add(KnownToolStripColor.CheckBackground, Color.FromArgb(0xe1, 230, 0xe8));
            colorMap.Add(KnownToolStripColor.MenuItemBorder, Color.FromArgb(0x31, 0x6a, 0xc5));
            colorMap.Add(KnownToolStripColor.CheckPressedBackground, Color.FromArgb(0x31, 0x6a, 0xc5));
            colorMap.Add(KnownToolStripColor.ToolStripDropDownBackground, Color.FromArgb(0xfc, 0xfc, 0xf9));
            colorMap.Add(KnownToolStripColor.MenuBorder, Color.FromArgb(0x8a, 0x86, 0x7a));
            colorMap.Add(KnownToolStripColor.SeparatorLight, Color.White);
            colorMap.Add(KnownToolStripColor.ToolStripBorder, Color.FromArgb(0xa3, 0xa3, 0x7c));
            colorMap.Add(KnownToolStripColor.MenuStripGradientBegin, Color.FromArgb(0xe5, 0xe5, 0xd7));
            colorMap.Add(KnownToolStripColor.MenuStripGradientEnd, Color.FromArgb(0xf4, 0xf2, 0xe8));
            colorMap.Add(KnownToolStripColor.ToolStripGradientBegin, Color.FromArgb(0xfe, 0xfe, 0xfb));
            colorMap.Add(KnownToolStripColor.ToolStripGradientMiddle, Color.FromArgb(0xec, 0xe7, 0xe0));
            colorMap.Add(KnownToolStripColor.ToolStripGradientEnd, Color.FromArgb(0xbd, 0xbd, 0xa3));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientBegin, Color.FromArgb(0xf3, 0xf2, 240));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientMiddle, Color.FromArgb(0xe2, 0xe1, 0xdb));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientEnd, Color.FromArgb(0x92, 0x92, 0x76));
            colorMap.Add(KnownToolStripColor.MenuItemPressedGradientBegin, Color.FromArgb(0xfc, 0xfc, 0xf9));
            colorMap.Add(KnownToolStripColor.MenuItemPressedGradientEnd, Color.FromArgb(0xf6, 0xf4, 0xec));
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientBegin, Color.FromArgb(0xf7, 0xf6, 0xef));
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientMiddle, Color.FromArgb(0xf2, 240, 0xe4));
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientEnd, Color.FromArgb(230, 0xe3, 210));
            colorMap.Add(KnownToolStripColor.ButtonCheckedGradientBegin, Color.FromArgb(0xe1, 230, 0xe8));
            colorMap.Add(KnownToolStripColor.ButtonSelectedGradientBegin, Color.FromArgb(0xc1, 210, 0xee));
            colorMap.Add(KnownToolStripColor.ButtonPressedGradientBegin, Color.FromArgb(0x98, 0xb5, 0xe2));
            colorMap.Add(KnownToolStripColor.GripLight, Color.White);
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

                case KnownToolStripColor.ButtonSelectedGradientEnd:
                case KnownToolStripColor.ButtonSelectedGradientMiddle:
                    return KnownToolStripColor.ButtonSelectedGradientBegin;
            }
            return base.MapKnownColor(knownColor);
        }
    }
}

