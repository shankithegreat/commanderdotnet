namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class MusicCdIdentifierFrame : Frame
    {
        private ByteVector field_data;

        public MusicCdIdentifierFrame() : base(FrameType.MCDI, 4)
        {
        }

        public MusicCdIdentifierFrame(ByteVector data, byte version) : base(data, version)
        {
            base.SetData(data, 0, version, true);
        }

        protected internal MusicCdIdentifierFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(header)
        {
            base.SetData(data, offset, version, false);
        }

        public override Frame Clone()
        {
            MusicCdIdentifierFrame frame = new MusicCdIdentifierFrame();
            if (this.field_data != null)
            {
                frame.field_data = new ByteVector(this.field_data);
            }
            return frame;
        }

        public static MusicCdIdentifierFrame Get(TagLib.Id3v2.Tag tag, bool create)
        {
            MusicCdIdentifierFrame frame;
            IEnumerator<Frame> enumerator = tag.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    frame = current as MusicCdIdentifierFrame;
                    if (frame != null)
                    {
                        return frame;
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
            if (!create)
            {
                return null;
            }
            frame = new MusicCdIdentifierFrame();
            tag.AddFrame(frame);
            return frame;
        }

        protected override void ParseFields(ByteVector data, byte version)
        {
            this.field_data = data;
        }

        protected override ByteVector RenderFields(byte version)
        {
            return ((this.field_data == null) ? new ByteVector() : this.field_data);
        }

        public ByteVector Data
        {
            get
            {
                return this.field_data;
            }
            set
            {
                this.field_data = value;
            }
        }
    }
}

