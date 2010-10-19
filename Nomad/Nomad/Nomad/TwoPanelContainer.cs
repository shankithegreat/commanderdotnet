namespace Nomad
{
    using Nomad.Commons.Threading;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Serialization;

    [DesignerCategory("Code"), ToolboxItem(false)]
    public class TwoPanelContainer : SplitContainer, ICloneable, IPersistComponentSettings
    {
        private VirtualFilePanel FLastCurrentPanel;
        private MouseWheelHelper FMouseWheelFix;
        private bool FNavigationLock;
        private bool FSaveSettings;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private TwoPanelContainerSettings FSettings;
        private System.Windows.Forms.Orientation? NewOrientation;
        private bool NewPanel1Collapsed;
        private bool NewPanel2Collapsed;
        private int NewSplitterPercent = -1;
        private bool ProcessingOnLayout;
        private Control RememberActiveControl;
        private static LazyInit<XmlSerializer> Serializer = new LazyInit<XmlSerializer>(delegate {
            return new XmlSerializer(typeof(TwoPanelTab));
        });

        public event EventHandler CurrentFolderChanged;

        public event EventHandler<VirtualItemDragEventArg> DragDropOnItem
        {
            add
            {
                this.LeftPanel.DragDropOnItem += value;
                this.RightPanel.DragDropOnItem += value;
            }
            remove
            {
                this.LeftPanel.DragDropOnItem -= value;
                this.RightPanel.DragDropOnItem -= value;
            }
        }

        public event EventHandler<VirtualItemDragEventArg> DragOverItem
        {
            add
            {
                this.LeftPanel.DragOverItem += value;
                this.RightPanel.DragOverItem += value;
            }
            remove
            {
                this.LeftPanel.DragOverItem -= value;
                this.RightPanel.DragOverItem -= value;
            }
        }

        public event EventHandler<HandleVirtualItemEventArgs> ExecuteItem
        {
            add
            {
                this.LeftPanel.ExecuteItem += value;
                this.RightPanel.ExecuteItem += value;
            }
            remove
            {
                this.LeftPanel.ExecuteItem -= value;
                this.RightPanel.ExecuteItem -= value;
            }
        }

        public event EventHandler<PreviewContextMenuEventArgs> PreviewContextMenu
        {
            add
            {
                this.LeftPanel.PreviewContextMenu += value;
                this.RightPanel.PreviewContextMenu += value;
            }
            remove
            {
                this.LeftPanel.PreviewContextMenu -= value;
                this.RightPanel.PreviewContextMenu -= value;
            }
        }

        private TwoPanelContainer()
        {
            base.TabStop = false;
            this.BackColor = Color.Transparent;
            base.SplitterDistance = 0x171;
            base.TabStop = false;
            base.SplitterMoving += new SplitterCancelEventHandler(this.TwoPanelContainer_SplitterMoving);
            base.SplitterMoved += new SplitterEventHandler(this.TwoPanelContainer_SplitterMoved);
            this.FSettings = new TwoPanelContainerSettings();
        }

        [CompilerGenerated]
        private static XmlSerializer <.cctor>b__6()
        {
            return new XmlSerializer(typeof(TwoPanelTab));
        }

        public void BeginLayout()
        {
            base.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            base.Panel1.SuspendLayout();
            this.RightPanel.SuspendLayout();
            base.Panel2.SuspendLayout();
        }

        public object Clone()
        {
            TwoPanelContainer container = new TwoPanelContainer {
                SettingsKey = "MainTab"
            };
            container.SuspendLayout();
            container.Panel1.SuspendLayout();
            container.Panel2.SuspendLayout();
            container.LeftPanel = (VirtualFilePanel) this.LeftPanel.Clone();
            container.LeftPanel.SettingsKey = "LeftPanel";
            container.InitializePanel(container.LeftPanel);
            container.Panel1.Controls.Add(container.LeftPanel);
            container.RightPanel = (VirtualFilePanel) this.RightPanel.Clone();
            container.RightPanel.SettingsKey = "RightPanel";
            container.InitializePanel(container.RightPanel);
            container.Panel2.Controls.Add(container.RightPanel);
            container.Orientation = this.Orientation;
            container.Panel2.ResumeLayout(false);
            container.Panel1.ResumeLayout(false);
            container.ResumeLayout(false);
            container.NewSplitterPercent = this.SplitterPercent;
            container.NewPanel1Collapsed = base.Panel1Collapsed;
            container.NewPanel2Collapsed = base.Panel2Collapsed;
            if (this.FLastCurrentPanel != null)
            {
                container.FLastCurrentPanel = (this.FLastCurrentPanel == this.LeftPanel) ? container.LeftPanel : container.RightPanel;
            }
            return container;
        }

        public static TwoPanelContainer Create()
        {
            TwoPanelContainer container = new TwoPanelContainer {
                SettingsKey = "MainTab"
            };
            container.SuspendLayout();
            container.Panel1.SuspendLayout();
            container.Panel2.SuspendLayout();
            container.LeftPanel = new VirtualFilePanel();
            container.LeftPanel.SettingsKey = "LeftPanel";
            container.InitializePanel(container.LeftPanel);
            container.Panel1.Controls.Add(container.LeftPanel);
            container.RightPanel = new VirtualFilePanel();
            container.RightPanel.SettingsKey = "RightPanel";
            container.InitializePanel(container.RightPanel);
            container.Panel2.Controls.Add(container.RightPanel);
            container.Panel2.ResumeLayout(false);
            container.Panel1.ResumeLayout(false);
            container.ResumeLayout(false);
            return container;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.FMouseWheelFix != null)
                {
                    this.FMouseWheelFix.Dispose();
                    this.FMouseWheelFix = null;
                }
                if (this.SaveSettings)
                {
                    this.SaveComponentSettings();
                }
            }
            base.Dispose(disposing);
        }

        public void EndLayout(bool performLayout)
        {
            base.Panel2.ResumeLayout(false);
            this.RightPanel.ResumeLayout(false);
            base.Panel1.ResumeLayout(false);
            this.LeftPanel.ResumeLayout(false);
            base.ResumeLayout(performLayout);
        }

        public TwoPanelLayout GetTwoPanelLayout(bool useRemembered, bool storeActivePanel)
        {
            TwoPanelLayout layout;
            layout = new TwoPanelLayout {
                OnePanel = this.OnePanelMode != SinglePanel.None,
                StoreEntry = layout.StoreEntry | TwoPanelLayoutEntry.OnePanel
            };
            if (layout.OnePanel)
            {
                layout.LeftLayout = this.CurrentPanel.GetPanelLayout(useRemembered, PanelLayoutEntry.None);
                layout.StoreEntry |= TwoPanelLayoutEntry.LeftLayout;
                return layout;
            }
            layout.PanelsOrientation = this.Orientation;
            layout.SplitterPercent = this.SplitterPercent;
            layout.LeftLayout = this.LeftPanel.GetPanelLayout(useRemembered, PanelLayoutEntry.None);
            layout.RightLayout = this.RightPanel.GetPanelLayout(useRemembered, PanelLayoutEntry.None);
            if (storeActivePanel)
            {
                if (this.CurrentPanel == this.LeftPanel)
                {
                    layout.ActivePanel = ActivePanel.Left;
                }
                else if (this.CurrentPanel == this.RightPanel)
                {
                    layout.ActivePanel = ActivePanel.Right;
                }
                else
                {
                    layout.ActivePanel = ActivePanel.Unchanged;
                }
                layout.StoreEntry |= TwoPanelLayoutEntry.ActivePanel;
            }
            layout.StoreEntry |= TwoPanelLayoutEntry.RightLayout | TwoPanelLayoutEntry.LeftLayout | TwoPanelLayoutEntry.PanelsOrientation;
            return layout;
        }

        private void InitializePanel(VirtualFilePanel panel)
        {
            panel.Dock = DockStyle.Fill;
            panel.Enter += new EventHandler(this.Panel_Enter);
            panel.CurrentFolderChanged += new EventHandler<VirtualFolderChangedEventArgs>(this.Panel_CurrentFolderChanged);
        }

        public void LoadComponentSettings()
        {
            this.Orientation = this.FSettings.PanelsOrientation;
            this.OnePanelMode = this.FSettings.OnePanelMode;
            this.SplitterPercent = this.FSettings.SplitterPercent;
            this.LeftPanel.LoadComponentSettings();
            this.RightPanel.LoadComponentSettings();
        }

        protected void OnCurrentFolderChanged(EventArgs e)
        {
            if (this.CurrentFolderChanged != null)
            {
                this.CurrentFolderChanged(this, e);
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            this.SplitterPercent = 500;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (base.Enabled)
            {
                if (this.LeftPanel.Visible)
                {
                    this.LeftPanel.DoVisibleChanged();
                }
                if (this.RightPanel.Visible)
                {
                    this.RightPanel.DoVisibleChanged();
                }
                if (this.FLastCurrentPanel != null)
                {
                    this.FLastCurrentPanel.SelectNextControl(null, true, true, true, false);
                }
            }
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            MethodInvoker method = null;
            base.OnLayout(e);
            if ((e.AffectedControl == this) && ((e.AffectedProperty == "Bounds") || (e.AffectedProperty == "Parent")))
            {
                this.ProcessingOnLayout = true;
                try
                {
                    if (this.NewOrientation.HasValue)
                    {
                        this.Orientation = this.NewOrientation.Value;
                        this.NewOrientation = null;
                    }
                    if (this.NewPanel1Collapsed != this.NewPanel2Collapsed)
                    {
                        base.Panel1Collapsed = this.NewPanel1Collapsed;
                        base.Panel2Collapsed = this.NewPanel2Collapsed;
                        this.NewPanel1Collapsed = false;
                        this.NewPanel2Collapsed = false;
                    }
                    if (this.NewSplitterPercent >= 0)
                    {
                        this.SplitterPercent = this.NewSplitterPercent;
                        if (base.IsHandleCreated)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    this.NewSplitterPercent = -1;
                                };
                            }
                            base.BeginInvoke(method);
                        }
                    }
                }
                finally
                {
                    this.ProcessingOnLayout = false;
                }
            }
        }

        protected override void OnMouseCaptureChanged(EventArgs e)
        {
            base.OnMouseCaptureChanged(e);
            if ((!base.Capture && (this.RememberActiveControl != null)) && (base.ActiveControl == null))
            {
                this.RememberActiveControl.Focus();
            }
            this.RememberActiveControl = null;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (base.Capture)
            {
                this.RememberActiveControl = base.ActiveControl;
            }
            base.OnMouseDown(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (((base.Visible && base.Enabled) && (this.FLastCurrentPanel != null)) && base.IsHandleCreated)
            {
                base.BeginInvoke(delegate (Control panel) {
                    panel.SelectNextControl(null, true, true, true, false);
                }, new object[] { this.FLastCurrentPanel });
            }
        }

        private void Panel_CurrentFolderChanged(object sender, VirtualFolderChangedEventArgs e)
        {
            if (this.NavigationLock)
            {
                bool flag = (e.PreviousFolder != null) && VirtualItem.Equals(e.CurrentFolder, e.PreviousFolder.Parent);
                bool flag2 = (!flag && (e.CurrentFolder != null)) && VirtualItem.Equals(e.CurrentFolder.Parent, e.PreviousFolder);
                VirtualFilePanel panel = (sender == this.LeftPanel) ? this.RightPanel : this.LeftPanel;
                bool flag3 = false;
                if (flag)
                {
                    if ((panel.ParentFolder != null) && string.Equals(e.PreviousFolder.Name, panel.CurrentFolder.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        flag3 = true;
                        panel.CurrentFolder = panel.ParentFolder;
                    }
                }
                else if (flag2)
                {
                    foreach (IVirtualItem item in panel.Items)
                    {
                        IVirtualFolder folder = item as IVirtualFolder;
                        if (((folder != null) && !folder.Equals(panel.ParentFolder)) && string.Equals(e.CurrentFolder.Name, folder.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            flag3 = true;
                            panel.CurrentFolder = folder;
                            break;
                        }
                    }
                }
                this.NavigationLock = flag3;
            }
            if (sender == this.CurrentPanel)
            {
                this.OnCurrentFolderChanged(e);
            }
        }

        private void Panel_Enter(object sender, EventArgs e)
        {
            this.FLastCurrentPanel = (VirtualFilePanel) sender;
            if (!VirtualItem.Equals(this.LeftPanel.CurrentFolder, this.RightPanel.CurrentFolder))
            {
                base.BeginInvoke(new Action<EventArgs>(this.OnCurrentFolderChanged), new object[] { e });
            }
        }

        public static GeneralTab ParseBookmark(XmlReader reader)
        {
            reader.MoveToContent();
            if (reader.Name == typeof(TwoPanelTab).Name)
            {
                GeneralTab tab;
                try
                {
                    tab = (TwoPanelTab) Serializer.Value.Deserialize(reader);
                }
                catch (InvalidOperationException)
                {
                }
                return tab;
            }
            return null;
        }

        public void ResetComponentSettings()
        {
            this.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.OnePanelMode = SinglePanel.None;
            this.SplitterPercent = 500;
            this.LeftPanel.ResetComponentSettings();
            this.RightPanel.ResetComponentSettings();
        }

        public void ResetVisualCache()
        {
            this.LeftPanel.ResetVisualCache();
            this.RightPanel.ResetVisualCache();
        }

        public void SaveComponentSettings()
        {
            this.FSettings.PanelsOrientation = this.Orientation;
            this.FSettings.OnePanelMode = this.OnePanelMode;
            this.FSettings.SplitterPercent = this.SplitterPercent;
            SettingsManager.RegisterSettings(this.FSettings);
            this.LeftPanel.SaveComponentSettings();
            this.RightPanel.SaveComponentSettings();
        }

        public static void SerializeBookmark(XmlWriter writer, GeneralTab tabBookmark)
        {
            if (tabBookmark == null)
            {
                throw new ArgumentNullException("tabBookmark");
            }
            if (!(tabBookmark is TwoPanelTab))
            {
                throw new ArgumentException();
            }
            Serializer.Value.Serialize(writer, tabBookmark);
        }

        private void TwoPanelContainer_SplitterMoved(object sender, EventArgs e)
        {
            if (base.IsHandleCreated && (this.HintTooltip != null))
            {
                this.HintTooltip.Hide(this);
            }
        }

        private void TwoPanelContainer_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            this.NewSplitterPercent = -1;
            if (this.HintTooltip != null)
            {
                double num;
                if (this.Orientation == System.Windows.Forms.Orientation.Vertical)
                {
                    num = (e.SplitX * 100f) / ((float) (base.Panel1.Width + base.Panel2.Width));
                }
                else
                {
                    num = (e.SplitY * 100f) / ((float) (base.Panel1.Height + base.Panel2.Height));
                }
                this.HintTooltip.Show(string.Format("{0:0.0}%", num), this, e.MouseCursorX, e.MouseCursorY + this.Cursor.GetPrefferedHeight());
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VirtualFilePanel CurrentPanel
        {
            get
            {
                if (this.RightPanel.ContainsFocus)
                {
                    return this.RightPanel;
                }
                if (this.LeftPanel.ContainsFocus)
                {
                    return this.LeftPanel;
                }
                return ((this.FLastCurrentPanel != null) ? this.FLastCurrentPanel : this.LeftPanel);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public VirtualFilePanel FarPanel
        {
            get
            {
                if (this.RightPanel.ContainsFocus)
                {
                    return this.LeftPanel;
                }
                if (this.LeftPanel.ContainsFocus)
                {
                    return this.RightPanel;
                }
                return (((this.FLastCurrentPanel == null) || (this.FLastCurrentPanel == this.LeftPanel)) ? this.RightPanel : this.LeftPanel);
            }
        }

        [DefaultValue(false)]
        public bool FixMouseWheel
        {
            get
            {
                return (this.FMouseWheelFix != null);
            }
            set
            {
                if (value)
                {
                    if (this.FMouseWheelFix == null)
                    {
                        this.FMouseWheelFix = new MouseWheelHelper();
                        this.FMouseWheelFix.SetFixMouseWheel(this, true);
                        this.FMouseWheelFix.ApplyToChildren(this, delegate (Control ctrl) {
                            return !(ctrl is ScrollableControl);
                        });
                    }
                }
                else
                {
                    if (this.FMouseWheelFix != null)
                    {
                        this.FMouseWheelFix.Dispose();
                    }
                    this.FMouseWheelFix = null;
                }
            }
        }

        [DefaultValue((string) null)]
        public ToolTip HintTooltip { get; set; }

        [Browsable(false)]
        public bool IsContentInitialized
        {
            get
            {
                return (this.LeftPanel.IsContentInitialized && this.RightPanel.IsContentInitialized);
            }
        }

        private bool IsLayoutSuspended
        {
            get
            {
                return ((this.IsLayoutSuspended() || (base.Parent == null)) && !this.ProcessingOnLayout);
            }
        }

        [Browsable(false)]
        public VirtualFilePanel LeftPanel { get; private set; }

        [DefaultValue(false)]
        public bool NavigationLock
        {
            get
            {
                return (this.FNavigationLock && (this.OnePanelMode == SinglePanel.None));
            }
            set
            {
                this.FNavigationLock = value;
            }
        }

        [DefaultValue(0)]
        public SinglePanel OnePanelMode
        {
            get
            {
                if (this.NewPanel1Collapsed != this.NewPanel2Collapsed)
                {
                    if (this.NewPanel1Collapsed)
                    {
                        return SinglePanel.Right;
                    }
                    if (this.NewPanel2Collapsed)
                    {
                        return SinglePanel.Left;
                    }
                }
                else
                {
                    if (base.Panel1Collapsed)
                    {
                        return SinglePanel.Right;
                    }
                    if (base.Panel2Collapsed)
                    {
                        return SinglePanel.Left;
                    }
                }
                return SinglePanel.None;
            }
            set
            {
                if (this.OnePanelMode != value)
                {
                    if (this.IsLayoutSuspended)
                    {
                        this.NewPanel1Collapsed = value == SinglePanel.Right;
                        this.NewPanel2Collapsed = value == SinglePanel.Left;
                    }
                    else
                    {
                        using (new LockWindowRedraw(this, true))
                        {
                            base.Panel1Collapsed = value == SinglePanel.Right;
                            base.Panel2Collapsed = value == SinglePanel.Left;
                        }
                        if (!this.LeftPanel.Visible)
                        {
                            this.LeftPanel.DoVisibleChanged();
                        }
                        if (!this.RightPanel.Visible)
                        {
                            this.RightPanel.DoVisibleChanged();
                        }
                    }
                }
            }
        }

        [DefaultValue(1)]
        public System.Windows.Forms.Orientation Orientation
        {
            get
            {
                return (this.NewOrientation.HasValue ? this.NewOrientation.Value : base.Orientation);
            }
            set
            {
                if (this.IsLayoutSuspended)
                {
                    this.NewOrientation = new System.Windows.Forms.Orientation?(value);
                }
                else
                {
                    LockWindowRedraw redraw = (this.OnePanelMode == SinglePanel.None) ? new LockWindowRedraw(this, true) : null;
                    try
                    {
                        int splitterPercent = this.SplitterPercent;
                        base.Orientation = value;
                        if (!((this.NewSplitterPercent >= 0) && this.ProcessingOnLayout))
                        {
                            this.SplitterPercent = splitterPercent;
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        if (redraw != null)
                        {
                            redraw.Dispose();
                        }
                    }
                }
            }
        }

        [Browsable(false)]
        public VirtualFilePanel RightPanel { get; private set; }

        public bool SaveSettings
        {
            get
            {
                return this.FSaveSettings;
            }
            set
            {
                this.FSaveSettings = value;
            }
        }

        public string SettingsKey
        {
            get
            {
                return base.Name;
            }
            set
            {
                this.FSettings.SettingsKey = value;
            }
        }

        public int SplitterPercent
        {
            get
            {
                int num;
                if (this.NewSplitterPercent >= 0)
                {
                    return this.NewSplitterPercent;
                }
                if (this.Orientation == System.Windows.Forms.Orientation.Vertical)
                {
                    num = base.Panel1.Width + base.Panel2.Width;
                }
                else
                {
                    num = base.Panel1.Height + base.Panel2.Height;
                }
                int num2 = (int) Math.Round((double) ((base.SplitterDistance * 1000.0) / ((double) num)));
                return (((num2 < 0) || (num2 > 0x3e8)) ? -1 : num2);
            }
            set
            {
                if ((value >= 0) && (value <= 0x3e8))
                {
                    if (this.IsLayoutSuspended)
                    {
                        this.NewSplitterPercent = value;
                    }
                    else
                    {
                        int num;
                        if (this.Orientation == System.Windows.Forms.Orientation.Vertical)
                        {
                            num = base.Panel1.Width + base.Panel2.Width;
                        }
                        else
                        {
                            num = base.Panel1.Height + base.Panel2.Height;
                        }
                        try
                        {
                            base.SplitterDistance = (int) Math.Round((double) (((double) (value * num)) / 1000.0));
                        }
                        catch (InvalidOperationException)
                        {
                        }
                    }
                }
            }
        }

        public GeneralTab TabBookmark
        {
            get
            {
                TwoPanelTab tab = new TwoPanelTab {
                    Caption = this.Text,
                    Layout = this.GetTwoPanelLayout(true, true)
                };
                if (this.OnePanelMode != SinglePanel.None)
                {
                    tab.Left = this.CurrentPanel.GetPanelContent(true);
                    return tab;
                }
                tab.Left = this.LeftPanel.GetPanelContent(true);
                tab.Right = this.RightPanel.GetPanelContent(true);
                return tab;
            }
            set
            {
                TwoPanelTab tab = value as TwoPanelTab;
                if (tab != null)
                {
                    this.Text = tab.Caption;
                    this.WindowLayout = tab.Layout;
                    if (this.OnePanelMode == SinglePanel.None)
                    {
                        this.LeftPanel.PanelContent = tab.Left;
                        this.RightPanel.PanelContent = tab.Right;
                    }
                    else
                    {
                        this.CurrentPanel.PanelContent = tab.Left;
                        this.FarPanel.PanelContent = tab.Right;
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public TwoPanelLayout WindowLayout
        {
            get
            {
                return this.GetTwoPanelLayout(false, false);
            }
            set
            {
                using (new LockWindowUpdate(this))
                {
                    if ((value.StoreEntry & TwoPanelLayoutEntry.OnePanel) > 0)
                    {
                        if (value.OnePanel)
                        {
                            this.OnePanelMode = (this.CurrentPanel == this.LeftPanel) ? SinglePanel.Left : SinglePanel.Right;
                        }
                        else
                        {
                            this.OnePanelMode = SinglePanel.None;
                        }
                    }
                    if ((value.StoreEntry & TwoPanelLayoutEntry.PanelsOrientation) > 0)
                    {
                        this.Orientation = value.PanelsOrientation;
                        this.SplitterPercent = value.SplitterPercent;
                    }
                    if (this.OnePanelMode == SinglePanel.None)
                    {
                        if ((value.StoreEntry & TwoPanelLayoutEntry.LeftLayout) > 0)
                        {
                            this.LeftPanel.PanelLayout = value.LeftLayout;
                        }
                        if ((value.StoreEntry & TwoPanelLayoutEntry.RightLayout) > 0)
                        {
                            this.RightPanel.PanelLayout = value.RightLayout;
                        }
                        if ((value.StoreEntry & TwoPanelLayoutEntry.ActivePanel) > 0)
                        {
                            switch (value.ActivePanel)
                            {
                                case ActivePanel.Left:
                                    this.FLastCurrentPanel = this.LeftPanel;
                                    return;

                                case ActivePanel.Right:
                                    this.FLastCurrentPanel = this.RightPanel;
                                    return;
                            }
                        }
                    }
                    else
                    {
                        if ((value.StoreEntry & TwoPanelLayoutEntry.LeftLayout) > 0)
                        {
                            this.CurrentPanel.PanelLayout = value.LeftLayout;
                        }
                        if ((value.StoreEntry & TwoPanelLayoutEntry.RightLayout) > 0)
                        {
                            this.FarPanel.PanelLayout = value.RightLayout;
                        }
                        if ((value.StoreEntry & TwoPanelLayoutEntry.ActivePanel) > 0)
                        {
                            switch (value.ActivePanel)
                            {
                                case ActivePanel.Left:
                                    this.FLastCurrentPanel = this.CurrentPanel;
                                    return;

                                case ActivePanel.Right:
                                    this.FLastCurrentPanel = this.FarPanel;
                                    return;
                            }
                        }
                    }
                }
            }
        }

        public enum SinglePanel
        {
            None,
            Left,
            Right
        }
    }
}

