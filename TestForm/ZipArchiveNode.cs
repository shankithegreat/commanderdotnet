using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class ZipArchiveNode : FileNode, IDisposable
    {
        private int handle;


        public ZipArchiveNode(FileSystemNode parent, FileInfo file)
            : base(parent, file)
        {
            tOpenArchiveData v = new tOpenArchiveData();

            v.ArcName = file.FullName;
            handle = ZipArchiveHelper.OpenArchive(ref v);
        }


        public override bool AllowOpen { get { return true; } set { base.AllowOpen = value; } }

        public override FileSystemNode[] ChildNodes { get { return base.ChildNodes ?? (base.ChildNodes = GetChildNodes()); } set { base.ChildNodes = value; } }


        public void Dispose()
        {
            ZipArchiveHelper.CloseArchive(handle);
        }


        private FileSystemNode[] GetChildNodes()
        {
            List<FileSystemNode> result = new List<FileSystemNode>(10);

            if (this.ParentNode != null)
            {
                result.Add(new UpLink(this));
            }

            
            
            while (true)
            {
                tHeaderData data = new tHeaderData();
                int r = ZipArchiveHelper.ReadHeader(handle, ref data);
                ZipArchiveHelper.ProcessFile(handle, OperationMode.Skip, null, null);
                if (r != 0)
                {
                    break;
                }

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
