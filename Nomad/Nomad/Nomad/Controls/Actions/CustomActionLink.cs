namespace Nomad.Controls.Actions
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public abstract class CustomActionLink : IActionLink, IDisposable
    {
        private EventHandler ApplicationIdleHandler;
        private CustomAction FAction;
        private BindActionProperty FBindProperty;
        private System.ComponentModel.Component FComponent;
        private object FTarget;

        public CustomActionLink(CustomAction action, System.ComponentModel.Component component, object target, BindActionProperty bind)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            this.FAction = action;
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            this.FComponent = component;
            this.FTarget = target;
            this.FBindProperty = bind;
            this.InitializeLink();
        }

        private void ActionDisposed(object sender, EventArgs e)
        {
            this.Dispose();
        }

        protected virtual void ApplicationIdle(object sender, EventArgs e)
        {
            this.UpdateAction();
        }

        protected bool CheckBindProperty(BindActionProperty property)
        {
            return ((this.FBindProperty & property) == property);
        }

        public void Dispose()
        {
            if (this.FAction != null)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            this.FAction.Disposed -= new EventHandler(this.ActionDisposed);
            this.StopApplicationIdleUpdate();
            this.FAction = null;
        }

        protected virtual void InitializeLink()
        {
            this.FAction.Disposed += new EventHandler(this.ActionDisposed);
        }

        protected void StartApplicationIdleUpdate()
        {
            if (this.ApplicationIdleHandler == null)
            {
                this.ApplicationIdleHandler = new EventHandler(this.ApplicationIdle);
                Application.Idle += this.ApplicationIdleHandler;
            }
        }

        protected void StopApplicationIdleUpdate()
        {
            if (this.ApplicationIdleHandler != null)
            {
                Application.Idle -= this.ApplicationIdleHandler;
                this.ApplicationIdleHandler = null;
            }
        }

        protected void UpdateAction()
        {
            if (this.FAction != null)
            {
                this.UpdateActionState(this.FAction.Update(this.Component, this.Target));
            }
        }

        protected virtual void UpdateActionState(ActionState state)
        {
        }

        public CustomAction Action
        {
            get
            {
                return this.FAction;
            }
        }

        public System.ComponentModel.Component Component
        {
            get
            {
                return this.FComponent;
            }
        }

        public virtual object Target
        {
            get
            {
                return this.FTarget;
            }
        }
    }
}

