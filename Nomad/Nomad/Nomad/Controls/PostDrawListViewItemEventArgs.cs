namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class PostDrawListViewItemEventArgs : ListViewItemStateEventArgs
    {
        private Rectangle FBounds;
        private System.Drawing.Graphics FGraphics;

        internal PostDrawListViewItemEventArgs(System.Drawing.Graphics g, ListView owner, int itemIndex, Rectangle bounds, ListViewItemStates state) : base(owner, itemIndex, state)
        {
            this.FGraphics = g;
            this.FBounds = bounds;
        }

        public Rectangle Bounds
        {
            get
            {
                return this.FBounds;
            }
        }

        public System.Drawing.Graphics Graphics
        {
            get
            {
                return this.FGraphics;
            }
        }
    }
}

