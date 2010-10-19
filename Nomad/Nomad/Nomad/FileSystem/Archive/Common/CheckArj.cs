namespace Nomad.FileSystem.Archive.Common
{
    using Nomad.Commons;
    using System;
    using System.Text;

    public static class CheckArj
    {
        public static int Check(byte[] data, int dataLength)
        {
            ArjHeader header = new ArjHeader();
            for (int i = 0; i < (dataLength - 0xa28); i++)
            {
                header.Mark = ByteArrayHelper.ReadUInt16(data, i);
                header.HeadSize = ByteArrayHelper.ReadUInt16(data, i + 2);
                header.FirstHeadSize = data[i + 4];
                header.ArjVer = data[i + 5];
                header.ArjExtrVer = data[i + 6];
                if (((((header.Mark == 0xea60) && (header.HeadSize <= 0xa28)) && ((header.FirstHeadSize < 0x40) && (header.ArjVer < 0x40))) && (header.ArjExtrVer < 0x20)) && ((i == 0) || ((i > 0x20) && (Encoding.ASCII.GetString(data, 0x1c, 4) == "RJSX"))))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}

