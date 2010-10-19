namespace Microsoft.Win32
{
    using System;

    public sealed class ThreadExecutionStateLock : IDisposable
    {
        public ThreadExecutionStateLock(bool systemRequired, bool displayRequired)
        {
            EXECUTION_STATE esFlags = -2147483648;
            if (systemRequired)
            {
                esFlags |= EXECUTION_STATE.ES_SYSTEM_REQUIRED;
            }
            if (displayRequired)
            {
                esFlags |= EXECUTION_STATE.ES_DISPLAY_REQUIRED;
            }
            Windows.SetThreadExecutionState(esFlags);
            if (esFlags == ((EXECUTION_STATE) (-2147483648)))
            {
                GC.SuppressFinalize(this);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            Windows.SetThreadExecutionState(-2147483648);
        }

        ~ThreadExecutionStateLock()
        {
            this.Dispose(false);
        }
    }
}

