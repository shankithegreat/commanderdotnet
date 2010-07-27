using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class IsoArchiveNode : ArchiveNode
    {
        public IsoArchiveNode(FileSystemNode parent, FileInfo file)
            : base(parent, file)
        {
        }


        public override void Dispose()
        {
            IsoArchiveHelper.CloseArchive(this.Handle);
        }


        protected override int GetHandle()
        {
            OpenArchiveData archiveData = new OpenArchiveData {ArcName = this.Path};
            return IsoArchiveHelper.OpenArchive(ref archiveData);
        }

        protected override HeaderData[] GetList()
        {
            List<HeaderData> items = new List<HeaderData>(40);

            HeaderData data = new HeaderData { ArcName = new string((char)0, 260), FileName = new string((char)0, 260) };
            while (IsoArchiveHelper.ReadHeader(this.Handle, ref data) == 0)
            {
                IsoArchiveHelper.ProcessFile(this.Handle, OperationMode.Skip, null, null);

                items.Add(data);
            }

            return items.ToArray();
        }
    }
}
