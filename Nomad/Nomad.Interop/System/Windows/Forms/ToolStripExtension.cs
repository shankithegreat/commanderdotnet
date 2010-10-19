namespace System.Windows.Forms
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class ToolStripExtension
    {
        private static MethodInfo ToolStripShouldSerializeForeColor;

        public static bool IsForeColorSet(this ToolStrip ts)
        {
            if (ToolStripShouldSerializeForeColor == null)
            {
                ToolStripShouldSerializeForeColor = typeof(ToolStrip).GetMethod("ShouldSerializeForeColor", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return ((ToolStripShouldSerializeForeColor != null) && ((bool) ToolStripShouldSerializeForeColor.Invoke(ts, null)));
        }

        public static void UnselectAll(this ToolStrip ts)
        {
            foreach (ToolStripItem item in ts.Items)
            {
                item.Unselect();
            }
        }
    }
}

