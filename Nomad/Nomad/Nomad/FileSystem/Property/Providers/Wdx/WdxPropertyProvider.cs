namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.Configuration;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Property.Providers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Text;

    public class WdxPropertyProvider : WdxContent, ILocalFilePropertyProvider, IPropertyProvider
    {
        private WdxDetectString CheckFile;
        private static string FDefaultIniName;
        private Dictionary<int, Tuple<int, int>> PropertyFieldMap;
        private ResourceManager Resources;

        protected WdxPropertyProvider(Microsoft.Win32.SafeHandles.SafeLibraryHandle libHandle, string libPath) : base(libHandle)
        {
            Version version;
            this.PropertyFieldMap = new Dictionary<int, Tuple<int, int>>();
            IntPtr procAddress = Windows.GetProcAddress(libHandle, "ContentSetDefaultParams");
            if (procAddress != IntPtr.Zero)
            {
                ContentDefaultParamStruct struct2;
                ContentSetDefaultParamsHandler delegateForFunctionPointer = (ContentSetDefaultParamsHandler) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ContentSetDefaultParamsHandler));
                struct2 = new ContentDefaultParamStruct {
                    size = Marshal.SizeOf(struct2),
                    PluginInterfaceVersionHi = 2,
                    PluginInterfaceVersionLow = 0,
                    DefaultIniName = DefaultIniName
                };
                delegateForFunctionPointer(ref struct2);
            }
            if (VersionHelper.TryParse(FileVersionInfo.GetVersionInfo(libPath).FileVersion, VersionStyles.Any, out version))
            {
                TypeDescriptor.AddAttributes(this, new Attribute[] { new VersionAttribute(version) });
            }
            string path = Path.ChangeExtension(libPath, ".lng");
            if (File.Exists(path))
            {
                this.Resources = new LngResourceManager(path);
            }
            this.Register(Path.GetFileName(libPath));
        }

        public IGetVirtualProperty AddProperties(FileSystemInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException();
            }
            if (this.CheckFile == null)
            {
                string detectString = base.DetectString;
                if (!string.IsNullOrEmpty(detectString))
                {
                    this.CheckFile = new WdxDetectString(detectString);
                }
            }
            if ((this.CheckFile == null) || ((info is FileInfo) && this.CheckFile.IsMatch((FileInfo) info)))
            {
                string fileName = PathHelper.ExcludeTrailingDirectorySeparator(info.FullName);
                return (base.CanSetValue ? new WdxSetPropertyBag(this, fileName) : new WdxPropertyBag(this, fileName));
            }
            return null;
        }

        public static WdxPropertyProvider Create(string wdxName)
        {
            Microsoft.Win32.SafeHandles.SafeLibraryHandle hModule = Windows.LoadLibrary(wdxName);
            if (!hModule.IsInvalid)
            {
                if (Windows.GetProcAddress(hModule, "ContentGetSupportedField") != IntPtr.Zero)
                {
                    return new WdxPropertyProvider(hModule, wdxName);
                }
                hModule.Close();
            }
            return null;
        }

        private string GetLocalizedPropertyName(VirtualProperty property)
        {
            Tuple<int, int> tuple;
            if (this.PropertyFieldMap.TryGetValue(property.PropertyId, out tuple))
            {
                WdxFieldInfo info = base.Fields[tuple.Item1];
                if (((info.FieldType != 7) && (info.Units != null)) && (info.Units.Length > tuple.Item2))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(this.GetLocalizedString(info.FieldName));
                    builder.Append('.');
                    builder.Append(this.GetLocalizedString(info.Units[tuple.Item2]));
                    return builder.ToString();
                }
                return this.GetLocalizedString(info.FieldName);
            }
            return property.PropertyName;
        }

        private string GetLocalizedString(string str)
        {
            string str2 = null;
            if (this.Resources != null)
            {
                str2 = this.Resources.GetString(str);
            }
            return (string.IsNullOrEmpty(str2) ? str : str2);
        }

        public VirtualPropertySet GetRegisteredProperties()
        {
            VirtualPropertySet set = new VirtualPropertySet();
            foreach (int num in this.PropertyFieldMap.Keys)
            {
                set[num] = true;
            }
            return set;
        }

        private void Register(string groupName)
        {
            int groupId = VirtualProperty.RegisterGroup(groupName);
            for (int i = 0; i < base.Fields.Count; i++)
            {
                Type type;
                TypeConverter converter;
                Func<VirtualProperty, string> func;
                int num4;
                WdxFieldInfo info = base.Fields[i];
                if (info.FieldName != "-")
                {
                    converter = null;
                    switch (info.FieldType)
                    {
                        case 1:
                            type = typeof(int);
                            if ((info.Flags & ContentFlag.contflags_substsize) > 0)
                            {
                                converter = SizeTypeConverter.Default;
                            }
                            goto Label_0124;

                        case 2:
                            type = typeof(long);
                            if ((info.Flags & ContentFlag.contflags_substsize) > 0)
                            {
                                converter = SizeTypeConverter.Default;
                            }
                            goto Label_0124;

                        case 3:
                            type = typeof(double);
                            goto Label_0124;

                        case 4:
                        case 10:
                            type = typeof(DateTime);
                            converter = DateTimeTypeConverter.Default;
                            goto Label_0124;

                        case 5:
                            type = typeof(TimeSpan);
                            converter = DurationTypeConverter.Default;
                            goto Label_0124;

                        case 6:
                            type = typeof(bool);
                            goto Label_0124;

                        case 7:
                        case 8:
                        case 11:
                            type = typeof(string);
                            goto Label_0124;

                        case 9:
                        {
                            continue;
                        }
                    }
                }
                continue;
            Label_0124:
                func = new Func<VirtualProperty, string>(this.GetLocalizedPropertyName);
                if (((info.FieldType != 7) && (info.Units != null)) && (info.Units.Length > 0))
                {
                    for (int j = 0; j < info.Units.Length; j++)
                    {
                        num4 = VirtualProperty.RegisterProperty(string.Format("{0}.{1}.{2}", groupName, info.FieldName, info.Units[j]), groupId, type, -1, converter, 0, func);
                        this.PropertyFieldMap.Add(num4, Tuple.Create<int, int>(i, j));
                    }
                }
                else
                {
                    num4 = VirtualProperty.RegisterProperty(string.Format("{0}.{1}", groupName, info.FieldName), groupId, type, -1, converter, 0, func);
                    this.PropertyFieldMap.Add(num4, Tuple.Create<int, int>(i, 0));
                }
            }
        }

        private static string DefaultIniName
        {
            get
            {
                if (FDefaultIniName == null)
                {
                    string fDefaultIniName = FDefaultIniName;
                }
                return (FDefaultIniName = Path.Combine(Path.GetDirectoryName(ConfigurableSettingsProvider.UserConfigPath), "wdx.ini"));
            }
        }

        private class WdxPropertyBag : CustomPropertyProvider, IGetVirtualProperty, IDisposable
        {
            protected VirtualPropertySet ActualAvailableSet;
            protected Dictionary<int, object> CachedProperties;
            protected readonly string FileName;
            protected readonly WdxPropertyProvider Provider;

            public WdxPropertyBag(WdxPropertyProvider provider, string fileName)
            {
                this.Provider = provider;
                this.FileName = fileName;
            }

            protected override VirtualPropertySet CreateAvailableSet()
            {
                return (this.ActualAvailableSet ?? this.Provider.GetRegisteredProperties());
            }

            public void Dispose()
            {
                this.Provider.StopGetValue(this.FileName);
            }

            private object GetProperty(int propertyId, bool delayed)
            {
                object obj2;
                Tuple<int, int> tuple;
                object obj3;
                if ((this.CachedProperties != null) && this.CachedProperties.TryGetValue(propertyId, out obj2))
                {
                    return obj2;
                }
                if (!(((this.ActualAvailableSet == null) || this.ActualAvailableSet[propertyId]) && this.Provider.PropertyFieldMap.TryGetValue(propertyId, out tuple)))
                {
                    return null;
                }
                try
                {
                    obj2 = this.Provider.GetValue(this.FileName, tuple.Item1, tuple.Item2, delayed);
                    if (obj2 == null)
                    {
                        if (this.ActualAvailableSet == null)
                        {
                            this.ActualAvailableSet = this.Provider.GetRegisteredProperties();
                        }
                        this.ActualAvailableSet[propertyId] = false;
                        base.ResetAvailableSet();
                    }
                    else
                    {
                        if ((obj2 == WdxFieldInfo.DelayedValue) || (obj2 == WdxFieldInfo.OnDemandValue))
                        {
                            return obj2;
                        }
                        if (this.CachedProperties == null)
                        {
                            this.CachedProperties = new Dictionary<int, object>();
                        }
                        this.CachedProperties.Add(propertyId, obj2);
                    }
                    obj3 = obj2;
                }
                catch (IOException)
                {
                    this.ActualAvailableSet = VirtualPropertySet.Empty;
                    base.ResetAvailableSet();
                    obj3 = null;
                }
                catch
                {
                    this.ActualAvailableSet = VirtualPropertySet.Empty;
                    base.ResetAvailableSet();
                    throw;
                }
                return obj3;
            }

            public override PropertyAvailability GetPropertyAvailability(int propertyId)
            {
                object property = this.GetProperty(propertyId, true);
                if (property == null)
                {
                    return PropertyAvailability.None;
                }
                if (property == WdxFieldInfo.DelayedValue)
                {
                    return PropertyAvailability.Slow;
                }
                if (property == WdxFieldInfo.OnDemandValue)
                {
                    return PropertyAvailability.OnDemand;
                }
                return PropertyAvailability.Normal;
            }

            public object this[int propertyId]
            {
                get
                {
                    return this.GetProperty(propertyId, false);
                }
            }
        }

        private class WdxSetPropertyBag : WdxPropertyProvider.WdxPropertyBag, IUpdateVirtualProperty, ISetVirtualProperty, IGetVirtualProperty
        {
            public WdxSetPropertyBag(WdxPropertyProvider provider, string fileName) : base(provider, fileName)
            {
            }

            public void BeginUpdate()
            {
                base.Provider.BeginUpdate(base.FileName);
            }

            public bool CanSetProperty(int propertyId)
            {
                Tuple<int, int> tuple;
                return (base.Provider.PropertyFieldMap.TryGetValue(propertyId, out tuple) && ((base.Provider.Fields[tuple.Item1].Flags & ContentFlag.contflags_edit) > 0));
            }

            public void EndUpdate()
            {
                int pendingUpdateCount = base.Provider.GetPendingUpdateCount(base.FileName);
                base.Provider.EndUpdate(base.FileName);
                if (pendingUpdateCount > 0)
                {
                    base.ActualAvailableSet = null;
                    base.ResetAvailableSet();
                }
            }

            public object this[int propertyId]
            {
                get
                {
                    return base[propertyId];
                }
                set
                {
                    Tuple<int, int> tuple;
                    if (!base.Provider.PropertyFieldMap.TryGetValue(propertyId, out tuple))
                    {
                        throw new NotSupportedException();
                    }
                    int pendingUpdateCount = base.Provider.GetPendingUpdateCount(base.FileName);
                    base.Provider.SetValue(base.FileName, tuple.Item1, tuple.Item2, value);
                    if (pendingUpdateCount < 0)
                    {
                        base.ActualAvailableSet = null;
                        base.ResetAvailableSet();
                    }
                }
            }
        }
    }
}

