namespace Nomad.FileSystem.Archive.SevenZip
{
    using Microsoft.Win32;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting;

    public class InStreamWrapper : StreamWrapper, ISequentialInStream, IInStream
    {
        public InStreamWrapper(Stream baseStream) : base(baseStream)
        {
        }

        public virtual uint Read(IntPtr data, uint size)
        {
            if (!RemotingServices.IsTransparentProxy(base.BaseStream))
            {
                FileStream baseStream = base.BaseStream as FileStream;
                if (baseStream != null)
                {
                    uint num;
                    Windows.ReadFile(baseStream.SafeFileHandle, data, size, out num, IntPtr.Zero);
                    return num;
                }
            }
            byte[] buffer = new byte[Math.Min(0x4000, size)];
            int length = base._BaseStream.Read(buffer, 0, buffer.Length);
            Marshal.Copy(buffer, 0, data, length);
            return (uint) length;
        }
    }
}

