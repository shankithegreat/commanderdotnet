namespace Nomad
{
    using Microsoft.COM;
    using Microsoft.Win32;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Windows.Forms;

    internal class VirtualClipboardFile : VirtualClipboardItem, IChangeVirtualFile, IVirtualFile, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        private System.Runtime.InteropServices.ComTypes.IDataObject FDataObject;
        private int FIndex;
        private long? FSize;

        public VirtualClipboardFile(FILEDESCRIPTORA descriptor, System.Runtime.InteropServices.ComTypes.IDataObject dataObject, int index) : base(descriptor)
        {
            if ((descriptor.dwFlags & FD.FD_FILESIZE) > ((FD) 0))
            {
                this.FSize = new long?((long) ((descriptor.nFileSizeHigh << 0x20) | descriptor.nFileSizeLow));
            }
            this.FDataObject = dataObject;
            this.FIndex = index;
        }

        public VirtualClipboardFile(FILEDESCRIPTORW descriptor, System.Runtime.InteropServices.ComTypes.IDataObject dataObject, int index) : base(descriptor)
        {
            if ((descriptor.dwFlags & FD.FD_FILESIZE) > ((FD) 0))
            {
                this.FSize = new long?((long) ((descriptor.nFileSizeHigh << 0x20) | descriptor.nFileSizeLow));
            }
            this.FDataObject = dataObject;
            this.FIndex = index;
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = base.CreateAvailableSet();
            set[3] = this.FSize.HasValue;
            return set;
        }

        public Stream Open(FileMode mode, FileAccess access, FileShare share, FileOptions options, long startOffset)
        {
            Stream stream2;
            STGMEDIUM Medium;
            if ((mode != FileMode.Open) || (access != FileAccess.Read))
            {
                throw new ArgumentException("Only open file mode and read file access supported.");
            }
            DataFormats.Format format = DataFormats.GetFormat("FileContents");
            if (format == null)
            {
                return null;
            }
            FORMATETC formatetc = new FORMATETC {
                cfFormat = (short) format.Id,
                dwAspect = DVASPECT.DVASPECT_CONTENT,
                lindex = this.FIndex,
                tymed = TYMED.TYMED_ISTREAM | TYMED.TYMED_HGLOBAL
            };
            this.FDataObject.GetData(ref formatetc, out Medium);
            try
            {
                byte[] buffer2;
                System.Runtime.InteropServices.ComTypes.IStream MediumStream;
                TYMED tymed = Medium.tymed;
                if (tymed != TYMED.TYMED_HGLOBAL)
                {
                    if (tymed != TYMED.TYMED_ISTREAM)
                    {
                        throw new ApplicationException(string.Format("Unsupported STGMEDIUM.tymed ({0})", Medium.tymed));
                    }
                    MediumStream = (System.Runtime.InteropServices.ComTypes.IStream) Marshal.GetTypedObjectForIUnknown(Medium.unionmember, typeof(System.Runtime.InteropServices.ComTypes.IStream));
                    ComStreamWrapper wrapper = new ComStreamWrapper(MediumStream, FileAccess.Read, ComRelease.None);
                    if (startOffset > 0L)
                    {
                        if (wrapper.CanSeek)
                        {
                            wrapper.Seek(startOffset, SeekOrigin.Begin);
                        }
                        else
                        {
                            byte[] buffer = new byte[0x100];
                            int num = 1;
                            while ((startOffset > 0L) && (num > 0))
                            {
                                num = wrapper.Read(buffer, 0, (int) Math.Min((long) buffer.Length, startOffset));
                                startOffset -= num;
                            }
                        }
                    }
                    wrapper.Closed += delegate (object sender, EventArgs e) {
                        Microsoft.COM.ActiveX.ReleaseStgMedium(ref Medium);
                        Marshal.FinalReleaseComObject(MediumStream);
                    };
                    return wrapper;
                }
                IntPtr hMem = Windows.GlobalLock(Medium.unionmember);
                try
                {
                    long num2 = this.FSize.HasValue ? this.FSize.Value : Windows.GlobalSize(hMem).ToInt64();
                    buffer2 = new byte[num2];
                    Marshal.Copy(hMem, buffer2, 0, (int) num2);
                }
                finally
                {
                    Windows.GlobalUnlock(Medium.unionmember);
                }
                Microsoft.COM.ActiveX.ReleaseStgMedium(ref Medium);
                Stream stream = new MemoryStream(buffer2, false);
                stream.Seek(startOffset, SeekOrigin.Begin);
                stream2 = stream;
            }
            catch
            {
                Microsoft.COM.ActiveX.ReleaseStgMedium(ref Medium);
                throw;
            }
            return stream2;
        }

        public bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public string Extension
        {
            get
            {
                return Path.GetExtension(base.Name);
            }
        }

        public override object this[int property]
        {
            get
            {
                if (property == 3)
                {
                    return this.FSize;
                }
                return base[property];
            }
        }
    }
}

