namespace Nomad.FileSystem.Archive.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public abstract class ArchiveFormatInfo
    {
        private bool FDisabled;
        private string[] FExtensionList;
        private bool FHideFormat;
        private int FInitCount;

        protected ArchiveFormatInfo()
        {
        }

        protected void BeginInit()
        {
            this.FInitCount++;
        }

        protected virtual void Changed()
        {
        }

        protected void EndInit()
        {
            if (this.FInitCount > 0)
            {
                this.FInitCount--;
            }
        }

        public abstract IEnumerable<ISimpleItem> OpenArchiveContent(Stream archiveStream, string archiveName);

        public abstract ArchiveFormatCapabilities Capabilities { get; }

        public abstract Guid ClassId { get; }

        public bool Disabled
        {
            get
            {
                return this.FDisabled;
            }
            set
            {
                if (this.FInitCount > 0)
                {
                    this.FDisabled = value;
                }
                else if (this.FDisabled != value)
                {
                    this.FDisabled = value;
                    this.Changed();
                }
            }
        }

        public string[] Extension
        {
            get
            {
                return this.FExtensionList;
            }
            set
            {
                if (this.FInitCount > 0)
                {
                    this.FExtensionList = value;
                }
                else
                {
                    bool flag = false;
                    if ((this.FExtensionList != null) && (value != null))
                    {
                        flag = this.FExtensionList.Length != value.Length;
                        if (!flag)
                        {
                            Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                            foreach (string str in this.FExtensionList)
                            {
                                dictionary.Add(str, dictionary.Count);
                            }
                            foreach (string str in value)
                            {
                                if (!dictionary.ContainsKey(str))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        bool flag2 = (this.FExtensionList == null) || (this.FExtensionList.Length == 0);
                        bool flag3 = (value == null) || (value.Length == 0);
                        flag = flag2 ^ flag3;
                    }
                    if (flag)
                    {
                        this.FExtensionList = value;
                        ArchiveFormatManager.ResetExtensionMap();
                        this.Changed();
                    }
                }
            }
        }

        public bool HideFormat
        {
            get
            {
                return this.FHideFormat;
            }
            set
            {
                if (this.FInitCount > 0)
                {
                    this.FHideFormat = value;
                }
                else if (this.FHideFormat != value)
                {
                    this.FHideFormat = value;
                    this.Changed();
                }
            }
        }

        public abstract string Name { get; }
    }
}

