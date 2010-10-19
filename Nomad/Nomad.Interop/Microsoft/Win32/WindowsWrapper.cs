namespace Microsoft.Win32
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public static class WindowsWrapper
    {
        public static uint ColorToCOLORREF(System.Drawing.Color color)
        {
            return (uint) (((color.B << 0x10) | (color.G << 8)) | color.R);
        }

        public static APPCOMMAND GET_APPCOMMAND_LPARAM(IntPtr lParam)
        {
            return (APPCOMMAND) ((ushort) (((ushort) (((long) lParam) >> 0x10)) & -61441));
        }

        public static FAPPCOMMAND GET_DEVICE_LPARAM(IntPtr lParam)
        {
            return (FAPPCOMMAND) ((ushort) (((ushort) (((long) lParam) >> 0x10)) & 0xf000));
        }

        public static ushort GET_KEYSTATE_LPARAM(IntPtr lParam)
        {
            return (ushort) ((long) lParam);
        }

        public static string GetClassName(IntPtr hWnd)
        {
            StringBuilder builder;
            return new StringBuilder(80) { Length = Windows.GetClassName(hWnd, builder, builder.Capacity) }.ToString();
        }

        public static Point GetMessagePos()
        {
            return new Point(Windows.GetMessagePos());
        }

        public static Keys HotkeyToShortcutKeys(short hotKey)
        {
            Keys keys = ((Keys) hotKey) & (Keys.OemClear | Keys.LButton);
            HOTKEYF hotkeyf = (HOTKEYF) ((byte) (hotKey >> 8));
            if (((byte) (hotkeyf & HOTKEYF.HOTKEYF_SHIFT)) > 0)
            {
                keys |= Keys.Shift;
            }
            if (((byte) (hotkeyf & HOTKEYF.HOTKEYF_CONTROL)) > 0)
            {
                keys |= Keys.Control;
            }
            if (((byte) (hotkeyf & HOTKEYF.HOTKEYF_ALT)) > 0)
            {
                keys |= Keys.Alt;
            }
            return keys;
        }

        public static string LoadString(Microsoft.Win32.SafeHandles.SafeLibraryHandle hInstance, uint uID)
        {
            IntPtr ptr;
            if (Windows.LoadString(hInstance, uID, out ptr, 0) > 0)
            {
                return Marshal.PtrToStringUni(ptr);
            }
            return null;
        }

        public static short ShortcutKeysToHotkey(Keys shortcut)
        {
            HOTKEYF hotkeyf = 0;
            if ((shortcut & Keys.Shift) > Keys.None)
            {
                hotkeyf = (HOTKEYF) ((byte) (hotkeyf | HOTKEYF.HOTKEYF_SHIFT));
            }
            if ((shortcut & Keys.Control) > Keys.None)
            {
                hotkeyf = (HOTKEYF) ((byte) (hotkeyf | HOTKEYF.HOTKEYF_CONTROL));
            }
            if ((shortcut & Keys.Alt) > Keys.None)
            {
                hotkeyf = (HOTKEYF) ((byte) (hotkeyf | HOTKEYF.HOTKEYF_ALT));
            }
            return (short) (((byte) (shortcut & Keys.KeyCode)) | (((byte) hotkeyf) << 8));
        }

        public static MOD ShortcutKeysToModifiers(Keys shortcut)
        {
            Keys keys = shortcut & ~Keys.KeyCode;
            MOD mod = 0;
            if ((keys & Keys.Alt) > Keys.None)
            {
                mod |= MOD.MOD_ALT;
            }
            if ((keys & Keys.Control) > Keys.None)
            {
                mod |= MOD.MOD_CONTROL;
            }
            if ((keys & Keys.Shift) > Keys.None)
            {
                mod |= MOD.MOD_SHIFT;
            }
            return mod;
        }
    }
}

