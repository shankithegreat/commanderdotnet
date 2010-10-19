namespace Nomad.Controls.Actions
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;

    [DesignerCategory("Code"), DesignTimeVisible(false), ToolboxItem(false)]
    public abstract class StandardAction : CustomAction
    {
        private CommandID FActionId;

        protected StandardAction(CommandID actionId)
        {
            this.FActionId = actionId;
        }

        public CommandID ActionId
        {
            get
            {
                return this.FActionId;
            }
        }

        public abstract string DefaultText { get; }

        public override string Text
        {
            get
            {
                return (string.IsNullOrEmpty(base.Text) ? this.DefaultText : base.Text);
            }
            set
            {
                base.Text = value;
            }
        }
    }
}

