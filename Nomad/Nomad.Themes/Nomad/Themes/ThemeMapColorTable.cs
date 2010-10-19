namespace Nomad.Themes
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class ThemeMapColorTable : ThemeDefaultColorTable
    {
        private Dictionary<KnownThemeColor, Color> FColorMap;

        public ThemeMapColorTable()
        {
            this.FColorMap = new Dictionary<KnownThemeColor, Color>();
            this.InitializeMap(this.FColorMap);
            if (this.FColorMap.Count == 0)
            {
                this.FColorMap = null;
            }
        }

        public ThemeMapColorTable(IDictionary<KnownThemeColor, Color> colorMap)
        {
            if (colorMap == null)
            {
                throw new ArgumentNullException();
            }
            this.FColorMap = new Dictionary<KnownThemeColor, Color>();
            this.InitializeMap(this.FColorMap);
            foreach (KeyValuePair<KnownThemeColor, Color> pair in colorMap)
            {
                this.FColorMap[pair.Key] = pair.Value;
            }
            if (this.FColorMap.Count == 0)
            {
                this.FColorMap = null;
            }
        }

        public override Color FromKnownColor(KnownThemeColor knownColor)
        {
            Color color;
            if ((this.FColorMap != null) && this.FColorMap.TryGetValue(knownColor, out color))
            {
                return color;
            }
            return base.FromKnownColor(knownColor);
        }

        protected virtual void InitializeMap(Dictionary<KnownThemeColor, Color> colorMap)
        {
        }
    }
}

