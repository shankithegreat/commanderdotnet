namespace Microsoft.Win32
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct NMCUSTOMDRAW
    {
        public NMHDR hdr;
        public CDDS dwDrawStage;
        public IntPtr hdc;
        public Microsoft.Win32.RECT rc;
        public IntPtr dwItemSpec;
        public CDIS uItemState;
        public int lItemlParam;
        public static unsafe CDDS GetDrawStage(IntPtr ptr)
        {
            return ((void*) ptr).dwDrawStage;
        }

        public static unsafe IntPtr GetItemSpec(IntPtr ptr)
        {
            return ((void*) ptr).dwItemSpec;
        }

        public static unsafe CDIS GetItemState(IntPtr ptr)
        {
            return ((void*) ptr).uItemState;
        }

        public static unsafe IntPtr GetHdc(IntPtr ptr)
        {
            return ((void*) ptr).hdc;
        }

        public static unsafe Rectangle GetRect(IntPtr ptr)
        {
            Microsoft.Win32.RECT* rectPtr = &((void*) ptr).rc;
            return Rectangle.FromLTRB(rectPtr->Left, rectPtr->Top, rectPtr->Right, rectPtr->Bottom);
        }
    }
}

