namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TabStripMapColorTable : TabStripColorTable
    {
        private Dictionary<KnownTabColor, Color> FColorMap;

        public TabStripMapColorTable()
        {
            this.InitializeColorMap(null);
        }

        public TabStripMapColorTable(ProfessionalColorTable table) : base(table)
        {
            this.InitializeColorMap(null);
        }

        public TabStripMapColorTable(ProfessionalColorTable table, IDictionary<KnownTabColor, Color> colorMap) : base(table)
        {
            if (colorMap == null)
            {
                throw new ArgumentNullException();
            }
            this.InitializeColorMap(colorMap);
        }

        public override Color FromKnownColor(KnownTabColor knownColor)
        {
            Color color;
            if ((this.FColorMap == null) || !this.FColorMap.TryGetValue(knownColor, out color))
            {
                knownColor = this.MapKnownColor(knownColor);
                color = this.GetKnownColor(knownColor);
                if (!color.IsEmpty)
                {
                    return color;
                }
                if ((this.FColorMap == null) || !this.FColorMap.TryGetValue(knownColor, out color))
                {
                    throw new InvalidEnumArgumentException();
                }
            }
            return color;
        }

        private void InitializeColorMap(IDictionary<KnownTabColor, Color> colorMap)
        {
            this.FColorMap = new Dictionary<KnownTabColor, Color>();
            this.InitializeMap(this.FColorMap);
            if (colorMap != null)
            {
                foreach (KeyValuePair<KnownTabColor, Color> pair in colorMap)
                {
                    this.FColorMap[pair.Key] = pair.Value;
                }
            }
            if (this.FColorMap.Count == 0)
            {
                this.FColorMap = null;
            }
        }

        protected virtual void InitializeMap(Dictionary<KnownTabColor, Color> colorMap)
        {
        }
    }
}

