namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class TabStripIE7ColorTable : TabStripMapColorTable
    {
        public TabStripIE7ColorTable()
        {
        }

        public TabStripIE7ColorTable(ProfessionalColorTable table) : base(table)
        {
        }

        public TabStripIE7ColorTable(ProfessionalColorTable table, IDictionary<KnownTabColor, Color> colorMap) : base(table, colorMap)
        {
        }

        protected override Color GetKnownColor(KnownTabColor knownColor)
        {
            switch (knownColor)
            {
                case KnownTabColor.TabActiveGradientBegin:
                case KnownTabColor.TabSelectedGradientBegin:
                case KnownTabColor.TabInactiveGradientBegin:
                    return Color.White;

                case KnownTabColor.TabActiveGradientEnd:
                case KnownTabColor.TabSelectedGradientEnd:
                    return Color.LightSteelBlue;

                case KnownTabColor.TabInactiveGradientEnd:
                    return Color.Gainsboro;

                case KnownTabColor.TabActiveBorderOuter:
                    return Color.Gray;

                case KnownTabColor.TabInactiveBorderOuter:
                    return Color.Silver;

                case KnownTabColor.TabStripBottomBorder:
                    return Color.Black;

                case KnownTabColor.TabActiveText:
                case KnownTabColor.TabInactiveText:
                    return SystemColors.ControlText;
            }
            return Color.Empty;
        }

        protected override void InitializeMap(Dictionary<KnownTabColor, Color> colorMap)
        {
            colorMap.Add(KnownTabColor.TabBorderInner, Color.FromArgb(120, 0xff, 0xff, 0xff));
        }
    }
}

