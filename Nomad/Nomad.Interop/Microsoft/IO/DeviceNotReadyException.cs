namespace Microsoft.IO
{
    using System;
    using System.IO;

    public class DeviceNotReadyException : IOException
    {
        public DeviceNotReadyException() : base("Device is not ready", -2147024875)
        {
        }

        public DeviceNotReadyException(string message) : base(message, -2147024875)
        {
        }

        public DeviceNotReadyException(string message, Exception innerException) : base(message, innerException)
        {
            base.HResult = -2147024875;
        }
    }
}

