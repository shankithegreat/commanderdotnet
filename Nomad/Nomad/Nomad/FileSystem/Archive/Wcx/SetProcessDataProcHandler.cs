﻿namespace Nomad.FileSystem.Archive.Wcx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void SetProcessDataProcHandler(IntPtr hArcData, ProcessDataProcCallback pProcessDataProc);
}

