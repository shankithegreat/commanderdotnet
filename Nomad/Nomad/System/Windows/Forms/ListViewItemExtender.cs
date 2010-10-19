namespace System.Windows.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class ListViewItemExtender
    {
        public static CanMoveListViewItem CanMove(this ListViewItem item)
        {
            if (item.ListView == null)
            {
                return 0;
            }
            CanMoveListViewItem item2 = 0;
            if (item.Index > 0)
            {
                item2 |= CanMoveListViewItem.Up;
            }
            if (item.Index < (item.ListView.Items.Count - 1))
            {
                item2 |= CanMoveListViewItem.Down;
            }
            List<ListViewItem> groupList = GetGroupList(item.ListView, item.Group);
            if (groupList != null)
            {
                int index = groupList.IndexOf(item);
                if (index > 0)
                {
                    item2 |= CanMoveListViewItem.UpInGroup;
                }
                if ((index >= 0) && (index < (groupList.Count - 1)))
                {
                    item2 |= CanMoveListViewItem.DownInGroup;
                }
            }
            return item2;
        }

        public static void Delete(this ListViewItem item, bool focusNext)
        {
            ListView listView = item.ListView;
            if (listView != null)
            {
                if (listView.VirtualMode)
                {
                    throw new InvalidOperationException();
                }
                int index = item.Index;
                listView.Items.RemoveAt(index);
                if (focusNext)
                {
                    if (index > (listView.Items.Count - 1))
                    {
                        index = listView.Items.Count - 1;
                    }
                    if (index >= 0)
                    {
                        listView.Items[index].Focus(true, false);
                    }
                }
            }
        }

        public static void Focus(this ListViewItem item, bool select, bool ensureVisible)
        {
            item.Focused = true;
            if (select)
            {
                if (item.ListView != null)
                {
                    item.ListView.SelectedItems.Clear();
                }
                item.Selected = true;
            }
            if (ensureVisible)
            {
                item.EnsureVisible();
            }
        }

        private static List<ListViewItem> GetGroupList(ListView list, ListViewGroup group)
        {
            if ((list == null) || (group == null))
            {
                return null;
            }
            List<ListViewItem> list2 = new List<ListViewItem>();
            foreach (ListViewItem item in list.Items)
            {
                if (item.Group == group)
                {
                    list2.Add(item);
                }
            }
            return list2;
        }

        public static int IndexInGroup(this ListViewItem item)
        {
            List<ListViewItem> groupList = GetGroupList(item.ListView, item.Group);
            if (groupList == null)
            {
                return -1;
            }
            return groupList.IndexOf(item);
        }

        private static void Move(ListViewItem item, int delta, bool focus)
        {
            if (item.ListView.VirtualMode)
            {
                throw new InvalidOperationException();
            }
            ListView listView = item.ListView;
            listView.BeginUpdate();
            try
            {
                int index = item.Index;
                listView.Items.RemoveAt(index);
                listView.Items.Insert(index + delta, item);
                if (focus)
                {
                    item.Focus(true, true);
                }
            }
            finally
            {
                listView.EndUpdate();
            }
        }

        public static void MoveDown(this ListViewItem item, bool focus)
        {
            if ((item.Index >= 0) && (item.Index < (item.ListView.Items.Count - 1)))
            {
                Move(item, 1, focus);
            }
        }

        public static void MoveDownInGroup(this ListViewItem item, bool focus)
        {
            List<ListViewItem> groupList = GetGroupList(item.ListView, item.Group);
            if (groupList != null)
            {
                int index = groupList.IndexOf(item);
                if ((index >= 0) && (index < (groupList.Count - 1)))
                {
                    MoveInGroup(item, index, groupList, 1, focus);
                }
            }
        }

        private static void MoveInGroup(ListViewItem item, int indexInGroup, List<ListViewItem> groupList, int delta, bool focus)
        {
            if (item.ListView.VirtualMode)
            {
                throw new InvalidOperationException();
            }
            ListView listView = item.ListView;
            listView.BeginUpdate();
            try
            {
                groupList.Remove(item);
                groupList.Insert(indexInGroup + delta, item);
                listView.Sort(new ListViewExtender.ListViewGroupIndexComparer(item.Group, groupList));
                if (focus)
                {
                    item.Focus(true, true);
                }
            }
            finally
            {
                listView.EndUpdate();
            }
        }

        public static void MoveUp(this ListViewItem item, bool focus)
        {
            if (item.Index >= 1)
            {
                Move(item, -1, focus);
            }
        }

        public static void MoveUpInGroup(this ListViewItem item, bool focus)
        {
            List<ListViewItem> groupList = GetGroupList(item.ListView, item.Group);
            if (groupList != null)
            {
                int index = groupList.IndexOf(item);
                if (index >= 1)
                {
                    MoveInGroup(item, index, groupList, -1, focus);
                }
            }
        }
    }
}

