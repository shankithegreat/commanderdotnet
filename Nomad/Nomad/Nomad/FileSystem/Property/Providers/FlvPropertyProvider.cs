namespace Nomad.FileSystem.Property.Providers
{
    using Nomad.Commons;
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    [Version(1, 0, 3, 0x10)]
    public class FlvPropertyProvider : CustomExtPropertyProvider, ILocalFilePropertyProvider, ISimplePropertyProvider, IPropertyProvider
    {
        private static Regex FlvExtRegex;
        private static VirtualPropertySet FRegisteredProperties;
        private static int PropertyImageHeight;
        private static int PropertyImageSize;
        private static int PropertyImageWidth;
        private static int PropertyMediaDuration;
        private static int PropertyVideoBitrate;

        public IGetVirtualProperty AddProperties(FileSystemInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException();
            }
            FileInfo fileInfo = info as FileInfo;
            if ((fileInfo != null) && FlvExtRegex.IsMatch(fileInfo.Extension))
            {
                return new FlvPropertyBag(fileInfo);
            }
            return null;
        }

        public VirtualPropertySet GetRegisteredProperties()
        {
            return RegisteredProperties;
        }

        public bool Register(Hashtable options)
        {
            FlvExtRegex = CustomExtPropertyProvider.InitializeExtRegex(ConfigurationManager.GetSection("propertyProviders/flvProvider") as ExtSection);
            if (FlvExtRegex == null)
            {
                return false;
            }
            int groupId = VirtualProperty.RegisterGroup("Image");
            PropertyImageWidth = DefaultProperty.RegisterProperty("ImageWidth", groupId, typeof(int), 4);
            PropertyImageHeight = DefaultProperty.RegisterProperty("ImageHeight", groupId, typeof(int), 4);
            PropertyImageSize = DefaultProperty.RegisterProperty("ImageSize", groupId, typeof(Size), -1, ImageSizeTypeConverter.Default, 0);
            groupId = VirtualProperty.RegisterGroup("Media");
            PropertyMediaDuration = DefaultProperty.RegisterProperty("Duration", groupId, typeof(TimeSpan), -1, DurationTypeConverter.Default, 0);
            groupId = VirtualProperty.RegisterGroup("Video");
            PropertyVideoBitrate = DefaultProperty.RegisterProperty("VideoBitrate", groupId, typeof(int), -1, BitrateTypeConverter.Default, 0);
            return true;
        }

        private static VirtualPropertySet RegisteredProperties
        {
            get
            {
                if (FRegisteredProperties == null)
                {
                    FRegisteredProperties = new VirtualPropertySet(new int[] { PropertyImageWidth, PropertyImageHeight, PropertyImageSize, PropertyMediaDuration, PropertyVideoBitrate }).AsReadOnly();
                }
                return FRegisteredProperties;
            }
        }

        private class FlvPropertyBag : CustomPropertyProvider, IGetVirtualProperty
        {
            private FileInfo _FileInfo;
            private TimeSpan Duration;
            private bool HasMetaData;
            private Size ImageSize;
            private int VideoBitrate;

            public FlvPropertyBag(FileInfo fileInfo)
            {
                this._FileInfo = fileInfo;
            }

            protected override VirtualPropertySet CreateAvailableSet()
            {
                if ((this._FileInfo != null) || this.HasMetaData)
                {
                    return FlvPropertyProvider.RegisteredProperties;
                }
                return VirtualPropertySet.Empty;
            }

            private static double ReadDouble(Stream stream)
            {
                byte[] buffer = new byte[8];
                if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                {
                    throw new EndOfStreamException();
                }
                Array.Reverse(buffer);
                return BitConverter.ToDouble(buffer, 0);
            }

            private void ReadFlvMetaInfo(Stream stream)
            {
                byte[] buffer = new byte[3];
                if ((stream.Read(buffer, 0, buffer.Length) == buffer.Length) && !(Encoding.ASCII.GetString(buffer) != "FLV"))
                {
                    stream.Seek(0x1bL, SeekOrigin.Begin);
                    byte[] buffer2 = new byte[10];
                    if ((stream.Read(buffer2, 0, buffer2.Length) == buffer2.Length) && !(Encoding.ASCII.GetString(buffer2) != "onMetaData"))
                    {
                        stream.Seek(0x10L, SeekOrigin.Current);
                        double num2 = ReadDouble(stream);
                        stream.Seek(8L, SeekOrigin.Current);
                        double a = ReadDouble(stream);
                        stream.Seek(9L, SeekOrigin.Current);
                        double num4 = ReadDouble(stream);
                        this.ImageSize = new Size((int) Math.Round(a), (int) Math.Round(num4));
                        this.Duration = new TimeSpan(Convert.ToInt64((double) (num2 * 10000000.0)));
                        stream.Seek(0x10L, SeekOrigin.Current);
                        this.VideoBitrate = (int) Math.Round(ReadDouble(stream));
                        this.HasMetaData = true;
                    }
                }
            }

            public object this[int property]
            {
                get
                {
                    if (this._FileInfo != null)
                    {
                        try
                        {
                            using (Stream stream = this._FileInfo.OpenRead())
                            {
                                this.ReadFlvMetaInfo(stream);
                            }
                        }
                        catch (Exception exception)
                        {
                            PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                        }
                        this._FileInfo = null;
                        base.ResetAvailableSet();
                    }
                    if (this.HasMetaData)
                    {
                        if (property == FlvPropertyProvider.PropertyImageWidth)
                        {
                            return this.ImageSize.Width;
                        }
                        if (property == FlvPropertyProvider.PropertyImageHeight)
                        {
                            return this.ImageSize.Height;
                        }
                        if (property == FlvPropertyProvider.PropertyImageSize)
                        {
                            return this.ImageSize;
                        }
                        if (property == FlvPropertyProvider.PropertyMediaDuration)
                        {
                            return this.Duration;
                        }
                        if (property == FlvPropertyProvider.PropertyVideoBitrate)
                        {
                            return this.VideoBitrate;
                        }
                    }
                    return null;
                }
            }
        }
    }
}

