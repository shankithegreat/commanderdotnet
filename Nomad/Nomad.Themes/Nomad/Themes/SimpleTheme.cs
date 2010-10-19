namespace Nomad.Themes
{
    using System;
    using System.Windows.Forms;

    public sealed class SimpleTheme : Theme
    {
        private System.Windows.Forms.ToolStripRenderer FTabStripRenderer;
        private ThemeColorTable FThemeColors;
        private System.Windows.Forms.ToolStripRenderer FToolStripRenderer;

        public SimpleTheme(System.Windows.Forms.ToolStripRenderer toolStripRenderer, System.Windows.Forms.ToolStripRenderer tabStripRenderer, ThemeColorTable themeColors)
        {
            if (toolStripRenderer == null)
            {
                throw new ArgumentNullException("toolStripRenderer");
            }
            if (tabStripRenderer == null)
            {
                throw new ArgumentNullException("tabStripRenderer");
            }
            if (themeColors == null)
            {
                throw new ArgumentNullException("themeColors");
            }
            this.FToolStripRenderer = toolStripRenderer;
            this.FTabStripRenderer = tabStripRenderer;
            this.FThemeColors = themeColors;
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

