namespace TagLib.NonContainer
{
    using System;
    using TagLib;
    using TagLib.Ape;
    using TagLib.Id3v1;
    using TagLib.Id3v2;

    public class EndTag : CombinedTag
    {
        private TagLib.File file;
        private static int read_size = ((int) Math.Max(Math.Max((uint) 0x20, (uint) 10), 0x80));

        public EndTag(TagLib.File file)
        {
            this.file = file;
        }

        public TagLib.Tag AddTag(TagTypes type, TagLib.Tag copy)
        {
            TagLib.Tag target = null;
            if (type == (TagTypes.None | TagTypes.Id3v1))
            {
                target = new TagLib.Id3v1.Tag();
            }
            else if (type == (TagTypes.None | TagTypes.Id3v2))
            {
                TagLib.Id3v2.Tag tag2;
                target = new TagLib.Id3v2.Tag {
                    Version = 4,
                    Flags = (byte) (tag2.Flags | HeaderFlags.FooterPresent)
                };
            }
            else if (type == (TagTypes.None | TagTypes.Ape))
            {
                target = new TagLib.Ape.Tag();
            }
            if (target != null)
            {
                if (copy != null)
                {
                    copy.CopyTo(target, true);
                }
                if (type == (TagTypes.None | TagTypes.Id3v1))
                {
                    base.AddTag(target);
                    return target;
                }
                base.InsertTag(0, target);
            }
            return target;
        }

        public long Read()
        {
            TagLib.Tag tag;
            base.ClearTags();
            long length = this.file.Length;
            while ((tag = this.ReadTag(ref length)) != null)
            {
                base.InsertTag(0, tag);
            }
            return length;
        }

        private TagLib.Tag ReadTag(ref long end)
        {
            long position = end;
            TagTypes types = this.ReadTagInfo(ref position);
            TagLib.Tag tag = null;
            try
            {
                TagTypes types2 = types;
                switch (types2)
                {
                    case (TagTypes.None | TagTypes.Id3v1):
                        tag = new TagLib.Id3v1.Tag(this.file, position);
                        break;

                    case (TagTypes.None | TagTypes.Id3v2):
                        tag = new TagLib.Id3v2.Tag(this.file, position);
                        break;

                    default:
                        if (types2 == (TagTypes.None | TagTypes.Ape))
                        {
                            tag = new TagLib.Ape.Tag(this.file, end - 0x20L);
                        }
                        break;
                }
                end = position;
            }
            catch (CorruptFileException)
            {
            }
            return tag;
        }

        private TagTypes ReadTagInfo(ref long position)
        {
            if ((position - read_size) >= 0L)
            {
                this.file.Seek(position - read_size);
                ByteVector vector = this.file.ReadBlock(read_size);
                try
                {
                    int offset = vector.Count - ((int) 0x20L);
                    if (vector.ContainsAt(TagLib.Ape.Footer.FileIdentifier, offset))
                    {
                        TagLib.Ape.Footer footer = new TagLib.Ape.Footer(vector.Mid(offset));
                        if ((footer.CompleteTagSize == 0) || ((footer.Flags & FooterFlags.IsHeader) != ((FooterFlags) 0)))
                        {
                            return TagTypes.None;
                        }
                        position -= footer.CompleteTagSize;
                        return (TagTypes.None | TagTypes.Ape);
                    }
                    offset = vector.Count - ((int) 10L);
                    if (vector.ContainsAt(TagLib.Id3v2.Footer.FileIdentifier, offset))
                    {
                        TagLib.Id3v2.Footer footer2 = new TagLib.Id3v2.Footer(vector.Mid(offset));
                        position -= footer2.CompleteTagSize;
                        return (TagTypes.None | TagTypes.Id3v2);
                    }
                    if (vector.StartsWith(TagLib.Id3v1.Tag.FileIdentifier))
                    {
                        position -= 0x80L;
                        return (TagTypes.None | TagTypes.Id3v1);
                    }
                }
                catch (CorruptFileException)
                {
                }
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
                else if (tag is TagLib.Id3v1.Tag)
                {
                    vector.Add((tag as TagLib.Id3v1.Tag).Render());
                }
            }
            return vector;
        }

        public long Write()
        {
            long totalSize = this.TotalSize;
            ByteVector data = this.Render();
            this.file.Insert(data, this.file.Length - totalSize, totalSize);
            return (this.file.Length - data.Count);
        }

        public long TotalSize
        {
            get
            {
                long length = this.file.Length;
                while (this.ReadTagInfo(ref length) != TagTypes.None)
                {
                }
                return (this.file.Length - length);
            }
        }
    }
}

