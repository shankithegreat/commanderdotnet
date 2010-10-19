namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class UniqueFileIdentifierFrame : Frame
    {
        private ByteVector identifier;
        private string owner;

        public UniqueFileIdentifierFrame(string owner) : this(owner, null)
        {
        }

        public UniqueFileIdentifierFrame(string owner, ByteVector identifier) : base(FrameType.UFID, 4)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            this.owner = owner;
            this.identifier = identifier;
        }

        public UniqueFileIdentifierFrame(ByteVector data, byte version) : base(data, version)
        {
            base.SetData(data, 0, version, true);
        }

        protected internal UniqueFileIdentifierFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(header)
        {
            base.SetData(data, offset, version, false);
        }

        public override Frame Clone()
        {
            UniqueFileIdentifierFrame frame = new UniqueFileIdentifierFrame(this.owner);
            if (this.identifier != null)
            {
                frame.identifier = new ByteVector(this.identifier);
            }
            return frame;
        }

        public static UniqueFileIdentifierFrame Get(TagLib.Id3v2.Tag tag, string owner, bool create)
        {
            UniqueFileIdentifierFrame frame;
            IEnumerator<Frame> enumerator = tag.GetFrames(FrameType.UFID).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    frame = current as UniqueFileIdentifierFrame;
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
            frame = new UniqueFileIdentifierFrame(owner, null);
            tag.AddFrame(frame);
            return frame;
        }

        protected override void ParseFields(ByteVector data, byte version)
        {
            ByteVectorCollection vectors = ByteVectorCollection.Split(data, 0);
            if (vectors.Count == 2)
            {
                this.owner = vectors[0].ToString(StringType.Latin1);
                this.identifier = vectors[1];
            }
        }

        protected override ByteVector RenderFields(byte version)
        {
            return new ByteVector { ByteVector.FromString(this.owner, 0), ByteVector.TextDelimiter(0), this.identifier };
        }

        public ByteVector Identifier
        {
            get
            {
                return this.identifier;
            }
            set
            {
                this.identifier = value;
            }
        }

        public string Owner
        {
            get
            {
                return this.owner;
            }
        }
    }
}

