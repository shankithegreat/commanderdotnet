namespace Nomad.FileSystem.Archive.SevenZip
{
    using Nomad.FileSystem.Archive.Common;
    using System;
    using System.IO;

    public class ReadWriteItemEventArgs : ProcessItemEventArgs, IGetStream
    {
        private ReadWriteExtractCallback Processor;

        internal ReadWriteItemEventArgs(ReadWriteExtractCallback processor)
        {
            this.Processor = processor;
        }

        public Stream GetStream()
        {
            this.Processor.CurrentStream = new ReadWriteStream(this.Processor);
            this.Processor.ConfirmExtractItem.Set();
            return this.Processor.CurrentStream;
        }

        public override ISequenceableItem Item
        {
            get
            {
                return this.Processor.CurrentItem;
            }
        }

        public override object UserState
        {
            get
            {
                return this.Processor.GetUserState(this.Processor.CurrentItem);
            }
        }
    }
}

