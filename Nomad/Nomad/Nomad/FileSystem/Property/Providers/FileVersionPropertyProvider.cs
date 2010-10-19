namespace Nomad.FileSystem.Property.Providers
{
    using Nomad.Commons;
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;

    [Version(1, 0, 2, 0x12)]
    public class FileVersionPropertyProvider : CustomExtPropertyProvider, ILocalFilePropertyProvider, ISimplePropertyProvider, IPropertyProvider
    {
        private static Regex FileVersionExtRegex;
        private static int PropertyCompanyName;
        private static int PropertyProductName;
        private static int PropertyProductVersion;

        public IGetVirtualProperty AddProperties(FileSystemInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException();
            }
            FileInfo info2 = info as FileInfo;
            if ((info2 != null) && FileVersionExtRegex.IsMatch(info2.Extension))
            {
                return new FileVersionPropertyBag(info2.FullName);
            }
            return null;
        }

        public VirtualPropertySet GetRegisteredProperties()
        {
            return new VirtualPropertySet(new int[] { 0x12, PropertyProductVersion, 0x13, PropertyProductName, PropertyCompanyName });
        }

        public bool Register(Hashtable options)
        {
            FileVersionExtRegex = CustomExtPropertyProvider.InitializeExtRegex(ConfigurationManager.GetSection("propertyProviders/fileVersionProvider") as ExtSection);
            if (FileVersionExtRegex == null)
            {
                return false;
            }
            int groupId = VirtualProperty.RegisterGroup("FileVersion");
            PropertyProductVersion = DefaultProperty.RegisterProperty("ProductVersion", groupId, typeof(Version), -1);
            PropertyProductName = DefaultProperty.RegisterProperty("ProductName", groupId, typeof(string), -1);
            PropertyCompanyName = DefaultProperty.RegisterProperty("CompanyName", groupId, typeof(string), -1);
            return true;
        }

        private class FileVersionPropertyBag : CustomPropertyProvider, IGetVirtualProperty
        {
            private string FFileName;
            private FileVersionInfo FVersionInfo;

            public FileVersionPropertyBag(string fileName)
            {
                this.FFileName = fileName;
            }

            private string ConvertStringValue(string value)
            {
                if (value == null)
                {
                    return value;
                }
                return value.Trim(new char[] { ' ', '\t', '\r', '\n' });
            }

            protected override VirtualPropertySet CreateAvailableSet()
            {
                VirtualPropertySet set = new VirtualPropertySet();
                if ((this.FVersionInfo == null) && (this.FFileName != null))
                {
                    set[11] = true;
                    set[0x12] = true;
                    set[FileVersionPropertyProvider.PropertyProductVersion] = true;
                    set[0x13] = true;
                    set[FileVersionPropertyProvider.PropertyProductName] = true;
                    set[FileVersionPropertyProvider.PropertyCompanyName] = true;
                    return set;
                }
                FileVersionInfo versionInfo = this.GetVersionInfo();
                if (versionInfo != null)
                {
                    set[11] = !string.IsNullOrEmpty(versionInfo.FileDescription);
                    set[0x12] = versionInfo.FileVersion != null;
                    set[FileVersionPropertyProvider.PropertyProductVersion] = versionInfo.ProductVersion != null;
                    set[0x13] = !string.IsNullOrEmpty(versionInfo.LegalCopyright);
                    set[FileVersionPropertyProvider.PropertyProductName] = !string.IsNullOrEmpty(versionInfo.ProductName);
                    set[FileVersionPropertyProvider.PropertyCompanyName] = !string.IsNullOrEmpty(versionInfo.CompanyName);
                }
                return set;
            }

            private FileVersionInfo GetVersionInfo()
            {
                if ((this.FVersionInfo == null) && (this.FFileName != null))
                {
                    try
                    {
                        this.FVersionInfo = FileVersionInfo.GetVersionInfo(this.FFileName);
                    }
                    catch (Exception exception)
                    {
                        PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                    }
                    this.FFileName = null;
                    base.ResetAvailableSet();
                }
                return this.FVersionInfo;
            }

            public object this[int property]
            {
                get
                {
                    FileVersionInfo versionInfo = this.GetVersionInfo();
                    if (versionInfo != null)
                    {
                        if (property == 11)
                        {
                            return this.ConvertStringValue(versionInfo.FileDescription);
                        }
                        if (property == 0x12)
                        {
                            return new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart);
                        }
                        if (property == FileVersionPropertyProvider.PropertyProductVersion)
                        {
                            return new Version(versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart, versionInfo.ProductPrivatePart);
                        }
                        if (property == 0x13)
                        {
                            return this.ConvertStringValue(versionInfo.LegalCopyright);
                        }
                        if (property == FileVersionPropertyProvider.PropertyProductName)
                        {
                            return this.ConvertStringValue(versionInfo.ProductName);
                        }
                        if (property == FileVersionPropertyProvider.PropertyCompanyName)
                        {
                            return this.ConvertStringValue(versionInfo.CompanyName);
                        }
                    }
                    return null;
                }
            }
        }
    }
}

