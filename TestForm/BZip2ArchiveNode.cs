using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class BZip2ArchiveNode : FileNode, IDisposable
    {
        private int handle;


        public BZip2ArchiveNode(FileSystemNode parent, FileInfo file)
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
                result.Add(new UpLink(this));
            }

            HeaderData data = new HeaderData() { ArcName = new string((char)0, 260), FileName = new string((char)0, 260) };
            while (Zip7ArchiveHelper.ReadHeader(handle, ref data) == 0)
            {
                Zip7ArchiveHelper.ProcessFile(handle, OperationMode.Skip, null, null);

                if ((data.FileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    var item = new ArchivedDirectoryNode(this);
                    item.Name = data.FileName;
                    item.Size = data.UnpSize;

                    result.Add(item);
                }
                else
                {
                    var item = new ArchivedFileNode(this);
                    item.Name = data.FileName;
                    item.Size = data.UnpSize;

                    result.Add(item);
                }
            }


            return result.ToArray();
        }        
    }
}
