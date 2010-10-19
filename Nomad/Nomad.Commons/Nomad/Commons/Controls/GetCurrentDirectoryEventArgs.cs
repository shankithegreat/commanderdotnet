namespace Nomad.Commons.Controls
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GetCurrentDirectoryEventArgs : EventArgs
    {
        public GetCurrentDirectoryEventArgs(Control target, string currentDir)
        {
            this.Target = target;
            this.CurrentDirectory = currentDir;
        }

        public string CurrentDirectory { get; set; }

        public Control Target { get; private set; }
    }
}

