namespace Nomad.FileSystem.Archive.SevenZip
{
    using Nomad.Commons;
    using Nomad.Dialogs;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    internal class ArchiveOpenCallback : IArchiveOpenCallback, IArchiveOpenVolumeCallback, ICryptoGetTextPassword
    {
        private string ArchiveFileName;
        private readonly SevenZipSharedArchiveContext Context;
        private bool IsRarArchive;
        private int NameCounter;
        private string NameFormat;
        private IWin32Window Owner;

        public ArchiveOpenCallback(IWin32Window owner, SevenZipSharedArchiveContext context)
        {
            this.Owner = owner;
            this.Context = context;
            this.ArchiveFileName = context.FileName;
            this.IsRarArchive = context.FormatInfo.KnownFormat == KnownSevenZipFormat.Rar;
        }

        public int CryptoGetTextPassword(out string password)
        {
            SecureString source = this.Context.ArchivePassword[0];
            if (source == null)
            {
                source = GetPassword(this.Owner, this.ArchiveFileName, this.Context.ArchivePassword);
            }
            if (source != null)
            {
                password = SimpleEncrypt.DecryptString(source);
                return 0;
            }
            password = null;
            return -2147467260;
        }

        public static SecureString GetPassword(IWin32Window owner, string archiveName, SecureString[] archivePassword)
        {
            bool flag;
            SecureString str = PasswordDialog.GetPassword(owner, archiveName, out flag);
            if (flag)
            {
                archivePassword[0] = str;
            }
            return str;
        }

        public void GetProperty(ItemPropId propID, IntPtr value)
        {
            if (propID == ItemPropId.kpidName)
            {
                if (this.IsRarArchive)
                {
                    Match match = new Regex(@"^(?<name>.+?)\.((?<a>part(?<num>\d{1,2})\.rar)|(?<b>r(?<num>\d{2}))|rar)$", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase).Match(this.ArchiveFileName);
                    if (match.Success)
                    {
                        if (match.Groups["a"].Success)
                        {
                            this.NameFormat = match.Groups["name"].Value + ".part{0}.rar";
                            this.NameCounter = Convert.ToInt32(match.Groups["num"].Value);
                        }
                        else
                        {
                            this.NameFormat = match.Groups["name"].Value + ".r{0:00}";
                            if (match.Groups["b"].Success)
                            {
                                this.NameCounter = Convert.ToInt32(match.Groups["num"].Value);
                            }
                            else
                            {
                                this.NameCounter = -1;
                            }
                        }
                    }
                }
                Marshal.GetNativeVariantForObject(this.ArchiveFileName, value);
            }
        }

        public int GetStream(string name, out IInStream inStream)
        {
            if (!(((name == null) || !name.Equals(".rar", StringComparison.OrdinalIgnoreCase)) || string.IsNullOrEmpty(this.NameFormat)))
            {
                name = string.Format(this.NameFormat, ++this.NameCounter);
            }
            if (File.Exists(name))
            {
                inStream = new InStreamTimedWrapper(this.Context.Proxy.Open(name, FileMode.Open, FileAccess.Read, FileShare.Read));
                return 0;
            }
            inStream = null;
            return 1;
        }

        public void SetCompleted(IntPtr files, IntPtr bytes)
        {
        }

        public void SetTotal(IntPtr files, IntPtr bytes)
        {
        }
    }
}

