namespace Nomad.Commons.Controls
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    public class PreviewFileSystemInfoEventArgs : PreviewValueEventArgs<FileSystemInfo>
    {
        internal PreviewFileSystemInfoEventArgs(Control target) : base(target)
        {
        }

        public PreviewFileSystemInfoEventArgs(Control target, FileSystemInfo value) : base(target, value)
        {
        }
    }
}

