namespace System.Windows.Forms
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class ToolStripItemTextRenderEventArgsExtender
    {
        private static FieldInfo TextColorChangedField;

        public static bool IsTextColorChanged(this ToolStripItemTextRenderEventArgs e)
        {
            if (TextColorChangedField == null)
            {
                TextColorChangedField = typeof(ToolStripItemTextRenderEventArgs).GetField("textColorChanged", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return ((TextColorChangedField != null) && ((bool) TextColorChangedField.GetValue(e)));
        }
    }
}

