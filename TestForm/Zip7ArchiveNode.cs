using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;

namespace TestForm
{
    public class Zip7ArchiveNode : ArchiveNode
    {
        public Zip7ArchiveNode(FileSystemNode parent, FileInfo file)
            : base(parent, file)
        {
        }


        public override void Dispose()
        {
            Zip7ArchiveHelper.CloseArchive(this.Handle);
        }


        protected override int GetHandle()
        {
            OpenArchiveData archiveData = new OpenArchiveData { ArcName = this.Path };
            return Zip7ArchiveHelper.OpenArchive(ref archiveData);
        }

        protected override HeaderData[] GetList()
        {
            List<HeaderData> items = new List<HeaderData>(40);

            HeaderData data = new HeaderData { ArcName = new string((char)0, 260), FileName = new string((char)0, 260) };
            while (Zip7ArchiveHelper.ReadHeader(this.Handle, ref data) == 0)
            {
                Zip7ArchiveHelper.ProcessFile(this.Handle, OperationMode.Skip, null, null);

                items.Add(data);
            }

            return items.ToArray();
        }
    }

    public class ZipArchiveNode : ArchiveNode
    {
        public ZipArchiveNode(FileSystemNode parent, FileInfo file)
            : base(parent, file)
        {
        }


        public override void Dispose()
        {
        }


        protected override int GetHandle()
        {
            return 0;
        }

        private string[] GetSubDirectories(string directory)
        {
            List<string> result = new List<string>();

            string[] list = directory.Split('/', '\\');
            string path = string.Empty;
            foreach (var item in list)
            {
                path += "/" + item;
                result.Add(path);
            }

            return result.ToArray();
        }

        protected override HeaderData[] GetList()
        {
            List<HeaderData> items = new List<HeaderData>(40);

            Dictionary<string, int> directories = new Dictionary<string, int>(10);
            
            using (ZipInputStream stream = new ZipInputStream(this.Path))
            {
                while (true)
                {
                    var item = stream.GetNextEntry();
                    if (item == null)
                    {
                        break;
                    }

                    HeaderData data = new HeaderData { ArcName = this.Path, FileName = item.FileName };
                    if (!item.IsDirectory)
                    {
                        string directory = System.IO.Path.GetDirectoryName(item.FileName);
                        if (!string.IsNullOrEmpty(directory) && !directories.ContainsKey(directory))
                        {
                            foreach (var sub in GetSubDirectories(directory))
                            {
                                if (!directories.ContainsKey(sub))
                                {
                                    directories.Add(sub, 0);
                                    items.Add(new HeaderData {ArcName = this.Path, FileName = directory, FileAttr = FileAttributes.Directory});
                                }
                            }                            
                        }
                    }
                    else
                    {
                        if (!directories.ContainsKey(item.FileName))
                        {
                            directories.Add(item.FileName, 0);
                        }
                    }
                    data.FileAttr = item.Attributes;
                    data.UnpSize = (int)item.UncompressedSize;
                    data.PackSize = (int)item.CompressedSize;

                    items.Add(data);
                }
            }


            /*using(ZipFile zip = new ZipFile(this.Path))
            {
                foreach(var item in zip.Entries)
                {
                    HeaderData data = new HeaderData { ArcName = this.Path, FileName = item.FileName };
                    if (item.IsDirectory)
                    {
                        data.FileAttr = FileAttributes.Directory;                        
                    }
                    data.UnpSize = (int)item.UncompressedSize;
                    data.PackSize = (int)item.CompressedSize;

                    items.Add(data);
                }
            }*/

            return items.ToArray();
        }
    }
}
