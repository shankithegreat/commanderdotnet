namespace Nomad.FileSystem.Archive.Wcx
{
    using System;

    public class WcxErrors
    {
        public const int E_BAD_ARCHIVE = 13;
        public const int E_BAD_DATA = 12;
        public const int E_EABORTED = 0x15;
        public const int E_ECLOSE = 0x11;
        public const int E_ECREATE = 0x10;
        public const int E_END_ARCHIVE = 10;
        public const int E_EOPEN = 15;
        public const int E_EREAD = 0x12;
        public const int E_EWRITE = 0x13;
        public const int E_NO_FILES = 0x16;
        public const int E_NO_MEMORY = 11;
        public const int E_NOT_SUPPORTED = 0x18;
        public const int E_SMALL_BUF = 20;
        public const int E_SUCCESS = 0;
        public const int E_TOO_MANY_FILES = 0x17;
        public const int E_UNKNOWN_FORMAT = 14;

        public static void ThrowExceptionForError(int errorCode)
        {
            switch (errorCode)
            {
                case 10:
                    throw new WcxException(errorCode, "No more files in archive");

                case 11:
                    throw new WcxException(errorCode, "Not enough memory");

                case 12:
                    throw new WcxException(errorCode, "Data is bad");

                case 13:
                    throw new WcxException(errorCode, "CRC error in archive data");

                case 14:
                    throw new WcxException(errorCode, "Archive format unknown");

                case 15:
                    throw new WcxException(errorCode, "Cannot open existing file");

                case 0x10:
                    throw new WcxException(errorCode, "Cannot create file");

                case 0x11:
                    throw new WcxException(errorCode, "Error closing file");

                case 0x12:
                    throw new WcxException(errorCode, "Error reading from file");

                case 0x13:
                    throw new WcxException(errorCode, "Error writing to file");

                case 20:
                    throw new WcxException(errorCode, "Buffer too small");

                case 0x15:
                    throw new WcxException(errorCode, "Function aborted by user");

                case 0x16:
                    throw new WcxException(errorCode, "No files found");

                case 0x17:
                    throw new WcxException(errorCode, "Too many files to pack");

                case 0x18:
                    throw new NotSupportedException("Function not supported");
            }
            throw new WcxException(-1, "Unknown error");
        }
    }
}

