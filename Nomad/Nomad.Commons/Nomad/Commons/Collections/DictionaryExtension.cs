namespace Nomad.Commons.Collections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Xml;
    using System.Xml.Serialization;

    public static class DictionaryExtension
    {
        private const string ElementItem = "item";
        private const string ElementKey = "key";
        private const string ElementValue = "value";

        private static void CreateConverters(Type type, out TypeConverter converter, out XmlSerializer serializer)
        {
            converter = TypeDescriptor.GetConverter(type);
            Type type2 = converter.GetType();
            if (!((((type2 != typeof(TypeConverter)) && (type2 != typeof(CollectionConverter))) && !type2.IsSubclassOf(typeof(CollectionConverter))) && converter.CanConvertTo(typeof(string))))
            {
                converter = null;
            }
            serializer = (converter == null) ? new XmlSerializer(type) : null;
        }

        public static void DeserializeData<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, XmlReader reader)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            if (reader.IsEmptyElement)
            {
                reader.Read();
            }
            else
            {
                TypeConverter converter;
                TypeConverter converter2;
                XmlSerializer serializer;
                XmlSerializer serializer2;
                CreateConverters(typeof(TKey), out converter, out serializer);
                CreateConverters(typeof(TValue), out converter2, out serializer2);
                for (bool flag = reader.ReadToFollowing("item"); flag; flag = reader.ReadToNextSibling("item"))
                {
                    TKey key = default(TKey);
                    TValue local2 = default(TValue);
                    if (converter != null)
                    {
                        key = (TKey) converter.ConvertFromInvariantString(reader.GetAttribute("key"));
                    }
                    if (converter2 != null)
                    {
                        local2 = (TValue) converter2.ConvertFromInvariantString(reader.GetAttribute("value"));
                    }
                    if (!reader.IsEmptyElement)
                    {
                        reader.Read();
                    }
                    if (serializer != null)
                    {
                        reader.ReadStartElement("key");
                        key = (TKey) serializer.Deserialize(reader);
                        reader.ReadEndElement();
                    }
                    if (serializer2 != null)
                    {
                        reader.ReadStartElement("value");
                        local2 = (TValue) serializer2.Deserialize(reader);
                        reader.ReadEndElement();
                    }
                    dictionary.Add(key, local2);
                }
                reader.ReadEndElement();
            }
        }

        public static int RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> match)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException();
            }
            List<TKey> list = null;
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                if (match(pair))
                {
                    if (list == null)
                    {
                        list = new List<TKey>();
                    }
                    list.Add(pair.Key);
                }
            }
            if (list == null)
            {
                return 0;
            }
            int num = 0;
            foreach (TKey local in list)
            {
                if (dictionary.Remove(local))
                {
                    num++;
                }
            }
            return num;
        }

        public static void SerializeData<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, XmlWriter writer)
        {
            TypeConverter converter;
            TypeConverter converter2;
            XmlSerializer serializer;
            XmlSerializer serializer2;
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            CreateConverters(typeof(TKey), out converter, out serializer);
            CreateConverters(typeof(TValue), out converter2, out serializer2);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                writer.WriteStartElement("item");
                if (converter != null)
                {
                    writer.WriteAttributeString("key", converter.ConvertToInvariantString(pair.Key));
                }
                if (converter2 != null)
                {
                    writer.WriteAttributeString("value", converter2.ConvertToInvariantString(pair.Value));
                }
                if (serializer != null)
                {
                    writer.WriteStartElement("key");
                    serializer.Serialize(writer, pair.Key, namespaces);
                    writer.WriteEndElement();
                }
                if (serializer2 != null)
                {
                    writer.WriteStartElement("value");
                    serializer2.Serialize(writer, pair.Value, namespaces);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }
    }
}

