namespace Nomad.Controls
{
    using System;
    using System.Drawing;

    public class ProgressBarMarqueeEventArgs : EventArgs
    {
        private Rectangle? FBounds;
        public object MarqueeTag;
        public readonly VistaProgressBar ProgressBar;

        public ProgressBarMarqueeEventArgs(VistaProgressBar progressBar, object marqueeTag)
        {
            if (progressBar == null)
            {
                throw new ArgumentNullException("progressBar");
            }
            this.ProgressBar = progressBar;
            this.MarqueeTag = marqueeTag;
        }

        public ProgressBarMarqueeEventArgs(VistaProgressBar progressBar, object marqueeTag, Rectangle bounds) : this(progressBar, marqueeTag)
        {
            this.FBounds = new Rectangle?(bounds);
        }

        public Rectangle Bounds
        {
            get
            {
                return (this.FBounds.HasValue ? this.FBounds.Value : this.ProgressBar.ClientRectangle);
            }
        }
    }
}

