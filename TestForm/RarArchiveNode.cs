using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class RarArchiveNode : ArchiveNode
    {
        public RarArchiveNode(FileSystemNode parent, FileInfo file)
            : base(parent, file)
        {
        }


        public override void Dispose()
        {
            RarArchiveHelper.CloseArchive(this.Handle);
        }


        protected override int GetHandle()
        {
            OpenArchiveData archiveData = new OpenArchiveData { ArcName = this.Path };
            return RarArchiveHelper.OpenArchive(ref archiveData);
        }

        protected override HeaderData[] GetList()
        {
            List<HeaderData> items = new List<HeaderData>(40);

            HeaderData data = new HeaderData { ArcName = new string((char)0, 260), FileName = new string((char)0, 260) };
            while (RarArchiveHelper.ReadHeader(this.Handle, ref data) == 0)
            {
                RarArchiveHelper.ProcessFile(this.Handle, OperationMode.Skip, null, null);

                items.Add(data);
            }

            return items.ToArray();
        }
    }
}
