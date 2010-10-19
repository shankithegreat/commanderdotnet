namespace TagLib.Id3v2
{
    using System;
    using TagLib;

    public static class SynchData
    {
        public static ByteVector FromUInt(uint value)
        {
            if ((value >> 0x1c) != 0)
            {
                throw new ArgumentOutOfRangeException("value", "value must be less than 268435456.");
            }
            ByteVector vector = new ByteVector(4, 0);
            for (int i = 0; i < 4; i++)
            {
                vector[i] = (byte) ((value >> ((3 - i) * 7)) & 0x7f);
            }
            return vector;
        }

        public static void ResynchByteVector(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            for (int i = data.Count - 2; i >= 0; i--)
            {
                if ((data[i] == 0xff) && (data[i + 1] == 0))
                {
                    data.RemoveAt(i + 1);
                }
            }
        }

        public static uint ToUInt(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            uint num = 0;
            int num2 = (data.Count <= 4) ? (data.Count - 1) : 3;
            for (int i = 0; i <= num2; i++)
            {
                num |= (uint) ((data[i] & 0x7f) << ((num2 - i) * 7));
            }
            return num;
        }

        public static void UnsynchByteVector(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            for (int i = data.Count - 2; i >= 0; i--)
            {
                if ((data[i] == 0xff) && ((data[i + 1] == 0) || ((data[i + 1] & 0xe0) != 0)))
                {
                    data.Insert(i + 1, (byte) 0);
                }
            }
        }
    }
}

