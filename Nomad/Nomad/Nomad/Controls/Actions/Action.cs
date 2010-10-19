namespace Nomad.Controls.Actions
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    [DesignerCategory("Code"), DesignTimeVisible(false), ToolboxItem(false), DefaultEvent("OnExecute")]
    public class Action : CustomAction
    {
        [Category("Action")]
        public event EventHandler<ActionEventArgs> OnExecute;

        [Category("Action")]
        public event EventHandler<UpdateActionEventArgs> OnUpdate;

        public Action()
        {
        }

        public Action(EventHandler<ActionEventArgs> onExecute)
        {
            this.OnExecute = (EventHandler<ActionEventArgs>) Delegate.Combine(this.OnExecute, onExecute);
        }

        public Action(EventHandler<ActionEventArgs> onExecute, EventHandler<UpdateActionEventArgs> onUpdate)
        {
            this.OnExecute = (EventHandler<ActionEventArgs>) Delegate.Combine(this.OnExecute, onExecute);
            this.OnUpdate = (EventHandler<UpdateActionEventArgs>) Delegate.Combine(this.OnUpdate, onUpdate);
        }

        protected override void DoExecute(ActionEventArgs e)
        {
            base.DoExecute(e);
            if (!(e.Handled || (this.OnExecute == null)))
            {
                this.OnExecute(this, e);
                e.Handled = true;
            }
        }

        protected override void DoUpdate(UpdateActionEventArgs e)
        {
            base.DoUpdate(e);
            if (!(e.Handled || (this.OnUpdate == null)))
            {
                this.OnUpdate(this, e);
                e.Handled = true;
            }
            else
            {
                e.State = ActionState.Visible | ((this.OnExecute != null) ? ActionState.Enabled : ActionState.None);
            }
        }
    }
}

