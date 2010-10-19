namespace Nomad.FileSystem.Archive.Common
{
    using System;
    using System.Text;

    public static class CheckFileSystem
    {
        public static int CheckFAT(byte[] data, int dataLength)
        {
            if (((dataLength > 0x200) && (data[510] == 0x55)) && (data[0x1ff] == 170))
            {
                string a = Encoding.ASCII.GetString(data, 0x36, 8);
                if (string.Equals(a, "FAT12   ", StringComparison.Ordinal) || string.Equals(a, "FAT16   ", StringComparison.Ordinal))
                {
                    return 0;
                }
                if (string.Equals(Encoding.ASCII.GetString(data, 0x52, 8), "FAT32   ", StringComparison.Ordinal))
                {
                    return 0;
                }
            }
            return -1;
        }

        public static int CheckMBR(byte[] data, int dataLength)
        {
            if (((((dataLength > 0x200) && (data[510] == 0x55)) && ((data[0x1ff] == 170) && ((data[0x1be] & 0x7f) == 0))) && (((data[0x1ce] & 0x7f) == 0) && ((data[0x1de] & 0x7f) == 0))) && ((data[0x1ee] & 0x7f) == 0))
            {
                return 0;
            }
            return -1;
        }
    }
}

