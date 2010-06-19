using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestForm.Messages;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            view.SelectedNode = new DirectoryNode(null, new DirectoryInfo(@"C:\"));

            MessageDispatcher.Dispatcher.Subscribe(this);                        
        }


        protected void OnSelectedDirectory(string directory)
        {
            MessageDispatcher.Dispatcher.Invoke(new DirectorySelectedAttribute(), new DirectorySelectedArgs(directory));
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    {
                        if (Directory.Exists(textBox.Text))
                        {
                            OnSelectedDirectory(textBox.Text);                            
                        }

                        return true;
                    }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
