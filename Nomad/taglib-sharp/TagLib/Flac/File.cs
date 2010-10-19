namespace TagLib.Flac
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using TagLib;
    using TagLib.NonContainer;
    using TagLib.Ogg;

    [SupportedMimeType("taglib/flac", "flac"), SupportedMimeType("audio/x-flac"), SupportedMimeType("application/x-flac"), SupportedMimeType("audio/flac")]
    public class File : TagLib.NonContainer.File
    {
        private ByteVector header_block;
        private Metadata metadata;
        private long stream_start;
        private CombinedTag tag;

        public File(string path) : base(path)
        {
        }

        public File(TagLib.File.IFileAbstraction abstraction) : base(abstraction)
        {
        }

        public File(string path, ReadStyle propertiesStyle) : base(path, propertiesStyle)
        {
        }

        public File(TagLib.File.IFileAbstraction abstraction, ReadStyle propertiesStyle) : base(abstraction, propertiesStyle)
        {
        }

        public override TagLib.Tag GetTag(TagTypes type, bool create)
        {
            TagTypes types;
            switch (type)
            {
                case TagTypes.Xiph:
                    return this.metadata.GetComment(create, this.tag);

                case TagTypes.FlacMetadata:
                    return this.metadata;

                default:
                {
                    TagLib.Tag tag = (base.Tag as TagLib.NonContainer.Tag).GetTag(type);
                    if ((tag != null) || !create)
                    {
                        return tag;
                    }
                    types = type;
                    switch (types)
                    {
                        case (TagTypes.None | TagTypes.Id3v1):
                            return base.EndTag.AddTag(type, this.Tag);

                        case (TagTypes.None | TagTypes.Id3v2):
                            return base.StartTag.AddTag(type, this.Tag);
                    }
                    break;
                }
            }
            if (types == (TagTypes.None | TagTypes.Ape))
            {
                return base.EndTag.AddTag(type, this.Tag);
            }
            return null;
        }

        private IList<Block> ReadBlocks(ref long start, out long end, BlockMode mode, params BlockType[] types)
        {
            BlockHeader header;
            List<Block> list = new List<Block>();
            long num = base.Find("fLaC", start);
            if (num < 0L)
            {
                throw new CorruptFileException("FLAC stream not found at starting position.");
            }
            end = start = num + 4L;
            base.Seek(start);
            do
            {
                header = new BlockHeader(base.ReadBlock(4));
                bool flag = false;
                foreach (BlockType type in types)
                {
                    if (header.BlockType == type)
                    {
                        flag = true;
                        break;
                    }
                }
                if (((mode == BlockMode.Whitelist) && flag) || ((mode == BlockMode.Blacklist) && !flag))
                {
                    list.Add(new Block(header, base.ReadBlock((int) header.BlockSize)));
                }
                else
                {
                    base.Seek((long) header.BlockSize, SeekOrigin.Current);
                }
                end += header.BlockSize + 4;
            }
            while (!header.IsLastBlock);
            return list;
        }

        protected override void ReadEnd(long end, ReadStyle propertiesStyle)
        {
            TagLib.Tag[] tags = new TagLib.Tag[] { this.metadata, base.Tag };
            this.tag = new CombinedTag(tags);
            this.GetTag(TagTypes.Xiph, true);
        }

        protected override Properties ReadProperties(long start, long end, ReadStyle propertiesStyle)
        {
            StreamHeader header = new StreamHeader(this.header_block, end - this.stream_start);
            return new Properties(TimeSpan.Zero, new ICodec[] { header });
        }

        protected override void ReadStart(long start, ReadStyle propertiesStyle)
        {
            long num;
            BlockType[] types = new BlockType[3];
            types[1] = BlockType.XiphComment;
            types[2] = BlockType.Picture;
            IList<Block> blocks = this.ReadBlocks(ref start, out num, BlockMode.Whitelist, types);
            this.metadata = new Metadata(blocks);
            base.TagTypesOnDisk |= this.metadata.TagTypes;
            if (propertiesStyle != ReadStyle.None)
            {
                if ((blocks.Count == 0) || (blocks[0].Type != BlockType.StreamInfo))
                {
                    throw new CorruptFileException("FLAC stream does not begin with StreamInfo.");
                }
                this.stream_start = num;
                this.header_block = blocks[0].Data;
            }
        }

        public override void RemoveTags(TagTypes types)
        {
            if ((types & TagTypes.Xiph) != TagTypes.None)
            {
                this.metadata.RemoveComment();
            }
            if ((types & TagTypes.FlacMetadata) != TagTypes.None)
            {
                this.metadata.Clear();
            }
            base.RemoveTags(types);
        }

        public override void Save()
        {
            base.Mode = TagLib.File.AccessMode.Write;
            try
            {
                long num2;
                long start = base.StartTag.Write();
                BlockType[] types = new BlockType[] { BlockType.XiphComment, BlockType.Picture };
                IList<Block> list = this.ReadBlocks(ref start, out num2, BlockMode.Blacklist, types);
                this.GetTag(TagTypes.Xiph, true);
                List<Block> list2 = new List<Block> {
                    list[0]
                };
                IEnumerator<Block> enumerator = list.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        Block current = enumerator.Current;
                        if (((current.Type != BlockType.StreamInfo) && (current.Type != BlockType.XiphComment)) && ((current.Type != BlockType.Picture) && (current.Type != BlockType.Padding)))
                        {
                            list2.Add(current);
                        }
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                list2.Add(new Block(BlockType.XiphComment, (this.GetTag(TagTypes.Xiph, true) as XiphComment).Render(false)));
                foreach (IPicture picture in this.metadata.Pictures)
                {
                    if (picture != null)
                    {
                        list2.Add(new Block(BlockType.Picture, new TagLib.Flac.Picture(picture).Render()));
                    }
                }
                long num4 = 0L;
                foreach (Block block2 in list2)
                {
                    num4 += block2.TotalSize;
                }
                long num5 = ((num2 - start) - 4L) - num4;
                if (num5 < 0L)
                {
                    num5 = 0x1000L;
                }
                if (num5 != 0)
                {
                    list2.Add(new Block(BlockType.Padding, new ByteVector((int) num5)));
                }
                ByteVector data = new ByteVector();
                for (int i = 0; i < list2.Count; i++)
                {
                    data.Add(list2[i].Render(i == (list2.Count - 1)));
                }
                base.Insert(data, start, num2 - start);
                base.EndTag.Write();
                base.TagTypesOnDisk = base.TagTypes;
            }
            finally
            {
                base.Mode = TagLib.File.AccessMode.Closed;
            }
        }

        public override TagLib.Tag Tag
        {
            get
            {
                return this.tag;
            }
        }

        private enum BlockMode
        {
            Blacklist,
            Whitelist
        }
    }
}

