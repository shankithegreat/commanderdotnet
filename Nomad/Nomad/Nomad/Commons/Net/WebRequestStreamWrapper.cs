namespace Nomad.Commons.Net
{
    using Nomad.Commons.IO;
    using System;
    using System.Net;

    internal class WebRequestStreamWrapper : StreamWrapper
    {
        private WebRequest Request;
        private WebResponse Response;

        public WebRequestStreamWrapper(WebRequest request) : base(request.GetRequestStream())
        {
            this.Request = request;
        }

        public WebRequestStreamWrapper(WebResponse response) : base(response.GetResponseStream())
        {
            this.Response = response;
        }

        public override void Close()
        {
            base.Close();
            if (this.Response == null)
            {
                this.Response = this.Request.GetResponse();
            }
            this.Response.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Response != null)
                {
                    ((IDisposable) this.Response).Dispose();
                    this.Response = null;
                }
                this.Request = null;
            }
            base.Dispose(disposing);
        }
    }
}

