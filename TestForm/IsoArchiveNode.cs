using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class IsoArchiveNode : FileNode, IDisposable
    {
        private int handle;


        public IsoArchiveNode(FileSystemNode parent, FileInfo file)
            : base(parent, file)
        {
            OpenArchiveData v = new OpenArchiveData();

            v.ArcName = file.FullName;
            handle = IsoArchiveHelper.OpenArchive(ref v);
        }


        public override bool AllowOpen { get { return true; } set { base.AllowOpen = value; } }

        public override FileSystemNode[] ChildNodes { get { return base.ChildNodes ?? (base.ChildNodes = GetChildNodes()); } set { base.ChildNodes = value; } }


        public void Dispose()
        {
            IsoArchiveHelper.CloseArchive(handle);
        }


        private FileSystemNode[] GetChildNodes()
        {
            List<FileSystemNode> result = new List<FileSystemNode>(10);

            if (this.ParentNode != null)
            {
                result.Add(new ArchiveUpLink(this));
            }

            HeaderData data = new HeaderData { ArcName = new string((char)0, 260), FileName = new string((char)0, 260) };
            while (IsoArchiveHelper.ReadHeader(handle, ref data) == 0)
            {
                IsoArchiveHelper.ProcessFile(handle, OperationMode.Skip, null, null);

                if ((data.FileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    if (!data.FileName.Contains(System.IO.Path.DirectorySeparatorChar))
                    {
                        result.Add(new ArchivedDirectoryNode(this, data));
                    }
                }
                else
                {
                    if (!data.FileName.Contains(System.IO.Path.DirectorySeparatorChar))
                    {
                        result.Add(new ArchivedFileNode(this, data));
                    }
                }
            }


            return result.ToArray();
        }
    }
}
