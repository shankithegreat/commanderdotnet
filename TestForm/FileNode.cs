using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Shell;

namespace TestForm
{
    public class FileNode : FileSystemNode, IExecutive
    {
        private FileInfo info;


        public FileNode(FileSystemNode parent, FileInfo info)
            : base(parent)
        {
            this.info = info;
            this.Attributes = info.Attributes;
        }


        public override long Size { get { return info.Length; } set { } }

        public override string Name { get { return info.Name; } set { base.Name = value; } }

        public override string Path { get { return info.FullName; } set { base.Path = value; } }

        public override bool AllowOpen { get { return false; } set { base.AllowOpen = value; } }


        public override string GetDateString()
        {
            return info.CreationTime.ToString("dd.MM.yyyy hh:mm");
        }

        public void Activate(ShellContextMenu context)
        {
            context.DefaultCommand(info);
        }

        public override int GetImageIndex()
        {
            return ShellHelper.GetLargeAssociatedIconIndex(this.Path, this.Attributes);
        }
    }
}
