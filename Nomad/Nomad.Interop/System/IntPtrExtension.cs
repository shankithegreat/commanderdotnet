namespace System
{
    using System.Runtime.CompilerServices;

    public static class IntPtrExtension
    {
        public static IntPtr Offset(this IntPtr source, int value)
        {
            if (IntPtr.Size == 4)
            {
                return new IntPtr(((int) source) + value);
            }
            return new IntPtr(((long) source) + value);
        }

        public static T SafeConvert<T>(this IntPtr value) where T: struct
        {
            Type enumType = typeof(T);
            object obj2 = SafeConvert(value, enumType.IsEnum ? Enum.GetUnderlyingType(enumType) : enumType);
            return ((obj2 == null) ? default(T) : ((T) obj2));
        }

        private static object SafeConvert(IntPtr value, Type resultType)
        {
            switch (Type.GetTypeCode(resultType))
            {
                case TypeCode.SByte:
                    return ((IntPtr.Size == 4) ? ((sbyte) ((int) value)) : ((sbyte) ((long) value)));

                case TypeCode.Byte:
                    return ((IntPtr.Size == 4) ? ((byte) ((int) value)) : ((byte) ((long) value)));

                case TypeCode.Int16:
                    return ((IntPtr.Size == 4) ? ((short) ((int) value)) : ((short) ((long) value)));

                case TypeCode.UInt16:
                    return ((IntPtr.Size == 4) ? ((ushort) ((int) value)) : ((ushort) ((long) value)));

                case TypeCode.Int32:
                    return ((IntPtr.Size == 4) ? ((int) value) : ((int) ((long) value)));

                case TypeCode.UInt32:
                    return ((IntPtr.Size == 4) ? ((uint) ((int) value)) : ((uint) ((long) value)));

                case TypeCode.Int64:
                    return (long) value;

                case TypeCode.UInt64:
                    return (ulong) ((long) value);
            }
            return null;
        }

        public static bool SafeEquals<T>(this IntPtr a, T b)
        {
            Type enumType = typeof(T);
            object obj2 = SafeConvert(a, enumType.IsEnum ? Enum.GetUnderlyingType(enumType) : enumType);
            return ((obj2 != null) && b.Equals((T) obj2));
        }
    }
}

