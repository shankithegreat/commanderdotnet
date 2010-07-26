using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Commander.Utility;

namespace TestForm
{
    public class DirectoryNode : FileSystemNode
    {
        private DirectoryInfo info;


        public DirectoryNode(FileSystemNode parent, DirectoryInfo info)
            : base(parent)
        {
            this.info = info;
            this.Attributes = info.Attributes;
        }


        public override long Size { get { return IoHelper.GetDirectorySize(info); } set { } }

        public override string Name { get { return info.Name; } set { base.Name = value; } }

        public override string Path { get { return info.FullName; } set { base.Path = value; } }

        public override FileSystemNode[] ChildNodes { get { return base.ChildNodes ?? (base.ChildNodes = GetChildNodes()); } set { base.ChildNodes = value; } }

        public override bool AllowOpen { get { return true; } set { base.AllowOpen = value; } }


        public override int GetImageIndex()
        {
            return SafeNativeMethods.GetLargeAssociatedIconIndex(this.Path);
        }

        public override string GetDateString()
        {
            return info.CreationTime.ToString("dd.MM.yyyy hh:mm");
        }


        private FileSystemNode[] GetChildNodes()
        {
            List<FileSystemNode> result = new List<FileSystemNode>(10);

            if (this.ParentNode != null)
            {
                result.Add(new UpLink(this));
            }

            FileSystemInfo[] list = info.GetFileSystemInfos();
            foreach (FileSystemInfo item in list)
            {
                if (item is DirectoryInfo)
                {
                    result.Add(new DirectoryNode(this, (DirectoryInfo)item));
                }
                else if (item is FileInfo)
                {
                    result.Add(new FileNode(this, (FileInfo)item));
                }
            }

            return result.ToArray();
        }
    }
}
