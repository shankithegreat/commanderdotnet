using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Shell;

namespace TestForm
{
    public class ArchiveUpLink : UpLink
    {
        public ArchiveUpLink(FileSystemNode parent)
            : base(parent)
        {
        }

        public override int GetImageIndex()
        {
            return ShellHelper.GetLargeAssociatedIconIndex("*", FileAttributes.Directory | FileAttributes.Normal);
        }
    }
}
