namespace TagLib.Mpeg4
{
    using System;
    using TagLib;

    [SupportedMimeType("taglib/m4a", "m4a"), SupportedMimeType("taglib/m4b", "m4b"), SupportedMimeType("taglib/m4v", "m4v"), SupportedMimeType("taglib/m4p", "m4p"), SupportedMimeType("taglib/mp4", "mp4"), SupportedMimeType("audio/mp4"), SupportedMimeType("audio/x-m4a"), SupportedMimeType("video/mp4"), SupportedMimeType("video/x-m4v")]
    public class File : TagLib.File
    {
        private AppleTag apple_tag;
        private TagLib.Properties properties;
        private CombinedTag tag;
        private IsoUserDataBox udta_box;

        public File(string path) : this(path, ReadStyle.Average)
        {
        }

        public File(TagLib.File.IFileAbstraction abstraction) : this(abstraction, ReadStyle.Average)
        {
        }

        public File(string path, ReadStyle propertiesStyle) : base(path)
        {
            this.Read(propertiesStyle);
        }

        public File(TagLib.File.IFileAbstraction abstraction, ReadStyle propertiesStyle) : base(abstraction)
        {
            this.Read(propertiesStyle);
        }

        public override TagLib.Tag GetTag(TagTypes type, bool create)
        {
            if (type != TagTypes.Apple)
            {
                return null;
            }
            if ((this.apple_tag == null) && create)
            {
                this.apple_tag = new AppleTag(this.udta_box);
                TagLib.Tag[] tags = new TagLib.Tag[] { this.apple_tag };
                this.tag.SetTags(tags);
            }
            return this.apple_tag;
        }

        private void Read(ReadStyle propertiesStyle)
        {
            this.tag = new CombinedTag();
            base.Mode = TagLib.File.AccessMode.Read;
            try
            {
                FileParser parser = new FileParser(this);
                if (propertiesStyle == ReadStyle.None)
                {
                    parser.ParseTag();
                }
                else
                {
                    parser.ParseTagAndProperties();
                }
                base.InvariantStartPosition = parser.MdatStartPosition;
                base.InvariantEndPosition = parser.MdatEndPosition;
                this.udta_box = parser.UserDataBox;
                if (((this.udta_box != null) && (this.udta_box.GetChild(BoxType.Meta) != null)) && (this.udta_box.GetChild(BoxType.Meta).GetChild(BoxType.Ilst) != null))
                {
                    base.TagTypesOnDisk |= TagTypes.Apple;
                }
                if (this.udta_box == null)
                {
                    this.udta_box = new IsoUserDataBox();
                }
                this.apple_tag = new AppleTag(this.udta_box);
                TagLib.Tag[] tags = new TagLib.Tag[] { this.apple_tag };
                this.tag.SetTags(tags);
                if (propertiesStyle == ReadStyle.None)
                {
                    base.Mode = TagLib.File.AccessMode.Closed;
                }
                else
                {
                    IsoMovieHeaderBox movieHeaderBox = parser.MovieHeaderBox;
                    if (movieHeaderBox == null)
                    {
                        base.Mode = TagLib.File.AccessMode.Closed;
                        throw new CorruptFileException("mvhd box not found.");
                    }
                    IsoAudioSampleEntry audioSampleEntry = parser.AudioSampleEntry;
                    IsoVisualSampleEntry visualSampleEntry = parser.VisualSampleEntry;
                    ICodec[] codecs = new ICodec[] { audioSampleEntry, visualSampleEntry };
                    this.properties = new TagLib.Properties(movieHeaderBox.Duration, codecs);
                }
            }
            finally
            {
                base.Mode = TagLib.File.AccessMode.Closed;
            }
        }

        public override void RemoveTags(TagTypes types)
        {
            if (((types & TagTypes.Apple) == TagTypes.Apple) && (this.apple_tag != null))
            {
                this.apple_tag.DetachIlst();
                this.apple_tag = null;
                this.tag.SetTags(new TagLib.Tag[0]);
            }
        }

        public override void Save()
        {
            if (this.udta_box == null)
            {
                this.udta_box = new IsoUserDataBox();
            }
            base.Mode = TagLib.File.AccessMode.Write;
            try
            {
                FileParser parser = new FileParser(this);
                parser.ParseBoxHeaders();
                base.InvariantStartPosition = parser.MdatStartPosition;
                base.InvariantEndPosition = parser.MdatEndPosition;
                long sizeChange = 0L;
                long start = 0L;
                ByteVector data = this.udta_box.Render();
                if (((parser.UdtaTree == null) || (parser.UdtaTree.Length == 0)) || (parser.UdtaTree[parser.UdtaTree.Length - 1].BoxType != BoxType.Udta))
                {
                    BoxHeader header = parser.MoovTree[parser.MoovTree.Length - 1];
                    sizeChange = data.Count;
                    start = header.Position + header.TotalBoxSize;
                    base.Insert(data, start, 0L);
                    for (int i = parser.MoovTree.Length - 1; i >= 0; i--)
                    {
                        sizeChange = parser.MoovTree[i].Overwrite(this, sizeChange);
                    }
                }
                else
                {
                    BoxHeader header2 = parser.UdtaTree[parser.UdtaTree.Length - 1];
                    sizeChange = data.Count - header2.TotalBoxSize;
                    start = header2.Position;
                    base.Insert(data, start, header2.TotalBoxSize);
                    for (int j = parser.UdtaTree.Length - 2; j >= 0; j--)
                    {
                        sizeChange = parser.UdtaTree[j].Overwrite(this, sizeChange);
                    }
                }
                if (sizeChange != 0)
                {
                    parser.ParseChunkOffsets();
                    base.InvariantStartPosition = parser.MdatStartPosition;
                    base.InvariantEndPosition = parser.MdatEndPosition;
                    foreach (Box box in parser.ChunkOffsetBoxes)
                    {
                        IsoChunkLargeOffsetBox box2 = box as IsoChunkLargeOffsetBox;
                        if (box2 != null)
                        {
                            box2.Overwrite(this, sizeChange, start);
                        }
                        else
                        {
                            IsoChunkOffsetBox box3 = box as IsoChunkOffsetBox;
                            if (box3 != null)
                            {
                                box3.Overwrite(this, sizeChange, start);
                            }
                        }
                    }
                }
                base.TagTypesOnDisk = base.TagTypes;
            }
            finally
            {
                base.Mode = TagLib.File.AccessMode.Closed;
            }
        }

        public override TagLib.Properties Properties
        {
            get
            {
                return this.properties;
            }
        }

        public override TagLib.Tag Tag
        {
            get
            {
                return this.tag;
            }
        }
    }
}

