namespace TagLib.Asf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using TagLib;

    public class Tag : TagLib.Tag, IEnumerable<ContentDescriptor>, IEnumerable
    {
        private TagLib.Asf.ContentDescriptionObject description;
        private TagLib.Asf.ExtendedContentDescriptionObject ext_description;
        private TagLib.Asf.MetadataLibraryObject metadata_library;

        public Tag()
        {
            this.description = new TagLib.Asf.ContentDescriptionObject();
            this.ext_description = new TagLib.Asf.ExtendedContentDescriptionObject();
            this.metadata_library = new TagLib.Asf.MetadataLibraryObject();
        }

        public Tag(HeaderObject header)
        {
            this.description = new TagLib.Asf.ContentDescriptionObject();
            this.ext_description = new TagLib.Asf.ExtendedContentDescriptionObject();
            this.metadata_library = new TagLib.Asf.MetadataLibraryObject();
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }
            IEnumerator<TagLib.Asf.Object> enumerator = header.Children.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    TagLib.Asf.Object current = enumerator.Current;
                    if (current is TagLib.Asf.ContentDescriptionObject)
                    {
                        this.description = current as TagLib.Asf.ContentDescriptionObject;
                    }
                    if (current is TagLib.Asf.ExtendedContentDescriptionObject)
                    {
                        this.ext_description = current as TagLib.Asf.ExtendedContentDescriptionObject;
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
            IEnumerator<TagLib.Asf.Object> enumerator2 = header.Extension.Children.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    TagLib.Asf.Object obj3 = enumerator2.Current;
                    if (obj3 is TagLib.Asf.MetadataLibraryObject)
                    {
                        this.metadata_library = obj3 as TagLib.Asf.MetadataLibraryObject;
                    }
                }
            }
            finally
            {
                if (enumerator2 == null)
                {
                }
                enumerator2.Dispose();
            }
        }

        public void AddDescriptor(ContentDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException("descriptor");
            }
            this.ext_description.AddDescriptor(descriptor);
        }

        public override void Clear()
        {
            this.description = new TagLib.Asf.ContentDescriptionObject();
            this.ext_description = new TagLib.Asf.ExtendedContentDescriptionObject();
            this.metadata_library.RemoveRecords(0, 0, "WM/Picture");
        }

        public IEnumerable<ContentDescriptor> GetDescriptors(params string[] names)
        {
            if (names == null)
            {
                throw new ArgumentNullException("names");
            }
            return this.ext_description.GetDescriptors(names);
        }

        public string GetDescriptorString(params string[] names)
        {
            if (names == null)
            {
                throw new ArgumentNullException("names");
            }
            IEnumerator<ContentDescriptor> enumerator = this.GetDescriptors(names).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ContentDescriptor current = enumerator.Current;
                    if ((current != null) && (current.Type == DataType.Unicode))
                    {
                        string str = current.ToString();
                        if (str != null)
                        {
                            return str;
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
            return null;
        }

        public string[] GetDescriptorStrings(params string[] names)
        {
            if (names == null)
            {
                throw new ArgumentNullException("names");
            }
            return SplitAndClean(this.GetDescriptorString(names));
        }

        public IEnumerator<ContentDescriptor> GetEnumerator()
        {
            return this.ext_description.GetEnumerator();
        }

        private static IPicture PictureFromData(ByteVector data)
        {
            if (data.Count < 9)
            {
                return null;
            }
            int startIndex = 0;
            Picture picture = new Picture {
                Type = data[startIndex]
            };
            startIndex++;
            int length = (int) data.Mid(startIndex, 4).ToUInt(false);
            startIndex += 4;
            int num3 = data.Find(ByteVector.TextDelimiter(StringType.UTF16LE), startIndex, 2);
            if (num3 < 0)
            {
                return null;
            }
            picture.MimeType = data.ToString(StringType.UTF16LE, startIndex, num3 - startIndex);
            startIndex = num3 + 2;
            num3 = data.Find(ByteVector.TextDelimiter(StringType.UTF16LE), startIndex, 2);
            if (num3 < 0)
            {
                return null;
            }
            picture.Description = data.ToString(StringType.UTF16LE, startIndex, num3 - startIndex);
            startIndex = num3 + 2;
            picture.Data = data.Mid(startIndex, length);
            return picture;
        }

        private static ByteVector PictureToData(IPicture picture)
        {
            return new ByteVector(new byte[] { (byte) picture.Type }) { TagLib.Asf.Object.RenderDWord((uint) picture.Data.Count), TagLib.Asf.Object.RenderUnicode(picture.MimeType), TagLib.Asf.Object.RenderUnicode(picture.Description), picture.Data };
        }

        public void RemoveDescriptors(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            this.ext_description.RemoveDescriptors(name);
        }

        public void SetDescriptors(string name, params ContentDescriptor[] descriptors)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            this.ext_description.SetDescriptors(name, descriptors);
        }

        public void SetDescriptorString(string value, params string[] names)
        {
            if (names == null)
            {
                throw new ArgumentNullException("names");
            }
            int index = 0;
            if ((value != null) && (value.Trim().Length != 0))
            {
                ContentDescriptor[] descriptors = new ContentDescriptor[] { new ContentDescriptor(names[0], value) };
                this.SetDescriptors(names[0], descriptors);
                index++;
            }
            while (index < names.Length)
            {
                this.RemoveDescriptors(names[index]);
                index++;
            }
        }

        public void SetDescriptorStrings(string[] value, params string[] names)
        {
            if (names == null)
            {
                throw new ArgumentNullException("names");
            }
            this.SetDescriptorString(string.Join("; ", value), names);
        }

        private static string[] SplitAndClean(string s)
        {
            if ((s == null) || (s.Trim().Length == 0))
            {
                return new string[0];
            }
            char[] separator = new char[] { ';' };
            string[] strArray = s.Split(separator);
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = strArray[i].Trim();
            }
            return strArray;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.ext_description.GetEnumerator();
        }

        public override string Album
        {
            get
            {
                string[] names = new string[] { "WM/AlbumTitle", "Album" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "WM/AlbumTitle", "Album" };
                this.SetDescriptorString(value, names);
            }
        }

        public override string[] AlbumArtists
        {
            get
            {
                string[] names = new string[] { "WM/AlbumArtist", "AlbumArtist" };
                return this.GetDescriptorStrings(names);
            }
            set
            {
                string[] names = new string[] { "WM/AlbumArtist", "AlbumArtist" };
                this.SetDescriptorStrings(value, names);
            }
        }

        public override string[] AlbumArtistsSort
        {
            get
            {
                string[] names = new string[] { "WM/AlbumArtistSortOrder" };
                return this.GetDescriptorStrings(names);
            }
            set
            {
                string[] names = new string[] { "WM/AlbumArtistSortOrder" };
                this.SetDescriptorStrings(value, names);
            }
        }

        public override string AlbumSort
        {
            get
            {
                string[] names = new string[] { "WM/AlbumSortOrder" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "WM/AlbumSortOrder" };
                this.SetDescriptorString(value, names);
            }
        }

        public override uint BeatsPerMinute
        {
            get
            {
                string[] names = new string[] { "WM/BeatsPerMinute" };
                IEnumerator<ContentDescriptor> enumerator = this.GetDescriptors(names).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        uint num = enumerator.Current.ToDWord();
                        if (num != 0)
                        {
                            return num;
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
                return 0;
            }
            set
            {
                if (value == 0)
                {
                    this.RemoveDescriptors("WM/BeatsPerMinute");
                }
                else
                {
                    ContentDescriptor[] descriptors = new ContentDescriptor[] { new ContentDescriptor("WM/BeatsPerMinute", value) };
                    this.SetDescriptors("WM/BeatsPerMinute", descriptors);
                }
            }
        }

        public override string Comment
        {
            get
            {
                return this.description.Description;
            }
            set
            {
                this.description.Description = value;
            }
        }

        public override string[] Composers
        {
            get
            {
                string[] names = new string[] { "WM/Composer", "Composer" };
                return this.GetDescriptorStrings(names);
            }
            set
            {
                string[] names = new string[] { "WM/Composer", "Composer" };
                this.SetDescriptorStrings(value, names);
            }
        }

        public override string Conductor
        {
            get
            {
                string[] names = new string[] { "WM/Conductor" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "WM/Conductor" };
                this.SetDescriptorString(value, names);
            }
        }

        public TagLib.Asf.ContentDescriptionObject ContentDescriptionObject
        {
            get
            {
                return this.description;
            }
        }

        public override string Copyright
        {
            get
            {
                return this.description.Copyright;
            }
            set
            {
                this.description.Copyright = value;
            }
        }

        public override uint Disc
        {
            get
            {
                uint num;
                string[] names = new string[] { "WM/PartOfSet" };
                string descriptorString = this.GetDescriptorString(names);
                if (descriptorString == null)
                {
                    return 0;
                }
                char[] separator = new char[] { '/' };
                string[] strArray = descriptorString.Split(separator);
                if (strArray.Length < 1)
                {
                    return 0;
                }
                return (!uint.TryParse(strArray[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out num) ? 0 : num);
            }
            set
            {
                uint discCount = this.DiscCount;
                if ((value == 0) && (discCount == 0))
                {
                    this.RemoveDescriptors("WM/PartOfSet");
                }
                else if (discCount != 0)
                {
                    object[] args = new object[] { value, discCount };
                    string[] names = new string[] { "WM/PartOfSet" };
                    this.SetDescriptorString(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", args), names);
                }
                else
                {
                    string[] textArray2 = new string[] { "WM/PartOfSet" };
                    this.SetDescriptorString(value.ToString(CultureInfo.InvariantCulture), textArray2);
                }
            }
        }

        public override uint DiscCount
        {
            get
            {
                uint num;
                string[] names = new string[] { "WM/PartOfSet" };
                string descriptorString = this.GetDescriptorString(names);
                if (descriptorString == null)
                {
                    return 0;
                }
                char[] separator = new char[] { '/' };
                string[] strArray = descriptorString.Split(separator);
                if (strArray.Length < 2)
                {
                    return 0;
                }
                return (!uint.TryParse(strArray[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out num) ? 0 : num);
            }
            set
            {
                uint disc = this.Disc;
                if ((disc == 0) && (value == 0))
                {
                    this.RemoveDescriptors("WM/PartOfSet");
                }
                else if (value != 0)
                {
                    object[] args = new object[] { disc, value };
                    string[] names = new string[] { "WM/PartOfSet" };
                    this.SetDescriptorString(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", args), names);
                }
                else
                {
                    string[] textArray2 = new string[] { "WM/PartOfSet" };
                    this.SetDescriptorString(disc.ToString(CultureInfo.InvariantCulture), textArray2);
                }
            }
        }

        public TagLib.Asf.ExtendedContentDescriptionObject ExtendedContentDescriptionObject
        {
            get
            {
                return this.ext_description;
            }
        }

        public override string[] Genres
        {
            get
            {
                string[] names = new string[] { "WM/Genre", "WM/GenreID", "Genre" };
                string descriptorString = this.GetDescriptorString(names);
                if ((descriptorString == null) || (descriptorString.Trim().Length == 0))
                {
                    return new string[0];
                }
                char[] separator = new char[] { ';' };
                string[] strArray = descriptorString.Split(separator);
                for (int i = 0; i < strArray.Length; i++)
                {
                    byte num2;
                    string str2 = strArray[i].Trim();
                    int index = str2.IndexOf(')');
                    if (((index > 0) && (str2[0] == '(')) && byte.TryParse(str2.Substring(1, index - 1), out num2))
                    {
                        str2 = TagLib.Genres.IndexToAudio(num2);
                    }
                    strArray[i] = str2;
                }
                return strArray;
            }
            set
            {
                string[] names = new string[] { "WM/Genre", "Genre", "WM/GenreID" };
                this.SetDescriptorString(string.Join("; ", value), names);
            }
        }

        public override string Grouping
        {
            get
            {
                string[] names = new string[] { "WM/ContentGroupDescription" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "WM/ContentGroupDescription" };
                this.SetDescriptorString(value, names);
            }
        }

        public override bool IsEmpty
        {
            get
            {
                return (this.description.IsEmpty && this.ext_description.IsEmpty);
            }
        }

        public override string Lyrics
        {
            get
            {
                string[] names = new string[] { "WM/Lyrics" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "WM/Lyrics" };
                this.SetDescriptorString(value, names);
            }
        }

        public TagLib.Asf.MetadataLibraryObject MetadataLibraryObject
        {
            get
            {
                return this.metadata_library;
            }
        }

        public override string MusicBrainzArtistId
        {
            get
            {
                string[] names = new string[] { "MusicBrainz/Artist Id" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "MusicBrainz/Artist Id" };
                this.SetDescriptorString(value, names);
            }
        }

        public override string MusicBrainzDiscId
        {
            get
            {
                string[] names = new string[] { "MusicBrainz/Disc Id" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "MusicBrainz/Disc Id" };
                this.SetDescriptorString(value, names);
            }
        }

        public override string MusicBrainzReleaseArtistId
        {
            get
            {
                string[] names = new string[] { "MusicBrainz/Album Artist Id" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "MusicBrainz/Album Artist Id" };
                this.SetDescriptorString(value, names);
            }
        }

        public override string MusicBrainzReleaseCountry
        {
            get
            {
                string[] names = new string[] { "MusicBrainz/Album Release Country" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "MusicBrainz/Album Release Country" };
                this.SetDescriptorString(value, names);
            }
        }

        public override string MusicBrainzReleaseId
        {
            get
            {
                string[] names = new string[] { "MusicBrainz/Album Id" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "MusicBrainz/Album Id" };
                this.SetDescriptorString(value, names);
            }
        }

        public override string MusicBrainzReleaseStatus
        {
            get
            {
                string[] names = new string[] { "MusicBrainz/Album Status" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "MusicBrainz/Album Status" };
                this.SetDescriptorString(value, names);
            }
        }

        public override string MusicBrainzReleaseType
        {
            get
            {
                string[] names = new string[] { "MusicBrainz/Album Type" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "MusicBrainz/Album Type" };
                this.SetDescriptorString(value, names);
            }
        }

        public override string MusicBrainzTrackId
        {
            get
            {
                string[] names = new string[] { "MusicBrainz/Track Id" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "MusicBrainz/Track Id" };
                this.SetDescriptorString(value, names);
            }
        }

        public override string MusicIpId
        {
            get
            {
                string[] names = new string[] { "MusicIP/PUID" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "MusicIP/PUID" };
                this.SetDescriptorString(value, names);
            }
        }

        public override string[] Performers
        {
            get
            {
                return SplitAndClean(this.description.Author);
            }
            set
            {
                this.description.Author = string.Join("; ", value);
            }
        }

        public override string[] PerformersSort
        {
            get
            {
                string[] names = new string[] { "WM/ArtistSortOrder" };
                return this.GetDescriptorStrings(names);
            }
            set
            {
                string[] names = new string[] { "WM/ArtistSortOrder" };
                this.SetDescriptorStrings(value, names);
            }
        }

        public override IPicture[] Pictures
        {
            get
            {
                List<IPicture> list = new List<IPicture>();
                string[] names = new string[] { "WM/Picture" };
                IEnumerator<ContentDescriptor> enumerator = this.GetDescriptors(names).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        IPicture item = PictureFromData(enumerator.Current.ToByteVector());
                        if (item != null)
                        {
                            list.Add(item);
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
                string[] textArray2 = new string[] { "WM/Picture" };
                IEnumerator<DescriptionRecord> enumerator2 = this.metadata_library.GetRecords(0, 0, textArray2).GetEnumerator();
                try
                {
                    while (enumerator2.MoveNext())
                    {
                        IPicture picture2 = PictureFromData(enumerator2.Current.ToByteVector());
                        if (picture2 != null)
                        {
                            list.Add(picture2);
                        }
                    }
                }
                finally
                {
                    if (enumerator2 == null)
                    {
                    }
                    enumerator2.Dispose();
                }
                return list.ToArray();
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    this.RemoveDescriptors("WM/Picture");
                    this.metadata_library.RemoveRecords(0, 0, "WM/Picture");
                }
                else
                {
                    List<ByteVector> list = new List<ByteVector>();
                    bool flag = false;
                    foreach (IPicture picture in value)
                    {
                        ByteVector item = PictureToData(picture);
                        list.Add(item);
                        if (item.Count > 0xffff)
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        DescriptionRecord[] records = new DescriptionRecord[list.Count];
                        for (int i = 0; i < list.Count; i++)
                        {
                            records[i] = new DescriptionRecord(0, 0, "WM/Picture", list[i]);
                        }
                        this.RemoveDescriptors("WM/Picture");
                        this.metadata_library.SetRecords(0, 0, "WM/Picture", records);
                    }
                    else
                    {
                        ContentDescriptor[] descriptors = new ContentDescriptor[list.Count];
                        for (int j = 0; j < list.Count; j++)
                        {
                            descriptors[j] = new ContentDescriptor("WM/Picture", list[j]);
                        }
                        this.metadata_library.RemoveRecords(0, 0, "WM/Picture");
                        this.SetDescriptors("WM/Picture", descriptors);
                    }
                }
            }
        }

        public override TagLib.TagTypes TagTypes
        {
            get
            {
                return TagLib.TagTypes.Asf;
            }
        }

        public override string Title
        {
            get
            {
                return this.description.Title;
            }
            set
            {
                this.description.Title = value;
            }
        }

        public override string TitleSort
        {
            get
            {
                string[] names = new string[] { "WM/TitleSortOrder" };
                return this.GetDescriptorString(names);
            }
            set
            {
                string[] names = new string[] { "WM/TitleSortOrder" };
                this.SetDescriptorString(value, names);
            }
        }

        public override uint Track
        {
            get
            {
                string[] names = new string[] { "WM/TrackNumber" };
                IEnumerator<ContentDescriptor> enumerator = this.GetDescriptors(names).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        uint num = enumerator.Current.ToDWord();
                        if (num != 0)
                        {
                            return num;
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
                return 0;
            }
            set
            {
                if (value == 0)
                {
                    this.RemoveDescriptors("WM/TrackNumber");
                }
                else
                {
                    ContentDescriptor[] descriptors = new ContentDescriptor[] { new ContentDescriptor("WM/TrackNumber", value) };
                    this.SetDescriptors("WM/TrackNumber", descriptors);
                }
            }
        }

        public override uint TrackCount
        {
            get
            {
                string[] names = new string[] { "TrackTotal" };
                IEnumerator<ContentDescriptor> enumerator = this.GetDescriptors(names).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        uint num = enumerator.Current.ToDWord();
                        if (num != 0)
                        {
                            return num;
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
                return 0;
            }
            set
            {
                if (value == 0)
                {
                    this.RemoveDescriptors("TrackTotal");
                }
                else
                {
                    ContentDescriptor[] descriptors = new ContentDescriptor[] { new ContentDescriptor("TrackTotal", value) };
                    this.SetDescriptors("TrackTotal", descriptors);
                }
            }
        }

        public override uint Year
        {
            get
            {
                uint num;
                string[] names = new string[] { "WM/Year" };
                string descriptorString = this.GetDescriptorString(names);
                if (((descriptorString != null) && (descriptorString.Length >= 4)) && uint.TryParse(descriptorString.Substring(0, 4), NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
                {
                    return num;
                }
                return 0;
            }
            set
            {
                if (value == 0)
                {
                    this.RemoveDescriptors("WM/Year");
                }
                else
                {
                    string[] names = new string[] { "WM/Year" };
                    this.SetDescriptorString(value.ToString(CultureInfo.InvariantCulture), names);
                }
            }
        }
    }
}

