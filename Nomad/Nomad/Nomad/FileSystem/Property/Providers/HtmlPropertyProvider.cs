namespace Nomad.FileSystem.Property.Providers
{
    using Microsoft.COM;
    using Microsoft.IE;
    using Microsoft.Win32;
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;

    [Version(1, 0, 2, 12)]
    public class HtmlPropertyProvider : ILocalFilePropertyProvider, ISimplePropertyProvider, IPropertyProvider
    {
        private static Regex HtmlExtRegex;
        private static int Magnify;
        private static int PropertyDocumentTitle;
        private static int PropertyEncoding;
        private static Size ThumbnailSize;
        private static Size WindowSize;

        public IGetVirtualProperty AddProperties(FileSystemInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException();
            }
            FileInfo fileInfo = info as FileInfo;
            if ((fileInfo != null) && HtmlExtRegex.IsMatch(fileInfo.Extension))
            {
                return new HtmlPropertyBag(fileInfo);
            }
            return null;
        }

        public VirtualPropertySet GetRegisteredProperties()
        {
            return new VirtualPropertySet(new int[] { 0x15, PropertyDocumentTitle, PropertyEncoding });
        }

        public bool Register(Hashtable options)
        {
            if (options == null)
            {
                return false;
            }
            string pattern = options["ext"] as string;
            if (pattern == null)
            {
                return false;
            }
            ThumbnailSize = PropertyProviderManager.ReadOption<Size>(options, "thumbnailSize", new Size(120, 120));
            WindowSize = PropertyProviderManager.ReadOption<Size>(options, "windowSize", new Size(800, 600));
            Magnify = PropertyProviderManager.ReadOption<int>(options, "magnify", 1);
            if (Magnify < 1)
            {
                Magnify = 1;
            }
            HtmlExtRegex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            int groupId = VirtualProperty.RegisterGroup("Document");
            PropertyDocumentTitle = DefaultProperty.RegisterProperty("DocumentTitle", groupId, typeof(string), -1);
            PropertyEncoding = DefaultProperty.RegisterProperty("Encoding", groupId, typeof(Encoding), -1, EncodingConveter.Default, 0);
            return true;
        }

        private class HtmlPropertyBag : CustomPropertyProvider, IGetVirtualProperty, IGetThumbnail
        {
            private FileInfo _FileInfo;
            private static Regex ContentTypeRegex;
            private System.Text.Encoding FEncoding;
            private string FTitle;
            private bool HasThumbnail;
            private static Regex MetaRegex;
            private WeakReference StoredThumbnail;
            private Size StoredThumbnailSize;
            private static Regex SubMetaRegex;
            private static Regex TitleRegex;

            public HtmlPropertyBag(FileInfo fileInfo)
            {
                this._FileInfo = fileInfo;
                this.HasThumbnail = !RemotingServices.IsTransparentProxy(fileInfo);
            }

            protected override VirtualPropertySet CreateAvailableSet()
            {
                VirtualPropertySet set = new VirtualPropertySet();
                set[0x15] = this.HasThumbnail;
                set[HtmlPropertyProvider.PropertyDocumentTitle] = this.FTitle != string.Empty;
                set[HtmlPropertyProvider.PropertyEncoding] = true;
                return set;
            }

            private Image CreateThumbnail(ref Size thumbnailSize)
            {
                Image target = null;
                if ((this.StoredThumbnail != null) && this.StoredThumbnail.IsAlive)
                {
                    target = (Image) this.StoredThumbnail.Target;
                }
                if ((target == null) || (this.StoredThumbnailSize != thumbnailSize))
                {
                    try
                    {
                        object[] parameter = new object[] { this._FileInfo.FullName, (Size) thumbnailSize };
                        Thread thread = new Thread(new ParameterizedThreadStart(this.CreateThumbnailAsync));
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start(parameter);
                        thread.Join();
                        if (parameter[0] is Exception)
                        {
                            throw ((Exception) parameter[0]);
                        }
                        target = parameter[0] as Image;
                    }
                    catch (Exception exception)
                    {
                        PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                        target = null;
                    }
                    if (target == null)
                    {
                        this.HasThumbnail = false;
                        base.ResetAvailableSet();
                    }
                    else
                    {
                        this.StoredThumbnail = new WeakReference(target);
                        this.StoredThumbnailSize = thumbnailSize;
                    }
                }
                return target;
            }

            private void CreateThumbnailAsync(object state)
            {
                object[] objArray = (object[]) state;
                string urlString = (string) objArray[0];
                Size maxThumbnailSize = (Size) objArray[1];
                try
                {
                    DummyClientSite pClientSite = new DummyClientSite();
                    using (WebBrowser browser = new WebBrowser())
                    {
                        browser.Size = HtmlPropertyProvider.WindowSize;
                        browser.ScriptErrorsSuppressed = true;
                        browser.ScrollBarsEnabled = false;
                        ((Microsoft.COM.IOleObject) browser.ActiveXInstance).SetClientSite(pClientSite);
                        int tickCount = Environment.TickCount;
                        browser.Navigate(urlString);
                        while (browser.ReadyState != WebBrowserReadyState.Complete)
                        {
                            if (Math.Abs((int) (Environment.TickCount - tickCount)) > 0x2710)
                            {
                                throw new TimeoutException();
                            }
                            Application.DoEvents();
                        }
                        if (browser.Document.DomDocument is Microsoft.COM.IViewObject)
                        {
                            Size thumbnailSize = ImageHelper.GetThumbnailSize(browser.Size, maxThumbnailSize);
                            Bitmap image = new Bitmap(thumbnailSize.Width, thumbnailSize.Height);
                            try
                            {
                                using (Graphics graphics = Graphics.FromImage(image))
                                {
                                    IntPtr hdc = graphics.GetHdc();
                                    try
                                    {
                                        Rectangle lprcBounds = new Rectangle(0, 0, image.Width * HtmlPropertyProvider.Magnify, image.Height * HtmlPropertyProvider.Magnify);
                                        int errorCode = Microsoft.COM.ActiveX.OleDraw(browser.Document.DomDocument, DVASPECT2.DVASPECT_DOCPRINT, hdc, ref lprcBounds);
                                        if (HRESULT.FAILED(errorCode))
                                        {
                                            Marshal.ThrowExceptionForHR(errorCode);
                                        }
                                    }
                                    finally
                                    {
                                        graphics.ReleaseHdc(hdc);
                                    }
                                }
                            }
                            catch
                            {
                                image.Dispose();
                                throw;
                            }
                            objArray[0] = image;
                        }
                    }
                }
                catch (Exception exception)
                {
                    objArray[0] = exception;
                }
            }

            private void ExtractProperties()
            {
                try
                {
                    int num;
                    byte[] buffer = new byte[0x2000];
                    using (Stream stream = this._FileInfo.OpenRead())
                    {
                        num = stream.Read(buffer, 0, buffer.Length);
                    }
                    System.Text.Encoding encoding = MLangHelper.DetectEncoding(buffer, num, DetectOption.SourceHtml);
                    if (encoding == null)
                    {
                        encoding = System.Text.Encoding.Default;
                    }
                    this.FEncoding = encoding;
                    string htmlData = this.FEncoding.GetString(buffer, 0, num);
                    foreach (HtmlMeta meta in Parse(htmlData))
                    {
                        ContentTypeMeta meta2 = meta as ContentTypeMeta;
                        if (meta2 != null)
                        {
                            try
                            {
                                this.FEncoding = System.Text.Encoding.GetEncoding(meta2.Charset);
                            }
                            catch
                            {
                            }
                            break;
                        }
                    }
                    if (this.FEncoding.CodePage != encoding.CodePage)
                    {
                        htmlData = this.FEncoding.GetString(buffer, 0, num);
                    }
                    if (TitleRegex == null)
                    {
                        TitleRegex = new Regex(@"<title>(?<title>[^\p{C}]+)</title>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    }
                    Match match = TitleRegex.Match(htmlData);
                    if (match.Success)
                    {
                        this.FTitle = match.Groups["title"].Value;
                    }
                }
                catch (Exception exception)
                {
                    PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                }
                if (this.FEncoding == null)
                {
                    this.FEncoding = System.Text.Encoding.Default;
                }
                if (this.FTitle == null)
                {
                    this.FTitle = string.Empty;
                }
            }

            public Image GetThumbnail(Size thumbSize)
            {
                if (this.HasThumbnail)
                {
                    return this.CreateThumbnail(ref thumbSize);
                }
                return null;
            }

            private static IEnumerable<HtmlMeta> Parse(string htmlData)
            {
                return new <Parse>d__0(-2) { <>3__htmlData = htmlData };
            }

            private System.Text.Encoding Encoding
            {
                get
                {
                    if (this.FEncoding == null)
                    {
                        this.ExtractProperties();
                    }
                    return this.FEncoding;
                }
            }

            public object this[int property]
            {
                get
                {
                    if (property == HtmlPropertyProvider.PropertyDocumentTitle)
                    {
                        return this.Title;
                    }
                    if (property == HtmlPropertyProvider.PropertyEncoding)
                    {
                        return this.Encoding;
                    }
                    if ((property == 0x15) && this.HasThumbnail)
                    {
                        return this.CreateThumbnail(ref HtmlPropertyProvider.ThumbnailSize);
                    }
                    return null;
                }
            }

            private string Title
            {
                get
                {
                    if (this.FTitle == null)
                    {
                        this.ExtractProperties();
                    }
                    return ((this.FTitle == string.Empty) ? null : this.FTitle);
                }
            }

            [CompilerGenerated]
            private sealed class <Parse>d__0 : IEnumerable<HtmlPropertyProvider.HtmlPropertyBag.HtmlMeta>, IEnumerable, IEnumerator<HtmlPropertyProvider.HtmlPropertyBag.HtmlMeta>, IEnumerator, IDisposable
            {
                private int <>1__state;
                private HtmlPropertyProvider.HtmlPropertyBag.HtmlMeta <>2__current;
                public string <>3__htmlData;
                public IEnumerator <>7__wrap8;
                public IDisposable <>7__wrap9;
                public IEnumerator <>7__wrapb;
                public IDisposable <>7__wrapc;
                private int <>l__initialThreadId;
                public Match <ContentMatch>5__6;
                public HtmlPropertyProvider.HtmlPropertyBag.HtmlMeta <Meta>5__2;
                public Match <metamatch>5__1;
                public string <Name>5__4;
                public Match <submetamatch>5__3;
                public HtmlPropertyProvider.HtmlPropertyBag.ContentTypeMeta <TypeMeta>5__7;
                public string <Value>5__5;
                public string htmlData;

                [DebuggerHidden]
                public <Parse>d__0(int <>1__state)
                {
                    this.<>1__state = <>1__state;
                    this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
                }

                private void <>m__Finallya()
                {
                    this.<>1__state = -1;
                    this.<>7__wrap9 = this.<>7__wrap8 as IDisposable;
                    if (this.<>7__wrap9 != null)
                    {
                        this.<>7__wrap9.Dispose();
                    }
                }

                private void <>m__Finallyd()
                {
                    this.<>1__state = 1;
                    this.<>7__wrapc = this.<>7__wrapb as IDisposable;
                    if (this.<>7__wrapc != null)
                    {
                        this.<>7__wrapc.Dispose();
                    }
                }

                private bool MoveNext()
                {
                    try
                    {
                        switch (this.<>1__state)
                        {
                            case 0:
                                this.<>1__state = -1;
                                if (HtmlPropertyProvider.HtmlPropertyBag.MetaRegex == null)
                                {
                                    HtmlPropertyProvider.HtmlPropertyBag.MetaRegex = new Regex("<meta\\s+(?:(?:\\b(\\w|-)+\\b\\s*(?:=\\s*(?:\"[^\"]*\"|'[^']*'|[^\"'<> ]+)\\s*)?)*)/?\\s*>", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
                                }
                                this.<>7__wrap8 = HtmlPropertyProvider.HtmlPropertyBag.MetaRegex.Matches(this.htmlData).GetEnumerator();
                                this.<>1__state = 1;
                                while (this.<>7__wrap8.MoveNext())
                                {
                                    this.<metamatch>5__1 = (Match) this.<>7__wrap8.Current;
                                    this.<Meta>5__2 = new HtmlPropertyProvider.HtmlPropertyBag.HtmlMeta();
                                    if (HtmlPropertyProvider.HtmlPropertyBag.SubMetaRegex == null)
                                    {
                                        HtmlPropertyProvider.HtmlPropertyBag.SubMetaRegex = new Regex("(?<name>\\b(\\w|-)+\\b)\\s*=\\s*(\"(?<value>[^\"]*)\"|'(?<value>[^']*)'|(?<value>[^\"'<> ]+)\\s*)+", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
                                    }
                                    this.<>7__wrapb = HtmlPropertyProvider.HtmlPropertyBag.SubMetaRegex.Matches(this.<metamatch>5__1.Value.ToString()).GetEnumerator();
                                    this.<>1__state = 2;
                                    while (this.<>7__wrapb.MoveNext())
                                    {
                                        this.<submetamatch>5__3 = (Match) this.<>7__wrapb.Current;
                                        this.<Name>5__4 = this.<submetamatch>5__3.Groups["name"].Value;
                                        this.<Value>5__5 = this.<submetamatch>5__3.Groups["value"].Value;
                                        if (string.Equals(this.<Name>5__4, "http-equiv", StringComparison.OrdinalIgnoreCase))
                                        {
                                            this.<Meta>5__2.HttpEquiv = this.<Value>5__5;
                                        }
                                        if (string.Equals(this.<Name>5__4, "name", StringComparison.OrdinalIgnoreCase) && (this.<Meta>5__2.HttpEquiv == string.Empty))
                                        {
                                            this.<Meta>5__2.Name = this.<Value>5__5;
                                        }
                                        if (!string.Equals(this.<Name>5__4, "content", StringComparison.OrdinalIgnoreCase))
                                        {
                                            goto Label_031B;
                                        }
                                        this.<Meta>5__2.Content = this.<Value>5__5;
                                        if (!string.Equals(this.<Meta>5__2.HttpEquiv, "Content-Type", StringComparison.OrdinalIgnoreCase))
                                        {
                                            goto Label_02FC;
                                        }
                                        if (HtmlPropertyProvider.HtmlPropertyBag.ContentTypeRegex == null)
                                        {
                                            HtmlPropertyProvider.HtmlPropertyBag.ContentTypeRegex = new Regex(@"\s*(?<mime>[\w/]+);\s*charset=(?<charset>[\w-]+)\s*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                        }
                                        this.<ContentMatch>5__6 = HtmlPropertyProvider.HtmlPropertyBag.ContentTypeRegex.Match(this.<Meta>5__2.Content);
                                        if (!this.<ContentMatch>5__6.Success)
                                        {
                                            goto Label_02FC;
                                        }
                                        this.<TypeMeta>5__7 = new HtmlPropertyProvider.HtmlPropertyBag.ContentTypeMeta();
                                        this.<TypeMeta>5__7.HttpEquiv = this.<Meta>5__2.HttpEquiv;
                                        this.<TypeMeta>5__7.Content = this.<Meta>5__2.Content;
                                        this.<TypeMeta>5__7.MimeType = this.<ContentMatch>5__6.Groups["mime"].Value;
                                        this.<TypeMeta>5__7.Charset = this.<ContentMatch>5__6.Groups["charset"].Value;
                                        this.<>2__current = this.<TypeMeta>5__7;
                                        this.<>1__state = 3;
                                        return true;
                                    Label_02F2:
                                        this.<>1__state = 2;
                                        continue;
                                    Label_02FC:
                                        this.<>2__current = this.<Meta>5__2;
                                        this.<>1__state = 4;
                                        return true;
                                    Label_0313:
                                        this.<>1__state = 2;
                                    Label_031B:;
                                    }
                                    this.<>m__Finallyd();
                                }
                                this.<>m__Finallya();
                                break;

                            case 3:
                                goto Label_02F2;

                            case 4:
                                goto Label_0313;
                        }
                        return false;
                    }
                    fault
                    {
                        this.System.IDisposable.Dispose();
                    }
                }

                [DebuggerHidden]
                IEnumerator<HtmlPropertyProvider.HtmlPropertyBag.HtmlMeta> IEnumerable<HtmlPropertyProvider.HtmlPropertyBag.HtmlMeta>.GetEnumerator()
                {
                    HtmlPropertyProvider.HtmlPropertyBag.<Parse>d__0 d__;
                    if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                    {
                        this.<>1__state = 0;
                        d__ = this;
                    }
                    else
                    {
                        d__ = new HtmlPropertyProvider.HtmlPropertyBag.<Parse>d__0(0);
                    }
                    d__.htmlData = this.<>3__htmlData;
                    return d__;
                }

                [DebuggerHidden]
                IEnumerator IEnumerable.GetEnumerator()
                {
                    return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Property.Providers.HtmlPropertyProvider.HtmlPropertyBag.HtmlMeta>.GetEnumerator();
                }

                [DebuggerHidden]
                void IEnumerator.Reset()
                {
                    throw new NotSupportedException();
                }

                void IDisposable.Dispose()
                {
                    switch (this.<>1__state)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            try
                            {
                                switch (this.<>1__state)
                                {
                                    case 2:
                                    case 3:
                                    case 4:
                                        break;

                                    default:
                                        break;
                                }
                                try
                                {
                                }
                                finally
                                {
                                    this.<>m__Finallyd();
                                }
                            }
                            finally
                            {
                                this.<>m__Finallya();
                            }
                            break;
                    }
                }

                HtmlPropertyProvider.HtmlPropertyBag.HtmlMeta IEnumerator<HtmlPropertyProvider.HtmlPropertyBag.HtmlMeta>.Current
                {
                    [DebuggerHidden]
                    get
                    {
                        return this.<>2__current;
                    }
                }

                object IEnumerator.Current
                {
                    [DebuggerHidden]
                    get
                    {
                        return this.<>2__current;
                    }
                }
            }

            private class ContentTypeMeta : HtmlPropertyProvider.HtmlPropertyBag.HtmlMeta
            {
                public string Charset;
                public string MimeType;
            }

            private class HtmlMeta
            {
                public string Content;
                public string HttpEquiv;
                public string Name;
            }
        }
    }
}

