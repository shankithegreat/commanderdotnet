namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class LunaBlueColorTable : LunaColorTable
    {
        public static readonly ProfessionalColorTable Default = new LunaBlueColorTable();

        protected override void InitializeMap(Dictionary<KnownToolStripColor, Color> colorMap)
        {
            colorMap.Add(KnownToolStripColor.ButtonCheckedGradientBegin, Color.FromArgb(0xff, 0xdf, 0x9a));
            colorMap.Add(KnownToolStripColor.ButtonCheckedGradientMiddle, Color.FromArgb(0xff, 0xc3, 0x74));
            colorMap.Add(KnownToolStripColor.ButtonCheckedGradientEnd, Color.FromArgb(0xff, 0xa6, 0x4c));
            colorMap.Add(KnownToolStripColor.ButtonPressedBorder, Color.FromArgb(0, 0, 0x80));
            colorMap.Add(KnownToolStripColor.ButtonPressedGradientBegin, Color.FromArgb(0xfe, 0x80, 0x3e));
            colorMap.Add(KnownToolStripColor.ButtonPressedGradientMiddle, Color.FromArgb(0xff, 0xb1, 0x6d));
            colorMap.Add(KnownToolStripColor.ButtonPressedGradientEnd, Color.FromArgb(0xff, 0xdf, 0x9a));
            colorMap.Add(KnownToolStripColor.ButtonSelectedGradientBegin, Color.FromArgb(0xff, 0xff, 0xde));
            colorMap.Add(KnownToolStripColor.ButtonSelectedGradientMiddle, Color.FromArgb(0xff, 0xe1, 0xac));
            colorMap.Add(KnownToolStripColor.ButtonSelectedGradientEnd, Color.FromArgb(0xff, 0xcb, 0x88));
            colorMap.Add(KnownToolStripColor.CheckBackground, Color.FromArgb(0xff, 0xc0, 0x6f));
            colorMap.Add(KnownToolStripColor.CheckPressedBackground, Color.FromArgb(0xfe, 0x80, 0x3e));
            colorMap.Add(KnownToolStripColor.GripDark, Color.FromArgb(0x27, 0x41, 0x76));
            colorMap.Add(KnownToolStripColor.GripLight, Color.White);
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientBegin, Color.FromArgb(0xcb, 0xdd, 0xf6));
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientMiddle, Color.FromArgb(0xa1, 0xc5, 0xf9));
            colorMap.Add(KnownToolStripColor.ImageMarginRevealedGradientEnd, Color.FromArgb(0x72, 0x9b, 0xd7));
            colorMap.Add(KnownToolStripColor.MenuBorder, Color.FromArgb(0, 0x2d, 150));
            colorMap.Add(KnownToolStripColor.MenuItemBorder, Color.FromArgb(0, 0, 0x80));
            colorMap.Add(KnownToolStripColor.MenuItemPressedGradientBegin, Color.FromArgb(0xe3, 0xef, 0xff));
            colorMap.Add(KnownToolStripColor.MenuItemPressedGradientEnd, Color.FromArgb(0x7b, 0xa4, 0xe0));
            colorMap.Add(KnownToolStripColor.MenuItemSelected, Color.FromArgb(0xff, 0xee, 0xc2));
            colorMap.Add(KnownToolStripColor.MenuStripGradientBegin, Color.FromArgb(0x9e, 190, 0xf5));
            colorMap.Add(KnownToolStripColor.MenuStripGradientEnd, Color.FromArgb(0xc4, 0xda, 250));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientBegin, Color.FromArgb(0x7f, 0xb1, 250));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientMiddle, Color.FromArgb(0x52, 0x7f, 0xd0));
            colorMap.Add(KnownToolStripColor.OverflowButtonGradientEnd, Color.FromArgb(0, 0x35, 0x91));
            colorMap.Add(KnownToolStripColor.SeparatorDark, Color.FromArgb(0x6a, 140, 0xcb));
            colorMap.Add(KnownToolStripColor.SeparatorLight, Color.FromArgb(0xf1, 0xf9, 0xff));
            colorMap.Add(KnownToolStripColor.ToolStripBorder, Color.FromArgb(0x3b, 0x61, 0x9c));
            colorMap.Add(KnownToolStripColor.ToolStripDropDownBackground, Color.FromArgb(0xf6, 0xf6, 0xf6));
            colorMap.Add(KnownToolStripColor.ToolStripGradientBegin, Color.FromArgb(0xe3, 0xef, 0xff));
            colorMap.Add(KnownToolStripColor.ToolStripGradientMiddle, Color.FromArgb(0xcb, 0xe1, 0xfc));
            colorMap.Add(KnownToolStripColor.ToolStripGradientEnd, Color.FromArgb(0x7b, 0xa4, 0xe0));
            colorMap.Add(KnownToolStripColor.ButtonPressedHighlight, Color.FromArgb(0x31, 0x6a, 0xc5));
            colorMap.Add(KnownToolStripColor.ButtonCheckedHighlight, Color.FromArgb(0xf1, 0xef, 0xe2));
        }
    }
}

