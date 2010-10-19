namespace System.Windows.Forms
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class CursorExtension
    {
        private static Dictionary<IntPtr, int> CursorHeightMap = new Dictionary<IntPtr, int>();

        private static int CalculateRealHeight(Cursor cursor)
        {
            using (Bitmap bitmap = new Bitmap(cursor.Size.Width, cursor.Size.Height))
            {
                Rectangle targetRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    cursor.Draw(graphics, targetRect);
                }
                BitmapData bitmapdata = bitmap.LockBits(targetRect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                try
                {
                    if (bitmapdata.PixelFormat == PixelFormat.Format32bppArgb)
                    {
                        int height = bitmapdata.Height;
                        do
                        {
                            int num2 = --height * bitmapdata.Stride;
                            for (int i = 0; i < bitmapdata.Width; i++)
                            {
                                if ((Marshal.ReadInt32(bitmapdata.Scan0, num2 + (i * 4)) != 0) && (height > 0))
                                {
                                    return height;
                                }
                            }
                        }
                        while (height > 0);
                    }
                }
                finally
                {
                    bitmap.UnlockBits(bitmapdata);
                }
            }
            return cursor.Size.Height;
        }

        public static void FixHandCursor()
        {
            FieldInfo field = typeof(Cursors).GetField("hand", BindingFlags.NonPublic | BindingFlags.Static);
            if (field != null)
            {
                field.SetValue(null, new Cursor(Windows.LoadCursor(IntPtr.Zero, PredefinedCursor.IDC_HAND)));
            }
        }

        public static int GetPrefferedHeight(this Cursor cursor)
        {
            if (cursor != null)
            {
                return Math.Min(cursor.Size.Height, cursor.GetRealHeight() + 4);
            }
            return 0x20;
        }

        public static int GetRealHeight(this Cursor cursor)
        {
            int num;
            if (!CursorHeightMap.TryGetValue(cursor.Handle, out num))
            {
                num = CalculateRealHeight(cursor);
                CursorHeightMap.Add(cursor.Handle, num);
            }
            return num;
        }
    }
}

