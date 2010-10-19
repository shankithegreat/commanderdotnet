namespace TagLib.Aac
{
    using System;
    using System.Runtime.InteropServices;
    using TagLib;

    public class AudioHeader : ICodec, IAudioCodec
    {
        private int audiobitrate;
        private int audiochannels;
        private int audiosamplerate;
        private static readonly int[] channels = new int[] { 0, 1, 2, 3, 4, 5, 6, 8 };
        private TimeSpan duration;
        private static readonly int[] sample_rates = new int[] { 0x17700, 0x15888, 0xfa00, 0xbb80, 0xac44, 0x7d00, 0x5dc0, 0x5622, 0x3e80, 0x2ee0, 0x2b11, 0x1f40, 0x1cb6 };
        private long stream_length;
        public static readonly AudioHeader Unknown = new AudioHeader();

        private AudioHeader()
        {
            this.stream_length = 0L;
            this.duration = TimeSpan.Zero;
            this.audiochannels = 0;
            this.audiobitrate = 0;
            this.audiosamplerate = 0;
        }

        private AudioHeader(int channels, int bitrate, int samplerate, int numberofsamples, int numberofframes)
        {
            this.duration = TimeSpan.Zero;
            this.stream_length = 0L;
            this.audiochannels = channels;
            this.audiobitrate = bitrate;
            this.audiosamplerate = samplerate;
        }

        public static bool Find(out AudioHeader header, TagLib.File file, long position)
        {
            return Find(out header, file, position, -1);
        }

        public static bool Find(out AudioHeader header, TagLib.File file, long position, int length)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            long num = position + length;
            header = Unknown;
            file.Seek(position);
            ByteVector vector = file.ReadBlock(3);
            if (vector.Count >= 3)
            {
                do
                {
                    file.Seek(position + 3L);
                    vector = vector.Mid(vector.Count - 3);
                    vector.Add(file.ReadBlock((int) TagLib.File.BufferSize));
                    for (int i = 0; (i < (vector.Count - 3)) && ((length < 0) || ((position + i) < num)); i++)
                    {
                        if ((vector[i] == 0xff) && (vector[i + 1] >= 240))
                        {
                            bool flag;
                            try
                            {
                                BitStream stream = new BitStream(vector.Mid(i, 7).Data);
                                stream.ReadInt32(12);
                                stream.ReadInt32(1);
                                stream.ReadInt32(2);
                                stream.ReadInt32(1);
                                stream.ReadInt32(2);
                                int index = stream.ReadInt32(4);
                                if (index >= sample_rates.Length)
                                {
                                    return false;
                                }
                                long num4 = sample_rates[index];
                                stream.ReadInt32(1);
                                int num5 = stream.ReadInt32(3);
                                if (num5 >= channels.Length)
                                {
                                    return false;
                                }
                                stream.ReadInt32(4);
                                long num6 = stream.ReadInt32(13);
                                if (num6 < 7L)
                                {
                                    return false;
                                }
                                stream.ReadInt32(11);
                                int numberofframes = stream.ReadInt32(2) + 1;
                                long num8 = numberofframes * 0x400;
                                long num9 = ((num6 * 8L) * num4) / num8;
                                header = new AudioHeader(channels[num5], (int) num9, (int) num4, (int) num8, numberofframes);
                                flag = true;
                            }
                            catch (CorruptFileException)
                            {
                            }
                            return flag;
                        }
                    }
                    position += TagLib.File.BufferSize;
                }
                while ((vector.Count > 3) && ((length < 0) || (position < num)));
            }
            return false;
        }

        public void SetStreamLength(long streamLength)
        {
            this.stream_length = streamLength;
            this.duration = TimeSpan.FromSeconds((this.stream_length * 8.0) / ((double) this.audiobitrate));
        }

        public int AudioBitrate
        {
            get
            {
                return this.audiobitrate;
            }
        }

        public int AudioChannels
        {
            get
            {
                return this.audiochannels;
            }
        }

        public int AudioSampleRate
        {
            get
            {
                return this.audiosamplerate;
            }
        }

        public string Description
        {
            get
            {
                return "ADTS AAC";
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return this.duration;
            }
        }

        public TagLib.MediaTypes MediaTypes
        {
            get
            {
                return TagLib.MediaTypes.Audio;
            }
        }
    }
}

