namespace TagLib.Mpeg4
{
    using System;
    using TagLib;

    public class UnknownBox : Box
    {
        private ByteVector data;

        public UnknownBox(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, handler)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            this.data = base.LoadData(file);
        }

        public override ByteVector Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }
    }
}

