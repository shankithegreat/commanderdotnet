using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class Zip7ArchiveNode : FileNode, IDisposable
    {
        private int handle;


        public Zip7ArchiveNode(FileSystemNode parent, FileInfo file)
            : base(parent, file)
        {
            OpenArchiveData v = new OpenArchiveData();

            v.ArcName = file.FullName;
            handle = Zip7ArchiveHelper.OpenArchive(ref v);
        }


        public override bool AllowOpen { get { return true; } set { base.AllowOpen = value; } }

        public override FileSystemNode[] ChildNodes { get { return base.ChildNodes ?? (base.ChildNodes = GetChildNodes()); } set { base.ChildNodes = value; } }


        public void Dispose()
        {
            Zip7ArchiveHelper.CloseArchive(handle);
        }


        private FileSystemNode[] GetChildNodes()
        {
            List<FileSystemNode> result = new List<FileSystemNode>(10);

            if (this.ParentNode != null)
            {
                result.Add(new ArchiveUpLink(this));
            }

            List<HeaderData> items = new List<HeaderData>(40);

            HeaderData data = new HeaderData { ArcName = new string((char)0, 260), FileName = new string((char)0, 260) };
            while (Zip7ArchiveHelper.ReadHeader(handle, ref data) == 0)
            {
                Zip7ArchiveHelper.ProcessFile(handle, OperationMode.Skip, null, null);

                items.Add(data);
            }

            HeaderData[] list = items.ToArray();
            foreach (HeaderData item in list)
            {
                if ((item.FileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    if (!item.FileName.Contains(System.IO.Path.DirectorySeparatorChar))
                    {
                        result.Add(new ArchivedDirectoryNode(this, item, list));
                    }
                }
                else
                {
                    if (!item.FileName.Contains(System.IO.Path.DirectorySeparatorChar))
                    {
                        result.Add(new ArchivedFileNode(this, item));
                    }
                }
            }


            return result.ToArray();
        }        
    }
}
