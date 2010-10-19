namespace Nomad.Controls
{
    using System;
    using System.Windows.Forms;

    public class ToolStripWrapperRenderer : ToolStripRenderer
    {
        private ToolStripRenderer FBaseRenderer;

        public ToolStripWrapperRenderer()
        {
        }

        public ToolStripWrapperRenderer(ToolStripRenderer baseRenderer)
        {
            this.FBaseRenderer = baseRenderer;
        }

        public static ToolStripRenderer GetRoot(ToolStripRenderer renderer)
        {
            for (ToolStripWrapperRenderer renderer2 = renderer as ToolStripWrapperRenderer; renderer2 != null; renderer2 = renderer as ToolStripWrapperRenderer)
            {
                renderer = renderer2.BaseRenderer;
            }
            return renderer;
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            this.BaseRenderer.DrawArrow(e);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            this.BaseRenderer.DrawButtonBackground(e);
        }

        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            this.BaseRenderer.DrawDropDownButtonBackground(e);
        }

        protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
        {
            this.BaseRenderer.DrawGrip(e);
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            this.BaseRenderer.DrawImageMargin(e);
        }

        protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
        {
            this.BaseRenderer.DrawItemBackground(e);
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            this.BaseRenderer.DrawItemCheck(e);
        }

        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            this.BaseRenderer.DrawItemImage(e);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            this.BaseRenderer.DrawItemText(e);
        }

        protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
        {
            this.BaseRenderer.DrawLabelBackground(e);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            this.BaseRenderer.DrawMenuItemBackground(e);
        }

        protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
        {
            this.BaseRenderer.DrawOverflowButtonBackground(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            this.BaseRenderer.DrawSeparator(e);
        }

        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            this.BaseRenderer.DrawSplitButton(e);
        }

        protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
        {
            this.BaseRenderer.DrawStatusStripSizingGrip(e);
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            this.BaseRenderer.DrawToolStripBackground(e);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            this.BaseRenderer.DrawToolStripBorder(e);
        }

        protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
        {
            this.BaseRenderer.DrawToolStripContentPanelBackground(e);
        }

        protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
        {
            this.BaseRenderer.DrawToolStripPanelBackground(e);
        }

        protected override void OnRenderToolStripStatusLabelBackground(ToolStripItemRenderEventArgs e)
        {
            this.BaseRenderer.DrawToolStripStatusLabelBackground(e);
        }

        public ToolStripRenderer BaseRenderer
        {
            get
            {
                if (this.FBaseRenderer != null)
                {
                    return this.FBaseRenderer;
                }
                return ToolStripManager.Renderer;
            }
        }

        public ToolStripRenderer RootRenderer
        {
            get
            {
                return GetRoot(this.BaseRenderer);
            }
        }
    }
}

