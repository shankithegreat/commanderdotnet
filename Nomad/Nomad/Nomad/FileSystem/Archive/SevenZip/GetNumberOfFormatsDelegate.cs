﻿namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int GetNumberOfFormatsDelegate(out uint numFormats);
}

