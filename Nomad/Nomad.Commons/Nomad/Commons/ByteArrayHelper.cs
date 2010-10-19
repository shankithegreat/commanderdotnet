namespace Nomad.Commons
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class ByteArrayHelper
    {
        public static T ByteArrayToStructure<T>(byte[] data)
        {
            return (T) ByteArrayToStructure(data, 0, typeof(T));
        }

        public static T ByteArrayToStructure<T>(byte[] data, int index)
        {
            return (T) ByteArrayToStructure(data, index, typeof(T));
        }

        public static void ByteArrayToStructure(byte[] data, object structure)
        {
            ByteArrayToStructure(data, 0, structure);
        }

        public static object ByteArrayToStructure(byte[] data, Type structureType)
        {
            return ByteArrayToStructure(data, 0, structureType);
        }

        public static void ByteArrayToStructure(byte[] data, int index, object structure)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (structure == null)
            {
                throw new ArgumentNullException("structure");
            }
            if (data.Length < Marshal.SizeOf(structure))
            {
                throw new ArgumentException("Data is too small to hold entire structure");
            }
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = handle.AddrOfPinnedObject();
                if (index != 0)
                {
                    if (IntPtr.Size == 4)
                    {
                        ptr = (IntPtr) (ptr.ToInt32() + index);
                    }
                    else
                    {
                        ptr = (IntPtr) (ptr.ToInt64() + index);
                    }
                }
                Marshal.PtrToStructure(ptr, structure);
            }
            finally
            {
                handle.Free();
            }
        }

        public static object ByteArrayToStructure(byte[] data, int index, Type structureType)
        {
            object obj2;
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (structureType == null)
            {
                throw new ArgumentNullException("structureType");
            }
            if (data.Length < Marshal.SizeOf(structureType))
            {
                throw new ArgumentException("Data is too small to hold entire structure");
            }
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = handle.AddrOfPinnedObject();
                if (index != 0)
                {
                    if (IntPtr.Size == 4)
                    {
                        ptr = (IntPtr) (ptr.ToInt32() + index);
                    }
                    else
                    {
                        ptr = (IntPtr) (ptr.ToInt64() + index);
                    }
                }
                obj2 = Marshal.PtrToStructure(ptr, structureType);
            }
            finally
            {
                handle.Free();
            }
            return obj2;
        }

        public static bool Equals(byte[] value1, byte[] value2)
        {
            if ((value1 != null) || (value2 != null))
            {
                if (((value1 == null) || (value2 == null)) || (value1.Length != value2.Length))
                {
                    return false;
                }
                for (int i = 0; i < value1.Length; i++)
                {
                    if (value1[i] != value2[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool Equals(byte[] value1, byte[] value2, int length)
        {
            if ((value1 != null) || (value2 != null))
            {
                if ((value1 == null) || (value2 == null))
                {
                    return false;
                }
                if ((value1.Length < length) || (value2.Length < length))
                {
                    throw new ArgumentException();
                }
                for (int i = 0; i < length; i++)
                {
                    if (value1[i] != value2[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static int IndexOf(byte[] sequense, byte[] array, int arrayLength)
        {
            if (sequense == null)
            {
                throw new ArgumentNullException("sequense");
            }
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if ((sequense.Length != 0) && (sequense.Length <= arrayLength))
            {
                int num3;
                if (sequense.Length == 1)
                {
                    for (int i = 0; i < arrayLength; i++)
                    {
                        if (array[i] == sequense[0])
                        {
                            return i;
                        }
                    }
                    return -1;
                }
                int[] numArray = preKmp(sequense);
                int index = num3 = 0;
                while (num3 < arrayLength)
                {
                    while ((index > -1) && (sequense[index] != array[num3]))
                    {
                        index = numArray[index];
                    }
                    index++;
                    num3++;
                    if (index >= sequense.Length)
                    {
                        return (num3 - index);
                    }
                }
            }
            return -1;
        }

        public static byte[] Parse(string str)
        {
            byte[] buffer;
            if (str == null)
            {
                throw new ArgumentNullException();
            }
            if (!TryParse(str, out buffer))
            {
                throw new FormatException();
            }
            return buffer;
        }

        private static int[] preKmp(byte[] x)
        {
            int[] numArray = new int[x.Length + 1];
            int index = 0;
            int num2 = numArray[0] = -1;
            while (index < x.Length)
            {
                while ((num2 > -1) && (x[index] != x[num2]))
                {
                    num2 = numArray[num2];
                }
                index++;
                num2++;
                numArray[index] = num2;
            }
            return numArray;
        }

        public static short ReadInt16(byte[] data, int index)
        {
            return (short) (data[index] | (data[index + 1] << 8));
        }

        public static int ReadInt32(byte[] data, int index)
        {
            return (((data[index] | (data[index + 1] << 8)) | (data[index + 2] << 0x10)) | (data[index + 3] << 0x18));
        }

        public static T ReadStructureFromStream<T>(Stream source) where T: struct
        {
            return (T) ReadStructureFromStream(source, typeof(T));
        }

        public static void ReadStructureFromStream(Stream source, object structure)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (structure == null)
            {
                throw new ArgumentNullException("structure");
            }
            if (source.CanSeek && ((source.Length - source.Position) < Marshal.SizeOf(structure)))
            {
                throw new ArgumentException("Srouce is too small to hold entire structure");
            }
            byte[] data = new byte[Marshal.SizeOf(structure)];
            ByteArrayToStructure(data, structure);
        }

        public static object ReadStructureFromStream(Stream source, Type structureType)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (structureType == null)
            {
                throw new ArgumentNullException("structureType");
            }
            byte[] buffer = new byte[Marshal.SizeOf(structureType)];
            int num = source.Read(buffer, 0, buffer.Length);
            if (num == buffer.Length)
            {
                return ByteArrayToStructure(buffer, structureType);
            }
            if (num != 0)
            {
                throw new ArgumentException("source is too small to hold entire structure");
            }
            return null;
        }

        public static ushort ReadUInt16(byte[] data, int index)
        {
            return (ushort) (data[index] | (data[index + 1] << 8));
        }

        public static byte[] StructureToByteArray(object structure)
        {
            if (structure == null)
            {
                throw new ArgumentNullException();
            }
            byte[] buffer = new byte[Marshal.SizeOf(structure)];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                Marshal.StructureToPtr(structure, handle.AddrOfPinnedObject(), false);
            }
            finally
            {
                handle.Free();
            }
            return buffer;
        }

        public static string ToString(byte[] data)
        {
            if ((data == null) || (data.Length == 0))
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder(data.Length * 2);
            foreach (byte num in data)
            {
                builder.Append(num.ToString("x2"));
            }
            return builder.ToString();
        }

        public static bool TryParse(string str, out byte[] data)
        {
            if (string.IsNullOrEmpty(str) || ((str.Length % 2) > 0))
            {
                data = null;
                return false;
            }
            data = new byte[str.Length / 2];
            for (int i = 0; i < str.Length; i += 2)
            {
                if (!byte.TryParse(str.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out data[i / 2]))
                {
                    return false;
                }
            }
            return true;
        }

        public static void WriteInt16(byte[] data, int index, short value)
        {
            data[index] = (byte) (value & 0xff);
            data[index + 1] = (byte) (value >> 8);
        }

        public static void WriteInt32(byte[] data, int index, int value)
        {
            data[index] = (byte) (value & 0xff);
            data[index + 1] = (byte) ((value >> 8) & 0xff);
            data[index + 2] = (byte) ((value >> 0x10) & 0xff);
            data[index + 3] = (byte) (value >> 0x18);
        }
    }
}

