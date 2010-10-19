namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class PostDrawTreeNodeEventArgs : NodeStateEventArgs
    {
        private Rectangle FBounds;
        private System.Drawing.Graphics FGraphics;

        public PostDrawTreeNodeEventArgs(System.Drawing.Graphics g, TreeNode node, Rectangle bounds, TreeNodeStates state) : base(node, state)
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

