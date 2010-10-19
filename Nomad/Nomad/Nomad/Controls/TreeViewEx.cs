namespace Nomad.Controls
{
    using Microsoft;
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class TreeViewEx : System.Windows.Forms.TreeView
    {
        private static readonly object EventGetNodeColors = new object();
        private static readonly object EventPostDrawNode = new object();
        private bool FExplorerTheme;
        private bool FFadePlusMinus;
        private static MethodInfo NodeFromHandleMethod;
        private static FieldInfo OnDrawNodeField;

        public event EventHandler<GetNodeColorsEventArgs> GetNodeColors
        {
            add
            {
                base.Events.AddHandler(EventGetNodeColors, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventGetNodeColors, value);
            }
        }

        public event EventHandler<PostDrawTreeNodeEventArgs> PostDrawNode
        {
            add
            {
                base.Events.AddHandler(EventPostDrawNode, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventPostDrawNode, value);
            }
        }

        static TreeViewEx()
        {
            System.Type type = typeof(System.Windows.Forms.TreeView);
            NodeFromHandleMethod = type.GetMethod("NodeFromHandle", BindingFlags.NonPublic | BindingFlags.Instance);
            OnDrawNodeField = type.GetField("onDrawNode", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public TreeViewEx()
        {
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            if (!OS.IsWinVista)
            {
                base.SetStyle(ControlStyles.UserPaint, true);
            }
        }

        protected TreeNode NodeFromHandle(IntPtr handle)
        {
            if (NodeFromHandleMethod != null)
            {
                return (TreeNode) NodeFromHandleMethod.Invoke(this, new object[] { handle });
            }
            return null;
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            if ((OnDrawNodeField != null) && (OnDrawNodeField.GetValue(this) == null))
            {
                e.DrawDefault = true;
            }
            base.OnDrawNode(e);
        }

        protected virtual void OnGetNodeColors(GetNodeColorsEventArgs e)
        {
            EventHandler<GetNodeColorsEventArgs> handler = base.Events[EventGetNodeColors] as EventHandler<GetNodeColorsEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.UpdateExtendedStyles();
            if (!OS.IsWinXP)
            {
                Windows.SendMessage(base.Handle, 0x111d, IntPtr.Zero, (IntPtr) ColorTranslator.ToWin32(this.BackColor));
            }
            if (!(base.DesignMode || !this.ExplorerTheme))
            {
                UxTheme.SetWindowTheme(base.Handle, "explorer", null);
            }
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

        protected virtual void OnPostDrawNode(PostDrawTreeNodeEventArgs e)
        {
            EventHandler<PostDrawTreeNodeEventArgs> handler = base.Events[EventPostDrawNode] as EventHandler<PostDrawTreeNodeEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void UpdateExtendedStyles()
        {
            if (base.IsHandleCreated)
            {
                TVS_EX dw = 0;
                if (this.DoubleBuffered)
                {
                    dw |= TVS_EX.TVS_EX_DOUBLEBUFFER;
                }
                if (this.FadePlusMinus)
                {
                    dw |= TVS_EX.TVS_EX_FADEINOUTEXPANDOS;
                }
                CommCtrl.TreeView_SetExtendedStyle(base.Handle, dw, TVS_EX.TVS_EX_FADEINOUTEXPANDOS | TVS_EX.TVS_EX_DOUBLEBUFFER);
            }
        }

        private void WmCustomDraw(ref Message m)
        {
            TreeNode node;
            TreeNodeStates itemState;
            switch (Microsoft.Win32.NMCUSTOMDRAW.GetDrawStage(m.LParam))
            {
                case CDDS.CDDS_ITEMPREPAINT:
                    node = this.NodeFromHandle(Microsoft.Win32.NMCUSTOMDRAW.GetItemSpec(m.LParam));
                    if (node != null)
                    {
                        if (!base.ClientRectangle.IntersectsWith(Microsoft.Win32.NMCUSTOMDRAW.GetRect(m.LParam)))
                        {
                            m.Result = (IntPtr) 4L;
                        }
                        else
                        {
                            System.Drawing.Color backColor = Microsoft.Win32.NMTVCUSTOMDRAW.GetBackColor(m.LParam);
                            System.Drawing.Color foreColor = Microsoft.Win32.NMTVCUSTOMDRAW.GetForeColor(m.LParam);
                            itemState = (TreeNodeStates) Microsoft.Win32.NMCUSTOMDRAW.GetItemState(m.LParam);
                            IntPtr ptr = Windows.SendMessage(base.Handle, 0x110a, (IntPtr) 8L, IntPtr.Zero);
                            if ((ptr != IntPtr.Zero) && (node.Handle == ptr))
                            {
                                itemState |= TreeNodeStates.Marked;
                            }
                            GetNodeColorsEventArgs e = new GetNodeColorsEventArgs(node, itemState, backColor, foreColor);
                            this.OnGetNodeColors(e);
                            if (e.BackColor != backColor)
                            {
                                Microsoft.Win32.NMTVCUSTOMDRAW.SetBackColor(m.LParam, e.BackColor);
                            }
                            if (e.ForeColor != foreColor)
                            {
                                Microsoft.Win32.NMTVCUSTOMDRAW.SetForeColor(m.LParam, e.ForeColor);
                            }
                            if (base.DrawMode != TreeViewDrawMode.Normal)
                            {
                                m.Result = (IntPtr) (((int) m.Result) | 0x10);
                            }
                        }
                        break;
                    }
                    break;

                case CDDS.CDDS_ITEMPOSTPAINT:
                    node = this.NodeFromHandle(Microsoft.Win32.NMCUSTOMDRAW.GetItemSpec(m.LParam));
                    if (node != null)
                    {
                        Rectangle bounds = node.Bounds;
                        if (base.ClientRectangle.IntersectsWith(bounds))
                        {
                            itemState = (TreeNodeStates) Microsoft.Win32.NMCUSTOMDRAW.GetItemState(m.LParam);
                            using (Graphics graphics = Graphics.FromHdcInternal(Microsoft.Win32.NMCUSTOMDRAW.GetHdc(m.LParam)))
                            {
                                PostDrawTreeNodeEventArgs args2 = new PostDrawTreeNodeEventArgs(graphics, node, bounds, itemState);
                                this.OnPostDrawNode(args2);
                            }
                        }
                    }
                    break;
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if ((m.Msg == 0x204e) && (Microsoft.Win32.NMHDR.GetNotifyCode(m.LParam) == -12))
            {
                this.WmCustomDraw(ref m);
            }
        }

        protected override bool DoubleBuffered
        {
            get
            {
                return base.DoubleBuffered;
            }
            set
            {
                if (base.DoubleBuffered != value)
                {
                    base.DoubleBuffered = value;
                    this.UpdateExtendedStyles();
                }
            }
        }

        [DefaultValue(false)]
        public bool ExplorerTheme
        {
            get
            {
                return (this.FExplorerTheme && (base.DesignMode || IsExplorerThemeSupported));
            }
            set
            {
                if (this.FExplorerTheme != value)
                {
                    this.FExplorerTheme = value;
                    if (base.IsHandleCreated && OS.IsWinVista)
                    {
                        UxTheme.SetWindowTheme(base.Handle, (this.FExplorerTheme && VisualStyleRenderer.IsSupported) ? "explorer" : null, null);
                    }
                }
            }
        }

        [DefaultValue(false)]
        public bool FadePlusMinus
        {
            get
            {
                return this.FFadePlusMinus;
            }
            set
            {
                if (this.FFadePlusMinus != value)
                {
                    this.FFadePlusMinus = value;
                    this.UpdateExtendedStyles();
                }
            }
        }

        public static bool IsExplorerThemeSupported
        {
            get
            {
                return (OS.IsWinVista && VisualStyleRenderer.IsSupported);
            }
        }
    }
}

