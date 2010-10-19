namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Xml.Serialization;

    public abstract class XmlSerializer1 : XmlSerializer
    {
        protected XmlSerializer1()
        {
        }

        protected override XmlSerializationReader CreateReader()
        {
            return new XmlSerializationReader1();
        }

        protected override XmlSerializationWriter CreateWriter()
        {
            return new XmlSerializationWriter1();
        }
    }
}

