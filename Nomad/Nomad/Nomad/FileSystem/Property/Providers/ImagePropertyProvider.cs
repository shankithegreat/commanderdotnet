namespace Nomad.FileSystem.Property.Providers
{
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Text;

    [Version(1, 0, 6, 0x1b)]
    public class ImagePropertyProvider : ILocalFilePropertyProvider, ISimplePropertyProvider, IPropertyProvider
    {
        private static Dictionary<string, TagCapability> ImageExtMap;
        private static bool KeepThumbnailSize;
        private static Size MaxThumbnailSize;
        private static int PropertyBPP;
        private static int PropertyColorSpace;
        private static int PropertyDPI;
        private static int PropertyFrameCount;
        private static int PropertyImageHeight = -1;
        private static int PropertyImageSize;
        private static int PropertyImageWidth = -1;
        private static Dictionary<PropertyTag, int> PropertyTagMap = new Dictionary<PropertyTag, int>();

        public IGetVirtualProperty AddProperties(FileSystemInfo info)
        {
            TagCapability capability;
            if (info == null)
            {
                throw new ArgumentNullException();
            }
            FileInfo fileInfo = info as FileInfo;
            if ((fileInfo != null) && ImageExtMap.TryGetValue(fileInfo.Extension, out capability))
            {
                return new ImagePropertyBag(fileInfo, capability);
            }
            return null;
        }

        public VirtualPropertySet GetRegisteredProperties()
        {
            VirtualPropertySet set = new VirtualPropertySet();
            foreach (int num in PropertyTagMap.Values)
            {
                set[num] = true;
            }
            set[11] = true;
            set[0x15] = true;
            set[PropertyImageWidth] = true;
            set[PropertyImageHeight] = true;
            set[PropertyImageSize] = true;
            set[PropertyFrameCount] = true;
            set[PropertyColorSpace] = true;
            set[PropertyBPP] = true;
            set[PropertyDPI] = true;
            return set;
        }

        public bool Register(Hashtable options)
        {
            if (options == null)
            {
                return false;
            }
            MaxThumbnailSize = PropertyProviderManager.ReadOption<Size>(options, "maxThumbnailSize", new Size(120, 120));
            KeepThumbnailSize = PropertyProviderManager.ReadOption<bool>(options, "keepThumbnailSize", true);
            List<PropertyTag> list = null;
            string str = options["propertyTags"] as string;
            if (str != null)
            {
                list = new List<PropertyTag>();
                foreach (string str2 in StringHelper.SplitString(str, new char[] { ',' }))
                {
                    try
                    {
                        list.Add((PropertyTag) Enum.Parse(typeof(PropertyTag), str2, true));
                    }
                    catch
                    {
                    }
                }
            }
            ImageExtMap = new Dictionary<string, TagCapability>(StringComparer.OrdinalIgnoreCase);
            foreach (ImageCodecInfo info in ImageCodecInfo.GetImageDecoders())
            {
                TagCapability size = TagCapability.Size;
                if (info.FormatID == ImageFormat.Gif.Guid)
                {
                    size |= TagCapability.Frames;
                }
                else if (info.FormatID == ImageFormat.Icon.Guid)
                {
                    size |= TagCapability.Frames;
                }
                else if (info.FormatID == ImageFormat.Jpeg.Guid)
                {
                    size |= TagCapability.Exif | TagCapability.Properties;
                }
                else if (info.FormatID == ImageFormat.Png.Guid)
                {
                    size |= TagCapability.Properties;
                }
                else if (info.FormatID == ImageFormat.Tiff.Guid)
                {
                    size |= TagCapability.Exif | TagCapability.Properties | TagCapability.Frames;
                }
                foreach (string str3 in StringHelper.SplitString(info.FilenameExtension, new char[] { ';' }))
                {
                    ImageExtMap.Add(Path.GetExtension(str3), size);
                }
            }
            int groupId = VirtualProperty.RegisterGroup("Image");
            if (list != null)
            {
                foreach (PropertyTag tag in list)
                {
                    Type type;
                    int length = -1;
                    TypeConverter converter = null;
                    PropertyTag tag2 = tag;
                    if (tag2 <= PropertyTag.ImageDescription)
                    {
                        switch (tag2)
                        {
                            case PropertyTag.ImageWidth:
                            case PropertyTag.ImageHeight:
                                length = 4;
                                goto Label_030A;

                            case PropertyTag.ImageDescription:
                            {
                                continue;
                            }
                        }
                        goto Label_030A;
                    }
                    if (tag2 != PropertyTag.SoftwareUsed)
                    {
                        if (tag2 == PropertyTag.ExifISOSpeed)
                        {
                            goto Label_0301;
                        }
                        goto Label_030A;
                    }
                    int num3 = VirtualProperty.RegisterGroup("Document");
                    int num4 = DefaultProperty.RegisterProperty("SoftwareUsed", num3, typeof(string), -1);
                    PropertyTagMap.Add(tag, num4);
                    continue;
                Label_0301:
                    converter = new ISOSpeedTypeConverter();
                Label_030A:
                    type = PropertyTagHelper.GetPropertyType(tag);
                    if (type == typeof(DateTime))
                    {
                        converter = DateTimeTypeConverter.Default;
                    }
                    int num5 = DefaultProperty.RegisterProperty(tag.ToString(), groupId, type, length, converter, 0);
                    switch (tag)
                    {
                        case PropertyTag.ImageWidth:
                            PropertyImageWidth = num5;
                            break;

                        case PropertyTag.ImageHeight:
                            PropertyImageHeight = num5;
                            break;
                    }
                    PropertyTagMap.Add(tag, num5);
                }
            }
            PropertyTagMap.Add(PropertyTag.ImageDescription, 11);
            if (PropertyImageWidth < 0)
            {
                PropertyImageWidth = DefaultProperty.RegisterProperty("ImageWidth", groupId, typeof(int), 4);
            }
            if (PropertyImageHeight < 0)
            {
                PropertyImageHeight = DefaultProperty.RegisterProperty("ImageHeight", groupId, typeof(int), 4);
            }
            PropertyImageSize = DefaultProperty.RegisterProperty("ImageSize", groupId, typeof(Size), -1, ImageSizeTypeConverter.Default, 0);
            PropertyFrameCount = DefaultProperty.RegisterProperty("FrameCount", groupId, typeof(int), 2);
            PropertyColorSpace = DefaultProperty.RegisterProperty("ColorSpace", groupId, typeof(ColorSpace), 5);
            PropertyBPP = DefaultProperty.RegisterProperty("BPP", groupId, typeof(int), 2);
            PropertyDPI = DefaultProperty.RegisterProperty("DPI", groupId, typeof(int), -1, DPITypeConverter.Default, 0);
            return true;
        }

        private class ImagePropertyBag : CustomPropertyProvider, ISetVirtualProperty, IGetVirtualProperty, IGetThumbnail
        {
            private FileInfo _FileInfo;
            private int BPP;
            private Size DPI;
            private int FrameCount;
            private ImagePropertyProvider.TagCapability HasCapabilities;
            private Dictionary<int, object> ImagePropertyList;
            private Size ImageSize;
            private ColorSpace Space;
            private WeakReference StoredThumbnail;
            private Size StoredThumbnailSize;
            private WeakReference StoredThumbnailStream;

            public ImagePropertyBag(FileInfo fileInfo, ImagePropertyProvider.TagCapability capabilities)
            {
                this._FileInfo = fileInfo;
                this.HasCapabilities = capabilities | ~(ImagePropertyProvider.TagCapability.Exif | ImagePropertyProvider.TagCapability.Properties | ImagePropertyProvider.TagCapability.Frames);
            }

            public bool CanSetProperty(int property)
            {
                return ((property == 11) && this.CheckCapability(ImagePropertyProvider.TagCapability.Properties));
            }

            private bool CheckCapability(ImagePropertyProvider.TagCapability capability)
            {
                return ((this.HasCapabilities & capability) == capability);
            }

            protected override VirtualPropertySet CreateAvailableSet()
            {
                VirtualPropertySet set = new VirtualPropertySet();
                set[11] = (this.HasCapabilities & ImagePropertyProvider.TagCapability.Properties) > 0;
                set[0x15] = (this.HasCapabilities & ImagePropertyProvider.TagCapability.Thumbnail) > 0;
                foreach (KeyValuePair<PropertyTag, int> pair in ImagePropertyProvider.PropertyTagMap)
                {
                    bool flag = false;
                    if (this.CheckCapability(ImagePropertyProvider.TagCapability.Dirty))
                    {
                        flag = this.CheckCapability(ImagePropertyProvider.TagCapability.Properties);
                        if (flag && pair.Key.ToString().StartsWith("Exif", StringComparison.OrdinalIgnoreCase))
                        {
                            flag = this.CheckCapability(ImagePropertyProvider.TagCapability.Exif);
                        }
                    }
                    else
                    {
                        flag = (this.ImagePropertyList != null) && this.ImagePropertyList.ContainsKey(pair.Value);
                    }
                    set[pair.Value] = flag;
                }
                if (this.CheckCapability(ImagePropertyProvider.TagCapability.Size))
                {
                    set[ImagePropertyProvider.PropertyImageWidth] = true;
                    set[ImagePropertyProvider.PropertyImageHeight] = true;
                    set[ImagePropertyProvider.PropertyImageSize] = true;
                }
                set[ImagePropertyProvider.PropertyFrameCount] = this.CheckCapability(ImagePropertyProvider.TagCapability.Frames);
                set[ImagePropertyProvider.PropertyColorSpace] = this.CheckCapability(ImagePropertyProvider.TagCapability.ColorSpace);
                set[ImagePropertyProvider.PropertyBPP] = this.CheckCapability(ImagePropertyProvider.TagCapability.BPP);
                set[ImagePropertyProvider.PropertyDPI] = this.CheckCapability(ImagePropertyProvider.TagCapability.DPI);
                return set;
            }

            public Image GetThumbnail(Size thumbSize)
            {
                if ((this.HasCapabilities & ImagePropertyProvider.TagCapability.Thumbnail) > 0)
                {
                    return this.ReadImageThumbnail(ref thumbSize);
                }
                return null;
            }

            private void ReadImageProperties()
            {
                this.HasCapabilities &= ImagePropertyProvider.TagCapability.Thumbnail;
                this.FrameCount = -1;
                this.BPP = -1;
                try
                {
                    PropertyItem[] propertyItems = null;
                    using (Stream stream = this._FileInfo.OpenRead())
                    {
                        using (Image image = Image.FromStream(stream, false, false))
                        {
                            propertyItems = image.PropertyItems;
                            this.ImageSize = image.Size;
                            this.HasCapabilities |= ImagePropertyProvider.TagCapability.Size;
                            this.BPP = Image.GetPixelFormatSize(image.PixelFormat);
                            this.HasCapabilities |= ImagePropertyProvider.TagCapability.BPP;
                            switch (image.PixelFormat)
                            {
                                case PixelFormat.Format16bppRgb555:
                                case PixelFormat.Format16bppRgb565:
                                case PixelFormat.Format24bppRgb:
                                case PixelFormat.Format32bppRgb:
                                case PixelFormat.Format16bppArgb1555:
                                case PixelFormat.Format32bppPArgb:
                                case PixelFormat.Format32bppArgb:
                                case PixelFormat.Format64bppArgb:
                                case PixelFormat.Format48bppRgb:
                                case PixelFormat.Format64bppPArgb:
                                    this.Space = ColorSpace.RGB;
                                    break;

                                case PixelFormat.Indexed:
                                case PixelFormat.Format4bppIndexed:
                                case PixelFormat.Format8bppIndexed:
                                case PixelFormat.Format1bppIndexed:
                                    this.Space = ColorSpace.Indexed;
                                    break;

                                case PixelFormat.Format16bppGrayScale:
                                    this.Space = ColorSpace.Grayscale;
                                    break;
                            }
                            if ((image.Flags & 0x10) > 0)
                            {
                                this.Space = ColorSpace.RGB;
                            }
                            else if ((image.Flags & 0x20) > 0)
                            {
                                this.Space = ColorSpace.CMYK;
                            }
                            else if ((image.Flags & 0x40) > 0)
                            {
                                this.Space = ColorSpace.Grayscale;
                            }
                            else if ((image.Flags & 0x80) > 0)
                            {
                                this.Space = ColorSpace.YCBCR;
                            }
                            else if ((image.Flags & 0x100) > 0)
                            {
                                this.Space = ColorSpace.YCCK;
                            }
                            if (image.RawFormat == ImageFormat.Gif)
                            {
                                this.Space = ColorSpace.Indexed;
                            }
                            this.HasCapabilities |= ImagePropertyProvider.TagCapability.ColorSpace;
                            if ((image.Flags & 0x1000) > 0)
                            {
                                this.DPI = new Size((int) image.HorizontalResolution, (int) image.VerticalResolution);
                                this.HasCapabilities |= ImagePropertyProvider.TagCapability.DPI;
                            }
                            foreach (Guid guid in image.FrameDimensionsList)
                            {
                                if (guid == FrameDimension.Page.Guid)
                                {
                                    this.FrameCount = image.GetFrameCount(FrameDimension.Page);
                                }
                                else if (guid == FrameDimension.Resolution.Guid)
                                {
                                    this.FrameCount = image.GetFrameCount(FrameDimension.Resolution);
                                }
                                else if (guid == FrameDimension.Time.Guid)
                                {
                                    this.FrameCount = image.GetFrameCount(FrameDimension.Time);
                                }
                                if (this.FrameCount > 1)
                                {
                                    goto Label_036D;
                                }
                            }
                        }
                    }
                Label_036D:
                    if (this.FrameCount > 0)
                    {
                        this.HasCapabilities |= ImagePropertyProvider.TagCapability.Frames;
                    }
                    if (propertyItems.Length > 0)
                    {
                        this.ImagePropertyList = new Dictionary<int, object>();
                        foreach (PropertyItem item in propertyItems)
                        {
                            int num;
                            if (ImagePropertyProvider.PropertyTagMap.TryGetValue((PropertyTag) item.Id, out num))
                            {
                                this.ImagePropertyList.Add(num, item);
                                this.HasCapabilities |= ImagePropertyProvider.TagCapability.Properties;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                }
            }

            private Image ReadImageThumbnail(ref Size maxThumbnailSize)
            {
                Image target = null;
                Stream stream = null;
                Image image = null;
                if ((this.StoredThumbnail != null) && this.StoredThumbnail.IsAlive)
                {
                    target = (Image) this.StoredThumbnail.Target;
                }
                if ((target == null) || (this.StoredThumbnailSize != maxThumbnailSize))
                {
                    try
                    {
                        stream = this._FileInfo.OpenRead();
                        image = Image.FromStream(stream, true);
                        if (((image.Width <= maxThumbnailSize.Width) && (image.Height <= maxThumbnailSize.Height)) && ImageAnimator.CanAnimate(image))
                        {
                            this.StoredThumbnailStream = new WeakReference(stream);
                            target = image;
                        }
                        else
                        {
                            try
                            {
                                using (MemoryStream stream2 = new MemoryStream(image.GetPropertyItem(0x501b).Value))
                                {
                                    using (Image image3 = Image.FromStream(stream2))
                                    {
                                        if (ImagePropertyProvider.KeepThumbnailSize || ((image3.Width <= maxThumbnailSize.Width) && (image3.Height <= maxThumbnailSize.Height)))
                                        {
                                            target = new Bitmap(image3);
                                        }
                                        else
                                        {
                                            target = new Bitmap(image3, ImageHelper.GetThumbnailSize(image3.Size, maxThumbnailSize));
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                            if (target == null)
                            {
                                if ((image.Width <= maxThumbnailSize.Width) && (image.Height <= maxThumbnailSize.Height))
                                {
                                    target = new Bitmap(image);
                                }
                                else
                                {
                                    target = new Bitmap(image, ImageHelper.GetThumbnailSize(image.Size, maxThumbnailSize));
                                }
                            }
                        }
                        RotateFlipType rotateFlipType = (RotateFlipType) Convert.ToInt32(PropertyTagHelper.GetPropertyValue(image, PropertyTag.Orientation));
                        if (rotateFlipType != RotateFlipType.RotateNoneFlipNone)
                        {
                            target.RotateFlip(rotateFlipType);
                        }
                        this.StoredThumbnail = new WeakReference(target);
                        this.StoredThumbnailSize = maxThumbnailSize;
                    }
                    catch (Exception exception)
                    {
                        PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                        this.HasCapabilities &= ~ImagePropertyProvider.TagCapability.Thumbnail;
                        base.ResetAvailableSet();
                    }
                    finally
                    {
                        if ((stream != null) && (image != target))
                        {
                            stream.Dispose();
                        }
                        if ((image != null) && (image != target))
                        {
                            image.Dispose();
                        }
                    }
                }
                return target;
            }

            private void SetImageDescription(string description)
            {
                string str;
                using (Stream stream = this._FileInfo.OpenRead())
                {
                    using (Image image = Image.FromStream(stream, true, true))
                    {
                        if (image.PropertyItems.Length == 0)
                        {
                            return;
                        }
                        PropertyItem propitem = image.PropertyItems[0];
                        propitem.Id = 270;
                        propitem.Type = 2;
                        propitem.Value = Encoding.ASCII.GetBytes(description + '\0');
                        propitem.Len = propitem.Value.Length;
                        image.SetPropertyItem(propitem);
                        str = Path.Combine(this._FileInfo.DirectoryName, StringHelper.GuidToCompactString(Guid.NewGuid()) + this._FileInfo.Extension);
                        image.Save(str, image.RawFormat);
                    }
                }
                try
                {
                    File.SetAttributes(str, this._FileInfo.Attributes);
                    File.SetAttributes(this._FileInfo.FullName, FileAttributes.Normal);
                    File.Delete(this._FileInfo.FullName);
                }
                catch (IOException)
                {
                    try
                    {
                        File.SetAttributes(str, FileAttributes.Normal);
                        File.Delete(str);
                    }
                    catch
                    {
                    }
                    throw;
                }
                File.Move(str, this._FileInfo.FullName);
            }

            public object this[int property]
            {
                get
                {
                    object propertyValue;
                    if ((property == 0x15) && ((this.HasCapabilities & ImagePropertyProvider.TagCapability.Thumbnail) > 0))
                    {
                        return this.ReadImageThumbnail(ref ImagePropertyProvider.MaxThumbnailSize);
                    }
                    if (this.CheckCapability(ImagePropertyProvider.TagCapability.Dirty))
                    {
                        this.ReadImageProperties();
                        base.ResetAvailableSet();
                    }
                    Debug.Assert(!this.CheckCapability(ImagePropertyProvider.TagCapability.Dirty));
                    if ((this.ImagePropertyList != null) && this.ImagePropertyList.TryGetValue(property, out propertyValue))
                    {
                        PropertyItem item = propertyValue as PropertyItem;
                        if (item != null)
                        {
                            propertyValue = PropertyTagHelper.GetPropertyValue(item);
                            this.ImagePropertyList[property] = propertyValue;
                        }
                        return propertyValue;
                    }
                    if ((property == ImagePropertyProvider.PropertyImageWidth) && this.CheckCapability(ImagePropertyProvider.TagCapability.Size))
                    {
                        return this.ImageSize.Width;
                    }
                    if ((property == ImagePropertyProvider.PropertyImageHeight) && this.CheckCapability(ImagePropertyProvider.TagCapability.Size))
                    {
                        return this.ImageSize.Height;
                    }
                    if ((property == ImagePropertyProvider.PropertyImageSize) && this.CheckCapability(ImagePropertyProvider.TagCapability.Size))
                    {
                        return this.ImageSize;
                    }
                    if ((property == ImagePropertyProvider.PropertyFrameCount) && this.CheckCapability(ImagePropertyProvider.TagCapability.Frames))
                    {
                        return this.FrameCount;
                    }
                    if ((property == ImagePropertyProvider.PropertyColorSpace) && this.CheckCapability(ImagePropertyProvider.TagCapability.ColorSpace))
                    {
                        return this.Space;
                    }
                    if ((property == ImagePropertyProvider.PropertyBPP) && this.CheckCapability(ImagePropertyProvider.TagCapability.BPP))
                    {
                        return this.BPP;
                    }
                    if ((property == ImagePropertyProvider.PropertyDPI) && this.CheckCapability(ImagePropertyProvider.TagCapability.DPI))
                    {
                        return this.DPI;
                    }
                    return null;
                }
                set
                {
                    if (property != 11)
                    {
                        throw new InvalidOperationException("Cannot set properties other than description.");
                    }
                    this.SetImageDescription((string) value);
                }
            }

            [Flags]
            private enum ImageFlags
            {
                Caching = 0x20000,
                ColorSpaceCMYK = 0x20,
                ColorSpaceGRAY = 0x40,
                ColorSpaceRGB = 0x10,
                ColorSpaceYCBCR = 0x80,
                ColorSpaceYCCK = 0x100,
                HasAlpha = 2,
                HasRealDPI = 0x1000,
                HasRealPixelSize = 0x2000,
                HasTranslucent = 4,
                None = 0,
                PartiallyScalable = 8,
                ReadOnly = 0x10000,
                Scalable = 1
            }
        }

        [Flags]
        private enum TagCapability
        {
            BPP = 0x10,
            ColorSpace = 8,
            Dirty = 0x8000,
            DPI = 0x20,
            Exif = 0x80,
            Frames = 2,
            Properties = 4,
            Size = 1,
            Thumbnail = 0x40
        }
    }
}

