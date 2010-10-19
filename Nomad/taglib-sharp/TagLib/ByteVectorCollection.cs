namespace TagLib
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [ComVisible(false)]
    public class ByteVectorCollection : ListBase<ByteVector>
    {
        public ByteVectorCollection()
        {
        }

        public ByteVectorCollection(IEnumerable<ByteVector> list)
        {
            if (list != null)
            {
                base.Add(list);
            }
        }

        public ByteVectorCollection(params ByteVector[] list)
        {
            if (list != null)
            {
                base.Add(list);
            }
        }

        public override void SortedInsert(ByteVector item, bool unique)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            int num = 0;
            while (num < this.Count)
            {
                if ((item == this[num]) && unique)
                {
                    return;
                }
                if (item >= this[num])
                {
                    break;
                }
                num++;
            }
            this.Insert(num + 1, item);
        }

        public static ByteVectorCollection Split(ByteVector vector, ByteVector pattern)
        {
            return Split(vector, pattern, 1);
        }

        public static ByteVectorCollection Split(ByteVector vector, ByteVector pattern, int byteAlign)
        {
            return Split(vector, pattern, byteAlign, 0);
        }

        public static ByteVectorCollection Split(ByteVector vector, ByteVector pattern, int byteAlign, int max)
        {
            if (vector == null)
            {
                throw new ArgumentNullException("vector");
            }
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            if (byteAlign < 1)
            {
                throw new ArgumentOutOfRangeException("byteAlign", "byteAlign must be at least 1.");
            }
            ByteVectorCollection vectors = new ByteVectorCollection();
            int startIndex = 0;
            for (int i = vector.Find(pattern, 0, byteAlign); (i != -1) && ((max < 1) || (max > (vectors.Count + 1))); i = vector.Find(pattern, i + pattern.Count, byteAlign))
            {
                vectors.Add(vector.Mid(startIndex, i - startIndex));
                startIndex = i + pattern.Count;
            }
            if (startIndex < vector.Count)
            {
                vectors.Add(vector.Mid(startIndex, vector.Count - startIndex));
            }
            return vectors;
        }

        public ByteVector ToByteVector(ByteVector separator)
        {
            if (separator == null)
            {
                throw new ArgumentNullException("separator");
            }
            ByteVector vector = new ByteVector();
            for (int i = 0; i < this.Count; i++)
            {
                if ((i != 0) && (separator.Count > 0))
                {
                    vector.Add(separator);
                }
                vector.Add(this[i]);
            }
            return vector;
        }
    }
}

