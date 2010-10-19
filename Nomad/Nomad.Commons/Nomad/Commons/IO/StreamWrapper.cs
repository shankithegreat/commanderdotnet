namespace Nomad.Commons.IO
{
    using System;
    using System.IO;

    public class StreamWrapper : CustomStreamWrapper
    {
        private Stream FBaseStream;

        public StreamWrapper(Stream baseStream)
        {
            this.FBaseStream = baseStream;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.FBaseStream = null;
            }
            base.Dispose(disposing);
        }

        protected override Stream BaseStream
        {
            get
            {
                return this.FBaseStream;
            }
        }
    }
}

