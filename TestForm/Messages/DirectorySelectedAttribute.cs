using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestForm.Messages
{
    public class DirectorySelectedAttribute : MessageAttribute
    {
        public DirectorySelectedAttribute()
        {
            this.ArgumentType = typeof(DirectorySelectedArgs);
        }
    }

    public class DirectorySelectedArgs : MessageArgs
    {
        public DirectorySelectedArgs(string selectedDirectory)
        {
            this.SelectedDirectory = selectedDirectory;
        }


        public string SelectedDirectory { get; set; }
    }

    public class ShellDirectorySelectedAttribute : MessageAttribute
    {
        public ShellDirectorySelectedAttribute()
        {
            this.ArgumentType = typeof(ShellDirectorySelectedArgs);
        }
    }

    public class ShellDirectorySelectedArgs : MessageArgs
    {
        public ShellDirectorySelectedArgs(Shell.ShellFolder selectedDirectory)
        {
            this.SelectedDirectory = selectedDirectory;
        }


        public Shell.ShellFolder SelectedDirectory { get; set; }
    }


    public class UpDirectorySelectedAttribute : MessageAttribute
    {
        public UpDirectorySelectedAttribute()
        {
            this.ArgumentType = typeof(MessageArgs);
        }
    }

    public class RootDirectorySelectedAttribute : MessageAttribute
    {
        public RootDirectorySelectedAttribute()
        {
            this.ArgumentType = typeof(MessageArgs);
        }
    }
}
