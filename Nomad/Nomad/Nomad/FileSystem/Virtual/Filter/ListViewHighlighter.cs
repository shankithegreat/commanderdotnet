namespace Nomad.FileSystem.Virtual.Filter
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Xml.Serialization;

    public class ListViewHighlighter : VirtualHighligher
    {
        [XmlIgnore]
        public Color ForeColor;

        public ListViewHighlighter()
        {
            this.ForeColor = Color.Empty;
        }

        public ListViewHighlighter(string name, IVirtualItemFilter filter) : base(name, filter)
        {
            this.ForeColor = Color.Empty;
        }

        public override bool Equals(IFilterContainter other)
        {
            ListViewHighlighter highlighter = other as ListViewHighlighter;
            return (((highlighter != null) && base.Equals(other)) && (this.ForeColor == highlighter.ForeColor));
        }

        [XmlElement("ForeColor"), EditorBrowsable(EditorBrowsableState.Never)]
        public string SerializableForeColor
        {
            get
            {
                return TypeDescriptor.GetConverter(typeof(Color)).ConvertToInvariantString(this.ForeColor);
            }
            set
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
                this.ForeColor = (Color) converter.ConvertFromInvariantString(value);
            }
        }
    }
}

