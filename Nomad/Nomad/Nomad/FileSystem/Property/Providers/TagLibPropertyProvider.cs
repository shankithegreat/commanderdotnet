namespace Nomad.FileSystem.Property.Providers
{
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using TagLib;
    using TagLib.Asf;
    using TagLib.Flac;
    using TagLib.Id3v2;
    using TagLib.Mpeg;
    using TagLib.Mpeg4;
    using TagLib.MusePack;
    using TagLib.Riff;
    using TagLib.WavPack;

    [Version(1, 0, 5, 20)]
    public class TagLibPropertyProvider : ILocalFilePropertyProvider, ISimplePropertyProvider, IPropertyProvider
    {
        private static VirtualPropertySet FRegisteredProperties;
        private static Size MaxThumbnailSize;
        private static int PropertyAlbumArtist;
        private static int PropertyAlbumTitle;
        private static int PropertyAudioBitrate;
        private static int PropertyAudioChannels;
        private static int PropertyAudioCodec;
        private static int PropertyAudioSampleRate;
        private static int PropertyComposer;
        private static int PropertyGenre;
        private static int PropertyImageHeight;
        private static int PropertyImageSize;
        private static int PropertyImageWidth;
        private static int PropertyMediaDuration;
        private static int PropertyPerformer;
        private static int PropertyRating;
        private static int PropertyTitle;
        private static int PropertyTrackNumber;
        private static int PropertyVideoBitrate;
        private static int PropertyVideoCodec;
        private static int PropertyVideoFrameRate;
        private static int PropertyYear;

        public IGetVirtualProperty AddProperties(FileSystemInfo info)
        {
            Type type;
            if (info == null)
            {
                throw new ArgumentNullException();
            }
            FileInfo fileInfo = info as FileInfo;
            if ((fileInfo == null) || string.IsNullOrEmpty(fileInfo.Extension))
            {
                return null;
            }
            string str = fileInfo.Extension.Substring(1).ToLower(CultureInfo.InvariantCulture);
            if (!FileTypes.AvailableTypes.TryGetValue("taglib/" + str, out type))
            {
                return null;
            }
            return new TagLibPropertyBag(fileInfo, type);
        }

        public VirtualPropertySet GetRegisteredProperties()
        {
            return RegisteredProperties;
        }

        public bool Register(Hashtable options)
        {
            MaxThumbnailSize = PropertyProviderManager.ReadOption<Size>(options, "maxThumbnailSize", new Size(0, 0));
            int groupId = VirtualProperty.RegisterGroup("Image");
            PropertyImageWidth = DefaultProperty.RegisterProperty("ImageWidth", groupId, typeof(int), 4);
            PropertyImageHeight = DefaultProperty.RegisterProperty("ImageHeight", groupId, typeof(int), 4);
            PropertyImageSize = DefaultProperty.RegisterProperty("ImageSize", groupId, typeof(Size), -1, ImageSizeTypeConverter.Default, 0);
            groupId = VirtualProperty.RegisterGroup("Media");
            PropertyMediaDuration = DefaultProperty.RegisterProperty("Duration", groupId, typeof(TimeSpan), -1, DurationTypeConverter.Default, 0);
            groupId = VirtualProperty.RegisterGroup("Audio");
            PropertyAudioBitrate = DefaultProperty.RegisterProperty("AudioBitrate", groupId, typeof(int), -1, BitrateTypeConverter.Default, 0);
            PropertyAudioChannels = DefaultProperty.RegisterProperty("AudioChannels", groupId, typeof(int), -1, AudioChannelsTypeConverter.Default, 0);
            PropertyAudioSampleRate = DefaultProperty.RegisterProperty("AudioSampleRate", groupId, typeof(int), -1, AudioSampleRateTypeConverter.Default, 0);
            PropertyAudioCodec = DefaultProperty.RegisterProperty("AudioCodec", groupId, typeof(string), -1);
            groupId = VirtualProperty.RegisterGroup("Video");
            PropertyVideoBitrate = DefaultProperty.RegisterProperty("VideoBitrate", groupId, typeof(int), -1, BitrateTypeConverter.Default, 0);
            PropertyVideoFrameRate = DefaultProperty.RegisterProperty("VideoFrameRate", groupId, typeof(double), 3);
            PropertyVideoCodec = DefaultProperty.RegisterProperty("VideoCodec", groupId, typeof(string), -1);
            groupId = VirtualProperty.RegisterGroup("Music");
            PropertyAlbumTitle = DefaultProperty.RegisterProperty("AlbumTitle", groupId, typeof(string), -1);
            PropertyAlbumArtist = DefaultProperty.RegisterProperty("AlbumArtist", groupId, typeof(string), -1);
            PropertyComposer = DefaultProperty.RegisterProperty("Composer", groupId, typeof(string), -1);
            PropertyGenre = DefaultProperty.RegisterProperty("Genre", groupId, typeof(string), -1);
            PropertyPerformer = DefaultProperty.RegisterProperty("Performer", groupId, typeof(string), -1);
            PropertyTitle = DefaultProperty.RegisterProperty("Title", groupId, typeof(string), -1);
            PropertyTrackNumber = DefaultProperty.RegisterProperty("TrackNumber", groupId, typeof(uint), 2);
            PropertyYear = DefaultProperty.RegisterProperty("Year", groupId, typeof(uint), 4);
            PropertyRating = DefaultProperty.RegisterProperty("Rating", groupId, typeof(byte), 5, RatingTypeConverter.Default, 0);
            return true;
        }

        private static VirtualPropertySet RegisteredProperties
        {
            get
            {
                if (FRegisteredProperties == null)
                {
                    FRegisteredProperties = new VirtualPropertySet(new int[] { 
                        11, 0x13, 0x15, PropertyImageWidth, PropertyImageHeight, PropertyImageSize, PropertyMediaDuration, PropertyAudioBitrate, PropertyAudioChannels, PropertyAudioSampleRate, PropertyAudioCodec, PropertyVideoBitrate, PropertyVideoFrameRate, PropertyVideoCodec, PropertyAlbumTitle, PropertyAlbumArtist, 
                        PropertyComposer, PropertyGenre, PropertyPerformer, PropertyTitle, PropertyTrackNumber, PropertyYear, PropertyRating
                     }).AsReadOnly();
                }
                return FRegisteredProperties;
            }
        }

        private class FileInfoAbstraction : TagLib.File.IFileAbstraction
        {
            private FileInfo _FileInfo;

            public FileInfoAbstraction(FileInfo fileInfo)
            {
                this._FileInfo = fileInfo;
            }

            public void CloseStream(Stream stream)
            {
                stream.Close();
            }

            public string Name
            {
                get
                {
                    return this._FileInfo.FullName;
                }
            }

            public Stream ReadStream
            {
                get
                {
                    return this._FileInfo.OpenRead();
                }
            }

            public Stream WriteStream
            {
                get
                {
                    return this._FileInfo.OpenWrite();
                }
            }
        }

        private class TagLibPropertyBag : CustomPropertyProvider, ISetVirtualProperty, IGetVirtualProperty, IGetThumbnail
        {
            private FileInfo _FileInfo;
            private Type FFileType;
            private bool HasThumbnail = true;
            private PopularimeterFrame RatingFrame;
            private WeakReference StoredThumbnail;
            private Size StoredThumbnailSize;
            private TagLib.File TagFile;

            public TagLibPropertyBag(FileInfo fileInfo, Type fileType)
            {
                this._FileInfo = fileInfo;
                this.FFileType = fileType;
            }

            public bool CanSetProperty(int property)
            {
                return (property == 11);
            }

            protected override VirtualPropertySet CreateAvailableSet()
            {
                VirtualPropertySet set;
                if (this._FileInfo != null)
                {
                    set = new VirtualPropertySet(TagLibPropertyProvider.RegisteredProperties);
                    if (this.FFileType != typeof(TagLib.Mpeg.File))
                    {
                        set[TagLibPropertyProvider.PropertyVideoBitrate] = false;
                        set[TagLibPropertyProvider.PropertyVideoFrameRate] = false;
                    }
                    bool flag = (((this.FFileType == typeof(TagLib.Flac.File)) || (this.FFileType == typeof(TagLib.MusePack.File))) || (this.FFileType == typeof(AudioFile))) || (this.FFileType == typeof(TagLib.WavPack.File));
                    if (!flag)
                    {
                        string str = this._FileInfo.Extension.ToUpper();
                        if ((this.FFileType == typeof(TagLib.Asf.File)) && (str == ".WMA"))
                        {
                            flag = true;
                        }
                        if ((this.FFileType == typeof(TagLib.Riff.File)) && (str == ".WAV"))
                        {
                            flag = true;
                        }
                        if ((this.FFileType == typeof(TagLib.Mpeg4.File)) && ((str == ".M4A") || (str == ".M4P")))
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        set[TagLibPropertyProvider.PropertyImageWidth] = false;
                        set[TagLibPropertyProvider.PropertyImageHeight] = false;
                        set[TagLibPropertyProvider.PropertyImageSize] = false;
                        set[TagLibPropertyProvider.PropertyVideoCodec] = false;
                    }
                    return set;
                }
                set = new VirtualPropertySet();
                if (this.TagFile != null)
                {
                    if (this.TagFile.Properties != null)
                    {
                        set[TagLibPropertyProvider.PropertyMediaDuration] = true;
                        MediaTypes mediaTypes = this.TagFile.Properties.MediaTypes;
                        if ((mediaTypes & MediaTypes.Audio) > MediaTypes.None)
                        {
                            set[TagLibPropertyProvider.PropertyAudioBitrate] = true;
                            set[TagLibPropertyProvider.PropertyAudioChannels] = true;
                            set[TagLibPropertyProvider.PropertyAudioSampleRate] = true;
                            set[TagLibPropertyProvider.PropertyAudioCodec] = true;
                        }
                        if ((mediaTypes & MediaTypes.Video) > MediaTypes.None)
                        {
                            set[TagLibPropertyProvider.PropertyImageWidth] = true;
                            set[TagLibPropertyProvider.PropertyImageHeight] = true;
                            set[TagLibPropertyProvider.PropertyImageSize] = true;
                            set[TagLibPropertyProvider.PropertyVideoCodec] = true;
                            if (this.GetStructCodec<VideoHeader>(MediaTypes.Video).HasValue)
                            {
                                set[TagLibPropertyProvider.PropertyVideoBitrate] = true;
                                set[TagLibPropertyProvider.PropertyVideoFrameRate] = true;
                            }
                        }
                    }
                    if (!this.TagFile.Tag.IsEmpty)
                    {
                        set[0x15] = ((this.TagFile.Tag.Pictures != null) && (this.TagFile.Tag.Pictures.Length > 0)) && this.HasThumbnail;
                        set[TagLibPropertyProvider.PropertyAlbumTitle] = !string.IsNullOrEmpty(this.TagFile.Tag.Album);
                        set[TagLibPropertyProvider.PropertyAlbumArtist] = !string.IsNullOrEmpty(this.TagFile.Tag.FirstAlbumArtist);
                        set[TagLibPropertyProvider.PropertyComposer] = !string.IsNullOrEmpty(this.TagFile.Tag.FirstComposer);
                        set[TagLibPropertyProvider.PropertyGenre] = !string.IsNullOrEmpty(this.TagFile.Tag.FirstGenre);
                        set[TagLibPropertyProvider.PropertyPerformer] = !string.IsNullOrEmpty(this.TagFile.Tag.FirstPerformer);
                        set[TagLibPropertyProvider.PropertyTitle] = !string.IsNullOrEmpty(this.TagFile.Tag.Title);
                        set[TagLibPropertyProvider.PropertyTrackNumber] = this.TagFile.Tag.Track > 0;
                        set[TagLibPropertyProvider.PropertyYear] = this.TagFile.Tag.Year > 0;
                        set[TagLibPropertyProvider.PropertyRating] = this.RatingFrame != null;
                        set[11] = !string.IsNullOrEmpty(this.TagFile.Tag.Comment);
                        set[0x13] = !string.IsNullOrEmpty(this.TagFile.Tag.Copyright);
                    }
                }
                return set;
            }

            private T GetClassCodec<T>(MediaTypes mediaType) where T: class
            {
                foreach (ICodec codec in this.TagFile.Properties.Codecs)
                {
                    if ((codec != null) && ((codec.MediaTypes & mediaType) != MediaTypes.None))
                    {
                        T local = codec as T;
                        if (local != null)
                        {
                            return local;
                        }
                    }
                }
                return default(T);
            }

            private string GetCodecDescription(MediaTypes mediaType)
            {
                ICodec classCodec = this.GetClassCodec<ICodec>(mediaType);
                return ((classCodec != null) ? classCodec.Description : null);
            }

            private T? GetStructCodec<T>(MediaTypes mediaType) where T: struct
            {
                foreach (ICodec codec in this.TagFile.Properties.Codecs)
                {
                    if (((codec != null) && ((codec.MediaTypes & mediaType) != MediaTypes.None)) && (codec is T))
                    {
                        return new T?((T) codec);
                    }
                }
                return null;
            }

            public Image GetThumbnail(Size thumbSize)
            {
                if (this.HasThumbnail)
                {
                    this.ReadTagFile();
                    return this.ReadThumbnail(ref thumbSize);
                }
                return null;
            }

            private void ReadTagFile()
            {
                if (this._FileInfo == null)
                {
                    return;
                }
                try
                {
                    this.TagFile = TagLib.File.Create(new TagLibPropertyProvider.FileInfoAbstraction(this._FileInfo));
                    if ((this.TagFile.Tag.TagTypes & (TagTypes.None | TagTypes.Id3v2)) > TagTypes.None)
                    {
                        TagLib.Id3v2.Tag tag = this.TagFile.GetTag(TagTypes.None | TagTypes.Id3v2, false) as TagLib.Id3v2.Tag;
                        if (tag != null)
                        {
                            foreach (Frame frame in tag)
                            {
                                this.RatingFrame = frame as PopularimeterFrame;
                                if (this.RatingFrame != null)
                                {
                                    goto Label_00CB;
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                }
            Label_00CB:
                this._FileInfo = null;
                base.ResetAvailableSet();
            }

            private Image ReadThumbnail(ref Size maxThumbnailSize)
            {
                if (this.HasThumbnail)
                {
                    Image target = null;
                    if ((this.StoredThumbnail != null) && this.StoredThumbnail.IsAlive)
                    {
                        target = (Image) this.StoredThumbnail.Target;
                    }
                    if ((target != null) && (this.StoredThumbnailSize == maxThumbnailSize))
                    {
                        return target;
                    }
                    try
                    {
                        this.HasThumbnail = ((this.TagFile != null) && (this.TagFile.Tag.Pictures != null)) && (this.TagFile.Tag.Pictures.Length > 0);
                        if (this.HasThumbnail)
                        {
                            TagLib.IPicture picture = this.TagFile.Tag.Pictures[0];
                            if (picture.MimeType == "image/jpeg")
                            {
                                using (Stream stream = new MemoryStream(picture.Data.Data))
                                {
                                    using (Image image2 = Image.FromStream(stream))
                                    {
                                        if (maxThumbnailSize.IsEmpty || ((image2.Width < maxThumbnailSize.Width) && (image2.Height < maxThumbnailSize.Height)))
                                        {
                                            target = new Bitmap(image2);
                                        }
                                        else
                                        {
                                            target = new Bitmap(image2, ImageHelper.GetThumbnailSize(image2.Size, maxThumbnailSize));
                                        }
                                    }
                                }
                                this.StoredThumbnail = new WeakReference(target);
                                this.StoredThumbnailSize = maxThumbnailSize;
                                return target;
                            }
                            this.HasThumbnail = false;
                        }
                    }
                    catch (Exception exception)
                    {
                        PropertyProviderManager.ProviderTrace.TraceException(TraceEventType.Error, exception);
                        this.HasThumbnail = false;
                    }
                    if (!this.HasThumbnail)
                    {
                        base.ResetAvailableSet();
                    }
                }
                return null;
            }

            public object this[int property]
            {
                get
                {
                    this.ReadTagFile();
                    if (this.TagFile != null)
                    {
                        if (this.TagFile.Properties != null)
                        {
                            if (property == TagLibPropertyProvider.PropertyMediaDuration)
                            {
                                return this.TagFile.Properties.Duration;
                            }
                            MediaTypes mediaTypes = this.TagFile.Properties.MediaTypes;
                            if ((mediaTypes & MediaTypes.Audio) > MediaTypes.None)
                            {
                                if (property == TagLibPropertyProvider.PropertyAudioBitrate)
                                {
                                    return this.TagFile.Properties.AudioBitrate;
                                }
                                if (property == TagLibPropertyProvider.PropertyAudioChannels)
                                {
                                    return this.TagFile.Properties.AudioChannels;
                                }
                                if (property == TagLibPropertyProvider.PropertyAudioSampleRate)
                                {
                                    return this.TagFile.Properties.AudioSampleRate;
                                }
                                if (property == TagLibPropertyProvider.PropertyAudioCodec)
                                {
                                    return this.GetCodecDescription(MediaTypes.Audio);
                                }
                            }
                            if ((mediaTypes & MediaTypes.Video) > MediaTypes.None)
                            {
                                VideoHeader? structCodec;
                                if (property == TagLibPropertyProvider.PropertyImageWidth)
                                {
                                    return this.TagFile.Properties.VideoWidth;
                                }
                                if (property == TagLibPropertyProvider.PropertyImageHeight)
                                {
                                    return this.TagFile.Properties.VideoHeight;
                                }
                                if (property == TagLibPropertyProvider.PropertyImageSize)
                                {
                                    IVideoCodec classCodec = this.GetClassCodec<IVideoCodec>(MediaTypes.Video);
                                    if (classCodec != null)
                                    {
                                        return new Size(classCodec.VideoWidth, classCodec.VideoHeight);
                                    }
                                    return null;
                                }
                                if (property == TagLibPropertyProvider.PropertyVideoBitrate)
                                {
                                    structCodec = this.GetStructCodec<VideoHeader>(MediaTypes.Video);
                                    if (structCodec.HasValue)
                                    {
                                        return structCodec.Value.VideoBitrate;
                                    }
                                    return null;
                                }
                                if (property == TagLibPropertyProvider.PropertyVideoFrameRate)
                                {
                                    structCodec = this.GetStructCodec<VideoHeader>(MediaTypes.Video);
                                    if (structCodec.HasValue)
                                    {
                                        return structCodec.Value.VideoFrameRate;
                                    }
                                    return null;
                                }
                                if (property == TagLibPropertyProvider.PropertyVideoCodec)
                                {
                                    return this.GetCodecDescription(MediaTypes.Video);
                                }
                            }
                        }
                        if (!this.TagFile.Tag.IsEmpty)
                        {
                            if (property == 0x15)
                            {
                                return this.ReadThumbnail(ref TagLibPropertyProvider.MaxThumbnailSize);
                            }
                            if (property == 11)
                            {
                                return this.TagFile.Tag.Comment;
                            }
                            if (property == 0x13)
                            {
                                return this.TagFile.Tag.Copyright;
                            }
                            if (property == TagLibPropertyProvider.PropertyAlbumTitle)
                            {
                                return this.TagFile.Tag.Album;
                            }
                            if (property == TagLibPropertyProvider.PropertyAlbumArtist)
                            {
                                return this.TagFile.Tag.FirstAlbumArtist;
                            }
                            if (property == TagLibPropertyProvider.PropertyComposer)
                            {
                                return this.TagFile.Tag.FirstComposer;
                            }
                            if (property == TagLibPropertyProvider.PropertyGenre)
                            {
                                return this.TagFile.Tag.FirstGenre;
                            }
                            if (property == TagLibPropertyProvider.PropertyPerformer)
                            {
                                return this.TagFile.Tag.FirstPerformer;
                            }
                            if (property == TagLibPropertyProvider.PropertyTitle)
                            {
                                return this.TagFile.Tag.Title;
                            }
                            if (property == TagLibPropertyProvider.PropertyTrackNumber)
                            {
                                return this.TagFile.Tag.Track;
                            }
                            if (property == TagLibPropertyProvider.PropertyYear)
                            {
                                return this.TagFile.Tag.Year;
                            }
                            if ((property == TagLibPropertyProvider.PropertyRating) && (this.RatingFrame != null))
                            {
                                return this.RatingFrame.Rating;
                            }
                        }
                    }
                    return null;
                }
                set
                {
                    if (property != 11)
                    {
                        throw new ArgumentException("Cannot set properties other than description.");
                    }
                    this.ReadTagFile();
                    string comment = this.TagFile.Tag.Comment;
                    this.TagFile.Tag.Comment = (string) value;
                    try
                    {
                        if (this.TagFile.Tag.IsEmpty && (this.TagFile.TagTypesOnDisk > TagTypes.None))
                        {
                            this.TagFile.RemoveTags(~TagTypes.None);
                        }
                        this.TagFile.Save();
                        this._FileInfo = new FileInfo(this.TagFile.Name);
                        this.TagFile = null;
                        this.RatingFrame = null;
                        base.ResetAvailableSet();
                    }
                    catch
                    {
                        this.TagFile.Tag.Comment = comment;
                        throw;
                    }
                }
            }
        }
    }
}

