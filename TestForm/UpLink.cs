using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class UpLink : FileSystemNode
    {
        public UpLink(FileSystemNode parent)
            : base(parent)
        {
            this.AllowOpen = this.ParentNode.AllowOpen;
            this.Name = "..";
        }


        public override FileSystemNode[] ChildNodes { get { return (this.ParentNode.ParentNode != null ? this.ParentNode.ParentNode.ChildNodes : null); } }

        public override string Path
        {
            get
            {
                return this.ParentNode.ParentNode.Path;
            }
            set
            {
                this.ParentNode.ParentNode.Path = value;
            }
        }
        

        public override int GetImageIndex()
        {
            return this.ParentNode.GetImageIndex();
        }
    }
}
