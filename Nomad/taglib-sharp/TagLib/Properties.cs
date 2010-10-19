namespace TagLib
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Properties : ICodec, IAudioCodec, IVideoCodec
    {
        private ICodec[] codecs;
        private TimeSpan duration;

        public Properties()
        {
            this.codecs = new ICodec[0];
            this.duration = TimeSpan.Zero;
        }

        public Properties(TimeSpan duration, params ICodec[] codecs)
        {
            this.codecs = new ICodec[0];
            this.duration = TimeSpan.Zero;
            this.duration = duration;
            if (codecs != null)
            {
                this.codecs = codecs;
            }
        }

        public Properties(TimeSpan duration, IEnumerable<ICodec> codecs)
        {
            this.codecs = new ICodec[0];
            this.duration = TimeSpan.Zero;
            this.duration = duration;
            if (codecs != null)
            {
                this.codecs = new List<ICodec>(codecs).ToArray();
            }
        }

        public int AudioBitrate
        {
            get
            {
                foreach (ICodec codec in this.codecs)
                {
                    if ((codec != null) && ((codec.MediaTypes & TagLib.MediaTypes.Audio) != TagLib.MediaTypes.None))
                    {
                        IAudioCodec codec2 = codec as IAudioCodec;
                        if ((codec2 != null) && (codec2.AudioBitrate != 0))
                        {
                            return codec2.AudioBitrate;
                        }
                    }
                }
                return 0;
            }
        }

        public int AudioChannels
        {
            get
            {
                foreach (ICodec codec in this.codecs)
                {
                    if ((codec != null) && ((codec.MediaTypes & TagLib.MediaTypes.Audio) != TagLib.MediaTypes.None))
                    {
                        IAudioCodec codec2 = codec as IAudioCodec;
                        if ((codec2 != null) && (codec2.AudioChannels != 0))
                        {
                            return codec2.AudioChannels;
                        }
                    }
                }
                return 0;
            }
        }

        public int AudioSampleRate
        {
            get
            {
                foreach (ICodec codec in this.codecs)
                {
                    if ((codec != null) && ((codec.MediaTypes & TagLib.MediaTypes.Audio) != TagLib.MediaTypes.None))
                    {
                        IAudioCodec codec2 = codec as IAudioCodec;
                        if ((codec2 != null) && (codec2.AudioSampleRate != 0))
                        {
                            return codec2.AudioSampleRate;
                        }
                    }
                }
                return 0;
            }
        }

        public int BitsPerSample
        {
            get
            {
                foreach (ICodec codec in this.codecs)
                {
                    if ((codec != null) && ((codec.MediaTypes & TagLib.MediaTypes.Audio) != TagLib.MediaTypes.None))
                    {
                        ILosslessAudioCodec codec2 = codec as ILosslessAudioCodec;
                        if ((codec2 != null) && (codec2.BitsPerSample != 0))
                        {
                            return codec2.BitsPerSample;
                        }
                    }
                }
                return 0;
            }
        }

        public IEnumerable<ICodec> Codecs
        {
            get
            {
                return this.codecs;
            }
        }

        public string Description
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach (ICodec codec in this.codecs)
                {
                    if (codec != null)
                    {
                        if (builder.Length != 0)
                        {
                            builder.Append("; ");
                        }
                        builder.Append(codec.Description);
                    }
                }
                return builder.ToString();
            }
        }

        public TimeSpan Duration
        {
            get
            {
                TimeSpan duration = this.duration;
                if (duration == TimeSpan.Zero)
                {
                    foreach (ICodec codec in this.codecs)
                    {
                        if ((codec != null) && (codec.Duration > duration))
                        {
                            duration = codec.Duration;
                        }
                    }
                }
                return duration;
            }
        }

        public TagLib.MediaTypes MediaTypes
        {
            get
            {
                TagLib.MediaTypes none = TagLib.MediaTypes.None;
                foreach (ICodec codec in this.codecs)
                {
                    if (codec != null)
                    {
                        none |= codec.MediaTypes;
                    }
                }
                return none;
            }
        }

        public int VideoHeight
        {
            get
            {
                foreach (ICodec codec in this.codecs)
                {
                    if ((codec != null) && ((codec.MediaTypes & TagLib.MediaTypes.Video) != TagLib.MediaTypes.None))
                    {
                        IVideoCodec codec2 = codec as IVideoCodec;
                        if ((codec2 != null) && (codec2.VideoHeight != 0))
                        {
                            return codec2.VideoHeight;
                        }
                    }
                }
                return 0;
            }
        }

        public int VideoWidth
        {
            get
            {
                foreach (ICodec codec in this.codecs)
                {
                    if ((codec != null) && ((codec.MediaTypes & TagLib.MediaTypes.Video) != TagLib.MediaTypes.None))
                    {
                        IVideoCodec codec2 = codec as IVideoCodec;
                        if ((codec2 != null) && (codec2.VideoWidth != 0))
                        {
                            return codec2.VideoWidth;
                        }
                    }
                }
                return 0;
            }
        }
    }
}

