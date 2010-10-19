namespace Microsoft.Win32.SafeHandles
{
    using System;

    public sealed class SafePipeHandle : SafeBasicHandle
    {
        public SafePipeHandle() : base(true)
        {
        }
    }
}

