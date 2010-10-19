namespace Nomad.FileSystem.Archive.Common
{
    using System;

    public static class CheckRar
    {
        public static int Check(byte[] data, int dataLength)
        {
            for (int i = 0; i < (dataLength - 7); i++)
            {
                if ((((data[i] == 0x52) && (data[i + 1] == 0x45)) && ((data[i + 2] == 0x7e) && (data[i + 3] == 0x5e))) || (((((data[i + 1] == 0x61) && (data[i + 2] == 0x72)) && ((data[i + 3] == 0x21) && (data[i + 4] == 0x1a))) && (data[i + 5] == 7)) && (data[i + 6] == 0)))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}

