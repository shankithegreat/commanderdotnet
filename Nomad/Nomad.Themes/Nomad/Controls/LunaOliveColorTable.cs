namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class LunaOliveColorTable : LunaColorTable
    {
        public static readonly ProfessionalColorTable Default = new LunaOliveColorTable();

        protected override void InitializeMap(Dictionary<KnownToolStripColor, Color> colorMap)
        {
            colorMap.Add(KnownToolStripColor.ButtonCheckedGradientBegin, Color.FromArgb(0xff, 0xdf, 0x9a));
            colorMap.Add(KnownToolStripColor.ButtonCheckedGradientMiddle, Color.FromArgb(0xff, 0xc3, 0x74));
            colorMap.Add(KnownToolStripColor.ButtonCheckedGradientEnd, Color.FromArgb(0xff, 0xa6, 0x4c));
            colorMap.Add(KnownToolStripColor.ButtonPressedBorder, Color.FromArgb(0x3f, 0x5d, 0x38));
            colorMap.Add(KnownToolStripColor.ButtonPressedGradientBegin, Color.FromArgb(0xfe, 0x80, 0x3e));
            colorMap.Add(KnownToolStripColor.ButtonPressedGradientMiddle, Color.FromArgb(0xff, 0xb1, 0x6d));
            colorMap.Add(KnownToolStripColor.ButtonPressedGradientEnd, Color.FromArgb(0xff, 0xdf, 0x9a));
            colorMap.Add(KnownToolStripColor.ButtonSelectedGradientBegin, Color.FromArgb(0xff, 0xff, 0xde));
            colorMap.Add(KnownToolStripColor.ButtonSelectedGradientMiddle, Color.FromArgb(0xff, 0xe1, 0xac));
            colorMap.Add(KnownToolStripColor.ButtonSelectedGradientEnd, Color.FromArgb(0xff, 0xcb, 0x88));
            colorMap.Add(KnownToolStripColor.CheckBackground, Color.FromArgb(0xff, 0xc0, 0x6f));
            colorMap.Add(KnownToolStripColor.CheckPressedBackground, Color.FromArgb(0xfe, 0x80, 0x3e));
            colorMap.Add(KnownToolStripColor.GripDark, Color.FromArgb(0x51, 0x5e, 0x33));
            colorMap.Add(KnownToolStripColor.GripLight, Color.White);
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientBegin, Color.FromArgb(230, 230, 0xd1));
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientMiddle, Color.FromArgb(0xba, 0xc9, 0x8f));
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientEnd, Color.FromArgb(160, 0xb1, 0x74));
            colorMap.Add(KnownToolStripColor.MenuBorder, Color.FromArgb(0x75, 0x8d, 0x5e));
            colorMap.Add(KnownToolStripColor.MenuItemBorder, Color.FromArgb(0x3f, 0x5d, 0x38));
            colorMap.Add(KnownToolStripColor.MenuItemPressedGradientBegin, Color.FromArgb(0xed, 240, 0xd6));
            colorMap.Add(KnownToolStripColor.MenuItemPressedGradientEnd, Color.FromArgb(0xb5, 0xc4, 0x8f));
            colorMap.Add(KnownToolStripColor.MenuItemSelected, Color.FromArgb(0xff, 0xee, 0xc2));
            colorMap.Add(KnownToolStripColor.MenuStripGradientBegin, Color.FromArgb(0xd9, 0xd9, 0xa7));
            colorMap.Add(KnownToolStripColor.MenuStripGradientEnd, Color.FromArgb(0xf2, 0xf1, 0xe4));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientBegin, Color.FromArgb(0xba, 0xcc, 150));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientMiddle, Color.FromArgb(0x8d, 160, 0x6b));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientEnd, Color.FromArgb(0x60, 0x77, 0x6b));
            colorMap.Add(KnownToolStripColor.SeparatorDark, Color.FromArgb(0x60, 0x80, 0x58));
            colorMap.Add(KnownToolStripColor.SeparatorLight, Color.FromArgb(0xf4, 0xf7, 0xde));
            colorMap.Add(KnownToolStripColor.ToolStripBorder, Color.FromArgb(0x60, 0x80, 0x58));
            colorMap.Add(KnownToolStripColor.ToolStripDropDownBackground, Color.FromArgb(0xf4, 0xf4, 0xee));
            colorMap.Add(KnownToolStripColor.ToolStripGradientBegin, Color.FromArgb(0xff, 0xff, 0xed));
            colorMap.Add(KnownToolStripColor.ToolStripGradientMiddle, Color.FromArgb(0xce, 220, 0xa7));
            colorMap.Add(KnownToolStripColor.ToolStripGradientEnd, Color.FromArgb(0xb5, 0xc4, 0x8f));
            colorMap.Add(KnownToolStripColor.ButtonPressedHighlight, Color.White);
            colorMap.Add(KnownToolStripColor.ButtonCheckedHighlight, Color.White);
        }
    }
}

