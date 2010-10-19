namespace Nomad.Configuration
{
    using Nomad.FileSystem.Property;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    [Serializable]
    public class ListViewColumnInfo : ICloneable, IEquatable<ListViewColumnInfo>
    {
        [XmlAttribute, DefaultValue(-1)]
        public int DefaultWidth;
        [XmlAttribute, DefaultValue(-1)]
        public int DisplayIndex;
        [NonSerialized]
        private VirtualProperty FProperty;
        [XmlIgnore]
        public int PropertyId;
        [XmlAttribute, DefaultValue(typeof(HorizontalAlignment), "Left")]
        public HorizontalAlignment TextAlign;
        [XmlAttribute, DefaultValue(false)]
        public bool Visible;
        [DefaultValue(0x48), XmlAttribute]
        public int Width;

        public ListViewColumnInfo()
        {
            this.DefaultWidth = -1;
            this.DisplayIndex = -1;
            this.Width = 0x48;
        }

        public ListViewColumnInfo(int propertyId, int width, bool visible)
        {
            this.DefaultWidth = -1;
            this.DisplayIndex = -1;
            this.Width = 0x48;
            this.PropertyId = propertyId;
            this.Width = width;
            this.Visible = visible;
        }

        public ListViewColumnInfo(int propertyId, HorizontalAlignment textAlign, int width, bool visible)
        {
            this.DefaultWidth = -1;
            this.DisplayIndex = -1;
            this.Width = 0x48;
            this.PropertyId = propertyId;
            this.TextAlign = textAlign;
            this.Width = width;
            this.Visible = visible;
        }

        public object Clone()
        {
            return base.MemberwiseClone();
        }

        public bool Equals(ListViewColumnInfo other)
        {
            return this.Equals(other, true);
        }

        public bool Equals(ListViewColumnInfo other, bool compareWidth)
        {
            return (((((other != null) && (other.PropertyId == this.PropertyId)) && ((other.DefaultWidth == this.DefaultWidth) && (other.DisplayIndex == this.DisplayIndex))) && ((other.TextAlign == this.TextAlign) && (!compareWidth || (other.Width == this.Width)))) && (other.Visible == this.Visible));
        }

        public bool IsEmpty
        {
            get
            {
                return ((((this.DefaultWidth < 0) && (this.DisplayIndex < 0)) && ((this.TextAlign == HorizontalAlignment.Left) && (this.Width == 0x48))) && !this.Visible);
            }
        }

        public VirtualProperty Property
        {
            get
            {
                if (this.FProperty == null)
                {
                    this.FProperty = VirtualProperty.Get(this.PropertyId);
                }
                return this.FProperty;
            }
        }

        [XmlAttribute("Property"), EditorBrowsable(EditorBrowsableState.Never)]
        public string PropertyStr
        {
            get
            {
                return VirtualProperty.Get(this.PropertyId).PropertyName;
            }
            set
            {
                this.PropertyId = VirtualProperty.Get(value).PropertyId;
            }
        }
    }
}

