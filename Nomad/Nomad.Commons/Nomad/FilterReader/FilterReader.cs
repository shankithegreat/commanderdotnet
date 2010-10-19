namespace Nomad.FilterReader
{
    using Microsoft.COM;
    using Microsoft.COM.IFilter;
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    public class FilterReader : TextReader
    {
        private Stream BaseStream;
        private char[] Buffer;
        private uint BufferLen;
        private int BufferPos;
        private STAT_CHUNK CurrentChunk;
        private bool CurrentChunkValid;
        private bool Done;
        private Microsoft.COM.IFilter.IFilter FFilter;
        private static Dictionary<string, FilterInfo> FilterCache = new Dictionary<string, FilterInfo>(StringComparer.OrdinalIgnoreCase);

        public FilterReader(string fileName)
        {
            this.Buffer = new char[0x2000];
            this.FFilter = LoadAndInitIFilter(fileName, Path.GetExtension(fileName));
            if (this.FFilter == null)
            {
                throw new ArgumentException("no filter defined for " + fileName);
            }
        }

        private FilterReader(Microsoft.COM.IFilter.IFilter filter, Stream stream)
        {
            this.Buffer = new char[0x2000];
            this.FFilter = filter;
            this.BaseStream = stream;
        }

        public FilterReader(Stream stream, string fileName)
        {
            this.Buffer = new char[0x2000];
            this.FFilter = LoadAndInitIFilter(stream, Path.GetExtension(fileName));
            if (this.FFilter == null)
            {
                throw new ArgumentException("no filter defined for " + fileName);
            }
            this.BaseStream = stream;
        }

        public override void Close()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public static Nomad.FilterReader.FilterReader Create(string fileName)
        {
            Microsoft.COM.IFilter.IFilter filter = LoadAndInitIFilter(fileName, Path.GetExtension(fileName));
            if (filter != null)
            {
                return new Nomad.FilterReader.FilterReader(filter, null);
            }
            return null;
        }

        public static Nomad.FilterReader.FilterReader Create(Stream stream, string fileName)
        {
            Microsoft.COM.IFilter.IFilter filter = LoadAndInitIFilter(stream, Path.GetExtension(fileName));
            if (filter != null)
            {
                return new Nomad.FilterReader.FilterReader(filter, stream);
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (this.FFilter != null)
            {
                Marshal.ReleaseComObject(this.FFilter);
            }
            if ((this.BaseStream != null) && disposing)
            {
                this.BaseStream.Close();
            }
        }

        ~FilterReader()
        {
            this.Dispose(false);
        }

        private static bool GetFilterDllAndClassFromPersistentHandler(string persistentHandlerClass, out string dllName, out string filterPersistClass)
        {
            dllName = null;
            filterPersistClass = null;
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Classes"))
            {
                filterPersistClass = ReadStringFromRegistry(key, @"CLSID\" + persistentHandlerClass + @"\PersistentAddinsRegistered\{89BCB740-6119-101A-BCB7-00DD010655AF}");
                if (string.IsNullOrEmpty(filterPersistClass))
                {
                    return false;
                }
                dllName = ReadStringFromRegistry(key, @"CLSID\" + filterPersistClass + @"\InprocServer32");
                return !string.IsNullOrEmpty(dllName);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static FilterInfo GetFilterInfo(string ext)
        {
            FilterInfo info = null;
            if (!FilterCache.TryGetValue(ext, out info))
            {
                string str2;
                string str3;
                string persistentHandlerClass = GetPersistentHandlerClass(ext, true);
                if (!string.IsNullOrEmpty(persistentHandlerClass) && GetFilterDllAndClassFromPersistentHandler(persistentHandlerClass, out str2, out str3))
                {
                    Microsoft.Win32.SafeHandles.SafeLibraryHandle hModule = Windows.LoadLibrary(str2);
                    if (!hModule.IsInvalid)
                    {
                        IntPtr procAddress = Windows.GetProcAddress(hModule, "DllGetClassObject");
                        if (procAddress != IntPtr.Zero)
                        {
                            DllGetClassObject delegateForFunctionPointer = (DllGetClassObject) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(DllGetClassObject));
                            info = new FilterInfo(hModule, new Guid(str3), delegateForFunctionPointer);
                        }
                    }
                }
                FilterCache.Add(ext, info);
            }
            return info;
        }

        private static string GetPersistentHandlerClass(string ext, bool searchContentType)
        {
            string persistentHandlerClassFromExtension = GetPersistentHandlerClassFromExtension(ext);
            if (string.IsNullOrEmpty(persistentHandlerClassFromExtension))
            {
                persistentHandlerClassFromExtension = GetPersistentHandlerClassFromDocumentType(ext);
            }
            if (searchContentType && string.IsNullOrEmpty(persistentHandlerClassFromExtension))
            {
                persistentHandlerClassFromExtension = GetPersistentHandlerClassFromContentType(ext);
            }
            return persistentHandlerClassFromExtension;
        }

        private static string GetPersistentHandlerClassFromContentType(string ext)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Classes"))
            {
                string str = ReadStringFromRegistry(key, ext, "Content Type");
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }
                string str2 = ReadStringFromRegistry(key, @"MIME\Database\Content Type\" + str, "Extension");
                if (string.IsNullOrEmpty(str2) || ext.Equals(str2, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
                return GetPersistentHandlerClass(str2, false);
            }
        }

        private static string GetPersistentHandlerClassFromDocumentType(string ext)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Classes"))
            {
                string str = ReadStringFromRegistry(key, ext);
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }
                string str2 = ReadStringFromRegistry(key, str + @"\CLSID");
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }
                return ReadStringFromRegistry(key, @"CLSID\" + str2 + @"\PersistentHandler");
            }
        }

        private static string GetPersistentHandlerClassFromExtension(string ext)
        {
            return ReadStringFromRegistry(Registry.LocalMachine, @"Software\Classes\" + ext + @"\PersistentHandler");
        }

        private static bool InitIFilter(Microsoft.COM.IFilter.IFilter filter)
        {
            IFILTER_FLAGS ifilter_flags;
            IFILTER_INIT grfFlags = IFILTER_INIT.APPLY_INDEX_ATTRIBUTES | IFILTER_INIT.FILTER_OWNED_VALUE_OK | IFILTER_INIT.CANON_PARAGRAPHS | IFILTER_INIT.CANON_SPACES | IFILTER_INIT.CANON_HYPHENS | IFILTER_INIT.HARD_LINE_BREAKS;
            return (filter.Init(grfFlags, 0, IntPtr.Zero, out ifilter_flags) == 0);
        }

        private static Microsoft.COM.IFilter.IFilter LoadAndInitIFilter(Stream stream, string ext)
        {
            Microsoft.COM.IFilter.IFilter filter = LoadIFilter(ext);
            if (filter != null)
            {
                IPersistStream stream2 = filter as IPersistStream;
                if (stream2 != null)
                {
                    stream2.Load(new ComStream(stream));
                    if (InitIFilter(filter))
                    {
                        return filter;
                    }
                }
                Marshal.ReleaseComObject(filter);
            }
            return null;
        }

        private static Microsoft.COM.IFilter.IFilter LoadAndInitIFilter(string fileName, string ext)
        {
            Microsoft.COM.IFilter.IFilter filter = LoadIFilter(ext);
            if (filter != null)
            {
                IPersistFile file = filter as IPersistFile;
                if (file != null)
                {
                    file.Load(fileName, 0);
                    if (InitIFilter(filter))
                    {
                        return filter;
                    }
                }
                Marshal.ReleaseComObject(filter);
            }
            return null;
        }

        private static Microsoft.COM.IFilter.IFilter LoadIFilter(string ext)
        {
            object obj2;
            object obj3;
            FilterInfo filterInfo = GetFilterInfo(ext);
            if (filterInfo == null)
            {
                return null;
            }
            Guid filterPersistClass = filterInfo.FilterPersistClass;
            if (filterInfo.GetClassObject(ref filterPersistClass, ref ActiveX.IID_IClassFactory, out obj2) != 0)
            {
                return null;
            }
            (obj2 as IClassFactory).CreateInstance(null, typeof(Microsoft.COM.IFilter.IFilter).GUID, out obj3);
            return (obj3 as Microsoft.COM.IFilter.IFilter);
        }

        public override int Peek()
        {
            if (this.Done || ((this.BufferPos >= this.BufferLen) && !this.ReadBuffer()))
            {
                return -1;
            }
            return this.Buffer[this.BufferPos];
        }

        public override int Read()
        {
            if (this.Done || ((this.BufferPos >= this.BufferLen) && !this.ReadBuffer()))
            {
                return -1;
            }
            return this.Buffer[this.BufferPos++];
        }

        public override int Read(char[] array, int offset, int count)
        {
            int num = 0;
            while (!this.Done && (num < count))
            {
                if (!((this.BufferPos < this.BufferLen) || this.ReadBuffer()))
                {
                    return num;
                }
                int length = Math.Min((int) (count - num), (int) (((int) this.BufferLen) - this.BufferPos));
                Array.Copy(this.Buffer, this.BufferPos, array, offset + num, length);
                num += length;
                this.BufferPos += length;
            }
            return num;
        }

        private bool ReadBuffer()
        {
            int num = 0;
            this.BufferPos = 0;
            while (!this.Done)
            {
                int chunk;
                if (!this.CurrentChunkValid)
                {
                    chunk = this.FFilter.GetChunk(out this.CurrentChunk);
                    this.CurrentChunkValid = (chunk == 0) && ((this.CurrentChunk.flags & CHUNKSTATE.CHUNK_TEXT) != ((CHUNKSTATE) 0));
                    if (chunk == -2147215616)
                    {
                        num++;
                    }
                    if (num > 1)
                    {
                        this.Done = true;
                    }
                }
                if (this.CurrentChunkValid)
                {
                    this.BufferLen = (uint) this.Buffer.Length;
                    chunk = this.FFilter.GetText(ref this.BufferLen, this.Buffer);
                    switch (chunk)
                    {
                        case 0x41709:
                        case -2147215615:
                            this.CurrentChunkValid = false;
                            break;
                    }
                    if ((chunk == 0) || (chunk == 0x41709))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static string ReadStringFromRegistry(RegistryKey key, string subKeyName)
        {
            return ReadStringFromRegistry(key, subKeyName, null);
        }

        private static string ReadStringFromRegistry(RegistryKey key, string subKeyName, string valueName)
        {
            RegistryKey key2 = key.OpenSubKey(subKeyName);
            if (key2 == null)
            {
                return null;
            }
            using (key2)
            {
                return (key2.GetValue(valueName) as string);
            }
        }

        private delegate int DllGetClassObject(ref Guid ClassId, ref Guid InterfaceId, [MarshalAs(UnmanagedType.Interface)] out object ppunk);

        private class FilterInfo
        {
            public readonly Microsoft.Win32.SafeHandles.SafeLibraryHandle FilterLibrary;
            public readonly Guid FilterPersistClass;
            public readonly Nomad.FilterReader.FilterReader.DllGetClassObject GetClassObject;

            public FilterInfo(Microsoft.Win32.SafeHandles.SafeLibraryHandle filterLibrary, Guid filterPersistClass, Nomad.FilterReader.FilterReader.DllGetClassObject getClassObject)
            {
                this.FilterLibrary = filterLibrary;
                this.FilterPersistClass = filterPersistClass;
                this.GetClassObject = getClassObject;
            }
        }
    }
}

