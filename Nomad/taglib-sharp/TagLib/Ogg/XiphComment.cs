namespace TagLib.Ogg
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using TagLib;

    public class XiphComment : Tag, IEnumerable, IEnumerable<string>
    {
        private string comment_field;
        private Dictionary<string, string[]> field_list;
        private string vendor_id;

        public XiphComment()
        {
            this.field_list = new Dictionary<string, string[]>();
            this.comment_field = "DESCRIPTION";
        }

        public XiphComment(ByteVector data)
        {
            this.field_list = new Dictionary<string, string[]>();
            this.comment_field = "DESCRIPTION";
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.Parse(data);
        }

        public override void Clear()
        {
            this.field_list.Clear();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.field_list.Keys.GetEnumerator();
        }

        public string[] GetField(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            key = key.ToUpper(CultureInfo.InvariantCulture);
            if (!this.field_list.ContainsKey(key))
            {
                return new string[0];
            }
            return (string[]) this.field_list[key].Clone();
        }

        public string GetFirstField(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            string[] field = this.GetField(key);
            return ((field.Length <= 0) ? null : field[0]);
        }

        protected void Parse(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            int startIndex = 0;
            int count = (int) data.Mid(startIndex, 4).ToUInt(false);
            startIndex += 4;
            this.vendor_id = data.ToString(StringType.UTF8, startIndex, count);
            startIndex += count;
            int num3 = (int) data.Mid(startIndex, 4).ToUInt(false);
            startIndex += 4;
            for (int i = 0; i < num3; i++)
            {
                int num5 = (int) data.Mid(startIndex, 4).ToUInt(false);
                startIndex += 4;
                string str = data.ToString(StringType.UTF8, startIndex, num5);
                startIndex += num5;
                int index = str.IndexOf('=');
                if (index >= 0)
                {
                    string[] strArray;
                    string key = str.Substring(0, index).ToUpper(CultureInfo.InvariantCulture);
                    string str3 = str.Substring(index + 1);
                    if (this.field_list.TryGetValue(key, out strArray))
                    {
                        Array.Resize<string>(ref strArray, strArray.Length + 1);
                        strArray[strArray.Length - 1] = str3;
                        this.field_list[key] = strArray;
                    }
                    else
                    {
                        string[] values = new string[] { str3 };
                        this.SetField(key, values);
                    }
                }
            }
        }

        public void RemoveField(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            key = key.ToUpper(CultureInfo.InvariantCulture);
            this.field_list.Remove(key);
        }

        public ByteVector Render(bool addFramingBit)
        {
            ByteVector vector = new ByteVector();
            ByteVector data = ByteVector.FromString(this.vendor_id, StringType.UTF8);
            vector.Add(ByteVector.FromUInt((uint) data.Count, false));
            vector.Add(data);
            vector.Add(ByteVector.FromUInt(this.FieldCount, false));
            foreach (KeyValuePair<string, string[]> pair in this.field_list)
            {
                foreach (string str in pair.Value)
                {
                    ByteVector vector3 = ByteVector.FromString(pair.Key, StringType.UTF8);
                    vector3.Add((byte) 0x3d);
                    vector3.Add(ByteVector.FromString(str, StringType.UTF8));
                    vector.Add(ByteVector.FromUInt((uint) vector3.Count, false));
                    vector.Add(vector3);
                }
            }
            if (addFramingBit)
            {
                vector.Add((byte) 1);
            }
            return vector;
        }

        public void SetField(string key, params string[] values)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            key = key.ToUpper(CultureInfo.InvariantCulture);
            if ((values == null) || (values.Length == 0))
            {
                this.RemoveField(key);
            }
            else
            {
                List<string> list = new List<string>();
                foreach (string str in values)
                {
                    if ((str != null) && (str.Trim().Length != 0))
                    {
                        list.Add(str);
                    }
                }
                if (list.Count == 0)
                {
                    this.RemoveField(key);
                }
                else if (this.field_list.ContainsKey(key))
                {
                    this.field_list[key] = list.ToArray();
                }
                else
                {
                    this.field_list.Add(key, list.ToArray());
                }
            }
        }

        public void SetField(string key, uint number)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (number == 0)
            {
                this.RemoveField(key);
            }
            else
            {
                string[] values = new string[] { number.ToString(CultureInfo.InvariantCulture) };
                this.SetField(key, values);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.field_list.Keys.GetEnumerator();
        }

        public override string Album
        {
            get
            {
                return this.GetFirstField("ALBUM");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("ALBUM", values);
            }
        }

        public override string[] AlbumArtists
        {
            get
            {
                string[] field = this.GetField("ALBUMARTIST");
                if ((field != null) && (field.Length > 0))
                {
                    return field;
                }
                field = this.GetField("ALBUM ARTIST");
                if ((field != null) && (field.Length > 0))
                {
                    return field;
                }
                return this.GetField("ENSEMBLE");
            }
            set
            {
                this.SetField("ALBUMARTIST", value);
            }
        }

        public override string[] AlbumArtistsSort
        {
            get
            {
                return this.GetField("ALBUMARTISTSORT");
            }
            set
            {
                this.SetField("ALBUMARTISTSORT", value);
            }
        }

        public override string AlbumSort
        {
            get
            {
                return this.GetFirstField("ALBUMSORT");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("ALBUMSORT", values);
            }
        }

        public override string AmazonId
        {
            get
            {
                return this.GetFirstField("ASIN");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("ASIN", values);
            }
        }

        public override uint BeatsPerMinute
        {
            get
            {
                double num;
                string firstField = this.GetFirstField("TEMPO");
                return ((((firstField == null) || !double.TryParse(firstField, out num)) || (num <= 0.0)) ? 0 : ((uint) Math.Round(num)));
            }
            set
            {
                this.SetField("TEMPO", value);
            }
        }

        public override string Comment
        {
            get
            {
                string firstField = this.GetFirstField(this.comment_field);
                if ((firstField != null) || (this.comment_field == "COMMENT"))
                {
                    return firstField;
                }
                this.comment_field = "COMMENT";
                return this.GetFirstField(this.comment_field);
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField(this.comment_field, values);
            }
        }

        public override string[] Composers
        {
            get
            {
                return this.GetField("COMPOSER");
            }
            set
            {
                this.SetField("COMPOSER", value);
            }
        }

        public override string[] ComposersSort
        {
            get
            {
                return this.GetField("COMPOSERSORT");
            }
            set
            {
                this.SetField("COMPOSERSORT", value);
            }
        }

        public override string Conductor
        {
            get
            {
                return this.GetFirstField("CONDUCTOR");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("CONDUCTOR", values);
            }
        }

        public override string Copyright
        {
            get
            {
                return this.GetFirstField("COPYRIGHT");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("COPYRIGHT", values);
            }
        }

        public override uint Disc
        {
            get
            {
                string firstField = this.GetFirstField("DISCNUMBER");
                if (firstField != null)
                {
                    string[] strArray;
                    uint num;
                    char[] separator = new char[] { '/' };
                    if (((strArray = firstField.Split(separator)).Length > 0) && uint.TryParse(strArray[0], out num))
                    {
                        return num;
                    }
                }
                return 0;
            }
            set
            {
                this.SetField("DISCTOTAL", this.DiscCount);
                this.SetField("DISCNUMBER", value);
            }
        }

        public override uint DiscCount
        {
            get
            {
                string firstField;
                uint num;
                if (((firstField = this.GetFirstField("DISCTOTAL")) != null) && uint.TryParse(firstField, out num))
                {
                    return num;
                }
                firstField = this.GetFirstField("DISCNUMBER");
                if (firstField != null)
                {
                    string[] strArray;
                    char[] separator = new char[] { '/' };
                    if (((strArray = firstField.Split(separator)).Length > 1) && uint.TryParse(strArray[1], out num))
                    {
                        return num;
                    }
                }
                return 0;
            }
            set
            {
                this.SetField("DISCTOTAL", value);
            }
        }

        public uint FieldCount
        {
            get
            {
                uint num = 0;
                foreach (string[] strArray in this.field_list.Values)
                {
                    num += (uint) strArray.Length;
                }
                return num;
            }
        }

        public override string[] Genres
        {
            get
            {
                return this.GetField("GENRE");
            }
            set
            {
                this.SetField("GENRE", value);
            }
        }

        public override string Grouping
        {
            get
            {
                return this.GetFirstField("GROUPING");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("GROUPING", values);
            }
        }

        public bool IsCompilation
        {
            get
            {
                string str;
                int num;
                return ((((str = this.GetFirstField("COMPILATION")) != null) && int.TryParse(str, out num)) && (num == 1));
            }
            set
            {
                if (value)
                {
                    string[] values = new string[] { "1" };
                    this.SetField("COMPILATION", values);
                }
                else
                {
                    this.RemoveField("COMPILATION");
                }
            }
        }

        public override bool IsEmpty
        {
            get
            {
                foreach (string[] strArray in this.field_list.Values)
                {
                    if (strArray.Length != 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public override string Lyrics
        {
            get
            {
                return this.GetFirstField("LYRICS");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("LYRICS", values);
            }
        }

        public override string MusicBrainzArtistId
        {
            get
            {
                return this.GetFirstField("MUSICBRAINZ_ARTISTID");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("MUSICBRAINZ_ARTISTID", values);
            }
        }

        public override string MusicBrainzDiscId
        {
            get
            {
                return this.GetFirstField("MUSICBRAINZ_DISCID");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("MUSICBRAINZ_DISCID", values);
            }
        }

        public override string MusicBrainzReleaseArtistId
        {
            get
            {
                return this.GetFirstField("MUSICBRAINZ_ALBUMARTISTID");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("MUSICBRAINZ_ALBUMARTISTID", values);
            }
        }

        public override string MusicBrainzReleaseCountry
        {
            get
            {
                return this.GetFirstField("RELEASECOUNTRY");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("RELEASECOUNTRY", values);
            }
        }

        public override string MusicBrainzReleaseId
        {
            get
            {
                return this.GetFirstField("MUSICBRAINZ_ALBUMID");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("MUSICBRAINZ_ALBUMID", values);
            }
        }

        public override string MusicBrainzReleaseStatus
        {
            get
            {
                return this.GetFirstField("MUSICBRAINZ_ALBUMSTATUS");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("MUSICBRAINZ_ALBUMSTATUS", values);
            }
        }

        public override string MusicBrainzReleaseType
        {
            get
            {
                return this.GetFirstField("MUSICBRAINZ_ALBUMTYPE");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("MUSICBRAINZ_ALBUMTYPE", values);
            }
        }

        public override string MusicBrainzTrackId
        {
            get
            {
                return this.GetFirstField("MUSICBRAINZ_TRACKID");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("MUSICBRAINZ_TRACKID", values);
            }
        }

        public override string MusicIpId
        {
            get
            {
                return this.GetFirstField("MUSICIP_PUID");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("MUSICIP_PUID", values);
            }
        }

        public override string[] Performers
        {
            get
            {
                return this.GetField("ARTIST");
            }
            set
            {
                this.SetField("ARTIST", value);
            }
        }

        public override string[] PerformersSort
        {
            get
            {
                return this.GetField("ARTISTSORT");
            }
            set
            {
                this.SetField("ARTISTSORT", value);
            }
        }

        public override IPicture[] Pictures
        {
            get
            {
                string[] field = this.GetField("COVERART");
                IPicture[] pictureArray = new Picture[field.Length];
                for (int i = 0; i < field.Length; i++)
                {
                    ByteVector data = new ByteVector(Convert.FromBase64String(field[i]));
                    pictureArray[i] = new Picture(data);
                }
                return pictureArray;
            }
            set
            {
                string[] values = new string[value.Length];
                for (int i = 0; i < value.Length; i++)
                {
                    IPicture picture = value[i];
                    values[i] = Convert.ToBase64String(picture.Data.Data);
                }
                this.SetField("COVERART", values);
            }
        }

        public override TagLib.TagTypes TagTypes
        {
            get
            {
                return TagLib.TagTypes.Xiph;
            }
        }

        public override string Title
        {
            get
            {
                return this.GetFirstField("TITLE");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("TITLE", values);
            }
        }

        public override string TitleSort
        {
            get
            {
                return this.GetFirstField("TITLESORT");
            }
            set
            {
                string[] values = new string[] { value };
                this.SetField("TITLESORT", values);
            }
        }

        public override uint Track
        {
            get
            {
                string firstField = this.GetFirstField("TRACKNUMBER");
                if (firstField != null)
                {
                    string[] strArray;
                    uint num;
                    char[] separator = new char[] { '/' };
                    if (((strArray = firstField.Split(separator)).Length > 0) && uint.TryParse(strArray[0], out num))
                    {
                        return num;
                    }
                }
                return 0;
            }
            set
            {
                this.SetField("TRACKTOTAL", this.TrackCount);
                this.SetField("TRACKNUMBER", value);
            }
        }

        public override uint TrackCount
        {
            get
            {
                string firstField;
                uint num;
                if (((firstField = this.GetFirstField("TRACKTOTAL")) != null) && uint.TryParse(firstField, out num))
                {
                    return num;
                }
                firstField = this.GetFirstField("TRACKNUMBER");
                if (firstField != null)
                {
                    string[] strArray;
                    char[] separator = new char[] { '/' };
                    if (((strArray = firstField.Split(separator)).Length > 1) && uint.TryParse(strArray[1], out num))
                    {
                        return num;
                    }
                }
                return 0;
            }
            set
            {
                this.SetField("TRACKTOTAL", value);
            }
        }

        public string VendorId
        {
            get
            {
                return this.vendor_id;
            }
        }

        public override uint Year
        {
            get
            {
                uint num;
                string firstField = this.GetFirstField("DATE");
                return (((firstField == null) || !uint.TryParse((firstField.Length <= 4) ? firstField : firstField.Substring(0, 4), out num)) ? 0 : num);
            }
            set
            {
                this.SetField("DATE", value);
            }
        }
    }
}

