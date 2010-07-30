using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shell
{
    public class ShellFile : ShellItem
    {
        public ShellFile(IShellItem item, IntPtr pidl)
            : base(item, pidl)
        {
        }

        public override bool IsFolder { get { return false; } }
    }
}
