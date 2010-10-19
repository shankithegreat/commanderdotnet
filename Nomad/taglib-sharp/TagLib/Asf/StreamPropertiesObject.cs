namespace TagLib.Asf
{
    using System;
    using TagLib;
    using TagLib.Riff;

    public class StreamPropertiesObject : TagLib.Asf.Object
    {
        private ByteVector error_correction_data;
        private System.Guid error_correction_type;
        private ushort flags;
        private uint reserved;
        private System.Guid stream_type;
        private ulong time_offset;
        private ByteVector type_specific_data;

        public StreamPropertiesObject(TagLib.Asf.File file, long position) : base(file, position)
        {
            if (!base.Guid.Equals(TagLib.Asf.Guid.AsfStreamPropertiesObject))
            {
                throw new CorruptFileException("Object GUID incorrect.");
            }
            if (base.OriginalSize < 0x4eL)
            {
                throw new CorruptFileException("Object size too small.");
            }
            this.stream_type = file.ReadGuid();
            this.error_correction_type = file.ReadGuid();
            this.time_offset = file.ReadQWord();
            int length = (int) file.ReadDWord();
            int num2 = (int) file.ReadDWord();
            this.flags = file.ReadWord();
            this.reserved = file.ReadDWord();
            this.type_specific_data = file.ReadBlock(length);
            this.error_correction_data = file.ReadBlock(num2);
        }

        public override ByteVector Render()
        {
            ByteVector data = this.stream_type.ToByteArray();
            data.Add(this.error_correction_type.ToByteArray());
            data.Add(TagLib.Asf.Object.RenderQWord(this.time_offset));
            data.Add(TagLib.Asf.Object.RenderDWord((uint) this.type_specific_data.Count));
            data.Add(TagLib.Asf.Object.RenderDWord((uint) this.error_correction_data.Count));
            data.Add(TagLib.Asf.Object.RenderWord(this.flags));
            data.Add(TagLib.Asf.Object.RenderDWord(this.reserved));
            data.Add(this.type_specific_data);
            data.Add(this.error_correction_data);
            return base.Render(data);
        }

        public ICodec Codec
        {
            get
            {
                if (this.stream_type == TagLib.Asf.Guid.AsfAudioMedia)
                {
                    return new WaveFormatEx(this.type_specific_data, 0);
                }
                if (this.stream_type == TagLib.Asf.Guid.AsfVideoMedia)
                {
                    return new BitmapInfoHeader(this.type_specific_data, 11);
                }
                return null;
            }
        }

        public ByteVector ErrorCorrectionData
        {
            get
            {
                return this.error_correction_data;
            }
        }

        public System.Guid ErrorCorrectionType
        {
            get
            {
                return this.error_correction_type;
            }
        }

        public ushort Flags
        {
            get
            {
                return this.flags;
            }
        }

        public System.Guid StreamType
        {
            get
            {
                return this.stream_type;
            }
        }

        public TimeSpan TimeOffset
        {
            get
            {
                return new TimeSpan((long) this.time_offset);
            }
        }

        public ByteVector TypeSpecificData
        {
            get
            {
                return this.type_specific_data;
            }
        }
    }
}

