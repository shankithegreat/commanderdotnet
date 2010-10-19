namespace Nomad.Commons.Controls
{
    using Nomad.Controls;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public static class SimpleBorderToolStipRenderer
    {
        public static ToolStripRenderer Create(ToolStripManagerRenderMode renderMode, ToolStripRenderer customRenderer, Color borderColor)
        {
            switch (renderMode)
            {
                case ToolStripManagerRenderMode.System:
                    return new SimpleBorderSystemRenderer(borderColor);

                case ToolStripManagerRenderMode.Professional:
                    return new SimpleBorderProfessionalRenderer(borderColor);
            }
            if (customRenderer == null)
            {
                return new SimpleBorderProfessionalRenderer(borderColor);
            }
            return new SimpleBorderWrapperRenderer(customRenderer, borderColor);
        }

        private class SimpleBorderProfessionalRenderer : ToolStripProfessionalRenderer
        {
            private Pen ToolStripBorderPen;

            public SimpleBorderProfessionalRenderer(Color borderColor)
            {
                base.RoundedEdges = false;
                this.ToolStripBorderPen = new Pen(borderColor);
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                if ((e.ToolStrip is ToolStripDropDown) || (e.ToolStrip is MenuStrip))
                {
                    base.OnRenderToolStripBackground(e);
                }
                using (Brush brush = new SolidBrush(e.BackColor))
                {
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
                }
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDown)
                {
                    base.OnRenderToolStripBorder(e);
                }
                else
                {
                    Rectangle affectedBounds = e.AffectedBounds;
                    affectedBounds = Rectangle.FromLTRB(affectedBounds.Left, affectedBounds.Top, affectedBounds.Right - 1, affectedBounds.Bottom - 1);
                    e.Graphics.DrawRectangle(this.ToolStripBorderPen, affectedBounds);
                }
            }
        }

        private class SimpleBorderSystemRenderer : ToolStripSystemRenderer
        {
            private Pen ToolStripBorderPen;

            public SimpleBorderSystemRenderer(Color borderColor)
            {
                this.ToolStripBorderPen = new Pen(borderColor);
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                if ((e.ToolStrip is ToolStripDropDown) || (e.ToolStrip is MenuStrip))
                {
                    base.OnRenderToolStripBackground(e);
                }
                using (Brush brush = new SolidBrush(e.BackColor))
                {
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
                }
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDown)
                {
                    base.OnRenderToolStripBorder(e);
                }
                else
                {
                    Rectangle affectedBounds = e.AffectedBounds;
                    affectedBounds = Rectangle.FromLTRB(affectedBounds.Left, affectedBounds.Top, affectedBounds.Right - 1, affectedBounds.Bottom - 1);
                    e.Graphics.DrawRectangle(this.ToolStripBorderPen, affectedBounds);
                }
            }
        }

        private class SimpleBorderWrapperRenderer : ToolStripWrapperRenderer
        {
            private Pen ToolStripBorderPen;

            public SimpleBorderWrapperRenderer(ToolStripRenderer baseRenderer, Color borderColor) : base(baseRenderer)
            {
                if (base.BaseRenderer is ToolStripProfessionalRenderer)
                {
                    ((ToolStripProfessionalRenderer) baseRenderer).RoundedEdges = false;
                }
                this.ToolStripBorderPen = new Pen(borderColor);
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                if ((e.ToolStrip is ToolStripDropDown) || (e.ToolStrip is MenuStrip))
                {
                    base.OnRenderToolStripBackground(e);
                }
                using (Brush brush = new SolidBrush(e.BackColor))
                {
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
                }
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDown)
                {
                    base.OnRenderToolStripBorder(e);
                }
                else
                {
                    Rectangle affectedBounds = e.AffectedBounds;
                    affectedBounds = Rectangle.FromLTRB(affectedBounds.Left, affectedBounds.Top, affectedBounds.Right - 1, affectedBounds.Bottom - 1);
                    e.Graphics.DrawRectangle(this.ToolStripBorderPen, affectedBounds);
                }
            }
        }
    }
}

