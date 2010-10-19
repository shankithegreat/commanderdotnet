namespace TagLib.Ogg
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using TagLib;

    [SupportedMimeType("taglib/ogg", "ogg"), SupportedMimeType("taglib/oga", "oga"), SupportedMimeType("taglib/ogv", "ogv"), SupportedMimeType("application/ogg"), SupportedMimeType("application/x-ogg"), SupportedMimeType("audio/vorbis"), SupportedMimeType("audio/x-vorbis"), SupportedMimeType("audio/x-vorbis+ogg"), SupportedMimeType("audio/ogg"), SupportedMimeType("audio/x-ogg"), SupportedMimeType("video/ogg"), SupportedMimeType("video/x-ogm+ogg"), SupportedMimeType("video/x-theora+ogg"), SupportedMimeType("video/x-theora")]
    public class File : TagLib.File
    {
        private TagLib.Properties properties;
        private GroupedComment tag;

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
            base.Mode = TagLib.File.AccessMode.Read;
            try
            {
                this.tag = new GroupedComment();
                this.Read(propertiesStyle);
                base.TagTypesOnDisk = base.TagTypes;
            }
            finally
            {
                base.Mode = TagLib.File.AccessMode.Closed;
            }
        }

        public override TagLib.Tag GetTag(TagTypes type, bool create)
        {
            if (type == TagTypes.Xiph)
            {
                IEnumerator<XiphComment> enumerator = this.tag.Comments.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        return enumerator.Current;
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
            }
            return null;
        }

        private void Read(ReadStyle propertiesStyle)
        {
            long num;
            Dictionary<uint, Bitstream> dictionary = this.ReadStreams(null, out num);
            List<ICodec> codecs = new List<ICodec>();
            base.InvariantStartPosition = num;
            base.InvariantEndPosition = base.Length;
            foreach (uint num2 in dictionary.Keys)
            {
                this.tag.AddComment(num2, dictionary[num2].Codec.CommentData);
                codecs.Add(dictionary[num2].Codec);
            }
            if (propertiesStyle != ReadStyle.None)
            {
                PageHeader lastPageHeader = this.LastPageHeader;
                TimeSpan duration = dictionary[lastPageHeader.StreamSerialNumber].GetDuration(lastPageHeader.AbsoluteGranularPosition);
                this.properties = new TagLib.Properties(duration, codecs);
            }
        }

        private Dictionary<uint, Bitstream> ReadStreams(List<Page> pages, out long end)
        {
            Dictionary<uint, Bitstream> dictionary = new Dictionary<uint, Bitstream>();
            List<Bitstream> list = new List<Bitstream>();
            long position = 0L;
            do
            {
                Bitstream bitstream = null;
                Page page = new Page(this, position);
                if (((byte) (page.Header.Flags & PageFlags.FirstPageOfStream)) != 0)
                {
                    bitstream = new Bitstream(page);
                    dictionary.Add(page.Header.StreamSerialNumber, bitstream);
                    list.Add(bitstream);
                }
                if (bitstream == null)
                {
                    bitstream = dictionary[page.Header.StreamSerialNumber];
                }
                if (list.Contains(bitstream) && bitstream.ReadPage(page))
                {
                    list.Remove(bitstream);
                }
                if (pages != null)
                {
                    pages.Add(page);
                }
                position += page.Size;
            }
            while (list.Count > 0);
            end = position;
            return dictionary;
        }

        public override void RemoveTags(TagTypes types)
        {
            if ((types & TagTypes.Xiph) != TagTypes.None)
            {
                this.tag.Clear();
            }
        }

        public override void Save()
        {
            base.Mode = TagLib.File.AccessMode.Write;
            try
            {
                long num;
                bool flag;
                List<Page> pages = new List<Page>();
                Dictionary<uint, Bitstream> dictionary = this.ReadStreams(pages, out num);
                Dictionary<uint, Paginator> dictionary2 = new Dictionary<uint, Paginator>();
                List<List<Page>> list2 = new List<List<Page>>();
                Dictionary<uint, int> shiftTable = new Dictionary<uint, int>();
                foreach (Page page in pages)
                {
                    uint streamSerialNumber = page.Header.StreamSerialNumber;
                    if (!dictionary2.ContainsKey(streamSerialNumber))
                    {
                        dictionary2.Add(streamSerialNumber, new Paginator(dictionary[streamSerialNumber].Codec));
                    }
                    dictionary2[streamSerialNumber].AddPage(page);
                }
                foreach (uint num3 in dictionary2.Keys)
                {
                    int num4;
                    dictionary2[num3].SetComment(this.tag.GetComment(num3));
                    list2.Add(new List<Page>(dictionary2[num3].Paginate(out num4)));
                    shiftTable.Add(num3, num4);
                }
                ByteVector data = new ByteVector();
                do
                {
                    flag = true;
                    foreach (List<Page> list3 in list2)
                    {
                        if (list3.Count != 0)
                        {
                            data.Add(list3[0].Render());
                            list3.RemoveAt(0);
                            if (list3.Count != 0)
                            {
                                flag = false;
                            }
                        }
                    }
                }
                while (!flag);
                base.Insert(data, 0L, num);
                base.InvariantStartPosition = data.Count;
                base.InvariantEndPosition = base.Length;
                base.TagTypesOnDisk = base.TagTypes;
                Page.OverwriteSequenceNumbers(this, (long) data.Count, shiftTable);
            }
            finally
            {
                base.Mode = TagLib.File.AccessMode.Closed;
            }
        }

        private PageHeader LastPageHeader
        {
            get
            {
                long position = base.RFind("OggS");
                if (position < 0L)
                {
                    throw new CorruptFileException("Could not find last header.");
                }
                return new PageHeader(this, position);
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

