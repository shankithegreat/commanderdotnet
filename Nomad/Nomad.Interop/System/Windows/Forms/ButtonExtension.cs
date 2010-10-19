namespace System.Windows.Forms
{
    using Microsoft;
    using Microsoft.Win32;
    using System;
    using System.Runtime.CompilerServices;

    public static class ButtonExtension
    {
        public static bool SetElevationRequiredState(this Button btn, bool required)
        {
            return (OS.IsWinVista && (CommCtrl.Button_SetElevationRequiredState(btn.Handle, required) == 1));
        }
    }
}

