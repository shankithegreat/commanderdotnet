namespace TagLib.Flac
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct BlockHeader
    {
        public const uint Size = 4;
        private TagLib.Flac.BlockType block_type;
        private bool is_last_block;
        private uint block_size;
        public BlockHeader(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Count < 4L)
            {
                throw new CorruptFileException("Not enough data in Flac header.");
            }
            this.block_type = ((TagLib.Flac.BlockType) data[0]) & ((TagLib.Flac.BlockType) 0x7f);
            this.is_last_block = (data[0] & 0x80) != 0;
            this.block_size = data.Mid(1, 3).ToUInt();
        }

        public BlockHeader(TagLib.Flac.BlockType type, uint blockSize)
        {
            this.block_type = type;
            this.is_last_block = false;
            this.block_size = blockSize;
        }

        public ByteVector Render(bool isLastBlock)
        {
            ByteVector vector = ByteVector.FromUInt(this.block_size);
            vector[0] = (byte) (this.block_type + (!isLastBlock ? 0 : 0x80));
            return vector;
        }

        public TagLib.Flac.BlockType BlockType
        {
            get
            {
                return this.block_type;
            }
        }
        public bool IsLastBlock
        {
            get
            {
                return this.is_last_block;
            }
        }
        public uint BlockSize
        {
            get
            {
                return this.block_size;
            }
        }
    }
}

