namespace Nomad.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class TwoPanelLayout
    {
        [DefaultValue(0)]
        public Nomad.Configuration.ActivePanel ActivePanel;
        private PanelLayout FLeftLayout;
        private PanelLayout FRightLayout;
        [XmlAttribute("name")]
        public string Name;
        [DefaultValue(false)]
        public bool OnePanel;
        [DefaultValue(1)]
        public Orientation PanelsOrientation = Orientation.Vertical;
        [DefaultValue(500)]
        public int SplitterPercent = 500;
        public TwoPanelLayoutEntry StoreEntry;

        public override string ToString()
        {
            return this.Name;
        }

        public PanelLayout LeftLayout
        {
            get
            {
                if (this.FLeftLayout == null)
                {
                    this.FLeftLayout = new PanelLayout();
                }
                return this.FLeftLayout;
            }
            set
            {
                this.FLeftLayout = value;
            }
        }

        public PanelLayout RightLayout
        {
            get
            {
                if (this.FRightLayout == null)
                {
                    this.FRightLayout = new PanelLayout();
                }
                return this.FRightLayout;
            }
            set
            {
                this.FRightLayout = value;
            }
        }
    }
}

