namespace Nomad.Commons.Controls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GetCustomSourceEventArgs : HandledEventArgs
    {
        public GetCustomSourceEventArgs(Control target, string text)
        {
            this.Target = target;
            this.Text = text;
        }

        public IEnumerable CustomSource { get; set; }

        public Control Target { get; private set; }

        public string Text { get; private set; }
    }
}

