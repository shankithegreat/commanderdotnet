namespace TagLib.Riff
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using TagLib;
    using TagLib.Id3v2;

    [SupportedMimeType("taglib/avi", "avi"), SupportedMimeType("taglib/wav", "wav"), SupportedMimeType("taglib/divx", "divx"), SupportedMimeType("video/avi"), SupportedMimeType("video/msvideo"), SupportedMimeType("video/x-msvideo"), SupportedMimeType("image/avi"), SupportedMimeType("application/x-troff-msvideo"), SupportedMimeType("audio/avi"), SupportedMimeType("audio/wav"), SupportedMimeType("audio/wave"), SupportedMimeType("audio/x-wav")]
    public class File : TagLib.File
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map2;
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map3;
        private DivXTag divx_tag;
        public static readonly ReadOnlyByteVector FileIdentifier = "RIFF";
        private TagLib.Id3v2.Tag id32_tag;
        private InfoTag info_tag;
        private MovieIdTag mid_tag;
        private TagLib.Properties properties;
        private CombinedTag tag;

        public File(string path) : this(path, ReadStyle.Average)
        {
        }

        public File(TagLib.File.IFileAbstraction abstraction) : this(abstraction, ReadStyle.Average)
        {
        }

        public File(string path, ReadStyle propertiesStyle) : this(new TagLib.File.LocalFileAbstraction(path), propertiesStyle)
        {
        }

        public File(TagLib.File.IFileAbstraction abstraction, ReadStyle propertiesStyle) : base(abstraction)
        {
            this.tag = new CombinedTag();
            base.Mode = TagLib.File.AccessMode.Read;
            try
            {
                uint num;
                long num2;
                long num3;
                this.Read(true, propertiesStyle, out num, out num2, out num3);
            }
            finally
            {
                base.Mode = TagLib.File.AccessMode.Closed;
            }
            base.TagTypesOnDisk = base.TagTypes;
            this.GetTag(TagTypes.None | TagTypes.Id3v2, true);
            this.GetTag(TagTypes.RiffInfo, true);
            this.GetTag(TagTypes.MovieId, true);
            this.GetTag(TagTypes.DivX, true);
        }

        public override TagLib.Tag GetTag(TagTypes type, bool create)
        {
            TagLib.Tag tag = null;
            switch (type)
            {
                case (TagTypes.None | TagTypes.Id3v2):
                    if ((this.id32_tag == null) && create)
                    {
                        this.id32_tag = new TagLib.Id3v2.Tag();
                        this.id32_tag.Version = 4;
                        this.id32_tag.Flags = (HeaderFlags) ((byte) (this.id32_tag.Flags | HeaderFlags.FooterPresent));
                        this.tag.CopyTo(this.id32_tag, true);
                    }
                    tag = this.id32_tag;
                    break;

                case TagTypes.RiffInfo:
                    if ((this.info_tag == null) && create)
                    {
                        this.info_tag = new InfoTag();
                        this.tag.CopyTo(this.info_tag, true);
                    }
                    tag = this.info_tag;
                    break;

                case TagTypes.MovieId:
                    if ((this.mid_tag == null) && create)
                    {
                        this.mid_tag = new MovieIdTag();
                        this.tag.CopyTo(this.mid_tag, true);
                    }
                    tag = this.mid_tag;
                    break;

                case TagTypes.DivX:
                    if ((this.divx_tag == null) && create)
                    {
                        this.divx_tag = new DivXTag();
                        this.tag.CopyTo(this.divx_tag, true);
                    }
                    tag = this.divx_tag;
                    break;
            }
            TagLib.Tag[] tags = new TagLib.Tag[] { this.id32_tag, this.info_tag, this.mid_tag, this.divx_tag };
            this.tag.SetTags(tags);
            return tag;
        }

        private void Read(bool read_tags, ReadStyle style, out uint riff_size, out long tag_start, out long tag_end)
        {
            bool flag;
            base.Seek(0L);
            if (base.ReadBlock(4) != FileIdentifier)
            {
                throw new CorruptFileException("File does not begin with RIFF identifier");
            }
            riff_size = base.ReadBlock(4).ToUInt(false);
            ByteVector vector = base.ReadBlock(4);
            tag_start = -1L;
            tag_end = -1L;
            long offset = 12L;
            long length = base.Length;
            uint num3 = 0;
            TimeSpan zero = TimeSpan.Zero;
            ICodec[] codecs = new ICodec[0];
        Label_0066:
            flag = false;
            base.Seek(offset);
            string str = base.ReadBlock(4).ToString(StringType.UTF8);
            num3 = base.ReadBlock(4).ToUInt(false);
            string key = str;
            if (key != null)
            {
                Dictionary<string, int> dictionary;
                int num4;
                if (<>f__switch$map3 == null)
                {
                    dictionary = new Dictionary<string, int>(6);
                    dictionary.Add("fmt ", 0);
                    dictionary.Add("data", 1);
                    dictionary.Add("LIST", 2);
                    dictionary.Add("ID32", 3);
                    dictionary.Add("IDVX", 4);
                    dictionary.Add("JUNK", 5);
                    <>f__switch$map3 = dictionary;
                }
                if (<>f__switch$map3.TryGetValue(key, out num4))
                {
                    switch (num4)
                    {
                        case 0:
                            if ((style != ReadStyle.None) && (vector == "WAVE"))
                            {
                                base.Seek(offset + 8L);
                                codecs = new ICodec[] { new WaveFormatEx(base.ReadBlock(0x12), 0) };
                                break;
                            }
                            break;

                        case 1:
                            if (vector == "WAVE")
                            {
                                base.InvariantStartPosition = offset;
                                base.InvariantEndPosition = offset + num3;
                                if (((style != ReadStyle.None) && (codecs.Length == 1)) && (codecs[0] is WaveFormatEx))
                                {
                                    WaveFormatEx ex = (WaveFormatEx) codecs[0];
                                    zero += TimeSpan.FromSeconds(((double) num3) / ((double) ex.AverageBytesPerSecond));
                                }
                                break;
                            }
                            break;

                        case 2:
                        {
                            string str3 = base.ReadBlock(4).ToString(StringType.UTF8);
                            if (str3 != null)
                            {
                                int num5;
                                if (<>f__switch$map2 == null)
                                {
                                    dictionary = new Dictionary<string, int>(4);
                                    dictionary.Add("hdrl", 0);
                                    dictionary.Add("INFO", 1);
                                    dictionary.Add("MID ", 2);
                                    dictionary.Add("movi", 3);
                                    <>f__switch$map2 = dictionary;
                                }
                                if (<>f__switch$map2.TryGetValue(str3, out num5))
                                {
                                    switch (num5)
                                    {
                                        case 0:
                                            if ((style != ReadStyle.None) && (vector == "AVI "))
                                            {
                                                AviHeaderList list = new AviHeaderList(this, offset + 12L, ((int) num3) - 4);
                                                zero = list.Header.Duration;
                                                codecs = list.Codecs;
                                                break;
                                            }
                                            goto Label_040E;

                                        case 1:
                                            if (read_tags && (this.info_tag == null))
                                            {
                                                this.info_tag = new InfoTag(this, offset + 12L, ((int) num3) - 4);
                                            }
                                            flag = true;
                                            break;

                                        case 2:
                                            if (read_tags && (this.mid_tag == null))
                                            {
                                                this.mid_tag = new MovieIdTag(this, offset + 12L, ((int) num3) - 4);
                                            }
                                            flag = true;
                                            break;

                                        case 3:
                                            if (vector == "AVI ")
                                            {
                                                base.InvariantStartPosition = offset;
                                                base.InvariantEndPosition = offset + num3;
                                                break;
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        }
                        case 3:
                            if (read_tags && (this.id32_tag == null))
                            {
                                this.id32_tag = new TagLib.Id3v2.Tag(this, offset + 8L);
                            }
                            flag = true;
                            break;

                        case 4:
                            if (read_tags && (this.divx_tag == null))
                            {
                                this.divx_tag = new DivXTag(this, offset + 8L);
                            }
                            flag = true;
                            break;

                        case 5:
                            if (tag_end == offset)
                            {
                                tag_end = (offset + 8L) + num3;
                            }
                            break;
                    }
                }
            }
            if (flag)
            {
                if (tag_start == -1L)
                {
                    tag_start = offset;
                    tag_end = (offset + 8L) + num3;
                }
                else if (tag_end == offset)
                {
                    tag_end = (offset + 8L) + num3;
                }
            }
        Label_040E:
            if (((offset += (8 + num3)) + 8L) < length)
            {
                goto Label_0066;
            }
            if (style != ReadStyle.None)
            {
                if (codecs.Length == 0)
                {
                    throw new UnsupportedFormatException("Unsupported RIFF type.");
                }
                this.properties = new TagLib.Properties(zero, codecs);
            }
            if (read_tags)
            {
                TagLib.Tag[] tags = new TagLib.Tag[] { this.id32_tag, this.info_tag, this.mid_tag, this.divx_tag };
                this.tag.SetTags(tags);
            }
        }

        public override void RemoveTags(TagTypes types)
        {
            if ((types & (TagTypes.None | TagTypes.Id3v2)) != TagTypes.None)
            {
                this.id32_tag = null;
            }
            if ((types & TagTypes.RiffInfo) != TagTypes.None)
            {
                this.info_tag = null;
            }
            if ((types & TagTypes.MovieId) != TagTypes.None)
            {
                this.mid_tag = null;
            }
            if ((types & TagTypes.DivX) != TagTypes.None)
            {
                this.divx_tag = null;
            }
            TagLib.Tag[] tags = new TagLib.Tag[] { this.id32_tag, this.info_tag, this.mid_tag, this.divx_tag };
            this.tag.SetTags(tags);
        }

        public override void Save()
        {
            base.Mode = TagLib.File.AccessMode.Write;
            try
            {
                uint num;
                long num2;
                long num3;
                ByteVector data = new ByteVector();
                if (this.id32_tag != null)
                {
                    ByteVector vector2 = this.id32_tag.Render();
                    if (vector2.Count > 10)
                    {
                        if ((vector2.Count % 2) == 1)
                        {
                            vector2.Add((byte) 0);
                        }
                        data.Add("ID32");
                        data.Add(ByteVector.FromUInt((uint) vector2.Count, false));
                        data.Add(vector2);
                    }
                }
                if (this.info_tag != null)
                {
                    data.Add(this.info_tag.RenderEnclosed());
                }
                if (this.mid_tag != null)
                {
                    data.Add(this.mid_tag.RenderEnclosed());
                }
                if ((this.divx_tag != null) && !this.divx_tag.IsEmpty)
                {
                    ByteVector vector3 = this.divx_tag.Render();
                    data.Add("IDVX");
                    data.Add(ByteVector.FromUInt((uint) vector3.Count, false));
                    data.Add(vector3);
                }
                this.Read(false, ReadStyle.None, out num, out num2, out num3);
                if ((num2 < 12L) || (num3 < num2))
                {
                    num2 = num3 = base.Length;
                }
                int num4 = (int) (num3 - num2);
                if (num3 != base.Length)
                {
                    int size = (num4 - data.Count) - 8;
                    if (size < 0)
                    {
                        size = 0x400;
                    }
                    data.Add("JUNK");
                    data.Add(ByteVector.FromUInt((uint) size, false));
                    data.Add(new ByteVector(size));
                }
                base.Insert(data, num2, (long) num4);
                if (((data.Count - num4) != 0) && (num2 <= num))
                {
                    base.Insert(ByteVector.FromUInt((uint) ((num + data.Count) - num4), false), 4L, 4L);
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

