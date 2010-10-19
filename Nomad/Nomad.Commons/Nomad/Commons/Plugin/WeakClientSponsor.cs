namespace Nomad.Commons.Plugin
{
    using Nomad.Commons.Collections;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.Remoting.Lifetime;

    public class WeakClientSponsor : MarshalByRefObject, ISponsor
    {
        private OneWayList<KeyValuePair<ILease, WeakReference>> LeaseObjectList;

        public WeakClientSponsor() : this(TimeSpan.FromMinutes(2.0))
        {
        }

        public WeakClientSponsor(TimeSpan renewalTime)
        {
            this.LeaseObjectList = new OneWayList<KeyValuePair<ILease, WeakReference>>();
            this.RenewalTime = renewalTime;
        }

        public void Register(MarshalByRefObject obj)
        {
            Func<KeyValuePair<ILease, WeakReference>, bool> predicate = null;
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            ILease Lease = (ILease) obj.GetLifetimeService();
            lock (this.LeaseObjectList)
            {
                if (predicate == null)
                {
                    predicate = delegate (KeyValuePair<ILease, WeakReference> x) {
                        return object.ReferenceEquals(x, Lease);
                    };
                }
                if (!this.LeaseObjectList.Any<KeyValuePair<ILease, WeakReference>>(predicate))
                {
                    this.LeaseObjectList.AddFirst(new KeyValuePair<ILease, WeakReference>(Lease, new WeakReference(obj)));
                    Lease.Register(this);
                }
            }
        }

        public TimeSpan Renewal(ILease lease)
        {
            Predicate<KeyValuePair<ILease, WeakReference>> match = null;
            OneWayList<KeyValuePair<ILease, WeakReference>> list;
            WeakReference reference = null;
            lock ((list = this.LeaseObjectList))
            {
                foreach (KeyValuePair<ILease, WeakReference> pair in this.LeaseObjectList)
                {
                    if (object.ReferenceEquals(pair.Key, lease))
                    {
                        reference = pair.Value;
                        goto Label_0090;
                    }
                }
            }
        Label_0090:
            if (reference != null)
            {
                if (reference.IsAlive && (reference.Target != null))
                {
                    lease.Renew(this.RenewalTime);
                    return this.RenewalTime;
                }
                lock ((list = this.LeaseObjectList))
                {
                    if (match == null)
                    {
                        match = delegate (KeyValuePair<ILease, WeakReference> x) {
                            return object.ReferenceEquals(x.Key, lease) || !x.Value.IsAlive;
                        };
                    }
                    this.LeaseObjectList.RemoveAll(match);
                }
            }
            return TimeSpan.Zero;
        }

        public bool Unregister(MarshalByRefObject obj)
        {
            Predicate<KeyValuePair<ILease, WeakReference>> match = null;
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            ILease Lease = (ILease) obj.GetLifetimeService();
            lock (this.LeaseObjectList)
            {
                if (match == null)
                {
                    match = delegate (KeyValuePair<ILease, WeakReference> x) {
                        return object.ReferenceEquals(x, Lease);
                    };
                }
                if (this.LeaseObjectList.RemoveAll(match) > 0)
                {
                    Lease.Unregister(this);
                    return true;
                }
            }
            return false;
        }

        public int ActiveClientCount
        {
            get
            {
                lock (this.LeaseObjectList)
                {
                    this.LeaseObjectList.RemoveAll(delegate (KeyValuePair<ILease, WeakReference> x) {
                        return !x.Value.IsAlive;
                    });
                    return this.LeaseObjectList.Count;
                }
            }
        }

        public TimeSpan RenewalTime { get; set; }
    }
}

