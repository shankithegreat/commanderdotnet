using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class ArchivedDirectoryNode : FileSystemNode
    {
        public ArchivedDirectoryNode(FileSystemNode parent)
            : base(parent)
        {
        }
    }
}
