namespace TagLib.Mpeg4
{
    using System;
    using TagLib;

    public class IsoChunkOffsetBox : FullBox
    {
        private uint[] offsets;

        public IsoChunkOffsetBox(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, file, handler)
        {
            ByteVector vector = file.ReadBlock(base.DataSize);
            this.offsets = new uint[vector.Mid(0, 4).ToUInt()];
            for (int i = 0; i < this.offsets.Length; i++)
            {
                this.offsets[i] = vector.Mid(4 + (i * 4), 4).ToUInt();
            }
        }

        public void Overwrite(TagLib.Mpeg4.File file, long sizeDifference, long after)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            file.Insert(this.Render(sizeDifference, after), base.Header.Position, (long) this.Size);
        }

        public ByteVector Render(long sizeDifference, long after)
        {
            for (int i = 0; i < this.offsets.Length; i++)
            {
                if (this.offsets[i] >= ((uint) after))
                {
                    this.offsets[i] += (uint) sizeDifference;
                }
            }
            return base.Render();
        }

        public override ByteVector Data
        {
            get
            {
                ByteVector vector = ByteVector.FromUInt((uint) this.offsets.Length);
                for (int i = 0; i < this.offsets.Length; i++)
                {
                    vector.Add(ByteVector.FromUInt(this.offsets[i]));
                }
                return vector;
            }
        }

        public uint[] Offsets
        {
            get
            {
                return this.offsets;
            }
        }
    }
}

