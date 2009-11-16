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

        private void label_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        private void label_MouseUp(object sender, MouseEventArgs e)
        {
            OnMouseUp(e);
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
    }

    public class BeforeEditEventArgs
    {
        public BeforeEditEventArgs(string text)
        {
            this.Text = text;
        }

        public string Text { get; set; }
    }

    public class AfterEditEventArgs
    {
        public AfterEditEventArgs(string text)
        {
            this.Text = text;
        }

        public string Text { get; set; }

        public bool Cancel { get; set; }
    }

    public delegate void BeforeEditEventHandler(object sender, BeforeEditEventArgs e);

    public delegate void AfterEditEventHandler(object sender, AfterEditEventArgs e);

}
