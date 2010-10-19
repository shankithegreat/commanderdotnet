namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public abstract class CachedPropertyProvider : ExtensiblePropertyProvider
    {
        private VirtualPropertySet CachedProperties;
        private object CacheLock = new object();
        private int[] FastCacheKeys;
        private object[] FastCacheValues;
        private Dictionary<int, object> SlowCache;

        protected CachedPropertyProvider()
        {
        }

        protected void AddPropertyToCache(int propertyId, object value)
        {
            lock (this.CacheLock)
            {
                if (this.SlowCache == null)
                {
                    if (this.FastCacheKeys == null)
                    {
                        if (value != null)
                        {
                            int[] numArray = new int[] { 0, -1, -1, -1, -1, -1, -1 };
                            numArray[0] = propertyId;
                            this.FastCacheKeys = numArray;
                            object[] objArray = new object[7];
                            objArray[0] = value;
                            this.FastCacheValues = objArray;
                        }
                    }
                    else
                    {
                        int num;
                        bool flag = false;
                        if (value == null)
                        {
                            if ((this.CachedProperties != null) && this.CachedProperties[propertyId])
                            {
                                for (num = 0; num < this.FastCacheKeys.Length; num++)
                                {
                                    if (this.FastCacheKeys[num] == propertyId)
                                    {
                                        this.FastCacheKeys[num] = -1;
                                        this.FastCacheValues[num] = null;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            flag = true;
                            for (num = 0; num < this.FastCacheKeys.Length; num++)
                            {
                                if ((this.FastCacheKeys[num] < 0) || (this.FastCacheKeys[num] == propertyId))
                                {
                                    this.FastCacheKeys[num] = propertyId;
                                    this.FastCacheValues[num] = value;
                                    flag = false;
                                    break;
                                }
                            }
                        }
                        if (flag)
                        {
                            this.SlowCache = new Dictionary<int, object>();
                            for (num = 0; num < this.FastCacheKeys.Length; num++)
                            {
                                this.SlowCache.Add(this.FastCacheKeys[num], this.FastCacheValues[num]);
                            }
                            this.FastCacheKeys = null;
                            this.FastCacheValues = null;
                        }
                    }
                }
                if (this.SlowCache != null)
                {
                    if (value == null)
                    {
                        this.SlowCache.Remove(propertyId);
                    }
                    else
                    {
                        this.SlowCache[propertyId] = value;
                    }
                }
                if (this.CachedProperties == null)
                {
                    this.CachedProperties = new VirtualPropertySet();
                }
                this.CachedProperties[propertyId] = true;
            }
            if (!(!base.HasAvailableSet || this.AvailableProperties[propertyId]))
            {
                base.ResetAvailableSet();
            }
        }

        protected virtual bool CacheProperty(int propertyId)
        {
            return false;
        }

        protected void ClearCache()
        {
            bool flag;
            lock (this.CacheLock)
            {
                flag = (this.CachedProperties != null) && this.CachedProperties.Any<int>();
                this.CachedProperties = null;
                this.SlowCache = null;
                this.FastCacheKeys = null;
                this.FastCacheValues = null;
            }
            if (flag)
            {
                base.ResetAvailableSet();
            }
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = new VirtualPropertySet();
            lock (this.CacheLock)
            {
                if (this.SlowCache != null)
                {
                    foreach (int num in this.SlowCache.Keys)
                    {
                        set[num] = true;
                    }
                }
                if (this.FastCacheKeys == null)
                {
                    return set;
                }
                foreach (int num in this.FastCacheKeys)
                {
                    if (num >= 0)
                    {
                        set[num] = true;
                    }
                }
            }
            return set;
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            object obj2;
            if (this.TryGetPropertyFromCache(propertyId, out obj2))
            {
                return ((obj2 != null) ? PropertyAvailability.Normal : PropertyAvailability.None);
            }
            return base.GetPropertyAvailability(propertyId);
        }

        protected override object InternalClone()
        {
            CachedPropertyProvider provider = (CachedPropertyProvider) base.InternalClone();
            provider.CacheLock = new object();
            return provider;
        }

        protected bool IsPropertyCached(int propertyId)
        {
            lock (this.CacheLock)
            {
                return ((this.CachedProperties != null) && this.CachedProperties[propertyId]);
            }
        }

        protected void RemovePropertyFromCache(int propertyId)
        {
            bool flag = false;
            lock (this.CacheLock)
            {
                if (this.SlowCache != null)
                {
                    flag = this.SlowCache.Remove(propertyId);
                }
                if (this.FastCacheKeys != null)
                {
                    for (int i = 0; i < this.FastCacheKeys.Length; i++)
                    {
                        if (this.FastCacheKeys[i] == propertyId)
                        {
                            this.FastCacheKeys[i] = -1;
                            this.FastCacheValues[i] = null;
                            flag = true;
                            break;
                        }
                    }
                }
                if (this.CachedProperties != null)
                {
                    this.CachedProperties[propertyId] = false;
                }
            }
            if ((flag && base.HasAvailableSet) && this.AvailableProperties[propertyId])
            {
                base.ResetAvailableSet();
            }
        }

        protected bool TryGetPropertyFromCache(int propertyId, out object value)
        {
            lock (this.CacheLock)
            {
                if ((this.CachedProperties != null) && this.CachedProperties[propertyId])
                {
                    if (this.SlowCache != null)
                    {
                        return this.SlowCache.TryGetValue(propertyId, out value);
                    }
                    if (this.FastCacheKeys != null)
                    {
                        for (int i = 0; i < this.FastCacheKeys.Length; i++)
                        {
                            if (this.FastCacheKeys[i] == propertyId)
                            {
                                value = this.FastCacheValues[i];
                                return true;
                            }
                        }
                    }
                    value = null;
                    return true;
                }
            }
            value = null;
            return false;
        }

        public override object this[int propertyId]
        {
            get
            {
                object obj2;
                if (!this.TryGetPropertyFromCache(propertyId, out obj2))
                {
                    try
                    {
                        obj2 = base[propertyId];
                    }
                    finally
                    {
                        if (this.CacheProperty(propertyId))
                        {
                            this.AddPropertyToCache(propertyId, obj2);
                        }
                    }
                }
                return obj2;
            }
            set
            {
                base[propertyId] = value;
            }
        }
    }
}

