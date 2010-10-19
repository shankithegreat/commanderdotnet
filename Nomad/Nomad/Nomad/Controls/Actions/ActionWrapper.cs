namespace Nomad.Controls.Actions
{
    using System;
    using System.ComponentModel;

    [ToolboxItem(false), DesignTimeVisible(false), DesignerCategory("Code")]
    public class ActionWrapper : CustomAction
    {
        private IAction FAction;

        public ActionWrapper()
        {
        }

        public ActionWrapper(IAction action)
        {
            this.FAction = action;
        }

        protected override void DoExecute(ActionEventArgs e)
        {
            base.DoExecute(e);
            e.Handled = e.Handled || ((this.FAction != null) && this.FAction.Execute(e.Source, e.Target));
        }

        protected override void DoUpdate(UpdateActionEventArgs e)
        {
            base.DoUpdate(e);
            if (!(e.Handled || (this.FAction == null)))
            {
                e.State = this.FAction.Update(e.Source, e.Target);
            }
        }

        public IAction Action
        {
            get
            {
                return this.FAction;
            }
            set
            {
                this.FAction = value;
            }
        }
    }
}

