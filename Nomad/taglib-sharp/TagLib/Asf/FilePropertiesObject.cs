namespace TagLib.Asf
{
    using System;
    using TagLib;

    public class FilePropertiesObject : TagLib.Asf.Object
    {
        private ulong creation_date;
        private ulong data_packets_count;
        private System.Guid file_id;
        private ulong file_size;
        private uint flags;
        private uint maximum_bitrate;
        private uint maximum_data_packet_size;
        private uint minimum_data_packet_size;
        private ulong play_duration;
        private ulong preroll;
        private ulong send_duration;

        public FilePropertiesObject(TagLib.Asf.File file, long position) : base(file, position)
        {
            if (!base.Guid.Equals(TagLib.Asf.Guid.AsfFilePropertiesObject))
            {
                throw new CorruptFileException("Object GUID incorrect.");
            }
            if (base.OriginalSize < 0x68L)
            {
                throw new CorruptFileException("Object size too small.");
            }
            this.file_id = file.ReadGuid();
            this.file_size = file.ReadQWord();
            this.creation_date = file.ReadQWord();
            this.data_packets_count = file.ReadQWord();
            this.send_duration = file.ReadQWord();
            this.play_duration = file.ReadQWord();
            this.preroll = file.ReadQWord();
            this.flags = file.ReadDWord();
            this.minimum_data_packet_size = file.ReadDWord();
            this.maximum_data_packet_size = file.ReadDWord();
            this.maximum_bitrate = file.ReadDWord();
        }

        public override ByteVector Render()
        {
            ByteVector data = this.file_id.ToByteArray();
            data.Add(TagLib.Asf.Object.RenderQWord(this.file_size));
            data.Add(TagLib.Asf.Object.RenderQWord(this.creation_date));
            data.Add(TagLib.Asf.Object.RenderQWord(this.data_packets_count));
            data.Add(TagLib.Asf.Object.RenderQWord(this.send_duration));
            data.Add(TagLib.Asf.Object.RenderQWord(this.play_duration));
            data.Add(TagLib.Asf.Object.RenderQWord(this.preroll));
            data.Add(TagLib.Asf.Object.RenderDWord(this.flags));
            data.Add(TagLib.Asf.Object.RenderDWord(this.minimum_data_packet_size));
            data.Add(TagLib.Asf.Object.RenderDWord(this.maximum_data_packet_size));
            data.Add(TagLib.Asf.Object.RenderDWord(this.maximum_bitrate));
            return base.Render(data);
        }

        public DateTime CreationDate
        {
            get
            {
                return new DateTime((long) this.creation_date);
            }
        }

        public ulong DataPacketsCount
        {
            get
            {
                return this.data_packets_count;
            }
        }

        public System.Guid FileId
        {
            get
            {
                return this.file_id;
            }
        }

        public ulong FileSize
        {
            get
            {
                return this.file_size;
            }
        }

        public uint Flags
        {
            get
            {
                return this.flags;
            }
        }

        public uint MaximumBitrate
        {
            get
            {
                return this.maximum_bitrate;
            }
        }

        public uint MaximumDataPacketSize
        {
            get
            {
                return this.maximum_data_packet_size;
            }
        }

        public uint MinimumDataPacketSize
        {
            get
            {
                return this.minimum_data_packet_size;
            }
        }

        public TimeSpan PlayDuration
        {
            get
            {
                return new TimeSpan((long) this.play_duration);
            }
        }

        public ulong Preroll
        {
            get
            {
                return this.preroll;
            }
        }

        public TimeSpan SendDuration
        {
            get
            {
                return new TimeSpan((long) this.send_duration);
            }
        }
    }
}

