namespace Nomad.Controls
{
    using System;
    using System.Windows.Forms;

    public class BeforeLabelEditEventArgs : LabelEditEventArgs
    {
        private string FLabel;
        private int FMaxLength;
        private int FSelLength;
        private int FSelStart;

        public BeforeLabelEditEventArgs(int item) : base(item)
        {
            this.FSelLength = -1;
        }

        public BeforeLabelEditEventArgs(int item, string label) : base(item, label)
        {
            this.FSelLength = -1;
        }

        public string Label
        {
            get
            {
                return ((this.FLabel != null) ? this.FLabel : base.Label);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this.FLabel = value;
            }
        }

        public int MaxLength
        {
            get
            {
                return this.FMaxLength;
            }
            set
            {
                this.FMaxLength = (value < 0) ? 0 : value;
            }
        }

        public int SelectionLength
        {
            get
            {
                return ((this.FSelLength < 0) ? this.Label.Length : this.FSelLength);
            }
            set
            {
                if (value < 0)
                {
                    this.FSelLength = 0;
                }
                else if (value >= this.Label.Length)
                {
                    this.FSelLength = -1;
                }
                else
                {
                    this.FSelLength = value;
                }
            }
        }

        public int SelectionStart
        {
            get
            {
                return this.FSelStart;
            }
            set
            {
                this.FSelStart = (value < 0) ? 0 : value;
            }
        }
    }
}

