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
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;

    [Version(1, 0, 2, 10)]
    public class ThumbsDbThumbnailProvider : CustomExtPropertyProvider, ILocalFilePropertyProvider, ISimplePropertyProvider, IPropertyProvider
    {
        private static Regex ThumbnailExtRegex;
        private static Dictionary<string, WeakReference> ThumbsDbMap = new Dictionary<string, WeakReference>();

        public IGetVirtualProperty AddProperties(FileSystemInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException();
            }
            if (info is DirectoryInfo)
            {
                return new ThumbsDbThumbnailBag(info.FullName, "{A42CD7B6-E9B9-4D02-B7A6-288B71AD28BA}");
            }
            FileInfo info2 = info as FileInfo;
            if ((info2 != null) && ThumbnailExtRegex.IsMatch(info2.Extension))
            {
                return new ThumbsDbThumbnailBag(info2.FullName);
            }
            return null;
        }

        public VirtualPropertySet GetRegisteredProperties()
        {
            return new VirtualPropertySet(new int[] { 0x15 });
        }

        public bool Register(Hashtable options)
        {
            ThumbnailExtRegex = CustomExtPropertyProvider.InitializeExtRegex(ConfigurationManager.GetSection("propertyProviders/thumbsDbProvider") as ExtSection);
            if (ThumbnailExtRegex == null)
            {
                return false;
            }
            return true;
        }

        private class ThumbsDbThumbnailBag : CustomPropertyProvider, IGetVirtualProperty
        {
            private string FThumbKey;
            private string FThumbsDbPath;
            private bool HasThumbnail;

            public ThumbsDbThumbnailBag(string fileName) : this(Path.GetDirectoryName(fileName), Path.GetFileName(fileName))
            {
            }

            public ThumbsDbThumbnailBag(string thumbsDbFolder, string thumbKey)
            {
                this.HasThumbnail = true;
                this.FThumbsDbPath = Path.Combine(thumbsDbFolder, "Thumbs.db");
                this.FThumbKey = thumbKey;
            }

            protected override VirtualPropertySet CreateAvailableSet()
            {
                VirtualPropertySet set = new VirtualPropertySet();
                set[0x15] = this.HasThumbnail;
                return set;
            }

            public object this[int propertyId]
            {
                get
                {
                    if ((propertyId == 0x15) && this.HasThumbnail)
                    {
                        try
                        {
                            WeakReference reference;
                            ThumbsDb target = null;
                            if (ThumbsDbThumbnailProvider.ThumbsDbMap.TryGetValue(this.FThumbsDbPath, out reference) && reference.IsAlive)
                            {
                                target = (ThumbsDb) reference.Target;
                            }
                            else if (File.Exists(this.FThumbsDbPath))
                            {
                                target = new ThumbsDb(this.FThumbsDbPath);
                                ThumbsDbThumbnailProvider.ThumbsDbMap[this.FThumbsDbPath] = new WeakReference(target);
                            }
                            else
                            {
                                ThumbsDbThumbnailProvider.ThumbsDbMap.Remove(this.FThumbsDbPath);
                            }
                            if (target != null)
                            {
                                return target.GetThumbnail(this.FThumbKey);
                            }
                        }
                        catch (Exception exception)
                        {
                            PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                            this.HasThumbnail = false;
                            base.ResetAvailableSet();
                        }
                    }
                    return null;
                }
            }
        }
    }
}

