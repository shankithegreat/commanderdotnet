namespace Nomad.Controls.Actions
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class ActionToolStripItemLink : CustomActionLink
    {
        public ActionToolStripItemLink(CustomAction action, Component component) : base(action, component, null, BindActionProperty.All)
        {
        }

        public ActionToolStripItemLink(CustomAction action, Component component, BindActionProperty bind) : base(action, component, null, bind)
        {
        }

        public ActionToolStripItemLink(CustomAction action, Component component, object target, BindActionProperty bind) : base(action, component, target, bind)
        {
        }

        private void ActionImageChanged(object sender, EventArgs e)
        {
            this.SetToolStripItemImage(this.BindItem);
        }

        private void ActionShortcutsChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem component = base.Component as ToolStripMenuItem;
            if (component != null)
            {
                try
                {
                    component.ShortcutKeys = base.Action.ShortcutKeys;
                }
                catch (InvalidEnumArgumentException)
                {
                    component.ShortcutKeys = Keys.None;
                    component.ShortcutKeyDisplayString = TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString(base.Action.ShortcutKeys);
                }
            }
        }

        private void ActionTextChanged(object sender, EventArgs e)
        {
            this.BindItem.Text = base.Action.Text;
        }

        protected override void ApplicationIdle(object sender, EventArgs e)
        {
            base.ApplicationIdle(sender, e);
            if (!this.BindItem.Visible)
            {
                base.StopApplicationIdleUpdate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Action.ImageChanged -= new EventHandler(this.ActionImageChanged);
            base.Action.ShortcutsChanged -= new EventHandler(this.ActionShortcutsChanged);
            base.Action.TextChanged -= new EventHandler(this.ActionTextChanged);
            ToolStripSplitButton bindItem = this.BindItem as ToolStripSplitButton;
            if (bindItem != null)
            {
                bindItem.ButtonClick -= new EventHandler(this.ItemClick);
            }
            this.BindItem.Click -= new EventHandler(this.ItemClick);
            this.BindItem.OwnerChanged -= new EventHandler(this.ItemOwnerChanged);
            this.BindItem.Paint -= new PaintEventHandler(this.ItemPaint);
            this.BindItem.Disposed -= new EventHandler(this.ItemDisposed);
            base.Dispose(disposing);
        }

        protected override void InitializeLink()
        {
            if (!(base.Component is ToolStripItem))
            {
                throw new ArgumentException("ToolStripItem component expected");
            }
            base.InitializeLink();
            if (base.CheckBindProperty(BindActionProperty.Text))
            {
                base.Action.TextChanged += new EventHandler(this.ActionTextChanged);
                this.BindItem.Text = base.Action.Text;
            }
            if (base.CheckBindProperty(BindActionProperty.Image))
            {
                base.Action.ImageChanged += new EventHandler(this.ActionImageChanged);
                this.SetToolStripItemImage(this.BindItem);
            }
            if (base.CheckBindProperty(BindActionProperty.Shortcuts))
            {
                ToolStripMenuItem bindItem = this.BindItem as ToolStripMenuItem;
                if (bindItem != null)
                {
                    base.Action.ShortcutsChanged += new EventHandler(this.ActionShortcutsChanged);
                    this.ActionShortcutsChanged(bindItem, EventArgs.Empty);
                }
            }
            this.BindItem.Disposed += new EventHandler(this.ItemDisposed);
            if (base.CheckBindProperty(BindActionProperty.CanClick))
            {
                ToolStripSplitButton button = this.BindItem as ToolStripSplitButton;
                if (button != null)
                {
                    button.ButtonClick += new EventHandler(this.ItemClick);
                }
                else
                {
                    this.BindItem.Click += new EventHandler(this.ItemClick);
                }
            }
            if (base.CheckBindProperty(BindActionProperty.CanUpdate))
            {
                this.BindItem.OwnerChanged += new EventHandler(this.ItemOwnerChanged);
                this.BindItem.Paint += new PaintEventHandler(this.ItemPaint);
                if (this.BindItem.Visible)
                {
                    base.StartApplicationIdleUpdate();
                }
            }
        }

        private void ItemClick(object sender, EventArgs e)
        {
            base.Action.Execute(base.Component, this.Target);
        }

        private void ItemDisposed(object sender, EventArgs e)
        {
            base.Dispose();
        }

        private void ItemOwnerChanged(object sender, EventArgs e)
        {
            if (this.BindItem.Owner != null)
            {
                base.UpdateAction();
            }
        }

        private void ItemPaint(object sender, PaintEventArgs e)
        {
            ToolStripItem bindItem = this.BindItem;
            if (!((((bindItem.Owner == null) || !bindItem.Owner.IsHandleCreated) || bindItem.Owner.IsDisposed) || bindItem.Owner.Disposing))
            {
                bindItem.Owner.BeginInvoke(new MethodInvoker(this.UpdateAction));
            }
            if (bindItem.Visible)
            {
                base.StartApplicationIdleUpdate();
            }
        }

        private void SetToolStripItemImage(ToolStripItem item)
        {
            if (item.Image != base.Action.Image)
            {
                item.Image = base.Action.Image;
            }
            if (item.ImageIndex != base.Action.ImageIndex)
            {
                item.ImageIndex = base.Action.ImageIndex;
            }
            if (item.ImageKey != base.Action.ImageKey)
            {
                item.ImageKey = base.Action.ImageKey;
            }
        }

        protected override void UpdateActionState(ActionState state)
        {
            base.UpdateActionState(state);
            if (base.CheckBindProperty(BindActionProperty.Enabled))
            {
                this.BindItem.Enabled = (state & ActionState.Enabled) > ActionState.None;
            }
            if (base.CheckBindProperty(BindActionProperty.Checked))
            {
                ToolStripButton bindItem = this.BindItem as ToolStripButton;
                if (bindItem != null)
                {
                    bindItem.Checked = (state & ActionState.Checked) > ActionState.None;
                }
                else
                {
                    ToolStripMenuItem item = this.BindItem as ToolStripMenuItem;
                    if (item != null)
                    {
                        if ((state & ActionState.Checked) > ActionState.None)
                        {
                            item.CheckState = CheckState.Checked;
                        }
                        else if ((state & ActionState.Indeterminate) > ActionState.None)
                        {
                            item.CheckState = CheckState.Indeterminate;
                        }
                        else
                        {
                            item.CheckState = CheckState.Unchecked;
                        }
                    }
                }
            }
            if (base.CheckBindProperty(BindActionProperty.Visible))
            {
                this.BindItem.Visible = (state & ActionState.Visible) > ActionState.None;
            }
        }

        public ToolStripItem BindItem
        {
            get
            {
                return (ToolStripItem) base.Component;
            }
        }

        public override object Target
        {
            get
            {
                object target = base.Target;
                if (target == null)
                {
                    ToolStripDropDown owner = this.BindItem.Owner as ToolStripDropDown;
                    while ((owner != null) && !(owner is ContextMenuStrip))
                    {
                        owner = (owner.OwnerItem != null) ? (owner.OwnerItem.Owner as ToolStripDropDown) : null;
                    }
                    ContextMenuStrip strip = owner as ContextMenuStrip;
                    if (strip != null)
                    {
                        return strip.SourceControl;
                    }
                }
                return target;
            }
        }
    }
}

