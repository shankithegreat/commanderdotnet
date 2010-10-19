namespace Nomad.Controls
{
    using System;
    using System.Drawing;

    public class ProgressBarValueRenderEventArgs : ProgressBarRenderEventArgs
    {
        public readonly int Maximum;
        public readonly int Minimum;
        public readonly int Value;

        public ProgressBarValueRenderEventArgs(Graphics graphics, VistaProgressBar progressBar) : base(graphics, progressBar)
        {
            this.Value = base.ProgressBar.Value;
            this.Minimum = base.ProgressBar.Minimum;
            this.Maximum = base.ProgressBar.Maximum;
        }
    }
}

