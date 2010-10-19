namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class PopularimeterFrame : Frame
    {
        private ulong play_count;
        private byte rating;
        private string user;

        public PopularimeterFrame(string user) : base(FrameType.POPM, 4)
        {
            this.user = string.Empty;
            this.User = user;
        }

        public PopularimeterFrame(ByteVector data, byte version) : base(data, version)
        {
            this.user = string.Empty;
            base.SetData(data, 0, version, true);
        }

        protected internal PopularimeterFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(header)
        {
            this.user = string.Empty;
            base.SetData(data, offset, version, false);
        }

        public override Frame Clone()
        {
            return new PopularimeterFrame(this.user) { play_count = this.play_count, rating = this.rating };
        }

        public static PopularimeterFrame Get(TagLib.Id3v2.Tag tag, string user, bool create)
        {
            PopularimeterFrame frame;
            IEnumerator<Frame> enumerator = tag.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    frame = current as PopularimeterFrame;
                    if ((frame != null) && frame.user.Equals(user))
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
            frame = new PopularimeterFrame(user);
            tag.AddFrame(frame);
            return frame;
        }

        protected override void ParseFields(ByteVector data, byte version)
        {
            ByteVector pattern = ByteVector.TextDelimiter(StringType.Latin1);
            int count = data.Find(pattern);
            if (count < 0)
            {
                throw new CorruptFileException("Popularimeter frame does not contain a text delimiter");
            }
            this.user = data.ToString(StringType.Latin1, 0, count);
            this.rating = data[count + 1];
            this.play_count = data.Mid(count + 2).ToULong();
        }

        protected override ByteVector RenderFields(byte version)
        {
            ByteVector vector = ByteVector.FromULong(this.play_count);
            while ((vector.Count > 0) && (vector[0] == 0))
            {
                vector.RemoveAt(0);
            }
            vector.Insert(0, this.rating);
            vector.Insert(0, (byte) 0);
            vector.Insert(0, ByteVector.FromString(this.user, StringType.Latin1));
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

        public byte Rating
        {
            get
            {
                return this.rating;
            }
            set
            {
                this.rating = value;
            }
        }

        public string User
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = (value == null) ? string.Empty : value;
            }
        }
    }
}

