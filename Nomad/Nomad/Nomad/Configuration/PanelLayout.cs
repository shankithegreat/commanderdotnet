namespace Nomad.Configuration
{
    using Nomad.FileSystem.Virtual;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class PanelLayout
    {
        [DefaultValue(true)]
        public bool AutoSizeColumns = true;
        [DefaultValue((string) null)]
        public ListViewColumnInfo[] Columns;
        [DefaultValue(1)]
        public Orientation FolderBarOrientation = Orientation.Vertical;
        [DefaultValue(false)]
        public bool FolderBarVisible;
        [DefaultValue(3)]
        public int ListColumnCount = 3;
        [DefaultValue(500)]
        public int SplitterPercent = 500;
        [DefaultValue(0)]
        public PanelLayoutEntry StoreEntry;
        [XmlIgnore]
        public Size ThumbnailSize;
        [XmlIgnore]
        public Size ThumbnailSpacing;
        [DefaultValue(3)]
        public PanelToolbar ToolbarsVisible = (PanelToolbar.Item | PanelToolbar.Folder);
        [DefaultValue(3)]
        public PanelView View = PanelView.List;

        private bool ShouldSerializeThumbnailSize()
        {
            return ((this.ThumbnailSize.Width != 0x60) || (this.ThumbnailSize.Height != 0x60));
        }

        private bool ShouldSerializeThumbnailSpacing()
        {
            return ((this.ThumbnailSpacing.Width > this.ThumbnailSize.Width) || (this.ThumbnailSpacing.Height > this.ThumbnailSize.Height));
        }

        [XmlElement("ThumbnailSize"), EditorBrowsable(EditorBrowsableState.Never)]
        public string SerializableThumbnailSize
        {
            get
            {
                return (this.ShouldSerializeThumbnailSize() ? XmlSerializable.ObjectToString<Size>(this.ThumbnailSize) : null);
            }
            set
            {
                this.ThumbnailSize = XmlSerializable.StringToObject<Size>(value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), XmlElement("ThumbnailSpacing")]
        public string SerializableThumbnailSpacing
        {
            get
            {
                return (this.ShouldSerializeThumbnailSpacing() ? XmlSerializable.ObjectToString<Size>(this.ThumbnailSpacing) : null);
            }
            set
            {
                this.ThumbnailSpacing = XmlSerializable.StringToObject<Size>(value);
            }
        }
    }
}

