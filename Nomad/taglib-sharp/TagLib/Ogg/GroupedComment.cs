namespace TagLib.Ogg
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class GroupedComment : Tag
    {
        private Dictionary<uint, XiphComment> comment_hash = new Dictionary<uint, XiphComment>();
        private List<XiphComment> tags = new List<XiphComment>();

        public void AddComment(uint streamSerialNumber, ByteVector data)
        {
            this.AddComment(streamSerialNumber, new XiphComment(data));
        }

        public void AddComment(uint streamSerialNumber, XiphComment comment)
        {
            this.comment_hash.Add(streamSerialNumber, comment);
            this.tags.Add(comment);
        }

        public override void Clear()
        {
            foreach (XiphComment comment in this.tags)
            {
                comment.Clear();
            }
        }

        public XiphComment GetComment(uint streamSerialNumber)
        {
            return this.comment_hash[streamSerialNumber];
        }

        public override string Album
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string album = comment.Album;
                        if ((album != null) && (album.Length > 0))
                        {
                            return album;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Album = value;
                }
            }
        }

        public override string[] AlbumArtists
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string[] albumArtists = comment.AlbumArtists;
                        if ((albumArtists != null) && (albumArtists.Length > 0))
                        {
                            return albumArtists;
                        }
                    }
                }
                return new string[0];
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].AlbumArtists = value;
                }
            }
        }

        public override string[] AlbumArtistsSort
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string[] albumArtistsSort = comment.AlbumArtistsSort;
                        if ((albumArtistsSort != null) && (albumArtistsSort.Length > 0))
                        {
                            return albumArtistsSort;
                        }
                    }
                }
                return new string[0];
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].AlbumArtistsSort = value;
                }
            }
        }

        public override string AlbumSort
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string albumSort = comment.AlbumSort;
                        if ((albumSort != null) && (albumSort.Length > 0))
                        {
                            return albumSort;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].AlbumSort = value;
                }
            }
        }

        public override string AmazonId
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string amazonId = comment.AmazonId;
                        if ((amazonId != null) && (amazonId.Length > 0))
                        {
                            return amazonId;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].AmazonId = value;
                }
            }
        }

        public override uint BeatsPerMinute
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if ((comment != null) && (comment.BeatsPerMinute != 0))
                    {
                        return comment.BeatsPerMinute;
                    }
                }
                return 0;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].BeatsPerMinute = value;
                }
            }
        }

        public override string Comment
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string str = comment.Comment;
                        if ((str != null) && (str.Length > 0))
                        {
                            return str;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Comment = value;
                }
            }
        }

        public IEnumerable<XiphComment> Comments
        {
            get
            {
                return this.tags;
            }
        }

        public override string[] Composers
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string[] composers = comment.Composers;
                        if ((composers != null) && (composers.Length > 0))
                        {
                            return composers;
                        }
                    }
                }
                return new string[0];
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Composers = value;
                }
            }
        }

        public override string[] ComposersSort
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string[] composersSort = comment.ComposersSort;
                        if ((composersSort != null) && (composersSort.Length > 0))
                        {
                            return composersSort;
                        }
                    }
                }
                return new string[0];
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].ComposersSort = value;
                }
            }
        }

        public override string Conductor
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string conductor = comment.Conductor;
                        if ((conductor != null) && (conductor.Length > 0))
                        {
                            return conductor;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Conductor = value;
                }
            }
        }

        public override string Copyright
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string copyright = comment.Copyright;
                        if ((copyright != null) && (copyright.Length > 0))
                        {
                            return copyright;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Copyright = value;
                }
            }
        }

        public override uint Disc
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if ((comment != null) && (comment.Disc != 0))
                    {
                        return comment.Disc;
                    }
                }
                return 0;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Disc = value;
                }
            }
        }

        public override uint DiscCount
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if ((comment != null) && (comment.DiscCount != 0))
                    {
                        return comment.DiscCount;
                    }
                }
                return 0;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].DiscCount = value;
                }
            }
        }

        public override string[] Genres
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string[] genres = comment.Genres;
                        if ((genres != null) && (genres.Length > 0))
                        {
                            return genres;
                        }
                    }
                }
                return new string[0];
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Genres = value;
                }
            }
        }

        public override string Grouping
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string grouping = comment.Grouping;
                        if ((grouping != null) && (grouping.Length > 0))
                        {
                            return grouping;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Grouping = value;
                }
            }
        }

        public override bool IsEmpty
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (!comment.IsEmpty)
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
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string lyrics = comment.Lyrics;
                        if ((lyrics != null) && (lyrics.Length > 0))
                        {
                            return lyrics;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Lyrics = value;
                }
            }
        }

        public override string MusicBrainzArtistId
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string musicBrainzArtistId = comment.MusicBrainzArtistId;
                        if ((musicBrainzArtistId != null) && (musicBrainzArtistId.Length > 0))
                        {
                            return musicBrainzArtistId;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].MusicBrainzArtistId = value;
                }
            }
        }

        public override string MusicBrainzDiscId
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string musicBrainzDiscId = comment.MusicBrainzDiscId;
                        if ((musicBrainzDiscId != null) && (musicBrainzDiscId.Length > 0))
                        {
                            return musicBrainzDiscId;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].MusicBrainzDiscId = value;
                }
            }
        }

        public override string MusicBrainzReleaseArtistId
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string musicBrainzReleaseArtistId = comment.MusicBrainzReleaseArtistId;
                        if ((musicBrainzReleaseArtistId != null) && (musicBrainzReleaseArtistId.Length > 0))
                        {
                            return musicBrainzReleaseArtistId;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].MusicBrainzReleaseArtistId = value;
                }
            }
        }

        public override string MusicBrainzReleaseCountry
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string musicBrainzReleaseCountry = comment.MusicBrainzReleaseCountry;
                        if ((musicBrainzReleaseCountry != null) && (musicBrainzReleaseCountry.Length > 0))
                        {
                            return musicBrainzReleaseCountry;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].MusicBrainzReleaseCountry = value;
                }
            }
        }

        public override string MusicBrainzReleaseId
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string musicBrainzReleaseId = comment.MusicBrainzReleaseId;
                        if ((musicBrainzReleaseId != null) && (musicBrainzReleaseId.Length > 0))
                        {
                            return musicBrainzReleaseId;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].MusicBrainzReleaseId = value;
                }
            }
        }

        public override string MusicBrainzReleaseStatus
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string musicBrainzReleaseStatus = comment.MusicBrainzReleaseStatus;
                        if ((musicBrainzReleaseStatus != null) && (musicBrainzReleaseStatus.Length > 0))
                        {
                            return musicBrainzReleaseStatus;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].MusicBrainzReleaseStatus = value;
                }
            }
        }

        public override string MusicBrainzReleaseType
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string musicBrainzReleaseType = comment.MusicBrainzReleaseType;
                        if ((musicBrainzReleaseType != null) && (musicBrainzReleaseType.Length > 0))
                        {
                            return musicBrainzReleaseType;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].MusicBrainzReleaseType = value;
                }
            }
        }

        public override string MusicBrainzTrackId
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string musicBrainzTrackId = comment.MusicBrainzTrackId;
                        if ((musicBrainzTrackId != null) && (musicBrainzTrackId.Length > 0))
                        {
                            return musicBrainzTrackId;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].MusicBrainzTrackId = value;
                }
            }
        }

        public override string MusicIpId
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string musicIpId = comment.MusicIpId;
                        if ((musicIpId != null) && (musicIpId.Length > 0))
                        {
                            return musicIpId;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].MusicIpId = value;
                }
            }
        }

        public override string[] Performers
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string[] performers = comment.Performers;
                        if ((performers != null) && (performers.Length > 0))
                        {
                            return performers;
                        }
                    }
                }
                return new string[0];
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Performers = value;
                }
            }
        }

        public override string[] PerformersSort
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string[] performersSort = comment.PerformersSort;
                        if ((performersSort != null) && (performersSort.Length > 0))
                        {
                            return performersSort;
                        }
                    }
                }
                return new string[0];
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].PerformersSort = value;
                }
            }
        }

        public override IPicture[] Pictures
        {
            get
            {
                IPicture[] pictures = new IPicture[0];
                foreach (XiphComment comment in this.tags)
                {
                    if ((comment != null) && (pictures.Length == 0))
                    {
                        pictures = comment.Pictures;
                    }
                }
                return pictures;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Pictures = value;
                }
            }
        }

        public override TagLib.TagTypes TagTypes
        {
            get
            {
                TagLib.TagTypes none = TagLib.TagTypes.None;
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        none |= comment.TagTypes;
                    }
                }
                return none;
            }
        }

        public override string Title
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string title = comment.Title;
                        if ((title != null) && (title.Length > 0))
                        {
                            return title;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Title = value;
                }
            }
        }

        public override string TitleSort
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if (comment != null)
                    {
                        string titleSort = comment.TitleSort;
                        if ((titleSort != null) && (titleSort.Length > 0))
                        {
                            return titleSort;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].TitleSort = value;
                }
            }
        }

        public override uint Track
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if ((comment != null) && (comment.Track != 0))
                    {
                        return comment.Track;
                    }
                }
                return 0;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Track = value;
                }
            }
        }

        public override uint TrackCount
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if ((comment != null) && (comment.TrackCount != 0))
                    {
                        return comment.TrackCount;
                    }
                }
                return 0;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].TrackCount = value;
                }
            }
        }

        public override uint Year
        {
            get
            {
                foreach (XiphComment comment in this.tags)
                {
                    if ((comment != null) && (comment.Year != 0))
                    {
                        return comment.Year;
                    }
                }
                return 0;
            }
            set
            {
                if (this.tags.Count > 0)
                {
                    this.tags[0].Year = value;
                }
            }
        }
    }
}

