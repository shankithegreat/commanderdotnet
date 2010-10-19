namespace TagLib.Mpeg
{
    using System;
    using TagLib;
    using TagLib.NonContainer;

    [SupportedMimeType("taglib/mpg", "mpg"), SupportedMimeType("taglib/mpeg", "mpeg"), SupportedMimeType("taglib/mpe", "mpe"), SupportedMimeType("taglib/mpv2", "mpv2"), SupportedMimeType("taglib/m2v", "m2v"), SupportedMimeType("video/x-mpg"), SupportedMimeType("video/mpeg")]
    public class File : TagLib.NonContainer.File
    {
        private bool audio_found;
        private AudioHeader audio_header;
        private double end_time;
        private static readonly ByteVector MarkerStart;
        private double? start_time;
        private TagLib.Mpeg.Version version;
        private bool video_found;
        private VideoHeader video_header;

        static File()
        {
            byte[] buffer1 = new byte[3];
            buffer1[2] = 1;
            MarkerStart = buffer1;
        }

        public File(string path) : base(path)
        {
        }

        public File(TagLib.File.IFileAbstraction abstraction) : base(abstraction)
        {
        }

        public File(string path, ReadStyle propertiesStyle) : base(path, propertiesStyle)
        {
        }

        public File(TagLib.File.IFileAbstraction abstraction, ReadStyle propertiesStyle) : base(abstraction, propertiesStyle)
        {
        }

        protected Marker FindMarker(ref long position)
        {
            position = base.Find(MarkerStart, position);
            if (position < 0L)
            {
                throw new CorruptFileException("Marker not found");
            }
            return this.GetMarker(position);
        }

        protected Marker FindMarker(ref long position, Marker marker)
        {
            ByteVector pattern = new ByteVector(MarkerStart) {
                (byte) marker
            };
            position = base.Find(pattern, position);
            if (position < 0L)
            {
                throw new CorruptFileException("Marker not found");
            }
            return this.GetMarker(position);
        }

        protected Marker GetMarker(long position)
        {
            base.Seek(position);
            ByteVector vector = base.ReadBlock(4);
            if ((vector.Count != 4) || !vector.StartsWith(MarkerStart))
            {
                throw new CorruptFileException("Invalid marker at position " + position);
            }
            return (Marker) vector[3];
        }

        public override TagLib.Tag GetTag(TagTypes type, bool create)
        {
            TagLib.Tag tag = (this.Tag as TagLib.NonContainer.Tag).GetTag(type);
            if ((tag != null) || !create)
            {
                return tag;
            }
            switch (type)
            {
                case (TagTypes.None | TagTypes.Id3v1):
                    return base.EndTag.AddTag(type, this.Tag);

                case (TagTypes.None | TagTypes.Id3v2):
                    return base.EndTag.AddTag(type, this.Tag);

                case (TagTypes.None | TagTypes.Ape):
                    return base.EndTag.AddTag(type, this.Tag);
            }
            return null;
        }

        private void ReadAudioPacket(ref long position)
        {
            base.Seek(position + 4L);
            int num = base.ReadBlock(2).ToUShort();
            if (!this.audio_found)
            {
                this.audio_found = AudioHeader.Find(out this.audio_header, this, position + 15L, num - 9);
            }
            position += num;
        }

        protected override void ReadEnd(long end, ReadStyle propertiesStyle)
        {
            this.GetTag(TagTypes.None | TagTypes.Id3v1, true);
            this.GetTag(TagTypes.None | TagTypes.Id3v2, true);
            if ((propertiesStyle != ReadStyle.None) && this.start_time.HasValue)
            {
                this.RFindMarker(ref end, Marker.SystemSyncPacket);
                this.end_time = this.ReadTimestamp(end + 4L);
            }
        }

        protected override Properties ReadProperties(long start, long end, ReadStyle propertiesStyle)
        {
            TimeSpan duration = this.start_time.HasValue ? TimeSpan.FromSeconds(this.end_time - this.start_time.Value) : TimeSpan.Zero;
            return new Properties(duration, new ICodec[] { this.video_header, this.audio_header });
        }

        protected override void ReadStart(long start, ReadStyle propertiesStyle)
        {
            if (propertiesStyle != ReadStyle.None)
            {
                this.FindMarker(ref start, Marker.SystemSyncPacket);
                this.ReadSystemFile(start);
            }
        }

        protected void ReadSystemFile(long position)
        {
            int num = 100;
            for (int i = 0; (i < num) && ((!this.start_time.HasValue || !this.audio_found) || !this.video_found); i++)
            {
                Marker marker2 = this.FindMarker(ref position);
                switch (marker2)
                {
                    case Marker.EndOfStream:
                        return;

                    case Marker.SystemSyncPacket:
                    {
                        this.ReadSystemSyncPacket(ref position);
                        continue;
                    }
                    case Marker.SystemPacket:
                    case Marker.PaddingPacket:
                    {
                        base.Seek(position + 4L);
                        position += base.ReadBlock(2).ToUShort() + 6;
                        continue;
                    }
                    case Marker.AudioPacket:
                    {
                        this.ReadAudioPacket(ref position);
                        continue;
                    }
                }
                if (marker2 == Marker.VideoPacket)
                {
                    this.ReadVideoPacket(ref position);
                }
                else
                {
                    position += 4L;
                }
            }
        }

        private void ReadSystemSyncPacket(ref long position)
        {
            int num = 0;
            base.Seek(position + 4L);
            byte num2 = base.ReadBlock(1)[0];
            if ((num2 & 240) == 0x20)
            {
                this.version = TagLib.Mpeg.Version.Version1;
                num = 12;
            }
            else
            {
                if ((num2 & 0xc0) != 0x40)
                {
                    throw new UnsupportedFormatException("Unknown MPEG version.");
                }
                this.version = TagLib.Mpeg.Version.Version2;
                base.Seek(position + 13L);
                num = 14 + (base.ReadBlock(1)[0] & 7);
            }
            if (!this.start_time.HasValue)
            {
                this.start_time = new double?(this.ReadTimestamp(position + 4L));
            }
            position += num;
        }

        private double ReadTimestamp(long position)
        {
            double num;
            uint num2;
            base.Seek(position);
            if (this.version == TagLib.Mpeg.Version.Version1)
            {
                ByteVector vector = base.ReadBlock(5);
                num = (vector[0] >> 3) & 1;
                num2 = (uint) (((((((vector[0] >> 1) & 3) << 30) | (vector[1] << 0x16)) | ((vector[2] >> 1) << 15)) | (vector[3] << 7)) | (vector[4] << 1));
            }
            else
            {
                ByteVector vector2 = base.ReadBlock(6);
                num = (vector2[0] & 0x20) >> 5;
                num2 = (uint) (((((((((vector2[0] & 0x18) >> 3) << 30) | ((vector2[0] & 3) << 0x1c)) | (vector2[1] << 20)) | ((vector2[2] & 0xf8) << 12)) | ((vector2[2] & 3) << 13)) | (vector2[3] << 5)) | (vector2[4] >> 3));
            }
            return ((((num * 65536.0) * 65536.0) + num2) / 90000.0);
        }

        private void ReadVideoPacket(ref long position)
        {
            base.Seek(position + 4L);
            int num = base.ReadBlock(2).ToUShort();
            long num2 = position + 6L;
            while (!this.video_found && (num2 < (position + num)))
            {
                if (this.FindMarker(ref num2) == Marker.VideoSyncPacket)
                {
                    this.video_header = new VideoHeader(this, num2 + 4L);
                    this.video_found = true;
                }
                else
                {
                    num2 += 6L;
                }
            }
            position += num;
        }

        protected Marker RFindMarker(ref long position, Marker marker)
        {
            ByteVector pattern = new ByteVector(MarkerStart) {
                (byte) marker
            };
            position = base.RFind(pattern, position);
            if (position < 0L)
            {
                throw new CorruptFileException("Marker not found");
            }
            return this.GetMarker(position);
        }
    }
}

