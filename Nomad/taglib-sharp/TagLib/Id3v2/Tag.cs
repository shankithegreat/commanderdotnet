namespace TagLib.Id3v2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using TagLib;

    public class Tag : TagLib.Tag, IEnumerable, IEnumerable<Frame>, ICloneable
    {
        private static StringType default_string_type = StringType.UTF8;
        private static byte default_version = 3;
        private ExtendedHeader extended_header;
        private static bool force_default_string_type = false;
        private static bool force_default_version = false;
        private List<Frame> frame_list;
        private Header header;
        private static string language = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;
        private static bool use_numeric_genres = true;

        public Tag()
        {
            this.header = new Header();
            this.frame_list = new List<Frame>();
        }

        public Tag(ByteVector data)
        {
            this.header = new Header();
            this.frame_list = new List<Frame>();
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Count < 10L)
            {
                throw new CorruptFileException("Does not contain enough header data.");
            }
            this.header = new Header(data);
            if (this.header.TagSize != 0)
            {
                if ((data.Count - 10L) < this.header.TagSize)
                {
                    throw new CorruptFileException("Does not contain enough tag data.");
                }
                this.Parse(data.Mid(10, (int) this.header.TagSize));
            }
        }

        public Tag(File file, long position)
        {
            this.header = new Header();
            this.frame_list = new List<Frame>();
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            file.Mode = File.AccessMode.Read;
            if ((position < 0L) || (position > (file.Length - 10L)))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            this.Read(file, position);
        }

        public void AddFrame(Frame frame)
        {
            if (frame == null)
            {
                throw new ArgumentNullException("frame");
            }
            this.frame_list.Add(frame);
        }

        public override void Clear()
        {
            this.frame_list.Clear();
        }

        public TagLib.Id3v2.Tag Clone()
        {
            TagLib.Id3v2.Tag tag = new TagLib.Id3v2.Tag {
                header = this.header
            };
            if (tag.extended_header != null)
            {
                tag.extended_header = this.extended_header.Clone();
            }
            foreach (Frame frame in this.frame_list)
            {
                tag.frame_list.Add(frame.Clone());
            }
            return tag;
        }

        public override void CopyTo(TagLib.Tag target, bool overwrite)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            TagLib.Id3v2.Tag tag = target as TagLib.Id3v2.Tag;
            if (tag == null)
            {
                base.CopyTo(target, overwrite);
            }
            else
            {
                List<Frame> list = new List<Frame>(this.frame_list);
                while (list.Count > 0)
                {
                    ByteVector frameId = list[0].FrameId;
                    bool flag = true;
                    if (overwrite)
                    {
                        tag.RemoveFrames(frameId);
                    }
                    else
                    {
                        foreach (Frame frame in tag.frame_list)
                        {
                            if (frame.FrameId.Equals(frameId))
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    int index = 0;
                    while (index < list.Count)
                    {
                        if (list[index].FrameId.Equals(frameId))
                        {
                            if (flag)
                            {
                                tag.frame_list.Add(list[index].Clone());
                            }
                            list.RemoveAt(index);
                        }
                        else
                        {
                            index++;
                        }
                    }
                }
            }
        }

        public IEnumerator<Frame> GetEnumerator()
        {
            return this.frame_list.GetEnumerator();
        }

        [DebuggerHidden]
        public IEnumerable<T> GetFrames<T>() where T: Frame
        {
            return new <GetFrames>c__Iterator5<T> { <>f__this = this, $PC = -2 };
        }

        public IEnumerable<Frame> GetFrames()
        {
            return this.frame_list;
        }

        [DebuggerHidden]
        public IEnumerable<T> GetFrames<T>(ByteVector ident) where T: Frame
        {
            return new <GetFrames>c__Iterator6<T> { ident = ident, <$>ident = ident, <>f__this = this, $PC = -2 };
        }

        [DebuggerHidden]
        public IEnumerable<Frame> GetFrames(ByteVector ident)
        {
            return new <GetFrames>c__Iterator4 { ident = ident, <$>ident = ident, <>f__this = this, $PC = -2 };
        }

        private string[] GetTextAsArray(ByteVector ident)
        {
            TextInformationFrame frame = TextInformationFrame.Get(this, ident, false);
            return ((frame != null) ? frame.Text : new string[0]);
        }

        private string GetTextAsString(ByteVector ident)
        {
            TextInformationFrame frame = TextInformationFrame.Get(this, ident, false);
            string str = (frame != null) ? frame.ToString() : null;
            return (!string.IsNullOrEmpty(str) ? str : null);
        }

        private uint GetTextAsUInt32(ByteVector ident, int index)
        {
            string textAsString = this.GetTextAsString(ident);
            if (textAsString != null)
            {
                uint num;
                char[] separator = new char[] { '/' };
                string[] strArray = textAsString.Split(separator, (int) (index + 2));
                if (strArray.Length < (index + 1))
                {
                    return 0;
                }
                if (uint.TryParse(strArray[index], out num))
                {
                    return num;
                }
            }
            return 0;
        }

        private string GetUfidText(string owner)
        {
            UniqueFileIdentifierFrame frame = UniqueFileIdentifierFrame.Get(this, owner, false);
            string str = (frame != null) ? frame.Identifier.ToString() : null;
            return (!string.IsNullOrEmpty(str) ? str : null);
        }

        private string GetUserTextAsString(string description)
        {
            UserTextInformationFrame frame = UserTextInformationFrame.Get(this, description, false);
            string str = (frame != null) ? string.Join(";", frame.Text) : null;
            return (!string.IsNullOrEmpty(str) ? str : null);
        }

        private void MakeFirstOfType(Frame frame)
        {
            ByteVector frameId = frame.FrameId;
            Frame item = null;
            for (int i = 0; i < this.frame_list.Count; i++)
            {
                if (item == null)
                {
                    if (!this.frame_list[i].FrameId.Equals(frameId))
                    {
                        continue;
                    }
                    item = frame;
                }
                Frame frame3 = this.frame_list[i];
                this.frame_list[i] = item;
                item = frame3;
                if (item == frame)
                {
                    return;
                }
            }
            if (item != null)
            {
                this.frame_list.Add(item);
            }
        }

        protected void Parse(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            bool alreadyUnsynched = (this.header.MajorVersion < 4) && (((byte) (this.header.Flags & (HeaderFlags.None | HeaderFlags.Unsynchronisation))) != 0);
            if (alreadyUnsynched)
            {
                SynchData.ResynchByteVector(data);
            }
            int offset = 0;
            int count = data.Count;
            if (((byte) (this.header.Flags & HeaderFlags.ExtendedHeader)) != 0)
            {
                this.extended_header = new ExtendedHeader(data, this.header.MajorVersion);
                if (this.extended_header.Size <= data.Count)
                {
                    offset += (int) this.extended_header.Size;
                    count -= (int) this.extended_header.Size;
                }
            }
            TextInformationFrame frame = null;
            TextInformationFrame frame2 = null;
            TextInformationFrame frame3 = null;
            TextInformationFrame frame4 = null;
            while (offset < (count - FrameHeader.Size(this.header.MajorVersion)))
            {
                if (data[offset] == 0)
                {
                    break;
                }
                Frame frame5 = null;
                try
                {
                    frame5 = FrameFactory.CreateFrame(data, ref offset, this.header.MajorVersion, alreadyUnsynched);
                }
                catch (NotImplementedException)
                {
                    continue;
                }
                if (frame5 == null)
                {
                    break;
                }
                if (frame5.Size != 0)
                {
                    this.AddFrame(frame5);
                    if (this.header.MajorVersion != 4)
                    {
                        if ((frame == null) && frame5.FrameId.Equals((ByteVector) FrameType.TDRC))
                        {
                            frame = frame5 as TextInformationFrame;
                        }
                        else
                        {
                            if ((frame2 == null) && frame5.FrameId.Equals((ByteVector) FrameType.TYER))
                            {
                                frame2 = frame5 as TextInformationFrame;
                                continue;
                            }
                            if ((frame3 == null) && frame5.FrameId.Equals((ByteVector) FrameType.TDAT))
                            {
                                frame3 = frame5 as TextInformationFrame;
                                continue;
                            }
                            if ((frame4 == null) && frame5.FrameId.Equals((ByteVector) FrameType.TIME))
                            {
                                frame4 = frame5 as TextInformationFrame;
                            }
                        }
                    }
                }
            }
            if (((frame != null) && (frame3 != null)) && (frame.ToString().Length <= 4))
            {
                string str = frame.ToString();
                if (str.Length == 4)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(str);
                    if (frame3 != null)
                    {
                        string str2 = frame3.ToString();
                        if (str2.Length == 4)
                        {
                            builder.Append("-").Append(str2, 0, 2).Append("-").Append(str2, 2, 2);
                            if (frame4 != null)
                            {
                                string str3 = frame4.ToString();
                                if (str3.Length == 4)
                                {
                                    builder.Append("T").Append(str3, 0, 2).Append(":").Append(str3, 2, 2);
                                }
                                this.RemoveFrames(FrameType.TIME);
                            }
                        }
                        this.RemoveFrames(FrameType.TDAT);
                    }
                    frame.Text = new string[] { builder.ToString() };
                }
            }
        }

        protected void Read(File file, long position)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            file.Mode = File.AccessMode.Read;
            if ((position < 0L) || (position > (file.Length - 10L)))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            file.Seek(position);
            this.header = new Header(file.ReadBlock(10));
            if (this.header.TagSize != 0)
            {
                this.Parse(file.ReadBlock((int) this.header.TagSize));
            }
        }

        public void RemoveFrame(Frame frame)
        {
            if (frame == null)
            {
                throw new ArgumentNullException("frame");
            }
            if (this.frame_list.Contains(frame))
            {
                this.frame_list.Remove(frame);
            }
        }

        public void RemoveFrames(ByteVector ident)
        {
            if (ident == null)
            {
                throw new ArgumentNullException("ident");
            }
            if (ident.Count != 4)
            {
                throw new ArgumentException("Identifier must be four bytes long.", "ident");
            }
            for (int i = this.frame_list.Count - 1; i >= 0; i--)
            {
                if (this.frame_list[i].FrameId.Equals(ident))
                {
                    this.frame_list.RemoveAt(i);
                }
            }
        }

        public ByteVector Render()
        {
            bool flag = ((byte) (this.header.Flags & HeaderFlags.FooterPresent)) != 0;
            bool flag2 = (((byte) (this.header.Flags & (HeaderFlags.None | HeaderFlags.Unsynchronisation))) != 0) && (this.Version >= 4);
            bool flag3 = (((byte) (this.header.Flags & (HeaderFlags.None | HeaderFlags.Unsynchronisation))) != 0) && (this.Version < 4);
            this.header.MajorVersion = !flag ? this.Version : ((byte) 4);
            ByteVector data = new ByteVector();
            this.header.Flags = (HeaderFlags) ((byte) (((int) this.header.Flags) & 0xbf));
            foreach (Frame frame in this.frame_list)
            {
                if (flag2)
                {
                    frame.Flags = (FrameFlags) ((ushort) (frame.Flags | FrameFlags.Unsynchronisation));
                }
                if (((ushort) (frame.Flags & (FrameFlags.None | FrameFlags.TagAlterPreservation))) == 0)
                {
                    try
                    {
                        data.Add(frame.Render(this.header.MajorVersion));
                        continue;
                    }
                    catch (NotImplementedException)
                    {
                        continue;
                    }
                }
            }
            if (flag3)
            {
                SynchData.UnsynchByteVector(data);
            }
            if (!flag)
            {
                data.Add(new ByteVector((data.Count >= this.header.TagSize) ? ((int) 0x400L) : (((int) this.header.TagSize) - data.Count)));
            }
            this.header.TagSize = (uint) data.Count;
            data.Insert(0, this.header.Render());
            if (flag)
            {
                data.Add(new Footer(this.header).Render());
            }
            return data;
        }

        public void ReplaceFrame(Frame oldFrame, Frame newFrame)
        {
            if (oldFrame == null)
            {
                throw new ArgumentNullException("oldFrame");
            }
            if (newFrame == null)
            {
                throw new ArgumentNullException("newFrame");
            }
            if (oldFrame != newFrame)
            {
                int index = this.frame_list.IndexOf(oldFrame);
                if (index >= 0)
                {
                    this.frame_list[index] = newFrame;
                }
                else
                {
                    this.frame_list.Add(newFrame);
                }
            }
        }

        public void SetNumberFrame(ByteVector ident, uint number, uint count)
        {
            if (ident == null)
            {
                throw new ArgumentNullException("ident");
            }
            if (ident.Count != 4)
            {
                throw new ArgumentException("Identifier must be four bytes long.", "ident");
            }
            if ((number == 0) && (count == 0))
            {
                this.RemoveFrames(ident);
            }
            else if (count != 0)
            {
                string[] text = new string[1];
                object[] args = new object[] { number, count };
                text[0] = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", args);
                this.SetTextFrame(ident, text);
            }
            else
            {
                string[] textArray2 = new string[] { number.ToString(CultureInfo.InvariantCulture) };
                this.SetTextFrame(ident, textArray2);
            }
        }

        [Obsolete("Use SetTextFrame(ByteVector,String[])")]
        public void SetTextFrame(ByteVector ident, StringCollection text)
        {
            if ((text == null) || (text.Count == 0))
            {
                this.RemoveFrames(ident);
            }
            else
            {
                this.SetTextFrame(ident, text.ToArray());
            }
        }

        public void SetTextFrame(ByteVector ident, params string[] text)
        {
            if (ident == null)
            {
                throw new ArgumentNullException("ident");
            }
            if (ident.Count != 4)
            {
                throw new ArgumentException("Identifier must be four bytes long.", "ident");
            }
            bool flag = true;
            if (text != null)
            {
                for (int i = 0; flag && (i < text.Length); i++)
                {
                    if (!string.IsNullOrEmpty(text[i]))
                    {
                        flag = false;
                    }
                }
            }
            if (flag)
            {
                this.RemoveFrames(ident);
            }
            else
            {
                TextInformationFrame frame = TextInformationFrame.Get(this, ident, true);
                frame.Text = text;
                frame.TextEncoding = DefaultEncoding;
            }
        }

        private void SetUfidText(string owner, string text)
        {
            UniqueFileIdentifierFrame frame = UniqueFileIdentifierFrame.Get(this, owner, true);
            if (!string.IsNullOrEmpty(text))
            {
                ByteVector vector = ByteVector.FromString(text, StringType.UTF8);
                frame.Identifier = vector;
            }
            else
            {
                this.RemoveFrame(frame);
            }
        }

        private void SetUserTextAsString(string description, string text)
        {
            UserTextInformationFrame frame = UserTextInformationFrame.Get(this, description, true);
            if (!string.IsNullOrEmpty(text))
            {
                char[] separator = new char[] { ';' };
                frame.Text = text.Split(separator);
            }
            else
            {
                this.RemoveFrame(frame);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.frame_list.GetEnumerator();
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public override string Album
        {
            get
            {
                return this.GetTextAsString(FrameType.TALB);
            }
            set
            {
                string[] text = new string[] { value };
                this.SetTextFrame(FrameType.TALB, text);
            }
        }

        public override string[] AlbumArtists
        {
            get
            {
                return this.GetTextAsArray(FrameType.TPE2);
            }
            set
            {
                this.SetTextFrame(FrameType.TPE2, value);
            }
        }

        public override string[] AlbumArtistsSort
        {
            get
            {
                return this.GetTextAsArray(FrameType.TSO2);
            }
            set
            {
                this.SetTextFrame(FrameType.TSO2, value);
            }
        }

        public override string AlbumSort
        {
            get
            {
                return this.GetTextAsString(FrameType.TSOA);
            }
            set
            {
                string[] text = new string[] { value };
                this.SetTextFrame(FrameType.TSOA, text);
            }
        }

        public override string AmazonId
        {
            get
            {
                return this.GetUserTextAsString("ASIN");
            }
            set
            {
                this.SetUserTextAsString("ASIN", value);
            }
        }

        public override uint BeatsPerMinute
        {
            get
            {
                double num;
                string textAsString = this.GetTextAsString(FrameType.TBPM);
                if ((textAsString != null) && (double.TryParse(textAsString, out num) && (num >= 0.0)))
                {
                    return (uint) Math.Round(num);
                }
                return 0;
            }
            set
            {
                this.SetNumberFrame(FrameType.TBPM, value, 0);
            }
        }

        public override string Comment
        {
            get
            {
                CommentsFrame frame = CommentsFrame.GetPreferred(this, string.Empty, Language);
                return ((frame == null) ? null : frame.ToString());
            }
            set
            {
                CommentsFrame frame;
                if (string.IsNullOrEmpty(value))
                {
                    while ((frame = CommentsFrame.GetPreferred(this, string.Empty, Language)) != null)
                    {
                        this.RemoveFrame(frame);
                    }
                }
                else
                {
                    frame = CommentsFrame.Get(this, string.Empty, Language, true);
                    frame.Text = value;
                    frame.TextEncoding = DefaultEncoding;
                    this.MakeFirstOfType(frame);
                }
            }
        }

        public override string[] Composers
        {
            get
            {
                return this.GetTextAsArray(FrameType.TCOM);
            }
            set
            {
                this.SetTextFrame(FrameType.TCOM, value);
            }
        }

        public override string[] ComposersSort
        {
            get
            {
                return this.GetTextAsArray(FrameType.TSOC);
            }
            set
            {
                this.SetTextFrame(FrameType.TSOC, value);
            }
        }

        public override string Conductor
        {
            get
            {
                return this.GetTextAsString(FrameType.TPE3);
            }
            set
            {
                string[] text = new string[] { value };
                this.SetTextFrame(FrameType.TPE3, text);
            }
        }

        public override string Copyright
        {
            get
            {
                return this.GetTextAsString(FrameType.TCOP);
            }
            set
            {
                string[] text = new string[] { value };
                this.SetTextFrame(FrameType.TCOP, text);
            }
        }

        public static StringType DefaultEncoding
        {
            get
            {
                return default_string_type;
            }
            set
            {
                default_string_type = value;
            }
        }

        public static byte DefaultVersion
        {
            get
            {
                return default_version;
            }
            set
            {
                if ((value < 2) || (value > 4))
                {
                    throw new ArgumentOutOfRangeException("value", "Version must be 2, 3, or 4");
                }
                default_version = value;
            }
        }

        public override uint Disc
        {
            get
            {
                return this.GetTextAsUInt32(FrameType.TPOS, 0);
            }
            set
            {
                this.SetNumberFrame(FrameType.TPOS, value, this.DiscCount);
            }
        }

        public override uint DiscCount
        {
            get
            {
                return this.GetTextAsUInt32(FrameType.TPOS, 1);
            }
            set
            {
                this.SetNumberFrame(FrameType.TPOS, this.Disc, value);
            }
        }

        public HeaderFlags Flags
        {
            get
            {
                return this.header.Flags;
            }
            set
            {
                this.header.Flags = value;
            }
        }

        public static bool ForceDefaultEncoding
        {
            get
            {
                return force_default_string_type;
            }
            set
            {
                force_default_string_type = value;
            }
        }

        public static bool ForceDefaultVersion
        {
            get
            {
                return force_default_version;
            }
            set
            {
                force_default_version = value;
            }
        }

        public override string[] Genres
        {
            get
            {
                string[] textAsArray = this.GetTextAsArray(FrameType.TCON);
                if (textAsArray.Length == 0)
                {
                    return textAsArray;
                }
                List<string> list = new List<string>();
                foreach (string str in textAsArray)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        string item = TagLib.Genres.IndexToAudio(str);
                        if (item != null)
                        {
                            list.Add(item);
                        }
                        else
                        {
                            list.Add(str);
                        }
                    }
                }
                return list.ToArray();
            }
            set
            {
                if ((value == null) || !use_numeric_genres)
                {
                    this.SetTextFrame(FrameType.TCON, value);
                }
                else
                {
                    value = (string[]) value.Clone();
                    for (int i = 0; i < value.Length; i++)
                    {
                        int num2 = TagLib.Genres.AudioToIndex(value[i]);
                        if (num2 != 0xff)
                        {
                            value[i] = num2.ToString(CultureInfo.InvariantCulture);
                        }
                    }
                    this.SetTextFrame(FrameType.TCON, value);
                }
            }
        }

        public override string Grouping
        {
            get
            {
                return this.GetTextAsString(FrameType.TIT1);
            }
            set
            {
                string[] text = new string[] { value };
                this.SetTextFrame(FrameType.TIT1, text);
            }
        }

        public bool IsCompilation
        {
            get
            {
                string textAsString = this.GetTextAsString(FrameType.TCMP);
                return (!string.IsNullOrEmpty(textAsString) && (textAsString != "0"));
            }
            set
            {
                string[] text = new string[] { !value ? null : "1" };
                this.SetTextFrame(FrameType.TCMP, text);
            }
        }

        public override bool IsEmpty
        {
            get
            {
                return (this.frame_list.Count == 0);
            }
        }

        public static string Language
        {
            get
            {
                return language;
            }
            set
            {
                language = ((value != null) && (value.Length >= 3)) ? value.Substring(0, 3) : "   ";
            }
        }

        public override string Lyrics
        {
            get
            {
                UnsynchronisedLyricsFrame frame = UnsynchronisedLyricsFrame.GetPreferred(this, string.Empty, Language);
                return ((frame == null) ? null : frame.ToString());
            }
            set
            {
                UnsynchronisedLyricsFrame frame;
                if (string.IsNullOrEmpty(value))
                {
                    while ((frame = UnsynchronisedLyricsFrame.GetPreferred(this, string.Empty, Language)) != null)
                    {
                        this.RemoveFrame(frame);
                    }
                }
                else
                {
                    frame = UnsynchronisedLyricsFrame.Get(this, string.Empty, Language, true);
                    frame.Text = value;
                    frame.TextEncoding = DefaultEncoding;
                }
            }
        }

        public override string MusicBrainzArtistId
        {
            get
            {
                return this.GetUserTextAsString("MusicBrainz Artist Id");
            }
            set
            {
                this.SetUserTextAsString("MusicBrainz Artist Id", value);
            }
        }

        public override string MusicBrainzDiscId
        {
            get
            {
                return this.GetUserTextAsString("MusicBrainz Disc Id");
            }
            set
            {
                this.SetUserTextAsString("MusicBrainz Disc Id", value);
            }
        }

        public override string MusicBrainzReleaseArtistId
        {
            get
            {
                return this.GetUserTextAsString("MusicBrainz Album Artist Id");
            }
            set
            {
                this.SetUserTextAsString("MusicBrainz Album Artist Id", value);
            }
        }

        public override string MusicBrainzReleaseCountry
        {
            get
            {
                return this.GetUserTextAsString("MusicBrainz Album Release Country");
            }
            set
            {
                this.SetUserTextAsString("MusicBrainz Album Release Country", value);
            }
        }

        public override string MusicBrainzReleaseId
        {
            get
            {
                return this.GetUserTextAsString("MusicBrainz Album Id");
            }
            set
            {
                this.SetUserTextAsString("MusicBrainz Album Id", value);
            }
        }

        public override string MusicBrainzReleaseStatus
        {
            get
            {
                return this.GetUserTextAsString("MusicBrainz Album Status");
            }
            set
            {
                this.SetUserTextAsString("MusicBrainz Album Status", value);
            }
        }

        public override string MusicBrainzReleaseType
        {
            get
            {
                return this.GetUserTextAsString("MusicBrainz Album Type");
            }
            set
            {
                this.SetUserTextAsString("MusicBrainz Album Type", value);
            }
        }

        public override string MusicBrainzTrackId
        {
            get
            {
                return this.GetUfidText("http://musicbrainz.org");
            }
            set
            {
                this.SetUfidText("http://musicbrainz.org", value);
            }
        }

        public override string MusicIpId
        {
            get
            {
                return this.GetUserTextAsString("MusicIP PUID");
            }
            set
            {
                this.SetUserTextAsString("MusicIP PUID", value);
            }
        }

        public override string[] Performers
        {
            get
            {
                return this.GetTextAsArray(FrameType.TPE1);
            }
            set
            {
                this.SetTextFrame(FrameType.TPE1, value);
            }
        }

        public override string[] PerformersSort
        {
            get
            {
                return this.GetTextAsArray(FrameType.TSOP);
            }
            set
            {
                this.SetTextFrame(FrameType.TSOP, value);
            }
        }

        public override IPicture[] Pictures
        {
            get
            {
                return new List<AttachedPictureFrame>(this.GetFrames<AttachedPictureFrame>(FrameType.APIC)).ToArray();
            }
            set
            {
                this.RemoveFrames(FrameType.APIC);
                if ((value != null) && (value.Length != 0))
                {
                    foreach (IPicture picture in value)
                    {
                        AttachedPictureFrame frame = picture as AttachedPictureFrame;
                        if (frame == null)
                        {
                            frame = new AttachedPictureFrame(picture);
                        }
                        this.AddFrame(frame);
                    }
                }
            }
        }

        public override TagLib.TagTypes TagTypes
        {
            get
            {
                return (TagLib.TagTypes.None | TagLib.TagTypes.Id3v2);
            }
        }

        public override string Title
        {
            get
            {
                return this.GetTextAsString(FrameType.TIT2);
            }
            set
            {
                string[] text = new string[] { value };
                this.SetTextFrame(FrameType.TIT2, text);
            }
        }

        public override string TitleSort
        {
            get
            {
                return this.GetTextAsString(FrameType.TSOT);
            }
            set
            {
                string[] text = new string[] { value };
                this.SetTextFrame(FrameType.TSOT, text);
            }
        }

        public override uint Track
        {
            get
            {
                return this.GetTextAsUInt32(FrameType.TRCK, 0);
            }
            set
            {
                this.SetNumberFrame(FrameType.TRCK, value, this.TrackCount);
            }
        }

        public override uint TrackCount
        {
            get
            {
                return this.GetTextAsUInt32(FrameType.TRCK, 1);
            }
            set
            {
                this.SetNumberFrame(FrameType.TRCK, this.Track, value);
            }
        }

        public static bool UseNumericGenres
        {
            get
            {
                return use_numeric_genres;
            }
            set
            {
                use_numeric_genres = value;
            }
        }

        public byte Version
        {
            get
            {
                return (!ForceDefaultVersion ? this.header.MajorVersion : DefaultVersion);
            }
            set
            {
                if ((value < 2) || (value > 4))
                {
                    throw new ArgumentOutOfRangeException("value", "Version must be 2, 3, or 4");
                }
                this.header.MajorVersion = value;
            }
        }

        public override uint Year
        {
            get
            {
                uint num;
                string textAsString = this.GetTextAsString(FrameType.TDRC);
                if (((textAsString != null) && (textAsString.Length >= 4)) && uint.TryParse(textAsString.Substring(0, 4), out num))
                {
                    return num;
                }
                return 0;
            }
            set
            {
                if (value > 0x270f)
                {
                    value = 0;
                }
                this.SetNumberFrame(FrameType.TDRC, value, 0);
            }
        }

        [CompilerGenerated]
        private sealed class <GetFrames>c__Iterator4 : IDisposable, IEnumerator, IEnumerable, IEnumerator<Frame>, IEnumerable<Frame>
        {
            internal Frame $current;
            internal int $PC;
            internal ByteVector <$>ident;
            internal List<Frame>.Enumerator <$s_201>__0;
            internal TagLib.Id3v2.Tag <>f__this;
            internal Frame <f>__1;
            internal ByteVector ident;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.<$s_201>__0.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        if (this.ident == null)
                        {
                            throw new ArgumentNullException("ident");
                        }
                        if (this.ident.Count != 4)
                        {
                            throw new ArgumentException("Identifier must be four bytes long.", "ident");
                        }
                        this.<$s_201>__0 = this.<>f__this.frame_list.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0101;
                }
                try
                {
                    while (this.<$s_201>__0.MoveNext())
                    {
                        this.<f>__1 = this.<$s_201>__0.Current;
                        if (this.<f>__1.FrameId.Equals(this.ident))
                        {
                            this.$current = this.<f>__1;
                            this.$PC = 1;
                            flag = true;
                            return true;
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_201>__0.Dispose();
                }
                this.$PC = -1;
            Label_0101:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<Frame> IEnumerable<Frame>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new TagLib.Id3v2.Tag.<GetFrames>c__Iterator4 { <>f__this = this.<>f__this, ident = this.<$>ident };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TagLib.Id3v2.Frame>.GetEnumerator();
            }

            Frame IEnumerator<Frame>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetFrames>c__Iterator5<T> : IDisposable, IEnumerator, IEnumerable, IEnumerable<T>, IEnumerator<T> where T: Frame
        {
            internal T $current;
            internal int $PC;
            internal List<Frame>.Enumerator <$s_202>__0;
            internal TagLib.Id3v2.Tag <>f__this;
            internal Frame <f>__1;
            internal T <tf>__2;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.<$s_202>__0.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.<$s_202>__0 = this.<>f__this.frame_list.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00CF;
                }
                try
                {
                    while (this.<$s_202>__0.MoveNext())
                    {
                        this.<f>__1 = this.<$s_202>__0.Current;
                        this.<tf>__2 = this.<f>__1 as T;
                        if (this.<tf>__2 != null)
                        {
                            this.$current = this.<tf>__2;
                            this.$PC = 1;
                            flag = true;
                            return true;
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_202>__0.Dispose();
                }
                this.$PC = -1;
            Label_00CF:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new TagLib.Id3v2.Tag.<GetFrames>c__Iterator5<T> { <>f__this = this.<>f__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
            }

            T IEnumerator<T>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetFrames>c__Iterator6<T> : IDisposable, IEnumerator, IEnumerable, IEnumerable<T>, IEnumerator<T> where T: Frame
        {
            internal T $current;
            internal int $PC;
            internal ByteVector <$>ident;
            internal List<Frame>.Enumerator <$s_203>__0;
            internal TagLib.Id3v2.Tag <>f__this;
            internal Frame <f>__1;
            internal T <tf>__2;
            internal ByteVector ident;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.<$s_203>__0.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        if (this.ident == null)
                        {
                            throw new ArgumentNullException("ident");
                        }
                        if (this.ident.Count != 4)
                        {
                            throw new ArgumentException("Identifier must be four bytes long.", "ident");
                        }
                        this.<$s_203>__0 = this.<>f__this.frame_list.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0127;
                }
                try
                {
                    while (this.<$s_203>__0.MoveNext())
                    {
                        this.<f>__1 = this.<$s_203>__0.Current;
                        this.<tf>__2 = this.<f>__1 as T;
                        if ((this.<tf>__2 != null) && this.<f>__1.FrameId.Equals(this.ident))
                        {
                            this.$current = this.<tf>__2;
                            this.$PC = 1;
                            flag = true;
                            return true;
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_203>__0.Dispose();
                }
                this.$PC = -1;
            Label_0127:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new TagLib.Id3v2.Tag.<GetFrames>c__Iterator6<T> { <>f__this = this.<>f__this, ident = this.<$>ident };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
            }

            T IEnumerator<T>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

