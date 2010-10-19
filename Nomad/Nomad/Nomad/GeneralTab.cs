namespace Nomad
{
    using Nomad.Configuration;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    [Serializable]
    public class GeneralTab
    {
        public string Caption;
        [XmlIgnore]
        public Keys Hotkey;

        [EditorBrowsable(EditorBrowsableState.Never), XmlElement("Hotkey")]
        public string SerializableHotkey
        {
            get
            {
                return XmlSerializable.ObjectToString<Keys>(this.Hotkey);
            }
            set
            {
                this.Hotkey = XmlSerializable.StringToObject<Keys>(value);
            }
        }
    }
}

