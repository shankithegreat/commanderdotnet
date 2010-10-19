namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.LocalFile;
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Xml.Serialization;

    public class VirtualItemContainer<T> where T: IVirtualItem
    {
        private T FItem;
        private string FLazyItemPath;
        private byte[] FLazyItemStream;

        public VirtualItemContainer()
        {
        }

        public VirtualItemContainer(T value)
        {
            this.Value = value;
        }

        public IAsyncResult BeginGetValue()
        {
            if (this.FItem != null)
            {
                return new DummyAsyncResult<T>(this.FItem);
            }
            Func<string, byte[], T> func = new Func<string, byte[], T>(this.GetValue);
            return func.BeginInvoke(this.FLazyItemPath, this.FLazyItemStream, null, func);
        }

        private void ClearSerializable()
        {
            this.FLazyItemStream = null;
            this.FLazyItemPath = null;
        }

        public T EndGetValue(IAsyncResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException();
            }
            using (DummyAsyncResult<T> result2 = result as DummyAsyncResult<T>)
            {
                if (result2 != null)
                {
                    return result2.Item;
                }
            }
            Func<string, byte[], T> asyncState = result.AsyncState as Func<string, byte[], T>;
            if (asyncState == null)
            {
                throw new ArgumentException();
            }
            this.FItem = asyncState.EndInvoke(result);
            this.ClearSerializable();
            return this.FItem;
        }

        protected virtual T GetValue(string lazyItemPath, byte[] lazyItemStream)
        {
            if (!string.IsNullOrEmpty(lazyItemPath))
            {
                Type type = typeof(T);
                bool flag = (type == typeof(IVirtualFolder)) || (type.GetInterface(typeof(IVirtualFolder).Name) != null);
                return (T) VirtualItem.FromFullName(lazyItemPath, flag ? VirtualItemType.Folder : VirtualItemType.Unknown);
            }
            if (lazyItemStream != null)
            {
                return VirtualItem.Deserialize<T>(lazyItemStream);
            }
            return default(T);
        }

        public bool IsEmpty
        {
            get
            {
                return (((this.FItem == null) && string.IsNullOrEmpty(this.FLazyItemPath)) && (this.FLazyItemStream == null));
            }
        }

        public bool IsLazy
        {
            get
            {
                return ((this.FItem == null) && (!string.IsNullOrEmpty(this.FLazyItemPath) || (this.FLazyItemStream != null)));
            }
            set
            {
                if (value)
                {
                    this.FLazyItemPath = null;
                    this.FLazyItemStream = null;
                    if (this.FItem != null)
                    {
                        if (this.FItem is FileSystemItem)
                        {
                            this.FLazyItemPath = this.FItem.FullName;
                        }
                        else
                        {
                            this.FLazyItemStream = VirtualItem.Serialize(this.FItem);
                        }
                    }
                }
                else
                {
                    T local = this.Value;
                }
            }
        }

        [XmlElement("ItemPath"), EditorBrowsable(EditorBrowsableState.Never)]
        public string SerializableItemPath
        {
            get
            {
                if (!((this.FItem == null) || (this.FItem is FileSystemItem)))
                {
                    return null;
                }
                if (this.FLazyItemPath != null)
                {
                    return this.FLazyItemPath;
                }
                return ((this.FItem is FileSystemItem) ? this.FItem.FullName : null);
            }
            set
            {
                this.FLazyItemPath = value;
                this.FItem = default(T);
                if (!string.IsNullOrEmpty(this.FLazyItemPath))
                {
                    this.FLazyItemStream = null;
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), XmlElement("ItemStream", DataType="base64Binary")]
        public byte[] SerializableItemStream
        {
            get
            {
                if (this.FItem is FileSystemItem)
                {
                    return null;
                }
                if (this.FLazyItemStream != null)
                {
                    return this.FLazyItemStream;
                }
                return ((this.FItem != null) ? VirtualItem.Serialize(this.FItem) : null);
            }
            set
            {
                this.FLazyItemStream = value;
                this.FItem = default(T);
                if (this.FLazyItemStream != null)
                {
                    this.FLazyItemPath = null;
                }
            }
        }

        [XmlIgnore]
        public T Value
        {
            get
            {
                if (this.FItem == null)
                {
                    this.FItem = this.GetValue(this.FLazyItemPath, this.FLazyItemStream);
                    this.ClearSerializable();
                }
                return this.FItem;
            }
            set
            {
                this.FItem = value;
                this.ClearSerializable();
            }
        }

        private class DummyAsyncResult : IAsyncResult, IDisposable
        {
            private WaitHandle FWaitHandle;
            internal readonly T Item;

            public DummyAsyncResult(T item)
            {
                this.FWaitHandle = new ManualResetEvent(true);
                this.Item = item;
            }

            public void Dispose()
            {
                if (this.FWaitHandle != null)
                {
                    this.FWaitHandle.Close();
                }
                this.FWaitHandle = null;
            }

            public object AsyncState
            {
                get
                {
                    return null;
                }
            }

            public WaitHandle AsyncWaitHandle
            {
                get
                {
                    return this.FWaitHandle;
                }
            }

            public bool CompletedSynchronously
            {
                get
                {
                    return true;
                }
            }

            public bool IsCompleted
            {
                get
                {
                    return true;
                }
            }
        }
    }
}

