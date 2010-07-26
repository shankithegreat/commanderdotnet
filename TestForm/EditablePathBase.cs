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
    public partial class EditablePathBase : UserControl
    {
        public EditablePathBase()
        {
            InitializeComponent();

            this.Text = @"c:\";
        }


        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text { get { return textBox.Text; } set { textBox.Text = value; } }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    {

                        return true;
                    }
                case Keys.Escape:
                    {

                        return true;
                    }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        /*private void EditablePathBase_DoubleClick(object sender, EventArgs e)
        {
            textBox.ReadOnly = false;
        }*/
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
