namespace Nomad.FileSystem.Archive.SevenZip
{
    using Nomad.Commons;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    public class SevenZipPropertiesBuilder
    {
        private KnownSevenZipFormat KnownFormat;
        private Dictionary<string, object> Properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public SevenZipPropertiesBuilder(KnownSevenZipFormat format)
        {
            this.KnownFormat = format;
        }

        public void Apply(ISetProperties setProperties)
        {
            int count = this.Properties.Count;
            string[] names = new string[count];
            PropVariant[] variantArray = new PropVariant[count];
            int index = 0;
            foreach (KeyValuePair<string, object> pair in this.Properties)
            {
                names[index] = pair.Key;
                variantArray[index] = new PropVariant(pair.Value);
                index++;
            }
            GCHandle handle = GCHandle.Alloc(variantArray, GCHandleType.Pinned);
            try
            {
                setProperties.SetProperties(names, handle.AddrOfPinnedObject(), count);
            }
            finally
            {
                handle.Free();
            }
            foreach (PropVariant variant in variantArray)
            {
                variant.Clear();
            }
        }

        public void SetEncryptHeaders(bool encrypt)
        {
            if (this.KnownFormat != KnownSevenZipFormat.SevenZip)
            {
                throw new NotSupportedException();
            }
            this.Properties["he"] = encrypt;
        }

        public void SetEncryptionMethod(EncryptionMethod method)
        {
            if (!Enum.IsDefined(typeof(EncryptionMethod), method))
            {
                throw new InvalidEnumArgumentException();
            }
            KnownSevenZipFormat knownFormat = this.KnownFormat;
            if (knownFormat != KnownSevenZipFormat.SevenZip)
            {
                if (knownFormat != KnownSevenZipFormat.Zip)
                {
                    throw new NotSupportedException();
                }
            }
            else
            {
                if (method != EncryptionMethod.AES256)
                {
                    throw new NotSupportedException();
                }
                return;
            }
            this.Properties["em"] = method.ToString();
        }

        public void SetLevel(CompressionLevel level)
        {
            if (!Enum.IsDefined(typeof(CompressionLevel), level))
            {
                throw new InvalidEnumArgumentException();
            }
            switch (this.KnownFormat)
            {
                case KnownSevenZipFormat.Xz:
                case KnownSevenZipFormat.BZip2:
                    if (level == CompressionLevel.Store)
                    {
                        throw new NotSupportedException();
                    }
                    break;

                case KnownSevenZipFormat.Tar:
                    if (level != CompressionLevel.Store)
                    {
                        throw new NotSupportedException();
                    }
                    break;

                case KnownSevenZipFormat.GZip:
                    switch (level)
                    {
                        case CompressionLevel.Store:
                        case CompressionLevel.Fast:
                            throw new NotSupportedException();
                    }
                    break;
            }
            this.Properties["x"] = Convert.ToUInt32(level);
        }

        public void SetMethod(CompressionMethod method)
        {
            if (!Enum.IsDefined(typeof(CompressionMethod), method))
            {
                throw new InvalidEnumArgumentException();
            }
            switch (this.KnownFormat)
            {
                case KnownSevenZipFormat.Xz:
                    if (method != CompressionMethod.LZMA2)
                    {
                        throw new NotSupportedException();
                    }
                    return;

                case KnownSevenZipFormat.Zip:
                    switch (method)
                    {
                        case CompressionMethod.Copy:
                        case CompressionMethod.LZMA:
                        case CompressionMethod.PPMd:
                        case CompressionMethod.BZip2:
                        case CompressionMethod.Deflate:
                        case CompressionMethod.Deflate64:
                            this.Properties["m"] = method.ToString();
                            return;
                    }
                    break;

                case KnownSevenZipFormat.Tar:
                    if (method != CompressionMethod.Copy)
                    {
                        throw new NotSupportedException();
                    }
                    return;

                case KnownSevenZipFormat.SevenZip:
                    switch (method)
                    {
                        case CompressionMethod.Copy:
                        case CompressionMethod.LZMA:
                        case CompressionMethod.LZMA2:
                        case CompressionMethod.PPMd:
                        case CompressionMethod.BZip2:
                            this.Properties["0"] = method.ToString();
                            return;
                    }
                    throw new NotSupportedException();

                case KnownSevenZipFormat.BZip2:
                    if (method != CompressionMethod.BZip2)
                    {
                        throw new NotSupportedException();
                    }
                    return;

                case KnownSevenZipFormat.GZip:
                    if (method != CompressionMethod.Deflate)
                    {
                        throw new NotSupportedException();
                    }
                    return;

                default:
                    throw new NotSupportedException();
            }
            throw new NotSupportedException();
        }

        public void SetMultiThread(bool multiThread)
        {
            switch (this.KnownFormat)
            {
                case KnownSevenZipFormat.SevenZip:
                case KnownSevenZipFormat.BZip2:
                case KnownSevenZipFormat.Xz:
                case KnownSevenZipFormat.Zip:
                    this.Properties["mt"] = multiThread;
                    return;
            }
            throw new NotSupportedException();
        }

        public void SetProperties(string properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException();
            }
            foreach (string str in StringHelper.SplitString(properties, new char[] { ' ' }))
            {
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }
                int index = str.IndexOf('=');
                if (index >= 0)
                {
                    this.Properties[str.Substring(0, index)] = str.Substring(index + 1);
                }
                else if (str.EndsWith("+", StringComparison.OrdinalIgnoreCase))
                {
                    this.Properties[str.Substring(0, str.Length - 1)] = true;
                }
                else if (str.EndsWith("-", StringComparison.OrdinalIgnoreCase))
                {
                    this.Properties[str.Substring(0, str.Length - 1)] = false;
                }
                else
                {
                    this.Properties[str] = null;
                }
            }
        }

        public void SetSolid(bool solid)
        {
            if (this.KnownFormat != KnownSevenZipFormat.SevenZip)
            {
                throw new NotSupportedException();
            }
            this.Properties["s"] = solid;
        }

        public void SetSolidSize(int size, SolidSizeUnit unit)
        {
            if (this.KnownFormat != KnownSevenZipFormat.SevenZip)
            {
                throw new NotSupportedException();
            }
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (!Enum.IsDefined(typeof(SolidSizeUnit), unit))
            {
                throw new InvalidEnumArgumentException();
            }
            if (size == 0)
            {
                this.Properties["s"] = false;
            }
            else
            {
                this.Properties["s"] = size.ToString() + unit.ToString().Substring(0, 1).ToLower();
            }
        }

        public void SetThreadCount(int threadCount)
        {
            if (threadCount < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            switch (this.KnownFormat)
            {
                case KnownSevenZipFormat.SevenZip:
                case KnownSevenZipFormat.BZip2:
                case KnownSevenZipFormat.Xz:
                case KnownSevenZipFormat.Zip:
                    this.Properties["mt"] = (uint) threadCount;
                    return;
            }
            throw new NotSupportedException();
        }

        public bool MultiThread
        {
            get
            {
                object obj2;
                if (this.Properties.TryGetValue("mt", out obj2))
                {
                    uint num;
                    TypeCode typeCode = Type.GetTypeCode(obj2.GetType());
                    switch (typeCode)
                    {
                        case TypeCode.Boolean:
                            return (bool) obj2;

                        case TypeCode.UInt32:
                            return (((uint) obj2) > 1);
                    }
                    if ((typeCode == TypeCode.String) && uint.TryParse((string) obj2, out num))
                    {
                        return (num > 1);
                    }
                }
                return false;
            }
            set
            {
                this.SetMultiThread(value);
            }
        }

        public enum SolidSizeUnit
        {
            B,
            Kb,
            Mb,
            Gb
        }
    }
}

