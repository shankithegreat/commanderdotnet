namespace Nomad.Commons
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Threading;
    using System.Xml.Serialization;

    [Serializable]
    public class HexContentFilter : CustomContentFilter
    {
        private ContentComparision FComparision;
        private byte[] FSequence;

        public HexContentFilter()
        {
            this.FComparision = ContentComparision.Ignore;
        }

        public HexContentFilter(byte[] sequence)
        {
            this.FComparision = ContentComparision.Ignore;
            this.Sequence = sequence;
            this.Comparision = ContentComparision.Contains;
        }

        public HexContentFilter(ContentComparision comparision, byte[] sequence) : this(sequence)
        {
            this.Comparision = comparision;
        }

        public override bool EqualTo(object obj)
        {
            HexContentFilter filter = obj as HexContentFilter;
            return (((filter != null) && (this.FComparision == filter.Comparision)) && ByteArrayHelper.Equals(this.FSequence, filter.Sequence));
        }

        public override bool MatchStream(Stream contentStream, string fileName)
        {
            if (this.FComparision == ContentComparision.Ignore)
            {
                return true;
            }
            bool flag = this.StreamContainsSeqeunce(contentStream);
            if (this.FComparision == ContentComparision.Contains)
            {
                return flag;
            }
            return !flag;
        }

        public bool StreamContainsSeqeunce(Stream contentStream)
        {
            int num;
            if (contentStream == null)
            {
                throw new ArgumentNullException("contentStream");
            }
            if (this.FSequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            if (this.FSequence.Length == 0)
            {
                return false;
            }
            byte[] buffer = new byte[0x40000];
            byte[] buffer2 = new byte[0x40000];
            byte[] buffer3 = buffer;
            byte[] buffer4 = buffer2;
            int offset = 0;
            bool flag = false;
            IAsyncResult asyncResult = contentStream.BeginRead(buffer3, 0, buffer3.Length, null, null);
            do
            {
                if (!base.OnProgress(0))
                {
                    break;
                }
                num = contentStream.EndRead(asyncResult);
                if (num > 0)
                {
                    buffer3 = Interlocked.Exchange<byte[]>(ref buffer4, buffer3);
                    int num3 = offset;
                    offset = Math.Min(this.FSequence.Length - 1, num);
                    asyncResult = contentStream.BeginRead(buffer3, offset, buffer3.Length - offset, null, null);
                    flag = ByteArrayHelper.IndexOf(this.FSequence, buffer4, buffer4.Length) >= 0;
                    Array.Copy(buffer4, (num + num3) - offset, buffer3, 0, offset);
                }
                else
                {
                    asyncResult = null;
                }
            }
            while ((num > 0) && !flag);
            if (asyncResult != null)
            {
                contentStream.EndRead(asyncResult);
            }
            return flag;
        }

        public ContentComparision Comparision
        {
            get
            {
                return this.FComparision;
            }
            set
            {
                this.FComparision = value;
            }
        }

        [XmlIgnore]
        public byte[] Sequence
        {
            get
            {
                return this.FSequence;
            }
            set
            {
                if (this.FSequence != value)
                {
                    this.FSequence = value;
                    if ((this.FSequence == null) || (this.FSequence.Length == 0))
                    {
                        this.Comparision = ContentComparision.Ignore;
                    }
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), XmlElement("Sequence")]
        public string SequenceAsString
        {
            get
            {
                return ByteArrayHelper.ToString(this.FSequence);
            }
            set
            {
                this.Sequence = ByteArrayHelper.Parse(value);
            }
        }
    }
}

