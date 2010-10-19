namespace Microsoft.IO
{
    using Microsoft;
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using Nomad.Interop.Commons;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    public class AlternateDataStreamInfo : MarshalByRefObject
    {
        private string FFileName;
        private long? FLength;
        private string FStreamName;

        private AlternateDataStreamInfo(string fileName)
        {
            this.FFileName = fileName;
        }

        public AlternateDataStreamInfo(string fileName, string streamName)
        {
            this.FFileName = fileName;
            this.FStreamName = streamName;
        }

        private AlternateDataStreamInfo(string fileName, string streamName, long length)
        {
            this.FFileName = fileName;
            this.FStreamName = streamName;
            this.FLength = new long?(length);
        }

        public FileStream Create()
        {
            return this.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        }

        public void Delete()
        {
            if (!Windows.DeleteFile(this.FullName))
            {
                throw IoHelper.GetIOException();
            }
            this.FLength = null;
        }

        public static IEnumerable<AlternateDataStreamInfo> GetStreams(string fileName)
        {
            return new <GetStreams>d__0(-2) { <>3__fileName = fileName };
        }

        public FileStream Open(FileMode mode)
        {
            return this.Open(mode, FileAccess.ReadWrite, FileShare.None, FileOptions.None);
        }

        public FileStream Open(FileMode mode, FileAccess access)
        {
            return this.Open(mode, access, FileShare.None, FileOptions.None);
        }

        public FileStream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return this.Open(mode, access, share, FileOptions.None);
        }

        public FileStream Open(FileMode mode, FileAccess access, FileShare share, FileOptions options)
        {
            return new FileStream(Microsoft.IO.File.Open(this.FullName, mode, access, share, options), access, 0x1000, (options & FileOptions.Asynchronous) > FileOptions.None);
        }

        public FileStream OpenRead()
        {
            return this.Open(FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.None);
        }

        public FileStream OpenWrite()
        {
            return this.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, FileOptions.None);
        }

        public bool Exists
        {
            get
            {
                using (SafeFileHandle handle = Windows.CreateFile(this.FullName, 0, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, FileOptions.None, IntPtr.Zero))
                {
                    return !handle.IsInvalid;
                }
            }
        }

        public string FileName
        {
            get
            {
                return this.FFileName;
            }
        }

        public string FullName
        {
            get
            {
                return (this.FFileName + this.FStreamName);
            }
        }

        public bool IsDefault
        {
            get
            {
                return ((this.FStreamName == null) || string.Equals(this.FStreamName, "::$DATA", StringComparison.Ordinal));
            }
        }

        public long Length
        {
            get
            {
                if (!this.FLength.HasValue)
                {
                    using (SafeFileHandle handle = Microsoft.IO.File.OpenReadAttributes(this.FullName))
                    {
                        this.FLength = new long?(Microsoft.IO.File.GetFileSize(handle));
                    }
                }
                return this.FLength.Value;
            }
        }

        public string StreamName
        {
            get
            {
                return this.FStreamName;
            }
        }

        [CompilerGenerated]
        private sealed class <GetStreams>d__0 : IEnumerable<AlternateDataStreamInfo>, IEnumerable, IEnumerator<AlternateDataStreamInfo>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private AlternateDataStreamInfo <>2__current;
            public string <>3__fileName;
            public Microsoft.Win32.SafeHandles.SafeFindHandle <>7__wrapd;
            private int <>l__initialThreadId;
            public Stream <BackupStream>5__6;
            public byte[] <buffer>5__7;
            public int <ErrorCode>5__3;
            public int <ErrorCode>5__4;
            public Microsoft.Win32.SafeHandles.SafeFindHandle <FindHandle>5__2;
            public SafeFileHandle <Handle>5__5;
            public bool <IsFolder>5__c;
            public char <LastChar>5__b;
            public string <name>5__a;
            public int <Readed>5__8;
            public WIN32_FIND_STREAM_DATA <StreamData>5__1;
            public WIN32_STREAM_ID <streamID>5__9;
            public string fileName;

            [DebuggerHidden]
            public <GetStreams>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally10()
            {
                this.<>1__state = 4;
                if (this.<BackupStream>5__6 != null)
                {
                    this.<BackupStream>5__6.Dispose();
                }
            }

            private void <>m__Finallye()
            {
                this.<>1__state = -1;
                if (this.<>7__wrapd != null)
                {
                    this.<>7__wrapd.Dispose();
                }
            }

            private void <>m__Finallyf()
            {
                this.<>1__state = -1;
                if (this.<Handle>5__5 != null)
                {
                    this.<Handle>5__5.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    bool flag2;
                    switch (this.<>1__state)
                    {
                        case 0:
                        {
                            this.<>1__state = -1;
                            if (this.fileName == null)
                            {
                                throw new ArgumentNullException();
                            }
                            if (this.fileName == string.Empty)
                            {
                                throw new ArgumentException();
                            }
                            if (!OS.IsWinVista)
                            {
                                goto Label_01C7;
                            }
                            this.<FindHandle>5__2 = Windows.FindFirstStreamW(this.fileName, STREAM_INFO_LEVELS.FindStreamInfoStandard, out this.<StreamData>5__1, 0);
                            if (!this.<FindHandle>5__2.IsInvalid)
                            {
                                break;
                            }
                            this.<ErrorCode>5__3 = Marshal.GetLastWin32Error();
                            int num = this.<ErrorCode>5__3;
                            if (num == 0x26)
                            {
                                goto Label_04A7;
                            }
                            if (num != 0x57)
                            {
                                throw IoHelper.GetIOException(this.<ErrorCode>5__3);
                            }
                            if (!System.IO.File.Exists(this.fileName))
                            {
                                goto Label_04A7;
                            }
                            this.<>2__current = new AlternateDataStreamInfo(this.fileName);
                            this.<>1__state = 1;
                            return true;
                        }
                        case 1:
                            this.<>1__state = -1;
                            goto Label_04A7;

                        case 3:
                            this.<>1__state = 2;
                            if (Windows.FindNextStreamW(this.<FindHandle>5__2, out this.<StreamData>5__1))
                            {
                                goto Label_0141;
                            }
                            this.<ErrorCode>5__4 = Marshal.GetLastWin32Error();
                            if (this.<ErrorCode>5__4 != 0x26)
                            {
                                throw IoHelper.GetIOException(this.<ErrorCode>5__4);
                            }
                            this.<>m__Finallye();
                            goto Label_04A7;

                        case 6:
                            goto Label_03A3;

                        case 7:
                            this.<>1__state = -1;
                            goto Label_04A7;

                        default:
                            goto Label_04A7;
                    }
                    this.<>7__wrapd = this.<FindHandle>5__2;
                    this.<>1__state = 2;
                Label_0141:
                    this.<>2__current = new AlternateDataStreamInfo(this.fileName, this.<StreamData>5__1.cStreamName, this.<StreamData>5__1.StreamSize);
                    this.<>1__state = 3;
                    return true;
                Label_01C7:
                    if (!OS.IsWinNT)
                    {
                        goto Label_03F8;
                    }
                    this.<Handle>5__5 = Windows.CreateFile(this.fileName, FileAccess.Read, FileShare.Read, IntPtr.Zero, FileMode.Open, 0x22000000, IntPtr.Zero);
                    this.<>1__state = 4;
                    if (this.<Handle>5__5.IsInvalid)
                    {
                        throw IoHelper.GetIOException();
                    }
                    this.<BackupStream>5__6 = new BackupFileStream(this.<Handle>5__5, FileAccess.Read);
                    this.<>1__state = 5;
                    this.<buffer>5__7 = new byte[0x400];
                    goto Label_03DB;
                Label_0249:
                    this.<Readed>5__8 = this.<BackupStream>5__6.Read(this.<buffer>5__7, 0, Marshal.SizeOf(typeof(WIN32_STREAM_ID)));
                    if (this.<Readed>5__8 == 0)
                    {
                        goto Label_03E2;
                    }
                    this.<streamID>5__9 = ByteArrayHelper.ByteArrayToStructure<WIN32_STREAM_ID>(this.<buffer>5__7);
                    this.<name>5__a = null;
                    if (this.<streamID>5__9.dwStreamNameSize > 0)
                    {
                        this.<Readed>5__8 = this.<BackupStream>5__6.Read(this.<buffer>5__7, 0, Math.Min(this.<buffer>5__7.Length, this.<streamID>5__9.dwStreamNameSize));
                        this.<name>5__a = Encoding.Unicode.GetString(this.<buffer>5__7, 0, this.<Readed>5__8);
                    }
                    StreamId dwStreamId = this.<streamID>5__9.dwStreamId;
                    if ((dwStreamId != StreamId.BACKUP_DATA) && (dwStreamId != StreamId.BACKUP_ALTERNATE_DATA))
                    {
                        goto Label_03AC;
                    }
                    if ((this.<name>5__a == null) && (this.<streamID>5__9.dwStreamId == StreamId.BACKUP_DATA))
                    {
                        VolumeInfo info = VolumeCache.FromPath(this.fileName);
                        if ((info != null) && ((info.Capabilities & VolumeCapabilities.FileNamedStreams) > ((VolumeCapabilities) 0)))
                        {
                            this.<name>5__a = "::$DATA";
                        }
                    }
                    this.<>2__current = new AlternateDataStreamInfo(this.fileName, this.<name>5__a, this.<streamID>5__9.Size);
                    this.<>1__state = 6;
                    return true;
                Label_03A3:
                    this.<>1__state = 5;
                Label_03AC:
                    if (this.<streamID>5__9.Size > 0L)
                    {
                        this.<BackupStream>5__6.Seek(this.<streamID>5__9.Size, SeekOrigin.Current);
                    }
                Label_03DB:
                    flag2 = true;
                    goto Label_0249;
                Label_03E2:
                    this.<>m__Finally10();
                    this.<>m__Finallyf();
                    goto Label_04A7;
                Label_03F8:
                    this.<LastChar>5__b = this.fileName[this.fileName.Length - 1];
                    this.<IsFolder>5__c = (this.<LastChar>5__b == Path.DirectorySeparatorChar) || (this.<LastChar>5__b == Path.AltDirectorySeparatorChar);
                    if (!this.<IsFolder>5__c && System.IO.File.Exists(this.fileName))
                    {
                        this.<>2__current = new AlternateDataStreamInfo(this.fileName);
                        this.<>1__state = 7;
                        return true;
                    }
                    if (!System.IO.Directory.Exists(this.fileName))
                    {
                        if (this.<IsFolder>5__c)
                        {
                            throw new DirectoryNotFoundException();
                        }
                        throw new FileNotFoundException();
                    }
                Label_04A7:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<AlternateDataStreamInfo> IEnumerable<AlternateDataStreamInfo>.GetEnumerator()
            {
                AlternateDataStreamInfo.<GetStreams>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new AlternateDataStreamInfo.<GetStreams>d__0(0);
                }
                d__.fileName = this.<>3__fileName;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Microsoft.IO.AlternateDataStreamInfo>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 2:
                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finallye();
                        }
                        break;

                    case 4:
                    case 5:
                    case 6:
                        try
                        {
                            switch (this.<>1__state)
                            {
                                case 5:
                                case 6:
                                    break;

                                default:
                                    break;
                            }
                            try
                            {
                            }
                            finally
                            {
                                this.<>m__Finally10();
                            }
                        }
                        finally
                        {
                            this.<>m__Finallyf();
                        }
                        break;
                }
            }

            AlternateDataStreamInfo IEnumerator<AlternateDataStreamInfo>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

