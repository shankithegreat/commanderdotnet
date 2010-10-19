namespace Nomad.Commons.IO
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class Ini
    {
        private List<string> Header;
        private Dictionary<string, IniSection> Sections = new Dictionary<string, IniSection>(StringComparer.OrdinalIgnoreCase);

        public IniSection AddSection(string sectionName)
        {
            return this.AddSection(sectionName, new List<string>());
        }

        public IniSection AddSection(string sectionName, IList<string> content)
        {
            if (sectionName == null)
            {
                throw new ArgumentNullException("sectionName");
            }
            if (sectionName == string.Empty)
            {
                throw new ArgumentException("sectionName is empty");
            }
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            for (int i = content.Count - 1; i >= 0; i--)
            {
                if (content[i] == string.Empty)
                {
                    content.RemoveAt(i);
                }
            }
            IniSection section = new IniSection(content);
            this.Sections.Add(sectionName, section);
            return section;
        }

        public void Clear()
        {
            this.Header = null;
            this.Sections.Clear();
        }

        public bool ContainsSection(string sectionName)
        {
            return this.Sections.ContainsKey(sectionName);
        }

        public string Get(string sectionName, string keyName)
        {
            IniSection section = this[sectionName];
            if (section != null)
            {
                return section[keyName];
            }
            return null;
        }

        public IEnumerable<string> GetSectionNames()
        {
            return this.Sections.Keys;
        }

        public void Read(IniReader reader)
        {
            this.Clear();
            string sectionName = null;
            List<string> content = null;
            while (reader.Read())
            {
                switch (reader.ElementType)
                {
                    case IniElementType.None:
                        break;

                    case IniElementType.Section:
                        if (sectionName != null)
                        {
                            this.AddSection(sectionName, content);
                        }
                        sectionName = reader.CurrentSection;
                        content = new List<string>();
                        break;

                    default:
                        if (content != null)
                        {
                            content.Add(reader.CurrentLine);
                        }
                        else
                        {
                            if (this.Header == null)
                            {
                                this.Header = new List<string>();
                            }
                            this.Header.Add(reader.CurrentLine);
                        }
                        break;
                }
            }
            if (sectionName != null)
            {
                this.AddSection(sectionName, content);
            }
        }

        public void Read(TextReader reader)
        {
            this.Read(new IniReader(reader));
        }

        public static IDictionary<string, string> ReadSection(TextReader reader, string section)
        {
            section = string.Format("[{0}]", section);
            Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string key = null;
            bool flag = false;
            while ((key = reader.ReadLine()) != null)
            {
                if (key.Equals(section, StringComparison.OrdinalIgnoreCase))
                {
                    flag = true;
                }
                else if (flag)
                {
                    if ((key == string.Empty) || (key[0] == '['))
                    {
                        break;
                    }
                    int index = key.IndexOf('=');
                    if (index >= 0)
                    {
                        dictionary.Add(key.Substring(0, index), key.Substring(index + 1));
                    }
                    else
                    {
                        dictionary.Add(key, null);
                    }
                }
            }
            return ((dictionary.Count > 0) ? dictionary : null);
        }

        public static string ReadValue(TextReader reader, string section, string value)
        {
            IniReader reader2 = new IniReader(reader);
            return reader2.ReadValue(section, value);
        }

        public static string ReadValue(string fileName, string section, string value)
        {
            using (TextReader reader = new StreamReader(fileName, Encoding.Default, true))
            {
                return ReadValue(reader, section, value);
            }
        }

        public bool RemoveSection(string sectionName)
        {
            return this.Sections.Remove(sectionName);
        }

        public void Set(string sectionName, string keyName, string value)
        {
            IniSection section = this[sectionName];
            if (section == null)
            {
                section = new IniSection(new List<string>());
                this.Sections.Add(sectionName, section);
            }
            section[keyName] = value;
        }

        public void Write(IniWriter writer)
        {
            if ((this.Header != null) && (this.Header.Count > 0))
            {
                foreach (string str in this.Header)
                {
                    writer.WriteLine(str);
                }
                writer.WriteLine();
            }
            foreach (KeyValuePair<string, IniSection> pair in this.Sections)
            {
                if (!(this.CompactWrite && (pair.Value.Count <= 0)))
                {
                    writer.WriteSection(pair.Key);
                    pair.Value.Write(writer);
                    writer.WriteLine();
                }
            }
        }

        public void Write(TextWriter writer)
        {
            this.Write(new IniWriter(writer));
        }

        private static void WriteValue(StringBuilder builder, string key, string value)
        {
            if (value != null)
            {
                builder.Append(key);
                builder.Append('=');
                builder.AppendLine(value);
            }
        }

        public static void WriteValue(string fileName, string section, string key, string value)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (section == null)
            {
                throw new ArgumentNullException("section");
            }
            if (section == string.Empty)
            {
                throw new ArgumentException("section");
            }
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if ((key == string.Empty) || (key.IndexOf('=') >= 0))
            {
                throw new ArgumentException("key");
            }
            bool flag = File.Exists(fileName);
            bool flag2 = false;
            StringBuilder builder = new StringBuilder();
            if (flag)
            {
                using (IniReader reader = new IniReader(new StreamReader(fileName)))
                {
                    bool flag3 = false;
                    while (reader.Read())
                    {
                        switch (reader.ElementType)
                        {
                            case IniElementType.Section:
                                if (!flag3)
                                {
                                    break;
                                }
                                if (!flag2)
                                {
                                    WriteValue(builder, key, value);
                                    flag2 = true;
                                }
                                goto Label_012D;

                            case IniElementType.KeyValuePair:
                            {
                                if (!reader.CheckCurrentKey(section, key))
                                {
                                    goto Label_012D;
                                }
                                WriteValue(builder, key, value);
                                flag2 = true;
                                continue;
                            }
                            default:
                                goto Label_012D;
                        }
                        flag3 = reader.CheckCurrentSection(section);
                    Label_012D:
                        builder.AppendLine(reader.CurrentLine);
                    }
                }
            }
            if (!flag2 && (value != null))
            {
                if (builder.Length > 0)
                {
                    builder.AppendLine();
                }
                builder.Append('[');
                builder.Append(section);
                builder.Append(']');
                builder.AppendLine();
                WriteValue(builder, key, value);
            }
            string str = builder.ToString();
            builder = null;
            if (string.IsNullOrEmpty(str) || string.Equals(str, "[" + section + "]" + Environment.NewLine, StringComparison.OrdinalIgnoreCase))
            {
                if (flag)
                {
                    File.Delete(fileName);
                }
            }
            else
            {
                using (TextWriter writer = new StreamWriter(fileName))
                {
                    writer.Write(str);
                }
            }
        }

        public bool CompactWrite { get; set; }

        public bool HasValues
        {
            get
            {
                foreach (IniSection section in this.Sections.Values)
                {
                    if (section.HasValues)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public IniSection this[string sectionName]
        {
            get
            {
                IniSection section;
                if (this.Sections.TryGetValue(sectionName, out section))
                {
                    return section;
                }
                return null;
            }
        }

        public int SectionCount
        {
            get
            {
                return this.Sections.Count;
            }
        }

        public class IniSection : IEnumerable<string>, IEnumerable<KeyValuePair<string, string>>, IEnumerable
        {
            private IList<string> SectionLines;
            private Dictionary<string, int> SectionMap;

            internal IniSection(IList<string> lines)
            {
                if (lines == null)
                {
                    throw new ArgumentNullException();
                }
                this.SectionLines = lines;
            }

            public bool Contains(string keyName)
            {
                if (this.SectionMap == null)
                {
                    this.RebuildSectionMap();
                }
                return this.SectionMap.ContainsKey(keyName);
            }

            private void RebuildSectionMap()
            {
                this.SectionMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < this.SectionLines.Count; i++)
                {
                    int index = this.SectionLines[i].IndexOf('=');
                    if (index > 0)
                    {
                        this.SectionMap.Add(this.SectionLines[i].Substring(0, index), i);
                    }
                }
            }

            public bool Remove(string keyName)
            {
                int num;
                if (this.SectionMap == null)
                {
                    this.RebuildSectionMap();
                }
                if (!((this.SectionMap != null) && this.SectionMap.TryGetValue(keyName, out num)))
                {
                    return false;
                }
                this.RemoveAt(num);
                return true;
            }

            public void RemoveAt(int index)
            {
                this.SectionLines.RemoveAt(index);
                this.SectionMap = null;
            }

            IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
            {
                return new GetEnumerator>d__0(0) { <>4__this = this };
            }

            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                return this.SectionLines.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.SectionLines.GetEnumerator();
            }

            public void Write(IniWriter writer)
            {
                foreach (string str in this.SectionLines)
                {
                    writer.WriteLine(str);
                }
            }

            public int Count
            {
                get
                {
                    return this.SectionLines.Count;
                }
            }

            public bool HasValues
            {
                get
                {
                    if (this.SectionMap == null)
                    {
                        this.RebuildSectionMap();
                    }
                    return (this.SectionMap.Count > 0);
                }
            }

            public string this[string keyName]
            {
                get
                {
                    int num;
                    if (this.SectionMap == null)
                    {
                        this.RebuildSectionMap();
                    }
                    if (!((this.SectionMap != null) && this.SectionMap.TryGetValue(keyName, out num)))
                    {
                        return null;
                    }
                    int index = this.SectionLines[num].IndexOf('=');
                    Debug.Assert(index > 0);
                    return this.SectionLines[num].Substring(index + 1);
                }
                set
                {
                    int num;
                    if (this.SectionMap == null)
                    {
                        this.RebuildSectionMap();
                    }
                    StringBuilder builder = null;
                    if (value != null)
                    {
                        builder = new StringBuilder();
                        builder.Append(keyName);
                        builder.Append('=');
                        builder.Append(value);
                    }
                    if (this.SectionMap.TryGetValue(keyName, out num))
                    {
                        if (builder != null)
                        {
                            this.SectionLines[num] = builder.ToString();
                        }
                        else
                        {
                            this.RemoveAt(num);
                        }
                    }
                    else if (builder != null)
                    {
                        this.SectionLines.Add(builder.ToString());
                        this.SectionMap.Add(keyName, this.SectionLines.Count - 1);
                    }
                }
            }

            public string this[int index]
            {
                get
                {
                    return this.SectionLines[index];
                }
                set
                {
                    if (value == null)
                    {
                        this.SectionLines.RemoveAt(index);
                    }
                    else
                    {
                        this.SectionLines[index] = value;
                    }
                    this.SectionMap = null;
                }
            }

            public string[] Keys
            {
                get
                {
                    if (this.SectionMap == null)
                    {
                        this.RebuildSectionMap();
                    }
                    string[] array = new string[(this.SectionMap == null) ? 0 : this.SectionMap.Keys.Count];
                    if (array.Length > 0)
                    {
                        this.SectionMap.Keys.CopyTo(array, 0);
                    }
                    return array;
                }
            }

            [CompilerGenerated]
            private sealed class GetEnumerator>d__0 : IEnumerator<KeyValuePair<string, string>>, IEnumerator, IDisposable
            {
                private int <>1__state;
                private KeyValuePair<string, string> <>2__current;
                public Ini.IniSection <>4__this;
                public int <DelimPos>5__2;
                public int <I>5__1;
                public string <Key>5__3;
                public string <Value>5__4;

                [DebuggerHidden]
                public GetEnumerator>d__0(int <>1__state)
                {
                    this.<>1__state = <>1__state;
                }

                private bool MoveNext()
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<I>5__1 = 0;
                            while (this.<I>5__1 < this.<>4__this.SectionLines.Count)
                            {
                                this.<DelimPos>5__2 = this.<>4__this.SectionLines[this.<I>5__1].IndexOf('=');
                                if (this.<DelimPos>5__2 <= 0)
                                {
                                    goto Label_00E7;
                                }
                                this.<Key>5__3 = this.<>4__this.SectionLines[this.<I>5__1].Substring(0, this.<DelimPos>5__2);
                                this.<Value>5__4 = this.<>4__this.SectionLines[this.<I>5__1].Substring(this.<DelimPos>5__2 + 1);
                                this.<>2__current = new KeyValuePair<string, string>(this.<Key>5__3, this.<Value>5__4);
                                this.<>1__state = 1;
                                return true;
                            Label_00DF:
                                this.<>1__state = -1;
                            Label_00E7:
                                this.<I>5__1++;
                            }
                            break;

                        case 1:
                            goto Label_00DF;
                    }
                    return false;
                }

                [DebuggerHidden]
                void IEnumerator.Reset()
                {
                    throw new NotSupportedException();
                }

                void IDisposable.Dispose()
                {
                }

                KeyValuePair<string, string> IEnumerator<KeyValuePair<string, string>>.Current
                {
                    [DebuggerHidden]
                    get
                    {
                        return this.<>2__current;
                    }
                }

                object IEnumerator.Current
                {
                    [DebuggerHidden]
                    get
                    {
                        return this.<>2__current;
                    }
                }
            }
        }
    }
}

