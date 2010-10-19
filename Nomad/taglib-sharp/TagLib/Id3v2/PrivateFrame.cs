namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class PrivateFrame : Frame
    {
        private ByteVector data;
        private string owner;

        public PrivateFrame(string owner) : this(owner, null)
        {
        }

        public PrivateFrame(string owner, ByteVector data) : base(FrameType.PRIV, 4)
        {
            this.owner = owner;
            this.data = data;
        }

        public PrivateFrame(ByteVector data, byte version) : base(data, version)
        {
            base.SetData(data, 0, version, true);
        }

        protected internal PrivateFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(header)
        {
            base.SetData(data, offset, version, false);
        }

        public override Frame Clone()
        {
            PrivateFrame frame = new PrivateFrame(this.owner);
            if (this.data != null)
            {
                frame.data = new ByteVector(this.data);
            }
            return frame;
        }

        public static PrivateFrame Get(TagLib.Id3v2.Tag tag, string owner, bool create)
        {
            PrivateFrame frame;
            IEnumerator<Frame> enumerator = tag.GetFrames(FrameType.PRIV).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    frame = current as PrivateFrame;
                    if ((frame != null) && (frame.Owner == owner))
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
            frame = new PrivateFrame(owner);
            tag.AddFrame(frame);
            return frame;
        }

        protected override void ParseFields(ByteVector data, byte version)
        {
            if (data.Count < 1)
            {
                throw new CorruptFileException("A private frame must contain at least 1 byte.");
            }
            ByteVectorCollection vectors = ByteVectorCollection.Split(data, ByteVector.TextDelimiter(StringType.Latin1), 1, 2);
            if (vectors.Count == 2)
            {
                this.owner = vectors[0].ToString(StringType.Latin1);
                this.data = vectors[1];
            }
        }

        protected override ByteVector RenderFields(byte version)
        {
            if (version < 3)
            {
                throw new NotImplementedException();
            }
            ByteVector vector = new ByteVector();
            vector.Add(ByteVector.FromString(this.owner, StringType.Latin1));
            vector.Add(ByteVector.TextDelimiter(StringType.Latin1));
            vector.Add(this.data);
            return vector;
        }

        public string Owner
        {
            get
            {
                return this.owner;
            }
        }

        public ByteVector PrivateData
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }
    }
}

