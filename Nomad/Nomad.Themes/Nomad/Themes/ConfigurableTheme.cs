namespace Nomad.Themes
{
    using Nomad.Controls;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class ConfigurableTheme : Theme
    {
        private ListViewColorTable FListViewColors;
        private bool FSelectedTabBoldFont;
        private FontFamily FTabStripFontFamily;
        private System.Windows.Forms.ToolStripRenderer FTabStripRenderer;
        private ThemeColorTable FThemeColors;
        private FontFamily FToolStripFontFamily;
        private System.Windows.Forms.ToolStripRenderer FToolStripRenderer;

        public ConfigurableTheme(ThemeConfigurationSection configuration)
        {
            this.FListViewColors = this.InitializeListView(configuration.ListViewColors);
            this.FToolStripRenderer = this.InitializeToolStrip(configuration.ToolStripRenderer);
            this.FTabStripRenderer = this.InitializeTabStrip(configuration.TabStripRenderer);
            this.FThemeColors = this.InitializeTheme(configuration.ThemeColors);
        }

        protected static Dictionary<T, Color> CreateColorMap<T>(ColorTableConfigurationElement tableConfiguration)
        {
            if (tableConfiguration.ColorTable.Count == 0)
            {
                return null;
            }
            Dictionary<T, Color> dictionary = new Dictionary<T, Color>(tableConfiguration.ColorTable.Count);
            foreach (ColorElement element in tableConfiguration.ColorTable)
            {
                Color baseColor = element.Value;
                if (element.Alpha != 0xff)
                {
                    baseColor = Color.FromArgb(element.Alpha, baseColor);
                }
                dictionary.Add((T) Enum.Parse(typeof(T), element.Color), baseColor);
            }
            return dictionary;
        }

        protected virtual ListViewColorTable InitializeListView(ColorTableConfigurationElement listViewConfiguration)
        {
            Dictionary<KnownListViewColor, Color> colorMap = CreateColorMap<KnownListViewColor>(listViewConfiguration);
            System.Type type = System.Type.GetType(listViewConfiguration.ColorTableType, false);
            if (type != null)
            {
                if (colorMap != null)
                {
                    return (ListViewColorTable) Activator.CreateInstance(type, new object[] { colorMap });
                }
                return (ListViewColorTable) Activator.CreateInstance(type);
            }
            if (colorMap != null)
            {
                return new ListViewMapColorTable(colorMap);
            }
            return ListViewColorTable.Default;
        }

        protected virtual System.Windows.Forms.ToolStripRenderer InitializeTabStrip(TabStripConfigurationElement tabConfiguration)
        {
            System.Windows.Forms.ToolStripRenderer renderer;
            TabStripColorTable colorTable = null;
            Dictionary<KnownTabColor, Color> colorMap = CreateColorMap<KnownTabColor>(tabConfiguration.ColorTable);
            ProfessionalColorTable toolStripColors = this.ToolStripColors;
            if (toolStripColors == null)
            {
                toolStripColors = new ProfessionalColorTable();
            }
            System.Type type = System.Type.GetType(tabConfiguration.ColorTable.ColorTableType, false);
            if (type != null)
            {
                if (colorMap != null)
                {
                    colorTable = (TabStripColorTable) Activator.CreateInstance(type, new object[] { toolStripColors, colorMap });
                }
                else if (type.GetConstructor(new System.Type[] { typeof(ProfessionalColorTable) }) != null)
                {
                    colorTable = (TabStripColorTable) Activator.CreateInstance(type, new object[] { toolStripColors });
                }
                else
                {
                    colorTable = (TabStripColorTable) Activator.CreateInstance(type);
                }
            }
            else if (colorMap != null)
            {
                colorTable = new TabStripMapColorTable(toolStripColors, colorMap);
            }
            System.Type type2 = System.Type.GetType(tabConfiguration.RendererType, false);
            if (type2 != null)
            {
                if (colorTable != null)
                {
                    renderer = (System.Windows.Forms.ToolStripRenderer) Activator.CreateInstance(type2, new object[] { colorTable });
                }
                else if (type2.GetConstructor(new System.Type[] { typeof(TabStripColorTable) }) != null)
                {
                    renderer = (System.Windows.Forms.ToolStripRenderer) Activator.CreateInstance(type2, new object[] { new TabStripColorTable(toolStripColors) });
                }
                else
                {
                    renderer = (System.Windows.Forms.ToolStripRenderer) Activator.CreateInstance(type2);
                }
            }
            else if (colorTable != null)
            {
                renderer = new TabStripProfessionalRenderer(colorTable);
            }
            else
            {
                renderer = new TabStripProfessionalRenderer(new TabStripColorTable(toolStripColors));
            }
            TabStripProfessionalRenderer renderer2 = renderer as TabStripProfessionalRenderer;
            if (renderer2 != null)
            {
                renderer2.UseBoldFont = tabConfiguration.UseBoldFont;
            }
            else
            {
                this.FSelectedTabBoldFont = tabConfiguration.UseBoldFont;
            }
            TabStripSystemRenderer renderer3 = renderer as TabStripSystemRenderer;
            if (renderer3 != null)
            {
                renderer3.DrawFocusRect = tabConfiguration.DrawFocusRect;
                renderer3.UseVisualStyles = tabConfiguration.UseVisualStyles;
            }
            if (!string.IsNullOrEmpty(tabConfiguration.FontFamily))
            {
                this.FTabStripFontFamily = new FontFamily(tabConfiguration.FontFamily);
            }
            return renderer;
        }

        protected virtual ThemeColorTable InitializeTheme(ColorTableConfigurationElement themeConfiguration)
        {
            Dictionary<KnownThemeColor, Color> colorMap = CreateColorMap<KnownThemeColor>(themeConfiguration);
            System.Type type = System.Type.GetType(themeConfiguration.ColorTableType, false);
            if (type != null)
            {
                if (colorMap != null)
                {
                    return (ThemeColorTable) Activator.CreateInstance(type, new object[] { colorMap });
                }
                return (ThemeColorTable) Activator.CreateInstance(type);
            }
            if (colorMap != null)
            {
                return new ThemeMapColorTable(colorMap);
            }
            return ThemeColorTable.Default;
        }

        protected virtual System.Windows.Forms.ToolStripRenderer InitializeToolStrip(ToolStripConfigurationElement toolStripConfiguration)
        {
            System.Windows.Forms.ToolStripRenderer renderer;
            ProfessionalColorTable professionalColorTable = null;
            Dictionary<KnownToolStripColor, Color> colorMap = CreateColorMap<KnownToolStripColor>(toolStripConfiguration.ColorTable);
            System.Type type = System.Type.GetType(toolStripConfiguration.ColorTable.ColorTableType, false);
            if (type != null)
            {
                if (colorMap != null)
                {
                    professionalColorTable = (ProfessionalColorTable) Activator.CreateInstance(type, new object[] { colorMap });
                }
                else
                {
                    professionalColorTable = (ProfessionalColorTable) Activator.CreateInstance(type);
                }
            }
            else if (colorMap != null)
            {
                professionalColorTable = new ToolStripMapColorTable(colorMap);
            }
            System.Type type2 = System.Type.GetType(toolStripConfiguration.RendererType, false);
            if (type2 != null)
            {
                if (professionalColorTable != null)
                {
                    renderer = (System.Windows.Forms.ToolStripRenderer) Activator.CreateInstance(type2, new object[] { professionalColorTable });
                }
                else
                {
                    renderer = (System.Windows.Forms.ToolStripRenderer) Activator.CreateInstance(type2);
                }
            }
            else if (professionalColorTable != null)
            {
                renderer = new ToolStripProfessionalRenderer(professionalColorTable);
            }
            else
            {
                renderer = new ToolStripProfessionalRenderer();
            }
            ToolStripProfessionalRenderer renderer2 = renderer as ToolStripProfessionalRenderer;
            if (renderer2 != null)
            {
                renderer2.RoundedEdges = toolStripConfiguration.RoundedEdges;
                renderer2.ColorTable.UseSystemColors = toolStripConfiguration.UseSystemColors;
            }
            if (!string.IsNullOrEmpty(toolStripConfiguration.FontFamily))
            {
                this.FToolStripFontFamily = new FontFamily(toolStripConfiguration.FontFamily);
            }
            return renderer;
        }

        public override ListViewColorTable ListViewColors
        {
            get
            {
                return this.FListViewColors;
            }
        }

        public override bool SelectedTabBoldFont
        {
            get
            {
                return this.FSelectedTabBoldFont;
            }
        }

        public override FontFamily TabStripFontFamily
        {
            get
            {
                return this.FTabStripFontFamily;
            }
        }

        public override System.Windows.Forms.ToolStripRenderer TabStripRenderer
        {
            get
            {
                return this.FTabStripRenderer;
            }
        }

        public override ThemeColorTable ThemeColors
        {
            get
            {
                return this.FThemeColors;
            }
        }

        public override System.Windows.Forms.ToolStripRenderer ToolStripRenderer
        {
            get
            {
                return this.FToolStripRenderer;
            }
        }
    }
}

