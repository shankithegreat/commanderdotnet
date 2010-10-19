namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class LunaSilverColorTable : LunaColorTable
    {
        public static readonly ProfessionalColorTable Default = new LunaSilverColorTable();

        protected override void InitializeMap(Dictionary<KnownToolStripColor, Color> colorMap)
        {
            colorMap.Add(KnownToolStripColor.ButtonCheckedGradientBegin, Color.FromArgb(0xff, 0xdf, 0x9a));
            colorMap.Add(KnownToolStripColor.ButtonCheckedGradientMiddle, Color.FromArgb(0xff, 0xc3, 0x74));
            colorMap.Add(KnownToolStripColor.ButtonCheckedGradientEnd, Color.FromArgb(0xff, 0xa6, 0x4c));
            colorMap.Add(KnownToolStripColor.ButtonPressedBorder, Color.FromArgb(0x4b, 0x4b, 0x6f));
            colorMap.Add(KnownToolStripColor.ButtonPressedGradientBegin, Color.FromArgb(0xfe, 0x80, 0x3e));
            colorMap.Add(KnownToolStripColor.ButtonPressedGradientMiddle, Color.FromArgb(0xff, 0xb1, 0x6d));
            colorMap.Add(KnownToolStripColor.ButtonPressedGradientEnd, Color.FromArgb(0xff, 0xdf, 0x9a));
            colorMap.Add(KnownToolStripColor.ButtonSelectedGradientBegin, Color.FromArgb(0xff, 0xff, 0xde));
            colorMap.Add(KnownToolStripColor.ButtonSelectedGradientMiddle, Color.FromArgb(0xff, 0xe1, 0xac));
            colorMap.Add(KnownToolStripColor.ButtonSelectedGradientEnd, Color.FromArgb(0xff, 0xcb, 0x88));
            colorMap.Add(KnownToolStripColor.CheckBackground, Color.FromArgb(0xff, 0xc0, 0x6f));
            colorMap.Add(KnownToolStripColor.CheckPressedBackground, Color.FromArgb(0xfe, 0x80, 0x3e));
            colorMap.Add(KnownToolStripColor.GripDark, Color.FromArgb(0x54, 0x54, 0x75));
            colorMap.Add(KnownToolStripColor.GripLight, Color.White);
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientBegin, Color.FromArgb(0xd7, 0xd7, 0xe2));
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientMiddle, Color.FromArgb(0xb8, 0xb9, 0xca));
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientEnd, Color.FromArgb(0x76, 0x74, 0x97));
            colorMap.Add(KnownToolStripColor.MenuBorder, Color.FromArgb(0x7c, 0x7c, 0x94));
            colorMap.Add(KnownToolStripColor.MenuItemBorder, Color.FromArgb(0x4b, 0x4b, 0x6f));
            colorMap.Add(KnownToolStripColor.MenuItemPressedGradientBegin, Color.FromArgb(0xe8, 0xe9, 0xf2));
            colorMap.Add(KnownToolStripColor.MenuItemPressedGradientEnd, Color.FromArgb(0xac, 170, 0xc2));
            colorMap.Add(KnownToolStripColor.MenuItemSelected, Color.FromArgb(0xff, 0xee, 0xc2));
            colorMap.Add(KnownToolStripColor.MenuStripGradientBegin, Color.FromArgb(0xd7, 0xd7, 0xe5));
            colorMap.Add(KnownToolStripColor.MenuStripGradientEnd, Color.FromArgb(0xf3, 0xf3, 0xf7));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientBegin, Color.FromArgb(0xba, 0xb9, 0xce));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientMiddle, Color.FromArgb(0x9c, 0x9b, 180));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientEnd, Color.FromArgb(0x76, 0x74, 0x92));
            colorMap.Add(KnownToolStripColor.SeparatorDark, Color.FromArgb(110, 0x6d, 0x8f));
            colorMap.Add(KnownToolStripColor.SeparatorLight, Color.White);
            colorMap.Add(KnownToolStripColor.ToolStripBorder, Color.FromArgb(0x7c, 0x7c, 0x94));
            colorMap.Add(KnownToolStripColor.ToolStripDropDownBackground, Color.FromArgb(0xfd, 250, 0xff));
            colorMap.Add(KnownToolStripColor.ToolStripGradientBegin, Color.FromArgb(0xf9, 0xf9, 0xff));
            colorMap.Add(KnownToolStripColor.ToolStripGradientMiddle, Color.FromArgb(0xe1, 0xe2, 0xec));
            colorMap.Add(KnownToolStripColor.ToolStripGradientEnd, Color.FromArgb(0x93, 0x91, 0xb0));
            colorMap.Add(KnownToolStripColor.ButtonPressedHighlight, Color.FromArgb(0xb2, 180, 0xbf));
            colorMap.Add(KnownToolStripColor.ButtonCheckedHighlight, Color.FromArgb(0xf1, 0xef, 0xe2));
        }
    }
}

