using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class Shell2FileNode : FileSystemNode, IExecutive
    {
        private Microsoft.WindowsAPICodePack.Shell.ShellFile info;


        public Shell2FileNode(FileSystemNode parent, Microsoft.WindowsAPICodePack.Shell.ShellFile info)
            : base(parent)
        {
            this.ParentNode = parent;
            this.info = info;
        }


        public override long Size { get { return 0; } set { } }

        public override string Name { get { return info.Name; } set { base.Name = value; } }

        public override string Path { get { return info.Path; } set { base.Path = value; } }

        public override bool AllowOpen { get { return false; } set { base.AllowOpen = value; } }


        public override string GetDateString()
        {
            return string.Empty;
        }

        public void Activate(ShellContextMenu context)
        {
            context.DefaultCommand(info.Path);
        }

        public override int GetImageIndex()
        {
            return 0;
        }
    }
}
