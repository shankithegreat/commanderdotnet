namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class VistaColorTable : RoyaleColorTable
    {
        public override Color FromKnownColor(KnownToolStripColor knownColor)
        {
            switch (knownColor)
            {
                case KnownToolStripColor.ButtonCheckedHighlightBorder:
                case KnownToolStripColor.ButtonPressedBorder:
                case KnownToolStripColor.ButtonPressedHighlightBorder:
                case KnownToolStripColor.ButtonSelectedBorder:
                case KnownToolStripColor.ButtonSelectedHighlightBorder:
                    knownColor = KnownToolStripColor.MenuItemBorder;
                    break;
            }
            return base.FromKnownColor(knownColor);
        }

        protected override void InitializeMap(Dictionary<KnownToolStripColor, Color> colorMap)
        {
            base.InitializeMap(colorMap);
            colorMap[KnownToolStripColor.ButtonCheckedHighlightBorder] = Color.FromArgb(0xc9, 210, 0xe5);
            colorMap[KnownToolStripColor.ButtonPressedGradientBegin] = Color.FromArgb(0x33, 0x5e, 0xa8);
            colorMap[KnownToolStripColor.ButtonPressedGradientEnd] = Color.FromArgb(0x33, 0x5e, 0xa8);
            colorMap[KnownToolStripColor.ButtonSelectedGradientBegin] = Color.FromArgb(0x80, 230, 0xf5, 0xfd);
            colorMap[KnownToolStripColor.ButtonSelectedGradientEnd] = Color.FromArgb(160, 0xc1, 0xe2, 0xf3);
            colorMap[KnownToolStripColor.MenuItemSelectedGradientBegin] = Color.FromArgb(0x80, 230, 0xf5, 0xfd);
            colorMap[KnownToolStripColor.MenuItemSelectedGradientEnd] = Color.FromArgb(160, 0xc1, 0xe2, 0xf3);
            colorMap[KnownToolStripColor.CheckBackground] = Color.FromArgb(230, 0xef, 0xf4);
            colorMap[KnownToolStripColor.MenuBorder] = Color.FromArgb(0x97, 0x97, 0x97);
            colorMap[KnownToolStripColor.MenuItemBorder] = Color.FromArgb(160, 0x85, 0xcc, 0xe7);
            colorMap[KnownToolStripColor.SeparatorDark] = Color.FromArgb(0xe2, 0xe3, 0xe3);
            colorMap[KnownToolStripColor.SeparatorLight] = Color.White;
            colorMap[KnownToolStripColor.ToolStripDropDownBackground] = Color.FromArgb(240, 240, 240);
        }
    }
}

