namespace TagLib.Asf
{
    using System;
    using TagLib;

    public abstract class Object
    {
        private System.Guid id;
        private ulong size;

        protected Object(System.Guid guid)
        {
            this.id = guid;
        }

        protected Object(TagLib.Asf.File file, long position)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if ((position < 0L) || (position > (file.Length - 0x18L)))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            file.Seek(position);
            this.id = file.ReadGuid();
            this.size = file.ReadQWord();
        }

        public abstract ByteVector Render();
        protected ByteVector Render(ByteVector data)
        {
            ulong num = (ulong) (((data == null) ? 0 : data.Count) + 0x18);
            ByteVector vector = this.id.ToByteArray();
            vector.Add(RenderQWord(num));
            vector.Add(data);
            return vector;
        }

        public static ByteVector RenderDWord(uint value)
        {
            return ByteVector.FromUInt(value, false);
        }

        public static ByteVector RenderQWord(ulong value)
        {
            return ByteVector.FromULong(value, false);
        }

        public static ByteVector RenderUnicode(string value)
        {
            ByteVector vector = ByteVector.FromString(value, StringType.UTF16LE);
            vector.Add(RenderWord(0));
            return vector;
        }

        public static ByteVector RenderWord(ushort value)
        {
            return ByteVector.FromUShort(value, false);
        }

        public System.Guid Guid
        {
            get
            {
                return this.id;
            }
        }

        public ulong OriginalSize
        {
            get
            {
                return this.size;
            }
        }
    }
}

