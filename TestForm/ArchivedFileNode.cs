using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Shell;

namespace TestForm
{
    public class ArchivedFileNode : FileSystemNode
    {
        public ArchivedFileNode(FileSystemNode parent, HeaderData info)
            : base(parent)
        {
            this.Name = System.IO.Path.GetFileName(info.FileName);
            this.Path = System.IO.Path.Combine(info.ArcName, info.FileName);
            this.InternalPath = info.FileName;
            this.Size = info.UnpSize;
            this.Attributes = info.FileAttr;            
        }


        public override bool AllowOpen { get { return false; } set { base.AllowOpen = value; } }

        public string InternalPath { get; private set; }

        public virtual bool IsVirtual { get { return true; } }


        public override int GetImageIndex()
        {
            string ext = System.IO.Path.GetExtension(this.Name);
            return ShellHelper.GetLargeAssociatedIconIndex(ext, FileAttributes.Normal);
        }
    }
}
