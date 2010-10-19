namespace Nomad.Controls
{
    using System;

    public interface IProgressBarRenderer
    {
        void DrawBackground(ProgressBarRenderEventArgs e);
        void DrawBarValue(ProgressBarValueRenderEventArgs e);
        void DrawMarquee(ProgressBarMarqueeRenderEventArgs e);
        bool UpdateMarquee(ProgressBarMarqueeEventArgs e);
    }
}

