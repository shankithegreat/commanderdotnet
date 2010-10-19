namespace TagLib.Riff
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class AviHeaderList
    {
        private List<ICodec> codecs = new List<ICodec>();
        private AviHeader header;

        public AviHeaderList(TagLib.File file, long position, int length)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            if ((position < 0L) || (position > (file.Length - length)))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            List list = new List(file, position, length);
            if (!list.ContainsKey("avih"))
            {
                throw new CorruptFileException("Avi header not found.");
            }
            ByteVector data = list["avih"][0];
            if (data.Count != 0x38)
            {
                throw new CorruptFileException("Invalid header length.");
            }
            this.header = new AviHeader(data, 0);
            IEnumerator<ByteVector> enumerator = list["LIST"].GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ByteVector current = enumerator.Current;
                    if (current.StartsWith("strl"))
                    {
                        this.codecs.Add(AviStream.ParseStreamList(current).Codec);
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
        }

        public ICodec[] Codecs
        {
            get
            {
                return this.codecs.ToArray();
            }
        }

        public AviHeader Header
        {
            get
            {
                return this.header;
            }
        }
    }
}

