namespace Nomad.Controls
{
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class FormEx : Form
    {
        private static readonly object EventThemeChanged = new object();
        private static readonly object EventWindowStateChanging = new object();

        public event EventHandler ThemeChanged
        {
            add
            {
                base.Events.AddHandler(EventThemeChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventThemeChanged, value);
            }
        }

        public event EventHandler<WindowStateChangingEventArgs> WindowStateChanging
        {
            add
            {
                base.Events.AddHandler(EventWindowStateChanging, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventWindowStateChanging, value);
            }
        }

        protected IEnumerable<T> FindAllChildren<T>() where T: Control
        {
            return new <FindAllChildren>d__0<T>(-2) { <>4__this = this };
        }

        protected virtual void OnThemeChanged(EventArgs e)
        {
            EventHandler handler = base.Events[EventThemeChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnWindowStateChanging(WindowStateChangingEventArgs e)
        {
            EventHandler<WindowStateChangingEventArgs> handler = base.Events[EventWindowStateChanging] as EventHandler<WindowStateChangingEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void WndProc(ref Message m)
        {
            FormWindowState? nullable;
            switch (m.Msg)
            {
                case 0x112:
                {
                    SC sc = ((SC) ((int) m.WParam)) & SC.SC_MASK;
                    nullable = null;
                    SC sc2 = sc;
                    if (sc2 != SC.SC_MINIMIZE)
                    {
                        if (sc2 == SC.SC_MAXIMIZE)
                        {
                            nullable = new FormWindowState?(FormWindowState.Maximized);
                            break;
                        }
                        if (sc2 == SC.SC_RESTORE)
                        {
                            nullable = new FormWindowState?(FormWindowState.Normal);
                            break;
                        }
                    }
                    else
                    {
                        nullable = new FormWindowState?(FormWindowState.Minimized);
                    }
                    break;
                }
                case 0x31a:
                    this.OnThemeChanged(EventArgs.Empty);
                    goto Label_00C9;

                default:
                    goto Label_00C9;
            }
            if (nullable.HasValue)
            {
                WindowStateChangingEventArgs e = new WindowStateChangingEventArgs(base.WindowState, nullable.Value);
                this.OnWindowStateChanging(e);
                if (e.Cancel)
                {
                    return;
                }
            }
        Label_00C9:
            base.WndProc(ref m);
        }

        [CompilerGenerated]
        private sealed class <FindAllChildren>d__0<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable where T: Control
        {
            private int <>1__state;
            private T <>2__current;
            public FormEx <>4__this;
            public IEnumerator <>7__wrap5;
            public IDisposable <>7__wrap6;
            private int <>l__initialThreadId;
            public Stack<Control> <Hierrarhy>5__1;
            public Control <NextControl>5__3;
            public T <NextResult>5__4;
            public Control <ParentControl>5__2;

            [DebuggerHidden]
            public <FindAllChildren>d__0(int <>1__state)
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
                            this.<Hierrarhy>5__1 = new Stack<Control>();
                            this.<Hierrarhy>5__1.Push(this.<>4__this);
                            while (this.<Hierrarhy>5__1.Count > 0)
                            {
                                this.<ParentControl>5__2 = this.<Hierrarhy>5__1.Pop();
                                this.<>7__wrap5 = this.<ParentControl>5__2.Controls.GetEnumerator();
                                this.<>1__state = 1;
                                while (this.<>7__wrap5.MoveNext())
                                {
                                    this.<NextControl>5__3 = (Control) this.<>7__wrap5.Current;
                                    this.<NextResult>5__4 = this.<NextControl>5__3 as T;
                                    if (this.<NextResult>5__4 == null)
                                    {
                                        goto Label_00E3;
                                    }
                                    this.<>2__current = this.<NextResult>5__4;
                                    this.<>1__state = 2;
                                    return true;
                                Label_00DC:
                                    this.<>1__state = 1;
                                Label_00E3:
                                    if (this.<NextControl>5__3.Controls.Count > 0)
                                    {
                                        this.<Hierrarhy>5__1.Push(this.<NextControl>5__3);
                                    }
                                }
                                this.<>m__Finally7();
                            }
                            break;

                        case 2:
                            goto Label_00DC;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return (FormEx.<FindAllChildren>d__0<T>) this;
                }
                return new FormEx.<FindAllChildren>d__0<T>(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
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

            T IEnumerator<T>.Current
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

