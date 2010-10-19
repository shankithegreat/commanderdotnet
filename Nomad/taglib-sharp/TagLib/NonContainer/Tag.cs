namespace TagLib.NonContainer
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using TagLib;
    using TagLib.Ape;
    using TagLib.Id3v1;
    using TagLib.Id3v2;

    public class Tag : CombinedTag
    {
        private TagLib.NonContainer.EndTag end_tag;
        private TagLib.NonContainer.StartTag start_tag;

        public Tag(TagLib.NonContainer.File file)
        {
            this.start_tag = new TagLib.NonContainer.StartTag(file);
            this.end_tag = new TagLib.NonContainer.EndTag(file);
            base.AddTag(this.start_tag);
            base.AddTag(this.end_tag);
        }

        public TagLib.Tag GetTag(TagLib.TagTypes type)
        {
            foreach (TagLib.Tag tag in this.Tags)
            {
                if ((type == (TagLib.TagTypes.None | TagLib.TagTypes.Id3v1)) && (tag is TagLib.Id3v1.Tag))
                {
                    return tag;
                }
                if ((type == (TagLib.TagTypes.None | TagLib.TagTypes.Id3v2)) && (tag is TagLib.Id3v2.Tag))
                {
                    return tag;
                }
                if ((type == (TagLib.TagTypes.None | TagLib.TagTypes.Ape)) && (tag is TagLib.Ape.Tag))
                {
                    return tag;
                }
            }
            return null;
        }

        public void Read(out long start, out long end)
        {
            start = this.ReadStart();
            end = this.ReadEnd();
        }

        public long ReadEnd()
        {
            return this.end_tag.Read();
        }

        public long ReadStart()
        {
            return this.start_tag.Read();
        }

        public void RemoveTags(TagLib.TagTypes types)
        {
            this.start_tag.RemoveTags(types);
            this.end_tag.RemoveTags(types);
        }

        public void Write(out long start, out long end)
        {
            start = this.start_tag.Write();
            end = this.end_tag.Write();
        }

        public TagLib.NonContainer.EndTag EndTag
        {
            get
            {
                return this.end_tag;
            }
        }

        public TagLib.NonContainer.StartTag StartTag
        {
            get
            {
                return this.start_tag;
            }
        }

        public override TagLib.Tag[] Tags
        {
            get
            {
                List<TagLib.Tag> list = new List<TagLib.Tag>();
                list.AddRange(this.start_tag.Tags);
                list.AddRange(this.end_tag.Tags);
                return list.ToArray();
            }
        }

        public override TagLib.TagTypes TagTypes
        {
            get
            {
                return (this.start_tag.TagTypes | this.end_tag.TagTypes);
            }
        }
    }
}

