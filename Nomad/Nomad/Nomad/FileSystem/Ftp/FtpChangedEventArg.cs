namespace Nomad.FileSystem.Ftp
{
    using System;
    using System.IO;

    internal class FtpChangedEventArg : EventArgs
    {
        public readonly Uri AbsoluteUri;
        public readonly Uri BaseUri;
        public readonly WatcherChangeTypes ChangeType;
        public readonly string NewName;

        public FtpChangedEventArg(WatcherChangeTypes changeType, Uri absoluteUri, Uri baseUri)
        {
            if (changeType == WatcherChangeTypes.Renamed)
            {
                throw new ArgumentException("Rename change type was incompatible with this constructor");
            }
            this.ChangeType = changeType;
            this.AbsoluteUri = absoluteUri;
            this.BaseUri = baseUri;
        }

        public FtpChangedEventArg(Uri absoluteUri, Uri baseUri, string newName)
        {
            if (newName == null)
            {
                throw new ArgumentNullException("newName");
            }
            this.ChangeType = WatcherChangeTypes.Renamed;
            this.AbsoluteUri = absoluteUri;
            this.BaseUri = baseUri;
            this.NewName = newName;
        }
    }
}

