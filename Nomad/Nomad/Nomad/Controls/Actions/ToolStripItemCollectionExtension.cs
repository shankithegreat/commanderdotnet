namespace Nomad.Controls.Actions
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public static class ToolStripItemCollectionExtension
    {
        public static ToolStripItem Add(this ToolStripItemCollection items, CustomAction action)
        {
            return items.Add(action, null, BindActionProperty.All);
        }

        public static ToolStripItem Add(this ToolStripItemCollection items, CustomAction action, BindActionProperty bind)
        {
            return items.Add(action, null, bind);
        }

        public static ToolStripItem Add(this ToolStripItemCollection items, CustomAction action, object target, BindActionProperty bind)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            ToolStripItem component = items.Add(action.Text);
            new ActionToolStripItemLink(action, component, target, bind);
            return component;
        }
    }
}

