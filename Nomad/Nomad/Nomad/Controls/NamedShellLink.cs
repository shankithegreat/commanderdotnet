namespace Nomad.Controls
{
    using Microsoft.Shell;
    using Nomad.Commons;
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class NamedShellLink : ShellLink, IEquatable<NamedShellLink>
    {
        private string FName;
        private string FOriginalName;
        private const uint IndexSig = 0xefefefef;

        public NamedShellLink()
        {
        }

        public NamedShellLink(string fileName) : base(fileName)
        {
            this.FOriginalName = fileName;
            this.FName = Path.GetFileNameWithoutExtension(fileName);
        }

        public override object Clone()
        {
            NamedShellLink link = (NamedShellLink) base.Clone();
            link.FOriginalName = this.FOriginalName;
            link.FName = this.FName;
            return link;
        }

        public bool Equals(NamedShellLink other)
        {
            return ((((string.Equals(this.Name, other.Name, StringComparison.OrdinalIgnoreCase) && string.Equals(base.Path, other.Path, StringComparison.OrdinalIgnoreCase)) && ((base.Hotkey == other.Hotkey) && string.Equals(base.Arguments, other.Arguments))) && (string.Equals(base.WorkingDirectory, other.WorkingDirectory, StringComparison.OrdinalIgnoreCase) && (base.WindowState == other.WindowState))) && (base.Flags == other.Flags));
        }

        public string GenerateHash()
        {
            using (MD5 md = MD5.Create())
            {
                byte[] bytes = Encoding.Unicode.GetBytes(base.Path);
                md.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
                bytes = Encoding.Unicode.GetBytes(base.Arguments);
                md.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
                bytes = Encoding.Unicode.GetBytes(base.WorkingDirectory);
                md.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
                bytes = new byte[12];
                ByteArrayHelper.WriteInt32(bytes, 0, (int) base.Hotkey);
                ByteArrayHelper.WriteInt32(bytes, 4, (int) base.WindowState);
                ByteArrayHelper.WriteInt32(bytes, 8, (int) base.Flags);
                md.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
                bytes = Encoding.Unicode.GetBytes(this.FName);
                md.TransformFinalBlock(bytes, 0, bytes.Length);
                return ByteArrayHelper.ToString(md.Hash);
            }
        }

        public override string ToString()
        {
            return this.FName;
        }

        public int Index
        {
            get
            {
                byte[] block = base.GetBlock(0xefefefef);
                if (block == null)
                {
                    return 0;
                }
                return BitConverter.ToInt32(block, 0);
            }
            set
            {
                base.RemoveDataBlock(0xefefefef);
                base.AddBlock(0xefefefef, BitConverter.GetBytes(value));
            }
        }

        public string Name
        {
            get
            {
                return this.FName;
            }
            set
            {
                this.FName = value;
            }
        }

        public string OriginalName
        {
            get
            {
                return this.FOriginalName;
            }
        }
    }
}

