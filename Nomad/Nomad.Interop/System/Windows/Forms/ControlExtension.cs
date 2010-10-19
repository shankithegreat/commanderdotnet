namespace System.Windows.Forms
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class ControlExtension
    {
        private static MethodInfo GetStateMethod;
        private static MethodInfo IsFontSetMethod;
        private static PropertyInfo IsLayoutSuspendedProperty;
        private static MethodInfo SetStateMethod;
        private static PropertyInfo ShowKeyboardCuesProperty;

        public static bool GetState(this Control control, int state)
        {
            if (GetStateMethod == null)
            {
                GetStateMethod = typeof(Control).GetMethod("GetState", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return (bool) GetStateMethod.Invoke(control, new object[] { state });
        }

        public static bool IsFontSet(this Control control)
        {
            if (IsFontSetMethod == null)
            {
                IsFontSetMethod = typeof(Control).GetMethod("IsFontSet", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return (bool) IsFontSetMethod.Invoke(control, null);
        }

        public static bool IsLayoutSuspended(this Control control)
        {
            if (IsLayoutSuspendedProperty == null)
            {
                IsLayoutSuspendedProperty = typeof(Control).GetProperty("IsLayoutSuspended", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return (bool) IsLayoutSuspendedProperty.GetValue(control, null);
        }

        public static void SetState(this Control control, int state, bool value)
        {
            if (SetStateMethod == null)
            {
                SetStateMethod = typeof(Control).GetMethod("SetState", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            SetStateMethod.Invoke(control, new object[] { state, value });
        }

        public static bool ShowKeyboardCues(this Control control)
        {
            if (ShowKeyboardCuesProperty == null)
            {
                ShowKeyboardCuesProperty = typeof(Control).GetProperty("ShowKeyboardCues", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return (bool) ShowKeyboardCuesProperty.GetValue(control, null);
        }
    }
}

