namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class PlayCountFrame : Frame
    {
        private ulong play_count;

        public PlayCountFrame() : base(FrameType.PCNT, 4)
        {
        }

        public PlayCountFrame(ByteVector data, byte version) : base(data, version)
        {
            base.SetData(data, 0, version, true);
        }

        protected internal PlayCountFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(header)
        {
            base.SetData(data, offset, version, false);
        }

        public override Frame Clone()
        {
            return new PlayCountFrame { play_count = this.play_count };
        }

        public static PlayCountFrame Get(TagLib.Id3v2.Tag tag, bool create)
        {
            PlayCountFrame frame;
            IEnumerator<Frame> enumerator = tag.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    frame = current as PlayCountFrame;
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
            frame = new PlayCountFrame();
            tag.AddFrame(frame);
            return frame;
        }

        protected override void ParseFields(ByteVector data, byte version)
        {
            this.play_count = data.ToULong();
        }

        protected override ByteVector RenderFields(byte version)
        {
            ByteVector vector = ByteVector.FromULong(this.play_count);
            while ((vector.Count > 4) && (vector[0] == 0))
            {
                vector.RemoveAt(0);
            }
            return vector;
        }

        public ulong PlayCount
        {
            get
            {
                return this.play_count;
            }
            set
            {
                this.play_count = value;
            }
        }
    }
}

