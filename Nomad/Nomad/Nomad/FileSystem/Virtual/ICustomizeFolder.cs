namespace Nomad.FileSystem.Virtual
{
    using Nomad.Commons.Drawing;
    using Nomad.Configuration;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Drawing;

    public interface ICustomizeFolder
    {
        bool? AutoSizeColumns { get; set; }

        Color BackColor { get; set; }

        ListViewColumnInfo[] Columns { get; set; }

        IVirtualItemFilter Filter { get; set; }

        Color ForeColor { get; set; }

        IconLocation Icon { get; set; }

        int? ListColumnCount { get; set; }

        VirtualItemComparer Sort { get; set; }

        Size ThumbnailSize { get; set; }

        Size ThumbnailSpacing { get; set; }

        CustomizeFolderParts UpdatableParts { get; }

        PanelView? View { get; set; }
    }
}

