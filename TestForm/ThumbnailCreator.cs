using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ShellDll;

namespace TestForm
{
    public class ThumbnailCreator : IDisposable
    {
        private IMalloc alloc;
        private bool disposed;


        public ThumbnailCreator()
        {
            this.DesiredSize = new Size(100, 100);
        }

        ~ThumbnailCreator()
        {
            Dispose();
        }


        public Bitmap ThumbNail { get; private set; }

        public Size DesiredSize { get; set; }

        private IMalloc Allocator
        {
            get
            {
                if (!this.disposed)
                {
                    if (this.alloc == null)
                    {
                        // Fails here at times
                        UnManagedMethods.SHGetMalloc(out this.alloc);
                    }
                }
                else
                {
                    Debug.Assert(false, "Object has been disposed.");
                }

                return this.alloc;
            }
        }

        private static IShellFolder DesktopFolder
        {
            get
            {
                IShellFolder folder;
                UnManagedMethods.SHGetDesktopFolder(out folder);
                return folder;
            }
        }


        public void Dispose()
        {
            if (!this.disposed)
            {
                if (this.alloc != null)
                {
                    Marshal.ReleaseComObject(this.alloc);
                }
                this.alloc = null;

                if (this.ThumbNail != null)
                {
                    this.ThumbNail.Dispose();
                }

                this.disposed = true;
            }
        }

        public Bitmap GetThumbnail(string file)
        {
            if ((!File.Exists(file)) && (!Directory.Exists(file)))
            {
                throw new FileNotFoundException(String.Format("The file '{0}' does not exist", file), file);
            }

            if (this.ThumbNail != null)
            {
                this.ThumbNail.Dispose();
                this.ThumbNail = null;
            }

            IShellFolder folder = DesktopFolder;

            if (folder != null)
            {
                IntPtr pidlMain;
                try
                {
                    int cParsed;
                    int pdwAttrib;
                    string filePath = Path.GetDirectoryName(file);
                    folder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, filePath, out cParsed, out pidlMain, out pdwAttrib);
                }
                catch
                {
                    Marshal.ReleaseComObject(folder);
                    throw;
                }

                if (pidlMain != IntPtr.Zero)
                {
                    // IShellFolder:
                    Guid iidShellFolder = new Guid("000214E6-0000-0000-C000-000000000046");
                    IShellFolder item = null;

                    try
                    {
                        folder.BindToObject(pidlMain, IntPtr.Zero, ref iidShellFolder, ref item);
                    }
                    catch
                    {
                        Marshal.ReleaseComObject(folder);
                        this.Allocator.Free(pidlMain);

                        throw;
                    }

                    if (item != null)
                    {
                        //
                        IEnumIDList idEnum = null;
                        try
                        {
                            item.EnumObjects(IntPtr.Zero, (ShellAPI.SHCONTF.FOLDERS | ShellAPI.SHCONTF.NONFOLDERS), ref idEnum);
                        }
                        catch
                        {
                            Marshal.ReleaseComObject(folder);
                            this.Allocator.Free(pidlMain);

                            throw;
                        }

                        if (idEnum != null)
                        {
                            // start reading the enum:
                            bool complete = false;
                            while (!complete)
                            {
                                int fetched;
                                IntPtr pidl;
                                int hRes = idEnum.Next(1, out pidl, out fetched);
                                if (hRes != 0)
                                {
                                    pidl = IntPtr.Zero;
                                    complete = true;
                                }
                                else
                                {
                                    if (GetThumbnail(file, pidl, item))
                                    {
                                        complete = true;
                                    }
                                }
                                if (pidl != IntPtr.Zero)
                                {
                                    this.Allocator.Free(pidl);
                                }
                            }

                            Marshal.ReleaseComObject(idEnum);
                        }


                        Marshal.ReleaseComObject(item);
                    }

                    this.Allocator.Free(pidlMain);
                }

                Marshal.ReleaseComObject(folder);
            }
            return this.ThumbNail;
        }


        private bool GetThumbnail(string file, IntPtr pidl, IShellFolder item)
        {
            IntPtr bmp = IntPtr.Zero;
            IExtractImage extractImage = null;

            try
            {
                string pidlPath = PathFromPidl(pidl);
                if (Path.GetFileName(pidlPath).ToUpper().Equals(Path.GetFileName(file).ToUpper()))
                {
                    // we have the item:
                    IUnknown iunk = null;
                    int prgf;
                    Guid iidExtractImage = new Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1");
                    item.GetUIObjectOf(IntPtr.Zero, 1, ref pidl, ref iidExtractImage, out prgf, ref iunk);
                    extractImage = (IExtractImage)iunk;

                    if (extractImage != null)
                    {
                        //Got an IExtractImage object!
                        ShellAPI.SIZE size = new ShellAPI.SIZE { cx = this.DesiredSize.Width, cy = this.DesiredSize.Height };
                        StringBuilder location = new StringBuilder(260, 260);
                        int priority = 0;
                        const int requestedColourDepth = 32;
                        const IEIFLAG flags = IEIFLAG.IEIFLAG_ASPECT | IEIFLAG.IEIFLAG_SCREEN;
                        int uFlags = (int)flags;

                        extractImage.GetLocation(location, location.Capacity, ref priority, ref size, requestedColourDepth, ref uFlags);

                        extractImage.Extract(out bmp);
                        if (bmp != IntPtr.Zero)
                        {
                            // create the image object:
                            this.ThumbNail = Image.FromHbitmap(bmp);
                            // is thumbNail owned by the Bitmap?
                        }

                        Marshal.ReleaseComObject(extractImage);
                        extractImage = null;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                if (bmp != IntPtr.Zero)
                {
                    UnManagedMethods.DeleteObject(bmp);
                }
                if (extractImage != null)
                {
                    Marshal.ReleaseComObject(extractImage);
                }

                throw;
            }
        }

        private string PathFromPidl(IntPtr pidl)
        {
            StringBuilder path = new StringBuilder(260, 260);
            bool result = ShellAPI.SHGetPathFromIDList(pidl, path);

            return (result ? path.ToString() : string.Empty);
        }



        [Flags]
        private enum IEIFLAG
        {
            IEIFLAG_ASYNC = 0x0001, // ask the extractor if it supports ASYNC extract (free threaded)
            IEIFLAG_CACHE = 0x0002, // returned from the extractor if it does NOT cache the thumbnail
            IEIFLAG_ASPECT = 0x0004, // passed to the extractor to beg it to render to the aspect ratio of the supplied rect
            IEIFLAG_OFFLINE = 0x0008, // if the extractor shouldn't hit the net to get any content neede for the rendering
            IEIFLAG_GLEAM = 0x0010, // does the image have a gleam ? this will be returned if it does
            IEIFLAG_SCREEN = 0x0020, // render as if for the screen (this is exlusive with IEIFLAG_ASPECT )
            IEIFLAG_ORIGSIZE = 0x0040, // render to the approx size passed, but crop if neccessary
            IEIFLAG_NOSTAMP = 0x0080, // returned from the extractor if it does NOT want an icon stamp on the thumbnail
            IEIFLAG_NOBORDER = 0x0100, // returned from the extractor if it does NOT want an a border around the thumbnail
            IEIFLAG_QUALITY = 0x0200 // passed to the Extract method to indicate that a slower, higher quality image is desired, re-compute the thumbnail
        }

        [Flags]
        private enum STRRET
        {
            STRRET_WSTR = 0x0000, // Use STRRET.pOleStr
            STRRET_OFFSET = 0x0001, // Use STRRET.uOffset to Ansi
            STRRET_CSTR = 0x0002 // Use STRRET.cStr
        }

        [ComImport]
        [Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        //helpstring("IExtractImage"),
        private interface IExtractImage
        {
            void GetLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPathBuffer, int cch, ref int pdwPriority, ref ShellAPI.SIZE prgSize, int dwRecClrDepth, ref int pdwFlags);

            void Extract(out IntPtr phBmpThumbnail);
        }

        [ComImport]
        [Guid("00000002-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        //helpstring("IMalloc interface")
        private interface IMalloc
        {
            [PreserveSig]
            IntPtr Alloc(int cb);

            [PreserveSig]
            IntPtr Realloc(IntPtr pv, int cb);

            [PreserveSig]
            void Free(IntPtr pv);

            [PreserveSig]
            int GetSize(IntPtr pv);

            [PreserveSig]
            int DidAlloc(IntPtr pv);

            [PreserveSig]
            void HeapMinimize();
        } ;

        [ComImport]
        [Guid("000214E6-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellFolder
        {
            void ParseDisplayName(IntPtr hwndOwner, IntPtr pbcReserved, [MarshalAs(UnmanagedType.LPWStr)] string lpszDisplayName, out int pchEaten, out IntPtr ppidl, out int pdwAttributes);

            void EnumObjects(IntPtr hwndOwner, [MarshalAs(UnmanagedType.U4)] ShellAPI.SHCONTF grfFlags, ref IEnumIDList ppenumIDList);

            void BindToObject(IntPtr pidl, IntPtr pbcReserved, ref Guid riid, ref IShellFolder ppvOut);

            void BindToStorage(IntPtr pidl, IntPtr pbcReserved, ref Guid riid, IntPtr ppvObj);

            [PreserveSig]
            int CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);

            void CreateViewObject(IntPtr hwndOwner, ref Guid riid, IntPtr ppvOut);

            void GetAttributesOf(int cidl, IntPtr apidl, [MarshalAs(UnmanagedType.U4)] ref ShellAPI.SFGAO rgfInOut);

            void GetUIObjectOf(IntPtr hwndOwner, int cidl, ref IntPtr apidl, ref Guid riid, out int prgfInOut, ref IUnknown ppvOut);

            void GetDisplayNameOf(IntPtr pidl, [MarshalAs(UnmanagedType.U4)] ShellAPI.SHGNO uFlags, ref STRRET_CSTR lpName);

            void SetNameOf(IntPtr hwndOwner, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string lpszName, [MarshalAs(UnmanagedType.U4)] ShellAPI.SHCONTF uFlags, ref IntPtr ppidlOut);
        } ;

        [ComImport]
        [Guid("00000000-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IUnknown
        {
            [PreserveSig]
            IntPtr QueryInterface(ref Guid riid, out IntPtr pVoid);

            [PreserveSig]
            IntPtr AddRef();

            [PreserveSig]
            IntPtr Release();
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
        private struct STRRET_ANY
        {
            [FieldOffset(0)]
            public STRRET uType;

            [FieldOffset(4)]
            public IntPtr pOLEString;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0, CharSet = CharSet.Auto)]
        private struct STRRET_CSTR
        {
            public STRRET uType;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 520)]
            public byte[] cStr;
        }

        private class UnManagedMethods
        {
            [DllImport("shell32", CharSet = CharSet.Auto)]
            internal static extern int SHGetMalloc(out IMalloc ppMalloc);

            [DllImport("shell32", CharSet = CharSet.Auto)]
            internal static extern int SHGetDesktopFolder(out IShellFolder ppshf);

            [DllImport("gdi32", CharSet = CharSet.Auto)]
            internal static extern int DeleteObject(IntPtr hObject);
        }
    }
}