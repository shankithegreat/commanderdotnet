namespace Nomad.FileSystem.Property.Providers
{
    using Nomad.Commons;
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Remoting;
    using System.Security.Cryptography;

    [Version(1, 0, 2, 10)]
    public class HashPropertyProvider : ILocalFilePropertyProvider, ISimplePropertyProvider, IPropertyProvider
    {
        private static PropertyAvailability HashAvailability;
        private static int PropertyHashSHA1;

        public IGetVirtualProperty AddProperties(FileSystemInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException();
            }
            FileInfo fileInfo = info as FileInfo;
            return ((fileInfo != null) ? new HashPropertyBag(fileInfo) : null);
        }

        public VirtualPropertySet GetRegisteredProperties()
        {
            return new VirtualPropertySet(new int[] { 0x18, 0x19, PropertyHashSHA1 });
        }

        public bool Register(Hashtable options)
        {
            if (!EnumHelper.TryParse<PropertyAvailability>(Convert.ToString(options["availability"]), true, out HashAvailability))
            {
                HashAvailability = PropertyAvailability.Slow;
            }
            VirtualPropertyOption slow = 0;
            switch (HashAvailability)
            {
                case PropertyAvailability.Slow:
                    slow = VirtualPropertyOption.Slow;
                    break;

                case PropertyAvailability.OnDemand:
                    slow = VirtualPropertyOption.OnDemand;
                    break;
            }
            int groupId = VirtualProperty.RegisterGroup("Hash");
            PropertyHashSHA1 = DefaultProperty.RegisterProperty("SHA1", groupId, typeof(byte[]), 40, HashConverter.Default, slow);
            return true;
        }

        private class HashPropertyBag : CustomPropertyProvider, IGetVirtualProperty, IDisposable
        {
            private FileInfo _FileInfo;

            public HashPropertyBag(FileInfo fileInfo)
            {
                this._FileInfo = fileInfo;
            }

            private byte[] CalculateHash(HashAlgorithm hashGenerator)
            {
                try
                {
                    Stream stream;
                    if (RemotingServices.IsTransparentProxy(this._FileInfo))
                    {
                        stream = this._FileInfo.OpenRead();
                    }
                    else
                    {
                        stream = new FileStream(this._FileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 0x400, FileOptions.SequentialScan);
                    }
                    using (stream)
                    {
                        using (hashGenerator)
                        {
                            using (Stream stream2 = new CryptoStream(stream, hashGenerator, CryptoStreamMode.Read))
                            {
                                byte[] buffer = new byte[0xf000];
                                while (stream2.Read(buffer, 0, buffer.Length) > 0)
                                {
                                    if (this._FileInfo == null)
                                    {
                                        return null;
                                    }
                                }
                            }
                            return hashGenerator.Hash;
                        }
                    }
                }
                catch (Exception exception)
                {
                    PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                    this._FileInfo = null;
                }
                finally
                {
                    if (this._FileInfo == null)
                    {
                        base.ResetAvailableSet();
                    }
                }
                return null;
            }

            protected override VirtualPropertySet CreateAvailableSet()
            {
                bool flag = this._FileInfo != null;
                VirtualPropertySet set = new VirtualPropertySet();
                set[0x18] = flag;
                set[0x19] = flag;
                set[HashPropertyProvider.PropertyHashSHA1] = flag;
                return set;
            }

            public void Dispose()
            {
                this._FileInfo = null;
            }

            public override PropertyAvailability GetPropertyAvailability(int propertyId)
            {
                if ((this._FileInfo != null) && (((propertyId == 0x18) || (propertyId == 0x19)) || (propertyId == HashPropertyProvider.PropertyHashSHA1)))
                {
                    return HashPropertyProvider.HashAvailability;
                }
                return PropertyAvailability.None;
            }

            public object this[int property]
            {
                get
                {
                    if (this._FileInfo != null)
                    {
                        if (property == 0x18)
                        {
                            byte[] buffer = this.CalculateHash(new Crc32());
                            if (buffer != null)
                            {
                                return BitConverter.ToInt32(buffer, 0);
                            }
                        }
                        if (property == 0x19)
                        {
                            return this.CalculateHash(MD5.Create());
                        }
                        if (property == HashPropertyProvider.PropertyHashSHA1)
                        {
                            return this.CalculateHash(SHA1.Create());
                        }
                    }
                    return null;
                }
            }
        }
    }
}

