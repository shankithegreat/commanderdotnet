namespace Nomad.Controls.Actions
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public abstract class CustomBindActionLink : CustomActionLink
    {
        private PropertyInfo FComponentCheckedProperty;

        public CustomBindActionLink(CustomAction action, Component component, object target, BindActionProperty bind) : base(action, component, target, bind)
        {
        }

        protected override void Dispose(bool disposing)
        {
            this.FComponentCheckedProperty = null;
            base.Dispose(disposing);
        }

        protected override void InitializeLink()
        {
            base.InitializeLink();
            if (base.CheckBindProperty(BindActionProperty.Checked))
            {
                this.FComponentCheckedProperty = base.Component.GetType().GetProperty("Checked", BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance);
            }
        }

        protected override void UpdateActionState(ActionState state)
        {
            base.UpdateActionState(state);
            if (this.FComponentCheckedProperty != null)
            {
                this.FComponentCheckedProperty.SetValue(base.Component, (state & ActionState.Checked) > ActionState.None, null);
            }
        }
    }
}

