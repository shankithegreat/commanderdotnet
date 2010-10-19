namespace TagLib.Id3v2
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using TagLib;

    public class RelativeVolumeFrame : Frame
    {
        private ChannelData[] channels;
        private string identification;

        public RelativeVolumeFrame(string identification) : base(FrameType.RVA2, 4)
        {
            this.channels = new ChannelData[9];
            this.identification = identification;
        }

        public RelativeVolumeFrame(ByteVector data, byte version) : base(data, version)
        {
            this.channels = new ChannelData[9];
            base.SetData(data, 0, version, true);
        }

        protected internal RelativeVolumeFrame(ByteVector data, int offset, FrameHeader header, byte version) : base(header)
        {
            this.channels = new ChannelData[9];
            base.SetData(data, offset, version, false);
        }

        private static int BitsToBytes(int i)
        {
            return (((i % 8) != 0) ? (((i - (i % 8)) / 8) + 1) : (i / 8));
        }

        public override Frame Clone()
        {
            RelativeVolumeFrame frame = new RelativeVolumeFrame(this.identification);
            for (int i = 0; i < 9; i++)
            {
                frame.channels[i] = this.channels[i];
            }
            return frame;
        }

        public static RelativeVolumeFrame Get(TagLib.Id3v2.Tag tag, string identification, bool create)
        {
            RelativeVolumeFrame frame;
            IEnumerator<Frame> enumerator = tag.GetFrames(FrameType.RVA2).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Frame current = enumerator.Current;
                    frame = current as RelativeVolumeFrame;
                    if ((frame != null) && (frame.Identification == identification))
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
            frame = new RelativeVolumeFrame(identification);
            tag.AddFrame(frame);
            return frame;
        }

        public double GetPeakVolume(ChannelType type)
        {
            return this.channels[(int) type].PeakVolume;
        }

        public ulong GetPeakVolumeIndex(ChannelType type)
        {
            return this.channels[(int) type].PeakVolumeIndex;
        }

        public float GetVolumeAdjustment(ChannelType type)
        {
            return this.channels[(int) type].VolumeAdjustment;
        }

        public short GetVolumeAdjustmentIndex(ChannelType type)
        {
            return this.channels[(int) type].VolumeAdjustmentIndex;
        }

        protected override void ParseFields(ByteVector data, byte version)
        {
            int startIndex = data.Find(ByteVector.TextDelimiter(StringType.Latin1));
            if (startIndex >= 0)
            {
                this.identification = data.ToString(StringType.Latin1, 0, startIndex++);
                while (startIndex <= (data.Count - 4))
                {
                    int index = data[startIndex++];
                    this.channels[index].VolumeAdjustmentIndex = (short) data.Mid(startIndex, 2).ToUShort();
                    startIndex += 2;
                    int length = BitsToBytes(data[startIndex++]);
                    if (data.Count < (startIndex + length))
                    {
                        break;
                    }
                    this.channels[index].PeakVolumeIndex = data.Mid(startIndex, length).ToULong();
                    startIndex += length;
                }
            }
        }

        protected override ByteVector RenderFields(byte version)
        {
            ByteVector vector = new ByteVector {
                ByteVector.FromString(this.identification, 0),
                ByteVector.TextDelimiter(0)
            };
            for (byte i = 0; i < 9; i = (byte) (i + 1))
            {
                if (this.channels[i].IsSet)
                {
                    vector.Add(i);
                    vector.Add(ByteVector.FromUShort((ushort) this.channels[i].VolumeAdjustmentIndex));
                    byte item = 0;
                    for (byte j = 0; j < 0x40; j = (byte) (j + 1))
                    {
                        if ((this.channels[i].PeakVolumeIndex & (((ulong) 1L) << j)) != 0)
                        {
                            item = (byte) (j + 1);
                        }
                    }
                    vector.Add(item);
                    if (item > 0)
                    {
                        vector.Add(ByteVector.FromULong(this.channels[i].PeakVolumeIndex).Mid(8 - BitsToBytes(item)));
                    }
                }
            }
            return vector;
        }

        public void SetPeakVolume(ChannelType type, double peak)
        {
            this.channels[(int) type].PeakVolume = peak;
        }

        public void SetPeakVolumeIndex(ChannelType type, ulong index)
        {
            this.channels[(int) type].PeakVolumeIndex = index;
        }

        public void SetVolumeAdjustment(ChannelType type, float adjustment)
        {
            this.channels[(int) type].VolumeAdjustment = adjustment;
        }

        public void SetVolumeAdjustmentIndex(ChannelType type, short index)
        {
            this.channels[(int) type].VolumeAdjustmentIndex = index;
        }

        public override string ToString()
        {
            return this.identification;
        }

        public ChannelType[] Channels
        {
            get
            {
                List<ChannelType> list = new List<ChannelType>();
                for (int i = 0; i < 9; i++)
                {
                    if (this.channels[i].IsSet)
                    {
                        list.Add((ChannelType) i);
                    }
                }
                return list.ToArray();
            }
        }

        public string Identification
        {
            get
            {
                return this.identification;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ChannelData
        {
            public short VolumeAdjustmentIndex;
            public ulong PeakVolumeIndex;
            public bool IsSet
            {
                get
                {
                    return ((this.VolumeAdjustmentIndex != 0) || (this.PeakVolumeIndex != 0L));
                }
            }
            public float VolumeAdjustment
            {
                get
                {
                    return (((float) this.VolumeAdjustmentIndex) / 512f);
                }
                set
                {
                    this.VolumeAdjustmentIndex = (short) (value * 512f);
                }
            }
            public double PeakVolume
            {
                get
                {
                    return (((double) this.PeakVolumeIndex) / 512.0);
                }
                set
                {
                    this.PeakVolumeIndex = (ulong) (value * 512.0);
                }
            }
        }
    }
}

