namespace Nomad.FileSystem.Virtual
{
    using Nomad.Commons.Drawing;
    using Nomad.Configuration;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.Serialization;

    [Serializable]
    public class SimpleCustomizeFolder : ICustomizeFolder, ISerializable
    {
        private const string EntryAutoSizeColumns = "AutoSizeColumns";
        private const string EntryBackColor = "BackColor";
        private const string EntryColumns = "Columns";
        private const string EntryFilter = "Filter";
        private const string EntryForeColor = "ForeColor";
        private const string EntryIconFile = "IconFile";
        private const string EntryIconIndex = "IconIndex";
        private const string EntryListColumnCount = "ListColumnCount";
        private const string EntrySort = "Sort";
        private const string EntryThumbnailSize = "ThumbnailSize";
        private const string EntryThumbnailSpacing = "ThumbnailSpacing";
        private const string EntryUpdateableParts = "UpdateableParts";
        private const string EntryView = "View";
        private bool? FAutoSizeColumns;
        private Color FBackColor;
        private ListViewColumnInfo[] FColumns;
        private IVirtualItemFilter FFilter;
        private Color FForeColor;
        private IconLocation FIcon;
        private int? FListColumnCount;
        private VirtualItemComparer FSort;
        private Size FThumbnailSize;
        private Size FThumbnailSpacing;
        private CustomizeFolderParts FUpdatebleParts;
        private PanelView? FView;

        public SimpleCustomizeFolder()
        {
            this.FUpdatebleParts = CustomizeFolderParts.All;
        }

        public SimpleCustomizeFolder(CustomizeFolderParts updatableParts)
        {
            this.FUpdatebleParts = updatableParts;
        }

        public SimpleCustomizeFolder(ICustomizeFolder source)
        {
            this.FAutoSizeColumns = source.AutoSizeColumns;
            this.FColumns = source.Columns;
            this.FIcon = source.Icon;
            this.FFilter = source.Filter;
            this.FSort = source.Sort;
            this.FListColumnCount = source.ListColumnCount;
            this.FThumbnailSize = source.ThumbnailSize;
            this.FThumbnailSpacing = source.ThumbnailSpacing;
            this.FView = source.View;
            this.FBackColor = source.BackColor;
            this.FForeColor = source.ForeColor;
        }

        public SimpleCustomizeFolder(CustomizeFolderParts updatableParts, ICustomizeFolder source) : this(source)
        {
            this.FUpdatebleParts = updatableParts;
        }

        protected SimpleCustomizeFolder(SerializationInfo info, StreamingContext context)
        {
            this.FUpdatebleParts = (CustomizeFolderParts) info.GetValue("UpdateableParts", typeof(CustomizeFolderParts));
            string str = null;
            int iconIndex = 0;
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                switch (current.Name)
                {
                    case "AutoSizeColumns":
                        this.FAutoSizeColumns = new bool?(Convert.ToBoolean(current.Value));
                        break;

                    case "Columns":
                        this.FColumns = (ListViewColumnInfo[]) current.Value;
                        break;

                    case "Filter":
                        this.FFilter = (IVirtualItemFilter) current.Value;
                        break;

                    case "Sort":
                        this.FSort = (VirtualItemComparer) current.Value;
                        break;

                    case "ListColumnCount":
                        this.FListColumnCount = new int?(Convert.ToInt32(current.Value));
                        break;

                    case "ThumbnailSize":
                        this.FThumbnailSize = (Size) current.Value;
                        break;

                    case "ThumbnailSpacing":
                        this.FThumbnailSpacing = (Size) current.Value;
                        break;

                    case "View":
                        this.FView = new PanelView?((PanelView) current.Value);
                        break;

                    case "IconFile":
                        str = current.Value as string;
                        break;

                    case "IconIndex":
                        iconIndex = Convert.ToInt32(current.Value);
                        break;

                    case "BackColor":
                        this.FBackColor = (Color) current.Value;
                        break;

                    case "ForeColor":
                        this.FForeColor = (Color) current.Value;
                        break;
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.FIcon = new IconLocation(str, iconIndex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UpdateableParts", this.FUpdatebleParts);
            if ((this.FColumns != null) && (this.FColumns.Length > 0))
            {
                info.AddValue("AutoSizeColumns", this.FAutoSizeColumns);
                info.AddValue("Columns", this.FColumns);
            }
            if (this.FFilter != null)
            {
                info.AddValue("Filter", this.FFilter);
            }
            if (this.FSort != null)
            {
                info.AddValue("Sort", this.FSort);
            }
            if (this.FListColumnCount.HasValue)
            {
                info.AddValue("ListColumnCount", this.FListColumnCount.Value);
            }
            if (!this.FThumbnailSize.IsEmpty)
            {
                info.AddValue("ThumbnailSize", this.FThumbnailSize);
            }
            if (!this.FThumbnailSpacing.IsEmpty)
            {
                info.AddValue("ThumbnailSpacing", this.FThumbnailSpacing);
            }
            if (this.FView.HasValue)
            {
                info.AddValue("View", this.FView.Value);
            }
            if (this.FIcon != null)
            {
                info.AddValue("IconFile", this.FIcon.IconFileName);
                info.AddValue("IconIndex", this.FIcon.IconIndex);
            }
            if (!this.FBackColor.IsEmpty)
            {
                info.AddValue("BackColor", this.FBackColor);
            }
            if (!this.FForeColor.IsEmpty)
            {
                info.AddValue("ForeColor", this.FForeColor);
            }
        }

        public void SetColumns(IEnumerable<ListViewColumnInfo> columns)
        {
            if (columns == null)
            {
                this.FColumns = null;
            }
            else
            {
                List<ListViewColumnInfo> list = new List<ListViewColumnInfo>(columns);
                if (list.Count > 0)
                {
                    this.FColumns = list.ToArray();
                }
                else
                {
                    this.FColumns = null;
                }
            }
        }

        public bool? AutoSizeColumns
        {
            get
            {
                return this.FAutoSizeColumns;
            }
            set
            {
                if ((this.FUpdatebleParts & CustomizeFolderParts.Columns) == 0)
                {
                    throw new NotSupportedException();
                }
                this.FAutoSizeColumns = value;
            }
        }

        public Color BackColor
        {
            get
            {
                return this.FBackColor;
            }
            set
            {
                if ((this.FUpdatebleParts & CustomizeFolderParts.Colors) == 0)
                {
                    throw new NotSupportedException();
                }
                this.FBackColor = value;
            }
        }

        public ListViewColumnInfo[] Columns
        {
            get
            {
                return this.FColumns;
            }
            set
            {
                if ((this.FUpdatebleParts & CustomizeFolderParts.Columns) == 0)
                {
                    throw new NotSupportedException();
                }
                this.FColumns = value;
            }
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                return this.FFilter;
            }
            set
            {
                if ((this.FUpdatebleParts & CustomizeFolderParts.Filter) == 0)
                {
                    throw new NotSupportedException();
                }
                this.FFilter = value;
            }
        }

        public Color ForeColor
        {
            get
            {
                return this.FForeColor;
            }
            set
            {
                if ((this.FUpdatebleParts & CustomizeFolderParts.Colors) == 0)
                {
                    throw new NotSupportedException();
                }
                this.FForeColor = value;
            }
        }

        public IconLocation Icon
        {
            get
            {
                return this.FIcon;
            }
            set
            {
                if ((this.FUpdatebleParts & CustomizeFolderParts.Icon) == 0)
                {
                    throw new NotSupportedException();
                }
                this.FIcon = value;
            }
        }

        public int? ListColumnCount
        {
            get
            {
                return this.FListColumnCount;
            }
            set
            {
                if ((this.FUpdatebleParts & CustomizeFolderParts.ListColumnCount) == 0)
                {
                    throw new NotSupportedException();
                }
                this.FListColumnCount = value;
            }
        }

        public VirtualItemComparer Sort
        {
            get
            {
                return this.FSort;
            }
            set
            {
                if ((this.FUpdatebleParts & CustomizeFolderParts.Sort) == 0)
                {
                    throw new NotSupportedException();
                }
                this.FSort = value;
            }
        }

        public Size ThumbnailSize
        {
            get
            {
                return this.FThumbnailSize;
            }
            set
            {
                if ((this.FUpdatebleParts & CustomizeFolderParts.ThumbnailSize) == 0)
                {
                    throw new NotSupportedException();
                }
                this.FThumbnailSize = value;
            }
        }

        public Size ThumbnailSpacing
        {
            get
            {
                return this.FThumbnailSpacing;
            }
            set
            {
                if ((this.FUpdatebleParts & CustomizeFolderParts.ThumbnailSize) == 0)
                {
                    throw new NotSupportedException();
                }
                this.FThumbnailSpacing = value;
            }
        }

        public CustomizeFolderParts UpdatableParts
        {
            get
            {
                return this.FUpdatebleParts;
            }
        }

        public PanelView? View
        {
            get
            {
                return this.FView;
            }
            set
            {
                if ((this.FUpdatebleParts & CustomizeFolderParts.View) == 0)
                {
                    throw new NotSupportedException();
                }
                this.FView = value;
            }
        }
    }
}

