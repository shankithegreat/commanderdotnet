namespace TagLib.Id3v2
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct FrameHeader
    {
        private ReadOnlyByteVector frame_id;
        private uint frame_size;
        private FrameFlags flags;
        private static readonly ReadOnlyByteVector[,] version2_frames;
        private static readonly ReadOnlyByteVector[,] version3_frames;
        public FrameHeader(ByteVector data, byte version)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.flags = FrameFlags.None;
            this.frame_size = 0;
            if ((version < 2) || (version > 4))
            {
                throw new CorruptFileException("Unsupported tag version.");
            }
            if (data.Count < ((version != 2) ? 4 : 3))
            {
                throw new CorruptFileException("Data must contain at least a frame ID.");
            }
            switch (version)
            {
                case 2:
                    this.frame_id = ConvertId(data.Mid(0, 3), version, false);
                    if (data.Count >= 6)
                    {
                        this.frame_size = data.Mid(3, 3).ToUInt();
                        return;
                    }
                    return;

                case 3:
                    this.frame_id = ConvertId(data.Mid(0, 4), version, false);
                    if (data.Count >= 10)
                    {
                        this.frame_size = data.Mid(4, 4).ToUInt();
                        this.flags = (FrameFlags) ((ushort) ((((data[8] << 7) & 0x7000) | ((data[9] >> 4) & 12)) | ((data[9] << 1) & 0x40)));
                        return;
                    }
                    return;

                case 4:
                    this.frame_id = new ReadOnlyByteVector(data.Mid(0, 4));
                    if (data.Count >= 10)
                    {
                        this.frame_size = SynchData.ToUInt(data.Mid(4, 4));
                        this.flags = (FrameFlags) data.Mid(8, 2).ToUShort();
                        return;
                    }
                    return;
            }
            throw new CorruptFileException("Unsupported tag version.");
        }

        static FrameHeader()
        {
            ReadOnlyByteVector[] vectorArray1 = new ReadOnlyByteVector[,] { 
                { "BUF", "RBUF" }, { "CNT", "PCNT" }, { "COM", "COMM" }, { "CRA", "AENC" }, { "ETC", "ETCO" }, { "GEO", "GEOB" }, { "IPL", "TIPL" }, { "MCI", "MCDI" }, { "MLL", "MLLT" }, { "PIC", "APIC" }, { "POP", "POPM" }, { "REV", "RVRB" }, { "SLT", "SYLT" }, { "STC", "SYTC" }, { "TAL", "TALB" }, { "TBP", "TBPM" }, 
                { "TCM", "TCOM" }, { "TCO", "TCON" }, { "TCP", "TCMP" }, { "TCR", "TCOP" }, { "TDA", "TDAT" }, { "TIM", "TIME" }, { "TDY", "TDLY" }, { "TEN", "TENC" }, { "TFT", "TFLT" }, { "TKE", "TKEY" }, { "TLA", "TLAN" }, { "TLE", "TLEN" }, { "TMT", "TMED" }, { "TOA", "TOAL" }, { "TOF", "TOFN" }, { "TOL", "TOLY" }, 
                { "TOR", "TDOR" }, { "TOT", "TOAL" }, { "TP1", "TPE1" }, { "TP2", "TPE2" }, { "TP3", "TPE3" }, { "TP4", "TPE4" }, { "TPA", "TPOS" }, { "TPB", "TPUB" }, { "TRC", "TSRC" }, { "TRK", "TRCK" }, { "TSS", "TSSE" }, { "TT1", "TIT1" }, { "TT2", "TIT2" }, { "TT3", "TIT3" }, { "TXT", "TOLY" }, { "TXX", "TXXX" }, 
                { "TYE", "TDRC" }, { "UFI", "UFID" }, { "ULT", "USLT" }, { "WAF", "WOAF" }, { "WAR", "WOAR" }, { "WAS", "WOAS" }, { "WCM", "WCOM" }, { "WCP", "WCOP" }, { "WPB", "WPUB" }, { "WXX", "WXXX" }, { "XRV", "RVA2" }
             };
            version2_frames = vectorArray1;
            ReadOnlyByteVector[] vectorArray2 = new ReadOnlyByteVector[,] { { "TORY", "TDOR" }, { "TYER", "TDRC" }, { "XRVA", "RVA2" } };
            version3_frames = vectorArray2;
        }

        public ReadOnlyByteVector FrameId
        {
            get
            {
                return this.frame_id;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.frame_id = (value.Count != 4) ? new ReadOnlyByteVector(value.Mid(0, 4)) : value;
            }
        }
        public uint FrameSize
        {
            get
            {
                return this.frame_size;
            }
            set
            {
                this.frame_size = value;
            }
        }
        public FrameFlags Flags
        {
            get
            {
                return this.flags;
            }
            set
            {
                if (((ushort) (value & (FrameFlags.Compression | FrameFlags.Encryption))) != 0)
                {
                    throw new ArgumentException("Encryption and compression are not supported.", "value");
                }
                this.flags = value;
            }
        }
        public ByteVector Render(byte version)
        {
            ByteVector vector = new ByteVector();
            ByteVector data = ConvertId(this.frame_id, version, true);
            if (data == null)
            {
                throw new NotImplementedException();
            }
            switch (version)
            {
                case 2:
                    vector.Add(data);
                    vector.Add(ByteVector.FromUInt(this.frame_size).Mid(1, 3));
                    return vector;

                case 3:
                {
                    ushort num = (ushort) ((((((ushort) this.flags) << 1) & 0xe000) | ((((ushort) this.flags) << 4) & 0xc0)) | ((((ushort) this.flags) >> 1) & 0x20));
                    vector.Add(data);
                    vector.Add(ByteVector.FromUInt(this.frame_size));
                    vector.Add(ByteVector.FromUShort(num));
                    return vector;
                }
                case 4:
                    vector.Add(data);
                    vector.Add(SynchData.FromUInt(this.frame_size));
                    vector.Add(ByteVector.FromUShort((ushort) this.flags));
                    return vector;
            }
            throw new NotImplementedException("Unsupported tag version.");
        }

        public static uint Size(byte version)
        {
            return ((version >= 3) ? 10 : 6);
        }

        private static ReadOnlyByteVector ConvertId(ByteVector id, byte version, bool toVersion)
        {
            if (version >= 4)
            {
                ReadOnlyByteVector vector = id as ReadOnlyByteVector;
                return ((vector == null) ? new ReadOnlyByteVector(id) : vector);
            }
            if ((id == null) || (version < 2))
            {
                return null;
            }
            if (!toVersion && (((id == FrameType.EQUA) || (id == FrameType.RVAD)) || ((id == FrameType.TRDA) || (id == FrameType.TSIZ))))
            {
                return null;
            }
            if (version == 2)
            {
                for (int i = 0; i < version2_frames.GetLength(0); i++)
                {
                    if (version2_frames[i, !toVersion ? 0 : 1].Equals(id))
                    {
                        return version2_frames[i, !toVersion ? 1 : 0];
                    }
                }
            }
            if (version == 3)
            {
                for (int j = 0; j < version3_frames.GetLength(0); j++)
                {
                    if (version3_frames[j, !toVersion ? 0 : 1].Equals(id))
                    {
                        return version3_frames[j, !toVersion ? 1 : 0];
                    }
                }
            }
            if (((id.Count != 4) && (version > 2)) || ((id.Count != 3) && (version == 2)))
            {
                return null;
            }
            return (!(id is ReadOnlyByteVector) ? new ReadOnlyByteVector(id) : (id as ReadOnlyByteVector));
        }
    }
}

