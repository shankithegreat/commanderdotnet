namespace Nomad.Commons.Controls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class PreviewValueEventArgs<T> : CancelEventArgs
    {
        internal T _Value;

        internal PreviewValueEventArgs(Control target)
        {
            this.Target = target;
        }

        public PreviewValueEventArgs(Control target, T value)
        {
            this.Target = target;
            this._Value = value;
        }

        public Control Target
        {
            [CompilerGenerated]
            get
            {
                return this.<Target>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Target>k__BackingField = value;
            }
        }

        public T Value
        {
            get
            {
                return this._Value;
            }
        }
    }
}

