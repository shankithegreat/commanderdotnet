namespace Nomad.Controls.Actions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;

    [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
    public class ActionList : List<Action>
    {
        private ActionManager FOwner;

        public ActionList()
        {
        }

        public ActionList(ActionManager owner)
        {
            this.FOwner = owner;
        }

        public void AddRange(Action[] actions)
        {
            base.AddRange((IEnumerable<Action>) actions);
            foreach (Action action in actions)
            {
                action.Owner = this.FOwner;
            }
        }

        public ActionManager Owner
        {
            get
            {
                return this.FOwner;
            }
        }
    }
}

