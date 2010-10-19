namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int GetHandlerProperty2Delegate(uint formatIndex, ArchivePropId propID, ref PropVariant value);
}

