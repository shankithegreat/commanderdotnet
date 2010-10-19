namespace Nomad.FileSystem.Property.Providers
{
    using Nomad.Commons;
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    [Version(1, 0, 1, 8)]
    public class DescriptionPropertyProvider : ILocalFilePropertyProvider, ISimplePropertyProvider, IPropertyProvider
    {
        private static string[] DescriptionFileNameList = new string[] { "descript.ion" };
        private static bool DescriptionHidden = true;

        public IGetVirtualProperty AddProperties(FileSystemInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException();
            }
            return new DescriptionPropertyBag(info.FullName);
        }

        public VirtualPropertySet GetRegisteredProperties()
        {
            return new VirtualPropertySet(new int[] { 11 });
        }

        public bool Register(Hashtable options)
        {
            DescriptionHidden = PropertyProviderManager.ReadOption<bool>(options, "hidden", true);
            if (options != null)
            {
                string str = options["descriptionFileName"] as string;
                if (string.IsNullOrEmpty(str))
                {
                    return true;
                }
                List<string> list = new List<string>();
                foreach (string str2 in StringHelper.SplitString(str, new char[] { ',' }))
                {
                    string item = str2.Trim();
                    if (item != string.Empty)
                    {
                        list.Add(item);
                    }
                }
                if (list.Count > 0)
                {
                    DescriptionFileNameList = list.ToArray();
                }
            }
            return true;
        }

        private class DescriptionPropertyBag : CustomPropertyProvider, ISetVirtualProperty, IGetVirtualProperty
        {
            private static Dictionary<string, WeakReference> DescriptionMap = new Dictionary<string, WeakReference>();
            private string FDescription;
            private string FFileFolder;
            private string FFileName;

            public DescriptionPropertyBag(string fileName)
            {
                this.FFileFolder = Path.GetDirectoryName(fileName);
                this.FFileName = Path.GetFileName(fileName);
            }

            public bool CanSetProperty(int property)
            {
                return (property == 11);
            }

            protected override VirtualPropertySet CreateAvailableSet()
            {
                VirtualPropertySet set = new VirtualPropertySet();
                set[11] = (this.FDescription == null) || (GetDescription(this.FFileFolder, false) != null);
                return set;
            }

            private static DescriptionFileInfo GetDescription(string folder, bool create)
            {
                WeakReference reference = null;
                Dictionary<string, WeakReference> dictionary;
                lock ((dictionary = DescriptionMap))
                {
                    DescriptionMap.TryGetValue(folder, out reference);
                }
                if ((reference != null) && reference.IsAlive)
                {
                    return (DescriptionFileInfo) reference.Target;
                }
                DescriptionFileInfo target = null;
                foreach (string str in DescriptionPropertyProvider.DescriptionFileNameList)
                {
                    string path = Path.Combine(folder, str);
                    if (File.Exists(path))
                    {
                        target = new DescriptionFileInfo(path);
                        break;
                    }
                }
                if (target == null)
                {
                    if (!create)
                    {
                        return null;
                    }
                    target = new DescriptionFileInfo(Path.Combine(folder, DescriptionPropertyProvider.DescriptionFileNameList[0]));
                }
                lock ((dictionary = DescriptionMap))
                {
                    DescriptionMap[folder] = new WeakReference(target);
                }
                return target;
            }

            public object this[int property]
            {
                get
                {
                    if (property != 11)
                    {
                        return null;
                    }
                    if (this.FDescription == null)
                    {
                        try
                        {
                            DescriptionFileInfo description = GetDescription(this.FFileFolder, true);
                            this.FDescription = description[this.FFileName];
                            if (this.FDescription == null)
                            {
                                this.FDescription = string.Empty;
                                base.ResetAvailableSet();
                            }
                        }
                        catch (Exception exception)
                        {
                            PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                        }
                    }
                    return ((this.FDescription == string.Empty) ? null : this.FDescription);
                }
                set
                {
                    if (property != 11)
                    {
                        throw new InvalidOperationException("Cannot set properties other than description.");
                    }
                    string str = (string) value;
                    if (string.IsNullOrEmpty(str))
                    {
                        str = null;
                    }
                    DescriptionFileInfo description = GetDescription(this.FFileFolder, true);
                    description[this.FFileName] = str;
                    if (description.Count == 0)
                    {
                        if (File.Exists(description.DescriptionFilePath))
                        {
                            File.SetAttributes(description.DescriptionFilePath, FileAttributes.Normal);
                            File.Delete(description.DescriptionFilePath);
                        }
                    }
                    else
                    {
                        if (File.Exists(description.DescriptionFilePath))
                        {
                            File.SetAttributes(description.DescriptionFilePath, FileAttributes.Normal);
                        }
                        description.Save();
                        if (DescriptionPropertyProvider.DescriptionHidden)
                        {
                            File.SetAttributes(description.DescriptionFilePath, FileAttributes.Hidden);
                        }
                    }
                    this.FDescription = (str == null) ? string.Empty : str;
                    base.ResetAvailableSet();
                }
            }

            private class DescriptionFileInfo
            {
                public readonly string DescriptionFilePath;
                private static Regex DescriptionLineRegEx = new Regex("^\"(?<file>[^\"]*)\" (?<text>.*)|(?<file>[^ ]*) (?<text>.*)$", RegexOptions.Singleline | RegexOptions.Compiled);
                private List<string> DescriptionList;
                private Dictionary<string, int> DescriptionMap;
                private ReaderWriterLock Lock = new ReaderWriterLock();

                public DescriptionFileInfo(string filePath)
                {
                    this.DescriptionFilePath = filePath;
                }

                public void Load()
                {
                    this.Lock.AcquireWriterLock(-1);
                    try
                    {
                        this.DescriptionList = new List<string>();
                        if (File.Exists(this.DescriptionFilePath))
                        {
                            using (TextReader reader = new StreamReader(this.DescriptionFilePath, Encoding.Default, true))
                            {
                                string str;
                                while ((str = reader.ReadLine()) != null)
                                {
                                    this.DescriptionList.Add(str);
                                }
                            }
                        }
                    }
                    finally
                    {
                        this.Lock.ReleaseWriterLock();
                    }
                }

                private void RebuildDescriptionMap()
                {
                    this.Lock.AcquireWriterLock(-1);
                    try
                    {
                        this.DescriptionMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                        for (int i = 0; i < this.DescriptionList.Count; i++)
                        {
                            Match match = DescriptionLineRegEx.Match(this.DescriptionList[i]);
                            if (match.Success)
                            {
                                this.DescriptionMap[match.Groups["file"].Value] = i;
                            }
                        }
                    }
                    finally
                    {
                        this.Lock.ReleaseWriterLock();
                    }
                }

                public void Save()
                {
                    if (this.DescriptionList != null)
                    {
                        using (TextWriter writer = new StreamWriter(this.DescriptionFilePath, false, Encoding.Default))
                        {
                            foreach (string str in this.DescriptionList)
                            {
                                writer.WriteLine(str);
                            }
                        }
                    }
                }

                public int Count
                {
                    get
                    {
                        if (this.DescriptionList == null)
                        {
                            this.Load();
                        }
                        return this.DescriptionList.Count;
                    }
                }

                public string this[string name]
                {
                    get
                    {
                        string str;
                        if (this.DescriptionList == null)
                        {
                            this.Load();
                        }
                        if (this.DescriptionMap == null)
                        {
                            this.RebuildDescriptionMap();
                        }
                        this.Lock.AcquireReaderLock(-1);
                        try
                        {
                            int num;
                            if (!((this.DescriptionMap != null) && this.DescriptionMap.TryGetValue(name, out num)))
                            {
                                return null;
                            }
                            Match match = DescriptionLineRegEx.Match(this.DescriptionList[num]);
                            Debug.Assert(match.Success);
                            str = match.Groups["text"].Value;
                        }
                        finally
                        {
                            this.Lock.ReleaseReaderLock();
                        }
                        return str;
                    }
                    set
                    {
                        if (this.DescriptionList == null)
                        {
                            this.Load();
                        }
                        if (this.DescriptionMap == null)
                        {
                            this.RebuildDescriptionMap();
                        }
                        StringBuilder builder = null;
                        if (value != null)
                        {
                            builder = new StringBuilder();
                            if (name.IndexOf(' ') >= 0)
                            {
                                builder.Append('"');
                                builder.Append(name);
                                builder.Append('"');
                            }
                            else
                            {
                                builder.Append(name);
                            }
                            builder.Append(' ');
                            builder.Append(value);
                        }
                        this.Lock.AcquireReaderLock(-1);
                        try
                        {
                            int num;
                            if (this.DescriptionMap.TryGetValue(name, out num))
                            {
                                this.Lock.UpgradeToWriterLock(-1);
                                if (builder != null)
                                {
                                    this.DescriptionList[num] = builder.ToString();
                                }
                                else
                                {
                                    this.DescriptionList.RemoveAt(num);
                                    this.DescriptionMap = null;
                                }
                            }
                            else if (builder != null)
                            {
                                this.Lock.UpgradeToWriterLock(-1);
                                this.DescriptionList.Add(builder.ToString());
                                this.DescriptionMap.Add(name, this.DescriptionList.Count - 1);
                            }
                        }
                        finally
                        {
                            this.Lock.ReleaseLock();
                        }
                    }
                }
            }
        }
    }
}

