namespace TagLib.NonContainer
{
    using System;
    using TagLib;
    using TagLib.Ape;
    using TagLib.Id3v2;

    public class StartTag : CombinedTag
    {
        private TagLib.File file;
        private int read_size = ((int) Math.Max((uint) 0x20, (uint) 10));

        public StartTag(TagLib.File file)
        {
            this.file = file;
        }

        public TagLib.Tag AddTag(TagTypes type, TagLib.Tag copy)
        {
            TagLib.Tag target = null;
            if (type == (TagTypes.None | TagTypes.Id3v2))
            {
                target = new TagLib.Id3v2.Tag();
            }
            else if (type == (TagTypes.None | TagTypes.Ape))
            {
                target = new TagLib.Ape.Tag();
                (target as TagLib.Ape.Tag).HeaderPresent = true;
            }
            if (target != null)
            {
                if (copy != null)
                {
                    copy.CopyTo(target, true);
                }
                base.AddTag(target);
            }
            return target;
        }

        public long Read()
        {
            TagLib.Tag tag;
            base.ClearTags();
            long start = 0L;
            while ((tag = this.ReadTag(ref start)) != null)
            {
                base.AddTag(tag);
            }
            return start;
        }

        private TagLib.Tag ReadTag(ref long start)
        {
            long position = start;
            TagTypes types = this.ReadTagInfo(ref position);
            TagLib.Tag tag = null;
            try
            {
                switch (types)
                {
                    case (TagTypes.None | TagTypes.Id3v2):
                        tag = new TagLib.Id3v2.Tag(this.file, start);
                        break;

                    case (TagTypes.None | TagTypes.Ape):
                        tag = new TagLib.Ape.Tag(this.file, start);
                        break;
                }
            }
            catch (CorruptFileException exception)
            {
                Console.Error.WriteLine("taglib-sharp caught exception creating tag: {0}", exception);
            }
            start = position;
            return tag;
        }

        private TagTypes ReadTagInfo(ref long position)
        {
            this.file.Seek(position);
            ByteVector data = this.file.ReadBlock(this.read_size);
            try
            {
                if (data.StartsWith(TagLib.Ape.Footer.FileIdentifier))
                {
                    TagLib.Ape.Footer footer = new TagLib.Ape.Footer(data);
                    position += footer.CompleteTagSize;
                    return (TagTypes.None | TagTypes.Ape);
                }
                if (data.StartsWith(Header.FileIdentifier))
                {
                    Header header = new Header(data);
                    position += header.CompleteTagSize;
                    return (TagTypes.None | TagTypes.Id3v2);
                }
            }
            catch (CorruptFileException)
            {
            }
            return TagTypes.None;
        }

        public void RemoveTags(TagTypes types)
        {
            for (int i = this.Tags.Length - 1; i >= 0; i--)
            {
                TagLib.Tag tag = this.Tags[i];
                if ((types == ~TagTypes.None) || ((tag.TagTypes & types) == tag.TagTypes))
                {
                    base.RemoveTag(tag);
                }
            }
        }

        public ByteVector Render()
        {
            ByteVector vector = new ByteVector();
            foreach (TagLib.Tag tag in this.Tags)
            {
                if (tag is TagLib.Ape.Tag)
                {
                    vector.Add((tag as TagLib.Ape.Tag).Render());
                }
                else if (tag is TagLib.Id3v2.Tag)
                {
                    vector.Add((tag as TagLib.Id3v2.Tag).Render());
                }
            }
            return vector;
        }

        public long Write()
        {
            ByteVector data = this.Render();
            this.file.Insert(data, 0L, this.TotalSize);
            return (long) data.Count;
        }

        public long TotalSize
        {
            get
            {
                long position = 0L;
                while (this.ReadTagInfo(ref position) != TagTypes.None)
                {
                }
                return position;
            }
        }
    }
}

