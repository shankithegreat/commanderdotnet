namespace Microsoft.IO
{
    using Microsoft;
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public static class VolumeEvents
    {
        private static DriveChars FDriveMask;
        private static VolumeFilter FFilter;
        private static DriveChars FReadyDriveMask;
        private static Form FTopLevelForm;
        private static EventHandler IdleHandler;

        public static  event EventHandler<VolumeEventArgs> Arrived
        {
            add
            {
                VolumeFilter filter = Filter;
                filter.Arrived = (EventHandler<VolumeEventArgs>) Delegate.Combine(filter.Arrived, value);
                Filter.Register(TopLevelForm);
                RegisterIdleHandler();
            }
            remove
            {
                VolumeFilter filter = Filter;
                filter.Arrived = (EventHandler<VolumeEventArgs>) Delegate.Remove(filter.Arrived, value);
                Filter.Unregister();
                UnregisterIdleHandler();
            }
        }

        public static  event EventHandler Changed
        {
            add
            {
                VolumeFilter filter = Filter;
                filter.Changed = (EventHandler) Delegate.Combine(filter.Changed, value);
                Filter.Register(TopLevelForm);
                RegisterIdleHandler();
            }
            remove
            {
                VolumeFilter filter = Filter;
                filter.Changed = (EventHandler) Delegate.Remove(filter.Changed, value);
                Filter.Unregister();
                UnregisterIdleHandler();
            }
        }

        public static  event EventHandler<VolumeEventArgs> Removed
        {
            add
            {
                VolumeFilter filter = Filter;
                filter.Removed = (EventHandler<VolumeEventArgs>) Delegate.Combine(filter.Removed, value);
                Filter.Register(TopLevelForm);
                RegisterIdleHandler();
            }
            remove
            {
                VolumeFilter filter = Filter;
                filter.Removed = (EventHandler<VolumeEventArgs>) Delegate.Remove(filter.Removed, value);
                Filter.Unregister();
                UnregisterIdleHandler();
            }
        }

        private static void ApplicationEvents_Idle(object sender, EventArgs e)
        {
            DriveChars logicalDrives = (DriveChars) Windows.GetLogicalDrives();
            DriveChars readyDriveMask = GetReadyDriveMask(FDriveMask & logicalDrives);
            bool flag = false;
            int num = 1;
            for (int i = 0; i < 0x1a; i++)
            {
                DriveChars driveChar = FDriveMask & (num << i);
                if (driveChar != DriveChars.None)
                {
                    if (((FReadyDriveMask & driveChar) == DriveChars.None) && ((readyDriveMask & driveChar) > DriveChars.None))
                    {
                        Filter.RaiseArrived(new VolumeEventArgs(driveChar));
                        flag = true;
                    }
                    if (((FReadyDriveMask & driveChar) > DriveChars.None) && ((readyDriveMask & driveChar) == DriveChars.None))
                    {
                        Filter.RaiseRemoved(new VolumeEventArgs(driveChar));
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                Filter.RaiseChanged(EventArgs.Empty);
            }
            FReadyDriveMask = readyDriveMask;
        }

        private static DriveChars GetReadyDriveMask(DriveChars driveMask)
        {
            DriveChars none = DriveChars.None;
            int num = 1;
            for (int i = 0; i < 0x1a; i++)
            {
                DriveChars chars2 = driveMask & (num << i);
                if (chars2 != DriveChars.None)
                {
                    VolumeInfo info = VolumeCache.Get(chars2.ToString() + @":\");
                    if ((info != null) && info.IsReady)
                    {
                        none |= chars2;
                    }
                }
            }
            return none;
        }

        public static void RaiseRemovingEvent(string drivePath)
        {
            Filter.RaiseRemoving(drivePath);
        }

        private static void RegisterIdleHandler()
        {
            if ((FDriveMask != DriveChars.None) && (IdleHandler == null))
            {
                IdleHandler = new EventHandler(VolumeEvents.ApplicationEvents_Idle);
                Application.Idle += IdleHandler;
            }
        }

        public static bool RegisterRemovingEvent(string drivePath, CancelEventHandler handler)
        {
            Filter.Register(TopLevelForm);
            return Filter.RegisterRemoving(drivePath, handler);
        }

        private static void UnregisterIdleHandler()
        {
            if (!((IdleHandler == null) || Filter.HasEvents))
            {
                Application.Idle -= IdleHandler;
                IdleHandler = null;
            }
        }

        public static void UnregisterRemovingEvent(string drivePath, CancelEventHandler handler)
        {
            Filter.UnregisterRemoving(drivePath, handler);
            Filter.Unregister();
        }

        public static DriveChars DriveMask
        {
            get
            {
                return FDriveMask;
            }
            set
            {
                FDriveMask = value;
                if ((FFilter != null) && FFilter.HasEvents)
                {
                    RegisterIdleHandler();
                }
                if (FDriveMask == DriveChars.None)
                {
                    UnregisterIdleHandler();
                }
            }
        }

        private static VolumeFilter Filter
        {
            get
            {
                if (FFilter == null)
                {
                    FFilter = new VolumeFilter();
                }
                return FFilter;
            }
        }

        public static Form TopLevelForm
        {
            get
            {
                if ((FTopLevelForm == null) && (Application.OpenForms.Count > 0))
                {
                    FTopLevelForm = Application.OpenForms[0];
                }
                return FTopLevelForm;
            }
            set
            {
                FTopLevelForm = value;
                if ((FFilter != null) && FFilter.HasEvents)
                {
                    FFilter.Register(TopLevelForm);
                }
            }
        }

        private class DeviceNotificationInfo
        {
            public SafeFileHandle FolderHandle;
            public SafeDeviceNotificationHandle NotificationHandle;

            public event CancelEventHandler Notification;

            public void Close()
            {
                this.NotificationHandle.Close();
                this.FolderHandle.Close();
            }

            public void OnNotification(CancelEventArgs e)
            {
                if (this.Notification != null)
                {
                    this.Notification(null, e);
                }
            }

            public bool HasNotification
            {
                get
                {
                    return (this.Notification != null);
                }
            }
        }

        private class VolumeFilter : NativeWindow
        {
            public EventHandler<VolumeEventArgs> Arrived;
            private DriveChars ArrivingChars;
            private Form BroadcastForm;
            public EventHandler Changed;
            private int MsgQueryCancelAutoPlay = -1;
            public EventHandler<VolumeEventArgs> Removed;
            private Dictionary<string, VolumeEvents.DeviceNotificationInfo> RemovingMap;
            private static readonly IntPtr TimerArriveId = ((IntPtr) 0x152a);

            private void FormHandle_Created(object sender, EventArgs e)
            {
                base.AssignHandle(((Control) sender).Handle);
            }

            private void FormHandle_Destroyed(object sender, EventArgs e)
            {
                this.ReleaseHandle();
            }

            private DriveChars GetVolumeDriveChars(IntPtr hdrPtr)
            {
                if (DEV_BROADCAST_HDR.GetDeviceType(hdrPtr) != DBT_DEVTYP.DBT_DEVTYP_VOLUME)
                {
                    return DriveChars.None;
                }
                _DEV_BROADCAST_VOLUME _dev_broadcast_volume = (_DEV_BROADCAST_VOLUME) Marshal.PtrToStructure(hdrPtr, typeof(_DEV_BROADCAST_VOLUME));
                return (DriveChars) _dev_broadcast_volume.dbcv_unitmask;
            }

            public void RaiseArrived(VolumeEventArgs e)
            {
                if (this.Arrived != null)
                {
                    this.Arrived(null, e);
                }
            }

            public void RaiseChanged(EventArgs e)
            {
                if (this.Changed != null)
                {
                    this.Changed(null, e);
                }
            }

            public void RaiseRemoved(VolumeEventArgs e)
            {
                if (this.Removed != null)
                {
                    this.Removed(null, e);
                }
            }

            public void RaiseRemoving(string drivePath)
            {
                VolumeEvents.DeviceNotificationInfo info;
                if ((this.RemovingMap != null) && this.RemovingMap.TryGetValue(drivePath, out info))
                {
                    CancelEventArgs e = new CancelEventArgs();
                    info.OnNotification(e);
                }
            }

            private void RaiseVolumeEvent(DriveChars chars, EventHandler<VolumeEventArgs> eventHandler)
            {
                if (eventHandler != null)
                {
                    uint num = 1;
                    for (int i = 0; i < 0x1a; i++)
                    {
                        DriveChars driveChar = chars & ((DriveChars) (num << i));
                        if (driveChar != DriveChars.None)
                        {
                            VolumeEventArgs e = new VolumeEventArgs(driveChar);
                            eventHandler(null, e);
                        }
                    }
                }
                this.RaiseChanged(EventArgs.Empty);
            }

            public void Register(Form form)
            {
                if ((form != this.BroadcastForm) && (form != null))
                {
                    if (this.BroadcastForm != null)
                    {
                        this.BroadcastForm.HandleCreated -= new EventHandler(this.FormHandle_Created);
                        this.BroadcastForm.HandleDestroyed -= new EventHandler(this.FormHandle_Destroyed);
                        if ((this.RemovingMap != null) && (this.RemovingMap.Count > 0))
                        {
                            foreach (VolumeEvents.DeviceNotificationInfo info in this.RemovingMap.Values)
                            {
                                info.Close();
                            }
                            this.RemovingMap = null;
                        }
                    }
                    this.BroadcastForm = form;
                    if (this.BroadcastForm.IsHandleCreated)
                    {
                        if (this.MsgQueryCancelAutoPlay < 0)
                        {
                            this.MsgQueryCancelAutoPlay = Windows.RegisterWindowMessage("QueryCancelAutoPlay");
                        }
                        base.AssignHandle(this.BroadcastForm.Handle);
                    }
                    this.BroadcastForm.HandleCreated += new EventHandler(this.FormHandle_Created);
                    this.BroadcastForm.HandleDestroyed += new EventHandler(this.FormHandle_Destroyed);
                }
            }

            public bool RegisterRemoving(string drivePath, CancelEventHandler handler)
            {
                VolumeEvents.DeviceNotificationInfo info;
                if (this.RemovingMap == null)
                {
                    this.RemovingMap = new Dictionary<string, VolumeEvents.DeviceNotificationInfo>(StringComparer.OrdinalIgnoreCase);
                }
                if (!this.RemovingMap.TryGetValue(drivePath, out info))
                {
                    _DEV_BROADCAST_HANDLE _dev_broadcast_handle;
                    info = new VolumeEvents.DeviceNotificationInfo {
                        FolderHandle = Windows.CreateFile(drivePath, 0, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, 0x2000000, IntPtr.Zero)
                    };
                    if (info.FolderHandle.IsInvalid)
                    {
                        return false;
                    }
                    _dev_broadcast_handle = new _DEV_BROADCAST_HANDLE {
                        dbch_size = Marshal.SizeOf(_dev_broadcast_handle),
                        dbch_devicetype = DBT_DEVTYP.DBT_DEVTYP_HANDLE,
                        dbch_handle = info.FolderHandle.DangerousGetHandle()
                    };
                    info.NotificationHandle = Dbt.RegisterDeviceNotification(base.Handle, ref _dev_broadcast_handle, DEVICE_NOTIFY.DEVICE_NOTIFY_WINDOW_HANDLE);
                    if (info.NotificationHandle.IsInvalid)
                    {
                        return false;
                    }
                    this.RemovingMap.Add(drivePath, info);
                }
                info.Notification += handler;
                return true;
            }

            public void Unregister()
            {
                if (!this.HasEvents)
                {
                    if (this.BroadcastForm != null)
                    {
                        this.BroadcastForm.HandleCreated -= new EventHandler(this.FormHandle_Created);
                        this.BroadcastForm.HandleDestroyed -= new EventHandler(this.FormHandle_Destroyed);
                        this.BroadcastForm = null;
                    }
                    if (base.Handle != IntPtr.Zero)
                    {
                        this.ReleaseHandle();
                    }
                }
            }

            public void UnregisterRemoving(string drivePath, CancelEventHandler handler)
            {
                VolumeEvents.DeviceNotificationInfo info;
                if ((this.RemovingMap != null) && this.RemovingMap.TryGetValue(drivePath, out info))
                {
                    info.Notification -= handler;
                    if (!info.HasNotification)
                    {
                        info.Close();
                        this.RemovingMap.Remove(drivePath);
                    }
                }
            }

            protected override void WndProc(ref Message m)
            {
                int msg = m.Msg;
                if (msg == 0x113)
                {
                    if (m.WParam == TimerArriveId)
                    {
                        Windows.KillTimer(base.Handle, TimerArriveId);
                        if (this.ArrivingChars != DriveChars.None)
                        {
                            this.RaiseVolumeEvent(this.ArrivingChars, this.Arrived);
                        }
                        else
                        {
                            this.RaiseChanged(EventArgs.Empty);
                        }
                        this.ArrivingChars = DriveChars.None;
                    }
                }
                else
                {
                    DriveChars none;
                    if (msg != 0x219)
                    {
                        if (m.Msg == this.MsgQueryCancelAutoPlay)
                        {
                            none = DriveChars.None;
                            if (OS.IsWinXP)
                            {
                                none = (DriveChars) (((int) 1) << ((int) m.WParam));
                                if ((this.ArrivingChars & none) > DriveChars.None)
                                {
                                    goto Label_02E5;
                                }
                                this.ArrivingChars |= none;
                            }
                            if (Windows.SetTimer(base.Handle, TimerArriveId, 100, IntPtr.Zero) == IntPtr.Zero)
                            {
                                if (none != DriveChars.None)
                                {
                                    this.RaiseArrived(new VolumeEventArgs(none));
                                }
                                this.RaiseChanged(EventArgs.Empty);
                            }
                        }
                    }
                    else
                    {
                        switch (((int) m.WParam))
                        {
                            case 0x8000:
                                none = this.GetVolumeDriveChars(m.LParam);
                                if (none != DriveChars.None)
                                {
                                    this.ArrivingChars = none;
                                    if (Windows.SetTimer(base.Handle, TimerArriveId, 0x76c, IntPtr.Zero) == IntPtr.Zero)
                                    {
                                        this.ArrivingChars = DriveChars.None;
                                        this.RaiseVolumeEvent(none, this.Arrived);
                                    }
                                }
                                goto Label_02E5;

                            case 0x8001:
                                if (DEV_BROADCAST_HDR.GetDeviceType(m.LParam) == DBT_DEVTYP.DBT_DEVTYP_HANDLE)
                                {
                                    _DEV_BROADCAST_HANDLE _dev_broadcast_handle = (_DEV_BROADCAST_HANDLE) Marshal.PtrToStructure(m.LParam, typeof(_DEV_BROADCAST_HANDLE));
                                    if (this.RemovingMap != null)
                                    {
                                        foreach (VolumeEvents.DeviceNotificationInfo info in this.RemovingMap.Values)
                                        {
                                            if (info.NotificationHandle.DangerousGetHandle() == _dev_broadcast_handle.dbch_hdevnotify)
                                            {
                                                CancelEventArgs e = new CancelEventArgs(false);
                                                info.OnNotification(e);
                                                if (e.Cancel)
                                                {
                                                    m.Result = (IntPtr) 0x424d5144;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                                goto Label_02E5;

                            case 0x8002:
                            case 0x8003:
                                goto Label_02E5;

                            case 0x8004:
                                none = this.GetVolumeDriveChars(m.LParam);
                                if (none != DriveChars.None)
                                {
                                    this.RaiseVolumeEvent(none, this.Removed);
                                }
                                goto Label_02E5;
                        }
                    }
                }
            Label_02E5:
                base.WndProc(ref m);
            }

            public bool HasEvents
            {
                get
                {
                    return ((((this.Changed != null) || (this.Arrived != null)) || (this.Removed != null)) || ((this.RemovingMap != null) && (this.RemovingMap.Count > 0)));
                }
            }
        }
    }
}

