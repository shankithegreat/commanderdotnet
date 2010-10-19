namespace Nomad.FileSystem.Virtual
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.Commons.Threading;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;

    public sealed class VirtualIcon
    {
        public static Nomad.FileSystem.Virtual.DelayedExtractMode DelayedExtractMode = Nomad.FileSystem.Virtual.DelayedExtractMode.OnSlowDrivesOnly;
        public static readonly LazyInit<WorkQueue> ExtractIconQueue;
        public static Nomad.FileSystem.Virtual.IconOptions IconOptions = (Nomad.FileSystem.Virtual.IconOptions.ShowOverlayIcons | Nomad.FileSystem.Virtual.IconOptions.ExtractIcons);

        static VirtualIcon()
        {
            ExtractIconQueue = new LazyInit<WorkQueue>(delegate {
                ThreadQueue queue = new ThreadQueue(ApartmentState.STA, true);
                queue.Error += delegate (object sender, ExceptionEventArgs e) {
                    Nomad.Trace.Error.TraceException(TraceEventType.Critical, e.ErrorException);
                };
                return queue;
            });
        }

        public static bool CheckIconOption(Nomad.FileSystem.Virtual.IconOptions option)
        {
            return ((IconOptions & option) == option);
        }

        public static VirtualHighligher GetHighlighter(IVirtualItem item)
        {
            IVirtualItemUI mui = item as IVirtualItemUI;
            return ((mui != null) ? mui.Highlighter : null);
        }

        public static Image GetIcon(IVirtualItem item, Size size)
        {
            return GetIcon(item, size, 0);
        }

        public static Image GetIcon(IVirtualItem item, Size size, IconStyle style)
        {
            IVirtualItemUI mui = item as IVirtualItemUI;
            if (mui != null)
            {
                if ((mui.Highlighter != null) && (mui.Highlighter.IconType == HighlighterIconType.HighlighterIcon))
                {
                    return mui.Highlighter.GetIcon(size);
                }
                return mui.GetIcon(size, style);
            }
            return null;
        }

        public static Image GetThumbnail(IVirtualItem item, Size thumbSize)
        {
            ExtensiblePropertyProvider provider = item as ExtensiblePropertyProvider;
            if (provider != null)
            {
                foreach (IGetVirtualProperty property in provider.GetProviders(0x15))
                {
                    Image image;
                    IGetThumbnail thumbnail = property as IGetThumbnail;
                    if (thumbnail != null)
                    {
                        image = thumbnail.GetThumbnail(thumbSize);
                    }
                    else
                    {
                        image = property[0x15] as Image;
                    }
                    if (image != null)
                    {
                        return image;
                    }
                }
            }
            return (item[0x15] as Image);
        }
    }
}

