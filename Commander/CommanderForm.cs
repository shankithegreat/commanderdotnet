using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ShellDll;

namespace Commander
{
    public partial class CommanderForm : Form
    {
        private CreateFolderForm createFolderForm = new CreateFolderForm();

        
        public CommanderForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }


        private bool CreateFolder(DirectoryInfo directory, string localPath)
        {
            if (string.IsNullOrEmpty(localPath))
            {
                return false;
            }

            try
            {
                directory.CreateSubdirectory(localPath);
                return true;
            }
            catch (Exception e)
            {
                ShowErrorMessage(e);
                return false;
            }
        }

        private void AppendCmd(string text)
        {
            cmdControl.Text += text;
        }

        private void Run(string fileName)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(fileName);
            psi.WorkingDirectory = mainView.CurrentDirectory.FullName;
            System.Diagnostics.Process.Start(psi);
        }

        private void Run(string fileName, string arguments)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(fileName, arguments);
            psi.WorkingDirectory = mainView.CurrentDirectory.FullName;
            System.Diagnostics.Process.Start(psi);
        }

        private void ShowErrorMessage(Exception e)
        {
            ShowErrorMessage(e.Message);
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void toolStrip_ButtonClick(string path, string[] args)
        {
            Run(path, Utility.StringHelper.ToArguments(args));
        }

        private void mainView_DirectorySelected(object sender, DirectoryInfo directory)
        {
            cmdControl.Title = directory.FullName;
        }

        private void fileView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && !e.Alt && !e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        {
                            FileSystemInfo fsi = mainView.GetFocusedItem();
                            AppendCmd(fsi.Name);
                            e.SuppressKeyPress = true;
                            break;
                        }
                    case Keys.P:
                        {
                            FileSystemInfo fsi = mainView.GetFocusedItem();
                            AppendCmd(ShellFolder.GetParentDirectoryPath(fsi));
                            break;
                        }
                }
            }
        }

        private void cmdControl_ComboBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string cmd = cmdControl.Text.Trim();

                if (!string.IsNullOrEmpty(cmd))
                {
                    if (cmd.StartsWith("cd ", StringComparison.InvariantCultureIgnoreCase))
                    {
                        int index = cmd.IndexOf("cd ", StringComparison.InvariantCultureIgnoreCase);
                        string path = cmd.Substring(index + 3);


                        if (!Path.IsPathRooted(path))
                        {
                            DirectoryInfo directory = mainView.CurrentDirectory;
                            path = Path.Combine(directory.FullName, path);
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
                            DirectoryInfo newDirectory = new DirectoryInfo(path);
                            if (newDirectory.Exists)
                            {
                                mainView.CurrentDirectory = newDirectory;
                                cmdControl.StoryCurrentText();
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
                            cmdControl.StoryCurrentText();
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show(exp.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    cmdControl.Text = string.Empty;
                }
            }
        }

        private void editMenuItem_Click(object sender, EventArgs e)
        {
            FileSystemInfo fsi = mainView.GetFocusedItem();
            if (fsi is FileInfo)
            {
                Run("notepad", string.Format("\"{0}\"", fsi.FullName));
            }
        }

        private void copyMenuItem_Click(object sender, EventArgs e)
        {
            mainView.Copy();
        }

        private void moveMenuItem_Click(object sender, EventArgs e)
        {
            mainView.MoveItem();
        }

        private void createFolderMenuItem_Click(object sender, EventArgs e)
        {
            if (createFolderForm.ShowDialog() == DialogResult.OK)
            {
                if (CreateFolder(mainView.CurrentDirectory, createFolderForm.DirectoryName))
                {
                    mainView.Refresh();
                }
            }
        }

        private void deleteMenuItem_Click(object sender, EventArgs e)
        {
            mainView.Delete();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CommanderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
