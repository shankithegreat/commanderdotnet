namespace System.Windows.Forms
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class ToolStripItemExtension
    {
        private static MethodInfo ToolStripItemShouldSerializeForeColor;
        private static MethodInfo ToolStripItemUnselect;

        public static bool IsForeColorSet(this ToolStripItem tsi)
        {
            if (ToolStripItemShouldSerializeForeColor == null)
            {
                ToolStripItemShouldSerializeForeColor = typeof(ToolStripItem).GetMethod("ShouldSerializeForeColor", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return ((ToolStripItemShouldSerializeForeColor != null) && ((bool) ToolStripItemShouldSerializeForeColor.Invoke(tsi, null)));
        }

        public static void Unselect(this ToolStripItem tsi)
        {
            if (tsi.Selected)
            {
                if (ToolStripItemUnselect == null)
                {
                    ToolStripItemUnselect = typeof(ToolStripItem).GetMethod("Unselect", BindingFlags.NonPublic | BindingFlags.Instance);
                }
                if (ToolStripItemUnselect != null)
                {
                    ToolStripItemUnselect.Invoke(tsi, null);
                }
            }
        }
    }
}

