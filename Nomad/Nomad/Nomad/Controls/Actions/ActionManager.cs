namespace Nomad.Controls.Actions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    [ProvideProperty("Action", typeof(Component)), ToolboxItemFilter("System.Windows.Forms"), DesignerCategory("Code"), DefaultProperty("ActionList")]
    public class ActionManager : Component, IExtenderProvider, ISupportInitialize
    {
        private Dictionary<Component, Action> ComponentActionMap;
        private ActionList FActionList;
        private List<KeyValuePair<Component, Action>> InitComponentActionList;
        private int InitCount;

        [Category("Action")]
        public event EventHandler<ActionEventArgs> PreviewExecuteAction;

        [Category("Action")]
        public event EventHandler<UpdateActionEventArgs> PreviewUpdateAction;

        public ActionManager()
        {
            this.ComponentActionMap = new Dictionary<Component, Action>();
        }

        public ActionManager(IContainer container)
        {
            this.ComponentActionMap = new Dictionary<Component, Action>();
            container.Add(this);
        }

        private void ActionDisposed(object sender, EventArgs e)
        {
            List<Component> list = new List<Component>();
            foreach (KeyValuePair<Component, Action> pair in this.ComponentActionMap)
            {
                if (pair.Value == sender)
                {
                    list.Add(pair.Key);
                }
            }
            foreach (Component component in list)
            {
                this.RemoveComponent(component);
            }
            ((Action) sender).Disposed -= new EventHandler(this.ActionDisposed);
        }

        private void AddComponentAction(Component component, Action action, object target, BindActionProperty bind)
        {
            IActionLink link = null;
            if (component is Control)
            {
                link = new ActionControlLink(action, component, target, bind);
            }
            else if (component is ToolStripItem)
            {
                link = new ActionToolStripItemLink(action, component, target, bind);
                if (base.DesignMode)
                {
                    TypeDescriptor.AddProvider(new ActionControlTypeDescriptionProvider(component), component);
                }
            }
            if (link != null)
            {
                this.ComponentActionMap.Add(component, action);
                component.Disposed += new EventHandler(this.ComponentDisposed);
                if (!this.ComponentActionMap.ContainsValue(action))
                {
                    action.Disposed += new EventHandler(this.ActionDisposed);
                }
            }
        }

        public void BeginInit()
        {
            this.InitCount++;
            if (this.InitComponentActionList == null)
            {
                this.InitComponentActionList = new List<KeyValuePair<Component, Action>>();
            }
        }

        public bool CanExtend(object extendee)
        {
            return ((extendee is Control) || (extendee is ToolStripItem));
        }

        private void ComponentDisposed(object sender, EventArgs e)
        {
            this.RemoveComponent((Component) sender);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (Action action in this.Actions)
                {
                    action.Dispose();
                }
                this.Actions.Clear();
            }
            base.Dispose(disposing);
        }

        public void EndInit()
        {
            if (this.InitCount == 0)
            {
                throw new InvalidOperationException();
            }
            this.InitCount--;
            if (this.InitCount == 0)
            {
                foreach (KeyValuePair<Component, Action> pair in this.InitComponentActionList)
                {
                    this.AddComponentAction(pair.Key, pair.Value, null, BindActionProperty.All);
                }
                this.InitComponentActionList = null;
            }
        }

        [DefaultValue(typeof(Action), "")]
        public Action GetAction(Component component)
        {
            Action action;
            if (component == null)
            {
                throw new ArgumentNullException();
            }
            if (this.ComponentActionMap.TryGetValue(component, out action))
            {
                return action;
            }
            return null;
        }

        public IEnumerable<Action> GetUnlinkedActions()
        {
            return new <GetUnlinkedActions>d__0(-2) { <>4__this = this };
        }

        public bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            foreach (Action action in this.Actions)
            {
                if (action.IsShortcutDefined(keyData))
                {
                    return action.Execute();
                }
            }
            return false;
        }

        protected internal void RaisePreviewExecuteAction(ActionEventArgs e)
        {
            if (this.PreviewExecuteAction != null)
            {
                this.PreviewExecuteAction(this, e);
            }
        }

        protected internal void RaisePreviewUpdateAction(UpdateActionEventArgs e)
        {
            if (this.PreviewUpdateAction != null)
            {
                this.PreviewUpdateAction(this, e);
            }
        }

        private void RemoveComponent(Component component)
        {
            component.Disposed -= new EventHandler(this.ComponentDisposed);
            this.ComponentActionMap.Remove(component);
        }

        public void SetAction(Component component, Action action)
        {
            this.SetAction(component, action, null, BindActionProperty.All);
        }

        public void SetAction(Component component, Action action, BindActionProperty bind)
        {
            this.SetAction(component, action, null, bind);
        }

        public void SetAction(Component component, Action action, object target, BindActionProperty bind)
        {
            Action action2;
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            if (this.ComponentActionMap.TryGetValue(component, out action2))
            {
                if (action2 == action)
                {
                    return;
                }
                this.RemoveComponent(component);
                if (!this.ComponentActionMap.ContainsValue(action2))
                {
                    action2.Disposed -= new EventHandler(this.ActionDisposed);
                }
            }
            if (action != null)
            {
                if (((this.InitComponentActionList != null) && (target == null)) && (bind == BindActionProperty.All))
                {
                    this.InitComponentActionList.Add(new KeyValuePair<Component, Action>(component, action));
                }
                else
                {
                    this.AddComponentAction(component, action, target, bind);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ActionList Actions
        {
            get
            {
                return (this.FActionList ?? (this.FActionList = new ActionList(this)));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICollection<Component> Components
        {
            get
            {
                return this.ComponentActionMap.Keys;
            }
        }

        [CompilerGenerated]
        private sealed class <GetUnlinkedActions>d__0 : IEnumerable<Action>, IEnumerable, IEnumerator<Action>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private Action <>2__current;
            public ActionManager <>4__this;
            public List<Action>.Enumerator <>7__wrap3;
            private int <>l__initialThreadId;
            public Dictionary<Action, int> <LinkedActionMap>5__1;
            public Action <NextAction>5__2;

            [DebuggerHidden]
            public <GetUnlinkedActions>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally4()
            {
                this.<>1__state = -1;
                this.<>7__wrap3.Dispose();
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    int num = this.<>1__state;
                    if (num != 0)
                    {
                        if (num != 3)
                        {
                            goto Label_010F;
                        }
                        goto Label_00EF;
                    }
                    this.<>1__state = -1;
                    this.<LinkedActionMap>5__1 = new Dictionary<Action, int>();
                    foreach (Action action in this.<>4__this.ComponentActionMap.Values)
                    {
                        if (!this.<LinkedActionMap>5__1.ContainsKey(action))
                        {
                            this.<LinkedActionMap>5__1.Add(action, 0);
                        }
                    }
                    this.<>7__wrap3 = this.<>4__this.Actions.GetEnumerator();
                    this.<>1__state = 2;
                    while (this.<>7__wrap3.MoveNext())
                    {
                        this.<NextAction>5__2 = this.<>7__wrap3.Current;
                        if (this.<LinkedActionMap>5__1.ContainsKey(this.<NextAction>5__2))
                        {
                            continue;
                        }
                        this.<>2__current = this.<NextAction>5__2;
                        this.<>1__state = 3;
                        return true;
                    Label_00EF:
                        this.<>1__state = 2;
                    }
                    this.<>m__Finally4();
                Label_010F:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<Action> IEnumerable<Action>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new ActionManager.<GetUnlinkedActions>d__0(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.Controls.Actions.Action>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 2:
                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally4();
                        }
                        break;
                }
            }

            Action IEnumerator<Action>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

