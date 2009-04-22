using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Commander
{
    public partial class CreateFolderForm : Form
    {
        public CreateFolderForm()
        {
            InitializeComponent();
        }

        
        public string DirectoryName
        {
            get
            {
                return comboBox.Text;
            }
            set
            {
                comboBox.Text = value;
            }
        }

        
        private void okButton_Click(object sender, EventArgs e)
        {
            comboBox.Items.Add(comboBox.Text);
        }

        private void comboBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        okButton_Click(this, null);
                        this.DialogResult = DialogResult.OK;
                        break;
                    }
                case Keys.Escape:
                    {
                        this.DialogResult = DialogResult.Cancel;
                        break;
                    }
            }
        }

        private void CreateFolderForm_Shown(object sender, EventArgs e)
        {
            comboBox.Focus();
        }
    }
}
