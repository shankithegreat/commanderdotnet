
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestForm.Shell
{
    public class ShellFolder : ShellItem
    {
        public ShellFolder(IntPtr pidl)
            : base(pidl, null)
        {
        }
    }
}
