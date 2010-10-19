namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct LVGROUP
    {
        public int cbSize;
        public LVGF mask;
        public IntPtr pszHeader;
        public int cchHeader;
        public IntPtr pszFooter;
        public int cchFooter;
        public int iGroupId;
        public LVGS stateMask;
        public LVGS state;
        public LVGA uAlign;
        public IntPtr pszSubtitle;
        public uint cchSubtitle;
        public IntPtr pszTask;
        public uint cchTask;
        public IntPtr pszDescriptionTop;
        public uint cchDescriptionTop;
        public IntPtr pszDescriptionBottom;
        public uint cchDescriptionBottom;
        public int iTitleImage;
        public int iExtendedImage;
        public int iFirstItem;
        public uint cItems;
        public IntPtr pszSubsetTitle;
        public static unsafe void SetMask(IntPtr ptr, LVGF mask, bool value)
        {
            if (value)
            {
                void* voidPtr1 = (void*) ptr;
                voidPtr1->mask |= mask;
            }
            else
            {
                void* voidPtr2 = (void*) ptr;
                voidPtr2->mask &= ~mask;
            }
        }

        public static unsafe LVGS GetState(IntPtr ptr)
        {
            return ((void*) ptr).state;
        }

        public static unsafe void SetGroupId(IntPtr ptr, int groupId)
        {
            ((void*) ptr).iGroupId = groupId;
        }

        public static unsafe void SetState(IntPtr ptr, LVGS mask, bool value)
        {
            if (value)
            {
                void* voidPtr1 = (void*) ptr;
                voidPtr1->state |= mask;
            }
            else
            {
                void* voidPtr2 = (void*) ptr;
                voidPtr2->state &= ~mask;
            }
        }

        public static unsafe void SetStateMask(IntPtr ptr, LVGS mask, bool value)
        {
            if (value)
            {
                void* voidPtr1 = (void*) ptr;
                voidPtr1->stateMask |= mask;
            }
            else
            {
                void* voidPtr2 = (void*) ptr;
                voidPtr2->stateMask &= ~mask;
            }
        }
    }
}

