namespace Microsoft.IE
{
    using Microsoft.COM;
    using Microsoft.IE.MLang;
    using Nomad.Interop.Commons;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public class MLangHelper
    {
        public static Encoding DetectEncoding(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            return DetectEncoding(data, data.Length, DetectOption.Default);
        }

        public static Encoding DetectEncoding(Stream stream)
        {
            return DetectEncoding(stream, DetectOption.Default);
        }

        public static Encoding DetectEncoding(byte[] data, int dataLength)
        {
            return DetectEncoding(data, dataLength, DetectOption.Default);
        }

        public static Encoding DetectEncoding(Stream stream, DetectOption flags)
        {
            Encoding encoding;
            IMultiLanguage2 o = new CMultiLanguageClass() as IMultiLanguage2;
            if (o == null)
            {
                byte[] buffer = new byte[4];
                int dataLength = stream.Read(buffer, 0, buffer.Length);
                return SimpleDetectEncoding(buffer, dataLength);
            }
            try
            {
                DetectEncodingInfo[] lpEncoding = new DetectEncodingInfo[1];
                int length = lpEncoding.Length;
                ComStream pstmIn = new ComStream(stream);
                int errorCode = o.DetectCodepageInIStream(((MLDETECTCP) flags) & MLDETECTCP.MLDETECTCP_MASK, 0, pstmIn, lpEncoding, ref length);
                switch (errorCode)
                {
                    case 0:
                    case 1:
                        if (length <= 0)
                        {
                            break;
                        }
                        return Encoding.GetEncoding((int) lpEncoding[0].nCodePage);

                    default:
                        throw Marshal.GetExceptionForHR(errorCode);
                }
                encoding = null;
            }
            finally
            {
                Marshal.FinalReleaseComObject(o);
            }
            return encoding;
        }

        public static Encoding DetectEncoding(byte[] data, DetectOption flags)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            return DetectEncoding(data, data.Length, flags);
        }

        public static Encoding DetectEncoding(byte[] data, int dataLength, DetectOption flags)
        {
            Encoding encoding2;
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Length < dataLength)
            {
                throw new ArgumentException("data length is less than dataLength");
            }
            if (dataLength == 0)
            {
                return null;
            }
            if ((flags & DetectOption.TrySimpleDetectFirst) > DetectOption.Default)
            {
                Encoding encoding = SimpleDetectEncoding(data, dataLength);
                if (encoding != null)
                {
                    return encoding;
                }
            }
            IMultiLanguage2 o = new CMultiLanguageClass() as IMultiLanguage2;
            if (o == null)
            {
                return (((flags & DetectOption.TrySimpleDetectFirst) > DetectOption.Default) ? null : SimpleDetectEncoding(data, dataLength));
            }
            try
            {
                DetectEncodingInfo[] lpEncoding = new DetectEncodingInfo[1];
                int length = lpEncoding.Length;
                switch (o.DetectInputCodepage(((MLDETECTCP) flags) & MLDETECTCP.MLDETECTCP_MASK, 0, data, ref dataLength, lpEncoding, ref length))
                {
                    case 0:
                    case 1:
                        if (length <= 0)
                        {
                            break;
                        }
                        return Encoding.GetEncoding((int) lpEncoding[0].nCodePage);
                }
                encoding2 = null;
            }
            finally
            {
                Marshal.FinalReleaseComObject(o);
            }
            return encoding2;
        }

        private static Encoding SimpleDetectEncoding(byte[] data, int dataLength)
        {
            byte[] preamble = Encoding.UTF8.GetPreamble();
            if ((dataLength >= preamble.Length) && ByteArrayHelper.Equals(preamble, data, preamble.Length))
            {
                return Encoding.UTF8;
            }
            preamble = Encoding.Unicode.GetPreamble();
            if ((dataLength >= preamble.Length) && ByteArrayHelper.Equals(preamble, data, preamble.Length))
            {
                return Encoding.Unicode;
            }
            preamble = Encoding.BigEndianUnicode.GetPreamble();
            if ((dataLength >= preamble.Length) && ByteArrayHelper.Equals(preamble, data, preamble.Length))
            {
                return Encoding.BigEndianUnicode;
            }
            return null;
        }
    }
}

