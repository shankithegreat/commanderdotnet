using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestForm
{
    public class Shell2DirectoryNode : FileSystemNode
    {
        private Microsoft.WindowsAPICodePack.Shell.ShellFolder info;


        public Shell2DirectoryNode(FileSystemNode parent, Microsoft.WindowsAPICodePack.Shell.ShellFolder info)
            : base(parent)
        {
            this.info = info;
        }


        public override long Size { get { return 0; } set { } }

        public override string Name { get { return info.Name; } set { base.Name = value; } }

        public override string Path { get { return info.ParsingName; } set { base.Path = value; } }

        public override FileSystemNode[] ChildNodes { get { return base.ChildNodes ?? (base.ChildNodes = GetChildNodes()); } set { base.ChildNodes = value; } }

        public override bool AllowOpen { get { return true; } set { base.AllowOpen = value; } }


        public override int GetImageIndex()
        {
            return 0;
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

            foreach (var item in info)
            {
                if (item is Microsoft.WindowsAPICodePack.Shell.ShellFolder)
                {
                    result.Add(new Shell2DirectoryNode(this, (Microsoft.WindowsAPICodePack.Shell.ShellFolder)item));
                }
                else if (item is Microsoft.WindowsAPICodePack.Shell.ShellFile)
                {
                    var sf = (Microsoft.WindowsAPICodePack.Shell.ShellFile)item;

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
                                    result.Add(new Shell2FileNode(this, sf));
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
