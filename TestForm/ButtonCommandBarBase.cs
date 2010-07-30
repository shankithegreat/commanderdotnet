using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestForm;

namespace Commander
{
    public partial class ButtonCommandBarBase : UserControl
    {
        private ShellContextMenu contextMenu = new ShellContextMenu();


        public ButtonCommandBarBase()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Occurs when a view button is clicked.
        /// </summary>
        [Category("Action")]
        public event EventHandler ViewClick
        {
            add
            {
                viewButton.Click += value;
            }
            remove
            {
                viewButton.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when a edit button is clicked.
        /// </summary>
        [Category("Action")]
        public event EventHandler EditClick
        {
            add
            {
                editButton.Click += value;
            }
            remove
            {
                editButton.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when a copy button is clicked.
        /// </summary>
        [Category("Action")]
        public event EventHandler CopyClick
        {
            add
            {
                copyButton.Click += value;
            }
            remove
            {
                copyButton.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when a move button is clicked.
        /// </summary>
        [Category("Action")]
        public event EventHandler MoveClick
        {
            add
            {
                moveButton.Click += value;
            }
            remove
            {
                moveButton.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when a create folder button is clicked.
        /// </summary>
        [Category("Action")]
        public event EventHandler CreteFolderClick
        {
            add
            {
                creteFolderButton.Click += value;
            }
            remove
            {
                creteFolderButton.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when a delete button is clicked.
        /// </summary>
        [Category("Action")]
        public event EventHandler DeleteClick
        {
            add
            {
                deleteButton.Click += value;
            }
            remove
            {
                deleteButton.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when a exit button is clicked.
        /// </summary>
        [Category("Action")]
        public event EventHandler ExitClick
        {
            add
            {
                exitButton.Click += value;
            }
            remove
            {
                exitButton.Click -= value;
            }
        }


        private void deleteButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point location = deleteButton.PointToScreen(e.Location);
                contextMenu.Show(location, Shell.SpecialFolderPath.RecycleBin);
            }
        }

        private void deleteButton_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void deleteButton_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            contextMenu.DeleteCommand(files);
        }
    }
}
