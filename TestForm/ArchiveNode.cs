using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestForm
{
    public abstract class ArchiveNode : FileNode, IDisposable
    {
        private int? handle;


        public ArchiveNode(FileSystemNode parent, FileInfo file)
            : base(parent, file)
        {
        }


        public override bool AllowOpen { get { return true; } set { base.AllowOpen = value; } }

        public override FileSystemNode[] ChildNodes { get { return base.ChildNodes ?? (base.ChildNodes = GetChildNodes()); } set { base.ChildNodes = value; } }

        protected int Handle { get { return (handle ?? (handle = GetHandle())).Value; } }


        public abstract void Dispose();


        protected abstract HeaderData[] GetList();

        protected abstract int GetHandle();


        private FileSystemNode[] GetChildNodes()
        {
            List<FileSystemNode> result = new List<FileSystemNode>(10);

            if (this.ParentNode != null)
            {
                result.Add(new ArchiveUpLink(this));
            }

            HeaderData[] list = GetList();

            foreach (HeaderData item in list)
            {
                if ((item.FileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    if (item.FileName.IndexOfAny(new[] { System.IO.Path.DirectorySeparatorChar, '/' }) == -1)
                    {
                        result.Add(new ArchivedDirectoryNode(this, item, list));
                    }
                }
                else
                {
                    if (item.FileName.IndexOfAny(new[] { System.IO.Path.DirectorySeparatorChar, '/' }) == -1)
                    {
                        result.Add(new ArchivedFileNode(this, item));
                    }
                }
            }


            return result.ToArray();
        }
    }
}
