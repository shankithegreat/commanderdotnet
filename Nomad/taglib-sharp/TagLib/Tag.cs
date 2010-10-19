namespace TagLib
{
    using System;

    public abstract class Tag
    {
        protected Tag()
        {
        }

        public abstract void Clear();
        public virtual void CopyTo(Tag target, bool overwrite)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (overwrite || IsNullOrLikeEmpty(target.Title))
            {
                target.Title = this.Title;
            }
            if (overwrite || IsNullOrLikeEmpty(target.AlbumArtists))
            {
                target.AlbumArtists = this.AlbumArtists;
            }
            if (overwrite || IsNullOrLikeEmpty(target.Performers))
            {
                target.Performers = this.Performers;
            }
            if (overwrite || IsNullOrLikeEmpty(target.Composers))
            {
                target.Composers = this.Composers;
            }
            if (overwrite || IsNullOrLikeEmpty(target.Album))
            {
                target.Album = this.Album;
            }
            if (overwrite || IsNullOrLikeEmpty(target.Comment))
            {
                target.Comment = this.Comment;
            }
            if (overwrite || IsNullOrLikeEmpty(target.Genres))
            {
                target.Genres = this.Genres;
            }
            if (overwrite || (target.Year == 0))
            {
                target.Year = this.Year;
            }
            if (overwrite || (target.Track == 0))
            {
                target.Track = this.Track;
            }
            if (overwrite || (target.TrackCount == 0))
            {
                target.TrackCount = this.TrackCount;
            }
            if (overwrite || (target.Disc == 0))
            {
                target.Disc = this.Disc;
            }
            if (overwrite || (target.DiscCount == 0))
            {
                target.DiscCount = this.DiscCount;
            }
            if (overwrite || (target.BeatsPerMinute == 0))
            {
                target.BeatsPerMinute = this.BeatsPerMinute;
            }
            if (overwrite || IsNullOrLikeEmpty(target.Grouping))
            {
                target.Grouping = this.Grouping;
            }
            if (overwrite || IsNullOrLikeEmpty(target.Conductor))
            {
                target.Conductor = this.Conductor;
            }
            if (overwrite || IsNullOrLikeEmpty(target.Copyright))
            {
                target.Copyright = this.Copyright;
            }
        }

        [Obsolete("Use Tag.CopyTo(Tag,bool)")]
        public static void Duplicate(Tag source, Tag target, bool overwrite)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            source.CopyTo(target, overwrite);
        }

        private static string FirstInGroup(string[] group)
        {
            return (((group != null) && (group.Length != 0)) ? group[0] : null);
        }

        private static bool IsNullOrLikeEmpty(string[] value)
        {
            if (value != null)
            {
                foreach (string str in value)
                {
                    if (!IsNullOrLikeEmpty(str))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool IsNullOrLikeEmpty(string value)
        {
            return ((value == null) || (value.Trim().Length == 0));
        }

        private static string JoinGroup(string[] group)
        {
            if (group == null)
            {
                return null;
            }
            return string.Join("; ", group);
        }

        public virtual string Album
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string[] AlbumArtists
        {
            get
            {
                return new string[0];
            }
            set
            {
            }
        }

        public virtual string[] AlbumArtistsSort
        {
            get
            {
                return new string[0];
            }
            set
            {
            }
        }

        public virtual string AlbumSort
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string AmazonId
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        [Obsolete("For album artists use AlbumArtists. For track artists, use Performers")]
        public virtual string[] Artists
        {
            get
            {
                return this.Performers;
            }
            set
            {
                this.Performers = value;
            }
        }

        public virtual uint BeatsPerMinute
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public virtual string Comment
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string[] Composers
        {
            get
            {
                return new string[0];
            }
            set
            {
            }
        }

        public virtual string[] ComposersSort
        {
            get
            {
                return new string[0];
            }
            set
            {
            }
        }

        public virtual string Conductor
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string Copyright
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual uint Disc
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public virtual uint DiscCount
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public string FirstAlbumArtist
        {
            get
            {
                return FirstInGroup(this.AlbumArtists);
            }
        }

        public string FirstAlbumArtistSort
        {
            get
            {
                return FirstInGroup(this.AlbumArtistsSort);
            }
        }

        [Obsolete("For album artists use FirstAlbumArtist. For track artists, use FirstPerformer")]
        public string FirstArtist
        {
            get
            {
                return this.FirstPerformer;
            }
        }

        public string FirstComposer
        {
            get
            {
                return FirstInGroup(this.Composers);
            }
        }

        public string FirstComposerSort
        {
            get
            {
                return FirstInGroup(this.ComposersSort);
            }
        }

        public string FirstGenre
        {
            get
            {
                return FirstInGroup(this.Genres);
            }
        }

        public string FirstPerformer
        {
            get
            {
                return FirstInGroup(this.Performers);
            }
        }

        public string FirstPerformerSort
        {
            get
            {
                return FirstInGroup(this.PerformersSort);
            }
        }

        public virtual string[] Genres
        {
            get
            {
                return new string[0];
            }
            set
            {
            }
        }

        public virtual string Grouping
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual bool IsEmpty
        {
            get
            {
                return (((((IsNullOrLikeEmpty(this.Title) && IsNullOrLikeEmpty(this.Grouping)) && (IsNullOrLikeEmpty(this.AlbumArtists) && IsNullOrLikeEmpty(this.Performers))) && ((IsNullOrLikeEmpty(this.Composers) && IsNullOrLikeEmpty(this.Conductor)) && (IsNullOrLikeEmpty(this.Copyright) && IsNullOrLikeEmpty(this.Album)))) && (((IsNullOrLikeEmpty(this.Comment) && IsNullOrLikeEmpty(this.Genres)) && ((this.Year == 0) && (this.BeatsPerMinute == 0))) && (((this.Track == 0) && (this.TrackCount == 0)) && (this.Disc == 0)))) && (this.DiscCount == 0));
            }
        }

        public string JoinedAlbumArtists
        {
            get
            {
                return JoinGroup(this.AlbumArtists);
            }
        }

        [Obsolete("For album artists use JoinedAlbumArtists. For track artists, use JoinedPerformers")]
        public string JoinedArtists
        {
            get
            {
                return this.JoinedPerformers;
            }
        }

        public string JoinedComposers
        {
            get
            {
                return JoinGroup(this.Composers);
            }
        }

        public string JoinedGenres
        {
            get
            {
                return JoinGroup(this.Genres);
            }
        }

        public string JoinedPerformers
        {
            get
            {
                return JoinGroup(this.Performers);
            }
        }

        public string JoinedPerformersSort
        {
            get
            {
                return JoinGroup(this.PerformersSort);
            }
        }

        public virtual string Lyrics
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string MusicBrainzArtistId
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string MusicBrainzDiscId
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string MusicBrainzReleaseArtistId
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string MusicBrainzReleaseCountry
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string MusicBrainzReleaseId
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string MusicBrainzReleaseStatus
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string MusicBrainzReleaseType
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string MusicBrainzTrackId
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string MusicIpId
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string[] Performers
        {
            get
            {
                return new string[0];
            }
            set
            {
            }
        }

        public virtual string[] PerformersSort
        {
            get
            {
                return new string[0];
            }
            set
            {
            }
        }

        public virtual IPicture[] Pictures
        {
            get
            {
                return new Picture[0];
            }
            set
            {
            }
        }

        public abstract TagLib.TagTypes TagTypes { get; }

        public virtual string Title
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string TitleSort
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual uint Track
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public virtual uint TrackCount
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public virtual uint Year
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }
    }
}

