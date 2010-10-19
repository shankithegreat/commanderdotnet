namespace Microsoft.IO.Pipes
{
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class NamedPipeStream : FileStream
    {
        private ListenHandler AsyncListen;
        private const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        private PeerType FPeerType;
        private const uint NMPWAIT_WAIT_FOREVER = uint.MaxValue;
        private const uint PIPE_ACCESS_DUPLEX = 3;
        private const uint PIPE_ACCESS_INBOUND = 1;
        private const uint PIPE_ACCESS_OUTBOUND = 2;
        private const uint PIPE_READMODE_BYTE = 0;
        private const uint PIPE_TYPE_BYTE = 0;
        private const uint PIPE_UNLIMITED_INSTANCES = 0xff;
        private const uint PIPE_WAIT = 0;
        private SafePipeHandle PipeHandle;

        public NamedPipeStream(string pipeName, FileAccess access) : base(Windows.CreateFile(pipeName, access, FileShare.None, IntPtr.Zero, FileMode.Open, FileOptions.None, IntPtr.Zero), access)
        {
            this.FPeerType = PeerType.Client;
        }

        protected NamedPipeStream(SafeFileHandle handle, FileAccess access, bool isAsync) : base(handle, access, 0x8000, isAsync)
        {
            this.FPeerType = PeerType.Client;
        }

        public IAsyncResult BeginListen()
        {
            this.Disconnect();
            if (this.IsAsync)
            {
                ListenAsyncResult result = new ListenAsyncResult();
                Windows.ConnectNamedPipe(this.PipeHandle, result.OverlappedPtr);
                return result;
            }
            return this.AsyncListen.BeginInvoke(null, null);
        }

        public static bool Check(string pipeName, FileAccess access)
        {
            SafeFileHandle handle = Windows.CreateFile(pipeName, access, FileShare.None, IntPtr.Zero, FileMode.Open, FileOptions.None, IntPtr.Zero);
            if (handle.IsInvalid)
            {
                return false;
            }
            handle.Close();
            return true;
        }

        public override void Close()
        {
            base.Close();
            this.PipeHandle.SetHandleAsInvalid();
        }

        public static NamedPipeStream Create(string pipeName, ServerMode mode, bool isAsync)
        {
            NamedPipeStream stream;
            SafePipeHandle handle = Windows.CreateNamedPipe(@"\\.\pipe\" + pipeName, (uint) (mode | (isAsync ? ((ServerMode) 0x40000000) : ((ServerMode) 0))), 0, 0xff, 0, 0x400, uint.MaxValue, IntPtr.Zero);
            if (handle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(HRESULT.HRESULT_FROM_WIN32(Marshal.GetLastWin32Error()));
            }
            FileAccess read = 0;
            switch (mode)
            {
                case ServerMode.Inbound:
                    read = FileAccess.Read;
                    break;

                case ServerMode.Outbound:
                    read = FileAccess.Write;
                    break;

                case ServerMode.Bidirectional:
                    read = FileAccess.ReadWrite;
                    break;
            }
            return new NamedPipeStream(new SafeFileHandle(handle.DangerousGetHandle(), true), read, isAsync) { PipeHandle = handle, FPeerType = PeerType.Server, AsyncListen = new ListenHandler(stream.Listen) };
        }

        public void Disconnect()
        {
            if (this.FPeerType != PeerType.Server)
            {
                throw new InvalidOperationException("Disconnect() is only for server-side streams");
            }
            Windows.DisconnectNamedPipe(this.PipeHandle);
        }

        public bool EndListen(IAsyncResult asyncResult)
        {
            if (this.IsAsync)
            {
                ListenAsyncResult result = asyncResult as ListenAsyncResult;
                if (result == null)
                {
                    throw new ArgumentException("asyncResult is not ListenAsyncResult");
                }
                using (ListenAsyncResult result2 = result)
                {
                    uint num;
                    return Windows.GetOverlappedResult(this.SafeFileHandle, result2.OverlappedPtr, out num, true);
                }
            }
            return this.AsyncListen.EndInvoke(asyncResult);
        }

        public bool Listen()
        {
            this.Disconnect();
            if (!Windows.ConnectNamedPipe(this.PipeHandle, IntPtr.Zero) && (Marshal.GetLastWin32Error() != 0x217))
            {
                return false;
            }
            return true;
        }

        public bool DataAvailable
        {
            get
            {
                uint num;
                uint num2;
                uint num3;
                return (Windows.PeekNamedPipe(this.PipeHandle, null, 0, out num, out num2, out num3) && (num2 > 0));
            }
        }

        public bool IsConnected
        {
            get
            {
                if (this.FPeerType != PeerType.Server)
                {
                    throw new InvalidOperationException("IsConnected() is only for server-side streams");
                }
                return (!Windows.ConnectNamedPipe(this.PipeHandle, IntPtr.Zero) && (Marshal.GetLastWin32Error() == 0x217));
            }
        }

        private delegate bool ListenHandler();

        private enum PeerType
        {
            Client,
            Server
        }

        public enum ServerMode : uint
        {
            Bidirectional = 3,
            Inbound = 1,
            Outbound = 2
        }
    }
}

