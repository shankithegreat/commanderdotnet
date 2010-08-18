using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Shell;

namespace TestForm
{
    public class ArchivedDirectoryNode : FileSystemNode
    {
        private HeaderData[] list;


        public ArchivedDirectoryNode(FileSystemNode parent, HeaderData info, HeaderData[] list)
            : base(parent)
        {
            this.Name = System.IO.Path.GetFileName(info.FileName.TrimEnd('\\', '/'));
            this.Path = System.IO.Path.Combine(info.ArcName, info.FileName.TrimEnd('\\', '/'));
            this.InternalPath = info.FileName.TrimEnd('\\', '/');
            this.Size = info.UnpSize;
            this.list = list;
        }


        public override bool AllowOpen { get { return true; } set { base.AllowOpen = value; } }

        public override FileSystemNode[] ChildNodes { get { return base.ChildNodes ?? (base.ChildNodes = GetChildNodes()); } set { base.ChildNodes = value; } }

        public string InternalPath { get; private set; }

        public virtual bool IsVirtual { get { return true; } }


        public override int GetImageIndex()
        {
            return ShellHelper.GetLargeAssociatedIconIndex("*", FileAttributes.Directory | FileAttributes.Normal);
        }


        private FileSystemNode[] GetChildNodes()
        {
            List<FileSystemNode> result = new List<FileSystemNode>(10);

            if (this.ParentNode != null)
            {
                result.Add(new ArchiveUpLink(this));
            }

            foreach (HeaderData item in list)
            {
                if ((item.FileName.StartsWith(this.InternalPath + System.IO.Path.DirectorySeparatorChar) || item.FileName.StartsWith(this.InternalPath + "/")) && item.FileName.Length > this.InternalPath.Length + 1)
                {
                    string subPath = item.FileName.Substring(this.InternalPath.Length + 1);

                    if ((item.FileAttr & FileAttributes.Directory) == FileAttributes.Directory || subPath.EndsWith("\\") || subPath.EndsWith("/"))
                    {
                        subPath = subPath.TrimEnd('\\', '/');

                        if (!subPath.Contains(System.IO.Path.DirectorySeparatorChar) && !subPath.Contains("/"))
                        {
                            result.Add(new ArchivedDirectoryNode(this, item, list));
                        }
                    }
                    else
                    {
                        if (!subPath.Contains(System.IO.Path.DirectorySeparatorChar) && !subPath.Contains("/"))
                        {
                            result.Add(new ArchivedFileNode(this, item));
                        }
                    }
                }
            }

            return result.ToArray();
        }
    }
}
