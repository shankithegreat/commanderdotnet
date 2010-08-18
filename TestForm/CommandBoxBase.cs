using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestForm
{
    public partial class CommandBoxBase : ComboBox
    {
        private string selectedDirectory;


        public CommandBoxBase()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets an object representing the collection of the items contained in this ComboBox.
        /// </summary>
        [Category("Data")]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [MergableProperty(false)]
        public string Lines
        {
            get
            {
                StringBuilder result = new StringBuilder();

                foreach (string item in this.Items)
                {
                    if (result.Length > 0)
                    {
                        result.Append("\r\n");
                    }

                    result.Append(item);
                }

                return result.ToString();
            }
            set
            {
                if (this.Lines != value)
                {
                    string[] items = value.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    this.Items.Clear();
                    this.Items.AddRange(items);

                    OnLinesChanged(EventArgs.Empty);
                }
            }
        }


        /// <summary>
        /// Occurs when the Lines property value changes.
        /// </summary>
        public event EventHandler LinesChanged;

        public event CdEventHandler CdCommand;


        protected virtual void OnLinesChanged(EventArgs args)
        {
            if (LinesChanged != null)
            {
                LinesChanged(this, args);
            }
        }

        protected virtual void OnCdCommand(string path)
        {
            if (CdCommand != null)
            {
                CdCommand(this, path);
            }
        }

        protected void StoryCurrentText()
        {
            string cmd = this.Text;

            this.Items.Remove(cmd);
            this.Items.Insert(0, cmd);

            OnLinesChanged(EventArgs.Empty);
        }

        protected void Run(string fileName)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(fileName);
            psi.WorkingDirectory = selectedDirectory;
            System.Diagnostics.Process.Start(psi);
        }


        private void CommandBoxBase_KeyDown(object sender, KeyEventArgs e)
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
                                OnCdCommand(path);
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
    }


    public delegate void CdEventHandler(object sender, string path);
}
