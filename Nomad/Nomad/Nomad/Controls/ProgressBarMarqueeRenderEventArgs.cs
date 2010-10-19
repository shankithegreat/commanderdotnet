namespace Nomad.Controls
{
    using System;
    using System.Drawing;

    public class ProgressBarMarqueeRenderEventArgs : ProgressBarRenderEventArgs
    {
        public object MarqueeTag;

        public ProgressBarMarqueeRenderEventArgs(Graphics graphics, VistaProgressBar progressBar, object marqueeTag) : base(graphics, progressBar)
        {
            this.MarqueeTag = marqueeTag;
        }
    }
}

