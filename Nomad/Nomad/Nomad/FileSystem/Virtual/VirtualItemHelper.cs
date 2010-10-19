namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    public static class VirtualItemHelper
    {
        public static bool CanCreateInFolder(IVirtualFolder folder)
        {
            return ((folder is ICreateVirtualFile) || (folder is ICreateVirtualFolder));
        }

        public static CanMoveResult CanCreateLinkIn(IEnumerable<IVirtualItem> source, IVirtualFolder dest)
        {
            return CanDoAction<ICreateVirtualLink>(source, dest, delegate (ICreateVirtualLink createLink, IVirtualFolder destFolder) {
                return createLink.CanCreateLinkIn(destFolder) != LinkType.None;
            });
        }

        private static CanMoveResult CanDoAction<T>(IEnumerable<IVirtualItem> source, IVirtualFolder dest, Func<T, IVirtualFolder, bool> check) where T: class
        {
            CanMoveResult all = CanMoveResult.All;
            foreach (IVirtualItem item in source)
            {
                T local = item as T;
                if (!((local == null) ? true : !check(local, dest)))
                {
                    if (all != CanMoveResult.All)
                    {
                        return CanMoveResult.Several;
                    }
                }
                else
                {
                    all = CanMoveResult.None;
                }
            }
            return all;
        }

        public static CanMoveResult CanMoveTo(IEnumerable<IVirtualItem> source, IVirtualFolder dest)
        {
            return CanDoAction<IChangeVirtualItem>(source, dest, delegate (IChangeVirtualItem changeItem, IVirtualFolder destFolder) {
                return changeItem.CanMoveTo(destFolder);
            });
        }

        public static bool Equals(IVirtualItem x, IVirtualItem y)
        {
            return (((x != null) && (y != null)) && x.Equals(y));
        }

        public static IVirtualFolder GetFolderRoot(IVirtualFolder folder)
        {
            IGetVirtualRoot root = folder as IGetVirtualRoot;
            return ((root != null) ? root.Root : null);
        }

        public static Color GetForeColor(IVirtualItem item, Color defaultColor)
        {
            IVirtualItemUI mui = item as IVirtualItemUI;
            if (mui != null)
            {
                ListViewHighlighter highlighter = mui.Highlighter as ListViewHighlighter;
                if (!((highlighter == null) || highlighter.ForeColor.IsEmpty))
                {
                    return highlighter.ForeColor;
                }
            }
            return defaultColor;
        }

        public static bool IsRoot(IVirtualFolder folder)
        {
            return ((folder != null) && folder.Equals(GetFolderRoot(folder)));
        }

        public static void ResetSystemAttributes(IVirtualItem item)
        {
            IChangeVirtualItem item2 = item as IChangeVirtualItem;
            if (((item2 != null) && ((item.Attributes & (FileAttributes.System | FileAttributes.Hidden | FileAttributes.ReadOnly)) > 0)) && item2.CanSetProperty(6))
            {
                item2[6] = FileAttributes.Normal | (item.Attributes & (FileAttributes.Encrypted | FileAttributes.Compressed));
            }
        }
    }
}

