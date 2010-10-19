namespace Nomad.FileSystem.Archive.Wcx
{
    using Nomad.FileSystem.Archive.Common;
    using System;
    using System.Collections.Generic;

    internal abstract class WcxProcessor : ISequenceProcessor
    {
        protected readonly WcxArchiveContext Context;
        private ProcessorState FState;
        protected Dictionary<int, WcxArchiveItem> Items;
        private Dictionary<ISequenceableItem, object> ItemUserStates;

        internal WcxProcessor(WcxArchiveContext context)
        {
            this.Context = context;
        }

        public void Add(ISequenceableItem item, object userState)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (this.State != ProcessorState.Initializing)
            {
                throw new InvalidOperationException("You can add items to processor only in Initializing state");
            }
            WcxArchiveItem item2 = item as WcxArchiveItem;
            if ((item2 == null) || (item.SequenceContext != this.Context))
            {
                throw new ArgumentException("item is not WcxArchiveItem or invalid sequence context.");
            }
            if (this.Items == null)
            {
                this.Items = new Dictionary<int, WcxArchiveItem>();
            }
            this.Items.Add(item2.Index, item2);
            if (userState != null)
            {
                if (this.ItemUserStates == null)
                {
                    this.ItemUserStates = new Dictionary<ISequenceableItem, object>();
                }
                this.ItemUserStates.Add(item, userState);
            }
        }

        protected abstract void DoProcess(ProcessItemHandler handler);
        public object GetUserState(ISequenceableItem item)
        {
            object obj2;
            if ((this.ItemUserStates != null) && this.ItemUserStates.TryGetValue(item, out obj2))
            {
                return obj2;
            }
            return null;
        }

        public void Process(ProcessItemHandler handler)
        {
            switch (this.State)
            {
                case ProcessorState.InProcess:
                    throw new InvalidOperationException("Processor already in process");

                case ProcessorState.Finished:
                    throw new InvalidOperationException("Cannot start one processor twice");
            }
            this.FState = ProcessorState.InProcess;
            try
            {
                this.DoProcess(handler);
            }
            finally
            {
                this.FState = ProcessorState.Finished;
            }
        }

        public ProcessorState State
        {
            get
            {
                return this.FState;
            }
        }
    }
}

