namespace Nomad.Workers
{
    using System;
    using System.Runtime.CompilerServices;

    internal delegate bool AsyncBuffersEqualCaller(byte[] buffer1, byte[] buffer2, int length);
}

