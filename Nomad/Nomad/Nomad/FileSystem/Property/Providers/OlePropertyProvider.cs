namespace Nomad.FileSystem.Property.Providers
{
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;

    [Version(1, 0, 2, 8)]
    public class OlePropertyProvider : CustomExtPropertyProvider, ILocalFilePropertyProvider, ISimplePropertyProvider, IPropertyProvider
    {
        private static Regex OleExtRegex;
        private static Dictionary<int, int> PropertyMap;

        public IGetVirtualProperty AddProperties(FileSystemInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException();
            }
            FileInfo fileInfo = info as FileInfo;
            if ((fileInfo != null) && OleExtRegex.IsMatch(fileInfo.Extension))
            {
                return new OlePropertyBag(fileInfo);
            }
            return null;
        }

        public VirtualPropertySet GetRegisteredProperties()
        {
            VirtualPropertySet set = new VirtualPropertySet(new int[] { 0x15 });
            foreach (int num in PropertyMap.Keys)
            {
                set[num] = true;
            }
            return set;
        }

        public bool Register(Hashtable options)
        {
            OleExtRegex = CustomExtPropertyProvider.InitializeExtRegex(ConfigurationManager.GetSection("propertyProviders/oleProvider") as ExtSection);
            if (OleExtRegex == null)
            {
                return false;
            }
            int groupId = VirtualProperty.RegisterGroup("Document");
            int key = DefaultProperty.RegisterProperty("DocumentTitle", groupId, typeof(string), -1);
            int num3 = DefaultProperty.RegisterProperty("Subject", groupId, typeof(string), -1);
            int num4 = DefaultProperty.RegisterProperty("Author", groupId, typeof(string), -1);
            int num5 = DefaultProperty.RegisterProperty("Keywords", groupId, typeof(string), -1);
            int num6 = DefaultProperty.RegisterProperty("Template", groupId, typeof(string), -1);
            int num7 = DefaultProperty.RegisterProperty("LastAuthor", groupId, typeof(string), -1);
            int num8 = DefaultProperty.RegisterProperty("PageCount", groupId, typeof(int), 3);
            int num9 = DefaultProperty.RegisterProperty("WordCount", groupId, typeof(int), 5);
            int num10 = DefaultProperty.RegisterProperty("CharCount", groupId, typeof(int), -1);
            int num11 = DefaultProperty.RegisterProperty("SoftwareUsed", groupId, typeof(string), -1);
            PropertyMap = new Dictionary<int, int>(15);
            PropertyMap.Add(11, 6);
            PropertyMap.Add(key, 2);
            PropertyMap.Add(num3, 3);
            PropertyMap.Add(num4, 4);
            PropertyMap.Add(num5, 5);
            PropertyMap.Add(num6, 7);
            PropertyMap.Add(num7, 8);
            PropertyMap.Add(num8, 14);
            PropertyMap.Add(num9, 15);
            PropertyMap.Add(num10, 0x10);
            PropertyMap.Add(num11, 0x12);
            return true;
        }

        private class OlePropertyBag : CustomPropertyProvider, IGetVirtualProperty
        {
            private FileInfo _FileInfo;
            private bool HasSummary = true;
            private bool HasThumbnail = true;
            private SummaryInformationSet Summary;
            private WeakReference Thumbnail;

            public OlePropertyBag(FileInfo fileInfo)
            {
                this._FileInfo = fileInfo;
            }

            protected override VirtualPropertySet CreateAvailableSet()
            {
                VirtualPropertySet set = new VirtualPropertySet();
                set[0x15] = this.HasThumbnail;
                if (this.HasSummary)
                {
                    if (this.Summary == null)
                    {
                        foreach (int num in OlePropertyProvider.PropertyMap.Keys)
                        {
                            set[num] = true;
                        }
                        return set;
                    }
                    foreach (KeyValuePair<int, int> pair in OlePropertyProvider.PropertyMap)
                    {
                        set[pair.Key] = this.Summary.ContainsProperty(pair.Value);
                    }
                }
                return set;
            }

            private void ReadSummary()
            {
                if (this.HasSummary && (!this.HasSummary || (this.Summary == null)))
                {
                    try
                    {
                        using (CompoundFile file = new CompoundFile(this._FileInfo.OpenRead()))
                        {
                            CompoundStreamEntry entry = file.Root["\x0005SummaryInformation"] as CompoundStreamEntry;
                            if (entry != null)
                            {
                                using (Stream stream = entry.OpenRead())
                                {
                                    PropertyStorage storage = new PropertyStorage(stream);
                                    this.Summary = storage.GetSection(SummaryInformationSet.FMTID_SummaryInformation) as SummaryInformationSet;
                                    if (this.Summary != null)
                                    {
                                        foreach (int num in OlePropertyProvider.PropertyMap.Values)
                                        {
                                            this.Summary.CacheProperty(num);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                    }
                    if (this.Summary == null)
                    {
                        this.HasSummary = false;
                        base.ResetAvailableSet();
                    }
                }
            }

            private Image ReadThumbnail()
            {
                if (!(this.HasSummary && this.HasThumbnail))
                {
                    return null;
                }
                if ((this.Thumbnail != null) && this.Thumbnail.IsAlive)
                {
                    return (Image) this.Thumbnail.Target;
                }
                Image target = null;
                try
                {
                    using (CompoundFile file = new CompoundFile(this._FileInfo.OpenRead()))
                    {
                        CompoundStreamEntry entry = file.Root["\x0005SummaryInformation"] as CompoundStreamEntry;
                        if (entry != null)
                        {
                            using (Stream stream = entry.OpenRead())
                            {
                                PropertyStorage storage = new PropertyStorage(stream);
                                this.Summary = storage.GetSection(SummaryInformationSet.FMTID_SummaryInformation) as SummaryInformationSet;
                                if (this.Summary != null)
                                {
                                    foreach (int num in OlePropertyProvider.PropertyMap.Values)
                                    {
                                        this.Summary.CacheProperty(num);
                                    }
                                    target = this.Summary.Thumbnail;
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                }
                if (this.Summary == null)
                {
                    this.HasSummary = false;
                }
                if (target == null)
                {
                    this.HasThumbnail = false;
                }
                else
                {
                    this.Thumbnail = new WeakReference(target);
                }
                if (!(this.HasSummary && this.HasThumbnail))
                {
                    base.ResetAvailableSet();
                }
                return target;
            }

            public object this[int property]
            {
                get
                {
                    if (this.HasSummary)
                    {
                        int num;
                        object obj2;
                        if (property == 0x15)
                        {
                            return this.ReadThumbnail();
                        }
                        this.ReadSummary();
                        if (((this.Summary != null) && OlePropertyProvider.PropertyMap.TryGetValue(property, out num)) && this.Summary.TryGetValue(num, out obj2))
                        {
                            return obj2;
                        }
                    }
                    return null;
                }
            }
        }
    }
}

