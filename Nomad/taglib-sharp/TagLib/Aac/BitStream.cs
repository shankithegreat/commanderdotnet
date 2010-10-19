namespace TagLib.Aac
{
    using System;
    using System.Collections;

    public class BitStream
    {
        private int bitindex;
        private BitArray bits;

        public BitStream(byte[] buffer)
        {
            if (buffer.Length != 7)
            {
                throw new ArgumentException("Buffer size must be 7 bytes");
            }
            this.bits = new BitArray(buffer.Length * 8);
            for (int i = 0; i < buffer.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    this.bits[(i * 8) + j] = (buffer[i] & (((int) 1) << (7 - j))) > 0;
                }
            }
            this.bitindex = 0;
        }

        public int ReadInt32(int numberOfBits)
        {
            if (numberOfBits <= 0)
            {
                throw new ArgumentException("Number of bits to read must be >= 1");
            }
            if (numberOfBits > 0x20)
            {
                throw new ArgumentException("Number of bits to read must be <= 32");
            }
            int num = 0;
            int num2 = (this.bitindex + numberOfBits) - 1;
            for (int i = 0; i < numberOfBits; i++)
            {
                num += !this.bits[num2] ? 0 : (((int) 1) << i);
                this.bitindex++;
                num2--;
            }
            return num;
        }
    }
}

