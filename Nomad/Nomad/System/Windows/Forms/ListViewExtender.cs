namespace System.Windows.Forms
{
    using Microsoft;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public static class ListViewExtender
    {
        public static DragDropEffects DoDragMove(this ListView list, ListViewItem item)
        {
            if (list != item.ListView)
            {
                throw new ArgumentException();
            }
            ListViewItemDrag drag = new ListViewItemDrag(item);
            return drag.DoDragMove();
        }

        public static void Sort(this ListView list, IComparer listViewComparer)
        {
            list.ListViewItemSorter = listViewComparer;
            list.Sort();
            list.ListViewItemSorter = null;
        }

        internal class ListViewGroupIndexComparer : IComparer
        {
            private ListViewGroup Group;
            private IList List;

            internal ListViewGroupIndexComparer(ListViewGroup group, IList list)
            {
                this.Group = group;
                this.List = list;
            }

            public int Compare(object x, object y)
            {
                ListViewItem item = (ListViewItem) x;
                ListViewItem item2 = (ListViewItem) y;
                int num = 0;
                if ((item.Group == this.Group) && (item2.Group == this.Group))
                {
                    int index = this.List.IndexOf(item);
                    int num3 = this.List.IndexOf(item2);
                    num = index - num3;
                }
                else
                {
                    if (item.Group == this.Group)
                    {
                        return -1;
                    }
                    if (item2.Group == this.Group)
                    {
                        return 1;
                    }
                }
                return num;
            }
        }

        private class ListViewItemDrag
        {
            private int FDragMarkY = -1;
            private IList GroupList;
            private ListViewItem Item;
            private ListView List;
            private const int ScrollMargin = 0x10;
            private Timer ScrollTimer;

            public ListViewItemDrag(ListViewItem item)
            {
                if (item == null)
                {
                    throw new ArgumentNullException();
                }
                if (item.ListView == null)
                {
                    throw new InvalidOperationException();
                }
                if (item.ListView.View != View.Details)
                {
                    throw new InvalidOperationException();
                }
                this.Item = item;
                this.List = this.Item.ListView;
                if ((this.List.ShowGroups && OS.IsWinXP) && (item.Group != null))
                {
                    this.GroupList = new List<ListViewItem>();
                    foreach (ListViewItem item2 in this.List.Items)
                    {
                        if (item2.Group == this.Item.Group)
                        {
                            this.GroupList.Add(item2);
                        }
                    }
                }
                else
                {
                    this.GroupList = this.List.Items;
                }
                this.ScrollTimer = new Timer();
                this.ScrollTimer.Interval = 150;
                this.ScrollTimer.Tick += new EventHandler(this.ScrollTimer_Tick);
            }

            public DragDropEffects DoDragMove()
            {
                DragDropEffects effects;
                this.List.DragEnter += new DragEventHandler(this.ListView_DragEnter);
                this.List.DragDrop += new DragEventHandler(this.ListView_DragDrop);
                this.List.DragOver += new DragEventHandler(this.ListView_DragOver);
                this.List.DragLeave += new EventHandler(this.ListView_DragLeave);
                try
                {
                    effects = this.List.DoDragDrop(this, DragDropEffects.Move);
                }
                finally
                {
                    this.ScrollTimer.Stop();
                    this.List.DragEnter -= new DragEventHandler(this.ListView_DragEnter);
                    this.List.DragDrop -= new DragEventHandler(this.ListView_DragDrop);
                    this.List.DragOver -= new DragEventHandler(this.ListView_DragOver);
                    this.List.DragLeave -= new EventHandler(this.ListView_DragLeave);
                }
                return effects;
            }

            private void DrawReversibleDropItemMark(int y)
            {
                ControlPaint.DrawReversibleLine(this.List.PointToScreen(new Point(0, y)), this.List.PointToScreen(new Point(this.List.ClientRectangle.Right, y)), Color.Black);
            }

            private int IndexOf(ListViewItem item)
            {
                if (this.GroupList == this.List.Items)
                {
                    return item.Index;
                }
                return this.GroupList.IndexOf(item);
            }

            private void ListView_DragDrop(object sender, DragEventArgs e)
            {
                if (e.Data.GetData(typeof(ListViewExtender.ListViewItemDrag)) == this)
                {
                    this.ScrollTimer.Stop();
                    Point point = this.List.PointToClient(new Point(e.X, e.Y));
                    ListViewItem itemAt = this.List.GetItemAt(point.X, point.Y);
                    if ((itemAt != null) && (itemAt.Group == this.Item.Group))
                    {
                        int index = this.IndexOf(this.Item);
                        int num2 = this.IndexOf(itemAt);
                        if ((num2 >= 0) && (index != num2))
                        {
                            this.List.BeginUpdate();
                            try
                            {
                                this.GroupList.Remove(this.Item);
                                this.GroupList.Insert(num2, this.Item);
                                if (this.GroupList != this.List.Items)
                                {
                                    this.List.Sort(new ListViewExtender.ListViewGroupIndexComparer(this.Item.Group, this.GroupList));
                                }
                                this.Item.Focus(true, false);
                            }
                            finally
                            {
                                this.List.EndUpdate();
                            }
                        }
                    }
                    this.DragMarkY = -1;
                }
            }

            private void ListView_DragEnter(object sender, DragEventArgs e)
            {
                System.Type format = typeof(ListViewExtender.ListViewItemDrag);
                if (e.Data.GetDataPresent(format) && (e.Data.GetData(format) == this))
                {
                    e.Effect = DragDropEffects.Move;
                }
            }

            private void ListView_DragLeave(object sender, EventArgs e)
            {
                this.ScrollTimer.Stop();
                this.DragMarkY = -1;
            }

            private void ListView_DragOver(object sender, DragEventArgs e)
            {
                System.Type format = typeof(ListViewExtender.ListViewItemDrag);
                if (e.Data.GetDataPresent(format) && (e.Data.GetData(format) == this))
                {
                    int index = -1;
                    int num2 = -1;
                    Point point = this.List.PointToClient(new Point(e.X, e.Y));
                    ListViewItem itemAt = this.List.GetItemAt(point.X, point.Y);
                    if (itemAt != null)
                    {
                        if (itemAt.Group == this.Item.Group)
                        {
                            index = this.IndexOf(this.Item);
                            num2 = this.IndexOf(itemAt);
                            if ((num2 < 0) || (index == num2))
                            {
                                itemAt = null;
                            }
                        }
                        else
                        {
                            itemAt = null;
                        }
                    }
                    int num3 = -1;
                    if (itemAt == null)
                    {
                        e.Effect = DragDropEffects.None;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.Move;
                        if ((point.Y < 0x10) && (num2 > 0))
                        {
                            this.ScrollTimer.Start();
                        }
                        else
                        {
                            if ((point.Y > (this.List.ClientRectangle.Bottom - 0x10)) && (num2 < (this.GroupList.Count - 1)))
                            {
                                this.ScrollTimer.Start();
                            }
                            num3 = (num2 < index) ? itemAt.Bounds.Top : itemAt.Bounds.Bottom;
                        }
                    }
                    this.DragMarkY = num3;
                }
            }

            private void ScrollTimer_Tick(object sender, EventArgs e)
            {
                Point point = this.List.PointToClient(Cursor.Position);
                ListViewItem itemAt = this.List.GetItemAt(point.X, point.Y);
                if (itemAt != null)
                {
                    int index = this.IndexOf(itemAt);
                    if ((point.Y < 0x10) && (index > 0))
                    {
                        itemAt = (ListViewItem) this.GroupList[index - 1];
                    }
                    else if ((point.Y > (this.List.ClientRectangle.Bottom - 0x10)) && (index < (this.GroupList.Count - 1)))
                    {
                        itemAt = (ListViewItem) this.GroupList[index + 1];
                    }
                    else
                    {
                        itemAt = null;
                    }
                }
                if (itemAt == null)
                {
                    this.ScrollTimer.Stop();
                }
                else
                {
                    this.DragMarkY = -1;
                    itemAt.EnsureVisible();
                }
            }

            private int DragMarkY
            {
                get
                {
                    return this.FDragMarkY;
                }
                set
                {
                    if (this.FDragMarkY != value)
                    {
                        if (this.FDragMarkY >= 0)
                        {
                            this.DrawReversibleDropItemMark(this.FDragMarkY);
                        }
                        this.FDragMarkY = value;
                        if ((this.FDragMarkY < this.List.ClientRectangle.Top) || (this.FDragMarkY > this.List.ClientRectangle.Bottom))
                        {
                            this.FDragMarkY = -1;
                        }
                        if (this.FDragMarkY >= 0)
                        {
                            this.DrawReversibleDropItemMark(this.FDragMarkY);
                        }
                    }
                }
            }
        }
    }
}

