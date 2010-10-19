namespace Nomad.FileSystem.Ftp
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Cache<TKey, TValue>
    {
        private ReaderWriterLock CacheLock;
        private Dictionary<TKey, TValue> FCache;

        public void Clear()
        {
            if (this.FCache != null)
            {
                this.CacheLock.AcquireWriterLock(-1);
                try
                {
                    this.FCache.Clear();
                }
                finally
                {
                    this.CacheLock.ReleaseWriterLock();
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            bool flag;
            if (this.FCache == null)
            {
                return false;
            }
            this.CacheLock.AcquireReaderLock(-1);
            try
            {
                flag = this.FCache.ContainsKey(key);
            }
            finally
            {
                this.CacheLock.ReleaseReaderLock();
            }
            return flag;
        }

        public bool Remove(TKey key)
        {
            bool flag;
            if (this.FCache == null)
            {
                return false;
            }
            this.CacheLock.AcquireWriterLock(-1);
            try
            {
                flag = this.FCache.Remove(key);
            }
            finally
            {
                this.CacheLock.ReleaseWriterLock();
            }
            return flag;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            bool flag;
            if (this.FCache == null)
            {
                value = default(TValue);
                return false;
            }
            this.CacheLock.AcquireReaderLock(-1);
            try
            {
                flag = this.FCache.TryGetValue(key, out value);
            }
            finally
            {
                this.CacheLock.ReleaseReaderLock();
            }
            return flag;
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue local;
                if (this.FCache == null)
                {
                    throw new KeyNotFoundException();
                }
                this.CacheLock.AcquireReaderLock(-1);
                try
                {
                    local = this.FCache[key];
                }
                finally
                {
                    this.CacheLock.ReleaseReaderLock();
                }
                return local;
            }
            set
            {
                if (this.FCache == null)
                {
                    this.FCache = new Dictionary<TKey, TValue>();
                }
                if (this.CacheLock == null)
                {
                    this.CacheLock = new ReaderWriterLock();
                }
                this.CacheLock.AcquireWriterLock(-1);
                try
                {
                    this.FCache[key] = value;
                }
                finally
                {
                    this.CacheLock.ReleaseWriterLock();
                }
            }
        }

        public ReaderWriterLock Lock
        {
            get
            {
                if (this.CacheLock == null)
                {
                    this.CacheLock = new ReaderWriterLock();
                }
                return this.CacheLock;
            }
        }
    }
}

