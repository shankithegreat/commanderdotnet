using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class ShellFileNode : FileSystemNode, IExecutive
    {
        private ShellDll.ShellFile info;


        public ShellFileNode(FileSystemNode parent, ShellDll.ShellFile info)
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
            return info.GetImageIndex();
        }
    }
}
