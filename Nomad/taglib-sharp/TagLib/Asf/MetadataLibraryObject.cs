namespace TagLib.Asf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TagLib;

    public class MetadataLibraryObject : TagLib.Asf.Object, IEnumerable<DescriptionRecord>, IEnumerable
    {
        private List<DescriptionRecord> records;

        public MetadataLibraryObject() : base(TagLib.Asf.Guid.AsfMetadataLibraryObject)
        {
            this.records = new List<DescriptionRecord>();
        }

        public MetadataLibraryObject(TagLib.Asf.File file, long position) : base(file, position)
        {
            this.records = new List<DescriptionRecord>();
            if (!base.Guid.Equals(TagLib.Asf.Guid.AsfMetadataLibraryObject))
            {
                throw new CorruptFileException("Object GUID incorrect.");
            }
            if (base.OriginalSize < 0x1aL)
            {
                throw new CorruptFileException("Object size too small.");
            }
            ushort num = file.ReadWord();
            for (ushort i = 0; i < num; i = (ushort) (i + 1))
            {
                DescriptionRecord record = new DescriptionRecord(file);
                this.AddRecord(record);
            }
        }

        public void AddRecord(DescriptionRecord record)
        {
            this.records.Add(record);
        }

        public IEnumerator<DescriptionRecord> GetEnumerator()
        {
            return this.records.GetEnumerator();
        }

        [DebuggerHidden]
        public IEnumerable<DescriptionRecord> GetRecords(ushort languageListIndex, ushort streamNumber, params string[] names)
        {
            return new <GetRecords>c__Iterator2 { languageListIndex = languageListIndex, streamNumber = streamNumber, names = names, <$>languageListIndex = languageListIndex, <$>streamNumber = streamNumber, <$>names = names, <>f__this = this, $PC = -2 };
        }

        public void RemoveRecords(ushort languageListIndex, ushort streamNumber, string name)
        {
            for (int i = this.records.Count - 1; i >= 0; i--)
            {
                DescriptionRecord record = this.records[i];
                if (((record.LanguageListIndex == languageListIndex) && (record.StreamNumber == streamNumber)) && (record.Name == name))
                {
                    this.records.RemoveAt(i);
                }
            }
        }

        public override ByteVector Render()
        {
            ByteVector vector = new ByteVector();
            ushort num = 0;
            foreach (DescriptionRecord record in this.records)
            {
                num = (ushort) (num + 1);
                vector.Add(record.Render());
            }
            return base.Render(TagLib.Asf.Object.RenderWord(num) + vector);
        }

        public void SetRecords(ushort languageListIndex, ushort streamNumber, string name, params DescriptionRecord[] records)
        {
            int count = this.records.Count;
            for (int i = this.records.Count - 1; i >= 0; i--)
            {
                DescriptionRecord record = this.records[i];
                if (((record.LanguageListIndex == languageListIndex) && (record.StreamNumber == streamNumber)) && (record.Name == name))
                {
                    this.records.RemoveAt(i);
                    count = i;
                }
            }
            this.records.InsertRange(count, records);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.records.GetEnumerator();
        }

        public bool IsEmpty
        {
            get
            {
                return (this.records.Count == 0);
            }
        }

        [CompilerGenerated]
        private sealed class <GetRecords>c__Iterator2 : IEnumerable<DescriptionRecord>, IEnumerator<DescriptionRecord>, IDisposable, IEnumerator, IEnumerable
        {
            internal DescriptionRecord $current;
            internal int $PC;
            internal ushort <$>languageListIndex;
            internal string[] <$>names;
            internal ushort <$>streamNumber;
            internal List<DescriptionRecord>.Enumerator <$s_27>__0;
            internal string[] <$s_28>__2;
            internal int <$s_29>__3;
            internal MetadataLibraryObject <>f__this;
            internal string <name>__4;
            internal DescriptionRecord <rec>__1;
            internal ushort languageListIndex;
            internal string[] names;
            internal ushort streamNumber;

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
                            this.<$s_27>__0.Dispose();
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
                        this.<$s_27>__0 = this.<>f__this.records.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0141;
                }
                try
                {
                    switch (num)
                    {
                        case 1:
                            goto Label_00EF;
                    }
                    while (this.<$s_27>__0.MoveNext())
                    {
                        this.<rec>__1 = this.<$s_27>__0.Current;
                        if ((this.<rec>__1.LanguageListIndex == this.languageListIndex) && (this.<rec>__1.StreamNumber == this.streamNumber))
                        {
                            this.<$s_28>__2 = this.names;
                            this.<$s_29>__3 = 0;
                            while (this.<$s_29>__3 < this.<$s_28>__2.Length)
                            {
                                this.<name>__4 = this.<$s_28>__2[this.<$s_29>__3];
                                if (this.<rec>__1.Name == this.<name>__4)
                                {
                                    this.$current = this.<rec>__1;
                                    this.$PC = 1;
                                    flag = true;
                                    return true;
                                }
                            Label_00EF:
                                this.<$s_29>__3++;
                            }
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_27>__0.Dispose();
                }
                this.$PC = -1;
            Label_0141:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<DescriptionRecord> IEnumerable<DescriptionRecord>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new MetadataLibraryObject.<GetRecords>c__Iterator2 { <>f__this = this.<>f__this, languageListIndex = this.<$>languageListIndex, streamNumber = this.<$>streamNumber, names = this.<$>names };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TagLib.Asf.DescriptionRecord>.GetEnumerator();
            }

            DescriptionRecord IEnumerator<DescriptionRecord>.Current
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

