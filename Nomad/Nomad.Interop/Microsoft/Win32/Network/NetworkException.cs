namespace Microsoft.Win32.Network
{
    using System;
    using System.IO;

    public class NetworkException : IOException
    {
        public readonly int ProviderErrorCode;
        public readonly string ProviderName;

        public NetworkException(string message, int errorCode, string providerName) : base(message)
        {
            this.ProviderErrorCode = errorCode;
            this.ProviderName = providerName;
        }
    }
}

