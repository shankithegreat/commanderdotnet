namespace TagLib.Riff
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    [StructLayout(LayoutKind.Sequential)]
    public struct WaveFormatEx : ICodec, IAudioCodec, ILosslessAudioCodec
    {
        private ushort format_tag;
        private ushort channels;
        private uint samples_per_second;
        private uint average_bytes_per_second;
        private ushort bits_per_sample;
        [Obsolete("Use WaveFormatEx(ByteVector,int)")]
        public WaveFormatEx(ByteVector data) : this(data, 0)
        {
        }

        public WaveFormatEx(ByteVector data, int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((offset + 0x10) > data.Count)
            {
                throw new CorruptFileException("Expected 16 bytes.");
            }
            this.format_tag = data.Mid(offset, 2).ToUShort(false);
            this.channels = data.Mid(offset + 2, 2).ToUShort(false);
            this.samples_per_second = data.Mid(offset + 4, 4).ToUInt(false);
            this.average_bytes_per_second = data.Mid(offset + 8, 4).ToUInt(false);
            this.bits_per_sample = data.Mid(offset + 14, 2).ToUShort(false);
        }

        int ILosslessAudioCodec.BitsPerSample
        {
            get
            {
                return this.bits_per_sample;
            }
        }
        public ushort FormatTag
        {
            get
            {
                return this.format_tag;
            }
        }
        public uint AverageBytesPerSecond
        {
            get
            {
                return this.average_bytes_per_second;
            }
        }
        public ushort BitsPerSample
        {
            get
            {
                return this.bits_per_sample;
            }
        }
        public int AudioBitrate
        {
            get
            {
                return (int) Math.Round((double) ((this.average_bytes_per_second * 8.0) / 1000.0));
            }
        }
        public int AudioSampleRate
        {
            get
            {
                return (int) this.samples_per_second;
            }
        }
        public int AudioChannels
        {
            get
            {
                return this.channels;
            }
        }
        public TagLib.MediaTypes MediaTypes
        {
            get
            {
                return TagLib.MediaTypes.Audio;
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
                ushort formatTag = this.FormatTag;
                switch (formatTag)
                {
                    case 0:
                        return "Unknown Wave Format";

                    case 1:
                        return "PCM Audio";

                    case 2:
                        return "Microsoft Adaptive PCM Audio";

                    case 3:
                        return "PCM Audio in IEEE floating-point format";

                    case 4:
                        return "Compaq VSELP Audio";

                    case 5:
                        return "IBM CVSD Audio";

                    case 6:
                        return "Microsoft ALAW Audio";

                    case 7:
                        return "Microsoft MULAW Audio";

                    case 8:
                        return "Microsoft DTS Audio";

                    case 9:
                        return "Microsoft DRM Encrypted Audio";

                    case 10:
                        return "Microsoft Speech Audio";

                    case 11:
                        return "Microsoft Windows Media RT Voice Audio";

                    case 0x10:
                        return "OKI ADPCM Audio";

                    case 0x11:
                        return "Intel ADPCM Audio";

                    case 0x12:
                        return "VideoLogic ADPCM Audio";

                    case 0x13:
                        return "Sierra ADPCM Audio";

                    case 20:
                        return "Antex ADPCM Audio";

                    case 0x15:
                        return "DSP DIGISTD Audio";

                    case 0x16:
                        return "DSP DIGIFIX Audio";

                    case 0x17:
                        return "Dialogic OKI ADPCM Audio";

                    case 0x18:
                        return "Media Vision ADPCM Audio for Jazz 16";

                    case 0x19:
                        return "Hewlett-Packard CU Audio";

                    case 0x1a:
                        return "Hewlett-Packard Dynamic Voice Audio";

                    case 0x20:
                        return "Yamaha ADPCM Audio";

                    case 0x21:
                        return "Speech Compression Audio";

                    case 0x22:
                        return "DSP Group True Speech Audio";

                    case 0x23:
                        return "Echo Speech Audio";

                    case 0x24:
                        return "Ahead AF36 Audio";

                    case 0x25:
                        return "Audio Processing Technology Audio";

                    case 0x26:
                        return "Ahead AF10 Audio";

                    case 0x27:
                        return "Aculab Prosody CTI Speech Card Audio";

                    case 40:
                        return "Merging Technologies LRC Audio";

                    case 0x30:
                        return "Dolby AC2 Audio";

                    case 0x31:
                        return "Microsoft GSM6.10 Audio";

                    case 50:
                        return "Microsoft MSN Audio";

                    case 0x33:
                        return "Antex ADPCME Audio";

                    case 0x34:
                        return "Control Resources VQLPC";

                    case 0x35:
                        return "DSP REAL Audio";

                    case 0x36:
                        return "DSP ADPCM Audio";

                    case 0x37:
                        return "Control Resources CR10 Audio";

                    case 0x38:
                        return "Natural MicroSystems VBXADPCM Audio";

                    case 0x39:
                        return "Roland RDAC Proprietary Audio Format";

                    case 0x3a:
                        return "Echo Speech Proprietary Audio Compression Format";

                    case 0x3b:
                        return "Rockwell ADPCM Audio";

                    case 60:
                        return "Rockwell DIGITALK Audio";

                    case 0x3d:
                        return "Xebec Proprietary Audio Compression Format";

                    case 0x40:
                        return "Antex G721 ADPCM Audio";

                    case 0x41:
                        return "Antex G728 CELP Audio";

                    case 0x42:
                        return "Microsoft MSG723 Audio";

                    case 0x43:
                        return "Microsoft MSG723.1 Audio";

                    case 0x44:
                        return "Microsoft MSG729 Audio";

                    case 0x45:
                        return "Microsoft SPG726 Audio";

                    case 80:
                        return "Microsoft MPEG Audio";

                    case 0x52:
                        return "InSoft RT24 Audio";

                    case 0x53:
                        return "InSoft PAC Audio";

                    case 0x55:
                        return "ISO/MPEG Layer 3 Audio";

                    case 0x59:
                        return "Lucent G723 Audio";

                    case 0x60:
                        return "Cirrus Logic Audio";

                    case 0x61:
                        return "ESS Technology PCM Audio";

                    case 0x62:
                        return "Voxware Audio";

                    case 0x63:
                        return "Canopus ATRAC Audio";

                    case 100:
                        return "APICOM G726 ADPCM Audio";

                    case 0x65:
                        return "APICOM G722 ADPCM Audio";

                    case 0x67:
                        return "Microsoft DSAT Display Audio";

                    case 0x69:
                        return "Voxware Byte Aligned Audio";

                    case 0x70:
                        return "Voxware AC8 Audio";

                    case 0x71:
                        return "Voxware AC10 Audio";

                    case 0x72:
                        return "Voxware AC16 Audio";

                    case 0x73:
                        return "Voxware AC20 Audio";

                    case 0x74:
                        return "Voxware RT24 Audio";

                    case 0x75:
                        return "Voxware RT29 Audio";

                    case 0x76:
                        return "Voxware RT29HW Audio";

                    case 0x77:
                        return "Voxware VR12 Audio";

                    case 120:
                        return "Voxware VR18 Audio";

                    case 0x79:
                        return "Voxware TQ40 Audio";

                    case 0x7a:
                        return "Voxware SC3 Audio";

                    case 0x7b:
                        return "Voxware SC3 Audio";

                    case 0x80:
                        return "SoftSound Audio";

                    case 0x81:
                        return "Voxware TQ60 Audio";

                    case 130:
                        return "Microsoft RT24 Audio";

                    case 0x83:
                        return "AT&T G729A Audio";

                    case 0x84:
                        return "Motion Pixels MVI2 Audio";

                    case 0x85:
                        return "Datafusion Systems G726 Audio";

                    case 0x86:
                        return "Datafusion Systems G610 Audio";

                    case 0x88:
                        return "Iterated Systems Audio";

                    case 0x89:
                        return "OnLive! Audio";

                    case 0x8a:
                        return "Multitude FT SX20 Audio";

                    case 0x8b:
                        return "InfoCom ITS ACM G721 Audio";

                    case 140:
                        return "Convedia G729 Audio";

                    case 0x8d:
                        return "Congruency Audio";

                    case 0x91:
                        return "Siemens Business Communications 24 Audio";

                    case 0x92:
                        return "Sonic Foundary Dolby AC3 Audio";

                    case 0x93:
                        return "MediaSonic G723 Audio";

                    case 0x94:
                        return "Aculab Prosody CTI Speech Card Audio";

                    case 0x97:
                        return "ZyXEL ADPCM";

                    case 0x98:
                        return "Philips Speech Processing LPCBB Audio";

                    case 0x99:
                        return "Studer Professional PACKED Audio";

                    case 160:
                        return "Malden Electronics Phony Talk Audio";

                    case 0xa1:
                        return "Racal Recorder GSM Audio";

                    case 0xa2:
                        return "Racal Recorder G720.a Audio";

                    case 0xa3:
                        return "Racal G723.1 Audio";

                    case 0xa4:
                        return "Racal Tetra ACELP Audio";

                    case 0xb0:
                        return "NEC AAC Audio";

                    case 0x130:
                        return "Sipro Lab ACELPNET Audio";

                    case 0x131:
                        return "Sipro Lab ACELP4800 Audio";

                    case 0x132:
                        return "Sipro Lab ACELP8v3 Audio";

                    case 0x133:
                        return "Sipro Lab G729 Audio";

                    case 0x134:
                        return "Sipro Lab G729A Audio";

                    case 0x135:
                        return "Sipro Lab KELVIN Audio";

                    case 310:
                        return "VoiceAge AMR Audio";

                    case 320:
                        return "Dictaphone G726 ADPCM Audio";

                    case 0x141:
                        return "Dictaphone CELP68 Audio";

                    case 0x142:
                        return "Dictaphone CELP54 Audio";

                    case 0x170:
                        return "Unisys NAP ADPCM Audio";

                    case 0x171:
                        return "Unisys NAP ULAW Audio";

                    case 370:
                        return "Unisys NAP ALAW Audio";

                    case 0x173:
                        return "Unisys NAP 16K Audio";

                    case 0x174:
                        return "SysCom ACM SYC008 Audio";

                    case 0x175:
                        return "SysCom ACM SYC701 G726L Audio";

                    case 0x176:
                        return "SysCom ACM SYC701 CELP54 Audio";

                    case 0x177:
                        return "SysCom ACM SYC701 CELP68 Audio";

                    case 0x178:
                        return "Knowledge Adventure ADPCM Audio";

                    case 0x180:
                        return "MPEG2 AAC Audio";
                }
                switch (formatTag)
                {
                    case 0x300:
                        return "Fujitsu FM TOWNS SND Audio";

                    case 0x301:
                    case 770:
                    case 0x303:
                    case 0x304:
                    case 0x305:
                    case 0x306:
                    case 0x307:
                    case 0x308:
                        return "Unknown Fujitsu Audio";

                    default:
                        switch (formatTag)
                        {
                            case 0xa100:
                                return "Comverse Infosys G723 1 Audio";

                            case 0xa101:
                                return "Comverse Infosys AVQSBC Audio";

                            case 0xa102:
                                return "Comverse Infosys OLDSBC Audio";

                            case 0xa103:
                                return "Symbol Technology G729A Audio";

                            case 0xa104:
                                return "VoiceAge AMR WB Audio";

                            case 0xa105:
                                return "Ingenient G726 Audio";

                            case 0xa106:
                                return "ISO/MPEG-4 Advanced Audio Coding";

                            case 0xa107:
                                return "Encore G726 Audio";

                            case 0x120:
                                return "Philips Speach Processing CELP Audio";

                            case 0x121:
                                return "Philips Speach Processing GRUNDIG Audio";

                            case 0x123:
                                return "Digital Equipment Corporation G723 Audio";

                            case 0x125:
                                return "Sanyo LD-ADPCM Audio";
                        }
                        break;
                }
                switch (formatTag)
                {
                    case 0x150:
                        return "QUALCOMM Pure Voice Audio";

                    case 0x151:
                        return "QUALCOMM Half Rate Audio";

                    case 0x155:
                        return "Ring Zero TUBGSM Audio";

                    case 640:
                        return "Telum Audio";

                    case 0x281:
                        return "Telum IA Audio";

                    case 0x285:
                        return "Norcom Voice Systems ADPCM Audio";
                }
                switch (formatTag)
                {
                    case 0x1000:
                        return "Ing. C. Olivetti & C., S.p.A. GSM Audio";

                    case 0x1001:
                        return "Ing. C. Olivetti & C., S.p.A. ADPCM Audio";

                    case 0x1002:
                        return "Ing. C. Olivetti & C., S.p.A. CELP Audio";

                    case 0x1003:
                        return "Ing. C. Olivetti & C., S.p.A. SBC Audio";

                    case 0x1004:
                        return "Ing. C. Olivetti & C., S.p.A. OPR Audio";

                    default:
                        switch (formatTag)
                        {
                            case 0x1100:
                                return "Lernout & Hauspie Audio";

                            case 0x1101:
                                return "Lernout & Hauspie CELP Audio";

                            case 0x1102:
                                return "Lernout & Hauspie SB8 Audio";

                            case 0x1103:
                                return "Lernout & Hauspie SB12 Audio";

                            case 0x1104:
                                return "Lernout & Hauspie SB16 Audio";

                            case 0x160:
                                return "Microsoft WMA1 Audio";

                            case 0x161:
                                return "Microsoft WMA2 Audio";

                            case 0x162:
                                return "Microsoft Multichannel WMA Audio";

                            case 0x163:
                                return "Microsoft Lossless WMA Audio";

                            case 0x200:
                                return "Creative ADPCM Audio";

                            case 0x202:
                                return "Creative FastSpeech8 Audio";

                            case 0x203:
                                return "Creative FastSpeech10 Audio";
                        }
                        break;
                }
                switch (formatTag)
                {
                    case 0x270:
                        return "Sony SCX Audio";

                    case 0x271:
                        return "Sony SCY Audio";

                    case 0x272:
                        return "Sony ATRAC3 Audio";

                    case 0x273:
                        return "Sony SPC Audio";

                    case 0x100:
                        return "Rhetorex ADPCM Audio";

                    case 0x101:
                        return "BeCubed IRAT Audio";

                    case 0x111:
                        return "Vivo G723 Audio";

                    case 0x112:
                        return "Vivo Siren Audio";

                    case 0x250:
                        return "Interactive Prodcuts HSX Audio";

                    case 0x251:
                        return "Interactive Products RPELP Audio";

                    case 0x350:
                        return "Micronas Semiconductors Development Audio";

                    case 0x351:
                        return "Micronas Semiconductors CELP833 Audio";

                    case 0x680:
                        return "AT&T VME VMPCM Audio";

                    case 0x681:
                        return "AT&T TPC Audio";

                    case 0x7a21:
                        return "Microsoft Adaptive Multirate Audio";

                    case 0x7a22:
                        return "Microsoft Adaptive Multirate Audio with silence detection";

                    case 400:
                        return "Digital Theater Systems DTS DS Audio";

                    case 0x210:
                        return "UHER ADPCM Audio";

                    case 0x220:
                        return "Quarterdeck Audio";

                    case 560:
                        return "I-Link VC Audio";

                    case 0x240:
                        return "Aureal RAW SPORT Audio";

                    case 0x260:
                        return "Consistens Software CS2 Audio";

                    case 0x400:
                        return "Brooktree Digital Audio";

                    case 0x450:
                        return "QDesign Audio";

                    case 0x1400:
                        return "Norris Audio";

                    case 0x1500:
                        return "AT&T Soundspace Musicompress Audio";

                    case 0x1971:
                        return "Sonic Foundry Lossless Audio";

                    case 0x1979:
                        return "Innings ADPCM Audio";

                    case 0x2000:
                        return "FAST Multimedia DVM Audio";

                    case 0x4143:
                        return "Divio AAC";

                    case 0x4201:
                        return "Nokia Adaptive Multirate Audio";

                    case 0x4243:
                        return "Divio G726 Audio";

                    case 0x7000:
                        return "3Com NBX Audio";
                }
                return ("Unknown Audio (" + this.FormatTag + ")");
            }
        }
        public override int GetHashCode()
        {
            return (int) ((((this.format_tag ^ this.channels) ^ this.samples_per_second) ^ this.average_bytes_per_second) ^ this.bits_per_sample);
        }

        public override bool Equals(object other)
        {
            return ((other is WaveFormatEx) && this.Equals((WaveFormatEx) other));
        }

        public bool Equals(WaveFormatEx other)
        {
            return ((((this.format_tag == other.format_tag) && (this.channels == other.channels)) && ((this.samples_per_second == other.samples_per_second) && (this.average_bytes_per_second == other.average_bytes_per_second))) && (this.bits_per_sample == other.bits_per_sample));
        }

        public static bool operator ==(WaveFormatEx first, WaveFormatEx second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(WaveFormatEx first, WaveFormatEx second)
        {
            return !first.Equals(second);
        }
    }
}

