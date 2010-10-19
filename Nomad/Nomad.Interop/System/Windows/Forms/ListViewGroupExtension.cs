namespace System.Windows.Forms
{
    using Microsoft;
    using Microsoft.Win32;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class ListViewGroupExtension
    {
        private static PropertyInfo GroupIdProperty;

        public static bool GetCollapsed(this ListViewGroup group)
        {
            if (group == null)
            {
                throw new ArgumentNullException();
            }
            return GetState(group, LVGS.LVGS_NORMAL | LVGS.LVGS_COLLAPSED);
        }

        public static bool GetCollapsible(this ListViewGroup group)
        {
            if (group == null)
            {
                throw new ArgumentNullException();
            }
            return (OS.IsWinVista ? GetState(group, LVGS.LVGS_COLLAPSIBLE) : false);
        }

        private static int GetGroupId(ListViewGroup group)
        {
            if (GroupIdProperty == null)
            {
                GroupIdProperty = typeof(ListViewGroup).GetProperty("ID", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return ((GroupIdProperty != null) ? ((int) GroupIdProperty.GetValue(group, null)) : -1);
        }

        private static bool GetState(ListViewGroup group, LVGS state)
        {
            bool flag;
            int groupId = GetGroupId(group);
            if (groupId < 0)
            {
                return false;
            }
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Microsoft.Win32.LVGROUP)));
            try
            {
                Microsoft.Win32.LVGROUP.SetMask(ptr, LVGF.LVGF_NONE | LVGF.LVGF_GROUPID | LVGF.LVGF_STATE, true);
                Microsoft.Win32.LVGROUP.SetGroupId(ptr, groupId);
                Microsoft.Win32.LVGROUP.SetStateMask(ptr, state, true);
                if (((int) Windows.SendMessage(group.ListView.Handle, 0x1095, (IntPtr) groupId, ptr)) >= 0)
                {
                    return ((Microsoft.Win32.LVGROUP.GetState(ptr) & state) > LVGS.LVGS_NORMAL);
                }
                flag = false;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return flag;
        }

        public static void SetCollapsed(this ListViewGroup group, bool value)
        {
            if (group == null)
            {
                throw new ArgumentNullException();
            }
            SetState(group, LVGS.LVGS_NORMAL | LVGS.LVGS_COLLAPSED, value);
        }

        public static void SetCollapsible(this ListViewGroup group, bool value)
        {
            if (group == null)
            {
                throw new ArgumentNullException();
            }
            if (!OS.IsWinVista)
            {
                throw new PlatformNotSupportedException();
            }
            SetState(group, LVGS.LVGS_COLLAPSIBLE, value);
        }

        private static void SetState(ListViewGroup group, LVGS state, bool value)
        {
            int groupId = GetGroupId(group);
            if (groupId >= 0)
            {
                Microsoft.Win32.LVGROUP lvgroup = new Microsoft.Win32.LVGROUP {
                    cbSize = Marshal.SizeOf(typeof(Microsoft.Win32.LVGROUP)),
                    mask = LVGF.LVGF_NONE | LVGF.LVGF_GROUPID | LVGF.LVGF_STATE,
                    iGroupId = groupId,
                    stateMask = state
                };
                if (value)
                {
                    lvgroup.state = state;
                }
                GCHandle handle = GCHandle.Alloc(lvgroup, GCHandleType.Pinned);
                try
                {
                    Windows.SendMessage(group.ListView.Handle, 0x1093, (IntPtr) groupId, handle.AddrOfPinnedObject());
                }
                finally
                {
                    handle.Free();
                }
            }
        }
    }
}

