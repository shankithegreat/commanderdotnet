namespace Nomad.FileSystem.Virtual
{
    using System;

    public class VirtualResolveContainer<T> : VirtualItemContainer<T> where T: IVirtualItem
    {
        private IVirtualLink FLink;

        protected override T GetValue(string lazyItemPath, byte[] lazyItemStream)
        {
            IVirtualItem target = null;
            if (!string.IsNullOrEmpty(lazyItemPath))
            {
                target = VirtualItem.FromFullName(lazyItemPath, VirtualItemType.Unknown);
            }
            else if (lazyItemStream != null)
            {
                target = VirtualItem.Deserialize<IVirtualItem>(lazyItemStream);
            }
            this.FLink = target as IVirtualLink;
            if (this.FLink != null)
            {
                target = this.FLink.Target;
            }
            return (T) target;
        }

        public IVirtualLink Link
        {
            get
            {
                return this.FLink;
            }
        }
    }
}

