using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Commander.Utility;

namespace TestForm
{
    public class DirectoryNode : FileSystemNode
    {
        private DirectoryInfo info;


        public DirectoryNode(FileSystemNode parent, DirectoryInfo info)
            : base(parent)
        {
            this.info = info;
            this.Attributes = info.Attributes;
        }


        public override long Size { get { return IoHelper.GetDirectorySize(info); } set { } }

        public override string Name { get { return info.Name; } set { base.Name = value; } }

        public override string Path { get { return info.FullName; } set { base.Path = value; } }

        public override FileSystemNode[] ChildNodes { get { return base.ChildNodes ?? (base.ChildNodes = GetChildNodes()); } set { base.ChildNodes = value; } }

        public override bool AllowOpen { get { return true; } set { base.AllowOpen = value; } }


        public override int GetImageIndex()
        {
            return SafeNativeMethods.GetLargeAssociatedIconIndex(this.Path, this.Attributes);
        }

        public override string GetDateString()
        {
            return info.CreationTime.ToString("dd.MM.yyyy hh:mm");
        }


        private FileSystemNode[] GetChildNodes()
        {
            List<FileSystemNode> result = new List<FileSystemNode>(10);

            if (this.ParentNode != null)
            {
                result.Add(new UpLink(this));
            }

            FileSystemInfo[] list = info.GetFileSystemInfos();
            foreach (FileSystemInfo item in list)
            {
                if (item is DirectoryInfo)
                {
                    result.Add(new DirectoryNode(this, (DirectoryInfo)item));
                }
                else if (item is FileInfo)
                {
                    FileInfo fi = (FileInfo) item;
                    switch (fi.Extension)
                    {
                        case ".gz":
                        case ".zip":
                            {
                                result.Add(new ZipArchiveNode(this, fi));
                                break;
                            }                        
                        case ".7z":
                            {
                                result.Add(new Zip7ArchiveNode(this, fi));
                                break;
                            }                        
                        case ".bz2":
                            {
                                result.Add(new BZip2ArchiveNode(this, fi));
                                break;
                            }

                        case ".iso":
                            {
                                result.Add(new IsoArchiveNode(this, fi));
                                break;
                            }                        
                        case ".rar":
                            {
                                result.Add(new RarArchiveNode(this, fi));
                                break;
                            }
                        default:
                            {
                                result.Add(new FileNode(this, fi));
                                break;
                            }
                    }                    
                }
            }

            return result.ToArray();
        }
    }
}
