namespace Nomad.Controls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal class TabPageSwitcherDesigner : ParentControlDesigner
    {
        private ISelectionService selectionService;
        private DesignerVerbCollection verbs;

        public override bool CanParent(Control control)
        {
            return (control is TabStripPage);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.SelectionService.SelectionChanged -= new EventHandler(this.SelectionService_SelectionChanged);
            }
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            this.SelectionService.SelectionChanged += new EventHandler(this.SelectionService_SelectionChanged);
        }

        private void OnAdd(object sender, EventArgs eevent)
        {
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            if (service != null)
            {
                DesignerTransaction transaction = null;
                try
                {
                    try
                    {
                        transaction = service.CreateTransaction("Add TabStrip page" + base.Component.Site.Name);
                    }
                    catch (CheckoutException exception)
                    {
                        if (exception != CheckoutException.Canceled)
                        {
                            throw exception;
                        }
                        return;
                    }
                    MemberDescriptor member = TypeDescriptor.GetProperties(this.ControlSwitcher)["Controls"];
                    TabStripPage page = service.CreateComponent(typeof(TabStripPage)) as TabStripPage;
                    base.RaiseComponentChanging(member);
                    this.ControlSwitcher.Controls.Add(page);
                    this.SetProperty("SelectedTabStripPage", page);
                    base.RaiseComponentChanged(member, null, null);
                    if (this.ControlSwitcher.TabStrip != null)
                    {
                        MemberDescriptor descriptor2 = TypeDescriptor.GetProperties(this.ControlSwitcher.TabStrip)["Items"];
                        Tab tab = service.CreateComponent(typeof(Tab)) as Tab;
                        base.RaiseComponentChanging(descriptor2);
                        this.ControlSwitcher.TabStrip.Items.Add(tab);
                        base.RaiseComponentChanged(descriptor2, null, null);
                        this.SetProperty(tab, "DisplayStyle", ToolStripItemDisplayStyle.ImageAndText);
                        this.SetProperty(tab, "Text", tab.Name);
                        this.SetProperty(tab, "TabStripPage", page);
                        this.SetProperty(this.ControlSwitcher.TabStrip, "SelectedTab", tab);
                    }
                }
                finally
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
            }
        }

        private void SelectionService_SelectionChanged(object sender, EventArgs e)
        {
            IList selectedComponents = (IList) this.SelectionService.GetSelectedComponents();
            if (selectedComponents.Count == 1)
            {
                Tab target = selectedComponents[0] as Tab;
                if (target != null)
                {
                    this.SetProperty("SelectedTabStripPage", target.TabStripPage);
                    this.SetProperty(target, "Checked", true);
                }
            }
        }

        private void SetProperty(string propname, object value)
        {
            this.SetProperty(this.ControlSwitcher, propname, value);
        }

        private void SetProperty(object target, string propname, object value)
        {
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(target)[propname];
            if (descriptor != null)
            {
                descriptor.SetValue(target, value);
            }
        }

        public TabPageSwitcher ControlSwitcher
        {
            get
            {
                return (base.Component as TabPageSwitcher);
            }
        }

        internal ISelectionService SelectionService
        {
            get
            {
                if (this.selectionService == null)
                {
                    this.selectionService = (ISelectionService) this.GetService(typeof(ISelectionService));
                    Debug.Assert(this.selectionService != null, "Failed to get Selection Service!");
                }
                return this.selectionService;
            }
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (this.verbs == null)
                {
                    this.verbs = new DesignerVerbCollection();
                    this.verbs.Add(new DesignerVerb("Add TabStrip page", new EventHandler(this.OnAdd)));
                }
                return this.verbs;
            }
        }
    }
}

