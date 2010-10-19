namespace System.Windows.Forms
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class ToolStripArrowRenderEventArgsExtender
    {
        private static FieldInfo ArrowColorChangedField;

        public static bool IsArrowColorChanged(this ToolStripArrowRenderEventArgs e)
        {
            if (ArrowColorChangedField == null)
            {
                ArrowColorChangedField = typeof(ToolStripArrowRenderEventArgs).GetField("arrowColorChanged", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return ((ArrowColorChangedField != null) && ((bool) ArrowColorChangedField.GetValue(e)));
        }
    }
}

