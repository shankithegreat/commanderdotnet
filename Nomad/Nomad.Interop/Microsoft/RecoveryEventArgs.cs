namespace Microsoft
{
    using Microsoft.Win32;
    using System;
    using System.Runtime.CompilerServices;

    public class RecoveryEventArgs : EventArgs
    {
        private bool FCancelled;

        public bool RecoveryInProgress()
        {
            return (Windows.ApplicationRecoveryInProgress(out this.FCancelled) == 0);
        }

        public bool Finished { get; set; }

        public bool UserCancelled
        {
            get
            {
                return this.FCancelled;
            }
        }
    }
}

