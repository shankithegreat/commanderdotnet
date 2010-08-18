using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Commander
{
    public partial class CommandBarBase2 : UserControl
    {
        public CommandBarBase2()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the text associated with this ComboBox control.
        /// </summary>
        [Category("Appearance")]
        public override string Text
        {
            get
            {
                return this.cmdComboBox.Text;
            }
            set
            {
                this.cmdComboBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the text associated with left label.
        /// </summary>
        [Category("Appearance")]
        public virtual string Title
        {
            get
            {
                return ((this.cmdLabel.Tag as string) ?? string.Empty);
            }
            set
            {
                this.cmdLabel.Text = string.Format("{0}>", value);
                this.cmdLabel.Tag = value;
            }
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

                foreach (string item in cmdComboBox.Items)
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
                    cmdComboBox.Items.Clear();
                    cmdComboBox.Items.AddRange(items);

                    OnLinesChanged(EventArgs.Empty);
                }
            }
        }


        /// <summary>
        /// Occurs when a key is pressed while the ComboBox control has focus.
        /// </summary>
        [Category("Key")]
        public event KeyEventHandler ComboBoxKeyDown
        {
            add
            {
                cmdComboBox.KeyDown += value;
            }
            remove
            {
                cmdComboBox.KeyDown -= value;
            }
        }

        /// <summary>
        /// Occurs when the Lines property value changes.
        /// </summary>
        public event EventHandler LinesChanged;

        /// <summary>
        /// Occurs when the Text property value changes.
        /// </summary>
        public new event EventHandler TextChanged
        {
            add
            {
                cmdComboBox.TextChanged += value;
            }
            remove
            {
                cmdComboBox.TextChanged -= value;
            }
        }


        public void StoryCurrentText()
        {
            string cmd = cmdComboBox.Text;

            cmdComboBox.Items.Remove(cmd);
            cmdComboBox.Items.Insert(0, cmd);

            OnLinesChanged(EventArgs.Empty);
        }


        protected virtual void OnLinesChanged(EventArgs args)
        {
            if (LinesChanged != null)
            {
                LinesChanged(this, args);
            }
        }
    }
}
