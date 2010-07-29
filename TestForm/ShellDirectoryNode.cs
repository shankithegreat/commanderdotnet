using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class ShellDirectoryNode : FileSystemNode
    {
        private Shell.ShellFolder info;


        public ShellDirectoryNode(FileSystemNode parent, Shell.ShellFolder info)
            : base(parent)
        {
            this.info = info;
        }


        public override long Size { get { return 0; } set { } }

        public override string Name { get { return info.Name; } set { base.Name = value; } }

        public override string Path { get { return info.Path; } set { base.Path = value; } }

        public override FileSystemNode[] ChildNodes { get { return base.ChildNodes ?? (base.ChildNodes = GetChildNodes()); } set { base.ChildNodes = value; } }

        public override bool AllowOpen { get { return true; } set { base.AllowOpen = value; } }


        public override int GetImageIndex()
        {
            return info.GetImageIndex();
        }

        public override string GetDateString()
        {
            return string.Empty;
        }


        private FileSystemNode[] GetChildNodes()
        {
            List<FileSystemNode> result = new List<FileSystemNode>(10);

            if (this.ParentNode != null)
            {
                result.Add(new UpLink(this));
            }

            foreach (Shell.ShellItem item in info.Childs)
            {
                if (item.IsFolder)
                {
                    result.Add(new ShellDirectoryNode(this, (Shell.ShellFolder)item));
                }
                else
                {
                    Shell.ShellFile sf = (Shell.ShellFile) item;

                    if (!string.IsNullOrEmpty(sf.Path))
                    {
                        switch (System.IO.Path.GetExtension(sf.Path))
                        {
                            case ".zip":
                                {
                                    result.Add(new ZipArchiveNode(this, new FileInfo(sf.Path)));
                                    break;
                                }
                            case ".7z":
                                {
                                    result.Add(new Zip7ArchiveNode(this, new FileInfo(sf.Path)));
                                    break;
                                }
                            case ".bz2":
                                {
                                    result.Add(new BZip2ArchiveNode(this, new FileInfo(sf.Path)));
                                    break;
                                }

                            case ".iso":
                                {
                                    result.Add(new IsoArchiveNode(this, new FileInfo(sf.Path)));
                                    break;
                                }
                            case ".rar":
                                {
                                    result.Add(new RarArchiveNode(this, new FileInfo(sf.Path)));
                                    break;
                                }
                            default:
                                {
                                    result.Add(new ShellFileNode(this, (Shell.ShellFile)item));
                                    break;
                                }
                        }
                    }
                }
            }

            return result.ToArray();
        }
    }
}
