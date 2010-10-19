namespace Nomad.Commons
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Cryptography;

    public class SimpleEncrypt
    {
        public static string DecryptString(SecureString source)
        {
            string str;
            IntPtr ptr = Marshal.SecureStringToBSTR(source);
            try
            {
                str = Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                Marshal.FreeBSTR(ptr);
            }
            return str;
        }

        public static string DecryptString(byte[] source, SymmetricAlgorithm algorithm)
        {
            string str;
            using (MemoryStream stream = new MemoryStream(source))
            {
                using (new BinaryReader(stream))
                {
                    byte[] buffer = new byte[algorithm.KeySize / 8];
                    stream.Read(buffer, 0, buffer.Length);
                    algorithm.IV = buffer;
                    stream.Read(buffer, 0, buffer.Length);
                    algorithm.Key = buffer;
                    using (CryptoStream stream2 = new CryptoStream(stream, algorithm.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (TextReader reader2 = new StreamReader(stream2))
                        {
                            str = reader2.ReadToEnd();
                        }
                    }
                }
            }
            return str;
        }

        public static byte[] EncryptString(string source, SymmetricAlgorithm algorithm)
        {
            byte[] buffer;
            algorithm.GenerateIV();
            algorithm.GenerateKey();
            using (MemoryStream stream = new MemoryStream(0x80))
            {
                using (new BinaryWriter(stream))
                {
                    stream.Write(algorithm.IV, 0, algorithm.IV.Length);
                    stream.Write(algorithm.Key, 0, algorithm.Key.Length);
                    using (CryptoStream stream2 = new CryptoStream(stream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (TextWriter writer2 = new StreamWriter(stream2))
                        {
                            writer2.Write(source);
                        }
                    }
                    buffer = stream.ToArray();
                }
            }
            return buffer;
        }
    }
}

