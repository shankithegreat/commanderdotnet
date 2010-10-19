namespace Nomad.FileSystem.Archive.Common
{
    using System;
    using System.ComponentModel;

    public abstract class ProcessItemEventArgs : CancelEventArgs
    {
        protected ProcessItemEventArgs()
        {
        }

        public abstract ISequenceableItem Item { get; }

        public abstract object UserState { get; }
    }
}

