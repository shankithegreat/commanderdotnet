namespace Nomad.FileSystem.LocalFile
{
    using Microsoft.IO;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;
    using System.Runtime.Serialization;

    public abstract class EjectableFileSystemFolder : CustomFileSystemFolder
    {
        private EventHandler<VolumeEventArgs> VolumeRemovedHandler;

        protected EjectableFileSystemFolder(DirectoryInfo info, IVirtualFolder parent) : base(info, parent)
        {
        }

        protected EjectableFileSystemFolder(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected EjectableFileSystemFolder(string folderPath, IVirtualFolder parent) : base(folderPath, parent)
        {
        }

        protected override void DisposeWatcher(bool disposing)
        {
            base.DisposeWatcher(disposing);
            if ((this.VolumeRemovedHandler != null) && disposing)
            {
                VolumeEvents.Removed -= this.VolumeRemovedHandler;
                base.SetCapability(FileSystemItem.ItemCapability.VolumeEventsAssigned, false);
                this.VolumeRemovedHandler = null;
            }
        }

        protected override void EnableWatcher()
        {
            base.EnableWatcher();
            if (!base.CheckCapability(FileSystemItem.ItemCapability.Deleted) && !((this.VolumeRemovedHandler == null) || base.CheckCapability(FileSystemItem.ItemCapability.VolumeEventsAssigned)))
            {
                VolumeEvents.Removed += this.VolumeRemovedHandler;
                base.SetCapability(FileSystemItem.ItemCapability.VolumeEventsAssigned, true);
            }
        }

        protected override void InitializeWatcher(string path)
        {
            base.InitializeWatcher(path);
            VolumeInfo folderVolume = base.FolderVolume;
            if ((folderVolume != null) && folderVolume.CanEject)
            {
                this.VolumeRemovedHandler = new EventHandler<VolumeEventArgs>(this.OnVolumeRemoved);
            }
        }

        private void OnVolumeRemoved(object source, VolumeEventArgs e)
        {
            if (base.FullName.StartsWith(e.DriveChar + ":", FileSystemItem.ComparisonRule))
            {
                this.ResetVisualCache();
                base.RaiseChanged(WatcherChangeTypes.Deleted, this);
            }
        }
    }
}

