namespace Nomad.Commons
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ControlTimer : NativeWindow, IDisposable
    {
        private Dictionary<int, int> ActiveTimerMap;
        private Control Parent;

        public event EventHandler<TimerEventArgs> Tick;

        public ControlTimer(Control parent)
        {
            this.Parent = parent;
            this.Parent.Disposed += new EventHandler(this.Control_Disposed);
        }

        private void Control_Disposed(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Control_HandleCreated(object sender, EventArgs e)
        {
            base.AssignHandle(this.Parent.Handle);
            foreach (KeyValuePair<int, int> pair in this.ActiveTimerMap)
            {
                this.Start(pair.Key, pair.Value);
            }
        }

        private void Control_HandleDestroyed(object sender, EventArgs e)
        {
            this.ReleaseHandle();
        }

        private void Detach()
        {
            this.Parent.HandleCreated -= new EventHandler(this.Control_HandleCreated);
            this.Parent.HandleDestroyed -= new EventHandler(this.Control_HandleDestroyed);
            this.ReleaseHandle();
        }

        public void Dispose()
        {
            if (this.Parent != null)
            {
                this.Parent.Disposed -= new EventHandler(this.Control_Disposed);
                this.Detach();
                this.Parent = null;
            }
            this.ActiveTimerMap = null;
        }

        protected void OnTick(TimerEventArgs e)
        {
            if (this.Tick != null)
            {
                this.Tick(this, e);
            }
        }

        public int Start(int interval)
        {
            int key = 0;
            while ((this.ActiveTimerMap != null) && this.ActiveTimerMap.ContainsKey(key))
            {
                key++;
            }
            return this.Start(key, interval);
        }

        public int Start(int id, int interval)
        {
            if (this.Parent == null)
            {
                throw new ObjectDisposedException("ControlTimer");
            }
            if (Windows.SetTimer(this.Parent.Handle, (IntPtr) id, interval, IntPtr.Zero) != IntPtr.Zero)
            {
                if (this.ActiveTimerMap == null)
                {
                    this.ActiveTimerMap = new Dictionary<int, int>();
                }
                if (this.ActiveTimerMap.Count == 0)
                {
                    this.Parent.HandleCreated += new EventHandler(this.Control_HandleCreated);
                    this.Parent.HandleDestroyed += new EventHandler(this.Control_HandleDestroyed);
                    base.AssignHandle(this.Parent.Handle);
                }
                this.ActiveTimerMap[id] = interval;
                return id;
            }
            return -1;
        }

        public bool Stop(int id)
        {
            if (this.Parent == null)
            {
                throw new ObjectDisposedException("ControlTimer");
            }
            if (this.ActiveTimerMap == null)
            {
                return false;
            }
            bool flag = Windows.KillTimer(base.Handle, (IntPtr) id);
            this.ActiveTimerMap.Remove(id);
            if (this.ActiveTimerMap.Count == 0)
            {
                this.Detach();
            }
            return flag;
        }

        public void StopAll()
        {
            if (this.Parent == null)
            {
                throw new ObjectDisposedException("ControlTimer");
            }
            if (this.ActiveTimerMap != null)
            {
                foreach (int num in this.ActiveTimerMap.Keys)
                {
                    Windows.KillTimer(base.Handle, (IntPtr) num);
                }
                this.ActiveTimerMap.Clear();
                this.Detach();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x113)
            {
                int wParam = (int) m.WParam;
                if ((this.ActiveTimerMap != null) && this.ActiveTimerMap.ContainsKey(wParam))
                {
                    TimerEventArgs e = new TimerEventArgs(wParam);
                    this.OnTick(e);
                    if (e.Cancel)
                    {
                        this.Stop(wParam);
                    }
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}

