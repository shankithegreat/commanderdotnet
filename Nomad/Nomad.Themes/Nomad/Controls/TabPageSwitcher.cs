namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [Designer(typeof(TabPageSwitcherDesigner)), DesignerCategory("code"), Docking(DockingBehavior.AutoDock), ToolboxItem(false)]
    public class TabPageSwitcher : ContainerControl
    {
        private TabStripPage FSelectedPage;
        private Nomad.Controls.TabStrip tabStrip;

        public event EventHandler Load;

        public event EventHandler SelectedTabStripPageChanged;

        public TabPageSwitcher()
        {
            this.ResetBackColor();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.Dock = DockStyle.Fill;
            if ((!this.IsLayoutSuspended() && (e.Control != this.FSelectedPage)) && e.Control.Enabled)
            {
                e.Control.SuspendLayout();
                e.Control.Enabled = false;
            }
            base.OnControlAdded(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!base.RecreatingHandle)
            {
                this.OnLoad(EventArgs.Empty);
            }
        }

        protected virtual void OnLoad(EventArgs e)
        {
            if (this.Load != null)
            {
                this.Load(this, e);
            }
            if (!base.DesignMode && (this.TabStrip != null))
            {
                if (this.TabStrip.SelectedTab == null)
                {
                    foreach (ToolStripItem item in this.TabStrip.Items)
                    {
                        this.TabStrip.SelectedTab = item as Tab;
                        if (this.TabStrip.SelectedTab != null)
                        {
                            break;
                        }
                    }
                }
                if (this.TabStrip.SelectedTab != null)
                {
                    this.SelectedTabStripPage = this.TabStrip.SelectedTab.TabStripPage;
                }
            }
        }

        protected virtual void OnSelectedTabStripPageChanged(EventArgs e)
        {
            if (this.SelectedTabStripPageChanged != null)
            {
                this.SelectedTabStripPageChanged(this, e);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys keys = keyData;
            if (keys != (Keys.Control | Keys.Tab))
            {
                if (keys != (Keys.Control | Keys.Shift | Keys.Tab))
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }
            }
            else
            {
                if (this.TabStrip != null)
                {
                    this.TabStrip.SelectNextTab(true);
                }
                return true;
            }
            if (this.TabStrip != null)
            {
                this.TabStrip.SelectNextTab(false);
            }
            return true;
        }

        public override void ResetBackColor()
        {
            this.BackColor = ProfessionalColors.ToolStripGradientEnd;
        }

        private bool ShouldSerializeBackColor()
        {
            return (this.BackColor != ProfessionalColors.ToolStripGradientEnd);
        }

        protected override Padding DefaultPadding
        {
            get
            {
                return new Padding(2);
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(150, 150);
            }
        }

        public TabStripPage SelectedTabStripPage
        {
            get
            {
                return this.FSelectedPage;
            }
            set
            {
                if (this.FSelectedPage != value)
                {
                    this.FSelectedPage = value;
                    if (this.FSelectedPage != null)
                    {
                        using (new LockWindowRedraw(this, true))
                        {
                            this.FSelectedPage.Enabled = true;
                            foreach (Control control in base.Controls)
                            {
                                if ((control != this.FSelectedPage) && control.Enabled)
                                {
                                    control.SuspendLayout();
                                    control.Enabled = false;
                                }
                            }
                            if (!base.Controls.Contains(value))
                            {
                                base.Controls.Add(this.FSelectedPage);
                            }
                            else
                            {
                                this.FSelectedPage.BringToFront();
                                this.FSelectedPage.ResumeLayout();
                            }
                        }
                    }
                    this.OnSelectedTabStripPageChanged(EventArgs.Empty);
                }
            }
        }

        public Nomad.Controls.TabStrip TabStrip
        {
            get
            {
                return this.tabStrip;
            }
            set
            {
                this.tabStrip = value;
            }
        }
    }
}

