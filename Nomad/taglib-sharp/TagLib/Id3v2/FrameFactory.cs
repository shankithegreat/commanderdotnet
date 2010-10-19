namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using TagLib;

    public static class FrameFactory
    {
        private static List<FrameCreator> frame_creators = new List<FrameCreator>();

        public static void AddFrameCreator(FrameCreator creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException("creator");
            }
            frame_creators.Insert(0, creator);
        }

        public static Frame CreateFrame(ByteVector data, ref int offset, byte version, bool alreadyUnsynched)
        {
            int startIndex = offset;
            FrameHeader header = new FrameHeader(data.Mid(startIndex, (int) FrameHeader.Size(version)), version);
            offset = (int) (offset + (header.FrameSize + FrameHeader.Size(version)));
            if (header.FrameId == null)
            {
                throw new NotImplementedException();
            }
            IEnumerator<byte> enumerator = header.FrameId.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    byte current = enumerator.Current;
                    char ch = (char) current;
                    if (((ch < 'A') || (ch > 'Z')) && ((ch < '1') || (ch > '9')))
                    {
                        return null;
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
            if (alreadyUnsynched)
            {
                header.Flags = (FrameFlags) ((ushort) (((int) header.Flags) & 0xfffd));
            }
            if (header.FrameSize == 0)
            {
                header.Flags = (FrameFlags) ((ushort) (header.Flags | (FrameFlags.None | FrameFlags.TagAlterPreservation)));
                return new UnknownFrame(data, startIndex, header, version);
            }
            if (((ushort) (header.Flags & FrameFlags.Compression)) != 0)
            {
                throw new NotImplementedException();
            }
            if (((ushort) (header.Flags & FrameFlags.Encryption)) != 0)
            {
                throw new NotImplementedException();
            }
            foreach (FrameCreator creator in frame_creators)
            {
                Frame frame = creator(data, startIndex, header, version);
                if (frame != null)
                {
                    return frame;
                }
            }
            if (header.FrameId == FrameType.TXXX)
            {
                return new UserTextInformationFrame(data, startIndex, header, version);
            }
            if (header.FrameId[0] == 0x54)
            {
                return new TextInformationFrame(data, startIndex, header, version);
            }
            if (header.FrameId == FrameType.UFID)
            {
                return new UniqueFileIdentifierFrame(data, startIndex, header, version);
            }
            if (header.FrameId == FrameType.MCDI)
            {
                return new MusicCdIdentifierFrame(data, startIndex, header, version);
            }
            if (header.FrameId == FrameType.USLT)
            {
                return new UnsynchronisedLyricsFrame(data, startIndex, header, version);
            }
            if (header.FrameId == FrameType.SYLT)
            {
                return new SynchronisedLyricsFrame(data, startIndex, header, version);
            }
            if (header.FrameId == FrameType.COMM)
            {
                return new CommentsFrame(data, startIndex, header, version);
            }
            if (header.FrameId == FrameType.RVA2)
            {
                return new RelativeVolumeFrame(data, startIndex, header, version);
            }
            if (header.FrameId == FrameType.APIC)
            {
                return new AttachedPictureFrame(data, startIndex, header, version);
            }
            if (header.FrameId == FrameType.GEOB)
            {
                return new GeneralEncapsulatedObjectFrame(data, startIndex, header, version);
            }
            if (header.FrameId == FrameType.PCNT)
            {
                return new PlayCountFrame(data, startIndex, header, version);
            }
            if (header.FrameId == FrameType.POPM)
            {
                return new PopularimeterFrame(data, startIndex, header, version);
            }
            if (header.FrameId == FrameType.USER)
            {
                return new TermsOfUseFrame(data, startIndex, header, version);
            }
            if (header.FrameId == FrameType.PRIV)
            {
                return new PrivateFrame(data, startIndex, header, version);
            }
            return new UnknownFrame(data, startIndex, header, version);
        }

        public delegate Frame FrameCreator(ByteVector data, int offset, FrameHeader header, byte version);
    }
}

