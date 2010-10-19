namespace Nomad.FileSystem.Property.Providers
{
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    [Version(1, 0, 3, 12)]
    public class PsdPropertyProvider : ILocalFilePropertyProvider, ISimplePropertyProvider, IPropertyProvider
    {
        private static int PropertyBPP;
        private static int PropertyColorSpace;
        private static int PropertyDPI;
        private static int PropertyImageHeight;
        private static int PropertyImageSize;
        private static int PropertyImageWidth;

        public IGetVirtualProperty AddProperties(FileSystemInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException();
            }
            FileInfo fileInfo = info as FileInfo;
            if ((fileInfo != null) && fileInfo.Extension.Equals(".psd", StringComparison.OrdinalIgnoreCase))
            {
                return new PsdPropertyBag(fileInfo);
            }
            return null;
        }

        public VirtualPropertySet GetRegisteredProperties()
        {
            return new VirtualPropertySet(new int[] { 0x15, PropertyImageWidth, PropertyImageHeight, PropertyImageSize, PropertyColorSpace, PropertyBPP, PropertyDPI });
        }

        public bool Register(Hashtable options)
        {
            int groupId = VirtualProperty.RegisterGroup("Image");
            PropertyImageWidth = DefaultProperty.RegisterProperty("ImageWidth", groupId, typeof(int), 4);
            PropertyImageHeight = DefaultProperty.RegisterProperty("ImageHeight", groupId, typeof(int), 4);
            PropertyImageSize = DefaultProperty.RegisterProperty("ImageSize", groupId, typeof(Size), -1, ImageSizeTypeConverter.Default, 0);
            PropertyColorSpace = DefaultProperty.RegisterProperty("ColorSpace", groupId, typeof(ColorSpace), 5);
            PropertyBPP = DefaultProperty.RegisterProperty("BPP", groupId, typeof(int), 2);
            PropertyDPI = DefaultProperty.RegisterProperty("DPI", groupId, typeof(int), -1, DPITypeConverter.Default, 0);
            return true;
        }

        private class PsdPropertyBag : CustomPropertyProvider, IGetVirtualProperty
        {
            private FileInfo _FileInfo;
            private int BPP;
            private Size DPI;
            private PsdCapability HasCapability;
            private Size ImageSize;
            private ColorSpace Space;
            private WeakReference StoredThumbnail;
            private int ThumbnailStreamSize;
            private long ThumbnailStreamStart;

            public PsdPropertyBag(FileInfo fileInfo)
            {
                this._FileInfo = fileInfo;
                this.HasCapability = PsdCapability.Dirty | PsdCapability.Thumbnail | PsdCapability.DPI | PsdCapability.BPP | PsdCapability.ColorSpace | PsdCapability.Size;
            }

            private bool CheckCapability(PsdCapability capability)
            {
                return ((this.HasCapability & capability) == capability);
            }

            protected override VirtualPropertySet CreateAvailableSet()
            {
                VirtualPropertySet set = new VirtualPropertySet();
                set[0x15] = this.CheckCapability(PsdCapability.Thumbnail);
                if (this.CheckCapability(PsdCapability.Size))
                {
                    set[PsdPropertyProvider.PropertyImageWidth] = true;
                    set[PsdPropertyProvider.PropertyImageHeight] = true;
                    set[PsdPropertyProvider.PropertyImageSize] = true;
                }
                set[PsdPropertyProvider.PropertyColorSpace] = this.CheckCapability(PsdCapability.ColorSpace);
                set[PsdPropertyProvider.PropertyBPP] = this.CheckCapability(PsdCapability.BPP);
                set[PsdPropertyProvider.PropertyDPI] = this.CheckCapability(PsdCapability.DPI);
                return set;
            }

            private void ReadPsd()
            {
                this.HasCapability = 0;
                try
                {
                    using (Stream stream = this._FileInfo.OpenRead())
                    {
                        PSD_HEADER psd_header = ByteArrayHelper.ReadStructureFromStream<PSD_HEADER>(stream);
                        if (Encoding.ASCII.GetString(psd_header.Signature) != "8BPS")
                        {
                            throw new InvalidDataException();
                        }
                        this.ImageSize = new Size(Swap(psd_header.Columns), Swap(psd_header.Rows));
                        this.BPP = Swap(psd_header.Depth) * Swap(psd_header.Channels);
                        switch (Swap(psd_header.Mode))
                        {
                            case 0:
                            case 2:
                                this.Space = ColorSpace.Indexed;
                                break;

                            case 1:
                                this.Space = ColorSpace.Grayscale;
                                break;

                            case 3:
                                this.Space = ColorSpace.RGB;
                                break;

                            case 4:
                                this.Space = ColorSpace.CMYK;
                                break;
                        }
                        this.HasCapability |= PsdCapability.BPP | PsdCapability.ColorSpace | PsdCapability.Size;
                        using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII))
                        {
                            int num4;
                            int num = Swap(reader.ReadInt32());
                            stream.Seek((long) num, SeekOrigin.Current);
                            for (num = Swap(reader.ReadInt32()); num > 0; num -= num4 + Math.Sign((int) (num4 % 2)))
                            {
                                string str = new string(reader.ReadChars(4));
                                num -= 4;
                                if (str != "8BIM")
                                {
                                    goto Label_02E9;
                                }
                                ushort num2 = Swap(reader.ReadUInt16());
                                num -= 2;
                                byte count = reader.ReadByte();
                                if (count > 0)
                                {
                                    reader.ReadChars(count);
                                }
                                else
                                {
                                    reader.ReadByte();
                                }
                                num -= Math.Max(2, 1 + count);
                                num4 = Swap(reader.ReadInt32());
                                int num5 = 0;
                                switch (num2)
                                {
                                    case 0x3ed:
                                    {
                                        RESOURCE_RESOLUTION structure = ByteArrayHelper.ReadStructureFromStream<RESOURCE_RESOLUTION>(stream);
                                        this.DPI = new Size(Swap(structure.hRes), Swap(structure.vRes));
                                        this.HasCapability |= PsdCapability.DPI;
                                        num5 = Marshal.SizeOf(structure);
                                        break;
                                    }
                                    case 0x40c:
                                    {
                                        RESOURCE_THUMBNAIL resource_thumbnail = ByteArrayHelper.ReadStructureFromStream<RESOURCE_THUMBNAIL>(stream);
                                        if (Swap(resource_thumbnail.nFormat) == 1)
                                        {
                                            this.ThumbnailStreamStart = stream.Position;
                                            this.ThumbnailStreamSize = Swap(resource_thumbnail.nCompressedSize);
                                            this.HasCapability |= PsdCapability.Thumbnail;
                                        }
                                        num5 = Marshal.SizeOf(resource_thumbnail);
                                        break;
                                    }
                                }
                                Debug.Assert(num5 <= num4);
                                stream.Seek((long) ((num4 + Math.Sign((int) (num4 % 2))) - num5), SeekOrigin.Current);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                }
            Label_02E9:
                if (!this.CheckCapability(PsdCapability.Thumbnail))
                {
                    this._FileInfo = null;
                }
            }

            private Image ReadThumbnail()
            {
                try
                {
                    using (Stream stream = this._FileInfo.OpenRead())
                    {
                        stream.Position = this.ThumbnailStreamStart;
                        using (Stream stream2 = new SubStream(stream, (long) this.ThumbnailStreamSize))
                        {
                            using (Image image = Image.FromStream(stream2))
                            {
                                return new Bitmap(image);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                    this.HasCapability &= ~PsdCapability.Thumbnail;
                    this._FileInfo = null;
                    base.ResetAvailableSet();
                }
                return null;
            }

            private static short Swap(short value)
            {
                return (short) Swap((ushort) value);
            }

            private static int Swap(int value)
            {
                return (int) Swap((uint) value);
            }

            private static ushort Swap(ushort value)
            {
                return (ushort) ((value << 8) | (value >> 8));
            }

            private static uint Swap(uint value)
            {
                return (uint) (((((value & 0xff) << 8) | ((value & 0xff00) >> 8)) << 0x10) | ((((value & 0xff0000) << 8) | ((value & -16777216) >> 8)) >> 0x10));
            }

            public object this[int property]
            {
                get
                {
                    if (this.CheckCapability(PsdCapability.Dirty))
                    {
                        this.ReadPsd();
                        base.ResetAvailableSet();
                    }
                    Debug.Assert(!this.CheckCapability(PsdCapability.Dirty));
                    if ((property == 0x15) && this.CheckCapability(PsdCapability.Thumbnail))
                    {
                        if ((this.StoredThumbnail != null) && this.StoredThumbnail.IsAlive)
                        {
                            return (Image) this.StoredThumbnail.Target;
                        }
                        Image target = this.ReadThumbnail();
                        if (target != null)
                        {
                            this.StoredThumbnail = new WeakReference(target);
                        }
                        return target;
                    }
                    if ((property == PsdPropertyProvider.PropertyImageWidth) && this.CheckCapability(PsdCapability.Size))
                    {
                        return this.ImageSize.Width;
                    }
                    if ((property == PsdPropertyProvider.PropertyImageHeight) && this.CheckCapability(PsdCapability.Size))
                    {
                        return this.ImageSize.Height;
                    }
                    if ((property == PsdPropertyProvider.PropertyImageSize) && this.CheckCapability(PsdCapability.Size))
                    {
                        return this.ImageSize;
                    }
                    if ((property == PsdPropertyProvider.PropertyColorSpace) && this.CheckCapability(PsdCapability.ColorSpace))
                    {
                        return this.Space;
                    }
                    if ((property == PsdPropertyProvider.PropertyBPP) && this.CheckCapability(PsdCapability.BPP))
                    {
                        return this.BPP;
                    }
                    if ((property == PsdPropertyProvider.PropertyDPI) && this.CheckCapability(PsdCapability.DPI))
                    {
                        return this.DPI;
                    }
                    return null;
                }
            }

            [StructLayout(LayoutKind.Sequential, Pack=2)]
            private struct PSD_HEADER
            {
                [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
                public byte[] Signature;
                public ushort Version;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst=6)]
                public byte[] Reserved;
                public ushort Channels;
                public int Rows;
                public int Columns;
                public ushort Depth;
                public ushort Mode;
            }

            [Flags]
            private enum PsdCapability
            {
                BPP = 4,
                ColorSpace = 2,
                Dirty = 0x8000,
                DPI = 8,
                Size = 1,
                Thumbnail = 0x10
            }

            [StructLayout(LayoutKind.Sequential, Pack=2)]
            public struct RESOURCE_RESOLUTION
            {
                public short hRes;
                public int hResUnit;
                public short widthUnit;
                public short vRes;
                public int vResUnit;
                public short heightUnit;
            }

            [StructLayout(LayoutKind.Sequential, Pack=2)]
            private struct RESOURCE_THUMBNAIL
            {
                public int nFormat;
                public int nWidth;
                public int nHeight;
                public int nWidthBytes;
                public int nSize;
                public int nCompressedSize;
                public short nBitPerPixel;
                public short nPlanes;
            }
        }
    }
}

