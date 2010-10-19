namespace Nomad.FileSystem.Archive
{
    using Nomad.Commons.IO;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Windows.Forms;

    public class ArchiveItem : CustomArchiveItem, IVirtualItemUI, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        private string FName;
        protected IVirtualFolder FParent;
        private static char[] InvalidPathChars = Path.GetInvalidPathChars();

        protected ArchiveItem(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            IEnumerable<ISimpleItem> deserializationContent = CustomArchiveItem.GetDeserializationContent(base.FArchiveUri, base.FFormatInfo);
            string str = info.GetString("ItemName");
            foreach (ISimpleItem item in deserializationContent)
            {
                if (str.Equals(item.Name, CustomArchiveItem.ComparisonRule))
                {
                    base.FItem = item;
                    this.FParent = (IVirtualFolder) CustomArchiveItem.FromName(base.FArchiveUri, deserializationContent, PathHelper.IncludeTrailingDirectorySeparator(Path.GetDirectoryName(item.Name)));
                    return;
                }
            }
            throw new SerializationException(string.Format("Cannot find file '{0}' in the '{1}' archive", str, base.FArchiveUri));
        }

        public ArchiveItem(ISimpleItem item, Uri archiveUri, ArchiveFormatInfo formatInfo, IVirtualFolder parent) : base(item, archiveUri, formatInfo)
        {
            this.FParent = parent;
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            return null;
        }

        public override bool Equals(IVirtualItem other)
        {
            ArchiveItem item = other as ArchiveItem;
            return (((item != null) && base.Equals(other)) && base.FItem.Name.Equals(item.FItem.Name, CustomArchiveItem.ComparisonRule));
        }

        public bool ExecuteVerb(IWin32Window owner, string verb)
        {
            return false;
        }

        internal static string GetFileName(string path)
        {
            int startIndex = 0;
            string str = PathHelper.ExcludeTrailingDirectorySeparator(path);
            startIndex = str.IndexOfAny(InvalidPathChars, startIndex);
            if (startIndex > 0)
            {
                StringBuilder builder = new StringBuilder(str);
                while (startIndex >= 0)
                {
                    builder[startIndex] = '_';
                    startIndex = str.IndexOfAny(InvalidPathChars, startIndex + 1);
                }
                str = builder.ToString();
            }
            return Path.GetFileName(str);
        }

        public Image GetIcon(Size size, IconStyle style)
        {
            return base.Extender.GetIcon(size, (style & IconStyle.CanUseAlphaBlending) > 0);
        }

        public void ShowProperties(IWin32Window owner)
        {
            using (PropertiesDialog dialog = new PropertiesDialog())
            {
                dialog.Execute(owner, new IVirtualItem[] { this });
            }
        }

        public override string FullName
        {
            get
            {
                return (base.FArchiveUri.ToString() + '#' + base.FItem.Name);
            }
        }

        public override string Name
        {
            get
            {
                if (this.FName == null)
                {
                    this.FName = GetFileName(base.FItem.Name);
                }
                return this.FName;
            }
        }

        public override IVirtualFolder Parent
        {
            get
            {
                return this.FParent;
            }
        }

        public string ToolTip
        {
            get
            {
                return base.Extender.ToolTip;
            }
        }
    }
}

