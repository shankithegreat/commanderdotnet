namespace Nomad.FileSystem.Ftp
{
    using Nomad.Commons.Net;
    using System;
    using System.IO;
    using System.Net;

    internal class WebRequestCreateFileStreamWrapper : WebRequestStreamWrapper
    {
        private Uri FFileUri;

        public WebRequestCreateFileStreamWrapper(WebRequest request, Uri fileUri) : base(request)
        {
            this.FFileUri = fileUri;
        }

        public override void Close()
        {
            base.Close();
            FtpContext.RaiseFtpChangedEvent(WatcherChangeTypes.Created, this.FFileUri, null);
        }
    }
}

