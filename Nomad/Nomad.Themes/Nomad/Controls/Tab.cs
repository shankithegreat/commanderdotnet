namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.None), DesignerCategory("Code")]
    public class Tab : ToolStripButton
    {
        private int FFixedWidth;
        private Nomad.Controls.TabStripPage tabStripPage;

        public event DragEventHandler DragHover;

        public event MouseEventHandler MouseClick;

        public Tab()
        {
            this.Initialize();
        }

        public Tab(Image image) : base(null, image, null)
        {
            this.Initialize();
        }

        public Tab(string text) : base(text, null, null)
        {
            this.Initialize();
        }

        public Tab(string text, Image image) : base(text, image, null)
        {
            this.Initialize();
        }

        public Tab(string text, Image image, EventHandler onClick) : base(text, image, onClick)
        {
            this.Initialize();
        }

        public Tab(string text, Image image, EventHandler onClick, string name) : base(text, image, onClick, name)
        {
            this.Initialize();
        }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            Size preferredSize = base.GetPreferredSize(constrainingSize);
            TabStrip owner = base.Owner as TabStrip;
            if (owner != null)
            {
                if (owner.Renderer is TabStripSystemRenderer)
                {
                    preferredSize.Height += 3;
                }
                if (owner.Renderer is TabStripIE7Renderer)
                {
                    preferredSize.Height += 7;
                }
            }
            return preferredSize;
        }

        private void Initialize()
        {
        }

        internal void InvokeDragEvent(DragEventType eventType, DragEventArgs dragEvent)
        {
            switch (eventType)
            {
                case DragEventType.Enter:
                    this.OnDragEnter(dragEvent);
                    break;

                case DragEventType.Leave:
                    this.OnDragLeave(EventArgs.Empty);
                    break;

                case DragEventType.Drop:
                    this.OnDragDrop(dragEvent);
                    break;

                case DragEventType.Over:
                    this.OnDragOver(dragEvent);
                    break;

                case DragEventType.Hover:
                    this.OnDragHover(dragEvent);
                    break;
            }
        }

        protected virtual void OnDragHover(DragEventArgs dragEvent)
        {
            if (this.DragHover != null)
            {
                this.DragHover(this, dragEvent);
            }
        }

        protected virtual void OnMouseClick(MouseEventArgs e)
        {
            if (this.MouseClick != null)
            {
                this.MouseClick(this, e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.OnMouseClick(e);
            base.OnMouseUp(e);
        }

        private void TabStripPage_Disposed(object sender, EventArgs e)
        {
            Nomad.Controls.TabStripPage page = (Nomad.Controls.TabStripPage) sender;
            page.Disposed -= new EventHandler(this.TabStripPage_Disposed);
            bool flag = false;
            TabStrip owner = base.Owner as TabStrip;
            if ((owner != null) && base.Checked)
            {
                Tab tab = owner.GetNextTab(this, !this.IsLastTab, false);
                if (tab != null)
                {
                    owner.SuspendLayout();
                    tab.PerformClick();
                    flag = true;
                }
            }
            base.Dispose();
            if (flag)
            {
                owner.ResumeLayout();
            }
        }

        [DefaultValue(false), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool CheckOnClick
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        protected override ToolStripItemDisplayStyle DefaultDisplayStyle
        {
            get
            {
                return ToolStripItemDisplayStyle.ImageAndText;
            }
        }

        protected override Padding DefaultPadding
        {
            get
            {
                TabStrip owner = base.Owner as TabStrip;
                if ((owner != null) && (owner.Renderer is TabStripProfessionalRenderer))
                {
                    return new Padding(0x23, 0, 6, 0);
                }
                return base.DefaultPadding;
            }
        }

        [DefaultValue(0)]
        public int FixedWidth
        {
            get
            {
                return this.FFixedWidth;
            }
            set
            {
                base.AutoSize = false;
                base.Width = value;
                this.FFixedWidth = value;
            }
        }

        public bool IsFirstTab
        {
            get
            {
                TabStrip owner = base.Owner as TabStrip;
                if (owner != null)
                {
                    for (int i = 0; i < owner.Items.Count; i++)
                    {
                        if (owner.Items[i] is Tab)
                        {
                            return (owner.Items[i] == this);
                        }
                    }
                }
                return false;
            }
        }

        public bool IsLastTab
        {
            get
            {
                TabStrip owner = base.Owner as TabStrip;
                if (owner != null)
                {
                    for (int i = owner.Items.Count - 1; i >= 0; i--)
                    {
                        if (owner.Items[i] is Tab)
                        {
                            return (owner.Items[i] == this);
                        }
                    }
                }
                return false;
            }
        }

        public int TabIndex
        {
            get
            {
                TabStrip owner = base.Owner as TabStrip;
                if (owner != null)
                {
                    int num = 0;
                    for (int i = 0; i < owner.Items.Count; i++)
                    {
                        if (owner.Items[i] == this)
                        {
                            return num;
                        }
                        if (owner.Items[i] is Tab)
                        {
                            num++;
                        }
                    }
                }
                return -1;
            }
        }

        [DefaultValue((string) null)]
        public Nomad.Controls.TabStripPage TabStripPage
        {
            get
            {
                return this.tabStripPage;
            }
            set
            {
                if (this.tabStripPage != value)
                {
                    if (this.tabStripPage != null)
                    {
                        this.tabStripPage.Disposed -= new EventHandler(this.TabStripPage_Disposed);
                    }
                    this.tabStripPage = value;
                    if (this.tabStripPage != null)
                    {
                        this.tabStripPage.Disposed += new EventHandler(this.TabStripPage_Disposed);
                    }
                }
            }
        }
    }
}

