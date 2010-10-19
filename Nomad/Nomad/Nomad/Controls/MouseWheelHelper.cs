namespace Nomad.Controls
{
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    [ProvideProperty("FixMouseWheel", typeof(Control)), DesignerCategory("Code")]
    public class MouseWheelHelper : Component, IExtenderProvider
    {
        private Dictionary<Control, MouseWheelFix> ControlFixMap = new Dictionary<Control, MouseWheelFix>();

        public void ApplyToChildren(Control parent, Predicate<Control> canExtend)
        {
            if (parent == null)
            {
                throw new ArgumentNullException();
            }
            foreach (Control control in GetChildren(parent))
            {
                if (canExtend(control))
                {
                    this.SetFixMouseWheel(control, true);
                }
            }
        }

        public void ApplyToChildren(Control parent, bool value)
        {
            if (parent == null)
            {
                throw new ArgumentNullException();
            }
            foreach (Control control in GetChildren(parent))
            {
                if (!(value && !this.CanExtend(control)))
                {
                    this.SetFixMouseWheel(control, value);
                }
            }
        }

        public bool CanExtend(object extendee)
        {
            return (((((extendee is TextBoxBase) || (extendee is ListControl)) || (((extendee is ContainerControl) && !(extendee is SplitContainer)) && (!(extendee is ToolStripContainer) && !(extendee is ToolStripPanel)))) || (((extendee is Panel) && !(extendee is ToolStripContentPanel)) || ((extendee is ListView) || (extendee is TreeView)))) || (extendee is TrackBar));
        }

        private void Control_Disposed(object sender, EventArgs e)
        {
            this.RemoveControl((Control) sender);
        }

        protected override void Dispose(bool disposing)
        {
            if (this.ControlFixMap != null)
            {
                foreach (KeyValuePair<Control, MouseWheelFix> pair in this.ControlFixMap)
                {
                    pair.Key.Disposed -= new EventHandler(this.Control_Disposed);
                    pair.Value.Dispose();
                }
                this.ControlFixMap = null;
            }
            base.Dispose(disposing);
        }

        private static IEnumerable<Control> GetChildren(Control parent)
        {
            return new <GetChildren>d__0(-2) { <>3__parent = parent };
        }

        [DefaultValue(false)]
        public bool GetFixMouseWheel(Control control)
        {
            if (this.ControlFixMap == null)
            {
                throw new ObjectDisposedException("MouseWheelHelper");
            }
            return this.ControlFixMap.ContainsKey(control);
        }

        private void RemoveControl(Control control)
        {
            MouseWheelFix fix;
            if (this.ControlFixMap.TryGetValue(control, out fix))
            {
                control.Disposed -= new EventHandler(this.Control_Disposed);
                fix.Dispose();
                this.ControlFixMap.Remove(control);
            }
        }

        public void SetFixMouseWheel(Control control, bool value)
        {
            if (this.ControlFixMap == null)
            {
                throw new ObjectDisposedException("MouseWheelHelper");
            }
            if (!this.ControlFixMap.ContainsKey(control) || !value)
            {
                this.RemoveControl(control);
                if (value)
                {
                    this.ControlFixMap.Add(control, base.DesignMode ? new MouseWheelFix() : new MouseWheelFix(control));
                    control.Disposed += new EventHandler(this.Control_Disposed);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetChildren>d__0 : IEnumerable<Control>, IEnumerable, IEnumerator<Control>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private Control <>2__current;
            public Control <>3__parent;
            public IEnumerator <>7__wrap3;
            public IDisposable <>7__wrap4;
            private int <>l__initialThreadId;
            public Stack<Control> <Containers>5__1;
            public Control <NextControl>5__2;
            public Control parent;

            [DebuggerHidden]
            public <GetChildren>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally5()
            {
                this.<>1__state = -1;
                this.<>7__wrap4 = this.<>7__wrap3 as IDisposable;
                if (this.<>7__wrap4 != null)
                {
                    this.<>7__wrap4.Dispose();
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
                            this.<Containers>5__1 = new Stack<Control>();
                            this.<Containers>5__1.Push(this.parent);
                            while (this.<Containers>5__1.Count > 0)
                            {
                                this.<>7__wrap3 = this.<Containers>5__1.Pop().Controls.GetEnumerator();
                                this.<>1__state = 1;
                                while (this.<>7__wrap3.MoveNext())
                                {
                                    this.<NextControl>5__2 = (Control) this.<>7__wrap3.Current;
                                    if (this.<NextControl>5__2.Controls.Count > 0)
                                    {
                                        this.<Containers>5__1.Push(this.<NextControl>5__2);
                                    }
                                    this.<>2__current = this.<NextControl>5__2;
                                    this.<>1__state = 2;
                                    return true;
                                Label_00D0:
                                    this.<>1__state = 1;
                                }
                                this.<>m__Finally5();
                            }
                            break;

                        case 2:
                            goto Label_00D0;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<Control> IEnumerable<Control>.GetEnumerator()
            {
                MouseWheelHelper.<GetChildren>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new MouseWheelHelper.<GetChildren>d__0(0);
                }
                d__.parent = this.<>3__parent;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Windows.Forms.Control>.GetEnumerator();
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
                            this.<>m__Finally5();
                        }
                        break;
                }
            }

            Control IEnumerator<Control>.Current
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

        private class MouseWheelFix : NativeWindow, IDisposable
        {
            private bool IsInMouseWheel;
            private Control Owner;

            public MouseWheelFix()
            {
            }

            public MouseWheelFix(Control control)
            {
                this.Owner = control;
                this.Owner.HandleCreated += new EventHandler(this.Control_HandleCreated);
                if (control.IsHandleCreated)
                {
                    base.AssignHandle(control.Handle);
                }
            }

            private void Control_HandleCreated(object sender, EventArgs e)
            {
                base.AssignHandle(((Control) sender).Handle);
            }

            public void Dispose()
            {
                if (this.Owner != null)
                {
                    this.Owner.HandleCreated -= new EventHandler(this.Control_HandleCreated);
                    this.Owner = null;
                }
                this.ReleaseHandle();
            }

            private IntPtr GetWheelHandle(ref Message m)
            {
                ComboBox owner = this.Owner as ComboBox;
                if ((owner == null) || !owner.DroppedDown)
                {
                    Point point = new Point((short) ((int) m.LParam), ((int) m.LParam) >> 0x10);
                    if ((this.Owner is ScrollableControl) && !(this.Owner is UpDownBase))
                    {
                        IntPtr handle = Windows.WindowFromPoint(point);
                        if (handle != IntPtr.Zero)
                        {
                            Control control = Control.FromChildHandle(handle);
                            if (((control != null) && control.IsHandleCreated) && control.Enabled)
                            {
                                return control.Handle;
                            }
                        }
                    }
                    if (((this.Owner.Parent != null) && this.Owner.Parent.IsHandleCreated) && (!this.Owner.Enabled || !this.Owner.ClientRectangle.Contains(this.Owner.PointToClient(point))))
                    {
                        return this.Owner.Parent.Handle;
                    }
                }
                return IntPtr.Zero;
            }

            protected override void WndProc(ref Message m)
            {
                if ((m.Msg == 0x20a) && !this.IsInMouseWheel)
                {
                    this.IsInMouseWheel = true;
                    try
                    {
                        IntPtr wheelHandle = this.GetWheelHandle(ref m);
                        if (wheelHandle != IntPtr.Zero)
                        {
                            Windows.SendMessage(wheelHandle, m.Msg, m.WParam, m.LParam);
                            m.Result = (IntPtr) 1;
                            return;
                        }
                    }
                    finally
                    {
                        this.IsInMouseWheel = false;
                    }
                }
                base.WndProc(ref m);
            }
        }
    }
}

