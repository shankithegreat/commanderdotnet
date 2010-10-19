namespace Nomad.Controls.Actions
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class UpdateActionEventArgs : ActionEventArgs
    {
        private ActionState FState;

        public UpdateActionEventArgs(CustomAction action) : this(action, null, null)
        {
        }

        public UpdateActionEventArgs(CustomAction action, object target) : this(action, null, target)
        {
        }

        public UpdateActionEventArgs(CustomAction action, object source, object target) : base(action, source, target)
        {
            this.FState = ActionState.Visible | ActionState.Enabled;
        }

        private bool CheckActionState(ActionState state)
        {
            return ((this.FState & state) == state);
        }

        private void SetActionState(ActionState state, bool value)
        {
            if (value)
            {
                this.FState |= state;
            }
            else
            {
                this.FState &= ~state;
            }
        }

        public bool Checked
        {
            get
            {
                return this.CheckActionState(ActionState.Checked);
            }
            set
            {
                this.SetActionState(ActionState.Checked, value);
            }
        }

        public System.Windows.Forms.CheckState CheckState
        {
            get
            {
                if (this.CheckActionState(ActionState.Checked))
                {
                    return System.Windows.Forms.CheckState.Checked;
                }
                if (this.CheckActionState(ActionState.Indeterminate))
                {
                    return System.Windows.Forms.CheckState.Indeterminate;
                }
                return System.Windows.Forms.CheckState.Unchecked;
            }
            set
            {
                switch (value)
                {
                    case System.Windows.Forms.CheckState.Unchecked:
                        this.SetActionState(ActionState.Indeterminate | ActionState.Checked, false);
                        break;

                    case System.Windows.Forms.CheckState.Checked:
                        this.SetActionState(ActionState.Checked, true);
                        this.SetActionState(ActionState.Indeterminate, false);
                        break;

                    case System.Windows.Forms.CheckState.Indeterminate:
                        this.SetActionState(ActionState.Checked, false);
                        this.SetActionState(ActionState.Indeterminate, true);
                        break;

                    default:
                        throw new InvalidEnumArgumentException();
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return this.CheckActionState(ActionState.Enabled);
            }
            set
            {
                this.SetActionState(ActionState.Enabled, value);
            }
        }

        public ActionState State
        {
            get
            {
                return this.FState;
            }
            set
            {
                this.FState = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this.CheckActionState(ActionState.Visible);
            }
            set
            {
                this.SetActionState(ActionState.Visible, value);
            }
        }
    }
}

