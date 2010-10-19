namespace Nomad.Controls
{
    using Microsoft;
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class ListViewEx : System.Windows.Forms.ListView
    {
        private LVIS CachedState;
        private static readonly object EVENT_DRAWCOLUMNHEADER;
        private static readonly object EVENT_DRAWITEM;
        private static readonly object EVENT_DRAWSUBITEM;
        private static readonly object EventBeforeLabelEdit = new object();
        private static readonly object EventColumnRightClick = new object();
        private static readonly object EventGetItemColors = new object();
        private static readonly object EventGetItemState = new object();
        private static readonly object EventItemTooltip = new object();
        private static readonly object EventPostDrawItem = new object();
        private static readonly object EventPostDrawSubItem = new object();
        private ImageList FHeaderImageList;
        private int FSortColumn = -1;
        private ListSortDirection FSortDirection;
        private StateEx FState = StateEx.CanResizeColumns;
        private static readonly MethodInfo GetSubItemRectMethod;
        private static readonly PropertyInfo GroupIdProperty;
        private ListViewHeader Header;
        private Point LastMouseUpLocation;
        private int LastMouseUpTickCount;
        private const int UM_CREATEHEADER = 0x1601;
        private const int UM_FIXFIRSTCOLUMN = 0x1600;

        [Category("Action")]
        public event BeforeLabelEditEventHandler BeforeLabelEdit
        {
            add
            {
                base.Events.AddHandler(EventBeforeLabelEdit, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventBeforeLabelEdit, value);
            }
        }

        [Category("Action")]
        public event ColumnClickEventHandler ColumnRightClick
        {
            add
            {
                base.Events.AddHandler(EventColumnRightClick, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventColumnRightClick, value);
            }
        }

        [Category("Behavior")]
        public event EventHandler<GetItemColorsEventArgs> GetItemColors
        {
            add
            {
                base.Events.AddHandler(EventGetItemColors, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventGetItemColors, value);
            }
        }

        [Category("Behavior")]
        public event EventHandler<GetItemStateEventArgs> GetItemState
        {
            add
            {
                base.Events.AddHandler(EventGetItemState, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventGetItemState, value);
            }
        }

        [Category("Action")]
        public event EventHandler<ItemTooltipEventArgs> ItemTooltip
        {
            add
            {
                base.Events.AddHandler(EventItemTooltip, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventItemTooltip, value);
            }
        }

        [Category("Behavior")]
        public event EventHandler<PostDrawListViewItemEventArgs> PostDrawItem
        {
            add
            {
                base.Events.AddHandler(EventPostDrawItem, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventPostDrawItem, value);
            }
        }

        [Category("Behavior")]
        public event EventHandler<PostDrawListViewSubItemEventArgs> PostDrawSubItem
        {
            add
            {
                base.Events.AddHandler(EventPostDrawSubItem, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventPostDrawSubItem, value);
            }
        }

        static ListViewEx()
        {
            System.Type type = typeof(System.Windows.Forms.ListView);
            GetSubItemRectMethod = type.GetMethod("GetSubItemRect", BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[] { typeof(int), typeof(int) }, null);
            FieldInfo field = type.GetField("EVENT_DRAWCOLUMNHEADER", BindingFlags.NonPublic | BindingFlags.Static);
            if (field != null)
            {
                EVENT_DRAWCOLUMNHEADER = field.GetValue(null);
            }
            FieldInfo info2 = type.GetField("EVENT_DRAWITEM", BindingFlags.NonPublic | BindingFlags.Static);
            if (info2 != null)
            {
                EVENT_DRAWITEM = info2.GetValue(null);
            }
            FieldInfo info3 = type.GetField("EVENT_DRAWSUBITEM", BindingFlags.NonPublic | BindingFlags.Static);
            if (info3 != null)
            {
                EVENT_DRAWSUBITEM = info3.GetValue(null);
            }
            GroupIdProperty = typeof(ListViewGroup).GetProperty("ID", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public ListViewEx()
        {
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            if (!OS.IsWinXP)
            {
                base.SetStyle(ControlStyles.UserPaint, true);
            }
        }

        private bool CheckState(StateEx state)
        {
            return ((this.FState & state) == state);
        }

        private void CreateHeader()
        {
            if (this.Header == null)
            {
                IntPtr headerWnd = Windows.SendMessage(base.Handle, 0x101f, IntPtr.Zero, IntPtr.Zero);
                if (headerWnd != IntPtr.Zero)
                {
                    this.Header = new ListViewHeader(this, headerWnd);
                    this.Header.SetImageList(this.HeaderImageList);
                }
            }
            if ((this.Header != null) && (this.FSortColumn >= 0))
            {
                this.Header.SetSortColumn(this.FSortColumn, this.FSortDirection);
            }
        }

        private void DrawColumnLines(IntPtr hdc)
        {
            using (Graphics graphics = Graphics.FromHdc(hdc))
            {
                System.Drawing.Color color = VisualStyleRenderer.IsSupported ? VisualStyleInformation.TextControlBorder : System.Drawing.Color.DarkGray;
                using (Pen pen = new Pen(color))
                {
                    int num = base.GetItemRect(base.TopItem.Index).Left - 1;
                    int[] columnsOrder = this.GetColumnsOrder();
                    for (int i = 0; i < (columnsOrder.Length - 1); i++)
                    {
                        num += base.Columns[columnsOrder[i]].Width;
                        graphics.DrawLine(pen, num, 0, num, base.ClientSize.Height);
                    }
                }
            }
        }

        private void FixFirstColumn(ListViewItem topItem, ListViewItem focusedItem)
        {
            if (topItem != null)
            {
                switch (base.View)
                {
                    case View.Details:
                    case View.List:
                        if (topItem.GetBounds(ItemBoundsPortion.Entire).Left != 0)
                        {
                            bool focused = this.Focused;
                            int num = (focusedItem != null) ? focusedItem.Index : -1;
                            using ((base.Parent == null) ? null : new LockWindowRedraw(base.Parent, true))
                            {
                                base.RecreateHandle();
                                if (num >= 0)
                                {
                                    base.Items[num].Focused = true;
                                    base.Items[num].EnsureVisible();
                                }
                                if (focused)
                                {
                                    base.Focus();
                                    Windows.SetFocus(base.Handle);
                                }
                            }
                        }
                        break;
                }
            }
        }

        private int[] GetColumnsOrder()
        {
            int[] numArray = new int[base.Columns.Count];
            GCHandle handle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
            try
            {
                Windows.SendMessage(base.Handle, 0x103b, (IntPtr) numArray.Length, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
            return numArray;
        }

        protected Rectangle GetItemRectOrEmpty(int index)
        {
            Microsoft.Win32.RECT rect;
            if ((index < 0) || (index >= base.Items.Count))
            {
                return Rectangle.Empty;
            }
            if ((base.View == View.Details) && (base.Columns.Count == 0))
            {
                return Rectangle.Empty;
            }
            if (!CommCtrl.ListView_GetItemRect(base.Handle, index, 0, out rect))
            {
                return Rectangle.Empty;
            }
            return rect.ToRectangle();
        }

        public IEnumerable<ColumnHeader> GetOrderedColumns()
        {
            return new <GetOrderedColumns>d__3(-2) { <>4__this = this };
        }

        protected Rectangle GetSubItemRect(int itemIndex, int subItemIndex)
        {
            if (GetSubItemRectMethod != null)
            {
                return (Rectangle) GetSubItemRectMethod.Invoke(this, new object[] { itemIndex, subItemIndex });
            }
            return Rectangle.Empty;
        }

        public ListViewHitTestInfo2 HitTest(Point point)
        {
            ListViewHitTestInfo info = base.HitTest(point);
            int columnIndex = -1;
            if ((base.View == View.Details) && (info.SubItem != null))
            {
                columnIndex = CommCtrl.ListView_SubItemHitTest(base.Handle, point);
            }
            return new ListViewHitTestInfo2(info.Item, info.SubItem, columnIndex, info.Location);
        }

        public ListViewHitTestInfo2 HitTest(int x, int y)
        {
            return this.HitTest(new Point(x, y));
        }

        protected override void OnBeforeLabelEdit(LabelEditEventArgs e)
        {
            BeforeLabelEditEventArgs args;
            bool SetMaxLength;
            bool SetText;
            bool SetSelection;
            BeforeLabelEditEventHandler handler = base.Events[EventBeforeLabelEdit] as BeforeLabelEditEventHandler;
            if (handler != null)
            {
                args = new BeforeLabelEditEventArgs(e.Item, e.Label);
                handler(this, args);
                if (args.CancelEdit)
                {
                    e.CancelEdit = args.CancelEdit;
                    base.OnBeforeLabelEdit(e);
                }
                else
                {
                    SetMaxLength = args.MaxLength > 0;
                    SetText = args.Label != e.Label;
                    SetSelection = !string.IsNullOrEmpty(args.Label) && ((args.SelectionStart > 0) || (args.SelectionLength != args.Label.Length));
                    if ((SetMaxLength || SetText) || SetSelection)
                    {
                        base.BeginInvoke(delegate {
                            IntPtr hWnd = CommCtrl.ListView_GetEditControl(this.Handle);
                            if (hWnd != IntPtr.Zero)
                            {
                                if (SetMaxLength)
                                {
                                    Windows.SendMessage(hWnd, 0xc5, (IntPtr) args.MaxLength, IntPtr.Zero);
                                }
                                if (SetText)
                                {
                                    Windows.SetWindowText(hWnd, args.Label);
                                }
                                if (SetText || SetSelection)
                                {
                                    Windows.SendMessage(hWnd, 0xb1, (IntPtr) args.SelectionStart, (IntPtr) (args.SelectionStart + args.SelectionLength));
                                }
                            }
                        });
                    }
                }
            }
        }

        protected virtual void OnColumnRightClick(ColumnClickEventArgs e)
        {
            ColumnClickEventHandler handler = base.Events[EventColumnRightClick] as ColumnClickEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            if ((EVENT_DRAWCOLUMNHEADER != null) && (base.Events[EVENT_DRAWCOLUMNHEADER] == null))
            {
                e.DrawDefault = true;
            }
            base.OnDrawColumnHeader(e);
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            if ((EVENT_DRAWITEM != null) && (base.Events[EVENT_DRAWITEM] == null))
            {
                e.DrawDefault = true;
            }
            base.OnDrawItem(e);
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            if ((EVENT_DRAWSUBITEM != null) && (base.Events[EVENT_DRAWSUBITEM] == null))
            {
                e.DrawDefault = true;
            }
            base.OnDrawSubItem(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            this.SetState(StateEx.HasFocused | StateEx.IsFocused, true);
            try
            {
                base.OnEnter(e);
            }
            finally
            {
                this.SetState(StateEx.HasFocused, false);
            }
        }

        protected virtual void OnGetItemColors(GetItemColorsEventArgs e)
        {
            if ((this.ExplorerTheme && !base.VirtualMode) && ((e.State & (ListViewItemStates.Hot | ListViewItemStates.Focused | ListViewItemStates.Selected)) > 0))
            {
                e.ForeColor = e.Item.ForeColor;
            }
            EventHandler<GetItemColorsEventArgs> handler = base.Events[EventGetItemColors] as EventHandler<GetItemColorsEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnGetItemState(GetItemStateEventArgs e)
        {
            EventHandler<GetItemStateEventArgs> handler = base.Events[EventGetItemState] as EventHandler<GetItemStateEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!base.DesignMode && this.ExplorerTheme)
            {
                UxTheme.SetWindowTheme(base.Handle, "explorer", null);
            }
            LVIS lvis = CommCtrl.ListView_GetCallbackMask(base.Handle);
            CommCtrl.ListView_SetCallbackMask(base.Handle, (lvis | LVIS.LVIS_CUT) | LVIS.LVIS_DROPHILITED);
            IntPtr hWnd = Windows.SendMessage(base.Handle, 0x104e, IntPtr.Zero, IntPtr.Zero);
            Windows.SendMessage(hWnd, 0x403, (IntPtr) 3L, (IntPtr) 0x3e8);
            Windows.SendMessage(hWnd, 0x403, (IntPtr) 1L, (IntPtr) 0x3e8);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            this.Header = null;
        }

        protected virtual void OnItemTooltip(ItemTooltipEventArgs e)
        {
            EventHandler<ItemTooltipEventArgs> handler = base.Events[EventItemTooltip] as EventHandler<ItemTooltipEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            this.SetState(StateEx.HasFocused, true);
            this.SetState(StateEx.IsFocused, false);
            try
            {
                base.OnLeave(e);
            }
            finally
            {
                this.SetState(StateEx.HasFocused, false);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (base.GetItemAt(e.X, e.Y) == null)
            {
                int tickCount = Environment.TickCount;
                int num2 = tickCount - this.LastMouseUpTickCount;
                this.LastMouseUpTickCount = tickCount;
                if (num2 < SystemInformation.DoubleClickTime)
                {
                    Size doubleClickSize = SystemInformation.DoubleClickSize;
                    Rectangle rectangle = new Rectangle(this.LastMouseUpLocation.X - (doubleClickSize.Width / 2), this.LastMouseUpLocation.Y - (doubleClickSize.Height / 2), doubleClickSize.Width, doubleClickSize.Height);
                    if (rectangle.Contains(e.Location))
                    {
                        e = new MouseEventArgs(e.Button, 2, e.X, e.Y, e.Delta);
                        this.OnDoubleClick(e);
                        this.OnMouseDoubleClick(e);
                        this.LastMouseUpTickCount = 0;
                    }
                }
                this.LastMouseUpLocation = e.Location;
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (base.GetStyle(ControlStyles.UserPaint))
            {
                Message m = new Message {
                    HWnd = base.Handle,
                    Msg = 0x318,
                    LParam = (IntPtr) 4L,
                    WParam = e.Graphics.GetHdc()
                };
                this.DefWndProc(ref m);
                e.Graphics.ReleaseHdc(m.WParam);
            }
            base.OnPaint(e);
        }

        protected virtual void OnPostDrawItem(PostDrawListViewItemEventArgs e)
        {
            EventHandler<PostDrawListViewItemEventArgs> handler = base.Events[EventPostDrawItem] as EventHandler<PostDrawListViewItemEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPostDrawSubItem(PostDrawListViewSubItemEventArgs e)
        {
            EventHandler<PostDrawListViewSubItemEventArgs> handler = base.Events[EventPostDrawSubItem] as EventHandler<PostDrawListViewSubItemEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void RedrawItem(int index, bool invalidateOnly)
        {
            base.RedrawItems(index, index, invalidateOnly);
        }

        public void SetColumnWidth(int column, int width)
        {
            if ((base.View == View.List) && (column > 0))
            {
                throw new InvalidOperationException();
            }
            int num = (int) Windows.SendMessage(base.Handle, 0x101d, (IntPtr) column, IntPtr.Zero);
            if (num != width)
            {
                bool flag = false;
                if (base.VirtualMode && (base.View == View.List))
                {
                    int num2 = (int) Windows.SendMessage(base.Handle, 0x1027, IntPtr.Zero, IntPtr.Zero);
                    flag = num2 > 0;
                }
                if (flag)
                {
                    LockWindowRedraw redraw;
                    int num3 = (base.FocusedItem != null) ? base.FocusedItem.Index : -1;
                    using (redraw = new LockWindowRedraw(this, false))
                    {
                        Windows.SendMessage(base.Handle, 0x1013, IntPtr.Zero, IntPtr.Zero);
                        Windows.SendMessage(base.Handle, 0x101e, IntPtr.Zero, (IntPtr) width);
                    }
                    if (num3 >= 0)
                    {
                        using (redraw = new LockWindowRedraw(this, false))
                        {
                            Windows.SendMessage(base.Handle, 0x1013, (IntPtr) num3, IntPtr.Zero);
                        }
                    }
                }
                else
                {
                    Windows.SendMessage(base.Handle, 0x101e, (IntPtr) column, (IntPtr) width);
                }
            }
        }

        public void SetIconSpacing(Size spacing)
        {
            this.SetIconSpacing(spacing.Width, spacing.Height);
        }

        public void SetIconSpacing(int cx, int cy)
        {
            CommCtrl.ListView_SetIconSpacing(base.Handle, cx, cy);
        }

        public void SetSortColumn(int column, ListSortDirection direction)
        {
            if (this.FSortColumn != column)
            {
                if ((this.Header != null) && (this.FSortColumn >= 0))
                {
                    this.Header.ResetSortColumn(this.FSortColumn);
                }
                this.FSortColumn = column;
            }
            else if (this.FSortDirection == direction)
            {
                return;
            }
            this.FSortDirection = direction;
            if ((this.Header != null) && (this.FSortColumn >= 0))
            {
                this.Header.SetSortColumn(this.FSortColumn, this.FSortDirection);
            }
        }

        private void SetState(StateEx state, bool value)
        {
            if (value)
            {
                this.FState |= state;
            }
            else
            {
                this.FState &= ~state;
            }
        }

        private void WmCustomDraw(ref Message m)
        {
            int itemSpec;
            Rectangle itemRectOrEmpty;
            ListViewItemStates itemState;
            Graphics graphics;
            switch (Microsoft.Win32.NMCUSTOMDRAW.GetDrawStage(m.LParam))
            {
                case CDDS.CDDS_PREPAINT:
                    if (this.CanDrawColumnLines)
                    {
                        m.Result = (IntPtr) (((int) m.Result) | 0x10);
                    }
                    break;

                case CDDS.CDDS_POSTPAINT:
                    if (this.CanDrawColumnLines)
                    {
                        this.DrawColumnLines(Microsoft.Win32.NMCUSTOMDRAW.GetHdc(m.LParam));
                    }
                    break;

                case CDDS.CDDS_ITEMPREPAINT:
                {
                    System.Drawing.Color backColor = Microsoft.Win32.NMLVCUSTOMDRAW.GetBackColor(m.LParam);
                    System.Drawing.Color foreColor = Microsoft.Win32.NMLVCUSTOMDRAW.GetForeColor(m.LParam);
                    itemState = (ListViewItemStates) Microsoft.Win32.NMCUSTOMDRAW.GetItemState(m.LParam);
                    if ((itemState & ListViewItemStates.Selected) > 0)
                    {
                        this.CachedState = CommCtrl.ListView_GetItemState(base.Handle, (int) Microsoft.Win32.NMCUSTOMDRAW.GetItemSpec(m.LParam), LVIS.LVIS_SELECTED);
                        if (this.CachedState == 0)
                        {
                            itemState &= ~ListViewItemStates.Selected;
                        }
                    }
                    GetItemColorsEventArgs e = new GetItemColorsEventArgs(this, (int) Microsoft.Win32.NMCUSTOMDRAW.GetItemSpec(m.LParam), itemState, backColor, foreColor);
                    this.OnGetItemColors(e);
                    if (e.BackColor != backColor)
                    {
                        Microsoft.Win32.NMLVCUSTOMDRAW.SetBackColor(m.LParam, e.BackColor);
                    }
                    if (e.ForeColor != foreColor)
                    {
                        Microsoft.Win32.NMLVCUSTOMDRAW.SetForeColor(m.LParam, e.ForeColor);
                    }
                    if (base.OwnerDraw)
                    {
                        CDRF result = (CDRF) ((int) m.Result);
                        if (base.Events[EventPostDrawItem] != null)
                        {
                            result |= CDRF.CDRF_NOTIFYPOSTPAINT;
                        }
                        if ((base.View == View.Details) && (base.Events[EventPostDrawSubItem] != null))
                        {
                            result |= CDRF.CDRF_NOTIFYITEMDRAW;
                        }
                        m.Result = (IntPtr) ((long) result);
                    }
                    break;
                }
                case CDDS.CDDS_ITEMPOSTPAINT:
                    itemSpec = (int) Microsoft.Win32.NMCUSTOMDRAW.GetItemSpec(m.LParam);
                    itemRectOrEmpty = this.GetItemRectOrEmpty(itemSpec);
                    if (base.ClientRectangle.IntersectsWith(itemRectOrEmpty))
                    {
                        itemState = (ListViewItemStates) Microsoft.Win32.NMCUSTOMDRAW.GetItemState(m.LParam);
                        if (((itemState & ListViewItemStates.Selected) > 0) && ((this.CachedState & LVIS.LVIS_SELECTED) == 0))
                        {
                            itemState &= ~ListViewItemStates.Selected;
                        }
                        using (graphics = Graphics.FromHdcInternal(Microsoft.Win32.NMCUSTOMDRAW.GetHdc(m.LParam)))
                        {
                            PostDrawListViewItemEventArgs args2 = new PostDrawListViewItemEventArgs(graphics, this, itemSpec, itemRectOrEmpty, itemState);
                            this.OnPostDrawItem(args2);
                        }
                        break;
                    }
                    break;

                case (CDDS.CDDS_SUBITEM | CDDS.CDDS_ITEMPREPAINT):
                    if (base.OwnerDraw && (base.Events[EventPostDrawSubItem] != null))
                    {
                        m.Result = (IntPtr) (((int) m.Result) | 0x10);
                    }
                    break;

                case (CDDS.CDDS_SUBITEM | CDDS.CDDS_ITEMPOSTPAINT):
                    itemSpec = (int) Microsoft.Win32.NMCUSTOMDRAW.GetItemSpec(m.LParam);
                    itemRectOrEmpty = this.GetItemRectOrEmpty(itemSpec);
                    if (base.ClientRectangle.IntersectsWith(itemRectOrEmpty))
                    {
                        int subItem = Microsoft.Win32.NMLVCUSTOMDRAW.GetSubItem(m.LParam);
                        if (subItem < base.Items[itemSpec].SubItems.Count)
                        {
                            Rectangle subItemRect = this.GetSubItemRect(itemSpec, subItem);
                            if ((subItem == 0) && (base.Items[itemSpec].SubItems.Count > 1))
                            {
                                subItemRect.Width = base.Columns[0].Width;
                            }
                            if (base.ClientRectangle.IntersectsWith(subItemRect))
                            {
                                using (graphics = Graphics.FromHdcInternal(Microsoft.Win32.NMCUSTOMDRAW.GetHdc(m.LParam)))
                                {
                                    PostDrawListViewSubItemEventArgs args3 = new PostDrawListViewSubItemEventArgs(graphics, this, itemSpec, subItem, subItemRect, (ListViewItemStates) Microsoft.Win32.NMCUSTOMDRAW.GetItemState(m.LParam));
                                    this.OnPostDrawSubItem(args3);
                                }
                            }
                        }
                        break;
                    }
                    break;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x18:
                    if (Convert.ToBoolean((int) m.WParam) && (base.View == View.Details))
                    {
                        this.CreateHeader();
                    }
                    break;

                case 0x4e:
                    switch (Microsoft.Win32.NMHDR.GetNotifyCode(m.LParam))
                    {
                        case -326:
                        case -306:
                            if (this.CanResizeColumns)
                            {
                                break;
                            }
                            m.Result = (IntPtr) 1;
                            return;

                        case -325:
                        case -305:
                            if (this.CanResizeColumns)
                            {
                                break;
                            }
                            return;

                        case -5:
                        {
                            Point point = base.PointToClient(WindowsWrapper.GetMessagePos());
                            IntPtr hWnd = Windows.ChildWindowFromPoint(base.Handle, point);
                            if (((hWnd != IntPtr.Zero) && (hWnd != base.Handle)) && (WindowsWrapper.GetClassName(hWnd) == "SysHeader32"))
                            {
                                int column = CommCtrl.Header_GetColumnAt(hWnd, new Point(point.X + Windows.GetScrollPos(base.Handle, SB.SB_HORZ), point.Y));
                                if (column >= 0)
                                {
                                    this.OnColumnRightClick(new ColumnClickEventArgs(column));
                                }
                            }
                            break;
                        }
                    }
                    break;

                case 0x102f:
                    m.LParam = (IntPtr) (((int) m.LParam) | 1);
                    if (base.VirtualMode && ((base.View == View.Details) || (base.View == View.List)))
                    {
                        Windows.PostMessage(m.HWnd, 0x1600, IntPtr.Zero, IntPtr.Zero);
                    }
                    break;

                case 0x1600:
                    if (base.VirtualMode && ((base.View == View.Details) || (base.View == View.List)))
                    {
                        this.FixFirstColumn(base.TopItem, base.FocusedItem);
                    }
                    return;

                case 0x1601:
                    this.CreateHeader();
                    return;

                case 0x204e:
                    switch (Microsoft.Win32.NMHDR.GetNotifyCode(m.LParam))
                    {
                        case -158:
                        case -157:
                            if ((base.View != View.SmallIcon) || (Microsoft.Win32.NMLVGETINFOTIP.GetItem(m.LParam) < (base.VirtualMode ? base.VirtualListSize : base.Items.Count)))
                            {
                                break;
                            }
                            return;

                        case -156:
                            this.SetState(StateEx.MarqueeSelectionActive, true);
                            break;

                        case -16:
                            this.SetState(StateEx.MarqueeSelectionActive, false);
                            break;
                    }
                    break;

                case 0x1091:
                case 0x1093:
                    if (this.CollapsibleGroups)
                    {
                        Microsoft.Win32.LVGROUP.SetMask(m.LParam, LVGF.LVGF_NONE | LVGF.LVGF_STATE, true);
                        Microsoft.Win32.LVGROUP.SetStateMask(m.LParam, LVGS.LVGS_COLLAPSIBLE, true);
                        Microsoft.Win32.LVGROUP.SetState(m.LParam, LVGS.LVGS_COLLAPSIBLE, true);
                    }
                    break;

                case 0x108e:
                {
                    LV_VIEW wParam = (LV_VIEW) ((int) m.WParam);
                    if (wParam == LV_VIEW.LV_VIEW_DETAILS)
                    {
                        Windows.PostMessage(m.HWnd, 0x1601, IntPtr.Zero, IntPtr.Zero);
                    }
                    if (base.VirtualMode && ((wParam == LV_VIEW.LV_VIEW_DETAILS) || (wParam == LV_VIEW.LV_VIEW_LIST)))
                    {
                        Windows.PostMessage(m.HWnd, 0x1600, IntPtr.Zero, IntPtr.Zero);
                    }
                    break;
                }
            }
            base.WndProc(ref m);
            if (m.Msg == 0x204e)
            {
                int notifyCode = Microsoft.Win32.NMHDR.GetNotifyCode(m.LParam);
                switch (notifyCode)
                {
                    case -150:
                    case -177:
                        if ((Microsoft.Win32.NMLVDISPINFO.GetMask(m.LParam) & LVIF.LVIF_STATE) > ((LVIF) 0))
                        {
                            LVIS stateMask = Microsoft.Win32.NMLVDISPINFO.GetStateMask(m.LParam);
                            if ((stateMask & (LVIS.LVIS_DROPHILITED | LVIS.LVIS_CUT)) > 0)
                            {
                                GetItemStateEventArgs e = new GetItemStateEventArgs(this, Microsoft.Win32.NMLVDISPINFO.GetItemIndex(m.LParam));
                                this.OnGetItemState(e);
                                LVIS lvis2 = 0;
                                if (((stateMask & LVIS.LVIS_CUT) > 0) && e.Cut)
                                {
                                    lvis2 |= LVIS.LVIS_CUT;
                                }
                                if (((stateMask & LVIS.LVIS_DROPHILITED) > 0) && e.DropHilited)
                                {
                                    lvis2 |= LVIS.LVIS_DROPHILITED;
                                }
                                if (lvis2 != 0)
                                {
                                    Microsoft.Win32.NMLVDISPINFO.SetState(m.LParam, Microsoft.Win32.NMLVDISPINFO.GetState(m.LParam) | lvis2);
                                }
                            }
                        }
                        return;

                    case -12:
                        this.WmCustomDraw(ref m);
                        break;

                    case -158:
                    case -157:
                    {
                        int item = Microsoft.Win32.NMLVGETINFOTIP.GetItem(m.LParam);
                        string text = Microsoft.Win32.NMLVGETINFOTIP.GetText(m.LParam, (notifyCode == -158) ? CharSet.Unicode : CharSet.Ansi);
                        ItemTooltipEventArgs args2 = new ItemTooltipEventArgs(base.Items[item], text);
                        this.OnItemTooltip(args2);
                        if (args2.Cancel)
                        {
                            Microsoft.Win32.NMLVGETINFOTIP.ClearText(m.LParam);
                        }
                        return;
                    }
                }
            }
        }

        private bool CanDrawColumnLines
        {
            get
            {
                return ((((base.View == View.Details) && this.ShowColumnLines) && ((base.Columns.Count > 0) && !this.ExplorerTheme)) && (base.TopItem != null));
            }
        }

        [Category("Behavior"), DefaultValue(true)]
        public bool CanResizeColumns
        {
            get
            {
                return this.CheckState(StateEx.CanResizeColumns);
            }
            set
            {
                this.SetState(StateEx.CanResizeColumns, value);
            }
        }

        [Category("Behavior"), DefaultValue(false)]
        public bool CollapsibleGroups
        {
            get
            {
                return (this.CheckState(StateEx.CollapsibleGroups) && (base.DesignMode || OS.IsWinVista));
            }
            set
            {
                if (this.CheckState(StateEx.CollapsibleGroups) != value)
                {
                    this.SetState(StateEx.CollapsibleGroups, value);
                    if (((base.IsHandleCreated && OS.IsWinVista) && (base.Groups.Count > 0)) && (GroupIdProperty != null))
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Microsoft.Win32.LVGROUP)));
                        try
                        {
                            Microsoft.Win32.LVGROUP.SetMask(ptr, LVGF.LVGF_NONE | LVGF.LVGF_GROUPID | LVGF.LVGF_STATE, true);
                            Microsoft.Win32.LVGROUP.SetStateMask(ptr, LVGS.LVGS_COLLAPSIBLE, true);
                            if (this.CheckState(StateEx.CollapsibleGroups))
                            {
                                Microsoft.Win32.LVGROUP.SetState(ptr, LVGS.LVGS_COLLAPSIBLE, true);
                            }
                            else
                            {
                                Microsoft.Win32.LVGROUP.SetState(ptr, LVGS.LVGS_COLLAPSIBLE | LVGS.LVGS_COLLAPSED, false);
                                Microsoft.Win32.LVGROUP.SetStateMask(ptr, LVGS.LVGS_NORMAL | LVGS.LVGS_COLLAPSED, true);
                            }
                            foreach (ListViewGroup group in base.Groups)
                            {
                                int groupId = (int) GroupIdProperty.GetValue(group, null);
                                Microsoft.Win32.LVGROUP.SetGroupId(ptr, groupId);
                                Windows.SendMessage(base.Handle, 0x1093, (IntPtr) groupId, ptr);
                            }
                        }
                        finally
                        {
                            Marshal.FreeHGlobal(ptr);
                        }
                    }
                }
            }
        }

        [Category("Appearance"), DefaultValue(false)]
        public bool ExplorerTheme
        {
            get
            {
                return (this.CheckState(StateEx.ExplorerTheme) && (base.DesignMode || IsExplorerThemeSupported));
            }
            set
            {
                if (this.CheckState(StateEx.ExplorerTheme) != value)
                {
                    this.SetState(StateEx.ExplorerTheme, value);
                    if (base.IsHandleCreated && OS.IsWinVista)
                    {
                        UxTheme.SetWindowTheme(base.Handle, (this.CheckState(StateEx.ExplorerTheme) && VisualStyleRenderer.IsSupported) ? "explorer" : null, null);
                    }
                }
            }
        }

        public override bool Focused
        {
            get
            {
                return (this.CheckState(StateEx.HasFocused) ? this.CheckState(StateEx.IsFocused) : base.Focused);
            }
        }

        public int FocusedItemIndex
        {
            get
            {
                if (base.IsHandleCreated)
                {
                    return CommCtrl.ListView_GetNextItem(base.Handle, -1, LVNI.LVNI_FOCUSED);
                }
                return -1;
            }
        }

        [DefaultValue((string) null), Category("Behavior")]
        public ImageList HeaderImageList
        {
            get
            {
                return this.FHeaderImageList;
            }
            set
            {
                if (this.FHeaderImageList != value)
                {
                    this.FHeaderImageList = value;
                    if (this.Header != null)
                    {
                        this.Header.SetImageList(value);
                    }
                }
            }
        }

        public bool IsBoxSelectionActive
        {
            get
            {
                return this.CheckState(StateEx.MarqueeSelectionActive);
            }
        }

        public bool IsEditing
        {
            get
            {
                IntPtr hWnd = CommCtrl.ListView_GetEditControl(base.Handle);
                return ((hWnd != IntPtr.Zero) && Windows.IsWindowVisible(hWnd));
            }
        }

        public static bool IsExplorerThemeSupported
        {
            get
            {
                return (OS.IsWinVista && VisualStyleRenderer.IsSupported);
            }
        }

        public int RowCount
        {
            get
            {
                try
                {
                    if (((base.View == View.List) || (base.View == View.Details)) && (base.Items.Count > 0))
                    {
                        return ((base.ClientSize.Height - 4) / base.GetItemRect(0, ItemBoundsPortion.Entire).Height);
                    }
                    return 0;
                }
                catch (ArgumentException)
                {
                    return 0;
                }
            }
        }

        [Browsable(false)]
        public ScrollBars ScollBarsVisibility
        {
            get
            {
                switch ((((int) Windows.GetWindowLong(base.Handle, -16)) & 0x300000))
                {
                    case 0x100000:
                        return ScrollBars.Horizontal;

                    case 0x200000:
                        return ScrollBars.Vertical;

                    case 0x300000:
                        return ScrollBars.Both;
                }
                return ScrollBars.None;
            }
        }

        [DefaultValue(false), Category("Appearance")]
        public bool ShowColumnLines
        {
            get
            {
                return this.CheckState(StateEx.ShowColumnLines);
            }
            set
            {
                if (this.CheckState(StateEx.ShowColumnLines) != value)
                {
                    this.SetState(StateEx.ShowColumnLines, value);
                    if (base.IsHandleCreated)
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        public ImageList SmallImageList
        {
            get
            {
                return base.SmallImageList;
            }
            set
            {
                base.SmallImageList = value;
                if ((base.SmallImageList == null) && base.IsHandleCreated)
                {
                    base.RecreateHandle();
                }
            }
        }

        [Category("Behavior"), DefaultValue(-1)]
        public int SortColumn
        {
            get
            {
                return this.FSortColumn;
            }
            set
            {
                this.SetSortColumn(value, this.FSortDirection);
            }
        }

        [DefaultValue(typeof(ListSortDirection), "Ascending"), Category("Behavior")]
        public ListSortDirection SortDirection
        {
            get
            {
                return this.FSortDirection;
            }
            set
            {
                this.SetSortColumn(this.FSortColumn, value);
            }
        }

        [CompilerGenerated]
        private sealed class <GetOrderedColumns>d__3 : IEnumerable<ColumnHeader>, IEnumerable, IEnumerator<ColumnHeader>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ColumnHeader <>2__current;
            public ListViewEx <>4__this;
            public int[] <>7__wrap7;
            public int <>7__wrap8;
            private int <>l__initialThreadId;
            public int[] <ColumnsOrder>5__4;
            public int <NextColumnIndex>5__5;

            [DebuggerHidden]
            public <GetOrderedColumns>d__3(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally6()
            {
                this.<>1__state = -1;
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<ColumnsOrder>5__4 = this.<>4__this.GetColumnsOrder();
                            this.<>1__state = 1;
                            this.<>7__wrap7 = this.<ColumnsOrder>5__4;
                            this.<>7__wrap8 = 0;
                            while (this.<>7__wrap8 < this.<>7__wrap7.Length)
                            {
                                this.<NextColumnIndex>5__5 = this.<>7__wrap7[this.<>7__wrap8];
                                this.<>2__current = this.<>4__this.Columns[this.<NextColumnIndex>5__5];
                                this.<>1__state = 2;
                                return true;
                            Label_0094:
                                this.<>1__state = 1;
                                this.<>7__wrap8++;
                            }
                            this.<>m__Finally6();
                            break;

                        case 2:
                            goto Label_0094;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<ColumnHeader> IEnumerable<ColumnHeader>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new ListViewEx.<GetOrderedColumns>d__3(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Windows.Forms.ColumnHeader>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        this.<>m__Finally6();
                        break;
                }
            }

            ColumnHeader IEnumerator<ColumnHeader>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [Flags]
        private enum StateEx
        {
            CanResizeColumns = 1,
            CollapsibleGroups = 8,
            ExplorerTheme = 2,
            HasFocused = 0x40,
            IsFocused = 0x20,
            MarqueeSelectionActive = 0x10,
            ShowColumnLines = 4
        }
    }
}

