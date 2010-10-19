namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;

    public class ToolStripMapColorTable : ToolStripColorTable
    {
        private Dictionary<KnownToolStripColor, Color> FColorMap;

        public ToolStripMapColorTable()
        {
            this.InitializeColorMap(null);
        }

        public ToolStripMapColorTable(IDictionary<KnownToolStripColor, Color> colorMap)
        {
            if (colorMap == null)
            {
                throw new ArgumentNullException();
            }
            this.InitializeColorMap(colorMap);
        }

        public override Color FromKnownColor(KnownToolStripColor knownColor)
        {
            Color color;
            if ((this.FColorMap == null) || !this.FColorMap.TryGetValue(knownColor, out color))
            {
                knownColor = this.MapKnownColor(knownColor);
                if ((this.FColorMap != null) && this.FColorMap.TryGetValue(knownColor, out color))
                {
                    return color;
                }
                color = this.GetKnownColor(knownColor);
                if (color == Color.Empty)
                {
                    throw new InvalidEnumArgumentException();
                }
            }
            return color;
        }

        private void InitializeColorMap(IDictionary<KnownToolStripColor, Color> colorMap)
        {
            this.FColorMap = new Dictionary<KnownToolStripColor, Color>();
            this.InitializeMap(this.FColorMap);
            if (colorMap != null)
            {
                foreach (KeyValuePair<KnownToolStripColor, Color> pair in colorMap)
                {
                    this.FColorMap[pair.Key] = pair.Value;
                }
            }
            if (this.FColorMap.Count == 0)
            {
                this.FColorMap = null;
            }
        }

        protected virtual void InitializeMap(Dictionary<KnownToolStripColor, Color> colorMap)
        {
        }
    }
}

