namespace Nomad.Commons.Net
{
    using Nomad.Commons.IO;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class Ftp
    {
        public ICredentials Credentials;
        public Encoding ListEncoding = Encoding.Default;
        public bool UsePassive = true;

        public string DecodeString(string value)
        {
            return this.RecodeString(value, Encoding.Default, this.ListEncoding);
        }

        public void DeleteFile(Uri fileUri)
        {
            this.GetStatusDescription(fileUri, "DELE", null);
        }

        public string EncodeString(string value)
        {
            return this.RecodeString(value, this.ListEncoding, Encoding.Default);
        }

        public DateTime GetDateTimestamp(Uri uri)
        {
            return DateTime.ParseExact(this.GetStatusDescription(NormalizeFolderUri(uri), "MDTM", null), "yyyyMMddHHmmss", CultureInfo.InvariantCulture.DateTimeFormat);
        }

        public long GetFileSize(Uri fileUri)
        {
            return Convert.ToInt64(this.GetStatusDescription(fileUri, "SIZE", null));
        }

        private FtpWebResponse GetFtpResponse(Uri uri, string method, string newName, string connectionGroup)
        {
            FtpWebResponse response;
            bool flag;
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            if (uri.Scheme != Uri.UriSchemeFtp)
            {
                throw new ArgumentException("Ftp uri scheme expected", "uri");
            }
            ICredentials credentials = this.Credentials;
        Label_01A8:
            flag = true;
            try
            {
                FtpWebRequest request = (FtpWebRequest) WebRequest.Create(uri);
                request.Method = method;
                request.ConnectionGroupName = connectionGroup;
                request.UsePassive = this.UsePassive;
                request.UseBinary = false;
                if (newName != null)
                {
                    request.RenameTo = newName;
                }
                if (credentials != null)
                {
                    request.Credentials = credentials;
                }
                response = (FtpWebResponse) request.GetResponse();
                this.Credentials = credentials;
                return response;
            }
            catch (WebException exception)
            {
                if (exception.Status != WebExceptionStatus.ProtocolError)
                {
                    goto Label_01A6;
                }
                response = (FtpWebResponse) exception.Response;
                FtpStatusCode statusCode = response.StatusCode;
                if (statusCode != FtpStatusCode.NotLoggedIn)
                {
                    if (statusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        goto Label_0131;
                    }
                    goto Label_013A;
                }
                CredentialsNeededEventArgs e = new CredentialsNeededEventArgs(uri);
                this.OnCredentialsNeeded(e);
                if (e.Credentials == null)
                {
                    goto Label_013A;
                }
                credentials = e.Credentials;
                goto Label_01A8;
            Label_0131:
                this.Credentials = credentials;
            Label_013A:;
                string str = response.StatusDescription.TrimEnd(new char[] { '\r', '\n' });
                int index = str.IndexOf(' ');
                if (index >= 0)
                {
                    str = str.Substring(index + 1);
                }
                exception = new WebException(string.Format("{0} ({1})", exception.Message, str), exception, exception.Status, exception.Response);
            Label_01A6:
                throw exception;
            }
            goto Label_01A8;
        }

        private string GetStatusDescription(Uri uri, string method, string connectionGroup)
        {
            string str;
            using (FtpWebResponse response = this.GetFtpResponse(uri, method, null, connectionGroup))
            {
                using (TextReader reader = new StringReader(response.StatusDescription))
                {
                    str = reader.ReadLine().Substring(4);
                }
            }
            return str;
        }

        public IEnumerable<string> ListDirectory(Uri uri)
        {
            return this.ListDirectory(uri, "NLST", null);
        }

        private IEnumerable<string> ListDirectory(Uri uri, string method, string connectionGroup)
        {
            return new <ListDirectory>d__0(-2) { <>4__this = this, <>3__uri = uri, <>3__method = method, <>3__connectionGroup = connectionGroup };
        }

        public IEnumerable<string> ListDirectoryDetails(Uri uri)
        {
            return this.ListDirectory(uri, "LIST", null);
        }

        public void MakeDirectory(Uri uri)
        {
            this.GetStatusDescription(uri, "MKD", null);
        }

        private static Uri NormalizeFolderUri(Uri uri)
        {
            string absolutePath = uri.AbsolutePath;
            if (PathHelper.HasTrailingDirectorySeparator(absolutePath))
            {
                return new Uri(uri, PathHelper.ExcludeTrailingDirectorySeparator(absolutePath));
            }
            return uri;
        }

        protected virtual void OnCredentialsNeeded(CredentialsNeededEventArgs e)
        {
        }

        public string PrintWorkingDirectory(Uri folderUri)
        {
            string str = this.GetStatusDescription(folderUri, "PWD", null);
            if ((str.Length > 0) && (str[0] == '"'))
            {
                return str.Substring(1, str.LastIndexOf('"') - 1);
            }
            return str;
        }

        private string RecodeString(string value, Encoding from, Encoding to)
        {
            if ((from == to) || (this.ListEncoding == Encoding.UTF8))
            {
                return value;
            }
            return to.GetString(from.GetBytes(value));
        }

        public void RemoveDirectory(Uri folderUri)
        {
            this.GetStatusDescription(folderUri, "RMD", null);
        }

        public void Rename(Uri uri, string newName)
        {
            this.Rename(uri, newName, null);
        }

        private void Rename(Uri uri, string newName, string connectionGroup)
        {
            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentException("newName is null or empty");
            }
            this.GetFtpResponse(uri, "RENAME", this.EncodeString(newName), connectionGroup).Close();
        }

        [CompilerGenerated]
        private sealed class <ListDirectory>d__0 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private string <>2__current;
            public string <>3__connectionGroup;
            public string <>3__method;
            public Uri <>3__uri;
            public Nomad.Commons.Net.Ftp <>4__this;
            private int <>l__initialThreadId;
            public string <Line>5__3;
            public TextReader <Reader>5__2;
            public FtpWebResponse <Response>5__1;
            public string connectionGroup;
            public string method;
            public Uri uri;

            [DebuggerHidden]
            public <ListDirectory>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally4()
            {
                this.<>1__state = -1;
                if (this.<Reader>5__2 != null)
                {
                    this.<Reader>5__2.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<Response>5__1 = this.<>4__this.GetFtpResponse(this.uri, this.method, null, this.connectionGroup);
                            if (this.<Response>5__1 != null)
                            {
                                this.<Reader>5__2 = new StreamReader(new WebRequestStreamWrapper(this.<Response>5__1), this.<>4__this.ListEncoding);
                                this.<>1__state = 1;
                                while ((this.<Line>5__3 = this.<Reader>5__2.ReadLine()) != null)
                                {
                                    this.<>2__current = this.<Line>5__3;
                                    this.<>1__state = 2;
                                    return true;
                                Label_00A2:
                                    this.<>1__state = 1;
                                }
                                this.<>m__Finally4();
                            }
                            break;

                        case 2:
                            goto Label_00A2;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                Nomad.Commons.Net.Ftp.<ListDirectory>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new Nomad.Commons.Net.Ftp.<ListDirectory>d__0(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__.uri = this.<>3__uri;
                d__.method = this.<>3__method;
                d__.connectionGroup = this.<>3__connectionGroup;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.String>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally4();
                        }
                        break;
                }
            }

            string IEnumerator<string>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

