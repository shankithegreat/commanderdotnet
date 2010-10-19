namespace Nomad.FileSystem.Shell
{
    using Microsoft.COM;
    using Microsoft.Shell;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Runtime.Serialization;

    [Serializable]
    public class ChangeShellFile : ShellFile, IChangeVirtualFile, IVirtualFile, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        protected ChangeShellFile(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        internal ChangeShellFile(SafeShellItem item, SFGAO attributes, IVirtualFolder parent) : base(item, attributes, parent)
        {
        }

        public Stream Open(FileMode mode, FileAccess access, FileShare share, FileOptions options, long startOffset)
        {
            if (!base.CheckItemAttribute(SFGAO.SFGAO_STREAM))
            {
                return null;
            }
            IStream baseStream = null;
            IShellFolder folder = base.ItemInfo.GetFolder();
            try
            {
                baseStream = folder.BindToStorage<IStream>(base.ItemInfo.RelativePidl);
            }
            finally
            {
                Marshal.ReleaseComObject(folder);
            }
            Stream stream2 = new ComStreamWrapper(baseStream, access, ComRelease.Final);
            if (startOffset > 0L)
            {
                if (stream2.CanSeek)
                {
                    stream2.Seek(startOffset, SeekOrigin.Begin);
                }
                else
                {
                    int num;
                    byte[] buffer = new byte[0x400];
                    do
                    {
                        num = stream2.Read(buffer, 0, (int) Math.Min((long) buffer.Length, startOffset));
                        startOffset -= num;
                    }
                    while ((num > 0) && (startOffset > 0L));
                }
            }
            return stream2;
        }

        public bool CanSeek
        {
            get
            {
                return base.CheckItemAttribute(SFGAO.SFGAO_FILESYSTEM);
            }
        }
    }
}

