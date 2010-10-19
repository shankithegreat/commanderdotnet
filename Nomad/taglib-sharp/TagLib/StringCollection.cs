namespace TagLib
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [ComVisible(false)]
    public class StringCollection : ListBase<string>
    {
        public StringCollection()
        {
        }

        public StringCollection(StringCollection values)
        {
            base.Add((ListBase<string>) values);
        }

        public StringCollection(params string[] values)
        {
            base.Add(values);
        }

        public StringCollection(ByteVectorCollection vectorList) : this(vectorList, StringType.UTF8)
        {
        }

        public StringCollection(ByteVectorCollection vectorList, StringType type)
        {
            IEnumerator<ByteVector> enumerator = vectorList.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    this.Add(enumerator.Current.ToString(type));
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
        }

        public static StringCollection Split(string value, string pattern)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            StringCollection strings = new StringCollection();
            int startIndex = 0;
            int index = value.IndexOf(pattern, 0);
            int length = pattern.Length;
            while (index != -1)
            {
                strings.Add(value.Substring(startIndex, index - startIndex));
                startIndex = index + length;
                index = value.IndexOf(pattern, startIndex);
            }
            strings.Add(value.Substring(startIndex));
            return strings;
        }
    }
}

