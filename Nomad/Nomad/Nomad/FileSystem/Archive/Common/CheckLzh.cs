namespace Nomad.FileSystem.Archive.Common
{
    using System;
    using System.Text;

    public static class CheckLzh
    {
        public static int Check(byte[] data, int dataLength)
        {
            for (int i = 2; i < (dataLength - 0x13); i++)
            {
                if ((data[i + 0x12] < 0) || (data[i + 0x12] > 2))
                {
                    continue;
                }
                string str = Encoding.ASCII.GetString(data, i, 5);
                char ch = str[3];
                switch (ch)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;

                    default:
                        if ((ch != 'd') && (ch != 's'))
                        {
                            continue;
                        }
                        break;
                }
                if ((((str[0] == '-') && (str[1] == 'l')) && (str[2] == 'h')) && (str[4] == '-'))
                {
                    return (i - 2);
                }
            }
            return -1;
        }
    }
}

