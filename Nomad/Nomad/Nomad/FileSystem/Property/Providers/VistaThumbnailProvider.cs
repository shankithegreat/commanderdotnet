namespace Nomad.FileSystem.Property.Providers
{
    using Microsoft;
    using Microsoft.Shell;
    using Microsoft.Win32;
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [Version(1, 0, 1, 10)]
    public class VistaThumbnailProvider : ILocalFilePropertyProvider, ISimplePropertyProvider, IPropertyProvider
    {
        private static bool CanUseIcons;
        private static bool FolderThumbnails;
        private static Size MaxThumbnailSize;

        public IGetVirtualProperty AddProperties(FileSystemInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException();
            }
            if ((info is FileInfo) || (FolderThumbnails && (info is DirectoryInfo)))
            {
                return new VistaThumbnailBag(info.FullName);
            }
            return null;
        }

        public VirtualPropertySet GetRegisteredProperties()
        {
            return new VirtualPropertySet(new int[] { 0x15 });
        }

        public bool Register(Hashtable options)
        {
            MaxThumbnailSize = PropertyProviderManager.ReadOption<Size>(options, "maxThumbnailSize", new Size(0x60, 0x60));
            FolderThumbnails = PropertyProviderManager.ReadOption<bool>(options, "folderThumbnails", false);
            CanUseIcons = PropertyProviderManager.ReadOption<bool>(options, "canUseIcons", false);
            return OS.IsWinVista;
        }

        private class VistaThumbnailBag : CustomPropertyProvider, IGetVirtualProperty, IGetThumbnail
        {
            private string FFileName;
            private bool HasThumbnail = true;
            private WeakReference StoredThumbnail;
            private Size StoredThumbnailSize;

            public VistaThumbnailBag(string fileName)
            {
                this.FFileName = fileName;
            }

            protected override VirtualPropertySet CreateAvailableSet()
            {
                VirtualPropertySet set = new VirtualPropertySet();
                set[0x15] = this.HasThumbnail;
                return set;
            }

            public Image GetThumbnail(Size thumbSize)
            {
                if (!this.HasThumbnail)
                {
                    return null;
                }
                Image target = null;
                if ((this.StoredThumbnail != null) && this.StoredThumbnail.IsAlive)
                {
                    target = (Image) this.StoredThumbnail.Target;
                }
                if ((target == null) || (this.StoredThumbnailSize != thumbSize))
                {
                    try
                    {
                        IShellItem o = ShellItem.GetItem(this.FFileName);
                        try
                        {
                            IShellItemImageFactory factory = o as IShellItemImageFactory;
                            if (factory != null)
                            {
                                IntPtr ptr;
                                factory.GetImage(thumbSize, VistaThumbnailProvider.CanUseIcons ? SIIGBF.SIIGBF_RESIZETOFIT : SIIGBF.SIIGBF_THUMBNAILONLY, out ptr);
                                if (!(ptr != IntPtr.Zero))
                                {
                                    return target;
                                }
                                try
                                {
                                    target = ImageHelper.FromHbitmapWithAlpha(ptr);
                                    this.StoredThumbnail = new WeakReference(target);
                                    this.StoredThumbnailSize = thumbSize;
                                }
                                finally
                                {
                                    Windows.DeleteObject(ptr);
                                }
                            }
                        }
                        finally
                        {
                            Marshal.ReleaseComObject(o);
                        }
                    }
                    catch (Exception exception)
                    {
                        if (Marshal.GetHRForException(exception) != -2147175936)
                        {
                            PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                        }
                        this.HasThumbnail = false;
                        base.ResetAvailableSet();
                    }
                }
                return target;
            }

            public object this[int propertyId]
            {
                get
                {
                    if (propertyId == 0x15)
                    {
                        return this.GetThumbnail(VistaThumbnailProvider.MaxThumbnailSize);
                    }
                    return null;
                }
            }
        }
    }
}

