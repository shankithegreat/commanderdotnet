using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Commander
{
    public class BeforeEditEventArgs
    {
        private string text;

        public BeforeEditEventArgs(string text)
        {
            this.text = text;
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
    }
    public class AfterEditEventArgs
    {
        private string text;
        private bool cancel = false;

        public AfterEditEventArgs(string text)
        {
            this.text = text;
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        public bool Cancel
        {
            get
            {
                return cancel;
            }
            set
            {
                cancel = value;
            }
        }
    }
    public delegate void BeforeEditEventHandler(object sender, BeforeEditEventArgs e);
    public delegate void AfterEditEventHandler(object sender, AfterEditEventArgs e);

    public partial class EditableLabel : UserControl
    {
        public EditableLabel()
        {            
            InitializeComponent();
        }

        public event BeforeEditEventHandler BeforeEdit;
        public event AfterEditEventHandler AfterEdit;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return label.Text;
            }
            set
            {
                if (textBox.Visible)
                {
                    textBox.Text = value;
                }
                label.Text = value;
            }
        }

        [Category("Appearance")]
        public Color TextBoxBackColor
        {
            get
            {
                return textBox.BackColor;
            }
            set
            {
                textBox.BackColor = value;
            }
        }

        private void HideTextBox()
        {
            textBox.Visible = false;
            AfterEditEventArgs args = new AfterEditEventArgs(textBox.Text);
            OnAfterEdit(args);
            if (!args.Cancel)
            {
                label.Text = args.Text;
            }            
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        HideTextBox();
                        break;
                    }
                case Keys.Escape:
                    {
                        textBox.Visible = false;
                        break;
                    }
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            textBox.Visible = false;
        }

        private void label_DoubleClick(object sender, EventArgs e)
        {
            textBox.Visible = true;
        }

        private void textBox_VisibleChanged(object sender, EventArgs e)
        {
            if (textBox.Visible)
            {
                BeforeEditEventArgs args = new BeforeEditEventArgs(label.Text);
                OnBeforeEdit(args);
                textBox.Text = args.Text;
                textBox.Focus();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            base.Height = 13;
        }

        protected virtual void OnBeforeEdit(BeforeEditEventArgs e)
        {
            if (BeforeEdit != null)
            {
                BeforeEdit(this, e);
            }
        }

        protected virtual void OnAfterEdit(AfterEditEventArgs e)
        {
            if (AfterEdit != null)
            {
                AfterEdit(this, e);
            }
        }
    }
}
