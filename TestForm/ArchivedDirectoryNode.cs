﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class ArchivedDirectoryNode : FileSystemNode
    {
        public ArchivedDirectoryNode(FileSystemNode parent, HeaderData info)
            : base(parent)
        {
            this.Name = info.FileName;
            this.Path = info.FileName;
            this.Size = info.UnpSize;
        }


        public override int GetImageIndex()
        {
            return SafeNativeMethods.GetLargeAssociatedIconIndex("", FileAttributes.Directory);
        }
    }
}
