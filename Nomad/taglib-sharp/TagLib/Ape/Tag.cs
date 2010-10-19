namespace TagLib.Ape
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using TagLib;

    public class Tag : TagLib.Tag, IEnumerable, IEnumerable<string>
    {
        [Obsolete("Use Footer.FileIdentifer")]
        public static readonly ReadOnlyByteVector FileIdentifier = Footer.FileIdentifier;
        private Footer footer;
        private List<Item> items;
        private static string[] picture_item_names = new string[] { 
            "Cover Art (other)", "Cover Art (icon)", "Cover Art (other icon)", "Cover Art (front)", "Cover Art (back)", "Cover Art (leaflet)", "Cover Art (media)", "Cover Art (lead)", "Cover Art (artist)", "Cover Art (conductor)", "Cover Art (band)", "Cover Art (composer)", "Cover Art (lyricist)", "Cover Art (studio)", "Cover Art (recording)", "Cover Art (performance)", 
            "Cover Art (movie scene)", "Cover Art (colored fish)", "Cover Art (illustration)", "Cover Art (band logo)", "Cover Art (publisher logo)"
         };

        public Tag()
        {
            this.footer = new Footer();
            this.items = new List<Item>();
        }

        public Tag(ByteVector data)
        {
            this.footer = new Footer();
            this.items = new List<Item>();
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Count < 0x20L)
            {
                throw new CorruptFileException("Does not contain enough footer data.");
            }
            this.footer = new Footer(data.Mid(data.Count - ((int) 0x20L)));
            if (this.footer.TagSize == 0)
            {
                throw new CorruptFileException("Tag size out of bounds.");
            }
            if ((this.footer.Flags & FooterFlags.IsHeader) != ((FooterFlags) 0))
            {
                throw new CorruptFileException("Footer was actually header.");
            }
            if (data.Count < this.footer.TagSize)
            {
                throw new CorruptFileException("Does not contain enough tag data.");
            }
            this.Parse(data.Mid(data.Count - ((int) this.footer.TagSize), ((int) this.footer.TagSize) - 0x20));
        }

        public Tag(TagLib.File file, long position)
        {
            this.footer = new Footer();
            this.items = new List<Item>();
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if ((position < 0L) || (position > (file.Length - 0x20L)))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            this.Read(file, position);
        }

        public void AddValue(string key, string[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if ((value != null) && (value.Length != 0))
            {
                int itemIndex = this.GetItemIndex(key);
                List<string> list = new List<string>();
                if (itemIndex >= 0)
                {
                    list.AddRange(this.items[itemIndex].ToStringArray());
                }
                list.AddRange(value);
                Item item = new Item(key, list.ToArray());
                if (itemIndex >= 0)
                {
                    this.items[itemIndex] = item;
                }
                else
                {
                    this.items.Add(item);
                }
            }
        }

        public void AddValue(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (!string.IsNullOrEmpty(value))
            {
                string[] textArray1 = new string[] { value };
                this.AddValue(key, textArray1);
            }
        }

        public void AddValue(string key, uint number, uint count)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if ((number != 0) || (count != 0))
            {
                if (count != 0)
                {
                    object[] args = new object[] { number, count };
                    this.AddValue(key, string.Format(CultureInfo.InvariantCulture, "{0}/{1}", args));
                }
                else
                {
                    this.AddValue(key, number.ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        public override void Clear()
        {
            this.items.Clear();
        }

        public override void CopyTo(TagLib.Tag target, bool overwrite)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            TagLib.Ape.Tag tag = target as TagLib.Ape.Tag;
            if (tag == null)
            {
                base.CopyTo(target, overwrite);
            }
            else
            {
                foreach (Item item in this.items)
                {
                    if (overwrite || (tag.GetItem(item.Key) == null))
                    {
                        tag.items.Add(item.Clone());
                    }
                }
            }
        }

        [DebuggerHidden]
        public IEnumerator<string> GetEnumerator()
        {
            return new <GetEnumerator>c__Iterator0 { <>f__this = this };
        }

        public Item GetItem(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            StringComparison invariantCultureIgnoreCase = StringComparison.InvariantCultureIgnoreCase;
            foreach (Item item in this.items)
            {
                if (key.Equals(item.Key, invariantCultureIgnoreCase))
                {
                    return item;
                }
            }
            return null;
        }

        private string GetItemAsString(string key)
        {
            Item item = this.GetItem(key);
            return ((item == null) ? null : item.ToString());
        }

        private string[] GetItemAsStrings(string key)
        {
            Item item = this.GetItem(key);
            return ((item == null) ? new string[0] : item.ToStringArray());
        }

        private uint GetItemAsUInt32(string key, int index)
        {
            string itemAsString = this.GetItemAsString(key);
            if (itemAsString != null)
            {
                uint num;
                char[] separator = new char[] { '/' };
                string[] strArray = itemAsString.Split(separator, (int) (index + 2));
                if (strArray.Length < (index + 1))
                {
                    return 0;
                }
                if (uint.TryParse(strArray[index], out num))
                {
                    return num;
                }
            }
            return 0;
        }

        private int GetItemIndex(string key)
        {
            StringComparison invariantCultureIgnoreCase = StringComparison.InvariantCultureIgnoreCase;
            for (int i = 0; i < this.items.Count; i++)
            {
                if (key.Equals(this.items[i].Key, invariantCultureIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool HasItem(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            return (this.GetItemIndex(key) >= 0);
        }

        protected void Parse(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            int offset = 0;
            try
            {
                for (uint i = 0; (i < this.footer.ItemCount) && (offset <= (data.Count - 11)); i++)
                {
                    Item item = new Item(data, offset);
                    this.SetItem(item);
                    offset += item.Size;
                }
            }
            catch (CorruptFileException)
            {
            }
        }

        protected void Read(TagLib.File file, long position)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            file.Mode = TagLib.File.AccessMode.Read;
            if ((position < 0L) || (position > (file.Length - 0x20L)))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            file.Seek(position);
            this.footer = new Footer(file.ReadBlock(0x20));
            if (this.footer.TagSize == 0)
            {
                throw new CorruptFileException("Tag size out of bounds.");
            }
            if ((this.footer.Flags & FooterFlags.IsHeader) == ((FooterFlags) 0))
            {
                file.Seek((position + 0x20L) - this.footer.TagSize);
            }
            this.Parse(file.ReadBlock(((int) this.footer.TagSize) - 0x20));
        }

        public void RemoveItem(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            StringComparison invariantCultureIgnoreCase = StringComparison.InvariantCultureIgnoreCase;
            for (int i = this.items.Count - 1; i >= 0; i--)
            {
                if (key.Equals(this.items[i].Key, invariantCultureIgnoreCase))
                {
                    this.items.RemoveAt(i);
                }
            }
        }

        public ByteVector Render()
        {
            ByteVector vector = new ByteVector();
            uint num = 0;
            foreach (Item item in this.items)
            {
                vector.Add(item.Render());
                num++;
            }
            this.footer.ItemCount = num;
            this.footer.TagSize = (uint) (vector.Count + 0x20L);
            this.HeaderPresent = true;
            vector.Insert(0, this.footer.RenderHeader());
            vector.Add(this.footer.RenderFooter());
            return vector;
        }

        public void SetItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            int itemIndex = this.GetItemIndex(item.Key);
            if (itemIndex >= 0)
            {
                this.items[itemIndex] = item;
            }
            else
            {
                this.items.Add(item);
            }
        }

        public void SetValue(string key, string[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if ((value == null) || (value.Length == 0))
            {
                this.RemoveItem(key);
            }
            else
            {
                Item item = new Item(key, value);
                int itemIndex = this.GetItemIndex(key);
                if (itemIndex >= 0)
                {
                    this.items[itemIndex] = item;
                }
                else
                {
                    this.items.Add(item);
                }
            }
        }

        public void SetValue(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (string.IsNullOrEmpty(value))
            {
                this.RemoveItem(key);
            }
            else
            {
                string[] textArray1 = new string[] { value };
                this.SetValue(key, textArray1);
            }
        }

        public void SetValue(string key, uint number, uint count)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if ((number == 0) && (count == 0))
            {
                this.RemoveItem(key);
            }
            else if (count != 0)
            {
                object[] args = new object[] { number, count };
                this.SetValue(key, string.Format(CultureInfo.InvariantCulture, "{0}/{1}", args));
            }
            else
            {
                this.SetValue(key, number.ToString(CultureInfo.InvariantCulture));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string Album
        {
            get
            {
                return this.GetItemAsString("Album");
            }
            set
            {
                this.SetValue("Album", value);
            }
        }

        public override string[] AlbumArtists
        {
            get
            {
                string[] itemAsStrings = this.GetItemAsStrings("Album Artist");
                if (itemAsStrings.Length == 0)
                {
                    itemAsStrings = this.GetItemAsStrings("AlbumArtist");
                }
                return itemAsStrings;
            }
            set
            {
                this.SetValue("Album Artist", value);
                if (this.HasItem("AlbumArtist"))
                {
                    this.SetValue("AlbumArtist", value);
                }
            }
        }

        public override string[] AlbumArtistsSort
        {
            get
            {
                return this.GetItemAsStrings("AlbumArtistSort");
            }
            set
            {
                this.SetValue("AlbumArtistSort", value);
            }
        }

        public override string AlbumSort
        {
            get
            {
                return this.GetItemAsString("AlbumSort");
            }
            set
            {
                this.SetValue("AlbumSort", value);
            }
        }

        public override string AmazonId
        {
            get
            {
                return this.GetItemAsString("ASIN");
            }
            set
            {
                this.SetValue("ASIN", value);
            }
        }

        public override uint BeatsPerMinute
        {
            get
            {
                double num;
                string itemAsString = this.GetItemAsString("BPM");
                if ((itemAsString != null) && double.TryParse(itemAsString, out num))
                {
                    return (uint) Math.Round(num);
                }
                return 0;
            }
            set
            {
                this.SetValue("BPM", value, 0);
            }
        }

        public override string Comment
        {
            get
            {
                return this.GetItemAsString("Comment");
            }
            set
            {
                this.SetValue("Comment", value);
            }
        }

        public override string[] Composers
        {
            get
            {
                return this.GetItemAsStrings("Composer");
            }
            set
            {
                this.SetValue("Composer", value);
            }
        }

        public override string[] ComposersSort
        {
            get
            {
                return this.GetItemAsStrings("ComposerSort");
            }
            set
            {
                this.SetValue("ComposerSort", value);
            }
        }

        public override string Conductor
        {
            get
            {
                return this.GetItemAsString("Conductor");
            }
            set
            {
                this.SetValue("Conductor", value);
            }
        }

        public override string Copyright
        {
            get
            {
                return this.GetItemAsString("Copyright");
            }
            set
            {
                this.SetValue("Copyright", value);
            }
        }

        public override uint Disc
        {
            get
            {
                return this.GetItemAsUInt32("Disc", 0);
            }
            set
            {
                this.SetValue("Disc", value, this.DiscCount);
            }
        }

        public override uint DiscCount
        {
            get
            {
                return this.GetItemAsUInt32("Disc", 1);
            }
            set
            {
                this.SetValue("Disc", this.Disc, value);
            }
        }

        public override string[] Genres
        {
            get
            {
                return this.GetItemAsStrings("Genre");
            }
            set
            {
                this.SetValue("Genre", value);
            }
        }

        public override string Grouping
        {
            get
            {
                return this.GetItemAsString("Grouping");
            }
            set
            {
                this.SetValue("Grouping", value);
            }
        }

        public bool HeaderPresent
        {
            get
            {
                return ((this.footer.Flags & ((FooterFlags) (-2147483648))) != ((FooterFlags) 0));
            }
            set
            {
                if (value)
                {
                    this.footer.Flags |= (FooterFlags) (-2147483648);
                }
                else
                {
                    this.footer.Flags &= (FooterFlags) 0x7fffffff;
                }
            }
        }

        public override bool IsEmpty
        {
            get
            {
                return (this.items.Count == 0);
            }
        }

        public override string Lyrics
        {
            get
            {
                return this.GetItemAsString("Lyrics");
            }
            set
            {
                this.SetValue("Lyrics", value);
            }
        }

        public override string MusicBrainzArtistId
        {
            get
            {
                return this.GetItemAsString("MUSICBRAINZ_ARTISTID");
            }
            set
            {
                this.SetValue("MUSICBRAINZ_ARTISTID", value);
            }
        }

        public override string MusicBrainzDiscId
        {
            get
            {
                return this.GetItemAsString("MUSICBRAINZ_DISCID");
            }
            set
            {
                this.SetValue("MUSICBRAINZ_DISCID", value);
            }
        }

        public override string MusicBrainzReleaseArtistId
        {
            get
            {
                return this.GetItemAsString("MUSICBRAINZ_ALBUMARTISTID");
            }
            set
            {
                this.SetValue("MUSICBRAINZ_ALBUMARTISTID", value);
            }
        }

        public override string MusicBrainzReleaseCountry
        {
            get
            {
                return this.GetItemAsString("RELEASECOUNTRY");
            }
            set
            {
                this.SetValue("RELEASECOUNTRY", value);
            }
        }

        public override string MusicBrainzReleaseId
        {
            get
            {
                return this.GetItemAsString("MUSICBRAINZ_ALBUMID");
            }
            set
            {
                this.SetValue("MUSICBRAINZ_ALBUMID", value);
            }
        }

        public override string MusicBrainzReleaseStatus
        {
            get
            {
                return this.GetItemAsString("MUSICBRAINZ_ALBUMSTATUS");
            }
            set
            {
                this.SetValue("MUSICBRAINZ_ALBUMSTATUS", value);
            }
        }

        public override string MusicBrainzReleaseType
        {
            get
            {
                return this.GetItemAsString("MUSICBRAINZ_ALBUMTYPE");
            }
            set
            {
                this.SetValue("MUSICBRAINZ_ALBUMTYPE", value);
            }
        }

        public override string MusicBrainzTrackId
        {
            get
            {
                return this.GetItemAsString("MUSICBRAINZ_TRACKID");
            }
            set
            {
                this.SetValue("MUSICBRAINZ_TRACKID", value);
            }
        }

        public override string MusicIpId
        {
            get
            {
                return this.GetItemAsString("MUSICIP_PUID");
            }
            set
            {
                this.SetValue("MUSICIP_PUID", value);
            }
        }

        public override string[] Performers
        {
            get
            {
                return this.GetItemAsStrings("Artist");
            }
            set
            {
                this.SetValue("Artist", value);
            }
        }

        public override string[] PerformersSort
        {
            get
            {
                return this.GetItemAsStrings("ArtistSort");
            }
            set
            {
                this.SetValue("ArtistSort", value);
            }
        }

        public override IPicture[] Pictures
        {
            get
            {
                List<IPicture> list = new List<IPicture>();
                for (int i = 0; i < picture_item_names.Length; i++)
                {
                    Item item = this.GetItem(picture_item_names[i]);
                    if ((item != null) && (item.Type == ItemType.Binary))
                    {
                        int count = item.Value.Find(ByteVector.TextDelimiter(StringType.UTF8));
                        if (count >= 0)
                        {
                            Picture picture = new Picture(item.Value.Mid(count + 1)) {
                                Description = item.Value.ToString(StringType.UTF8, 0, count),
                                Type = i
                            };
                            list.Add(picture);
                        }
                    }
                }
                return list.ToArray();
            }
            set
            {
                foreach (string str in picture_item_names)
                {
                    this.RemoveItem(str);
                }
                if ((value != null) && (value.Length != 0))
                {
                    foreach (IPicture picture in value)
                    {
                        int type = (int) picture.Type;
                        if (type >= picture_item_names.Length)
                        {
                            type = 0;
                        }
                        string key = picture_item_names[type];
                        if (this.GetItem(key) == null)
                        {
                            ByteVector vector = ByteVector.FromString(picture.Description, StringType.UTF8);
                            vector.Add(ByteVector.TextDelimiter(StringType.UTF8));
                            vector.Add(picture.Data);
                            this.SetItem(new Item(key, vector));
                        }
                    }
                }
            }
        }

        public override TagLib.TagTypes TagTypes
        {
            get
            {
                return (TagLib.TagTypes.None | TagLib.TagTypes.Ape);
            }
        }

        public override string Title
        {
            get
            {
                return this.GetItemAsString("Title");
            }
            set
            {
                this.SetValue("Title", value);
            }
        }

        public override string TitleSort
        {
            get
            {
                return this.GetItemAsString("TitleSort");
            }
            set
            {
                this.SetValue("TitleSort", value);
            }
        }

        public override uint Track
        {
            get
            {
                return this.GetItemAsUInt32("Track", 0);
            }
            set
            {
                this.SetValue("Track", value, this.TrackCount);
            }
        }

        public override uint TrackCount
        {
            get
            {
                return this.GetItemAsUInt32("Track", 1);
            }
            set
            {
                this.SetValue("Track", this.Track, value);
            }
        }

        public override uint Year
        {
            get
            {
                uint num;
                string itemAsString = this.GetItemAsString("Year");
                if ((itemAsString == null) || (itemAsString.Length == 0))
                {
                    return 0;
                }
                if (!uint.TryParse(itemAsString, out num) && ((itemAsString.Length < 4) || !uint.TryParse(itemAsString.Substring(0, 4), out num)))
                {
                    return 0;
                }
                return num;
            }
            set
            {
                this.SetValue("Year", value, 0);
            }
        }

        [CompilerGenerated]
        private sealed class <GetEnumerator>c__Iterator0 : IDisposable, IEnumerator, IEnumerator<string>
        {
            internal string $current;
            internal int $PC;
            internal List<Item>.Enumerator <$s_10>__0;
            internal TagLib.Ape.Tag <>f__this;
            internal Item <item>__1;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.<$s_10>__0.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.<$s_10>__0 = this.<>f__this.items.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00AE;
                }
                try
                {
                    while (this.<$s_10>__0.MoveNext())
                    {
                        this.<item>__1 = this.<$s_10>__0.Current;
                        this.$current = this.<item>__1.Key;
                        this.$PC = 1;
                        flag = true;
                        return true;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_10>__0.Dispose();
                }
                this.$PC = -1;
            Label_00AE:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            string IEnumerator<string>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

