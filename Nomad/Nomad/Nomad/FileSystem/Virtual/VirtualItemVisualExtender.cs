namespace Nomad.FileSystem.Virtual
{
    using Microsoft.Shell;
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;

    public class VirtualItemVisualExtender
    {
        private VirtualHighligher FHighlighter;
        private bool FHighlighterAcquired;
        private IDictionary<Size, Image> FIcons;
        private int FStoredChangeVector;
        private string FToolTip;
        private string FType;
        private static Func<IVirtualItem, string, CultureInfo, string> GetTooltipHandler = new Func<IVirtualItem, string, CultureInfo, string>(VirtualItemVisualExtender.GetTooltipAsync);
        public readonly IVirtualItem Item;
        private static Dictionary<string, string> TooltipPropertiesCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public VirtualItemVisualExtender(IVirtualItem item)
        {
            this.Item = item;
            this.FStoredChangeVector = ChangeVector.Value;
        }

        private static void AppendLineBefore(StringBuilder builder, string value)
        {
            if (builder.Length != 0)
            {
                builder.AppendLine();
            }
            builder.Append(value);
        }

        public static string GetContentToolTip(IEnumerable<IVirtualItem> content)
        {
            StringBuilder toolTipBuilder = new StringBuilder();
            UpdateContentToolTip(toolTipBuilder, content);
            return toolTipBuilder.ToString();
        }

        public Image GetIcon(Size size, bool canUseAlpha)
        {
            Image icon;
            bool flag = !ChangeVector.Equals(this.FStoredChangeVector, ChangeVector.Icon);
            if (flag)
            {
                ChangeVector.CopyTo(ref this.FStoredChangeVector, ChangeVector.Icon);
            }
            if (((this.FIcons == null) || flag) || !this.FIcons.TryGetValue(size, out icon))
            {
                if ((this.Highlighter != null) && (this.Highlighter.IconType == HighlighterIconType.HighlighterIcon))
                {
                    icon = this.Highlighter.GetIcon(size);
                }
                else if (this.Item is IVirtualFolder)
                {
                    icon = ImageProvider.Default.GetDefaultIcon(DefaultIcon.Folder, size);
                }
                else
                {
                    icon = ImageProvider.Default.GetDefaultFileIcon(this.Item.Name, size);
                }
                if ((canUseAlpha && (this.Highlighter != null)) && this.Highlighter.AlphaBlend)
                {
                    icon = ImageHelper.CreateBlendImage(icon, this.Highlighter.BlendColor, this.Highlighter.BlendLevel);
                }
                if ((this.FIcons == null) || flag)
                {
                    this.FIcons = IconCollection.Create();
                }
                this.FIcons[size] = icon;
            }
            return icon;
        }

        public static string GetItemTooltip(IVirtualItem item)
        {
            return GetItemTooltip(item, GetTooltipProperties(item));
        }

        private static string GetItemTooltip(IVirtualItem item, string properties)
        {
            if (string.IsNullOrEmpty(properties))
            {
                return string.Empty;
            }
            StringBuilder toolTipBuilder = new StringBuilder();
            foreach (string str in StringHelper.SplitString(properties, new char[] { ',' }))
            {
                IVirtualFolder folder;
                IVirtualCachedFolder folder2;
                VirtualProperty property;
                string str3 = str;
                if (str3 == null)
                {
                    goto Label_0168;
                }
                if (!(str3 == ""))
                {
                    if (str3 == "_Content")
                    {
                        goto Label_00A7;
                    }
                    if (str3 == "_CachedContent")
                    {
                        goto Label_0140;
                    }
                    goto Label_0168;
                }
                if (toolTipBuilder.Length > 0)
                {
                    toolTipBuilder.AppendLine();
                }
                continue;
            Label_00A7:
                folder = item as IVirtualFolder;
                if (folder != null)
                {
                    int length = toolTipBuilder.Length;
                    try
                    {
                        IEnumerable<IVirtualItem> content = null;
                        folder2 = item as IVirtualCachedFolder;
                        if ((folder2 != null) && ((folder2.CacheState & CacheState.HasContent) > CacheState.Unknown))
                        {
                            content = folder2.GetCachedContent();
                        }
                        else
                        {
                            content = folder.GetContent();
                        }
                        UpdateContentToolTip(toolTipBuilder, content);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        AppendLineBefore(toolTipBuilder, Resources.sToolTipContentAccessDenied);
                    }
                    if (toolTipBuilder.Length == length)
                    {
                        AppendLineBefore(toolTipBuilder, Resources.sToolTipEmptyFolder);
                    }
                }
                continue;
            Label_0140:
                folder2 = item as IVirtualCachedFolder;
                if (folder2 != null)
                {
                    UpdateContentToolTip(toolTipBuilder, folder2.GetCachedContent());
                }
                continue;
            Label_0168:;
                if (str.IndexOfAny(new char[] { '-', '?' }) == 0)
                {
                    property = VirtualProperty.Get(str.Substring(1));
                }
                else
                {
                    property = VirtualProperty.Get(str);
                }
                if ((property != null) && (item.GetPropertyAvailability(property.PropertyId) == PropertyAvailability.Normal))
                {
                    object obj2 = item[property.PropertyId];
                    if ((obj2 == null) || ((obj2 is string) && obj2.Equals(string.Empty)))
                    {
                        continue;
                    }
                    if (toolTipBuilder.Length > 0)
                    {
                        toolTipBuilder.AppendLine();
                    }
                    if (str.IndexOf('-') != 0)
                    {
                        toolTipBuilder.Append(property.LocalizedName);
                        toolTipBuilder.Append(": ");
                    }
                    toolTipBuilder.Append(property.ConvertToString(obj2));
                }
            }
            return toolTipBuilder.ToString();
        }

        private void GetItemTooltipCallback(object state)
        {
            try
            {
                this.FToolTip = GetTooltipHandler.EndInvoke((IAsyncResult) state);
            }
            catch (Exception exception)
            {
                this.FToolTip = exception.Message;
            }
        }

        private static string GetTooltipAsync(IVirtualItem item, string properties, CultureInfo uiCulture)
        {
            Thread.CurrentThread.CurrentUICulture = uiCulture;
            return GetItemTooltip(item, properties);
        }

        private static string GetTooltipProperties(IVirtualItem item)
        {
            string str = null;
            Dictionary<string, string> dictionary;
            string str2 = (item is IVirtualFolder) ? "Folder" : (item[1] as string);
            System.Type baseType = item.GetType();
            string key = baseType.Name + '.' + str2;
            lock ((dictionary = TooltipPropertiesCache))
            {
                if (TooltipPropertiesCache.TryGetValue(key, out str))
                {
                    return str;
                }
            }
            NameValueCollection section = null;
            while ((section == null) && (baseType != null))
            {
                section = ConfigurationManager.GetSection("toolTips/" + baseType.Name) as NameValueCollection;
                baseType = baseType.BaseType;
            }
            if (section != null)
            {
                str = section[str2];
                if (!(string.IsNullOrEmpty(str) || (str[0] != '=')))
                {
                    str2 = str.Substring(1);
                    str = section[str2];
                }
                if (str == null)
                {
                    str = section["Default"];
                }
            }
            if (str == null)
            {
                section = ConfigurationManager.GetSection("toolTips/default") as NameValueCollection;
                if (section != null)
                {
                    str = section[str2];
                    if (str == null)
                    {
                        str = section["Default"];
                    }
                }
            }
            lock ((dictionary = TooltipPropertiesCache))
            {
                TooltipPropertiesCache[key] = str;
            }
            return str;
        }

        public bool HasIcon(Size size)
        {
            return ((this.FIcons != null) && this.FIcons.ContainsKey(size));
        }

        public void SetIcon(Image icon, Size size)
        {
            if (icon == null)
            {
                throw new ArgumentNullException("icon");
            }
            if (this.FIcons == null)
            {
                this.FIcons = IconCollection.Create();
            }
            this.FIcons[size] = icon;
        }

        private static void UpdateContentToolTip(StringBuilder toolTipBuilder, IEnumerable<IVirtualItem> content)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            bool flag = false;
            bool flag2 = false;
            IVirtualItemFilter hiddenItemsFilter = VirtualFilePanelSettings.Default.HiddenItemsFilter;
            foreach (IVirtualItem item in content)
            {
                if (flag && flag2)
                {
                    break;
                }
                if ((hiddenItemsFilter == null) || !hiddenItemsFilter.IsMatch(item))
                {
                    bool flag3 = item is IVirtualFolder;
                    if (!(flag3 ? flag : flag2))
                    {
                        StringBuilder builder3 = flag3 ? builder : builder2;
                        if (builder3.Length > 0)
                        {
                            builder3.Append(", ");
                        }
                        if (builder3.Length > 30)
                        {
                            builder3.Append('…');
                            if (flag3)
                            {
                                flag = true;
                            }
                            else
                            {
                                flag2 = true;
                            }
                        }
                        else
                        {
                            builder3.Append(item.Name);
                        }
                    }
                }
            }
            if (builder.Length > 0)
            {
                AppendLineBefore(toolTipBuilder, Resources.sToolTipFolders);
                toolTipBuilder.Append(builder);
            }
            if (builder2.Length > 0)
            {
                AppendLineBefore(toolTipBuilder, Resources.sToolTipFiles);
                toolTipBuilder.Append(builder2);
            }
        }

        public bool HasType
        {
            get
            {
                return ((this.FType != null) && ChangeVector.Equals(this.FStoredChangeVector, ChangeVector.Localization));
            }
        }

        public VirtualHighligher Highlighter
        {
            get
            {
                if (!ChangeVector.Equals(this.FStoredChangeVector, ChangeVector.Highlighters))
                {
                    this.FHighlighter = null;
                    this.FHighlighterAcquired = false;
                    ChangeVector.CopyTo(ref this.FStoredChangeVector, ChangeVector.Highlighters);
                }
                if (!this.FHighlighterAcquired)
                {
                    ListViewHighlighter[] highlighters = Settings.Default.Highlighters;
                    if (highlighters != null)
                    {
                        foreach (ListViewHighlighter highlighter in highlighters)
                        {
                            if ((highlighter.Filter != null) && highlighter.Filter.IsMatch(this.Item))
                            {
                                this.FHighlighter = highlighter;
                                break;
                            }
                        }
                    }
                    this.FHighlighterAcquired = true;
                }
                return this.FHighlighter;
            }
        }

        public string ToolTip
        {
            get
            {
                if (!ChangeVector.Equals(this.FStoredChangeVector, ChangeVector.Localization))
                {
                    this.FType = null;
                    this.FToolTip = null;
                    ChangeVector.CopyTo(ref this.FStoredChangeVector, ChangeVector.Localization);
                }
                if (this.FToolTip == null)
                {
                    int tooltipTimeout = Settings.Default.TooltipTimeout;
                    try
                    {
                        string tooltipProperties = GetTooltipProperties(this.Item);
                        if (tooltipTimeout == -1)
                        {
                            this.FToolTip = GetItemTooltip(this.Item, tooltipProperties);
                        }
                        else
                        {
                            IAsyncResult state = GetTooltipHandler.BeginInvoke(this.Item, tooltipProperties, Thread.CurrentThread.CurrentUICulture, null, null);
                            if (!state.AsyncWaitHandle.WaitOne(tooltipTimeout, false))
                            {
                                ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetItemTooltipCallback), state);
                                return Resources.sTimeOutElapsed;
                            }
                            this.FToolTip = GetTooltipHandler.EndInvoke(state);
                        }
                        if (tooltipProperties.IndexOf('?') >= 0)
                        {
                            string fToolTip = this.FToolTip;
                            this.FToolTip = null;
                            return fToolTip;
                        }
                    }
                    catch (Exception exception)
                    {
                        this.FToolTip = exception.Message;
                    }
                }
                return this.FToolTip;
            }
            set
            {
                this.FToolTip = value;
            }
        }

        public string Type
        {
            get
            {
                if (!ChangeVector.Equals(this.FStoredChangeVector, ChangeVector.Localization))
                {
                    this.FType = null;
                    this.FToolTip = null;
                    ChangeVector.CopyTo(ref this.FStoredChangeVector, ChangeVector.Localization);
                }
                if (this.FType == null)
                {
                    if ((this.Item.Attributes & FileAttributes.Directory) > 0)
                    {
                        this.FType = Resources.sItemTypeFolder;
                    }
                    else
                    {
                        SHFILEINFO psfi = new SHFILEINFO();
                        Shell32.SHGetFileInfo(Path.Combine(@"c:\", this.Item.Name), this.Item.Attributes, ref psfi, SHGFI.SHGFI_LARGEICON | SHGFI.SHGFI_USEFILEATTRIBUTES | SHGFI.SHGFI_TYPENAME);
                        this.FType = string.Intern(psfi.szTypeName);
                    }
                }
                return this.FType;
            }
            set
            {
                this.FType = value;
            }
        }
    }
}

