namespace Microsoft.IO
{
    using Microsoft.Win32.IOCTL;
    using System;
    using System.Text;

    public class ReparsePointInfo
    {
        private const string MountPointPathPrefix = @"\??\Volume";
        public readonly string Name;
        internal const string NonInterpretedPathPrefix = @"\??\";
        public readonly Microsoft.IO.ReparseType ReparseType;
        public readonly string Target;

        internal ReparsePointInfo(REPARSE_DATA_BUFFER buffer)
        {
            if (buffer.ReparseTag == 0xa000000c)
            {
                this.Target = Encoding.Unicode.GetString(buffer.PathBuffer, buffer.SubstituteNameOffset + 4, buffer.SubstituteNameLength);
                this.Name = Encoding.Unicode.GetString(buffer.PathBuffer, buffer.PrintNameOffset + 4, buffer.PrintNameLength);
                this.ReparseType = Microsoft.IO.ReparseType.SymbolicLink;
            }
            else if (buffer.ReparseTag == 0xa0000003)
            {
                this.Target = Encoding.Unicode.GetString(buffer.PathBuffer, buffer.SubstituteNameOffset, buffer.SubstituteNameLength);
                this.Name = Encoding.Unicode.GetString(buffer.PathBuffer, buffer.PrintNameOffset, buffer.PrintNameLength);
                if (this.Target.StartsWith(@"\??\Volume", StringComparison.Ordinal))
                {
                    this.Target = @"\\?\Volume" + this.Target.Substring(@"\??\Volume".Length);
                    this.ReparseType = Microsoft.IO.ReparseType.MountPoint;
                }
                else
                {
                    if (this.Target.StartsWith(@"\??\", StringComparison.Ordinal))
                    {
                        this.Target = this.Target.Substring(@"\??\".Length);
                    }
                    this.ReparseType = Microsoft.IO.ReparseType.JunctionPoint;
                }
            }
            else
            {
                this.ReparseType = Microsoft.IO.ReparseType.Unknown;
            }
        }
    }
}

