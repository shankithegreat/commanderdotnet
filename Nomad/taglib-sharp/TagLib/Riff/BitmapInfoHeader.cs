namespace TagLib.Riff
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct BitmapInfoHeader : ICodec, IVideoCodec
    {
        private uint size;
        private uint width;
        private uint height;
        private ushort planes;
        private ushort bit_count;
        private ByteVector compression_id;
        private uint size_of_image;
        private uint x_pixels_per_meter;
        private uint y_pixels_per_meter;
        private uint colors_used;
        private uint colors_important;
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map1;
        [Obsolete("Use BitmapInfoHeader(ByteVector,int)")]
        public BitmapInfoHeader(ByteVector data) : this(data, 0)
        {
        }

        public BitmapInfoHeader(ByteVector data, int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if ((offset + 40) > data.Count)
            {
                throw new CorruptFileException("Expected 40 bytes.");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            this.size = data.Mid(offset, 4).ToUInt(false);
            this.width = data.Mid(offset + 4, 4).ToUInt(false);
            this.height = data.Mid(offset + 8, 4).ToUInt(false);
            this.planes = data.Mid(offset + 12, 2).ToUShort(false);
            this.bit_count = data.Mid(offset + 14, 2).ToUShort(false);
            this.compression_id = data.Mid(offset + 0x10, 4);
            this.size_of_image = data.Mid(offset + 20, 4).ToUInt(false);
            this.x_pixels_per_meter = data.Mid(offset + 0x18, 4).ToUInt(false);
            this.y_pixels_per_meter = data.Mid(offset + 0x1c, 4).ToUInt(false);
            this.colors_used = data.Mid(offset + 0x20, 4).ToUInt(false);
            this.colors_important = data.Mid(offset + 0x24, 4).ToUInt(false);
        }

        public uint HeaderSize
        {
            get
            {
                return this.size;
            }
        }
        public ushort Planes
        {
            get
            {
                return this.planes;
            }
        }
        public ushort BitCount
        {
            get
            {
                return this.bit_count;
            }
        }
        public ByteVector CompressionId
        {
            get
            {
                return this.compression_id;
            }
        }
        public uint ImageSize
        {
            get
            {
                return this.size_of_image;
            }
        }
        public uint XPixelsPerMeter
        {
            get
            {
                return this.x_pixels_per_meter;
            }
        }
        public uint YPixelsPerMeter
        {
            get
            {
                return this.y_pixels_per_meter;
            }
        }
        public uint ColorsUsed
        {
            get
            {
                return this.colors_used;
            }
        }
        public uint ImportantColors
        {
            get
            {
                return this.colors_important;
            }
        }
        public int VideoWidth
        {
            get
            {
                return (int) this.width;
            }
        }
        public int VideoHeight
        {
            get
            {
                return (int) this.height;
            }
        }
        public TagLib.MediaTypes MediaTypes
        {
            get
            {
                return TagLib.MediaTypes.Video;
            }
        }
        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.Zero;
            }
        }
        public string Description
        {
            get
            {
                string key = this.CompressionId.ToString(StringType.UTF8).ToUpper(CultureInfo.InvariantCulture);
                if (key != null)
                {
                    int num;
                    if (<>f__switch$map1 == null)
                    {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>(0xd6);
                        dictionary.Add("AEMI", 0);
                        dictionary.Add("ALPH", 1);
                        dictionary.Add("AMPG", 2);
                        dictionary.Add("ANIM", 3);
                        dictionary.Add("AP41", 4);
                        dictionary.Add("AUR2", 5);
                        dictionary.Add("AURA", 6);
                        dictionary.Add("AUVX", 7);
                        dictionary.Add("BT20", 8);
                        dictionary.Add("BTCV", 9);
                        dictionary.Add("CC12", 10);
                        dictionary.Add("CDVC", 11);
                        dictionary.Add("CGDI", 12);
                        dictionary.Add("CHAM", 13);
                        dictionary.Add("CM10", 14);
                        dictionary.Add("CPLA", 15);
                        dictionary.Add("CT10", 0x10);
                        dictionary.Add("CVID", 0x11);
                        dictionary.Add("CWLT", 0x12);
                        dictionary.Add("CYUV", 0x13);
                        dictionary.Add("DIV3", 20);
                        dictionary.Add("MP43", 20);
                        dictionary.Add("DIV4", 0x15);
                        dictionary.Add("DIVX", 0x16);
                        dictionary.Add("DJPG", 0x17);
                        dictionary.Add("DP16", 0x18);
                        dictionary.Add("DP18", 0x19);
                        dictionary.Add("DP26", 0x1a);
                        dictionary.Add("DP28", 0x1b);
                        dictionary.Add("DP96", 0x1c);
                        dictionary.Add("DP98", 0x1d);
                        dictionary.Add("DP9L", 30);
                        dictionary.Add("DUCK", 0x1f);
                        dictionary.Add("DV25", 0x20);
                        dictionary.Add("DV50", 0x21);
                        dictionary.Add("DVE2", 0x22);
                        dictionary.Add("DVH1", 0x23);
                        dictionary.Add("DVHD", 0x24);
                        dictionary.Add("DVNM", 0x25);
                        dictionary.Add("DVSD", 0x26);
                        dictionary.Add("DVSL", 0x27);
                        dictionary.Add("DVX1", 40);
                        dictionary.Add("DVX2", 0x29);
                        dictionary.Add("DVX3", 0x2a);
                        dictionary.Add("DXTC", 0x2b);
                        dictionary.Add("DX50", 0x2c);
                        dictionary.Add("EMWC", 0x2d);
                        dictionary.Add("ETV1", 0x2e);
                        dictionary.Add("ETV2", 0x2e);
                        dictionary.Add("ETVC", 0x2e);
                        dictionary.Add("FLJP", 0x2f);
                        dictionary.Add("FRWA", 0x30);
                        dictionary.Add("FRWD", 0x31);
                        dictionary.Add("FRWT", 50);
                        dictionary.Add("FVF1", 0x33);
                        dictionary.Add("FXT1", 0x34);
                        dictionary.Add("GWLT", 0x35);
                        dictionary.Add("H260", 0x36);
                        dictionary.Add("H261", 0x36);
                        dictionary.Add("H262", 0x36);
                        dictionary.Add("H263", 0x36);
                        dictionary.Add("H264", 0x36);
                        dictionary.Add("H265", 0x36);
                        dictionary.Add("H266", 0x36);
                        dictionary.Add("H267", 0x36);
                        dictionary.Add("H268", 0x36);
                        dictionary.Add("H269", 0x36);
                        dictionary.Add("I263", 0x37);
                        dictionary.Add("I420", 0x38);
                        dictionary.Add("IAN", 0x39);
                        dictionary.Add("ICLB", 0x3a);
                        dictionary.Add("IFO9", 0x3b);
                        dictionary.Add("ILVC", 60);
                        dictionary.Add("ILVR", 0x3d);
                        dictionary.Add("IMAC", 0x3e);
                        dictionary.Add("IPDV", 0x3f);
                        dictionary.Add("IRAW", 0x40);
                        dictionary.Add("ISME", 0x41);
                        dictionary.Add("IUYV", 0x42);
                        dictionary.Add("IV30", 0x43);
                        dictionary.Add("IV31", 0x43);
                        dictionary.Add("IV32", 0x43);
                        dictionary.Add("IV33", 0x43);
                        dictionary.Add("IV34", 0x43);
                        dictionary.Add("IV35", 0x43);
                        dictionary.Add("IV36", 0x43);
                        dictionary.Add("IV37", 0x43);
                        dictionary.Add("IV38", 0x43);
                        dictionary.Add("IV39", 0x43);
                        dictionary.Add("IV40", 0x44);
                        dictionary.Add("IV41", 0x44);
                        dictionary.Add("IV42", 0x44);
                        dictionary.Add("IV43", 0x44);
                        dictionary.Add("IV44", 0x44);
                        dictionary.Add("IV45", 0x44);
                        dictionary.Add("IV46", 0x44);
                        dictionary.Add("IV47", 0x44);
                        dictionary.Add("IV48", 0x44);
                        dictionary.Add("IV49", 0x44);
                        dictionary.Add("IV50", 0x45);
                        dictionary.Add("IY41", 70);
                        dictionary.Add("IYU1", 0x47);
                        dictionary.Add("IYU2", 0x48);
                        dictionary.Add("JPEG", 0x49);
                        dictionary.Add("LEAD", 0x4a);
                        dictionary.Add("LIA1", 0x4b);
                        dictionary.Add("LJPG", 0x4c);
                        dictionary.Add("LSV0", 0x4d);
                        dictionary.Add("LSVC", 0x4e);
                        dictionary.Add("LSVW", 0x4f);
                        dictionary.Add("M101", 80);
                        dictionary.Add("M4S2", 0x51);
                        dictionary.Add("MJPG", 0x52);
                        dictionary.Add("MMES", 0x53);
                        dictionary.Add("MMIF", 0x54);
                        dictionary.Add("MP2A", 0x55);
                        dictionary.Add("MP2T", 0x56);
                        dictionary.Add("MP2V", 0x57);
                        dictionary.Add("MP42", 0x58);
                        dictionary.Add("MP4A", 0x59);
                        dictionary.Add("MP4S", 90);
                        dictionary.Add("MP4T", 0x5b);
                        dictionary.Add("MP4V", 0x5c);
                        dictionary.Add("MPEG", 0x5d);
                        dictionary.Add("MPG4", 0x5e);
                        dictionary.Add("MRCA", 0x5f);
                        dictionary.Add("MRLE", 0x60);
                        dictionary.Add("MSS1", 0x61);
                        dictionary.Add("MSV1", 0x62);
                        dictionary.Add("MSVC", 0x63);
                        dictionary.Add("MV10", 100);
                        dictionary.Add("MV11", 100);
                        dictionary.Add("MV12", 100);
                        dictionary.Add("MV99", 100);
                        dictionary.Add("MVC1", 100);
                        dictionary.Add("MVC2", 100);
                        dictionary.Add("MVC9", 100);
                        dictionary.Add("NTN1", 0x65);
                        dictionary.Add("NY12", 0x66);
                        dictionary.Add("NYUV", 0x67);
                        dictionary.Add("PCL2", 0x68);
                        dictionary.Add("PCLE", 0x69);
                        dictionary.Add("PHMO", 0x6a);
                        dictionary.Add("QPEG", 0x6b);
                        dictionary.Add("RGBT", 0x6c);
                        dictionary.Add("RIVA", 0x6d);
                        dictionary.Add("RLND", 110);
                        dictionary.Add("RT21", 0x6f);
                        dictionary.Add("RVX", 0x70);
                        dictionary.Add("S263", 0x71);
                        dictionary.Add("SCCD", 0x72);
                        dictionary.Add("SDCC", 0x73);
                        dictionary.Add("SFMC", 0x74);
                        dictionary.Add("SMSC", 0x75);
                        dictionary.Add("SMSD", 0x75);
                        dictionary.Add("SPLC", 0x76);
                        dictionary.Add("SQZ2", 0x77);
                        dictionary.Add("STVA", 120);
                        dictionary.Add("STVB", 0x79);
                        dictionary.Add("STVC", 0x7a);
                        dictionary.Add("SV10", 0x7b);
                        dictionary.Add("SV3M", 0x7c);
                        dictionary.Add("TLMS", 0x7d);
                        dictionary.Add("TLST", 0x7d);
                        dictionary.Add("TM20", 0x7e);
                        dictionary.Add("TMIC", 0x7f);
                        dictionary.Add("TMOT", 0x80);
                        dictionary.Add("TR20", 0x81);
                        dictionary.Add("ULTI", 130);
                        dictionary.Add("UYVP", 0x83);
                        dictionary.Add("V261", 0x84);
                        dictionary.Add("V422", 0x85);
                        dictionary.Add("V655", 0x86);
                        dictionary.Add("VCR1", 0x87);
                        dictionary.Add("VCWV", 0x88);
                        dictionary.Add("VDCT", 0x89);
                        dictionary.Add("VIDS", 0x8a);
                        dictionary.Add("VGPX", 0x8b);
                        dictionary.Add("VIVO", 140);
                        dictionary.Add("VIXL", 0x8d);
                        dictionary.Add("VJPG", 0x8e);
                        dictionary.Add("VLV1", 0x8f);
                        dictionary.Add("VQC1", 0x90);
                        dictionary.Add("VQC2", 0x91);
                        dictionary.Add("VQJP", 0x92);
                        dictionary.Add("VQS4", 0x93);
                        dictionary.Add("VX1K", 0x94);
                        dictionary.Add("VX2K", 0x95);
                        dictionary.Add("VXSP", 150);
                        dictionary.Add("WBVC", 0x97);
                        dictionary.Add("WINX", 0x98);
                        dictionary.Add("WJPG", 0x99);
                        dictionary.Add("WMV1", 0x9a);
                        dictionary.Add("WMV2", 0x9b);
                        dictionary.Add("WMV3", 0x9c);
                        dictionary.Add("WNV1", 0x9d);
                        dictionary.Add("WPY2", 0x9d);
                        dictionary.Add("WZCD", 0x9e);
                        dictionary.Add("WZDC", 0x9f);
                        dictionary.Add("XJPG", 160);
                        dictionary.Add("XLV0", 0xa1);
                        dictionary.Add("XVID", 0xa2);
                        dictionary.Add("YC12", 0xa3);
                        dictionary.Add("YCCK", 0xa4);
                        dictionary.Add("YU92", 0xa5);
                        dictionary.Add("YUV8", 0xa6);
                        dictionary.Add("YUV9", 0xa7);
                        dictionary.Add("YUYP", 0xa8);
                        dictionary.Add("YUYV", 0xa9);
                        dictionary.Add("ZPEG", 170);
                        dictionary.Add("ZPG1", 0xab);
                        dictionary.Add("ZPG2", 0xab);
                        dictionary.Add("ZPG3", 0xab);
                        dictionary.Add("ZPG4", 0xab);
                        <>f__switch$map1 = dictionary;
                    }
                    if (<>f__switch$map1.TryGetValue(key, out num))
                    {
                        switch (num)
                        {
                            case 0:
                                return "Array VideoONE MPEG1-I capture";

                            case 1:
                                return "Ziracom Video";

                            case 2:
                                return "Array VideoONE capture/compression";

                            case 3:
                                return "Intel RDX";

                            case 4:
                                return "Microsoft Corporation Video";

                            case 5:
                                return "AuraVision Aura 2 codec";

                            case 6:
                                return "AuraVision Aura 1 codec";

                            case 7:
                                return "USH GmbH AUVX video codec";

                            case 8:
                                return "Brooktree MediaStream codec";

                            case 9:
                                return "Brooktree composite video codec";

                            case 10:
                                return "Intel YUV12 codec";

                            case 11:
                                return "Canopus DV codec";

                            case 12:
                                return "Microsoft CamCorder in Office 97 (screen capture codec)";

                            case 13:
                                return "Winnov Caviara Champagne";

                            case 14:
                                return "CyberLink Corporation MediaShow 1.0";

                            case 15:
                                return "Weitek 4:2:0 YUV planar";

                            case 0x10:
                                return "CyberLink Corporation TalkingShow 1.0";

                            case 0x11:
                                return "Cinepak by SuperMac";

                            case 0x12:
                                return "Microsoft Corporation Video";

                            case 0x13:
                                return "Creative Labs YUV";

                            case 20:
                                return "Microsoft MPEG-4 Version 3 Video";

                            case 0x15:
                                return "Microsoft Corporation Video";

                            case 0x16:
                                return "DivX Video";

                            case 0x17:
                                return "Broadway 101 Motion JPEG codec";

                            case 0x18:
                                return "YUV411 with DPCM 6-bit compression";

                            case 0x19:
                                return "YUV411 with DPCM 8-bit compression";

                            case 0x1a:
                                return "YUV422 with DPCM 6-bit compression";

                            case 0x1b:
                                return "YUV422 with DPCM 8-bit compression";

                            case 0x1c:
                                return "YVU9 with DPCM 6-bit compression";

                            case 0x1d:
                                return "YVU9 with DPCM 8-bit compression";

                            case 30:
                                return "YVU9 with DPCM 6-bit compression and thinned-out";

                            case 0x1f:
                                return "The Duck Corporation TrueMotion 1.0";

                            case 0x20:
                                return "SMPTE 314M 25Mb/s compressed";

                            case 0x21:
                                return "SMPTE 314M 50Mb/s compressed";

                            case 0x22:
                                return "DVE-2 videoconferencing codec";

                            case 0x23:
                                return "DVC Pro HD";

                            case 0x24:
                                return "DV data as defined in Part 3 of the Specification of Consumer-use Digital VCRs";

                            case 0x25:
                                return "Matsushita Electric Industrial Co., Ltd. Video";

                            case 0x26:
                                return "DV data as defined in Part 2 of the Specification of Consumer-use Digital VCRs";

                            case 0x27:
                                return "DV data as defined in Part 6 of Specification of Consumer-use Digital VCRs";

                            case 40:
                                return "Lucent DVX1000SP video decoder.";

                            case 0x29:
                                return "Lucent DVX2000S video decoder";

                            case 0x2a:
                                return "Lucent DVX3000S video decoder";

                            case 0x2b:
                                return "DirectX texture compression";

                            case 0x2c:
                                return "DivX Version 5 Video";

                            case 0x2d:
                                return "EverAd Marquee WMA codec";

                            case 0x2e:
                                return "eTreppid video codec";

                            case 0x2f:
                                return "Field-encoded motion JPEG with LSI bitstream format";

                            case 0x30:
                                return "Softlab-Nsk Ltd. Forward alpha";

                            case 0x31:
                                return "Softlab-Nsk Ltd. Forward JPEG";

                            case 50:
                                return "Softlab-Nsk Ltd. Forward JPEG+alpha";

                            case 0x33:
                                return "Iterated Systems, Inc. Fractal video frame";

                            case 0x34:
                                return "3dfx Interactive, Inc. Video";

                            case 0x35:
                                return "Microsoft Corporation Video";

                            case 0x36:
                                return ("Intel " + this.CompressionId.ToString(StringType.UTF8) + " Conferencing codec");

                            case 0x37:
                                return "Intel I263";

                            case 0x38:
                                return "Intel Indeo 4 codec";

                            case 0x39:
                                return "Intel RDX";

                            case 0x3a:
                                return "InSoft, Inc. CellB videoconferencing codec";

                            case 0x3b:
                                return "Intel intermediate YUV9";

                            case 60:
                                return "Intel layered Video";

                            case 0x3d:
                                return "ITU-T's H.263+ compression standard";

                            case 0x3e:
                                return "Intel hardware motion compensation";

                            case 0x3f:
                                return "IEEE 1394 digital video control and capture board format";

                            case 0x40:
                                return "Intel YUV uncompressed";

                            case 0x41:
                                return "Intel's next-generation video codec";

                            case 0x42:
                                return "UYVY interlaced (even, then odd lines)";

                            case 0x43:
                                return "Intel Indeo Video Version 3";

                            case 0x44:
                                return "Intel Indeo Video Version 4";

                            case 0x45:
                                return "Intel Indeo Video Version 5";

                            case 70:
                                return "LEAD Technologies, Inc. Y41P interlaced (even, then odd lines)";

                            case 0x47:
                                return "IEEE 1394 Digital Camera 1.04 Specification: mode 2, 12-bit YUV (4:1:1)";

                            case 0x48:
                                return "IEEE 1394 Digital Camera 1.04 Specification: mode 2, 24 bit YUV (4:4:4)";

                            case 0x49:
                                return "Microsoft Corporation Still image JPEG DIB.";

                            case 0x4a:
                                return "LEAD Technologies, Inc. Proprietary MCMP compression";

                            case 0x4b:
                                return "Liafail";

                            case 0x4c:
                                return "LEAD Technologies, Inc. Lossless JPEG compression";

                            case 0x4d:
                                return "Infinop Inc. Video";

                            case 0x4e:
                                return "Infinop Lightning Strike constant bit rate video codec";

                            case 0x4f:
                                return "Infinop Lightning Strike multiple bit rate video codec";

                            case 80:
                                return "Matrox Electronic Systems, Ltd. Uncompressed field-based YUY2";

                            case 0x51:
                                return "Microsoft ISO MPEG-4 video V1.1";

                            case 0x52:
                                return "Motion JPEG";

                            case 0x53:
                                return "Matrox MPEG-2 elementary video stream";

                            case 0x54:
                                return "Matrox MPEG-2 elementary I-frame-only video stream";

                            case 0x55:
                                return "Media Excel Inc. MPEG-2 audio";

                            case 0x56:
                                return "Media Excel Inc. MPEG-2 transport";

                            case 0x57:
                                return "Media Excel Inc. MPEG-2 video";

                            case 0x58:
                                return "Microsoft MPEG-4 video codec V2";

                            case 0x59:
                                return "Media Excel Inc. MPEG-4 audio";

                            case 90:
                                return "Microsoft ISO MPEG-4 video V1.0";

                            case 0x5b:
                                return "Media Excel Inc. MPEG-4 transport";

                            case 0x5c:
                                return "Media Excel Inc. MPEG-4 video";

                            case 0x5d:
                                return "Chromatic Research, Inc. MPEG-1 video, I frame";

                            case 0x5e:
                                return "Microsoft MPEG-4 Version 1 Video";

                            case 0x5f:
                                return "FAST Multimedia AG Mrcodec";

                            case 0x60:
                                return "Microsoft Run length encoding";

                            case 0x61:
                                return "Microsoft screen codec V1";

                            case 0x62:
                                return "Microsoft video codec V1";

                            case 0x63:
                                return "Microsoft Video 1";

                            case 100:
                                return "Nokia MVC video codec";

                            case 0x65:
                                return "Nogatech video compression 1";

                            case 0x66:
                                return "Nogatech YUV 12 format";

                            case 0x67:
                                return "Nogatech YUV 422 format";

                            case 0x68:
                                return "Pinnacle RL video codec";

                            case 0x69:
                                return "Pinnacle Studio 400 video codec";

                            case 0x6a:
                                return "IBM Corporation Photomotion";

                            case 0x6b:
                                return "Q-Team QPEG 1.1 format video codec";

                            case 0x6c:
                                return "Computer Concepts Ltd. 32-bit support";

                            case 0x6d:
                                return "NVIDIA Corporation Swizzled texture format";

                            case 110:
                                return "Roland Corporation Video";

                            case 0x6f:
                                return "Intel Indeo 2.1";

                            case 0x70:
                                return "Intel RDX";

                            case 0x71:
                                return "Sorenson Vision H.263";

                            case 0x72:
                                return "Luminositi SoftCam codec";

                            case 0x73:
                                return "Sun Digital Camera codec";

                            case 0x74:
                                return "Crystal Net SFM codec";

                            case 0x75:
                                return "Radius Proprietary";

                            case 0x76:
                                return "Splash Studios ACM audio codec";

                            case 0x77:
                                return "Microsoft VXtreme video codec V2";

                            case 120:
                                return "ST CMOS Imager Data (Bayer)";

                            case 0x79:
                                return "ST CMOS Imager Data (Nudged Bayer)";

                            case 0x7a:
                                return "ST CMOS Imager Data (Bunched)";

                            case 0x7b:
                                return "Sorenson Video R1";

                            case 0x7c:
                                return "Sorenson SV3 module decoder";

                            case 0x7d:
                                return "TeraLogic motion intraframe codec";

                            case 0x7e:
                                return "The Duck Corporation TrueMotion 2.0";

                            case 0x7f:
                                return "TeraLogic motion intraframe codec";

                            case 0x80:
                                return "TrueMotion video compression algorithm";

                            case 0x81:
                                return "The Duck Corporation TrueMotion RT 2.0";

                            case 130:
                                return "IBM Corporation Ultimotion";

                            case 0x83:
                                return "Evans & Sutherland YCbCr 4:2:2 extended precision, 10 bits per component (U0Y0V0Y1)";

                            case 0x84:
                                return "Lucent VX3000S video codec";

                            case 0x85:
                                return "VITEC Multimedia 24-bit YUV 4:2:2 format (CCIR 601)";

                            case 0x86:
                                return "VITEC Multimedia 16-bit YUV 4:2:2 format";

                            case 0x87:
                                return "ATI VCR 1.0";

                            case 0x88:
                                return "VideoCon wavelet";

                            case 0x89:
                                return "VITEC Multimedia Video Maker Pro DIB";

                            case 0x8a:
                                return "VITEC Multimedia YUV 4:2:2 CCIR 601 for v422";

                            case 0x8b:
                                return "Alaris VGPixel video";

                            case 140:
                                return "Vivo H.263 video codec";

                            case 0x8d:
                                return "miro Computer Products AG";

                            case 0x8e:
                                return "Video Communication Systems - A JPEG-based compression scheme for RGB bitmaps";

                            case 0x8f:
                                return "VideoLogic Systems VLCAP.DRV";

                            case 0x90:
                                return "ViewQuest Technologies Inc. 0x31435156";

                            case 0x91:
                                return "ViewQuest Technologies Inc. 0x32435156";

                            case 0x92:
                                return "ViewQuest Technologies Inc. VQ630 dual-mode digital camera";

                            case 0x93:
                                return "ViewQuest Technologies Inc. VQ110 digital video camera";

                            case 0x94:
                                return "Lucent VX1000S video codec";

                            case 0x95:
                                return "Lucent VX2000S video codec";

                            case 150:
                                return "Lucent VX1000SP video codec9";

                            case 0x97:
                                return "Winbond Electronics Corporation W9960";

                            case 0x98:
                                return "Winnov, Inc. Video";

                            case 0x99:
                                return "Winbond motion JPEG bitstream format";

                            case 0x9a:
                                return "Microsoft Windows Media Video Version 7";

                            case 0x9b:
                                return "Microsoft Windows Media Video Version 8";

                            case 0x9c:
                                return "Microsoft Windows Media Video Version 9";

                            case 0x9d:
                                return "Winnov, Inc. Video";

                            case 0x9e:
                                return "CORE Co. Ltd. iScan";

                            case 0x9f:
                                return "CORE Co. Ltd. iSnap";

                            case 160:
                                return "Xirlink JPEG-like compressor";

                            case 0xa1:
                                return "XL video decoder";

                            case 0xa2:
                                return "XviD Video";

                            case 0xa3:
                                return "Intel YUV12 Video";

                            case 0xa4:
                                return "Uncompressed YCbCr Video with key data";

                            case 0xa5:
                                return "Intel YUV Video";

                            case 0xa6:
                                return "Winnov Caviar YUV8 Video";

                            case 0xa7:
                                return "Intel YUV Video";

                            case 0xa8:
                                return "Evans & Sutherland YCbCr 4:2:2 extended precision, 10 bits per component Video";

                            case 0xa9:
                                return "Canopus YUYV Compressor Video";

                            case 170:
                                return "Metheus Corporation Video Zipper";

                            case 0xab:
                                return "VoDeo Solutions Video";
                        }
                    }
                }
                object[] args = new object[] { this.CompressionId };
                return string.Format(CultureInfo.InvariantCulture, "Unknown Image ({0})", args);
            }
        }
        public override int GetHashCode()
        {
            return (int) ((((((((((this.size ^ this.width) ^ this.height) ^ this.planes) ^ this.bit_count) ^ this.compression_id.ToUInt()) ^ this.size_of_image) ^ this.x_pixels_per_meter) ^ this.y_pixels_per_meter) ^ this.colors_used) ^ this.colors_important);
        }

        public override bool Equals(object other)
        {
            return ((other is BitmapInfoHeader) && this.Equals((BitmapInfoHeader) other));
        }

        public bool Equals(BitmapInfoHeader other)
        {
            return ((((((this.size == other.size) && (this.width == other.width)) && ((this.height == other.height) && (this.planes == other.planes))) && (((this.bit_count == other.bit_count) && (this.compression_id == other.compression_id)) && ((this.size_of_image == other.size_of_image) && (this.x_pixels_per_meter == other.x_pixels_per_meter)))) && ((this.y_pixels_per_meter == other.y_pixels_per_meter) && (this.colors_used == other.colors_used))) && (this.colors_important == other.colors_important));
        }

        public static bool operator ==(BitmapInfoHeader first, BitmapInfoHeader second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(BitmapInfoHeader first, BitmapInfoHeader second)
        {
            return !first.Equals(second);
        }
    }
}

