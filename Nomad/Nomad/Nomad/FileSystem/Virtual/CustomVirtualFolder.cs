namespace Nomad.FileSystem.Virtual
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    [Serializable]
    public abstract class CustomVirtualFolder
    {
        public event EventHandler<VirtualItemChangedEventArgs> OnChanged;

        protected CustomVirtualFolder()
        {
        }

        protected void Changed(WatcherChangeTypes changeType, IVirtualItem item)
        {
            if (this.OnChanged != null)
            {
                this.OnChanged(this, new VirtualItemChangedEventArgs(changeType, item));
            }
        }

        public abstract IEnumerable<IVirtualItem> GetContent();
        public virtual IEnumerable<IVirtualFolder> GetFolders()
        {
            return this.GetContent().OfType<IVirtualFolder>();
        }
    }
}

