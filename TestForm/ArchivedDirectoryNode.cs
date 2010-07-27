using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TestForm
{
    public class ArchivedDirectoryNode : FileSystemNode
    {
        private HeaderData[] list;


        public ArchivedDirectoryNode(FileSystemNode parent, HeaderData info, HeaderData[] list)
            : base(parent)
        {
            this.Name = System.IO.Path.GetFileName(info.FileName);
            this.Path = info.FileName;
            this.Size = info.UnpSize;
            this.list = list;
        }

        public override bool AllowOpen { get { return true; } set { base.AllowOpen = value; } }

        public override FileSystemNode[] ChildNodes { get { return base.ChildNodes ?? (base.ChildNodes = GetChildNodes()); } set { base.ChildNodes = value; } }


        private FileSystemNode[] GetChildNodes()
        {
            List<FileSystemNode> result = new List<FileSystemNode>(10);

            if (this.ParentNode != null)
            {
                result.Add(new ArchiveUpLink(this));
            }

            foreach (HeaderData item in list)
            {
                if (item.FileName.StartsWith(this.Path + System.IO.Path.DirectorySeparatorChar))
                {
                    string subPath = item.FileName.Substring(this.Path.Length + 1);

                    if ((item.FileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        if (!subPath.Contains(System.IO.Path.DirectorySeparatorChar))
                        {
                            result.Add(new ArchivedDirectoryNode(this, item, list));
                        }
                    }
                    else
                    {
                        if (!subPath.Contains(System.IO.Path.DirectorySeparatorChar))
                        {
                            result.Add(new ArchivedFileNode(this, item));
                        }
                    }
                }
            }

            return result.ToArray();
        }

        public override int GetImageIndex()
        {
            return SafeNativeMethods.GetLargeAssociatedIconIndex("", FileAttributes.Directory);
        }
    }
}
