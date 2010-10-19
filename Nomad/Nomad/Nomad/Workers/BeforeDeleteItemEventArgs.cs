namespace Nomad.Workers
{
    using Nomad.FileSystem.Virtual;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class BeforeDeleteItemEventArgs : VirtualItemEventArgs
    {
        private DialogResult FAction;

        public BeforeDeleteItemEventArgs(IVirtualItem item) : base(item)
        {
            this.FAction = DialogResult.Yes;
        }

        public DialogResult Action
        {
            get
            {
                return this.FAction;
            }
            set
            {
                switch (value)
                {
                    case DialogResult.Yes:
                    case DialogResult.No:
                    case DialogResult.Cancel:
                        this.FAction = value;
                        return;
                }
                throw new InvalidEnumArgumentException();
            }
        }
    }
}

