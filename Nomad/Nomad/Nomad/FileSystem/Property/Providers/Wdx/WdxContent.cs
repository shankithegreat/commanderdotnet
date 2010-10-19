namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public class WdxContent : IDisposable
    {
        private ContentGetDetectStringHandler ContentGetDetectString;
        private ContentGetSupportedFieldHandler ContentGetSupportedField;
        private ContentGetSupportedFieldFlagsHandler ContentGetSupportedFieldFlags;
        private ContentGetValueHandler ContentGetValue;
        private ContentGetValueWHandler ContentGetValueW;
        private ContentSetValueHandler ContentSetValue;
        private ContentSetValueWHandler ContentSetValueW;
        private ContentStopGetValueHandler ContentStopGetValue;
        private ReadOnlyCollection<WdxFieldInfo> FieldsCollection;
        private Microsoft.Win32.SafeHandles.SafeLibraryHandle LibHandle;
        private Dictionary<string, List<KeyValuePair<Tuple<int, int>, object>>> UpdateMap;

        protected WdxContent(Microsoft.Win32.SafeHandles.SafeLibraryHandle libHandle)
        {
            this.LibHandle = libHandle;
            IntPtr procAddress = Windows.GetProcAddress(this.LibHandle, "ContentGetDetectString");
            if (procAddress != IntPtr.Zero)
            {
                this.ContentGetDetectString = (ContentGetDetectStringHandler) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ContentGetDetectStringHandler));
            }
            this.ContentGetSupportedField = (ContentGetSupportedFieldHandler) Marshal.GetDelegateForFunctionPointer(Windows.GetProcAddress(this.LibHandle, "ContentGetSupportedField"), typeof(ContentGetSupportedFieldHandler));
            procAddress = Windows.GetProcAddress(this.LibHandle, "ContentGetValueW");
            if (procAddress != IntPtr.Zero)
            {
                this.ContentGetValueW = (ContentGetValueWHandler) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ContentGetValueWHandler));
            }
            else
            {
                procAddress = Windows.GetProcAddress(this.LibHandle, "ContentGetValue");
                if (procAddress == IntPtr.Zero)
                {
                    throw new ArgumentException();
                }
                this.ContentGetValue = (ContentGetValueHandler) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ContentGetValueHandler));
            }
            procAddress = Windows.GetProcAddress(this.LibHandle, "ContentGetSupportedFieldFlags");
            if (procAddress != IntPtr.Zero)
            {
                this.ContentGetSupportedFieldFlags = (ContentGetSupportedFieldFlagsHandler) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ContentGetSupportedFieldFlagsHandler));
            }
            procAddress = Windows.GetProcAddress(this.LibHandle, "ContentSetValueW");
            if (procAddress != IntPtr.Zero)
            {
                this.ContentSetValueW = (ContentSetValueWHandler) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ContentSetValueWHandler));
            }
            else
            {
                procAddress = Windows.GetProcAddress(this.LibHandle, "ContentSetValue");
                if (procAddress != IntPtr.Zero)
                {
                    this.ContentSetValue = (ContentSetValueHandler) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ContentSetValueHandler));
                }
            }
            procAddress = Windows.GetProcAddress(libHandle, "ContentStopGetValue");
            if (procAddress != IntPtr.Zero)
            {
                this.ContentStopGetValue = (ContentStopGetValueHandler) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ContentStopGetValueHandler));
            }
            List<WdxFieldInfo> list = new List<WdxFieldInfo>();
            StringBuilder fieldName = new StringBuilder(0x400);
            StringBuilder units = new StringBuilder(fieldName.Capacity);
            int fieldIndex = 0;
            for (int i = this.ContentGetSupportedField(fieldIndex, fieldName, units, fieldName.Capacity); i != 0; i = this.ContentGetSupportedField(++fieldIndex, fieldName, units, fieldName.Capacity))
            {
                WdxFieldInfo item = new WdxFieldInfo {
                    FieldName = fieldName.ToString(),
                    FieldType = i
                };
                if (units.Length > 0)
                {
                    item.Units = units.ToString().Split(new char[] { '|' });
                }
                if (this.ContentGetSupportedFieldFlags != null)
                {
                    item.Flags = this.ContentGetSupportedFieldFlags(fieldIndex);
                }
                list.Add(item);
            }
            this.FieldsCollection = new ReadOnlyCollection<WdxFieldInfo>(list);
        }

        public void BeginUpdate(string fileName)
        {
            if (!this.CanSetValue)
            {
                if (this.LibHandle == null)
                {
                    throw new ObjectDisposedException("WdxContent");
                }
                throw new NotSupportedException();
            }
            if (this.UpdateMap == null)
            {
                this.UpdateMap = new Dictionary<string, List<KeyValuePair<Tuple<int, int>, object>>>();
            }
            this.UpdateMap.Add(fileName, new List<KeyValuePair<Tuple<int, int>, object>>());
        }

        public virtual void Dispose()
        {
            if (this.LibHandle != null)
            {
                IntPtr procAddress = Windows.GetProcAddress(this.LibHandle, "ContentPluginUnloading");
                if (procAddress != IntPtr.Zero)
                {
                    ContentPluginUnloadingHandler delegateForFunctionPointer = (ContentPluginUnloadingHandler) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ContentPluginUnloadingHandler));
                    delegateForFunctionPointer();
                }
                this.ContentGetDetectString = null;
                this.ContentGetSupportedField = null;
                this.ContentGetValue = null;
                this.ContentGetValueW = null;
                this.ContentGetSupportedFieldFlags = null;
                this.ContentSetValue = null;
                this.ContentSetValueW = null;
                this.ContentStopGetValue = null;
                this.LibHandle.Close();
                this.LibHandle = null;
                this.UpdateMap = null;
            }
        }

        public void EndUpdate(string fileName)
        {
            List<KeyValuePair<Tuple<int, int>, object>> list;
            if (!this.CanSetValue)
            {
                if (this.LibHandle == null)
                {
                    throw new ObjectDisposedException("WdxContent");
                }
                throw new NotSupportedException();
            }
            if (!((this.UpdateMap != null) && this.UpdateMap.TryGetValue(fileName, out list)))
            {
                throw new InvalidOperationException();
            }
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                int flags = 0;
                if (i == 0)
                {
                    flags |= 1;
                }
                if (i == (count - 1))
                {
                    flags |= 2;
                }
                KeyValuePair<Tuple<int, int>, object> pair = list[i];
                pair = list[i];
                pair = list[i];
                this.InternalSetValue(fileName, pair.Key.Item1, pair.Key.Item2, pair.Value, flags);
            }
            this.UpdateMap.Remove(fileName);
        }

        public int GetPendingUpdateCount(string fileName)
        {
            List<KeyValuePair<Tuple<int, int>, object>> list;
            if (!((this.UpdateMap != null) && this.UpdateMap.TryGetValue(fileName, out list)))
            {
                return -1;
            }
            return list.Count;
        }

        public object GetValue(string fileName, int fieldIndex, int unitIndex, bool delayed)
        {
            object obj2;
            IntPtr fieldValue = Marshal.AllocHGlobal(0x200);
            try
            {
                int num;
                int flags = delayed ? 1 : 0;
                if (this.ContentGetValueW != null)
                {
                    num = this.ContentGetValueW(fileName, fieldIndex, unitIndex, fieldValue, 0x200, flags);
                }
                else
                {
                    if (this.ContentGetValue == null)
                    {
                        throw new ObjectDisposedException("WdxContent");
                    }
                    num = this.ContentGetValue(fileName, fieldIndex, unitIndex, fieldValue, 0x200, flags);
                }
                switch (num)
                {
                    case -5:
                        throw new NotSupportedException();

                    case -4:
                        return WdxFieldInfo.OnDemandValue;

                    case -3:
                        return null;

                    case -2:
                        throw new IOException();

                    case -1:
                        throw new ArgumentException();

                    case 0:
                        return WdxFieldInfo.DelayedValue;

                    case 1:
                        return Marshal.ReadInt32(fieldValue);

                    case 2:
                        return Marshal.ReadInt64(fieldValue);

                    case 3:
                    {
                        double[] destination = new double[1];
                        Marshal.Copy(fieldValue, destination, 0, 1);
                        return destination[0];
                    }
                    case 4:
                    {
                        tdateformat tdateformat = (tdateformat) Marshal.PtrToStructure(fieldValue, typeof(tdateformat));
                        return new DateTime(tdateformat.wYear, tdateformat.wMonth, tdateformat.wDay);
                    }
                    case 5:
                    {
                        ttimeformat ttimeformat = (ttimeformat) Marshal.PtrToStructure(fieldValue, typeof(ttimeformat));
                        return new TimeSpan(ttimeformat.wHour, ttimeformat.wMinute, ttimeformat.wSecond);
                    }
                    case 6:
                        return Convert.ToBoolean(Marshal.ReadInt32(fieldValue));

                    case 7:
                    case 8:
                        return Marshal.PtrToStringAnsi(fieldValue);

                    case 10:
                        return DateTime.FromFileTime(Marshal.ReadInt64(fieldValue));

                    case 11:
                        return Marshal.PtrToStringUni(fieldValue);
                }
                throw new NotSupportedException();
            }
            finally
            {
                Marshal.FreeHGlobal(fieldValue);
            }
            return obj2;
        }

        private void InternalSetObject(string fileName, int fieldIndex, int unitIndex, int fieldType, object value, int flags)
        {
            GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            try
            {
                if (this.ContentSetValueW != null)
                {
                    this.ContentSetValueW(fileName, fieldIndex, unitIndex, fieldType, handle.AddrOfPinnedObject(), flags);
                }
                else
                {
                    this.ContentSetValue(fileName, fieldIndex, unitIndex, fieldType, handle.AddrOfPinnedObject(), flags);
                }
            }
            finally
            {
                handle.Free();
            }
        }

        private void InternalSetString(string fileName, int fieldIndex, int unitIndex, int fieldType, string value, int flags)
        {
            IntPtr zero = IntPtr.Zero;
            try
            {
                switch (fieldType)
                {
                    case 8:
                        zero = Marshal.StringToHGlobalAnsi(value);
                        break;

                    case 11:
                        zero = Marshal.StringToHGlobalUni(value);
                        break;

                    default:
                        throw new InvalidOperationException();
                }
                if (this.ContentSetValueW != null)
                {
                    this.ContentSetValueW(fileName, fieldIndex, unitIndex, fieldType, zero, flags);
                }
                else
                {
                    this.ContentSetValue(fileName, fieldIndex, unitIndex, fieldType, zero, flags);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
            }
        }

        private void InternalSetValue(string fileName, int fieldIndex, int unitIndex, object value, int flags)
        {
            ttimeformat ttimeformat;
            int fieldType = this.Fields[fieldIndex].FieldType;
            switch (fieldType)
            {
                case 1:
                    this.InternalSetObject(fileName, fieldIndex, unitIndex, fieldType, Convert.ToInt32(value), flags);
                    return;

                case 2:
                    this.InternalSetObject(fileName, fieldIndex, unitIndex, fieldType, Convert.ToInt64(value), flags);
                    return;

                case 3:
                    this.InternalSetObject(fileName, fieldIndex, unitIndex, fieldType, Convert.ToDouble(value), flags);
                    return;

                case 4:
                {
                    tdateformat tdateformat = new tdateformat();
                    DateTime time = Convert.ToDateTime(value);
                    tdateformat.wYear = (ushort) time.Year;
                    tdateformat.wMonth = (ushort) time.Month;
                    tdateformat.wDay = (ushort) time.Day;
                    this.InternalSetObject(fileName, fieldIndex, unitIndex, fieldType, tdateformat, flags);
                    return;
                }
                case 5:
                {
                    ttimeformat = new ttimeformat();
                    if (!(value is TimeSpan))
                    {
                        DateTime time2 = Convert.ToDateTime(value);
                        ttimeformat.wHour = (ushort) time2.Hour;
                        ttimeformat.wMinute = (ushort) time2.Minute;
                        ttimeformat.wSecond = (ushort) time2.Second;
                        break;
                    }
                    TimeSpan span = (TimeSpan) value;
                    ttimeformat.wHour = (ushort) span.Hours;
                    ttimeformat.wMinute = (ushort) span.Minutes;
                    ttimeformat.wSecond = (ushort) span.Seconds;
                    break;
                }
                case 6:
                    this.InternalSetObject(fileName, fieldIndex, unitIndex, fieldType, Convert.ToInt32(Convert.ToBoolean(value)), flags);
                    return;

                case 8:
                case 11:
                    this.InternalSetString(fileName, fieldIndex, unitIndex, fieldType, Convert.ToString(value), flags);
                    return;

                case 10:
                    this.InternalSetObject(fileName, fieldIndex, unitIndex, fieldType, Convert.ToDateTime(value).ToFileTimeUtc(), flags);
                    return;

                default:
                    throw new InvalidOperationException();
            }
            this.InternalSetObject(fileName, fieldIndex, unitIndex, fieldType, ttimeformat, flags);
        }

        public void SetValue(string fileName, int fieldIndex, int unitIndex, object value)
        {
            List<KeyValuePair<Tuple<int, int>, object>> list;
            if (!this.CanSetValue)
            {
                if (this.LibHandle == null)
                {
                    throw new ObjectDisposedException("WdxContent");
                }
                throw new NotSupportedException();
            }
            if ((this.UpdateMap != null) && this.UpdateMap.TryGetValue(fileName, out list))
            {
                list.Add(new KeyValuePair<Tuple<int, int>, object>(Tuple.Create<int, int>(fieldIndex, unitIndex), value));
            }
            else
            {
                this.InternalSetValue(fileName, fieldIndex, unitIndex, value, 3);
            }
        }

        public void StopGetValue(string fileName)
        {
            if (this.ContentStopGetValue != null)
            {
                this.ContentStopGetValue(fileName);
            }
        }

        public bool CanSetValue
        {
            get
            {
                return ((this.ContentSetValueW != null) || (this.ContentSetValue != null));
            }
        }

        public string DetectString
        {
            get
            {
                if (this.ContentGetDetectString != null)
                {
                    StringBuilder detectString = new StringBuilder(0x800);
                    this.ContentGetDetectString(detectString, detectString.Capacity);
                    return detectString.ToString();
                }
                return string.Empty;
            }
        }

        public ReadOnlyCollection<WdxFieldInfo> Fields
        {
            get
            {
                return this.FieldsCollection;
            }
        }
    }
}

