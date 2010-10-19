namespace Nomad.Themes
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class ListViewMapColorTable : ListViewDefaultColorTable
    {
        private Dictionary<KnownListViewColor, Color> FColorMap;

        public ListViewMapColorTable()
        {
        }

        public ListViewMapColorTable(IDictionary<KnownListViewColor, Color> colorMap)
        {
            if (colorMap == null)
            {
                throw new ArgumentNullException();
            }
            if (colorMap.Count == 0)
            {
                throw new ArgumentException();
            }
            this.FColorMap = new Dictionary<KnownListViewColor, Color>();
            foreach (KeyValuePair<KnownListViewColor, Color> pair in colorMap)
            {
                this.FColorMap[pair.Key] = pair.Value;
            }
            if (this.FColorMap.Count == 0)
            {
                this.FColorMap = null;
            }
        }

        public ListViewMapColorTable(ListViewColorTable original, IDictionary<KnownListViewColor, Color> colorMap)
        {
            if (original == null)
            {
                throw new ArgumentNullException("original");
            }
            if (colorMap == null)
            {
                throw new ArgumentNullException("colorMap");
            }
            if (colorMap.Count == 0)
            {
                throw new ArgumentException();
            }
            this.FColorMap = new Dictionary<KnownListViewColor, Color>();
            ListViewMapColorTable table = original as ListViewMapColorTable;
            if (table != null)
            {
                foreach (KeyValuePair<KnownListViewColor, Color> pair in table.FColorMap)
                {
                    this.FColorMap[pair.Key] = pair.Value;
                }
            }
            foreach (KeyValuePair<KnownListViewColor, Color> pair in colorMap)
            {
                this.FColorMap[pair.Key] = pair.Value;
            }
            if (this.FColorMap.Count == 0)
            {
                this.FColorMap = null;
            }
        }

        public override Color FromKnownColor(KnownListViewColor knownColor)
        {
            Color color;
            if ((this.FColorMap != null) && this.FColorMap.TryGetValue(knownColor, out color))
            {
                return color;
            }
            return base.FromKnownColor(knownColor);
        }
    }
}

