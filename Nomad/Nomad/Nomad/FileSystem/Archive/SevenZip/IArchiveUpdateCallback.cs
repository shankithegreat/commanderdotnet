﻿namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("23170F69-40C1-278A-0000-000600800000"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IArchiveUpdateCallback
    {
        void SetTotal(ulong total);
        void SetCompleted([In] ref ulong completeValue);
        void GetUpdateItemInfo(int index, out int newData, out int newProperties, out uint indexInArchive);
        void GetProperty(int index, ItemPropId propID, IntPtr value);
        void GetStream(int index, out ISequentialInStream inStream);
        void SetOperationResult(int operationResult);
    }
}

