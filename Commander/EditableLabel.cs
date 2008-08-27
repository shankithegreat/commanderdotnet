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
            
            label.Text = textBox.Text;
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
                textBox.Text = label.Text;
                textBox.Focus();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            base.Height = 13;
        }
    }
}
