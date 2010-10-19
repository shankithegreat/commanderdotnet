namespace Nomad.Controls
{
    using Microsoft;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Forms.Layout;

    [ToolboxItem(typeof(TabStripToolboxItem)), DesignerCategory("Code")]
    public class TabStrip : MenuStrip
    {
        private Tab DoDragHoverTab;
        private System.Windows.Forms.Timer DragHoverTimer;
        private Rectangle DragStartRect;
        private static readonly object EventAfterPaint = new object();
        private static readonly object EventBeforePaint = new object();
        private static readonly object EventSelectedTabChanged = new object();
        private static readonly object EventSelectedTabChanging = new object();
        private bool FAllowItemReorder;
        private Font FBoldFont;
        private int FDropMarkX = -1;
        private System.Windows.Forms.Layout.LayoutEngine FLayoutEngine;
        private Tab FSelectedTab;
        private Nomad.Controls.TabOrientation FTabOrientation;
        private bool FUseBoldFont;
        private int tabOverlap = 10;

        [Category("Appearance")]
        public event PaintEventHandler AfterPaint
        {
            add
            {
                base.Events.AddHandler(EventAfterPaint, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventAfterPaint, value);
            }
        }

        [Category("Appearance")]
        public event PaintEventHandler BeforePaint
        {
            add
            {
                base.Events.AddHandler(EventBeforePaint, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventBeforePaint, value);
            }
        }

        public event EventHandler SelectedTabChanged
        {
            add
            {
                base.Events.AddHandler(EventSelectedTabChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventSelectedTabChanged, value);
            }
        }

        public event EventHandler<TabStripCancelEventArgs> SelectedTabChanging
        {
            add
            {
                base.Events.AddHandler(EventSelectedTabChanging, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventSelectedTabChanging, value);
            }
        }

        public TabStrip()
        {
            base.Renderer = new TabStripProfessionalRenderer();
        }

        protected override ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick)
        {
            return new Tab(text, image, onClick);
        }

        private void DragHoverTimer_Tick(object sender, EventArgs e)
        {
            this.DragHoverTimer.Stop();
            this.DoDragHoverTab = this.DragHoverTimer.Tag as Tab;
        }

        public IEnumerable<Tab> GetAllTabs()
        {
            return new <GetAllTabs>d__0(-2) { <>4__this = this };
        }

        public Tab GetNextTab(Tab tab, bool forward, bool wrap)
        {
            if (this.Items.Count != 0)
            {
                int index = 0;
                if (tab != null)
                {
                    index = this.Items.IndexOf(tab);
                }
                int num2 = index;
                do
                {
                    num2 += forward ? 1 : -1;
                    if (num2 < 0)
                    {
                        num2 = wrap ? (this.Items.Count - 1) : index;
                    }
                    if (num2 > (this.Items.Count - 1))
                    {
                        num2 = wrap ? 0 : index;
                    }
                    Tab tab2 = this.Items[num2] as Tab;
                    if ((tab2 != null) && (num2 != index))
                    {
                        return tab2;
                    }
                }
                while (num2 != index);
            }
            return null;
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size empty = Size.Empty;
            foreach (ToolStripItem item in this.Items)
            {
                empty = Nomad.Controls.LayoutUtils.UnionSizes(empty, item.GetPreferredSize(proposedSize) + item.Padding.Size);
            }
            return (empty + base.Padding.Size);
        }

        protected virtual void OnAfterPaint(PaintEventArgs e)
        {
            PaintEventHandler handler = base.Events[EventAfterPaint] as PaintEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnBeforePaint(PaintEventArgs e)
        {
            PaintEventHandler handler = base.Events[EventBeforePaint] as PaintEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            this.DropMarkX = -1;
            DragImage.DragLeave(this);
            this.SetDragTimerTab(null);
            this.DragStartRect = Rectangle.Empty;
            if (drgevent.Data.GetDataPresent(typeof(Tab)))
            {
                Tab itemAt = base.GetItemAt(base.PointToClient(new Point(drgevent.X, drgevent.Y))) as Tab;
                Tab data = (Tab) drgevent.Data.GetData(typeof(Tab));
                if ((data != itemAt) && ((itemAt != null) || !data.IsLastTab))
                {
                    base.SuspendLayout();
                    this.Items.Remove(data);
                    this.Items.Insert((itemAt != null) ? this.Items.IndexOf(itemAt) : this.Items.Count, data);
                    base.ResumeLayout();
                }
            }
            else
            {
                base.OnDragDrop(drgevent);
            }
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (drgevent.Data.GetDataPresent(typeof(Tab)))
            {
                DragImage.DragEnter(this, drgevent);
                drgevent.Effect = DragDropEffects.Move;
            }
            else
            {
                base.OnDragEnter(drgevent);
            }
        }

        protected override void OnDragLeave(EventArgs e)
        {
            this.DropMarkX = -1;
            DragImage.DragLeave(this);
            this.SetDragTimerTab(null);
            this.UnselectAll();
            base.OnDragLeave(e);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            Tab itemAt;
            if (drgevent.Data.GetDataPresent(typeof(Tab)))
            {
                Tab data;
                DragImage.DragOver(this, drgevent);
                int num = -1;
                Point point = base.PointToClient(new Point(drgevent.X, drgevent.Y));
                itemAt = base.GetItemAt(point) as Tab;
                if (itemAt == null)
                {
                    itemAt = this.LastTab;
                    if ((itemAt != null) && (point.X > itemAt.Bounds.Right))
                    {
                        data = drgevent.Data.GetData(typeof(Tab)) as Tab;
                        if (data != this.LastTab)
                        {
                            num = itemAt.Bounds.Right - 1;
                        }
                    }
                }
                else
                {
                    data = drgevent.Data.GetData(typeof(Tab)) as Tab;
                    if ((itemAt != data) && (itemAt != this.GetNextTab(data, true, false)))
                    {
                        Tab tab3 = this.GetNextTab(itemAt, false, false);
                        num = (tab3 != null) ? (tab3.Bounds.Right - 1) : itemAt.Bounds.Left;
                    }
                }
                this.DropMarkX = num;
                drgevent.Effect = (this.DropMarkX >= 0) ? DragDropEffects.Move : DragDropEffects.None;
            }
            else
            {
                itemAt = base.GetItemAt(base.PointToClient(new Point(drgevent.X, drgevent.Y))) as Tab;
                if (this.DoDragHoverTab != null)
                {
                    if (this.DoDragHoverTab == itemAt)
                    {
                        this.DoDragHoverTab.InvokeDragEvent(DragEventType.Hover, drgevent);
                    }
                    this.DoDragHoverTab = null;
                }
                else
                {
                    if (itemAt != null)
                    {
                        itemAt.InvokeDragEvent(DragEventType.Over, drgevent);
                    }
                    else
                    {
                        this.UnselectAll();
                    }
                    this.SetDragTimerTab(itemAt);
                }
                base.OnDragOver(drgevent);
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            this.FBoldFont = null;
            base.OnFontChanged(e);
        }

        protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
        {
            Tab clickedItem = e.ClickedItem as Tab;
            if (clickedItem != null)
            {
                this.SetSelectedTab(clickedItem);
            }
            base.OnItemClicked(e);
        }

        protected override void OnMouseDown(MouseEventArgs mea)
        {
            base.OnMouseDown(mea);
            if (this.AllowItemReorder)
            {
                Size dragSize = SystemInformation.DragSize;
                this.DragStartRect = new Rectangle(mea.X - (dragSize.Width / 2), mea.Y - (dragSize.Height / 2), dragSize.Width, dragSize.Height);
            }
        }

        protected override void OnMouseMove(MouseEventArgs mea)
        {
            base.OnMouseMove(mea);
            if (((mea.Button & MouseButtons.Left) > MouseButtons.None) && !this.DragStartRect.Contains(mea.Location))
            {
                Tab itemAt = base.GetItemAt(mea.Location) as Tab;
                if (itemAt != null)
                {
                    if (itemAt.Width <= 0x100)
                    {
                        using (Bitmap bitmap = new Bitmap(itemAt.Width, itemAt.Height))
                        {
                            using (Graphics graphics = Graphics.FromImage(bitmap))
                            {
                                base.Renderer.DrawButtonBackground(new ToolStripItemRenderEventArgs(graphics, itemAt));
                                base.Renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(graphics, itemAt, itemAt.Text, itemAt.ContentRectangle, itemAt.ForeColor, itemAt.Font, itemAt.TextAlign));
                            }
                            Point location = mea.Location;
                            location.Offset(-itemAt.Bounds.Left, -itemAt.Bounds.Top);
                            using (new DragImage(bitmap, location))
                            {
                                base.DoDragDrop(itemAt, DragDropEffects.Move);
                            }
                        }
                    }
                    else
                    {
                        base.DoDragDrop(itemAt, DragDropEffects.Move);
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs mea)
        {
            base.OnMouseUp(mea);
            this.DragStartRect = Rectangle.Empty;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.OnBeforePaint(e);
            base.OnPaint(e);
            if (this.DropMarkX >= 0)
            {
                e.Graphics.SmoothingMode = SmoothingMode.Default;
                Point[] points = new Point[] { new Point(this.DropMarkX - 3, 0), new Point(this.DropMarkX, 4), new Point(this.DropMarkX + 4, 0) };
                e.Graphics.FillPolygon(Brushes.Black, points);
                points = new Point[] { new Point(this.DropMarkX - 4, base.ClientRectangle.Bottom), new Point(this.DropMarkX, base.ClientRectangle.Bottom - 5), new Point(this.DropMarkX + 4, base.ClientRectangle.Bottom) };
                e.Graphics.FillPolygon(Brushes.Black, points);
            }
            this.OnAfterPaint(e);
        }

        protected virtual void OnSelectedTabChanged(EventArgs e)
        {
            EventHandler handler = base.Events[EventSelectedTabChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSelectedTabChanging(TabStripCancelEventArgs e)
        {
            EventHandler<TabStripCancelEventArgs> handler = base.Events[EventSelectedTabChanging] as EventHandler<TabStripCancelEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void ResetPadding()
        {
            base.Padding = this.DefaultPadding;
        }

        public void ResetTabsHeight()
        {
            base.SuspendLayout();
            foreach (ToolStripItem item in this.Items)
            {
                if ((item.Available && (item is Tab)) && !item.AutoSize)
                {
                    item.Height = item.GetPreferredSize(this.DisplayRectangle.Size).Height;
                }
            }
            base.ResumeLayout();
        }

        public Tab SelectNextTab(bool forward)
        {
            Tab tab = this.GetNextTab(this.SelectedTab, forward, true);
            if (tab != null)
            {
                tab.PerformClick();
            }
            return tab;
        }

        protected override void SetDisplayedItems()
        {
            base.SetDisplayedItems();
            for (int i = 0; i < this.DisplayedItems.Count; i++)
            {
                if (this.DisplayedItems[i] == this.SelectedTab)
                {
                    this.DisplayedItems.Add(this.SelectedTab);
                    break;
                }
            }
        }

        private void SetDragTimerTab(Tab tab)
        {
            if (tab == null)
            {
                if (this.DragHoverTimer != null)
                {
                    this.DragHoverTimer.Stop();
                    this.DragHoverTimer.Tag = null;
                }
                this.DoDragHoverTab = null;
            }
            else if (this.DragHoverTimer == null)
            {
                this.DragHoverTimer = new System.Windows.Forms.Timer();
                this.DragHoverTimer.Interval = OS.MouseHoverTime;
                this.DragHoverTimer.Tick += new EventHandler(this.DragHoverTimer_Tick);
                this.DragHoverTimer.Tag = tab;
                this.DragHoverTimer.Start();
            }
            else if (this.DragHoverTimer.Tag != tab)
            {
                this.DragHoverTimer.Stop();
                this.DragHoverTimer.Tag = tab;
                this.DragHoverTimer.Start();
            }
        }

        private void SetSelectedTab(Tab selectedTab)
        {
            if (selectedTab != null)
            {
                TabStripCancelEventArgs e = new TabStripCancelEventArgs(selectedTab, false);
                this.OnSelectedTabChanging(e);
                if (e.Cancel)
                {
                    return;
                }
            }
            this.FSelectedTab = selectedTab;
            if (this.FSelectedTab != null)
            {
                bool flag = false;
                using (new LockWindowRedraw(this, true))
                {
                    base.SuspendLayout();
                    foreach (Tab tab in this.GetAllTabs())
                    {
                        if (tab != selectedTab)
                        {
                            if (tab.Checked)
                            {
                                if (this.UseBoldFont)
                                {
                                    tab.ResetFont();
                                }
                                tab.Checked = false;
                            }
                            continue;
                        }
                        if (!tab.Checked)
                        {
                            if (this.UseBoldFont)
                            {
                                if (this.FBoldFont == null)
                                {
                                    this.FBoldFont = new Font(this.Font, FontStyle.Bold);
                                }
                                tab.Font = this.FBoldFont;
                            }
                            tab.Checked = true;
                            flag = true;
                        }
                    }
                    base.ResumeLayout(false);
                    base.PerformLayout();
                }
                if (this.FSelectedTab.TabStripPage != null)
                {
                    this.FSelectedTab.TabStripPage.Activate();
                }
                if (flag)
                {
                    this.OnSelectedTabChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(false)]
        public bool AllowItemReorder
        {
            get
            {
                return this.FAllowItemReorder;
            }
            set
            {
                this.FAllowItemReorder = value;
                if (this.FAllowItemReorder)
                {
                    this.AllowDrop = true;
                }
            }
        }

        protected override Padding DefaultPadding
        {
            get
            {
                if (base.Renderer is TabStripProfessionalRenderer)
                {
                    return new Padding(0, 4, 0, 4);
                }
                if (base.Renderer is TabStripSystemRenderer)
                {
                    return new Padding(2, 2, 0, 2);
                }
                if (base.Renderer is TabStripIE7Renderer)
                {
                    return new Padding(5, 2, 0, 5);
                }
                if (base.Renderer is TabStripVS2k3Renderer)
                {
                    return new Padding(5, 2, 2, 0);
                }
                return base.DefaultPadding;
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return base.DefaultSize;
            }
        }

        private int DropMarkX
        {
            get
            {
                return this.FDropMarkX;
            }
            set
            {
                if (this.FDropMarkX != value)
                {
                    if (this.FDropMarkX >= 0)
                    {
                        base.Invalidate(new Rectangle(this.FDropMarkX - 3, 0, 7, 4));
                        base.Invalidate(new Rectangle(this.FDropMarkX - 3, base.ClientRectangle.Bottom - 5, 7, 5));
                    }
                    this.FDropMarkX = value;
                    if (this.FDropMarkX >= 0)
                    {
                        base.Invalidate(new Rectangle(this.FDropMarkX - 3, 0, 7, 4));
                        base.Invalidate(new Rectangle(this.FDropMarkX - 3, base.ClientRectangle.Bottom - 5, 7, 5));
                    }
                    DragImage.Hide();
                    base.Update();
                    DragImage.Show();
                }
            }
        }

        [Browsable(false)]
        public Nomad.Controls.TabOrientation EffectiveOrientation
        {
            get
            {
                if (this.TabOrientation == Nomad.Controls.TabOrientation.Default)
                {
                    if (this.Dock == DockStyle.Bottom)
                    {
                        return Nomad.Controls.TabOrientation.Bottom;
                    }
                    return Nomad.Controls.TabOrientation.Top;
                }
                return this.TabOrientation;
            }
        }

        public Tab FirstTab
        {
            get
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    Tab tab = this.Items[i] as Tab;
                    if (tab != null)
                    {
                        return tab;
                    }
                }
                return null;
            }
        }

        public Tab LastTab
        {
            get
            {
                for (int i = this.Items.Count - 1; i >= 0; i--)
                {
                    Tab tab = this.Items[i] as Tab;
                    if (tab != null)
                    {
                        return tab;
                    }
                }
                return null;
            }
        }

        public override System.Windows.Forms.Layout.LayoutEngine LayoutEngine
        {
            get
            {
                if (base.Renderer is TabStripProfessionalRenderer)
                {
                    if (!(this.FLayoutEngine is TabStripProfessionalLayoutEngine))
                    {
                        this.FLayoutEngine = new TabStripProfessionalLayoutEngine();
                    }
                }
                else if ((base.Renderer is TabStripSystemRenderer) || (base.Renderer is TabStripIE7Renderer))
                {
                    if (!(this.FLayoutEngine is TabStripSystemLayoutEngine))
                    {
                        this.FLayoutEngine = new TabStripSystemLayoutEngine();
                    }
                }
                else if (!(this.FLayoutEngine is TabStripSimpleLayoutEngine))
                {
                    this.FLayoutEngine = new TabStripSimpleLayoutEngine();
                }
                if (this.FLayoutEngine != null)
                {
                    return this.FLayoutEngine;
                }
                return base.LayoutEngine;
            }
        }

        public Tab SelectedTab
        {
            get
            {
                return this.FSelectedTab;
            }
            set
            {
                if (this.FSelectedTab != value)
                {
                    if ((value == null) || (value.Owner != this))
                    {
                        throw new InvalidOperationException();
                    }
                    this.SetSelectedTab(value);
                }
            }
        }

        [Browsable(false)]
        public int TabCount
        {
            get
            {
                int num = 0;
                foreach (ToolStripItem item in this.Items)
                {
                    if (item is Tab)
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        [DefaultValue(0)]
        public Nomad.Controls.TabOrientation TabOrientation
        {
            get
            {
                return this.FTabOrientation;
            }
            set
            {
                if (this.FTabOrientation != value)
                {
                    Nomad.Controls.TabOrientation effectiveOrientation = this.EffectiveOrientation;
                    this.FTabOrientation = value;
                    if (this.EffectiveOrientation != effectiveOrientation)
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        [DefaultValue(10)]
        public int TabOverlap
        {
            get
            {
                return this.tabOverlap;
            }
            set
            {
                if (this.tabOverlap != value)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("Tab overlap must be greater than 0");
                    }
                    this.tabOverlap = value;
                    if (base.Renderer is TabStripProfessionalRenderer)
                    {
                        base.PerformLayout();
                    }
                }
            }
        }

        [DefaultValue(false)]
        public bool UseBoldFont
        {
            get
            {
                return this.FUseBoldFont;
            }
            set
            {
                if (this.FUseBoldFont != value)
                {
                    this.FUseBoldFont = value;
                    base.SuspendLayout();
                    foreach (Tab tab in this.GetAllTabs())
                    {
                        if (this.FUseBoldFont && tab.Checked)
                        {
                            if (this.FBoldFont == null)
                            {
                                this.FBoldFont = new Font(this.Font, FontStyle.Bold);
                            }
                            tab.Font = this.FBoldFont;
                            continue;
                        }
                        tab.ResetFont();
                    }
                    base.ResumeLayout();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetAllTabs>d__0 : IEnumerable<Tab>, IEnumerable, IEnumerator<Tab>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private Tab <>2__current;
            public TabStrip <>4__this;
            public IEnumerator <>7__wrap3;
            public IDisposable <>7__wrap4;
            private int <>l__initialThreadId;
            public ToolStripItem <NextItem>5__1;
            public Tab <NextTab>5__2;

            [DebuggerHidden]
            public <GetAllTabs>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally5()
            {
                this.<>1__state = -1;
                this.<>7__wrap4 = this.<>7__wrap3 as IDisposable;
                if (this.<>7__wrap4 != null)
                {
                    this.<>7__wrap4.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrap3 = this.<>4__this.Items.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap3.MoveNext())
                            {
                                this.<NextItem>5__1 = (ToolStripItem) this.<>7__wrap3.Current;
                                this.<NextTab>5__2 = this.<NextItem>5__1 as Tab;
                                if (this.<NextTab>5__2 == null)
                                {
                                    goto Label_009F;
                                }
                                this.<>2__current = this.<NextTab>5__2;
                                this.<>1__state = 2;
                                return true;
                            Label_0098:
                                this.<>1__state = 1;
                            Label_009F:;
                            }
                            this.<>m__Finally5();
                            break;

                        case 2:
                            goto Label_0098;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<Tab> IEnumerable<Tab>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new TabStrip.<GetAllTabs>d__0(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.Controls.Tab>.GetEnumerator();
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
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally5();
                        }
                        break;
                }
            }

            Tab IEnumerator<Tab>.Current
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

        private abstract class TabStripLayoutEngine : LayoutEngine
        {
            protected TabStripLayoutEngine()
            {
            }

            protected static Dictionary<Tab, IntRef> CreateTabWidthMap(TabStrip tabStrip, int displayWidth, int tabOverlap)
            {
                Dictionary<Tab, IntRef> dictionary = new Dictionary<Tab, IntRef>(tabStrip.Items.Count);
                foreach (ToolStripItem item in tabStrip.Items)
                {
                    int width;
                    if (!item.Available)
                    {
                        continue;
                    }
                    Tab key = item as Tab;
                    if (item.AutoSize)
                    {
                        width = item.GetPreferredSize(tabStrip.DisplayRectangle.Size).Width;
                    }
                    else if ((key != null) && (key.FixedWidth > 0))
                    {
                        width = key.FixedWidth;
                    }
                    else
                    {
                        width = item.Width;
                    }
                    if (key != null)
                    {
                        dictionary.Add(key, new IntRef(width));
                    }
                    else
                    {
                        displayWidth -= width;
                    }
                }
                int num2 = 0;
                foreach (IntRef ref2 in dictionary.Values)
                {
                    num2 += ref2.Value;
                }
                num2 -= tabOverlap * (dictionary.Count - 1);
                while (num2 >= displayWidth)
                {
                    int num3 = 0;
                    foreach (IntRef ref2 in dictionary.Values)
                    {
                        num3 = Math.Max(num3, ref2.Value);
                    }
                    foreach (IntRef ref2 in dictionary.Values)
                    {
                        if (ref2.Value == num3)
                        {
                            ref2.Value--;
                            num2--;
                            if (num2 < displayWidth)
                            {
                                break;
                            }
                        }
                    }
                }
                return dictionary;
            }

            public override bool Layout(object container, LayoutEventArgs e)
            {
                TabStrip tabStrip = (TabStrip) container;
                Tab affectedComponent = e.AffectedComponent as Tab;
                if (!((((e.AffectedControl != null) || !(e.AffectedProperty == "Text")) || (affectedComponent == null)) || affectedComponent.AutoSize))
                {
                    return false;
                }
                Rectangle displayRectangle = tabStrip.DisplayRectangle;
                if (displayRectangle.Width <= 0)
                {
                    return false;
                }
                LayoutRightItems(tabStrip);
                return this.LayoutTabStrip(tabStrip, ref displayRectangle);
            }

            protected static void LayoutRightItems(TabStrip tabStrip)
            {
                Rectangle displayRectangle = tabStrip.DisplayRectangle;
                Point location = new Point(displayRectangle.Right, displayRectangle.Top);
                for (int i = tabStrip.Items.Count - 1; i >= 0; i--)
                {
                    ToolStripItem item = tabStrip.Items[i];
                    if (item.Available)
                    {
                        Tab tab = item as Tab;
                        if (!(item is Tab) && (item.Alignment == ToolStripItemAlignment.Right))
                        {
                            if (item.AutoSize)
                            {
                                item.Size = item.GetPreferredSize(displayRectangle.Size);
                            }
                            location.X -= item.Width;
                            tabStrip.SetItemLocation(item, location);
                        }
                    }
                }
            }

            protected abstract bool LayoutTabStrip(TabStrip tabStrip, ref Rectangle displayRect);

            protected class IntRef
            {
                public int Value;

                public IntRef(int value)
                {
                    this.Value = value;
                }
            }
        }

        private class TabStripProfessionalLayoutEngine : TabStrip.TabStripLayoutEngine
        {
            protected override bool LayoutTabStrip(TabStrip tabStrip, ref Rectangle displayRect)
            {
                Dictionary<Tab, TabStrip.TabStripLayoutEngine.IntRef> dictionary = TabStrip.TabStripLayoutEngine.CreateTabWidthMap(tabStrip, displayRect.Width, tabStrip.TabOverlap);
                Point location = displayRect.Location;
                for (int i = 0; i < tabStrip.Items.Count; i++)
                {
                    ToolStripItem item = tabStrip.Items[i];
                    if (item.Available && (item.Alignment != ToolStripItemAlignment.Right))
                    {
                        Tab tab = item as Tab;
                        tabStrip.SetItemLocation(item, location);
                        if (item.AutoSize)
                        {
                            if (tab != null)
                            {
                                item.Size = new Size(dictionary[tab].Value, displayRect.Height);
                            }
                            else
                            {
                                item.Size = item.GetPreferredSize(displayRect.Size);
                            }
                        }
                        else if (tab != null)
                        {
                            tab.Width = dictionary[tab].Value;
                        }
                        if (((tab != null) && ((i + 1) < tabStrip.Items.Count)) && (tabStrip.Items[i + 1] is Tab))
                        {
                            location.X += item.Width - tabStrip.TabOverlap;
                        }
                        else
                        {
                            location.X += item.Width;
                        }
                    }
                }
                return tabStrip.AutoSize;
            }
        }

        private class TabStripSimpleLayoutEngine : TabStrip.TabStripLayoutEngine
        {
            protected override bool LayoutTabStrip(TabStrip tabStrip, ref Rectangle displayRect)
            {
                Dictionary<Tab, TabStrip.TabStripLayoutEngine.IntRef> dictionary = TabStrip.TabStripLayoutEngine.CreateTabWidthMap(tabStrip, displayRect.Width - tabStrip.Padding.Left, 0);
                Point location = displayRect.Location;
                for (int i = 0; i < tabStrip.Items.Count; i++)
                {
                    ToolStripItem item = tabStrip.Items[i];
                    if (item.Available && (item.Alignment != ToolStripItemAlignment.Right))
                    {
                        Tab tab = item as Tab;
                        if (tab != null)
                        {
                            item.Size = new Size(dictionary[tab].Value, displayRect.Height);
                        }
                        else if (item.AutoSize)
                        {
                            item.Size = item.GetPreferredSize(displayRect.Size);
                        }
                        tabStrip.SetItemLocation(item, location);
                        location.X += item.Size.Width;
                    }
                }
                return tabStrip.AutoSize;
            }
        }

        private class TabStripSystemLayoutEngine : TabStrip.TabStripLayoutEngine
        {
            protected override bool LayoutTabStrip(TabStrip tabStrip, ref Rectangle displayRect)
            {
                Dictionary<Tab, TabStrip.TabStripLayoutEngine.IntRef> dictionary = TabStrip.TabStripLayoutEngine.CreateTabWidthMap(tabStrip, displayRect.Width - tabStrip.Padding.Left, 0);
                Point location = displayRect.Location;
                int num = (tabStrip.EffectiveOrientation == TabOrientation.Top) ? 2 : 0;
                location.Y += num;
                Tab tab = null;
                Rectangle empty = Rectangle.Empty;
                for (int i = 0; i < tabStrip.Items.Count; i++)
                {
                    ToolStripItem item = tabStrip.Items[i];
                    if (item.Available && (item.Alignment != ToolStripItemAlignment.Right))
                    {
                        Size preferredSize;
                        Tab tab2 = item as Tab;
                        if (tab2 != null)
                        {
                            preferredSize = new Size(dictionary[tab2].Value, displayRect.Height - 2);
                        }
                        else if (item.AutoSize)
                        {
                            preferredSize = item.GetPreferredSize(displayRect.Size);
                        }
                        else
                        {
                            preferredSize = item.Bounds.Size;
                        }
                        if (tab2.Checked)
                        {
                            tab = tab2;
                            empty = new Rectangle(location.X - 2, location.Y - num, preferredSize.Width + 4, preferredSize.Height + 3);
                        }
                        else
                        {
                            tabStrip.SetItemLocation(item, location);
                            item.Size = preferredSize;
                        }
                        location.X += preferredSize.Width;
                    }
                }
                if (tab != null)
                {
                    tabStrip.SetItemLocation(tab, empty.Location);
                    tab.Size = empty.Size;
                }
                return tabStrip.AutoSize;
            }
        }
    }
}

