namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [DesignerCategory("Code"), ToolboxItem(false), Docking(DockingBehavior.Never)]
    public class TabStripPage : Panel
    {
        public void Activate()
        {
            TabPageSwitcher parent = base.Parent as TabPageSwitcher;
            if (parent != null)
            {
                parent.SelectedTabStripPage = this;
            }
        }
    }
}

