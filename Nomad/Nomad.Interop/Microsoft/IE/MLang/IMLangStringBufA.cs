namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComConversionLoss, Guid("D24ACD23-BA72-11D0-B188-00AA0038C969")]
    public interface IMLangStringBufA
    {
        void GetStatus(out int plFlags, out int pcchBuf);
        void LockBuf(int cchOffset, int cchMaxLock, [Out] IntPtr ppszBuf, out int pcchBuf);
        void UnlockBuf([In] ref sbyte pszBuf, int cchOffset, int cchWrite);
        void Insert(int cchOffset, int cchMaxInsert, out int pcchActual);
        void Delete(int cchOffset, int cchDelete);
    }
}

