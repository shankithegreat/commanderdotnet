using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class ArchivedFileNode : FileSystemNode
    {
        public ArchivedFileNode(FileSystemNode parent)
            : base(parent)
        {
        }
    }
}
