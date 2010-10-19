namespace Nomad.Commons.Controls
{
    using Nomad.Controls;
    using System;
    using System.Windows.Forms;

    public static class BorderLessToolStripRenderer
    {
        private static ToolStripRenderer FDefault;

        static BorderLessToolStripRenderer()
        {
            ToolStripManager.RendererChanged += new EventHandler(BorderLessToolStripRenderer.RendererChanged);
        }

        public static ToolStripRenderer Create(ToolStripManagerRenderMode renderMode, ToolStripRenderer customRenderer)
        {
            switch (renderMode)
            {
                case ToolStripManagerRenderMode.System:
                    return new BorderLessSystemRenderer();

                case ToolStripManagerRenderMode.Professional:
                    return new BorderLessProfessionalRenderer();
            }
            if (customRenderer != null)
            {
                return new BorderLessWrapperRenderer(customRenderer);
            }
            return new BorderLessProfessionalRenderer();
        }

        private static void RendererChanged(object sender, EventArgs e)
        {
            FDefault = null;
        }

        public static ToolStripRenderer Default
        {
            get
            {
                if (FDefault == null)
                {
                    ToolStripRenderer fDefault = FDefault;
                }
                return (FDefault = Create(ToolStripManager.RenderMode, ToolStripManager.Renderer));
            }
        }

        private class BorderLessProfessionalRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDown)
                {
                    base.OnRenderToolStripBorder(e);
                }
            }
        }

        private class BorderLessSystemRenderer : ToolStripSystemRenderer
        {
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDown)
                {
                    base.OnRenderToolStripBorder(e);
                }
            }
        }

        private class BorderLessWrapperRenderer : ToolStripWrapperRenderer
        {
            public BorderLessWrapperRenderer(ToolStripRenderer baseRenderer) : base(baseRenderer)
            {
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDown)
                {
                    base.OnRenderToolStripBackground(e);
                }
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDown)
                {
                    base.OnRenderToolStripBorder(e);
                }
            }
        }
    }
}

