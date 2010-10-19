namespace TagLib.Flac
{
    using System;
    using System.Collections.Generic;
    using TagLib;
    using TagLib.Ogg;

    public class Metadata : CombinedTag
    {
        private List<IPicture> pictures;

        public Metadata(IEnumerable<Block> blocks)
        {
            this.pictures = new List<IPicture>();
            if (blocks == null)
            {
                throw new ArgumentNullException("blocks");
            }
            IEnumerator<Block> enumerator = blocks.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Block current = enumerator.Current;
                    if (current.Data.Count != 0)
                    {
                        if (current.Type == BlockType.XiphComment)
                        {
                            base.AddTag(new XiphComment(current.Data));
                        }
                        else if (current.Type == BlockType.Picture)
                        {
                            this.pictures.Add(new TagLib.Flac.Picture(current.Data));
                        }
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
        }

        [Obsolete("Use Metadata(IEnumerable<Block>)")]
        public Metadata(List<Block> blocks) : this((IEnumerable<Block>) blocks)
        {
        }

        public override void Clear()
        {
            this.pictures.Clear();
        }

        public XiphComment GetComment(bool create, Tag copy)
        {
            foreach (Tag tag in this.Tags)
            {
                if (tag is XiphComment)
                {
                    return (tag as XiphComment);
                }
            }
            if (!create)
            {
                return null;
            }
            XiphComment target = new XiphComment();
            if (copy != null)
            {
                copy.CopyTo(target, true);
            }
            base.AddTag(target);
            return target;
        }

        public void RemoveComment()
        {
            XiphComment comment;
            while ((comment = this.GetComment(false, null)) != null)
            {
                base.RemoveTag(comment);
            }
        }

        public override IPicture[] Pictures
        {
            get
            {
                return this.pictures.ToArray();
            }
            set
            {
                this.pictures.Clear();
                if (value != null)
                {
                    this.pictures.AddRange(value);
                }
            }
        }

        public override TagLib.TagTypes TagTypes
        {
            get
            {
                return (TagLib.TagTypes.FlacMetadata | base.TagTypes);
            }
        }
    }
}

