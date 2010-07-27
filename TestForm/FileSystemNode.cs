using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestForm
{
    public abstract class FileSystemNode
    {
        protected FileSystemNode(FileSystemNode parent)
        {
            this.ParentNode = parent;
        }


        public virtual string Name { get; set; }

        public virtual long Size { get; set; }

        public virtual string Path { get; set; }

        public virtual FileAttributes Attributes { get; set; }

        public virtual FileSystemNode ParentNode { get; set; }

        public virtual FileSystemNode[] ChildNodes { get; set; }

        public virtual bool AllowOpen { get; set; }

        public virtual bool IsVirtual { get { return false; } }


        public string Extension { get; set; }


        public virtual int GetImageIndex()
        {
            return 0;
        }

        public virtual string GetSizeString()
        {
            return string.Empty;
        }


        public virtual string GetDateString()
        {
            return string.Empty;
        }
    }
}
