namespace Nomad.Controls.Actions
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ActionEventArgs : HandledEventArgs
    {
        public ActionEventArgs(CustomAction action) : this(action, null, null)
        {
        }

        public ActionEventArgs(CustomAction action, object target) : this(action, null, target)
        {
        }

        public ActionEventArgs(CustomAction action, object source, object target)
        {
            this.Action = action;
            this.Source = source;
            this.Target = target;
        }

        public CustomAction Action { get; private set; }

        public object Source { get; private set; }

        public object Tag
        {
            get
            {
                return this.Action.Tag;
            }
        }

        public object Target { get; private set; }
    }
}

