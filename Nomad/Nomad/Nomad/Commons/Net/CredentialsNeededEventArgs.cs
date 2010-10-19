namespace Nomad.Commons.Net
{
    using System;
    using System.Net;

    public class CredentialsNeededEventArgs : EventArgs
    {
        public ICredentials Credentials;
        public readonly System.Uri Uri;

        public CredentialsNeededEventArgs(System.Uri uri)
        {
            this.Uri = uri;
        }
    }
}

