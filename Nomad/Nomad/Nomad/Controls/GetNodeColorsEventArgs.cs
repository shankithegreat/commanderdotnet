namespace Nomad.Controls
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GetNodeColorsEventArgs : NodeStateEventArgs
    {
        internal GetNodeColorsEventArgs(TreeNode node, TreeNodeStates state, Color backColor, Color foreColor) : base(node, state)
        {
            this.BackColor = backColor;
            this.ForeColor = foreColor;
        }

        public Color BackColor { get; set; }

        public Color ForeColor { get; set; }
    }
}

