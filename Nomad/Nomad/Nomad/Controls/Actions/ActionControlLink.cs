namespace Nomad.Controls.Actions
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class ActionControlLink : CustomBindActionLink
    {
        private KeyEventHandler ControlKeyDownHandler;

        public ActionControlLink(CustomAction action, Component component) : base(action, component, null, BindActionProperty.All)
        {
        }

        public ActionControlLink(CustomAction action, Component component, BindActionProperty bind) : base(action, component, null, bind)
        {
        }

        public ActionControlLink(CustomAction action, Component component, object target, BindActionProperty bind) : base(action, component, target, bind)
        {
        }

        private void ActionShortcutsChanged(object sender, EventArgs e)
        {
            if (base.Action.ShortcutKeys != Keys.None)
            {
                this.StartControlKeyDownWatch();
            }
            else
            {
                this.StopControlKeyDownWatch();
            }
        }

        private void ActionTextChanged(object sender, EventArgs e)
        {
            this.BindControl.Text = base.Action.Text;
        }

        protected override void ApplicationIdle(object sender, EventArgs e)
        {
            base.ApplicationIdle(sender, e);
            if (!this.BindControl.Visible)
            {
                base.StopApplicationIdleUpdate();
            }
        }

        private void ControlClick(object sender, EventArgs e)
        {
            base.Action.Execute(base.Component, this.Target);
        }

        private void ControlDisposed(object sender, EventArgs e)
        {
            base.Dispose();
        }

        private void ControlKeyDown(object sender, KeyEventArgs e)
        {
            if (base.Action.IsShortcutDefined(e.KeyData))
            {
                base.Action.Execute(base.Component, this.Target);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void ControlVisibleChanged(object sender, EventArgs e)
        {
            base.UpdateAction();
            if (this.BindControl.Visible)
            {
                base.StartApplicationIdleUpdate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Action.ShortcutsChanged -= new EventHandler(this.ActionShortcutsChanged);
            base.Action.TextChanged -= new EventHandler(this.ActionTextChanged);
            this.BindControl.Click -= new EventHandler(this.ControlClick);
            this.BindControl.Disposed -= new EventHandler(this.ControlDisposed);
            this.BindControl.VisibleChanged -= new EventHandler(this.ControlVisibleChanged);
            this.StopControlKeyDownWatch();
            base.Dispose(disposing);
        }

        protected override void InitializeLink()
        {
            if (!(base.Component is Control))
            {
                throw new ArgumentException("Control component expected");
            }
            base.InitializeLink();
            if (base.CheckBindProperty(BindActionProperty.Text))
            {
                base.Action.TextChanged += new EventHandler(this.ActionTextChanged);
                this.BindControl.Text = base.Action.Text;
            }
            if (base.CheckBindProperty(BindActionProperty.Shortcuts))
            {
                base.Action.ShortcutsChanged += new EventHandler(this.ActionShortcutsChanged);
                if (base.Action.ShortcutKeys != Keys.None)
                {
                    this.StartControlKeyDownWatch();
                }
            }
            this.BindControl.Disposed += new EventHandler(this.ControlDisposed);
            if (base.CheckBindProperty(BindActionProperty.CanClick))
            {
                this.BindControl.Click += new EventHandler(this.ControlClick);
            }
            if (base.CheckBindProperty(BindActionProperty.CanUpdate))
            {
                this.BindControl.VisibleChanged += new EventHandler(this.ControlVisibleChanged);
                if (this.BindControl.Visible)
                {
                    base.StartApplicationIdleUpdate();
                }
            }
        }

        private void StartControlKeyDownWatch()
        {
            if (this.ControlKeyDownHandler == null)
            {
                this.ControlKeyDownHandler = new KeyEventHandler(this.ControlKeyDown);
                this.BindControl.KeyDown += this.ControlKeyDownHandler;
            }
        }

        private void StopControlKeyDownWatch()
        {
            if (this.ControlKeyDownHandler != null)
            {
                this.BindControl.KeyDown -= this.ControlKeyDownHandler;
                this.ControlKeyDownHandler = null;
            }
        }

        protected override void UpdateActionState(ActionState state)
        {
            base.UpdateActionState(state);
            if (base.CheckBindProperty(BindActionProperty.Enabled))
            {
                this.BindControl.Enabled = (state & ActionState.Enabled) > ActionState.None;
            }
            if (base.CheckBindProperty(BindActionProperty.Visible))
            {
                this.BindControl.Visible = (state & ActionState.Visible) > ActionState.None;
            }
        }

        public Control BindControl
        {
            get
            {
                return (Control) base.Component;
            }
        }
    }
}

