namespace Nomad.Controls
{
    using System;
    using System.Drawing;

    public class ThrobberRenderEventArgs : EventArgs
    {
        public readonly Rectangle Bounds;
        public readonly bool Enabled;
        public readonly System.Drawing.Graphics Graphics;
        public int Position;

        public ThrobberRenderEventArgs(System.Drawing.Graphics graphics, Rectangle bounds, int position, bool enabled)
        {
            this.Graphics = graphics;
            this.Bounds = bounds;
            this.Position = position;
            this.Enabled = enabled;
        }
    }
}

