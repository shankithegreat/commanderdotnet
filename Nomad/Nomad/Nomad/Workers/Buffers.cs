namespace Nomad.Workers
{
    using System;

    internal class Buffers
    {
        public readonly byte[] Buffer1;
        public readonly byte[] Buffer2;

        public Buffers(int length)
        {
            this.Buffer1 = new byte[length];
            this.Buffer2 = new byte[length];
        }
    }
}

