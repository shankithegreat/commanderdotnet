namespace Nomad.Commons.Controls
{
    using Microsoft.Win32;
    using System;
    using System.Windows.Forms;

    public class WaitCursor : IDisposable
    {
        private Control FControl;

        public WaitCursor()
        {
            Show();
        }

        public WaitCursor(Control control)
        {
            Show(control);
            this.FControl = control;
        }

        private static void Application_Idle(object sender, EventArgs e)
        {
            Application.Idle -= new EventHandler(WaitCursor.Application_Idle);
            Hide();
        }

        public void Dispose()
        {
            if (this.FControl != null)
            {
                Hide(this.FControl);
            }
            else
            {
                Hide();
            }
        }

        public static void Hide()
        {
            Application.UseWaitCursor = false;
            UpdateCursor(Form.ActiveForm);
        }

        public static void Hide(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException();
            }
            control.UseWaitCursor = false;
            UpdateCursor(control);
        }

        public static void Show()
        {
            Application.UseWaitCursor = true;
            UpdateCursor(Form.ActiveForm);
        }

        public static void Show(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException();
            }
            control.UseWaitCursor = true;
            UpdateCursor(control);
        }

        public static void ShowUntilIdle()
        {
            Show();
            Application.Idle += new EventHandler(WaitCursor.Application_Idle);
        }

        private static void UpdateCursor(Control control)
        {
            if (!(((control == null) || !control.IsHandleCreated) || control.InvokeRequired))
            {
                Windows.SendMessage(control.Handle, 0x20, control.Handle, (IntPtr) 1);
            }
        }
    }
}

