namespace TagLib
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ByteVector : IEnumerable, IList<byte>, ICollection<byte>, IEnumerable<byte>, IComparable<ByteVector>
    {
        private static uint[] crc_table = new uint[] { 
            0, 0x4c11db7, 0x9823b6e, 0xd4326d9, 0x130476dc, 0x17c56b6b, 0x1a864db2, 0x1e475005, 0x2608edb8, 0x22c9f00f, 0x2f8ad6d6, 0x2b4bcb61, 0x350c9b64, 0x31cd86d3, 0x3c8ea00a, 0x384fbdbd, 
            0x4c11db70, 0x48d0c6c7, 0x4593e01e, 0x4152fda9, 0x5f15adac, 0x5bd4b01b, 0x569796c2, 0x52568b75, 0x6a1936c8, 0x6ed82b7f, 0x639b0da6, 0x675a1011, 0x791d4014, 0x7ddc5da3, 0x709f7b7a, 0x745e66cd, 
            0x9823b6e0, 0x9ce2ab57, 0x91a18d8e, 0x95609039, 0x8b27c03c, 0x8fe6dd8b, 0x82a5fb52, 0x8664e6e5, 0xbe2b5b58, 0xbaea46ef, 0xb7a96036, 0xb3687d81, 0xad2f2d84, 0xa9ee3033, 0xa4ad16ea, 0xa06c0b5d, 
            0xd4326d90, 0xd0f37027, 0xddb056fe, 0xd9714b49, 0xc7361b4c, 0xc3f706fb, 0xceb42022, 0xca753d95, 0xf23a8028, 0xf6fb9d9f, 0xfbb8bb46, 0xff79a6f1, 0xe13ef6f4, 0xe5ffeb43, 0xe8bccd9a, 0xec7dd02d, 
            0x34867077, 0x30476dc0, 0x3d044b19, 0x39c556ae, 0x278206ab, 0x23431b1c, 0x2e003dc5, 0x2ac12072, 0x128e9dcf, 0x164f8078, 0x1b0ca6a1, 0x1fcdbb16, 0x18aeb13, 0x54bf6a4, 0x808d07d, 0xcc9cdca, 
            0x7897ab07, 0x7c56b6b0, 0x71159069, 0x75d48dde, 0x6b93dddb, 0x6f52c06c, 0x6211e6b5, 0x66d0fb02, 0x5e9f46bf, 0x5a5e5b08, 0x571d7dd1, 0x53dc6066, 0x4d9b3063, 0x495a2dd4, 0x44190b0d, 0x40d816ba, 
            0xaca5c697, 0xa864db20, 0xa527fdf9, 0xa1e6e04e, 0xbfa1b04b, 0xbb60adfc, 0xb6238b25, 0xb2e29692, 0x8aad2b2f, 0x8e6c3698, 0x832f1041, 0x87ee0df6, 0x99a95df3, 0x9d684044, 0x902b669d, 0x94ea7b2a, 
            0xe0b41de7, 0xe4750050, 0xe9362689, 0xedf73b3e, 0xf3b06b3b, 0xf771768c, 0xfa325055, 0xfef34de2, 0xc6bcf05f, 0xc27dede8, 0xcf3ecb31, 0xcbffd686, 0xd5b88683, 0xd1799b34, 0xdc3abded, 0xd8fba05a, 
            0x690ce0ee, 0x6dcdfd59, 0x608edb80, 0x644fc637, 0x7a089632, 0x7ec98b85, 0x738aad5c, 0x774bb0eb, 0x4f040d56, 0x4bc510e1, 0x46863638, 0x42472b8f, 0x5c007b8a, 0x58c1663d, 0x558240e4, 0x51435d53, 
            0x251d3b9e, 0x21dc2629, 0x2c9f00f0, 0x285e1d47, 0x36194d42, 0x32d850f5, 0x3f9b762c, 0x3b5a6b9b, 0x315d626, 0x7d4cb91, 0xa97ed48, 0xe56f0ff, 0x1011a0fa, 0x14d0bd4d, 0x19939b94, 0x1d528623, 
            0xf12f560e, 0xf5ee4bb9, 0xf8ad6d60, 0xfc6c70d7, 0xe22b20d2, 0xe6ea3d65, 0xeba91bbc, 0xef68060b, 0xd727bbb6, 0xd3e6a601, 0xdea580d8, 0xda649d6f, 0xc423cd6a, 0xc0e2d0dd, 0xcda1f604, 0xc960ebb3, 
            0xbd3e8d7e, 0xb9ff90c9, 0xb4bcb610, 0xb07daba7, 0xae3afba2, 0xaafbe615, 0xa7b8c0cc, 0xa379dd7b, 0x9b3660c6, 0x9ff77d71, 0x92b45ba8, 0x9675461f, 0x8832161a, 0x8cf30bad, 0x81b02d74, 0x857130c3, 
            0x5d8a9099, 0x594b8d2e, 0x5408abf7, 0x50c9b640, 0x4e8ee645, 0x4a4ffbf2, 0x470cdd2b, 0x43cdc09c, 0x7b827d21, 0x7f436096, 0x7200464f, 0x76c15bf8, 0x68860bfd, 0x6c47164a, 0x61043093, 0x65c52d24, 
            0x119b4be9, 0x155a565e, 0x18197087, 0x1cd86d30, 0x29f3d35, 0x65e2082, 0xb1d065b, 0xfdc1bec, 0x3793a651, 0x3352bbe6, 0x3e119d3f, 0x3ad08088, 0x2497d08d, 0x2056cd3a, 0x2d15ebe3, 0x29d4f654, 
            0xc5a92679, 0xc1683bce, 0xcc2b1d17, 0xc8ea00a0, 0xd6ad50a5, 0xd26c4d12, 0xdf2f6bcb, 0xdbee767c, 0xe3a1cbc1, 0xe760d676, 0xea23f0af, 0xeee2ed18, 0xf0a5bd1d, 0xf464a0aa, 0xf9278673, 0xfde69bc4, 
            0x89b8fd09, 0x8d79e0be, 0x803ac667, 0x84fbdbd0, 0x9abc8bd5, 0x9e7d9662, 0x933eb0bb, 0x97ffad0c, 0xafb010b1, 0xab710d06, 0xa6322bdf, 0xa2f33668, 0xbcb4666d, 0xb8757bda, 0xb5365d03, 0xb1f740b4
         };
        private List<byte> data;
        private static Encoding last_utf16_encoding = Encoding.Unicode;
        private static readonly ReadOnlyByteVector td1 = new ReadOnlyByteVector(1);
        private static readonly ReadOnlyByteVector td2 = new ReadOnlyByteVector(2);
        private static bool use_broken_latin1 = false;

        public ByteVector()
        {
            this.data = new List<byte>();
        }

        public ByteVector(int size) : this(size, 0)
        {
        }

        public ByteVector(params byte[] data)
        {
            this.data = new List<byte>();
            if (data != null)
            {
                this.data.AddRange(data);
            }
        }

        public ByteVector(ByteVector vector)
        {
            this.data = new List<byte>();
            if (vector != null)
            {
                this.data.AddRange(vector);
            }
        }

        public ByteVector(byte[] data, int length)
        {
            this.data = new List<byte>();
            if (length > data.Length)
            {
                throw new ArgumentOutOfRangeException("length", "Length exceeds size of data.");
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "Length is less than zero.");
            }
            if (length == data.Length)
            {
                this.data.AddRange(data);
            }
            else
            {
                byte[] destinationArray = new byte[length];
                Array.Copy(data, 0, destinationArray, 0, length);
                this.data.AddRange(destinationArray);
            }
        }

        public ByteVector(int size, byte value)
        {
            this.data = new List<byte>();
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size", "Size is less than zero.");
            }
            if (size != 0)
            {
                byte[] collection = new byte[size];
                for (int i = 0; i < size; i++)
                {
                    collection[i] = value;
                }
                this.data.AddRange(collection);
            }
        }

        public void Add(byte item)
        {
            if (this.IsReadOnly)
            {
                throw new NotSupportedException("Cannot edit readonly objects.");
            }
            this.data.Add(item);
        }

        public void Add(byte[] data)
        {
            if (this.IsReadOnly)
            {
                throw new NotSupportedException("Cannot edit readonly objects.");
            }
            if (data != null)
            {
                this.data.AddRange(data);
            }
        }

        public void Add(ByteVector data)
        {
            if (this.IsReadOnly)
            {
                throw new NotSupportedException("Cannot edit readonly objects.");
            }
            if (data != null)
            {
                this.data.AddRange(data);
            }
        }

        public void Clear()
        {
            if (this.IsReadOnly)
            {
                throw new NotSupportedException("Cannot edit readonly objects.");
            }
            this.data.Clear();
        }

        public int CompareTo(ByteVector other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            int num = this.Count - other.Count;
            for (int i = 0; (num == 0) && (i < this.Count); i++)
            {
                num = this[i] - other[i];
            }
            return num;
        }

        public bool Contains(byte item)
        {
            return this.data.Contains(item);
        }

        public bool ContainsAt(ByteVector pattern, int offset)
        {
            return this.ContainsAt(pattern, offset, 0);
        }

        public bool ContainsAt(ByteVector pattern, int offset, int patternOffset)
        {
            return this.ContainsAt(pattern, offset, patternOffset, 0x7fffffff);
        }

        public bool ContainsAt(ByteVector pattern, int offset, int patternOffset, int patternLength)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            if (pattern.Count < patternLength)
            {
                patternLength = pattern.Count;
            }
            if (((patternLength > this.data.Count) || (offset >= this.data.Count)) || (((patternOffset >= pattern.Count) || (patternLength <= 0)) || (offset < 0)))
            {
                return false;
            }
            for (int i = 0; i < (patternLength - patternOffset); i++)
            {
                if (this.data[i + offset] != pattern[i + patternOffset])
                {
                    return false;
                }
            }
            return true;
        }

        public void CopyTo(byte[] array, int arrayIndex)
        {
            this.data.CopyTo(array, arrayIndex);
        }

        public bool EndsWith(ByteVector pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            return this.ContainsAt(pattern, this.data.Count - pattern.Count);
        }

        public int EndsWithPartialMatch(ByteVector pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            if (pattern.Count <= this.data.Count)
            {
                int num = this.data.Count - pattern.Count;
                for (int i = 1; i < pattern.Count; i++)
                {
                    if (this.ContainsAt(pattern, num + i, 0, pattern.Count - i))
                    {
                        return (num + i);
                    }
                }
            }
            return -1;
        }

        public override bool Equals(object other)
        {
            return ((other is ByteVector) && this.Equals((ByteVector) other));
        }

        public bool Equals(ByteVector other)
        {
            return (this.CompareTo(other) == 0);
        }

        public int Find(ByteVector pattern)
        {
            return this.Find(pattern, 0, 1);
        }

        public int Find(ByteVector pattern, int offset)
        {
            return this.Find(pattern, offset, 1);
        }

        public int Find(ByteVector pattern, int offset, int byteAlign)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", "offset must be at least 0.");
            }
            if (byteAlign < 1)
            {
                throw new ArgumentOutOfRangeException("byteAlign", "byteAlign must be at least 1.");
            }
            if (pattern.Count <= (this.Count - offset))
            {
                if (pattern.Count == 1)
                {
                    byte num = pattern[0];
                    for (int m = offset; m < this.data.Count; m += byteAlign)
                    {
                        if (this.data[m] == num)
                        {
                            return m;
                        }
                    }
                    return -1;
                }
                int[] numArray = new int[0x100];
                for (int i = 0; i < 0x100; i++)
                {
                    numArray[i] = pattern.Count;
                }
                for (int j = 0; j < (pattern.Count - 1); j++)
                {
                    numArray[pattern[j]] = (pattern.Count - j) - 1;
                }
                for (int k = (pattern.Count - 1) + offset; k < this.data.Count; k += numArray[this.data[k]])
                {
                    int num6 = k;
                    int num7 = pattern.Count - 1;
                    while ((num7 >= 0) && (this.data[num6] == pattern[num7]))
                    {
                        num6--;
                        num7--;
                    }
                    if ((num7 == -1) && ((((num6 + 1) - offset) % byteAlign) == 0))
                    {
                        return (num6 + 1);
                    }
                }
            }
            return -1;
        }

        public static ByteVector FromFile(TagLib.File.IFileAbstraction abstraction)
        {
            byte[] buffer;
            return FromFile(abstraction, out buffer, false);
        }

        internal static ByteVector FromFile(TagLib.File.IFileAbstraction abstraction, out byte[] firstChunk, bool copyFirstChunk)
        {
            if (abstraction == null)
            {
                throw new ArgumentNullException("abstraction");
            }
            Stream readStream = abstraction.ReadStream;
            ByteVector vector = FromStream(readStream, out firstChunk, copyFirstChunk);
            abstraction.CloseStream(readStream);
            return vector;
        }

        public static ByteVector FromPath(string path)
        {
            byte[] buffer;
            return FromPath(path, out buffer, false);
        }

        internal static ByteVector FromPath(string path, out byte[] firstChunk, bool copyFirstChunk)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            return FromFile(new TagLib.File.LocalFileAbstraction(path), out firstChunk, copyFirstChunk);
        }

        public static ByteVector FromStream(Stream stream)
        {
            byte[] buffer;
            return FromStream(stream, out buffer, false);
        }

        internal static ByteVector FromStream(Stream stream, out byte[] firstChunk, bool copyFirstChunk)
        {
            int num3;
            ByteVector vector = new ByteVector();
            byte[] array = new byte[0x1000];
            int length = array.Length;
            int num2 = 0;
            bool flag = false;
            firstChunk = null;
            do
            {
                Array.Clear(array, 0, array.Length);
                num3 = stream.Read(array, 0, length);
                vector.Add(array);
                num2 += num3;
                if (!flag)
                {
                    if (copyFirstChunk)
                    {
                        if ((firstChunk == null) || (firstChunk.Length != length))
                        {
                            firstChunk = new byte[length];
                        }
                        Array.Copy(array, 0, firstChunk, 0, num3);
                    }
                    flag = true;
                }
            }
            while (((num2 != stream.Length) || (stream.Length <= 0L)) && ((num3 >= length) || (stream.Length > 0L)));
            if ((stream.Length > 0L) && (vector.Count != stream.Length))
            {
                vector.Resize((int) stream.Length);
            }
            return vector;
        }

        [Obsolete("Use FromString(string,StringType)")]
        public static ByteVector FromString(string text)
        {
            return FromString(text, StringType.UTF8);
        }

        public static ByteVector FromString(string text, int length)
        {
            return FromString(text, StringType.UTF8, length);
        }

        public static ByteVector FromString(string text, StringType type)
        {
            return FromString(text, type, 0x7fffffff);
        }

        public static ByteVector FromString(string text, StringType type, int length)
        {
            ByteVector bom = new ByteVector();
            if (type == StringType.UTF16)
            {
                byte[] data = new byte[] { 0xff, 0xfe };
                bom.Add(data);
            }
            if ((text != null) && (text.Length != 0))
            {
                if (text.Length > length)
                {
                    text = text.Substring(0, length);
                }
                bom.Add(StringTypeToEncoding(type, bom).GetBytes(text));
            }
            return bom;
        }

        public static ByteVector FromUInt(uint value)
        {
            return FromUInt(value, true);
        }

        public static ByteVector FromUInt(uint value, bool mostSignificantByteFirst)
        {
            ByteVector vector = new ByteVector();
            for (int i = 0; i < 4; i++)
            {
                int num2 = !mostSignificantByteFirst ? i : (3 - i);
                vector.Add((byte) ((value >> (num2 * 8)) & 0xff));
            }
            return vector;
        }

        public static ByteVector FromULong(ulong value)
        {
            return FromULong(value, true);
        }

        public static ByteVector FromULong(ulong value, bool mostSignificantByteFirst)
        {
            ByteVector vector = new ByteVector();
            for (int i = 0; i < 8; i++)
            {
                int num2 = !mostSignificantByteFirst ? i : (7 - i);
                vector.Add((byte) ((value >> (num2 * 8)) & ((ulong) 0xffL)));
            }
            return vector;
        }

        public static ByteVector FromUShort(ushort value)
        {
            return FromUShort(value, true);
        }

        public static ByteVector FromUShort(ushort value, bool mostSignificantByteFirst)
        {
            ByteVector vector = new ByteVector();
            for (int i = 0; i < 2; i++)
            {
                int num2 = !mostSignificantByteFirst ? i : (1 - i);
                vector.Add((byte) ((value >> (num2 * 8)) & 0xff));
            }
            return vector;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return (int) this.Checksum;
        }

        public int IndexOf(byte item)
        {
            return this.data.IndexOf(item);
        }

        public void Insert(int index, byte item)
        {
            if (this.IsReadOnly)
            {
                throw new NotSupportedException("Cannot edit readonly objects.");
            }
            this.data.Insert(index, item);
        }

        public void Insert(int index, byte[] data)
        {
            if (this.IsReadOnly)
            {
                throw new NotSupportedException("Cannot edit readonly objects.");
            }
            if (data != null)
            {
                this.data.InsertRange(index, data);
            }
        }

        public void Insert(int index, ByteVector data)
        {
            if (this.IsReadOnly)
            {
                throw new NotSupportedException("Cannot edit readonly objects.");
            }
            if (data != null)
            {
                this.data.InsertRange(index, data);
            }
        }

        public ByteVector Mid(int index)
        {
            return this.Mid(index, this.Count - index);
        }

        public ByteVector Mid(int startIndex, int length)
        {
            if ((startIndex < 0) || (startIndex > this.Count))
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }
            if ((length < 0) || ((startIndex + length) > this.Count))
            {
                throw new ArgumentOutOfRangeException("length");
            }
            if (length == 0)
            {
                return new ByteVector();
            }
            if ((startIndex + length) > this.data.Count)
            {
                length = this.data.Count - startIndex;
            }
            byte[] array = new byte[length];
            this.data.CopyTo(startIndex, array, 0, length);
            return array;
        }

        public static ByteVector operator +(ByteVector first, ByteVector second)
        {
            return new ByteVector(first) { second };
        }

        public static bool operator ==(ByteVector first, ByteVector second)
        {
            bool flag = first == null;
            bool flag2 = second == null;
            return ((flag && flag2) || ((!flag && !flag2) && first.Equals(second)));
        }

        public static bool operator >(ByteVector first, ByteVector second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            return (first.CompareTo(second) > 0);
        }

        public static bool operator >=(ByteVector first, ByteVector second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            return (first.CompareTo(second) >= 0);
        }

        public static implicit operator ByteVector(byte[] value)
        {
            return new ByteVector(value);
        }

        public static implicit operator ByteVector(byte value)
        {
            return new ByteVector(new byte[] { value });
        }

        public static implicit operator ByteVector(string value)
        {
            return FromString(value, StringType.UTF8);
        }

        public static bool operator !=(ByteVector first, ByteVector second)
        {
            return !(first == second);
        }

        public static bool operator <(ByteVector first, ByteVector second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            return (first.CompareTo(second) < 0);
        }

        public static bool operator <=(ByteVector first, ByteVector second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            return (first.CompareTo(second) <= 0);
        }

        public bool Remove(byte item)
        {
            if (this.IsReadOnly)
            {
                throw new NotSupportedException("Cannot edit readonly objects.");
            }
            return this.data.Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (this.IsReadOnly)
            {
                throw new NotSupportedException("Cannot edit readonly objects.");
            }
            this.data.RemoveAt(index);
        }

        public void RemoveRange(int index, int count)
        {
            if (this.IsReadOnly)
            {
                throw new NotSupportedException("Cannot edit readonly objects.");
            }
            this.data.RemoveRange(index, count);
        }

        public ByteVector Resize(int size)
        {
            return this.Resize(size, 0);
        }

        public ByteVector Resize(int size, byte padding)
        {
            if (this.IsReadOnly)
            {
                throw new NotSupportedException("Cannot edit readonly objects.");
            }
            if (this.data.Count > size)
            {
                this.data.RemoveRange(size, this.data.Count - size);
            }
            while (this.data.Count < size)
            {
                this.data.Add(padding);
            }
            return this;
        }

        public int RFind(ByteVector pattern)
        {
            return this.RFind(pattern, 0, 1);
        }

        public int RFind(ByteVector pattern, int offset)
        {
            return this.RFind(pattern, offset, 1);
        }

        public int RFind(ByteVector pattern, int offset, int byteAlign)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((pattern.Count != 0) && (pattern.Count <= (this.Count - offset)))
            {
                if (pattern.Count == 1)
                {
                    byte num = pattern[0];
                    for (int m = (this.Count - offset) - 1; m >= 0; m -= byteAlign)
                    {
                        if (this.data[m] == num)
                        {
                            return m;
                        }
                    }
                    return -1;
                }
                int[] numArray = new int[0x100];
                for (int i = 0; i < 0x100; i++)
                {
                    numArray[i] = pattern.Count;
                }
                for (int j = pattern.Count - 1; j > 0; j--)
                {
                    numArray[pattern[j]] = j;
                }
                for (int k = (this.Count - offset) - pattern.Count; k >= 0; k -= numArray[this.data[k]])
                {
                    if ((((offset - k) % byteAlign) == 0) && this.ContainsAt(pattern, k))
                    {
                        return k;
                    }
                }
            }
            return -1;
        }

        public bool StartsWith(ByteVector pattern)
        {
            return this.ContainsAt(pattern, 0);
        }

        private static Encoding StringTypeToEncoding(StringType type, ByteVector bom)
        {
            switch (type)
            {
                case StringType.UTF16:
                    if (bom != null)
                    {
                        if ((bom[0] == 0xff) && (bom[1] == 0xfe))
                        {
                            return (last_utf16_encoding = Encoding.Unicode);
                        }
                        if ((bom[1] == 0xff) && (bom[0] == 0xfe))
                        {
                            return (last_utf16_encoding = Encoding.BigEndianUnicode);
                        }
                        return last_utf16_encoding;
                    }
                    return last_utf16_encoding;

                case StringType.UTF16BE:
                    return Encoding.BigEndianUnicode;

                case StringType.UTF8:
                    return Encoding.UTF8;

                case StringType.UTF16LE:
                    return Encoding.Unicode;
            }
            if (use_broken_latin1)
            {
                return Encoding.Default;
            }
            try
            {
                return Encoding.GetEncoding("latin1");
            }
            catch (ArgumentException)
            {
                return Encoding.UTF8;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        public static ByteVector TextDelimiter(StringType type)
        {
            return ((((type != StringType.UTF16) && (type != StringType.UTF16BE)) && (type != StringType.UTF16LE)) ? td1 : td2);
        }

        public override string ToString()
        {
            return this.ToString(StringType.UTF8);
        }

        public string ToString(StringType type)
        {
            return this.ToString(type, 0, this.Count);
        }

        [Obsolete("Use ToString(StringType,int,int)")]
        public string ToString(StringType type, int offset)
        {
            return this.ToString(type, offset, this.Count - offset);
        }

        public string ToString(StringType type, int offset, int count)
        {
            if ((offset < 0) || (offset > this.Count))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((count < 0) || ((count + offset) > this.Count))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            ByteVector bom = ((type != StringType.UTF16) || ((this.data.Count - offset) <= 1)) ? null : this.Mid(offset, 2);
            string str = StringTypeToEncoding(type, bom).GetString(this.Data, offset, count);
            if ((str.Length == 0) || ((str[0] != 0xfffe) && (str[0] != 0xfeff)))
            {
                return str;
            }
            return str.Substring(1);
        }

        public string[] ToStrings(StringType type, int offset)
        {
            return this.ToStrings(type, offset, 0x7fffffff);
        }

        public string[] ToStrings(StringType type, int offset, int count)
        {
            int num = 0;
            int num2 = offset;
            List<string> list = new List<string>();
            ByteVector pattern = TextDelimiter(type);
            int byteAlign = pattern.Count;
            while ((num < count) && (num2 < this.Count))
            {
                int num4 = num2;
                if ((num + 1) == count)
                {
                    num2 = offset + count;
                }
                else
                {
                    num2 = this.Find(pattern, num4, byteAlign);
                    if (num2 < 0)
                    {
                        num2 = this.Count;
                    }
                }
                int num5 = num2 - num4;
                if (num5 == 0)
                {
                    list.Add(string.Empty);
                }
                else
                {
                    string item = this.ToString(type, num4, num5);
                    if ((item.Length != 0) && ((item[0] == 0xfffe) || (item[0] == 0xfeff)))
                    {
                        item = item.Substring(1);
                    }
                    list.Add(item);
                }
                num2 += byteAlign;
            }
            return list.ToArray();
        }

        public uint ToUInt()
        {
            return this.ToUInt(true);
        }

        public uint ToUInt(bool mostSignificantByteFirst)
        {
            uint num = 0;
            int num2 = (this.Count <= 4) ? (this.Count - 1) : 3;
            for (int i = 0; i <= num2; i++)
            {
                int num4 = !mostSignificantByteFirst ? i : (num2 - i);
                num |= (uint) (this[i] << (num4 * 8));
            }
            return num;
        }

        public ulong ToULong()
        {
            return this.ToULong(true);
        }

        public ulong ToULong(bool mostSignificantByteFirst)
        {
            ulong num = 0L;
            int num2 = (this.Count <= 8) ? (this.Count - 1) : 7;
            for (int i = 0; i <= num2; i++)
            {
                int num4 = !mostSignificantByteFirst ? i : (num2 - i);
                num |= this[i] << ((num4 * 8) & 0x3f);
            }
            return num;
        }

        public ushort ToUShort()
        {
            return this.ToUShort(true);
        }

        public ushort ToUShort(bool mostSignificantByteFirst)
        {
            ushort num = 0;
            int num2 = (this.Count <= 2) ? (this.Count - 1) : 1;
            for (int i = 0; i <= num2; i++)
            {
                int num4 = !mostSignificantByteFirst ? i : (num2 - i);
                num = (ushort) (num | ((ushort) (this[i] << (num4 * 8))));
            }
            return num;
        }

        public uint Checksum
        {
            get
            {
                uint num = 0;
                foreach (byte num2 in this.data)
                {
                    num = (num << 8) ^ crc_table[(int) ((IntPtr) (((num >> 0x18) & 0xff) ^ num2))];
                }
                return num;
            }
        }

        public int Count
        {
            get
            {
                return this.data.Count;
            }
        }

        public byte[] Data
        {
            get
            {
                return this.data.ToArray();
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (this.data.Count == 0);
            }
        }

        public virtual bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public byte this[int index]
        {
            get
            {
                return this.data[index];
            }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new NotSupportedException("Cannot edit readonly objects.");
                }
                this.data[index] = value;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        public static bool UseBrokenLatin1Behavior
        {
            get
            {
                return use_broken_latin1;
            }
            set
            {
                use_broken_latin1 = value;
            }
        }
    }
}

