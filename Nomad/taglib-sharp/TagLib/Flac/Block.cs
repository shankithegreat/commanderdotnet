namespace TagLib.Flac
{
    using System;
    using TagLib;

    public class Block
    {
        private ByteVector data;
        private BlockHeader header;

        public Block(BlockHeader header, ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (header.BlockSize != data.Count)
            {
                throw new CorruptFileException("Data count not equal to block size.");
            }
            this.header = header;
            this.data = data;
        }

        public Block(BlockType type, ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.header = new BlockHeader(type, (uint) data.Count);
            this.data = data;
        }

        public ByteVector Render(bool isLastBlock)
        {
            if (this.data == null)
            {
                throw new InvalidOperationException("Cannot render empty blocks.");
            }
            ByteVector vector = this.header.Render(isLastBlock);
            vector.Add(this.data);
            return vector;
        }

        public ByteVector Data
        {
            get
            {
                return this.data;
            }
        }

        public uint DataSize
        {
            get
            {
                return this.header.BlockSize;
            }
        }

        public bool IsLastBlock
        {
            get
            {
                return this.header.IsLastBlock;
            }
        }

        public uint TotalSize
        {
            get
            {
                return (this.DataSize + 4);
            }
        }

        public BlockType Type
        {
            get
            {
                return this.header.BlockType;
            }
        }
    }
}

