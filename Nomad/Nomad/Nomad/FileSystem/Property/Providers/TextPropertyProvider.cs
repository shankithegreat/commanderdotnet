namespace Nomad.FileSystem.Property.Providers
{
    using Microsoft.IE;
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Text;

    [Version(1, 0, 2, 11)]
    public class TextPropertyProvider : ILocalFilePropertyProvider, ISimplePropertyProvider, IPropertyProvider
    {
        private static int PropertyEncoding;
        private static TextProviderSection ProviderSection;

        public IGetVirtualProperty AddProperties(FileSystemInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException();
            }
            FileInfo fileInfo = info as FileInfo;
            if (fileInfo == null)
            {
                return null;
            }
            TextProviderElement textInfo = ProviderSection.TextProviders[fileInfo.Extension];
            if (textInfo == null)
            {
                return null;
            }
            return new TextPropertyBag(fileInfo, textInfo);
        }

        public VirtualPropertySet GetRegisteredProperties()
        {
            return new VirtualPropertySet(new int[] { 0x15, PropertyEncoding });
        }

        public bool Register(Hashtable options)
        {
            ProviderSection = ConfigurationManager.GetSection("propertyProviders/textProvider") as TextProviderSection;
            if (ProviderSection == null)
            {
                return false;
            }
            TextProviderCollection textProviders = ProviderSection.TextProviders;
            if ((textProviders == null) || (textProviders.Count == 0))
            {
                return false;
            }
            int groupId = VirtualProperty.RegisterGroup("Document");
            PropertyEncoding = DefaultProperty.RegisterProperty("Encoding", groupId, typeof(Encoding), -1, EncodingConveter.Default, 0);
            return true;
        }

        private class TextPropertyBag : CustomPropertyProvider, IGetVirtualProperty, IGetThumbnail
        {
            private FileInfo _FileInfo;
            private Encoding FEncoding;
            private TextProviderElement FTextInfo;
            private bool HasThumbnail = true;
            private WeakReference StoredThumbnail;

            public TextPropertyBag(FileInfo fileInfo, TextProviderElement textInfo)
            {
                this._FileInfo = fileInfo;
                this.FTextInfo = textInfo;
            }

            protected override VirtualPropertySet CreateAvailableSet()
            {
                VirtualPropertySet set = new VirtualPropertySet();
                set[0x15] = this.HasThumbnail;
                set[TextPropertyProvider.PropertyEncoding] = true;
                return set;
            }

            private Image CreateThumbnail(Size thumbnailSize)
            {
                Image target = null;
                if ((this.StoredThumbnail != null) && this.StoredThumbnail.IsAlive)
                {
                    target = (Image) this.StoredThumbnail.Target;
                }
                if ((target != null) && (target.Size == thumbnailSize))
                {
                    return target;
                }
                try
                {
                    int num;
                    byte[] buffer = new byte[0x2000];
                    using (Stream stream = this._FileInfo.OpenRead())
                    {
                        num = stream.Read(buffer, 0, buffer.Length);
                    }
                    this.InitializeEncoding(buffer, num);
                    string s = this.FEncoding.GetString(buffer, 0, num);
                    if (this.FTextInfo.RemoveBlankLines)
                    {
                        StringBuilder builder = new StringBuilder();
                        using (TextReader reader = new StringReader(s))
                        {
                            string str2;
                            while ((str2 = reader.ReadLine()) != null)
                            {
                                if (str2 != string.Empty)
                                {
                                    builder.AppendLine(str2);
                                }
                            }
                        }
                        s = builder.ToString();
                    }
                    target = new Bitmap(thumbnailSize.Width, thumbnailSize.Height);
                    using (Graphics graphics = Graphics.FromImage(target))
                    {
                        graphics.Clear(this.FTextInfo.BackColor);
                        graphics.DrawRectangle(Pens.DarkGray, 0, 0, target.Width - 1, target.Height - 1);
                        RectangleF layoutRectangle = new Rectangle(3, 3, target.Width - 7, target.Height - 7);
                        using (StringFormat format = new StringFormat())
                        {
                            format.FormatFlags = this.FTextInfo.WrapLines ? ((StringFormatFlags) 0) : StringFormatFlags.NoWrap;
                            format.Trimming = StringTrimming.None;
                            using (Brush brush = new SolidBrush(this.FTextInfo.ForeColor))
                            {
                                graphics.DrawString(s, this.FTextInfo.Font, brush, layoutRectangle, format);
                            }
                        }
                    }
                    this.StoredThumbnail = new WeakReference(target);
                    return target;
                }
                catch (Exception exception)
                {
                    PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                    this.HasThumbnail = false;
                    base.ResetAvailableSet();
                }
                return null;
            }

            public Image GetThumbnail(Size thumbSize)
            {
                if (this.HasThumbnail)
                {
                    return this.CreateThumbnail(thumbSize);
                }
                return null;
            }

            private void InitializeEncoding(byte[] data, int dataLength)
            {
                if (this.FEncoding == null)
                {
                    if (this.FTextInfo.DetectEncoding)
                    {
                        try
                        {
                            if (data == null)
                            {
                                data = new byte[0x2000];
                                using (Stream stream = this._FileInfo.OpenRead())
                                {
                                    dataLength = stream.Read(data, 0, data.Length);
                                }
                            }
                            this.FEncoding = MLangHelper.DetectEncoding(data, dataLength, DetectOption.TrySimpleDetectFirst);
                        }
                        catch (Exception exception)
                        {
                            PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                        }
                    }
                    if (this.FEncoding == null)
                    {
                        this.FEncoding = this.FTextInfo.Encoding;
                    }
                    if (this.FEncoding == null)
                    {
                        this.FEncoding = Encoding.Default;
                    }
                }
            }

            public object this[int property]
            {
                get
                {
                    if ((property == 0x15) && this.HasThumbnail)
                    {
                        return this.CreateThumbnail(TextPropertyProvider.ProviderSection.ThumbnailSize);
                    }
                    if (property == TextPropertyProvider.PropertyEncoding)
                    {
                        if (this.FEncoding == null)
                        {
                            this.InitializeEncoding(null, 0);
                        }
                        return this.FEncoding;
                    }
                    return null;
                }
            }
        }
    }
}

