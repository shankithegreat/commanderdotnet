namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;

    public class NamedComponentDesigner : ComponentDesigner
    {
        private IComponentChangeService Service;

        private void ComponentRenameEventHandler(object sender, ComponentRenameEventArgs e)
        {
            ((NamedComponent) base.Component).Name = base.Component.Site.Name;
        }

        protected override void Dispose(bool disposing)
        {
            if (this.Service != null)
            {
                this.Service.ComponentRename -= new System.ComponentModel.Design.ComponentRenameEventHandler(this.ComponentRenameEventHandler);
                this.Service = null;
            }
            base.Dispose(disposing);
        }

        public override void Initialize(IComponent c)
        {
            base.Initialize(c);
            ((NamedComponent) c).Name = c.Site.Name;
            this.Service = (IComponentChangeService) this.GetService(typeof(IComponentChangeService));
            this.Service.ComponentRename += new System.ComponentModel.Design.ComponentRenameEventHandler(this.ComponentRenameEventHandler);
        }
    }
}

