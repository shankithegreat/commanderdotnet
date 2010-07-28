using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
            return SafeNativeMethods.GetLargeAssociatedIconIndex("*", FileAttributes.Directory | FileAttributes.Normal);
        }
    }
}
