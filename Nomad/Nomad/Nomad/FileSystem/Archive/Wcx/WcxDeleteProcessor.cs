namespace Nomad.FileSystem.Archive.Wcx
{
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.LocalFile;
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class WcxDeleteProcessor : WcxProcessor
    {
        public WcxDeleteProcessor(WcxArchiveContext context) : base(context)
        {
        }

        protected override void DoProcess(ProcessItemHandler handler)
        {
            if (!base.Context.FormatInfo.CanDeleteFiles)
            {
                throw new NotSupportedException();
            }
            List<string> files = new List<string>(base.Items.Values.Count);
            foreach (WcxArchiveItem item in base.Items.Values)
            {
                files.Add(item.Name);
                if (handler != null)
                {
                    handler(new SimpleProcessItemEventArgs(item, base.GetUserState(item)));
                }
            }
            int errorCode = base.Context.FormatInfo.DeleteFiles(base.Context.ArchiveName, files);
            if (errorCode != 0)
            {
                WcxErrors.ThrowExceptionForError(errorCode);
            }
            LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Changed, base.Context.ArchiveName);
        }
    }
}

