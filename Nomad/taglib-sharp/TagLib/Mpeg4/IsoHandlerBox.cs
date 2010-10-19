namespace TagLib.Mpeg4
{
    using System;
    using TagLib;

    public class IsoHandlerBox : FullBox
    {
        private ByteVector handler_type;
        private string name;

        public IsoHandlerBox(ByteVector handlerType, string name) : base("hdlr", 0, 0)
        {
            if (handlerType == null)
            {
                throw new ArgumentNullException("handlerType");
            }
            if (handlerType.Count < 4)
            {
                throw new ArgumentException("The handler type must be four bytes long.", "handlerType");
            }
            this.handler_type = handlerType.Mid(0, 4);
            this.name = name;
        }

        public IsoHandlerBox(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, file, handler)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            file.Seek(this.DataPosition + 4L);
            ByteVector vector = file.ReadBlock(base.DataSize - 4);
            this.handler_type = vector.Mid(0, 4);
            int count = vector.Find(0, 0x10);
            if (count < 0x10)
            {
                count = vector.Count;
            }
            this.name = vector.ToString(StringType.UTF8, 0x10, count - 0x10);
        }

        public override ByteVector Data
        {
            get
            {
                return new ByteVector(4) { this.handler_type, new ByteVector(12), ByteVector.FromString(this.name, 3), new ByteVector(2) };
            }
        }

        public ByteVector HandlerType
        {
            get
            {
                return this.handler_type;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}

