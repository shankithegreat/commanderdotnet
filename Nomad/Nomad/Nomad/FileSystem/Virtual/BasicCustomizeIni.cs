namespace Nomad.FileSystem.Virtual
{
    using Nomad.Commons.IO;
    using Nomad.Configuration;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class BasicCustomizeIni : PersistentIni
    {
        private const string KeyAutoSizeColumns = "AutoSizeColumns";
        private const string KeyListColumnCount = "ListColumnCount";
        private const string KeySort = "Sort";
        private const string KeyThumbnailSize = "ThumbnailSize";
        private const string KeyThumbnailSpacing = "ThumbnailSpacing";
        private const string KeyView = "View";
        protected const string SectionNomad = "Nomad";
        private const string SectionNomadColumns = "Nomad.Columns";
        private const string SectionNomadFilter = "Nomad.Filter";

        public BasicCustomizeIni(string iniPath) : base(iniPath)
        {
        }

        private T GetValue<T>(string sectionName, string keyName)
        {
            string text = base.Get(sectionName, keyName);
            if (text == null)
            {
                return default(T);
            }
            return (T) TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(text);
        }

        private static List<string> Serialize(object value)
        {
            StringBuilder stringBuilder;
            XmlSerializer serializer = new XmlSerializer(value.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize((TextWriter) writer, value);
                stringBuilder = writer.GetStringBuilder();
            }
            List<string> list = new List<string>();
            using (StringReader reader = new StringReader(stringBuilder.ToString()))
            {
                stringBuilder = null;
                string item = reader.ReadLine();
                while ((item = reader.ReadLine()) != null)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        private void SetValue<T>(string sectionName, string keyName, T value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            base.Set(sectionName, keyName, converter.ConvertToInvariantString(value));
        }

        public bool? AutoSizeColumns
        {
            get
            {
                string str = base.Get("Nomad", "AutoSizeColumns");
                if (str == null)
                {
                    return null;
                }
                return new bool?(Convert.ToBoolean(str));
            }
            set
            {
                base.Set("Nomad", "AutoSizeColumns", !value.HasValue ? null : value.Value.ToString());
            }
        }

        public ListViewColumnInfo[] Columns
        {
            get
            {
                ListViewColumnInfo[] infoArray;
                Ini.IniSection section = base["Nomad.Columns"];
                if (section == null)
                {
                    return null;
                }
                try
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (string str in (IEnumerable<string>) section)
                    {
                        builder.AppendLine(str);
                    }
                    XmlSerializer serializer = new XmlSerializer(typeof(ListViewColumnInfo[]));
                    using (TextReader reader = new StringReader(builder.ToString()))
                    {
                        builder = null;
                        infoArray = (ListViewColumnInfo[]) serializer.Deserialize(reader);
                    }
                }
                catch (InvalidOperationException)
                {
                    infoArray = null;
                }
                return infoArray;
            }
            set
            {
                base.RemoveSection("Nomad.Columns");
                if ((value != null) && (value.Length != 0))
                {
                    base.AddSection("Nomad.Columns", Serialize(value));
                }
            }
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                IVirtualItemFilter filter;
                Ini.IniSection section = base["Nomad.Filter"];
                if (section == null)
                {
                    return null;
                }
                try
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (string str in (IEnumerable<string>) section)
                    {
                        builder.AppendLine(str);
                    }
                    XmlSerializer serializer = new XmlSerializer(typeof(FilterContainer));
                    using (TextReader reader = new StringReader(builder.ToString()))
                    {
                        builder = null;
                        filter = ((FilterContainer) serializer.Deserialize(reader)).Filter;
                    }
                }
                catch (InvalidOperationException)
                {
                    filter = null;
                }
                return filter;
            }
            set
            {
                base.RemoveSection("Nomad.Filter");
                if (value != null)
                {
                    base.AddSection("Nomad.Filter", Serialize(new FilterContainer(value)));
                }
            }
        }

        public int? ListColumnCount
        {
            get
            {
                string str = base.Get("Nomad", "ListColumnCount");
                if (str == null)
                {
                    return null;
                }
                return new int?(Convert.ToInt32(str));
            }
            set
            {
                base.Set("Nomad", "ListColumnCount", !value.HasValue ? null : value.Value.ToString());
            }
        }

        public VirtualItemComparer Sort
        {
            get
            {
                string text = base.Get("Nomad", "Sort");
                if (text == null)
                {
                    return null;
                }
                return (TypeDescriptor.GetConverter(typeof(VirtualItemComparer)).ConvertFromInvariantString(text) as VirtualItemComparer);
            }
            set
            {
                if (value == null)
                {
                    base.Set("Nomad", "Sort", null);
                }
                else
                {
                    this.SetValue<VirtualItemComparer>("Nomad", "Sort", value);
                }
            }
        }

        public Size ThumbnailSize
        {
            get
            {
                return this.GetValue<Size>("Nomad", "ThumbnailSize");
            }
            set
            {
                if (value.IsEmpty)
                {
                    base.Set("Nomad", "ThumbnailSize", null);
                }
                else
                {
                    this.SetValue<Size>("Nomad", "ThumbnailSize", value);
                }
            }
        }

        public Size ThumbnailSpacing
        {
            get
            {
                return this.GetValue<Size>("Nomad", "ThumbnailSpacing");
            }
            set
            {
                if (value.IsEmpty)
                {
                    base.Set("Nomad", "ThumbnailSpacing", null);
                }
                else
                {
                    this.SetValue<Size>("Nomad", "ThumbnailSpacing", value);
                }
            }
        }

        public PanelView? View
        {
            get
            {
                string str = base.Get("Nomad", "View");
                if (str == null)
                {
                    return null;
                }
                try
                {
                    return new PanelView?((PanelView) Enum.Parse(typeof(PanelView), str, true));
                }
                catch (ArgumentException)
                {
                    return null;
                }
            }
            set
            {
                base.Set("Nomad", "View", !value.HasValue ? null : value.Value.ToString());
            }
        }
    }
}

