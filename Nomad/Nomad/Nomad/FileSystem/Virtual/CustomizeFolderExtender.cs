namespace Nomad.FileSystem.Virtual
{
    using System;
    using System.Runtime.CompilerServices;

    public static class CustomizeFolderExtender
    {
        public static bool IsEmpty(this ICustomizeFolder customize)
        {
            return ((((!customize.AutoSizeColumns.HasValue && ((customize.Columns == null) || (customize.Columns.Length == 0))) && (((customize.Icon == null) && (customize.Filter == null)) && ((customize.Sort == null) && !customize.ListColumnCount.HasValue))) && ((customize.ThumbnailSize.IsEmpty && customize.ThumbnailSpacing.IsEmpty) && (!customize.View.HasValue && customize.BackColor.IsEmpty))) && customize.ForeColor.IsEmpty);
        }

        public static void Set(this ICustomizeFolder dest, ICustomizeFolder source)
        {
            dest.Set(source, CustomizeFolderParts.All);
        }

        public static void Set(this ICustomizeFolder dest, ICustomizeFolder source, CustomizeFolderParts parts)
        {
            if ((parts & CustomizeFolderParts.Columns) > 0)
            {
                dest.AutoSizeColumns = source.AutoSizeColumns;
                dest.Columns = source.Columns;
            }
            if ((parts & CustomizeFolderParts.Icon) > 0)
            {
                dest.Icon = source.Icon;
            }
            if ((parts & CustomizeFolderParts.Filter) > 0)
            {
                dest.Filter = source.Filter;
            }
            if ((parts & CustomizeFolderParts.Sort) > 0)
            {
                dest.Sort = source.Sort;
            }
            if ((parts & CustomizeFolderParts.ListColumnCount) > 0)
            {
                dest.ListColumnCount = source.ListColumnCount;
            }
            if ((parts & CustomizeFolderParts.ThumbnailSize) > 0)
            {
                dest.ThumbnailSize = source.ThumbnailSize;
                dest.ThumbnailSpacing = source.ThumbnailSpacing;
            }
            if ((parts & CustomizeFolderParts.View) > 0)
            {
                dest.View = source.View;
            }
            if ((parts & CustomizeFolderParts.Colors) > 0)
            {
                dest.BackColor = source.BackColor;
                dest.ForeColor = source.ForeColor;
            }
        }
    }
}

