namespace Nomad.Controls
{
    using System;
    using System.Drawing;

    public class ProgressBarRenderEventArgs : EventArgs
    {
        private Rectangle? FBounds;
        public readonly System.Drawing.Graphics Graphics;
        public readonly VistaProgressBar ProgressBar;

        public ProgressBarRenderEventArgs(System.Drawing.Graphics graphics, VistaProgressBar progressBar)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException("graphics");
            }
            if (progressBar == null)
            {
                throw new ArgumentNullException("progressBar");
            }
            this.Graphics = graphics;
            this.ProgressBar = progressBar;
        }

        public ProgressBarRenderEventArgs(System.Drawing.Graphics graphics, VistaProgressBar progressBar, Rectangle bounds) : this(graphics, progressBar)
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

