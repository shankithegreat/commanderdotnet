﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
}
