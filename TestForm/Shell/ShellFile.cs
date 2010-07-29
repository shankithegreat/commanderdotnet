using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestForm.Shell
{
    public class ShellFile : ShellItem
    {
        public ShellFile(IntPtr pidl)
            : base(pidl, null)
        {
        }
    }
}
