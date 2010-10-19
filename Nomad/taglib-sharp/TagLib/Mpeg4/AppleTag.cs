namespace TagLib.Mpeg4
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TagLib;

    public class AppleTag : Tag, IEnumerable, IEnumerable<Box>
    {
        private AppleItemListBox ilst_box;
        private IsoMetaBox meta_box;

        public AppleTag(IsoUserDataBox box)
        {
            if (box == null)
            {
                throw new ArgumentNullException("box");
            }
            this.meta_box = box.GetChild(BoxType.Meta) as IsoMetaBox;
            if (this.meta_box == null)
            {
                this.meta_box = new IsoMetaBox("mdir", null);
                box.AddChild(this.meta_box);
            }
            this.ilst_box = this.meta_box.GetChild(BoxType.Ilst) as AppleItemListBox;
            if (this.ilst_box == null)
            {
                this.ilst_box = new AppleItemListBox();
                this.meta_box.AddChild(this.ilst_box);
            }
        }

        public override void Clear()
        {
            this.ilst_box.ClearChildren();
        }

        public void ClearData(ByteVector type)
        {
            this.ilst_box.RemoveChild(FixId(type));
        }

        public IEnumerable<AppleDataBox> DataBoxes(params ByteVector[] types)
        {
            return this.DataBoxes((IEnumerable<ByteVector>) types);
        }

        [DebuggerHidden]
        public IEnumerable<AppleDataBox> DataBoxes(IEnumerable<ByteVector> types)
        {
            return new <DataBoxes>c__Iterator7 { types = types, <$>types = types, <>f__this = this, $PC = -2 };
        }

        [DebuggerHidden]
        public IEnumerable<AppleDataBox> DataBoxes(string mean, string name)
        {
            return new <DataBoxes>c__Iterator8 { mean = mean, name = name, <$>mean = mean, <$>name = name, <>f__this = this, $PC = -2 };
        }

        public void DetachIlst()
        {
            this.meta_box.RemoveChild(this.ilst_box);
        }

        internal static ReadOnlyByteVector FixId(ByteVector id)
        {
            if (id.Count == 4)
            {
                ReadOnlyByteVector vector = id as ReadOnlyByteVector;
                if (vector != null)
                {
                    return vector;
                }
                return new ReadOnlyByteVector(id);
            }
            if (id.Count == 3)
            {
                return new ReadOnlyByteVector(new byte[] { 0xa9, id[0], id[1], id[2] });
            }
            return null;
        }

        private AppleDataBox GetDashAtoms(string meanstring, string namestring)
        {
            IEnumerator<Box> enumerator = this.ilst_box.Children.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Box current = enumerator.Current;
                    if (current.BoxType == BoxType.DASH)
                    {
                        AppleAdditionalInfoBox child = (AppleAdditionalInfoBox) current.GetChild(BoxType.Mean);
                        AppleAdditionalInfoBox box3 = (AppleAdditionalInfoBox) current.GetChild(BoxType.Name);
                        if ((((child != null) && (box3 != null)) && (child.Text == meanstring)) && (box3.Text == namestring))
                        {
                            return (AppleDataBox) current.GetChild(BoxType.Data);
                        }
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
            return null;
        }

        public string GetDashBox(string meanstring, string namestring)
        {
            AppleDataBox dashAtoms = this.GetDashAtoms(meanstring, namestring);
            if (dashAtoms != null)
            {
                return dashAtoms.Text;
            }
            return null;
        }

        public IEnumerator<Box> GetEnumerator()
        {
            return this.ilst_box.Children.GetEnumerator();
        }

        private AppleAnnotationBox GetParentDashBox(string meanstring, string namestring)
        {
            IEnumerator<Box> enumerator = this.ilst_box.Children.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Box current = enumerator.Current;
                    if (current.BoxType == BoxType.DASH)
                    {
                        AppleAdditionalInfoBox child = (AppleAdditionalInfoBox) current.GetChild(BoxType.Mean);
                        AppleAdditionalInfoBox box3 = (AppleAdditionalInfoBox) current.GetChild(BoxType.Name);
                        if ((((child != null) && (box3 != null)) && (child.Text == meanstring)) && (box3.Text == namestring))
                        {
                            return (AppleAnnotationBox) current;
                        }
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
            return null;
        }

        public string[] GetText(ByteVector type)
        {
            List<string> list = new List<string>();
            ByteVector[] types = new ByteVector[] { type };
            IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AppleDataBox current = enumerator.Current;
                    if (current.Text != null)
                    {
                        char[] separator = new char[] { ';' };
                        foreach (string str in current.Text.Split(separator))
                        {
                            list.Add(str.Trim());
                        }
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
            return list.ToArray();
        }

        public void SetDashBox(string meanstring, string namestring, string datastring)
        {
            AppleDataBox dashAtoms = this.GetDashAtoms(meanstring, namestring);
            if ((dashAtoms != null) && string.IsNullOrEmpty(datastring))
            {
                AppleAnnotationBox parentDashBox = this.GetParentDashBox(meanstring, namestring);
                parentDashBox.ClearChildren();
                this.ilst_box.RemoveChild(parentDashBox);
            }
            else if (dashAtoms != null)
            {
                dashAtoms.Text = datastring;
            }
            else
            {
                AppleAdditionalInfoBox box = new AppleAdditionalInfoBox(BoxType.Mean, 0, 1);
                AppleAdditionalInfoBox box4 = new AppleAdditionalInfoBox(BoxType.Name, 0, 1);
                AppleDataBox box5 = new AppleDataBox(BoxType.Data, 1);
                box.Text = meanstring;
                box4.Text = namestring;
                box5.Text = datastring;
                AppleAnnotationBox box6 = new AppleAnnotationBox(BoxType.DASH);
                box6.AddChild(box);
                box6.AddChild(box4);
                box6.AddChild(box5);
                this.ilst_box.AddChild(box6);
            }
        }

        public void SetData(ByteVector type, AppleDataBox[] boxes)
        {
            type = FixId(type);
            bool flag = false;
            IEnumerator<Box> enumerator = this.ilst_box.Children.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Box current = enumerator.Current;
                    if (type == current.BoxType)
                    {
                        current.ClearChildren();
                        if (!flag)
                        {
                            flag = true;
                            foreach (AppleDataBox box2 in boxes)
                            {
                                current.AddChild(box2);
                            }
                        }
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
            if (!flag)
            {
                Box box = new AppleAnnotationBox(type);
                this.ilst_box.AddChild(box);
                foreach (AppleDataBox box4 in boxes)
                {
                    box.AddChild(box4);
                }
            }
        }

        public void SetData(ByteVector type, ByteVector data, uint flags)
        {
            if ((data == null) || (data.Count == 0))
            {
                this.ClearData(type);
            }
            else
            {
                ByteVector[] list = new ByteVector[] { data };
                this.SetData(type, new ByteVectorCollection(list), flags);
            }
        }

        public void SetData(ByteVector type, ByteVectorCollection data, uint flags)
        {
            if ((data == null) || (data.Count == 0))
            {
                this.ClearData(type);
            }
            else
            {
                AppleDataBox[] boxes = new AppleDataBox[data.Count];
                for (int i = 0; i < data.Count; i++)
                {
                    boxes[i] = new AppleDataBox(data[i], flags);
                }
                this.SetData(type, boxes);
            }
        }

        public void SetText(ByteVector type, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                this.ilst_box.RemoveChild(FixId(type));
            }
            else
            {
                ByteVectorCollection data = new ByteVectorCollection();
                data.Add(ByteVector.FromString(text, StringType.UTF8));
                this.SetData(type, data, 1);
            }
        }

        public void SetText(ByteVector type, string[] text)
        {
            if (text == null)
            {
                this.ilst_box.RemoveChild(FixId(type));
            }
            else
            {
                this.SetText(type, string.Join("; ", text));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.ilst_box.Children.GetEnumerator();
        }

        public override string Album
        {
            get
            {
                string[] text = this.GetText(BoxType.Alb);
                return ((text.Length != 0) ? text[0] : null);
            }
            set
            {
                this.SetText(BoxType.Alb, value);
            }
        }

        public override string[] AlbumArtists
        {
            get
            {
                return this.GetText(BoxType.Aart);
            }
            set
            {
                this.SetText(BoxType.Aart, value);
            }
        }

        public override string[] AlbumArtistsSort
        {
            get
            {
                return this.GetText(BoxType.Soaa);
            }
            set
            {
                this.SetText(BoxType.Soaa, value);
            }
        }

        public override string AlbumSort
        {
            get
            {
                string[] text = this.GetText(BoxType.Soal);
                return ((text.Length != 0) ? text[0] : null);
            }
            set
            {
                this.SetText(BoxType.Soal, value);
            }
        }

        public override string AmazonId
        {
            get
            {
                return this.GetDashBox("com.apple.iTunes", "ASIN");
            }
            set
            {
                this.SetDashBox("com.apple.iTunes", "ASIN", value);
            }
        }

        public override uint BeatsPerMinute
        {
            get
            {
                ByteVector[] types = new ByteVector[] { BoxType.Tmpo };
                IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AppleDataBox current = enumerator.Current;
                        if (current.Flags == 0x15)
                        {
                            return current.Data.ToUInt();
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
                return 0;
            }
            set
            {
                if (value == 0)
                {
                    this.ClearData(BoxType.Tmpo);
                }
                else
                {
                    this.SetData(BoxType.Tmpo, ByteVector.FromUShort((ushort) value), 0x15);
                }
            }
        }

        public override string Comment
        {
            get
            {
                string[] text = this.GetText(BoxType.Cmt);
                return ((text.Length != 0) ? text[0] : null);
            }
            set
            {
                this.SetText(BoxType.Cmt, value);
            }
        }

        public override string[] Composers
        {
            get
            {
                return this.GetText(BoxType.Wrt);
            }
            set
            {
                this.SetText(BoxType.Wrt, value);
            }
        }

        public override string[] ComposersSort
        {
            get
            {
                return this.GetText(BoxType.Soco);
            }
            set
            {
                this.SetText(BoxType.Soco, value);
            }
        }

        public override string Conductor
        {
            get
            {
                ByteVector[] types = new ByteVector[] { BoxType.Cond };
                IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AppleDataBox current = enumerator.Current;
                        return current.Text;
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                return null;
            }
            set
            {
                this.SetText(BoxType.Cond, value);
            }
        }

        public override string Copyright
        {
            get
            {
                ByteVector[] types = new ByteVector[] { BoxType.Cprt };
                IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AppleDataBox current = enumerator.Current;
                        return current.Text;
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                return null;
            }
            set
            {
                this.SetText(BoxType.Cprt, value);
            }
        }

        public override uint Disc
        {
            get
            {
                ByteVector[] types = new ByteVector[] { BoxType.Disk };
                IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AppleDataBox current = enumerator.Current;
                        if ((current.Flags == 0) && (current.Data.Count >= 4))
                        {
                            return current.Data.Mid(2, 2).ToUShort();
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
                return 0;
            }
            set
            {
                uint discCount = this.DiscCount;
                if ((value == 0) && (discCount == 0))
                {
                    this.ClearData(BoxType.Disk);
                }
                else
                {
                    ByteVector data = ByteVector.FromUShort(0);
                    data.Add(ByteVector.FromUShort((ushort) value));
                    data.Add(ByteVector.FromUShort((ushort) discCount));
                    data.Add(ByteVector.FromUShort(0));
                    this.SetData(BoxType.Disk, data, 0);
                }
            }
        }

        public override uint DiscCount
        {
            get
            {
                ByteVector[] types = new ByteVector[] { BoxType.Disk };
                IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AppleDataBox current = enumerator.Current;
                        if ((current.Flags == 0) && (current.Data.Count >= 6))
                        {
                            return current.Data.Mid(4, 2).ToUShort();
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
                return 0;
            }
            set
            {
                uint disc = this.Disc;
                if ((value == 0) && (disc == 0))
                {
                    this.ClearData(BoxType.Disk);
                }
                else
                {
                    ByteVector data = ByteVector.FromUShort(0);
                    data.Add(ByteVector.FromUShort((ushort) disc));
                    data.Add(ByteVector.FromUShort((ushort) value));
                    data.Add(ByteVector.FromUShort(0));
                    this.SetData(BoxType.Disk, data, 0);
                }
            }
        }

        public override string[] Genres
        {
            get
            {
                string[] text = this.GetText(BoxType.Gen);
                if (text.Length <= 0)
                {
                    ByteVector[] types = new ByteVector[] { BoxType.Gnre };
                    IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            AppleDataBox current = enumerator.Current;
                            if (current.Flags == 0)
                            {
                                ushort num = current.Data.ToUShort(true);
                                if (num != 0)
                                {
                                    string str = TagLib.Genres.IndexToAudio((byte) (num - 1));
                                    if (str != null)
                                    {
                                        return new string[] { str };
                                    }
                                }
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
                return text;
            }
            set
            {
                this.ClearData(BoxType.Gnre);
                this.SetText(BoxType.Gen, value);
            }
        }

        public override string Grouping
        {
            get
            {
                ByteVector[] types = new ByteVector[] { BoxType.Grp };
                IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AppleDataBox current = enumerator.Current;
                        return current.Text;
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                return null;
            }
            set
            {
                this.SetText(BoxType.Grp, value);
            }
        }

        public bool IsCompilation
        {
            get
            {
                ByteVector[] types = new ByteVector[] { BoxType.Cpil };
                IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AppleDataBox current = enumerator.Current;
                        return (current.Data.ToUInt() != 0);
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                return false;
            }
            set
            {
                byte[] data = new byte[] { !value ? ((byte) 0) : ((byte) 1) };
                this.SetData(BoxType.Cpil, new ByteVector(data), 0x15);
            }
        }

        public override bool IsEmpty
        {
            get
            {
                return !this.ilst_box.HasChildren;
            }
        }

        public override string Lyrics
        {
            get
            {
                ByteVector[] types = new ByteVector[] { BoxType.Lyr };
                IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AppleDataBox current = enumerator.Current;
                        return current.Text;
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                return null;
            }
            set
            {
                this.SetText(BoxType.Lyr, value);
            }
        }

        public override string MusicBrainzArtistId
        {
            get
            {
                return this.GetDashBox("com.apple.iTunes", "MusicBrainz Artist Id");
            }
            set
            {
                this.SetDashBox("com.apple.iTunes", "MusicBrainz Artist Id", value);
            }
        }

        public override string MusicBrainzDiscId
        {
            get
            {
                return this.GetDashBox("com.apple.iTunes", "MusicBrainz Disc Id");
            }
            set
            {
                this.SetDashBox("com.apple.iTunes", "MusicBrainz Disc Id", value);
            }
        }

        public override string MusicBrainzReleaseArtistId
        {
            get
            {
                return this.GetDashBox("com.apple.iTunes", "MusicBrainz Album Artist Id");
            }
            set
            {
                this.SetDashBox("com.apple.iTunes", "MusicBrainz Album Artist Id", value);
            }
        }

        public override string MusicBrainzReleaseCountry
        {
            get
            {
                return this.GetDashBox("com.apple.iTunes", "MusicBrainz Album Release Country");
            }
            set
            {
                this.SetDashBox("com.apple.iTunes", "MusicBrainz Album Release Country", value);
            }
        }

        public override string MusicBrainzReleaseId
        {
            get
            {
                return this.GetDashBox("com.apple.iTunes", "MusicBrainz Album Id");
            }
            set
            {
                this.SetDashBox("com.apple.iTunes", "MusicBrainz Album Id", value);
            }
        }

        public override string MusicBrainzReleaseStatus
        {
            get
            {
                return this.GetDashBox("com.apple.iTunes", "MusicBrainz Album Status");
            }
            set
            {
                this.SetDashBox("com.apple.iTunes", "MusicBrainz Album Status", value);
            }
        }

        public override string MusicBrainzReleaseType
        {
            get
            {
                return this.GetDashBox("com.apple.iTunes", "MusicBrainz Album Type");
            }
            set
            {
                this.SetDashBox("com.apple.iTunes", "MusicBrainz Album Type", value);
            }
        }

        public override string MusicBrainzTrackId
        {
            get
            {
                return this.GetDashBox("com.apple.iTunes", "MusicIP PUID");
            }
            set
            {
                this.SetDashBox("com.apple.iTunes", "MusicIP PUID", value);
            }
        }

        public override string MusicIpId
        {
            get
            {
                return this.GetDashBox("com.apple.iTunes", "MusicIP PUID");
            }
            set
            {
                this.SetDashBox("com.apple.iTunes", "MusicIP PUID", value);
            }
        }

        public override string[] Performers
        {
            get
            {
                return this.GetText(BoxType.Art);
            }
            set
            {
                this.SetText(BoxType.Art, value);
            }
        }

        public override string[] PerformersSort
        {
            get
            {
                return this.GetText(BoxType.Soar);
            }
            set
            {
                this.SetText(BoxType.Soar, value);
            }
        }

        public override IPicture[] Pictures
        {
            get
            {
                List<Picture> list = new List<Picture>();
                ByteVector[] types = new ByteVector[] { BoxType.Covr };
                IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AppleDataBox current = enumerator.Current;
                        Picture item = new Picture(current.Data) {
                            Type = PictureType.FrontCover
                        };
                        list.Add(item);
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                return list.ToArray();
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    this.ClearData(BoxType.Covr);
                }
                else
                {
                    AppleDataBox[] boxes = new AppleDataBox[value.Length];
                    for (int i = 0; i < value.Length; i++)
                    {
                        uint flags = 0;
                        if (value[i].MimeType == "image/jpeg")
                        {
                            flags = 13;
                        }
                        else if (value[i].MimeType == "image/png")
                        {
                            flags = 14;
                        }
                        boxes[i] = new AppleDataBox(value[i].Data, flags);
                    }
                    this.SetData(BoxType.Covr, boxes);
                }
            }
        }

        public override TagLib.TagTypes TagTypes
        {
            get
            {
                return TagLib.TagTypes.Apple;
            }
        }

        public override string Title
        {
            get
            {
                string[] text = this.GetText(BoxType.Nam);
                return ((text.Length != 0) ? text[0] : null);
            }
            set
            {
                this.SetText(BoxType.Nam, value);
            }
        }

        public override string TitleSort
        {
            get
            {
                string[] text = this.GetText(BoxType.Sonm);
                return ((text.Length != 0) ? text[0] : null);
            }
            set
            {
                this.SetText(BoxType.Sonm, value);
            }
        }

        public override uint Track
        {
            get
            {
                ByteVector[] types = new ByteVector[] { BoxType.Trkn };
                IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AppleDataBox current = enumerator.Current;
                        if ((current.Flags == 0) && (current.Data.Count >= 4))
                        {
                            return current.Data.Mid(2, 2).ToUShort();
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
                return 0;
            }
            set
            {
                uint trackCount = this.TrackCount;
                if ((value == 0) && (trackCount == 0))
                {
                    this.ClearData(BoxType.Trkn);
                }
                else
                {
                    ByteVector data = ByteVector.FromUShort(0);
                    data.Add(ByteVector.FromUShort((ushort) value));
                    data.Add(ByteVector.FromUShort((ushort) trackCount));
                    data.Add(ByteVector.FromUShort(0));
                    this.SetData(BoxType.Trkn, data, 0);
                }
            }
        }

        public override uint TrackCount
        {
            get
            {
                ByteVector[] types = new ByteVector[] { BoxType.Trkn };
                IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AppleDataBox current = enumerator.Current;
                        if ((current.Flags == 0) && (current.Data.Count >= 6))
                        {
                            return current.Data.Mid(4, 2).ToUShort();
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
                return 0;
            }
            set
            {
                uint track = this.Track;
                if ((value == 0) && (track == 0))
                {
                    this.ClearData(BoxType.Trkn);
                }
                else
                {
                    ByteVector data = ByteVector.FromUShort(0);
                    data.Add(ByteVector.FromUShort((ushort) track));
                    data.Add(ByteVector.FromUShort((ushort) value));
                    data.Add(ByteVector.FromUShort(0));
                    this.SetData(BoxType.Trkn, data, 0);
                }
            }
        }

        public override uint Year
        {
            get
            {
                ByteVector[] types = new ByteVector[] { BoxType.Day };
                IEnumerator<AppleDataBox> enumerator = this.DataBoxes(types).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        uint num;
                        AppleDataBox current = enumerator.Current;
                        if ((current.Text != null) && (uint.TryParse(current.Text, out num) || uint.TryParse((current.Text.Length <= 4) ? current.Text : current.Text.Substring(0, 4), out num)))
                        {
                            return num;
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
                return 0;
            }
            set
            {
                if (value == 0)
                {
                    this.ClearData(BoxType.Day);
                }
                else
                {
                    this.SetText(BoxType.Day, value.ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        [CompilerGenerated]
        private sealed class <DataBoxes>c__Iterator7 : IDisposable, IEnumerator, IEnumerable, IEnumerable<AppleDataBox>, IEnumerator<AppleDataBox>
        {
            internal AppleDataBox $current;
            internal int $PC;
            internal IEnumerable<ByteVector> <$>types;
            internal IEnumerator<Box> <$s_225>__0;
            internal IEnumerator<ByteVector> <$s_226>__2;
            internal IEnumerator<Box> <$s_227>__4;
            internal AppleTag <>f__this;
            internal AppleDataBox <adb>__6;
            internal Box <box>__1;
            internal Box <data_box>__5;
            internal ByteVector <v>__3;
            internal IEnumerable<ByteVector> types;

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
                            try
                            {
                                try
                                {
                                }
                                finally
                                {
                                    if (this.<$s_227>__4 == null)
                                    {
                                    }
                                    this.<$s_227>__4.Dispose();
                                }
                            }
                            finally
                            {
                                if (this.<$s_226>__2 == null)
                                {
                                }
                                this.<$s_226>__2.Dispose();
                            }
                        }
                        finally
                        {
                            if (this.<$s_225>__0 == null)
                            {
                            }
                            this.<$s_225>__0.Dispose();
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
                        this.<$s_225>__0 = this.<>f__this.ilst_box.Children.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_01C0;
                }
                try
                {
                    switch (num)
                    {
                        case 1:
                            goto Label_0077;
                    }
                    while (this.<$s_225>__0.MoveNext())
                    {
                        this.<box>__1 = this.<$s_225>__0.Current;
                        this.<$s_226>__2 = this.types.GetEnumerator();
                        num = 0xfffffffd;
                    Label_0077:
                        try
                        {
                            switch (num)
                            {
                                case 1:
                                    goto Label_00D7;
                            }
                            while (this.<$s_226>__2.MoveNext())
                            {
                                this.<v>__3 = this.<$s_226>__2.Current;
                                if (AppleTag.FixId(this.<v>__3) != this.<box>__1.BoxType)
                                {
                                    continue;
                                }
                                this.<$s_227>__4 = this.<box>__1.Children.GetEnumerator();
                                num = 0xfffffffd;
                            Label_00D7:
                                try
                                {
                                    while (this.<$s_227>__4.MoveNext())
                                    {
                                        this.<data_box>__5 = this.<$s_227>__4.Current;
                                        this.<adb>__6 = this.<data_box>__5 as AppleDataBox;
                                        if (this.<adb>__6 != null)
                                        {
                                            this.$current = this.<adb>__6;
                                            this.$PC = 1;
                                            flag = true;
                                            return true;
                                        }
                                    }
                                    continue;
                                }
                                finally
                                {
                                    if (!flag)
                                    {
                                    }
                                    if (this.<$s_227>__4 == null)
                                    {
                                    }
                                    this.<$s_227>__4.Dispose();
                                }
                            }
                            continue;
                        }
                        finally
                        {
                            if (!flag)
                            {
                            }
                            if (this.<$s_226>__2 == null)
                            {
                            }
                            this.<$s_226>__2.Dispose();
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    if (this.<$s_225>__0 == null)
                    {
                    }
                    this.<$s_225>__0.Dispose();
                }
                this.$PC = -1;
            Label_01C0:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<AppleDataBox> IEnumerable<AppleDataBox>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new AppleTag.<DataBoxes>c__Iterator7 { <>f__this = this.<>f__this, types = this.<$>types };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TagLib.Mpeg4.AppleDataBox>.GetEnumerator();
            }

            AppleDataBox IEnumerator<AppleDataBox>.Current
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
        private sealed class <DataBoxes>c__Iterator8 : IDisposable, IEnumerator, IEnumerable, IEnumerable<AppleDataBox>, IEnumerator<AppleDataBox>
        {
            internal AppleDataBox $current;
            internal int $PC;
            internal string <$>mean;
            internal string <$>name;
            internal IEnumerator<Box> <$s_228>__0;
            internal IEnumerator<Box> <$s_229>__4;
            internal AppleTag <>f__this;
            internal AppleDataBox <adb>__6;
            internal Box <box>__1;
            internal Box <data_box>__5;
            internal AppleAdditionalInfoBox <mean_box>__2;
            internal AppleAdditionalInfoBox <name_box>__3;
            internal string mean;
            internal string name;

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
                            try
                            {
                            }
                            finally
                            {
                                if (this.<$s_229>__4 == null)
                                {
                                }
                                this.<$s_229>__4.Dispose();
                            }
                        }
                        finally
                        {
                            if (this.<$s_228>__0 == null)
                            {
                            }
                            this.<$s_228>__0.Dispose();
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
                        this.<$s_228>__0 = this.<>f__this.ilst_box.Children.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_01DD;
                }
                try
                {
                    switch (num)
                    {
                        case 1:
                            goto Label_0122;
                    }
                    while (this.<$s_228>__0.MoveNext())
                    {
                        this.<box>__1 = this.<$s_228>__0.Current;
                        if (this.<box>__1.BoxType != BoxType.DASH)
                        {
                            continue;
                        }
                        this.<mean_box>__2 = (AppleAdditionalInfoBox) this.<box>__1.GetChild(BoxType.Mean);
                        this.<name_box>__3 = (AppleAdditionalInfoBox) this.<box>__1.GetChild(BoxType.Name);
                        if ((((this.<mean_box>__2 == null) || (this.<name_box>__3 == null)) || (this.<mean_box>__2.Text != this.mean)) || (this.<name_box>__3.Text != this.name))
                        {
                            continue;
                        }
                        this.<$s_229>__4 = this.<box>__1.Children.GetEnumerator();
                        num = 0xfffffffd;
                    Label_0122:
                        try
                        {
                            while (this.<$s_229>__4.MoveNext())
                            {
                                this.<data_box>__5 = this.<$s_229>__4.Current;
                                this.<adb>__6 = this.<data_box>__5 as AppleDataBox;
                                if (this.<adb>__6 != null)
                                {
                                    this.$current = this.<adb>__6;
                                    this.$PC = 1;
                                    flag = true;
                                    return true;
                                }
                            }
                            continue;
                        }
                        finally
                        {
                            if (!flag)
                            {
                            }
                            if (this.<$s_229>__4 == null)
                            {
                            }
                            this.<$s_229>__4.Dispose();
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    if (this.<$s_228>__0 == null)
                    {
                    }
                    this.<$s_228>__0.Dispose();
                }
                this.$PC = -1;
            Label_01DD:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<AppleDataBox> IEnumerable<AppleDataBox>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new AppleTag.<DataBoxes>c__Iterator8 { <>f__this = this.<>f__this, mean = this.<$>mean, name = this.<$>name };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TagLib.Mpeg4.AppleDataBox>.GetEnumerator();
            }

            AppleDataBox IEnumerator<AppleDataBox>.Current
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

