namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad;
    using Nomad.Commons.Drawing;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Xml.Serialization;

    public class VirtualHighligher : NamedFilter
    {
        [DefaultValue(false)]
        public bool AlphaBlend;
        [XmlIgnore]
        public Color BlendColor;
        [DefaultValue((float) 0.5f)]
        public float BlendLevel;
        private Nomad.Commons.Drawing.IconLocation FIconLocation;
        private string FIconLocationStr;
        private IDictionary<Size, Image> FIcons;
        [DefaultValue(2)]
        public HighlighterIconType IconType;

        public VirtualHighligher()
        {
            this.IconType = HighlighterIconType.ExtractedIcon;
            this.BlendColor = Color.White;
            this.BlendLevel = 0.5f;
        }

        public VirtualHighligher(string name, IVirtualItemFilter filter) : base(name, filter)
        {
            this.IconType = HighlighterIconType.ExtractedIcon;
            this.BlendColor = Color.White;
            this.BlendLevel = 0.5f;
        }

        public override bool Equals(IFilterContainter other)
        {
            VirtualHighligher highligher = other as VirtualHighligher;
            return (((((highligher != null) && base.Equals(other)) && ((this.IconType == highligher.IconType) && Nomad.Commons.Drawing.IconLocation.Equals(this.FIconLocation, highligher.FIconLocation))) && ((this.AlphaBlend == highligher.AlphaBlend) && (this.BlendColor == highligher.BlendColor))) && (this.BlendLevel == highligher.BlendLevel));
        }

        public Image GetIcon(Size size)
        {
            Image image;
            if (this.FIconLocation == null)
            {
                return null;
            }
            if ((this.FIcons == null) || !this.FIcons.TryGetValue(size, out image))
            {
                image = CustomImageProvider.LoadIconFromLocation(this.FIconLocation, size);
                if (this.FIcons == null)
                {
                    this.FIcons = IconCollection.Create();
                }
                this.FIcons.Add(size, image);
            }
            return image;
        }

        [XmlElement("Icon"), EditorBrowsable(EditorBrowsableState.Never)]
        public string IconLocation
        {
            get
            {
                return this.FIconLocationStr;
            }
            set
            {
                if (!(this.FIconLocationStr == value))
                {
                    this.FIconLocationStr = value;
                    this.FIconLocation = Nomad.Commons.Drawing.IconLocation.TryParse(Environment.ExpandEnvironmentVariables(this.FIconLocationStr));
                    this.FIcons = null;
                }
            }
        }

        [DefaultValue("White"), EditorBrowsable(EditorBrowsableState.Never), XmlElement("BlendColor")]
        public string SerializableBlendColor
        {
            get
            {
                return TypeDescriptor.GetConverter(typeof(Color)).ConvertToInvariantString(this.BlendColor);
            }
            set
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
                this.BlendColor = (Color) converter.ConvertFromInvariantString(value);
            }
        }
    }
}

