namespace Nomad.Workers
{
    using Microsoft.Win32;
    using Nomad;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    internal class CompareFoldersWorker : EventBackgroundWorker
    {
        public IList<string> EqualNames;
        private Buffers FBuffers1;
        private Buffers FBuffers2;
        private IEnumerable<IVirtualItem> FList1;
        private IEnumerable<IVirtualItem> FList2;
        private long FProcessedSize;
        private int FStoredProgress;
        private long FTotalSize;
        private CompareFoldersOptions Options;

        public CompareFoldersWorker()
        {
            this.Options = CompareFoldersOptions.CompareLastWriteTime | CompareFoldersOptions.CompareAttributes;
            this.FStoredProgress = -1;
        }

        public CompareFoldersWorker(IEnumerable<IVirtualItem> list1, IEnumerable<IVirtualItem> list2, CompareFoldersOptions options)
        {
            this.Options = CompareFoldersOptions.CompareLastWriteTime | CompareFoldersOptions.CompareAttributes;
            this.FStoredProgress = -1;
            this.FList1 = list1;
            this.FList2 = list2;
            this.Options = options;
        }

        public IList<string> CompareFolders(IEnumerable<IVirtualItem> list1, IEnumerable<IVirtualItem> list2, CompareFoldersOptions options)
        {
            return this.CompareFolders(list1, list2, options, false);
        }

        private IList<string> CompareFolders(IEnumerable<IVirtualItem> list1, IEnumerable<IVirtualItem> list2, CompareFoldersOptions options, bool cancellable)
        {
            bool flag = (options & CompareFoldersOptions.CompareContent) > 0;
            int num = 0;
            this.FTotalSize = 0L;
            this.FProcessedSize = 0L;
            this.FStoredProgress = -1;
            ICollection is2 = list1 as ICollection;
            List<IVirtualItem> list = new List<IVirtualItem>((is2 != null) ? is2.Count : 0x80);
            foreach (IVirtualItem item in list1)
            {
                if (!(item is IVirtualFolder))
                {
                    list.Add(item);
                    if (flag)
                    {
                        this.FTotalSize += Convert.ToInt64(item[3]);
                    }
                }
            }
            ICollection is3 = list2 as ICollection;
            Dictionary<string, IVirtualItem> dictionary = new Dictionary<string, IVirtualItem>((is3 != null) ? is3.Count : 0x80, StringComparer.OrdinalIgnoreCase);
            foreach (IVirtualItem item in list2)
            {
                if (!(item is IVirtualFolder))
                {
                    dictionary.Add(item.Name, item);
                }
            }
            List<string> list3 = new List<string>(Math.Min(list.Count, dictionary.Count));
            IComparer defaultInvariant = Comparer.DefaultInvariant;
            if (flag)
            {
                this.OnProgress();
            }
            foreach (IVirtualItem item2 in list)
            {
                IVirtualItem item3;
                if (cancellable)
                {
                    base.CheckSuspendingPending();
                    if (base.CancellationPending)
                    {
                        return null;
                    }
                }
                bool flag2 = false;
                if (dictionary.TryGetValue(item2.Name, out item3))
                {
                    bool flag3 = string.Equals(item2.Name, item3.Name, StringComparison.OrdinalIgnoreCase);
                    if (flag3)
                    {
                        if ((CompareFoldersOptions.CompareAttributes & options) > 0)
                        {
                            flag3 = defaultInvariant.Compare(item2.Attributes, item3.Attributes) == 0;
                        }
                        if (flag3 && ((options & CompareFoldersOptions.CompareLastWriteTime) > 0))
                        {
                            bool flag4 = item2.IsPropertyAvailable(8);
                            bool flag5 = item3.IsPropertyAvailable(8);
                            if (flag4 && flag5)
                            {
                                DateTime time = (DateTime) item2[8];
                                DateTime time2 = (DateTime) item3[8];
                                TimeSpan span = (TimeSpan) (time - time2);
                                flag3 = Math.Abs(span.Ticks) < 0x989680L;
                            }
                            else
                            {
                                flag3 = flag4 == flag5;
                            }
                        }
                        if (flag3 && ((options & CompareFoldersOptions.CompareSize) > 0))
                        {
                            flag3 = defaultInvariant.Compare(item2[3], item3[3]) == 0;
                        }
                        if (flag3 && flag)
                        {
                            IChangeVirtualFile file = item2 as IChangeVirtualFile;
                            IChangeVirtualFile file2 = item3 as IChangeVirtualFile;
                            flag3 = (file != null) && (file2 != null);
                            if (flag3)
                            {
                                bool async = (options & CompareFoldersOptions.CompareContentAsync) > 0;
                                if ((options & CompareFoldersOptions.AutoCompareContentAsync) > 0)
                                {
                                    IGetVirtualVolume parent = item2.Parent as IGetVirtualVolume;
                                    IGetVirtualVolume volume2 = item3.Parent as IGetVirtualVolume;
                                    async = ((parent != null) && (volume2 != null)) && ((parent.Location != volume2.Location) || (parent.VolumeType != volume2.VolumeType));
                                }
                                FileOptions options2 = FileOptions.SequentialScan | (async ? FileOptions.Asynchronous : FileOptions.None);
                                using (Stream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read, options2, 0L))
                                {
                                    using (Stream stream2 = file2.Open(FileMode.Open, FileAccess.Read, FileShare.Read, options2, 0L))
                                    {
                                        flag3 = this.StreamsEqual(stream, stream2, 0L, async);
                                        flag2 = true;
                                    }
                                }
                            }
                        }
                        if (flag3)
                        {
                            list3.Add(item2.Name);
                        }
                    }
                }
                if (flag)
                {
                    if (!flag2)
                    {
                        this.FProcessedSize += (long) item2[3];
                    }
                    this.OnProgress();
                }
                else
                {
                    int progressPercent = (++num * 100) / list.Count;
                    if (progressPercent != this.FStoredProgress)
                    {
                        base.RaiseProgressChanged(progressPercent, null);
                        this.FStoredProgress = progressPercent;
                    }
                }
            }
            return list3;
        }

        protected override void DoWork()
        {
            using (new ThreadExecutionStateLock(true, false))
            {
                this.EqualNames = this.CompareFolders(this.FList1, this.FList2, this.Options, true);
            }
        }

        private void OnProgress()
        {
            int progressPercent = (this.FTotalSize != 0L) ? ((int) ((this.FProcessedSize * 100L) / this.FTotalSize)) : ((int) 0L);
            if ((progressPercent <= 100) && (this.FStoredProgress != progressPercent))
            {
                base.RaiseProgressChanged(progressPercent, null);
                this.FStoredProgress = progressPercent;
            }
        }

        private static int ReadBuffer(Stream stream, byte[] buffer, int readSize)
        {
            int num;
            int offset = 0;
            do
            {
                num = stream.Read(buffer, offset, readSize - offset);
                offset += num;
            }
            while ((num != 0) && (offset != readSize));
            return offset;
        }

        private static int ReadBuffersAsync(Stream stream1, Stream stream2, Buffers buffers, int readSize)
        {
            int offset = 0;
            int num2 = 0;
            int num3 = 1;
            int num4 = 1;
            do
            {
                IAsyncResult asyncResult = null;
                IAsyncResult result2 = null;
                if (num3 > 0)
                {
                    asyncResult = stream1.BeginRead(buffers.Buffer1, offset, readSize - offset, null, null);
                }
                if (num4 > 0)
                {
                    result2 = stream2.BeginRead(buffers.Buffer2, num2, readSize - num2, null, null);
                }
                if (asyncResult != null)
                {
                    num3 = stream1.EndRead(asyncResult);
                }
                if (result2 != null)
                {
                    num4 = stream2.EndRead(result2);
                }
                offset += num3;
                num2 += num4;
            }
            while (((num3 != 0) || (num4 != 0)) && ((offset != readSize) || (num2 != readSize)));
            if (offset == num2)
            {
                return offset;
            }
            return -1;
        }

        public bool StreamsEqual(Stream stream1, Stream stream2, long size, bool async)
        {
            if (size == 0L)
            {
                if (stream1.CanSeek && stream2.CanSeek)
                {
                    long num = stream1.Seek(0L, SeekOrigin.End);
                    long num2 = stream1.Seek(0L, SeekOrigin.End);
                    if (num != num2)
                    {
                        return false;
                    }
                    if ((num == 0L) && (num2 == 0L))
                    {
                        return true;
                    }
                    size = num;
                    stream1.Seek(0L, SeekOrigin.Begin);
                    stream2.Seek(0L, SeekOrigin.Begin);
                }
                else
                {
                    size = 0x7fffffffL;
                }
            }
            if (this.FBuffers1 == null)
            {
                this.FBuffers1 = new Buffers(0x40000);
            }
            if (this.FBuffers2 == null)
            {
                this.FBuffers2 = new Buffers(0x40000);
            }
            Buffers buffers = this.FBuffers1;
            Buffers buffers2 = this.FBuffers2;
            int length = this.FBuffers1.Buffer1.Length;
            AsyncBuffersEqualCaller caller = new AsyncBuffersEqualCaller(ByteArrayHelper.Equals);
            IAsyncResult result = null;
            while ((size > 0L) && (length == this.FBuffers1.Buffer1.Length))
            {
                base.CheckSuspendingPending();
                if (base.CancellationPending)
                {
                    return false;
                }
                length = (int) Math.Min(size, (long) this.FBuffers1.Buffer1.Length);
                if (async)
                {
                    length = ReadBuffersAsync(stream1, stream2, buffers, length);
                }
                else
                {
                    int num4 = ReadBuffer(stream1, buffers.Buffer1, length);
                    if (num4 != ReadBuffer(stream2, buffers.Buffer2, length))
                    {
                        length = -1;
                    }
                    else
                    {
                        length = num4;
                    }
                }
                if ((length < 0) || ((result != null) && !caller.EndInvoke(result)))
                {
                    return false;
                }
                if (length == 0)
                {
                    return true;
                }
                buffers2 = Interlocked.Exchange<Buffers>(ref buffers, buffers2);
                this.FProcessedSize += length;
                this.OnProgress();
                result = caller.BeginInvoke(buffers2.Buffer1, buffers2.Buffer2, length, null, null);
                size -= length;
            }
            return caller.EndInvoke(result);
        }

        public override string Name
        {
            get
            {
                return "Compare Folders";
            }
        }
    }
}

