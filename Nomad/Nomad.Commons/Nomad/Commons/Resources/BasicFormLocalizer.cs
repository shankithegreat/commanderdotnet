namespace Nomad.Commons.Resources
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public abstract class BasicFormLocalizer
    {
        private Stack<Control> RootControls = new Stack<Control>();
        private Stack<Control> SuspendedControls = new Stack<Control>();

        protected BasicFormLocalizer()
        {
        }

        protected virtual void AfterLocalizeRootControl()
        {
        }

        protected abstract void ApplyResources(Component component, string componentName, CultureInfo culture);
        protected virtual void BeforeLocalizeRootControl(Control rootControl)
        {
        }

        protected virtual void BeforeLocalizeType(System.Type controlType)
        {
        }

        private void DoLocalize(CultureInfo culture)
        {
            while (this.RootControls.Count > 0)
            {
                Control item = this.RootControls.Pop();
                item.SuspendLayout();
                this.SuspendedControls.Push(item);
                this.BeforeLocalizeRootControl(item);
                this.LocalizeRootControl(item, culture);
                this.AfterLocalizeRootControl();
            }
            while (this.SuspendedControls.Count > 0)
            {
                Control control2 = this.SuspendedControls.Pop();
                control2.ResumeLayout((culture != null) || (control2 is ToolStrip));
            }
        }

        private IEnumerable<ToolStripItem> GetChildrenItems(ToolStrip ts)
        {
            return new <GetChildrenItems>d__0(-2) { <>4__this = this, <>3__ts = ts };
        }

        public void Localize(Form form)
        {
            this.Localize(form, null);
        }

        public void Localize(UserControl userControl)
        {
            this.Localize(userControl, null);
        }

        public void Localize(Form form, CultureInfo culture)
        {
            if (form == null)
            {
                throw new ArgumentNullException();
            }
            if (this.RootControls.Count > 0)
            {
                throw new InvalidOperationException();
            }
            this.RootControls.Push(form);
            this.DoLocalize(culture);
        }

        public void Localize(UserControl userControl, CultureInfo culture)
        {
            if (userControl == null)
            {
                throw new ArgumentNullException();
            }
            if (this.RootControls.Count > 0)
            {
                throw new InvalidOperationException();
            }
            this.RootControls.Push(userControl);
            this.DoLocalize(culture);
        }

        private void LocalizeComponent(Component component, string componentName, CultureInfo culture)
        {
            this.ApplyResources(component, componentName, culture);
        }

        private void LocalizeRootControl(Control rootControl, CultureInfo culture)
        {
            Control control;
            UserControl control3;
            ToolStrip strip;
            Stack<Control> stack = new Stack<Control>();
            stack.Push(rootControl);
            while (stack.Count > 0)
            {
                control = stack.Pop();
                foreach (Control control2 in control.Controls)
                {
                    if (!((control2 is UserControl) || (control2.Controls.Count <= 0)))
                    {
                        stack.Push(control2);
                        control2.SuspendLayout();
                        this.SuspendedControls.Push(control2);
                    }
                }
            }
            Stack<System.Type> stack2 = new Stack<System.Type>();
            System.Type item = rootControl.GetType();
            while ((item != typeof(Form)) && (item != typeof(UserControl)))
            {
                stack2.Push(item);
                item = item.BaseType;
            }
            while (stack2.Count > 0)
            {
                item = stack2.Pop();
                this.BeforeLocalizeType(item);
                FieldInfo[] fields = item.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (FieldInfo info in fields)
                {
                    if (info.FieldType.IsSubclassOf(typeof(Component)))
                    {
                        Component component = (Component) info.GetValue(rootControl);
                        if (component != null)
                        {
                            control3 = component as UserControl;
                            if ((control3 != null) && !this.RootControls.Contains(control3))
                            {
                                this.RootControls.Push(control3);
                            }
                            strip = component as ToolStrip;
                            if (strip != null)
                            {
                                this.LocalizeToolStripContent(strip, culture);
                            }
                            this.LocalizeComponent(component, info.Name, culture);
                        }
                    }
                }
            }
            stack.Push(rootControl);
            while (stack.Count > 0)
            {
                control = stack.Pop();
                foreach (Control control2 in control.Controls)
                {
                    control3 = control2 as UserControl;
                    if (control3 != null)
                    {
                        if (!this.RootControls.Contains(control3))
                        {
                            this.RootControls.Push(control3);
                        }
                    }
                    else if (control2.Controls.Count > 0)
                    {
                        stack.Push(control2);
                    }
                    strip = control2 as ToolStrip;
                    if (strip != null)
                    {
                        this.LocalizeToolStripContent(strip, culture);
                    }
                    this.LocalizeComponent(control2, control2.Name, culture);
                }
            }
            if (rootControl is Form)
            {
                this.LocalizeComponent(rootControl, "$this", culture);
            }
            IUpdateCulture culture2 = rootControl as IUpdateCulture;
            if (culture2 != null)
            {
                culture2.UpdateCulture();
            }
        }

        private void LocalizeToolStripContent(ToolStrip toolStrip, CultureInfo culture)
        {
            foreach (ToolStripItem item in this.GetChildrenItems(toolStrip))
            {
                this.LocalizeComponent(item, item.Name, culture);
            }
        }

        [CompilerGenerated]
        private sealed class <GetChildrenItems>d__0 : IEnumerable<ToolStripItem>, IEnumerable, IEnumerator<ToolStripItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ToolStripItem <>2__current;
            public ToolStrip <>3__ts;
            public BasicFormLocalizer <>4__this;
            public IEnumerator <>7__wrap5;
            public IDisposable <>7__wrap6;
            private int <>l__initialThreadId;
            public ToolStripDropDownItem <NextDropDownItem>5__4;
            public ToolStripItem <NextItem>5__3;
            public ToolStrip <NextToolStrip>5__2;
            public Stack<ToolStrip> <ParentToolStrips>5__1;
            public ToolStrip ts;

            [DebuggerHidden]
            public <GetChildrenItems>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally7()
            {
                this.<>1__state = -1;
                this.<>7__wrap6 = this.<>7__wrap5 as IDisposable;
                if (this.<>7__wrap6 != null)
                {
                    this.<>7__wrap6.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<ParentToolStrips>5__1 = new Stack<ToolStrip>();
                            this.<ParentToolStrips>5__1.Push(this.ts);
                            while (this.<ParentToolStrips>5__1.Count > 0)
                            {
                                this.<NextToolStrip>5__2 = this.<ParentToolStrips>5__1.Pop();
                                this.<>7__wrap5 = this.<NextToolStrip>5__2.Items.GetEnumerator();
                                this.<>1__state = 1;
                                while (this.<>7__wrap5.MoveNext())
                                {
                                    this.<NextItem>5__3 = (ToolStripItem) this.<>7__wrap5.Current;
                                    this.<NextDropDownItem>5__4 = this.<NextItem>5__3 as ToolStripDropDownItem;
                                    if ((this.<NextDropDownItem>5__4 != null) && (this.<NextDropDownItem>5__4.DropDownItems.Count > 0))
                                    {
                                        this.<ParentToolStrips>5__1.Push(this.<NextDropDownItem>5__4.DropDown);
                                    }
                                    this.<>2__current = this.<NextItem>5__3;
                                    this.<>1__state = 2;
                                    return true;
                                Label_0101:
                                    this.<>1__state = 1;
                                }
                                this.<>m__Finally7();
                            }
                            break;

                        case 2:
                            goto Label_0101;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<ToolStripItem> IEnumerable<ToolStripItem>.GetEnumerator()
            {
                BasicFormLocalizer.<GetChildrenItems>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new BasicFormLocalizer.<GetChildrenItems>d__0(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__.ts = this.<>3__ts;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Windows.Forms.ToolStripItem>.GetEnumerator();
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
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally7();
                        }
                        break;
                }
            }

            ToolStripItem IEnumerator<ToolStripItem>.Current
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

