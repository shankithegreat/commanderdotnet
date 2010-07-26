using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Commander;
using TestForm.Messages;

namespace TestForm
{
    public partial class CommandBar : CommandBarBase
    {
        private string selectedDirectory;


        public CommandBar()
        {
            InitializeComponent();

            MessageDispatcher.Dispatcher.Subscribe(this);
        }


        private void Run(string fileName)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(fileName);
            psi.WorkingDirectory = selectedDirectory;
            System.Diagnostics.Process.Start(psi);
        }

        private void CommandBar_ComboBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string cmd = this.Text.Trim();

                if (!string.IsNullOrEmpty(cmd))
                {
                    // cd ...
                    if (cmd.StartsWith("cd ", StringComparison.InvariantCultureIgnoreCase))
                    {
                        int index = cmd.IndexOf("cd ", StringComparison.InvariantCultureIgnoreCase);
                        string path = cmd.Substring(index + 3);


                        if (!Path.IsPathRooted(path))
                        {
                            path = Path.Combine(selectedDirectory, path);
                        }
                        else
                        {
                            if (path.EndsWith(":"))
                            {
                                path = path.ToUpper() + Path.DirectorySeparatorChar;
                            }
                        }

                        try
                        {
                            if (Directory.Exists(path))
                            {
                                MessageDispatcher.Dispatcher.Invoke(new DirectorySelectedAttribute(), new DirectorySelectedArgs(path));
                                this.StoryCurrentText();
                            }

                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show(exp.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {

                        try
                        {
                            Run(cmd);
                            this.StoryCurrentText();
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show(exp.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    this.Text = string.Empty;
                }
            }
        }

        [DirectorySelected]
        private void dispatcher_DirectorySelected(DirectorySelectedArgs e)
        {
            selectedDirectory = e.SelectedDirectory;
        }
    }
}
