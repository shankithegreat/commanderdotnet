namespace Nomad.Controls
{
    using System;
    using System.Windows.Forms;

    public class NodeStateEventArgs : EventArgs
    {
        private TreeNode FNode;
        private TreeNodeStates FState;

        internal NodeStateEventArgs(TreeNode node, TreeNodeStates state)
        {
            this.FNode = node;
            this.FState = state;
        }

        public TreeNode Node
        {
            get
            {
                return this.FNode;
            }
        }

        public TreeNodeStates State
        {
            get
            {
                return this.FState;
            }
        }
    }
}

