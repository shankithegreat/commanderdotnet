namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;
    using System.IO;

    public class VirtualItemChangedEventArgs : VirtualItemEventArgs
    {
        public readonly WatcherChangeTypes ChangeType;
        public readonly VirtualPropertySet PropertySet;

        public VirtualItemChangedEventArgs(IVirtualItem item, VirtualPropertySet propertySet) : base(item)
        {
            this.ChangeType = WatcherChangeTypes.Changed;
            this.PropertySet = propertySet;
        }

        public VirtualItemChangedEventArgs(WatcherChangeTypes changeType, IVirtualItem item) : base(item)
        {
            this.ChangeType = changeType;
            this.PropertySet = VirtualProperty.All;
        }
    }
}

