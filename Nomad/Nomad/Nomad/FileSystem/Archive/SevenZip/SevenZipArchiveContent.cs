namespace Nomad.FileSystem.Archive.SevenZip
{
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class SevenZipArchiveContent : IEnumerable<ISimpleItem>, IEnumerable, IDisposable, IGetArchiveFormatInfo, IFlushStream
    {
        private string ArchiveName;
        private SevenZipSharedArchiveContext Context;
        private WeakReference FContent;

        public SevenZipArchiveContent(SevenZipSharedArchiveContext context, string archiveName)
        {
            this.Context = context;
            this.ArchiveName = archiveName;
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        public void Flush()
        {
            this.FContent = null;
            this.Context.Flush();
        }

        private ISimpleItem[] GetContent()
        {
            uint numberOfItems = this.Context.GetNumberOfItems();
            ISimpleItem[] itemArray = new ISimpleItem[numberOfItems];
            for (uint i = 0; i < numberOfItems; i++)
            {
                itemArray[i] = new SevenZipArchiveItem(this.Context, i);
            }
            if ((numberOfItems == 1) && (itemArray[0].Name == null))
            {
                string extension = Path.GetExtension(this.ArchiveName);
                if (extension.StartsWith(".", StringComparison.Ordinal))
                {
                    extension = extension.Substring(1);
                }
                string archiveName = this.ArchiveName;
                for (int j = 0; j < this.FormatInfo.Extension.Length; j++)
                {
                    if (extension.Equals(this.FormatInfo.Extension[j], StringComparison.OrdinalIgnoreCase))
                    {
                        archiveName = Path.GetFileNameWithoutExtension(this.ArchiveName);
                        string[] addExtension = this.Context.FormatInfo.AddExtension;
                        if (((addExtension != null) && (j < addExtension.Length)) && (addExtension[j] != "*"))
                        {
                            archiveName = archiveName + addExtension[j];
                        }
                        break;
                    }
                }
                ((SevenZipArchiveItem) itemArray[0]).Name = archiveName;
            }
            return itemArray;
        }

        public IEnumerator<ISimpleItem> GetEnumerator()
        {
            return new <GetEnumerator>d__0(0) { <>4__this = this };
        }

        public bool RefreshContent()
        {
            this.FContent = null;
            return this.Context.Reopen();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Content.GetEnumerator();
        }

        protected ISimpleItem[] Content
        {
            get
            {
                if ((this.FContent != null) && this.FContent.IsAlive)
                {
                    return (ISimpleItem[]) this.FContent.Target;
                }
                ISimpleItem[] content = this.GetContent();
                this.FContent = new WeakReference(content);
                return content;
            }
        }

        public ArchiveFormatInfo FormatInfo
        {
            get
            {
                return this.Context.FormatInfo;
            }
        }

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__0 : IEnumerator<ISimpleItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ISimpleItem <>2__current;
            public SevenZipArchiveContent <>4__this;
            public ISimpleItem[] <>7__wrap3;
            public int <>7__wrap4;
            public ISimpleItem <Item>5__1;

            [DebuggerHidden]
            public <GetEnumerator>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private void <>m__Finally2()
            {
                this.<>1__state = -1;
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>1__state = 1;
                            this.<>7__wrap3 = this.<>4__this.Content;
                            this.<>7__wrap4 = 0;
                            while (this.<>7__wrap4 < this.<>7__wrap3.Length)
                            {
                                this.<Item>5__1 = this.<>7__wrap3[this.<>7__wrap4];
                                this.<>2__current = this.<Item>5__1;
                                this.<>1__state = 2;
                                return true;
                            Label_0078:
                                this.<>1__state = 1;
                                this.<>7__wrap4++;
                            }
                            this.<>m__Finally2();
                            break;

                        case 2:
                            goto Label_0078;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        this.<>m__Finally2();
                        break;
                }
            }

            ISimpleItem IEnumerator<ISimpleItem>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

