namespace Microsoft.COM
{
    using System;
    using System.Runtime.InteropServices;

    public sealed class FileGroupDescriptorHelper
    {
        private static T[] GetDescriptors<T>(IntPtr descriptors) where T: struct
        {
            uint num = (uint) Marshal.ReadInt32(descriptors);
            long num2 = descriptors.ToInt64() + 4L;
            T[] localArray = new T[num];
            for (int i = 0; i < num; i++)
            {
                localArray[i] = (T) Marshal.PtrToStructure(new IntPtr(num2), typeof(T));
                num2 += Marshal.SizeOf(typeof(T));
            }
            return localArray;
        }

        public static FILEDESCRIPTORA[] GetDescriptorsA(IntPtr descriptors)
        {
            return GetDescriptors<FILEDESCRIPTORA>(descriptors);
        }

        public static FILEDESCRIPTORW[] GetDescriptorsW(IntPtr descriptors)
        {
            return GetDescriptors<FILEDESCRIPTORW>(descriptors);
        }
    }
}

