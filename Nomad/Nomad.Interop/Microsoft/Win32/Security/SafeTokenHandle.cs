namespace Microsoft.Win32.Security
{
    using Microsoft.Win32.SafeHandles;
    using System;

    public sealed class SafeTokenHandle : SafeBasicHandle
    {
        public SafeTokenHandle() : base(true)
        {
        }
    }
}

