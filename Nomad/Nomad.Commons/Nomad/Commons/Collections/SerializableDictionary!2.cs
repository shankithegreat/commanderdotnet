namespace Nomad.Commons.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    [XmlRoot("Dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            this.DeserializeData<TKey, TValue>(reader);
        }

        public void WriteXml(XmlWriter writer)
        {
            this.SerializeData<TKey, TValue>(writer);
        }
    }
}

