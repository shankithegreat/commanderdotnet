namespace TagLib
{
    using System;
    using System.Collections.Generic;

    public class CombinedTag : Tag
    {
        private List<Tag> tags;

        public CombinedTag()
        {
            this.tags = new List<Tag>();
        }

        public CombinedTag(params Tag[] tags)
        {
            this.tags = new List<Tag>(tags);
        }

        protected void AddTag(Tag tag)
        {
            this.tags.Add(tag);
        }

        public override void Clear()
        {
            foreach (Tag tag in this.tags)
            {
                tag.Clear();
            }
        }

        protected void ClearTags()
        {
            this.tags.Clear();
        }

        protected void InsertTag(int index, Tag tag)
        {
            this.tags.Insert(index, tag);
        }

        protected void RemoveTag(Tag tag)
        {
            this.tags.Remove(tag);
        }

        public void SetTags(params Tag[] tags)
        {
            this.tags.Clear();
            this.tags.AddRange(tags);
        }

        public override string Album
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string album = tag.Album;
                        if (album != null)
                        {
                            return album;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Album = value;
                    }
                }
            }
        }

        public override string[] AlbumArtists
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string[] albumArtists = tag.AlbumArtists;
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
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.AlbumArtists = value;
                    }
                }
            }
        }

        public override string[] AlbumArtistsSort
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string[] albumArtistsSort = tag.AlbumArtistsSort;
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
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.AlbumArtistsSort = value;
                    }
                }
            }
        }

        public override string AlbumSort
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string albumSort = tag.AlbumSort;
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
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.AlbumSort = value;
                    }
                }
            }
        }

        public override string AmazonId
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string amazonId = tag.AmazonId;
                        if (amazonId != null)
                        {
                            return amazonId;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.AmazonId = value;
                    }
                }
            }
        }

        public override uint BeatsPerMinute
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        uint beatsPerMinute = tag.BeatsPerMinute;
                        if (beatsPerMinute != 0)
                        {
                            return beatsPerMinute;
                        }
                    }
                }
                return 0;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.BeatsPerMinute = value;
                    }
                }
            }
        }

        public override string Comment
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string comment = tag.Comment;
                        if (comment != null)
                        {
                            return comment;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Comment = value;
                    }
                }
            }
        }

        public override string[] Composers
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string[] composers = tag.Composers;
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
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Composers = value;
                    }
                }
            }
        }

        public override string[] ComposersSort
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string[] composersSort = tag.ComposersSort;
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
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.ComposersSort = value;
                    }
                }
            }
        }

        public override string Conductor
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string conductor = tag.Conductor;
                        if (conductor != null)
                        {
                            return conductor;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Conductor = value;
                    }
                }
            }
        }

        public override string Copyright
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string copyright = tag.Copyright;
                        if (copyright != null)
                        {
                            return copyright;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Copyright = value;
                    }
                }
            }
        }

        public override uint Disc
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        uint disc = tag.Disc;
                        if (disc != 0)
                        {
                            return disc;
                        }
                    }
                }
                return 0;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Disc = value;
                    }
                }
            }
        }

        public override uint DiscCount
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        uint discCount = tag.DiscCount;
                        if (discCount != 0)
                        {
                            return discCount;
                        }
                    }
                }
                return 0;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.DiscCount = value;
                    }
                }
            }
        }

        public override string[] Genres
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string[] genres = tag.Genres;
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
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Genres = value;
                    }
                }
            }
        }

        public override string Grouping
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string grouping = tag.Grouping;
                        if (grouping != null)
                        {
                            return grouping;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Grouping = value;
                    }
                }
            }
        }

        public override bool IsEmpty
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag.IsEmpty)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public override string Lyrics
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string lyrics = tag.Lyrics;
                        if (lyrics != null)
                        {
                            return lyrics;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Lyrics = value;
                    }
                }
            }
        }

        public override string MusicBrainzArtistId
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string musicBrainzArtistId = tag.MusicBrainzArtistId;
                        if (musicBrainzArtistId != null)
                        {
                            return musicBrainzArtistId;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.MusicBrainzArtistId = value;
                    }
                }
            }
        }

        public override string MusicBrainzDiscId
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string musicBrainzDiscId = tag.MusicBrainzDiscId;
                        if (musicBrainzDiscId != null)
                        {
                            return musicBrainzDiscId;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.MusicBrainzDiscId = value;
                    }
                }
            }
        }

        public override string MusicBrainzReleaseArtistId
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string musicBrainzReleaseArtistId = tag.MusicBrainzReleaseArtistId;
                        if (musicBrainzReleaseArtistId != null)
                        {
                            return musicBrainzReleaseArtistId;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.MusicBrainzReleaseArtistId = value;
                    }
                }
            }
        }

        public override string MusicBrainzReleaseCountry
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string musicBrainzReleaseCountry = tag.MusicBrainzReleaseCountry;
                        if (musicBrainzReleaseCountry != null)
                        {
                            return musicBrainzReleaseCountry;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.MusicBrainzReleaseCountry = value;
                    }
                }
            }
        }

        public override string MusicBrainzReleaseId
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string musicBrainzReleaseId = tag.MusicBrainzReleaseId;
                        if (musicBrainzReleaseId != null)
                        {
                            return musicBrainzReleaseId;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.MusicBrainzReleaseId = value;
                    }
                }
            }
        }

        public override string MusicBrainzReleaseStatus
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string musicBrainzReleaseStatus = tag.MusicBrainzReleaseStatus;
                        if (musicBrainzReleaseStatus != null)
                        {
                            return musicBrainzReleaseStatus;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.MusicBrainzReleaseStatus = value;
                    }
                }
            }
        }

        public override string MusicBrainzReleaseType
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string musicBrainzReleaseType = tag.MusicBrainzReleaseType;
                        if (musicBrainzReleaseType != null)
                        {
                            return musicBrainzReleaseType;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.MusicBrainzReleaseType = value;
                    }
                }
            }
        }

        public override string MusicBrainzTrackId
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string musicBrainzTrackId = tag.MusicBrainzTrackId;
                        if (musicBrainzTrackId != null)
                        {
                            return musicBrainzTrackId;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.MusicBrainzTrackId = value;
                    }
                }
            }
        }

        public override string MusicIpId
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string musicIpId = tag.MusicIpId;
                        if (musicIpId != null)
                        {
                            return musicIpId;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.MusicIpId = value;
                    }
                }
            }
        }

        public override string[] Performers
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string[] performers = tag.Performers;
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
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Performers = value;
                    }
                }
            }
        }

        public override string[] PerformersSort
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string[] performersSort = tag.PerformersSort;
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
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.PerformersSort = value;
                    }
                }
            }
        }

        public override IPicture[] Pictures
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        IPicture[] pictures = tag.Pictures;
                        if ((pictures != null) && (pictures.Length > 0))
                        {
                            return pictures;
                        }
                    }
                }
                return base.Pictures;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Pictures = value;
                    }
                }
            }
        }

        public virtual Tag[] Tags
        {
            get
            {
                return this.tags.ToArray();
            }
        }

        public override TagLib.TagTypes TagTypes
        {
            get
            {
                TagLib.TagTypes none = TagLib.TagTypes.None;
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        none |= tag.TagTypes;
                    }
                }
                return none;
            }
        }

        public override string Title
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string title = tag.Title;
                        if (title != null)
                        {
                            return title;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Title = value;
                    }
                }
            }
        }

        public override string TitleSort
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        string titleSort = tag.TitleSort;
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
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.TitleSort = value;
                    }
                }
            }
        }

        public override uint Track
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        uint track = tag.Track;
                        if (track != 0)
                        {
                            return track;
                        }
                    }
                }
                return 0;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Track = value;
                    }
                }
            }
        }

        public override uint TrackCount
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        uint trackCount = tag.TrackCount;
                        if (trackCount != 0)
                        {
                            return trackCount;
                        }
                    }
                }
                return 0;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.TrackCount = value;
                    }
                }
            }
        }

        public override uint Year
        {
            get
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        uint year = tag.Year;
                        if (year != 0)
                        {
                            return year;
                        }
                    }
                }
                return 0;
            }
            set
            {
                foreach (Tag tag in this.tags)
                {
                    if (tag != null)
                    {
                        tag.Year = value;
                    }
                }
            }
        }
    }
}

