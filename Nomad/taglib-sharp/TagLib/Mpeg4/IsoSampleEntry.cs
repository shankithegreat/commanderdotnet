namespace TagLib.Mpeg4
{
    using System;
    using TagLib;

    public class IsoSampleEntry : Box
    {
        private ushort data_reference_index;

        public IsoSampleEntry(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, handler)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            file.Seek(base.DataPosition + 6L);
            this.data_reference_index = file.ReadBlock(2).ToUShort();
        }

        protected override long DataPosition
        {
            get
            {
                return (base.DataPosition + 8L);
            }
        }

        public ushort DataReferenceIndex
        {
            get
            {
                return this.data_reference_index;
            }
        }
    }
}

