namespace Nomad.Commons.Controls
{
    using Microsoft;
    using Microsoft.Shell;
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class TaskbarProgress : IDisposable
    {
        private Form FForm;
        private long FMaxValue = 100L;
        private TaskbarProgressState FState;
        private long FValue;
        private ITaskbarList3 TaskBar;

        private TaskbarProgress(Form form, ITaskbarList3 taskBar)
        {
            this.FForm = form;
            this.FForm.HandleCreated += new EventHandler(this.Form_HandleCreated);
            this.TaskBar = taskBar;
        }

        public static TaskbarProgress Create(Form form)
        {
            if (!IsSupported)
            {
                throw new PlatformNotSupportedException();
            }
            if (form == null)
            {
                throw new ArgumentNullException();
            }
            ITaskbarList3 taskBar = (ITaskbarList3) new CoTaskbarList();
            taskBar.HrInit();
            return new TaskbarProgress(form, taskBar);
        }

        public void Dispose()
        {
            if (this.TaskBar != null)
            {
                Marshal.ReleaseComObject(this.TaskBar);
            }
            this.TaskBar = null;
            if (this.FForm != null)
            {
                this.FForm.HandleCreated -= new EventHandler(this.Form_HandleCreated);
            }
            this.FForm = null;
        }

        private void Form_HandleCreated(object sender, EventArgs e)
        {
            if (this.FState != TaskbarProgressState.NoProgress)
            {
                this.TaskBar.SetProgressValue(this.FForm.Handle, (ulong) this.FValue, (ulong) this.FMaxValue);
                if (this.FState != TaskbarProgressState.Normal)
                {
                    this.TaskBar.SetProgressState(this.FForm.Handle, (TBPF) this.FState);
                }
            }
        }

        public static TaskbarProgress TryCreate(Form form)
        {
            if (form == null)
            {
                throw new ArgumentNullException();
            }
            if (IsSupported)
            {
                object o = new CoTaskbarList();
                try
                {
                    ITaskbarList3 taskBar = o as ITaskbarList3;
                    if (taskBar != null)
                    {
                        taskBar.HrInit();
                        return new TaskbarProgress(form, taskBar);
                    }
                }
                catch
                {
                    Marshal.ReleaseComObject(o);
                }
            }
            return null;
        }

        public static bool IsSupported
        {
            get
            {
                return OS.IsWin7;
            }
        }

        public long Maximum
        {
            get
            {
                return this.FMaxValue;
            }
            set
            {
                if ((this.TaskBar == null) || (this.FForm == null))
                {
                    throw new ObjectDisposedException("TaskbarProgress");
                }
                if (value < 0L)
                {
                    throw new ArgumentException();
                }
                if (this.FMaxValue != value)
                {
                    this.FMaxValue = value;
                    if (this.FForm.IsHandleCreated)
                    {
                        this.TaskBar.SetProgressValue(this.FForm.Handle, (ulong) this.FValue, (ulong) this.FMaxValue);
                    }
                }
                if (this.FState == TaskbarProgressState.Marquee)
                {
                    this.FState = TaskbarProgressState.Normal;
                }
            }
        }

        public TaskbarProgressState State
        {
            get
            {
                return this.FState;
            }
            set
            {
                if ((this.TaskBar == null) || (this.FForm == null))
                {
                    throw new ObjectDisposedException("TaskbarProgress");
                }
                if (!Enum.IsDefined(typeof(TaskbarProgressState), value))
                {
                    throw new InvalidEnumArgumentException();
                }
                if (this.FState != value)
                {
                    this.FState = value;
                    if (this.FForm.IsHandleCreated)
                    {
                        this.TaskBar.SetProgressState(this.FForm.Handle, (TBPF) this.FState);
                    }
                }
            }
        }

        public long Value
        {
            get
            {
                return this.FValue;
            }
            set
            {
                if ((this.TaskBar == null) || (this.FForm == null))
                {
                    throw new ObjectDisposedException("TaskbarProgress");
                }
                if ((value < 0L) || (value > this.Maximum))
                {
                    throw new ArgumentException();
                }
                if (this.FValue != value)
                {
                    this.FValue = value;
                    if (this.FForm.IsHandleCreated)
                    {
                        this.TaskBar.SetProgressValue(this.FForm.Handle, (ulong) this.FValue, (ulong) this.FMaxValue);
                    }
                }
                if (this.FState == TaskbarProgressState.Marquee)
                {
                    this.FState = TaskbarProgressState.Normal;
                }
            }
        }
    }
}

